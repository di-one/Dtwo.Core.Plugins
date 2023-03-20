using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dtwo.API;
using Dtwo.Plugins;

namespace Dtwo.Core.Plugins
{
    public static class PluginsManager
    {
        private static List<Plugin> m_plugins = new List<Plugin>();
        public static Action<Plugin> OnLoadPluginEvent;
        public static Action<Plugin> BeforeLoadPluginEvent;
        public static Action OnAllPluginsLoaded;

        public static int LoadPlugins(List<byte[]> bytes, string otherPluginsFolder,  Action<int> progressionStep = null, Func<PluginInfos, bool> canLoadCallback = null)
        {
            int loadedPlugins = 0;

            try
            {
                LogManager.Log("Load Plugins ");
                List<Plugin> plugins = PluginManager.LoadPlugins<Plugin>(bytes, "dRgUkXp2s5v8y/B?E(G+KbPeShVmYq3t", otherPluginsFolder);

                if (plugins == null)
                {
                    return -1;
                }

                LogManager.Log($"{plugins.Count} plugins founded");

                for (int i = 0; i < plugins.Count; i++)
                {
                    if (i != 0)
                    {
                        int step = (int)(((float)i / (float)plugins.Count) * 100.0f);
                        progressionStep?.Invoke(step);
                    }

                    var plugin = plugins[i];

                    if (canLoadCallback != null && canLoadCallback.Invoke(plugin.Infos) == false)
                    {
                        LogManager.LogWarning($"Le mod {plugin.Infos.Name} n'est pas compatible pour cette version du jeu");
                        continue;
                    }

                    if (LoadPlugin(plugin))
                    {
                        loadedPlugins++;                      
                    }
                }

                return loadedPlugins;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error on load plugins : " + ex.Message);
                return -1;
            }
        }

        public static void OnCoreLoaded()
        {
            Console.Write("On core loaded");

            // start plugins when all uploaded
            for (int i = 0; i < m_plugins.Count; i++)
            {
                Console.Write("start plugin " + i);
                m_plugins[i].OnStart();
            }

            for (int i = 0; i < m_plugins.Count; i++)
            {
                m_plugins[i].OnAllPluginsLoaded();
            }

            OnAllPluginsLoaded?.Invoke();
        }

        private static bool LoadPlugin(Plugin plugin)
        {
            BeforeLoadPluginEvent?.Invoke(plugin);

            EventPlaylistManager.AddPlugin(plugin);

            if (PluginAssemblyManager.LoadAllAttributes(plugin) == false)
            {
                LogManager.LogWarning($"Impossible de charger le plugin {plugin.Infos}", 1);
                EventPlaylistManager.RemovePlugin(plugin);
                return false;
            }

            m_plugins.Add(plugin);

            // Set register events callback
            plugin.OnRegisterEvent += OnPluginRegisterEvent;
            plugin.OnUnRegisterEvent += OnPluginUnregisterEvent;
     
            Dtwo.API.PluginsHelper.PluginsAssemblies.Add(plugin.Assembly);
            PluginsHelper.Plugins.Add(plugin);

            OnLoadPluginEvent?.Invoke(plugin);

            return true;
        }

        private static void OnPluginRegisterEvent(Plugin plugin, string methodNam, Type messageType)
        {
            EventPlaylistManager.RegisterEvent(plugin, methodNam, messageType.FullName);
        }

        private static void OnPluginUnregisterEvent(Plugin plugin, string methodNam, Type messageType)
        {
            EventPlaylistManager.UnRegisterEvent(plugin, methodNam, messageType.FullName);
        }
    }
}
