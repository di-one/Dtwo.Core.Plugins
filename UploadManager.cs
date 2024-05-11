using System.Reflection;
using Dtwo.Plugins;

namespace Dtwo.Core.Plugins
{
    internal static class UploadManager
    {
    //    internal static async Task<bool> Upload(Stream stream, string fileName)
    //    {
    //        try
    //        {
    //            string filePath = Path.PLUGINS_PATH + "/" + fileName.Split('.')[0];

    //            using (MemoryStream ms = new MemoryStream())
    //            {
    //                await stream.CopyToAsync(ms);

    //                if (ZipManager.Unzip(ms, filePath) == false)
    //                {
    //                    Console.WriteLine("Error on upload theme");
    //                    return false;
    //                }

    //                try
    //                {
    //                    string txt = File.ReadAllText(filePath + "/infos.json");
    //                    PluginInfos infos = JSonSerializer<PluginInfos>.DeSerialize(txt);

    //                    if (infos != null)
    //                    {
    //                        string newPath = Path.PLUGINS_PATH + "/" + infos.Name;
    //                        if (Directory.Exists(newPath))
    //                        {
    //                            Console.WriteLine($"Upload theme : error, theme {infos.Name} already exist");
    //                            return false;
    //                        }

    //                        Directory.Move(filePath, newPath);
    //                        PluginsManager.LoadPlugin(newPath, infos);

    //                        return true;
    //                    }
    //                }

    //                catch (Exception ex)
    //                {
    //                    Console.WriteLine($"UploaTheme: error ({ex.Message})");
    //                    return false;
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Error : " + ex.Message);
    //        }

    //        return false;
    //    }
    }
}
