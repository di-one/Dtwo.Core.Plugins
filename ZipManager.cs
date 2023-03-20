

using System.IO.Compression;

namespace Dtwo.Core.Plugins
{
    internal class ZipManager
    {
        internal static bool Unzip(Stream stream, string destPath)
        {
            if (Directory.Exists(destPath))
            {
                Console.WriteLine($"UnzipTheme : Directory {destPath} already exist");
                return false;
            }

            Directory.CreateDirectory(destPath);

            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                bool dllFounded = false;
                bool jsonFounded = false;

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    Console.WriteLine("entrie : " + entry.Name);

                    bool b = false;

                    if (entry.Name == "infos.json")
                    {
                        b = true;
                        jsonFounded = true;
                    }

                    else if (entry.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        b = true;
                        dllFounded = true;

                    }

                    if (b)
                    {
                        string destinationPath = destPath + "/" + entry.Name;
                        entry.ExtractToFile(destinationPath);
                    }
                }

                if (dllFounded == false || jsonFounded == false)
                {
                    Directory.Delete(destPath);

                    Console.Write("UnzipTheme : Dll or Json not found, delete directory");
                    return false;
                }
            }

            return true;
        }
    }
}
