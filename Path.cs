using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtwo.Core.Plugins
{
    internal class Path
    {
        public const string BASE_PATH = @".\";
        public const string TEMP_PATH = BASE_PATH + "temp";
        public const string PLUGINS_PATH = BASE_PATH + "plugins";

        public static string PLUGINS_ABOSLUTE_PATH = AppDomain.CurrentDomain.BaseDirectory + "plugins";
    }
}
