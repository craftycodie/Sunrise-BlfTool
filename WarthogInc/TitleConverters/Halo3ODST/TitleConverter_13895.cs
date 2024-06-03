using SunriseBlfTool;
using SunriseBlfTool;
using SunriseBlfTool.TitleConverters;
using SunriseBlfTool.TitleConverters.Halo3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseBlfTool.BlfChunks.ChunkNameMaps;
using SunriseBlfTool.BlfChunks.ChunkNameMaps.Halo3;

namespace SunriseBlfTool.TitleConverters.Halo3ODST
{
    public class TitleConverter_13895 : ITitleConverter
    {
        // This is using the wrong chunk map, but it works for now.
        private static readonly AbstractBlfChunkNameMap chunkNameMap = new BlfChunkNameMap12070();
        public void ConvertBlfToJson(string blfFolder, string jsonFolder)
        {
            Console.WriteLine("Converting BLF files to JSON...");

            var titleDirectoryEnumerator = Directory.EnumerateFiles(blfFolder, "*.*", SearchOption.AllDirectories).GetEnumerator();

            while (titleDirectoryEnumerator.MoveNext())
            {
                // We remake the manifest on conversion back to BLF.
                if (titleDirectoryEnumerator.Current.EndsWith("manifest_001.bin"))
                    continue;

                string fileRelativePath = titleDirectoryEnumerator.Current.Replace(blfFolder, "");
                if (fileRelativePath.Contains("\\"))
                {
                    string fileDirectoryRelativePath = fileRelativePath.Substring(0, fileRelativePath.LastIndexOf("\\"));
                    Directory.CreateDirectory(jsonFolder + fileDirectoryRelativePath);
                }

                if (titleDirectoryEnumerator.Current.EndsWith(".bin")
                    || titleDirectoryEnumerator.Current.EndsWith(".blf")
                    || !titleDirectoryEnumerator.Current.Contains('.')
                )
                {
                    Console.WriteLine("Converting file: " + fileRelativePath);

                    try
                    {
                        BlfFile blfFile = new BlfFile();
                        blfFile.ReadFile(titleDirectoryEnumerator.Current, chunkNameMap);
                        string output = blfFile.ToJSON();

                        File.WriteAllText(jsonFolder + fileRelativePath.Replace(".bin", "").Replace(".blf", "") + ".json", output);
                        Console.WriteLine("Converted file: " + fileRelativePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to convert file: " + titleDirectoryEnumerator.Current);
                        Console.WriteLine(ex.Message);
                        //File.Copy(titleDirectoryEnumerator.Current, jsonFolder + fileRelativePath, true);
                    }
                }
                else if (titleDirectoryEnumerator.Current.EndsWith(".jpg"))
                {
                    if (titleDirectoryEnumerator.Current.Equals(jsonFolder + fileRelativePath))
                        continue;
                    File.Copy(titleDirectoryEnumerator.Current, jsonFolder + fileRelativePath, true);
                }
            }
        }

        public string GetVersion()
        {
            return "13895";
        }

        public void ConvertJsonToBlf(string jsonFolder, string blfFolder)
        {
            jsonFolder += "\\";
            blfFolder += "\\";

            Console.WriteLine("Converting JSON files to BLF...");

            var hoppersFolderEnumerator = Directory.EnumerateDirectories(jsonFolder, "*", SearchOption.TopDirectoryOnly).GetEnumerator();

            while (hoppersFolderEnumerator.MoveNext())
            {
                var hopperFolder = hoppersFolderEnumerator.Current;
                var hopperFolderName = hopperFolder.Substring(hopperFolder.LastIndexOf("\\") + 1);

                Console.WriteLine("Converting " + hopperFolderName);

                var jsonFileEnumerator = Directory.EnumerateFiles(hopperFolder, "*.*", SearchOption.AllDirectories).GetEnumerator();

                var fileHashes = new Dictionary<string, byte[]>();

                while (jsonFileEnumerator.MoveNext())
                {
                    string fileName = jsonFileEnumerator.Current;
                    if (fileName.Contains("\\"))
                        fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);

                    string fileRelativePath = jsonFileEnumerator.Current.Replace(jsonFolder, "");
                    string fileDirectoryRelativePath;
                    if (fileRelativePath.Contains("\\"))
                    {
                        fileDirectoryRelativePath = fileRelativePath.Substring(0, fileRelativePath.LastIndexOf("\\"));
                        Directory.CreateDirectory(blfFolder + fileDirectoryRelativePath);
                    }

                    if (fileName.EndsWith(".bin") || fileName.EndsWith(".jpg"))
                    {
                        File.Copy(jsonFileEnumerator.Current, blfFolder + fileRelativePath, true);
                        Console.WriteLine("Copied file: " + fileRelativePath);

                        continue;
                    }

                    BlfFile blfFile = BlfFile.FromJSON(File.ReadAllText(jsonFileEnumerator.Current), chunkNameMap);

                    blfFile.WriteFile(blfFolder + fileRelativePath.Replace(".json", ".bin"));

                    Console.WriteLine("Converted file: " + fileRelativePath);

                    if (blfFile.HasChunk<MatchmakingHopperDescriptions3>()
                        || blfFile.HasChunk<MatchmakingTips>()
                        || blfFile.HasChunk<MapManifest>()
                        || blfFile.HasChunk<MatchmakingBanhammerMessages>())
                    {
                        fileHashes.Add("/title/default_hoppers/" + fileRelativePath.Replace("\\", "/").Replace(".json", ".bin"), BlfFile.ComputeHash(blfFolder + fileRelativePath.Replace(".json", ".bin")));
                    }
                }

                fileHashes.Add("/title/default_hoppers/network_configuration_128.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\network_configuration_128.bin"));

                Manifest.FileEntry[] fileEntries = new Manifest.FileEntry[fileHashes.Count];
                int i = 0;
                foreach (KeyValuePair<string, byte[]> fileNameHash in fileHashes)
                {
                    fileEntries[i] = new Manifest.FileEntry()
                    {
                        filePath = fileNameHash.Key,
                        fileHash = fileNameHash.Value
                    };
                    i++;
                }

                var onfm = new Manifest()
                {
                    files = fileEntries
                };

                BlfFile manifestFile = new BlfFile();
                manifestFile.AddChunk(onfm);
                manifestFile.WriteFile(blfFolder + "\\default_hoppers\\manifest_001.bin");

                Console.WriteLine("Created file: manifest_001.bin");
            }
        }
    }
}
