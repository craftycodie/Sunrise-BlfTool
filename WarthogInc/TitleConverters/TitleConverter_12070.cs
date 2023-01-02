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
    public class TitleConverter_12070 : ITitleConverter
    {
        private static readonly AbstractBlfChunkNameMap chunkNameMap = new BlfChunkNameMap12070();
        public void ConvertBlfToJson(string blfFolder, string jsonFolder)
        {
            throw new NotImplementedException();
        }

        public string GetVersion()
        {
            return "12070";
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

                    if (fileName == "game_set_006.json")
                        continue; // handle after variants

                    if (fileName == "matchmaking_hopper_011.json")
                        continue; // Handle after game sets


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

                jsonFileEnumerator = Directory.EnumerateFiles(jsonFolder, "*.*", SearchOption.AllDirectories).GetEnumerator();

                while (jsonFileEnumerator.MoveNext())
                {
                    string fileName = jsonFileEnumerator.Current;
                    if (fileName.Contains("\\"))
                        fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);

                    string fileRelativePath = jsonFileEnumerator.Current.Replace(jsonFolder, "");
                    string fileDirectoryRelativePath = "";
                    if (fileRelativePath.Contains("\\"))
                    {
                        fileDirectoryRelativePath = fileRelativePath.Substring(0, fileRelativePath.LastIndexOf("\\"));
                        Directory.CreateDirectory(blfFolder + fileDirectoryRelativePath);
                    }

                    if (fileName.EndsWith(".bin") || fileName.EndsWith(".jpg"))
                    {
                        continue;
                    }

                    BlfFile blfFile = BlfFile.FromJSON(File.ReadAllText(jsonFileEnumerator.Current), chunkNameMap);

                    IBLFChunk blfChunk = null;

                    if (fileName == "game_set_006.json")
                        blfChunk = blfFile.GetChunk<GameSet6>();

                    if (blfChunk != null)
                    {

                        foreach (GameSet6.GameEntry entry in (blfChunk as GameSet6).gameEntries)
                        {
                            entry.gameVariantHash = BlfFile.ComputeHash(blfFolder + fileDirectoryRelativePath + "\\" + entry.gameVariantFileName + "_010.bin");
                            entry.mapVariantHash = BlfFile.ComputeHash(blfFolder + fileDirectoryRelativePath + "\\map_variants\\" + entry.mapVariantFileName + "_012.bin");

                            string mapJsonPath = jsonFolder + fileDirectoryRelativePath + "\\map_variants\\" + entry.mapVariantFileName + "_012.json";
                            try
                            {
                                var blfMapFile = BlfFile.FromJSON(File.ReadAllText(mapJsonPath), chunkNameMap);
                                var map = blfMapFile.GetChunk<PackedMapVariant>();
                                entry.mapID = map.mapID;
                            }
                            catch (FileNotFoundException)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("File Not Found: " + mapJsonPath, ConsoleColor.Red);
                                Console.ResetColor();
                            }
                        }

                        blfFile.WriteFile(blfFolder + fileRelativePath.Replace(".json", ".bin"));

                        Console.WriteLine("Converted file: " + fileRelativePath);
                    }
                }

                // And now for the manual ones!
                // First up, matchmaking playlists.
                var hopperConfigurationTableBlfFile = BlfFile.FromJSON(File.ReadAllText(jsonFolder + "default_hoppers\\matchmaking_hopper_011.json"), chunkNameMap);
                var mhcf = hopperConfigurationTableBlfFile.GetChunk<HopperConfigurationTable11>();

                //We need to calculate the hash of every gameset.
                foreach (HopperConfigurationTable11.HopperConfiguration hopperConfiguration in mhcf.configurations)
                {
                    hopperConfiguration.gameSetHash = BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\" + hopperConfiguration.identifier.ToString("D5") + "\\game_set_006.bin");
                }
                BlfFile hoppersFile = new BlfFile();
                hoppersFile.AddChunk(mhcf);
                hoppersFile.WriteFile(blfFolder + "\\default_hoppers\\matchmaking_hopper_011.bin");

                Console.WriteLine("Converted file: default_hoppers\\matchmaking_hopper_011.json");

                fileHashes.Add("/title/default_hoppers/matchmaking_hopper_011.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\matchmaking_hopper_011.bin"));
                fileHashes.Add("/title/default_hoppers/network_configuration_135.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\network_configuration_135.bin"));

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
