using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dtwo.API;

namespace Dtwo.Core.Plugins
{
    internal static class PluginAssemblyManager
    {
        public static bool LoadAllAttributes(Plugin plugin)
        {
            LogManager.Log($"Load all attributes of plugin {plugin.Infos.Name}");

            var assembly = plugin.Assembly;

            if (assembly == null)
            {
                return false;
            }

            var pluginType = assembly.GetType(plugin.GetType().ToString());

            if (pluginType == null)
            {
                LogManager.LogWarning($"Error: pluginType is null for {plugin.Infos.Name}");
                return false;
            }

            var methods = pluginType.GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                var method = methods[i];
                var attributes = method.GetCustomAttributes(true);


                for (int j = 0; j < attributes.Length; j++)
                {
                    var attribute = attributes[j];

                    DofusEvent? evnt = attribute as DofusEvent;
                    ExportedMethod? exportedMethod = attribute as ExportedMethod;

                    if (evnt != null || exportedMethod != null)
                    {
                        var parameters = method.GetParameters();
                        string methodName = method.Name;

                        if (evnt != null)
                        {
                            if (parameters != null && parameters.Length == 2)
                            {
                                ParameterInfo param1 = parameters[1];
                                Type paramType = param1.ParameterType;

                                if (paramType.FullName == null)
                                {
                                    LogManager.Log($"Error: paramType.FullName is null for {plugin.Infos.Name} {methodName}");
                                    continue;
                                }
                                
                                EventPlaylistManager.RegisterEvent(plugin, methodName, paramType.FullName);
                            }
                        }
                        else if (exportedMethod != null)
                        {
                            if (parameters != null && parameters.Length >= 1)
                            {
                                ParameterInfo param1 = parameters[1];
                                Type paramType = param1.ParameterType;


                                List<Type> parametersTypes = new List<Type>();
                                foreach (var param in parameters)
                                {
                                    parametersTypes.Add(param.ParameterType);
                                }

                                ExportedMethod.RegisterExportedMethod(plugin, methodName, pluginType, parametersTypes);
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
