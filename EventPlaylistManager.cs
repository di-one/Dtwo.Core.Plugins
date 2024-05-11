using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dtwo.API;
using Dtwo.API.DofusBase.Network.Messages;

namespace Dtwo.Core.Plugins
{
    public class EventPlaylistManager
    {
        // Plugin, Dictionary<MessageType, MethodName>
        private static readonly Dictionary<Plugin, Dictionary<string, string>> m_events = new Dictionary<Plugin, Dictionary<string, string>>();

        public static void AddPlugin(Plugin plugin)
        {
            if (m_events.ContainsKey(plugin) == false)
            {
                m_events.Add(plugin, new Dictionary<string, string>());
            }
        }

        public static void RemovePlugin(Plugin plugin)
        {
            if (m_events.ContainsKey(plugin))
            {
                m_events.Remove(plugin);
            }
        }

        public static void RegisterEvent(Plugin plugin, string methodName, string messageType)
        {
            Dictionary<string, string>? events;
            if (m_events.TryGetValue(plugin, out events))
            {
                if (events.ContainsKey(messageType) == false)
                {
                    events.Add(messageType, methodName);
                    return;
                }
            }
        }

        public static void UnRegisterEvent(Plugin plugin, string methodName, string messageType)
        {
            Dictionary<string, string> events;
            if (m_events.TryGetValue(plugin, out events))
            {
                if (events.ContainsKey(messageType) == false)
                {
                    events.Remove(messageType);
                }
            }
        }

        public static void CallEvent(DofusWindow window, DofusMessage networkMessage)
        {
            for (int i = 0; i < m_events.Count(); i++)
            {
                var evnt = m_events.ElementAt(i);

                if (evnt.Value == null)
                {
                    continue;
                }

                for (int j = 0; j < evnt.Value.Count; j++)
                {

                    var paramTypeAndMethod = evnt.Value.ElementAt(j);

                    if (paramTypeAndMethod.Key == networkMessage.GetType().ToString())
                    {
                        Type pluginType = evnt.Key.GetType();
                        MethodInfo? method = pluginType.GetMethod(paramTypeAndMethod.Value);
                       
                        if (method != null)
                        {
                            method.Invoke(evnt.Key, new dynamic[] { window, networkMessage }); // Cast automatique ??
                        }
                    }
                }
            }
        }
    }
}
