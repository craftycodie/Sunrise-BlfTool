using SunriseBlfTool.BlfChunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarthogInc.BlfChunks;

namespace Sunrise.BlfTool.TitleConverters
{
    public class TitleConverter_reach_12065 : ITitleConverter
    {
        private static readonly AbstractBlfChunkNameMap chunkNameMap = new BlfChunkNameMap_reach_12065();
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
                    || titleDirectoryEnumerator.Current.EndsWith(".mvar")
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

                        File.WriteAllText(jsonFolder + fileRelativePath.Replace(".bin", "").Replace(".mvar", "").Replace(".blf", "") + ".json", output);
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
            return "reach_12065";
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

                    try
                    {
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
                    catch (Exception ex)
                    {
                        Console.WriteLine("FAILED TO CONVERT FILE " + fileRelativePath);
                    }
                }

                fileHashes.Add("/dlc_map_manifest.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\dlc_map_manifest.bin"));
                fileHashes.Add("/matchmaking_hopper_027.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\matchmaking_hopper_027.bin"));
                fileHashes.Add("/network_configuration_241.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\network_configuration_241.bin"));
                fileHashes.Add("/network_configuration_245.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\network_configuration_245.bin"));
                fileHashes.Add("/en/file_megalo_categories.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\en\\file_megalo_categories.bin"));
                fileHashes.Add("/en/file_predefined_queries.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\en\\file_predefined_queries.bin"));
                //fileHashes.Add("/en/matchmaking_banhammer_messages.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\en\\matchmaking_banhammer_messages.bin"));
                //fileHashes.Add("/en/matchmaking_hopper_descriptions_003.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\en\\matchmaking_hopper_descriptions_003.bin"));
                //fileHashes.Add("/en/matchmaking_tips.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\en\\matchmaking_tips.bin"));
                fileHashes.Add("/en/rsa_manifest.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\en\\rsa_manifest.bin"));
                fileHashes.Add("/00102/images/hopper.jpg/", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\00102\\images\\hopper.jpg"));
                //fileHashes.Add("/00104/images/hopper.jpg/", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\00104\\images\\hopper.jpg"));

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

                Console.WriteLine(blfFolder + "\\default_hoppers\\manifest_001.bin");

                Console.WriteLine("Created file: manifest_001.bin");
            }
        }
    }
}
