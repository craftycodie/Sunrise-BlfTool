using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using Sunrise.BlfTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using WarthogInc.BlfChunks;

namespace WarthogInc
{
    class Program
    {
        static void Main(string[] args)
        {

            //ConvertBlfToJson("D:\\Projects\\Local\\Halo 3 Matchmaking\\title storage\\title\\default_hoppers\\", "../../../../json/");
            //ConvertBlfToJson("D:\\Projects\\Local\\Halo 3 Matchmaking\\sunrise pre blf tool\\", "../../../../sunrise/");

            //ConvertJsonToBlf(@"D:\Projects\Local\Halo 3 Matchmaking\BlfWorker\json\", @"D:\Projects\Local\Halo 3 Matchmaking\BlfWorker\blf\");
            ConvertJsonToBlf(@"D:\Projects\GitHub\Sunrise-Content\Title Storage\json\", @"D:\Projects\GitHub\Sunrise-Content\Title Storage\blf\");
        }

        public static void ConvertJsonToBlf(string jsonFolder, string blfFolder)
        {
            Console.WriteLine("Converting JSON files to BLF...");

            var jsonFileEnumerator = Directory.EnumerateFiles(jsonFolder, "*.*", SearchOption.AllDirectories).GetEnumerator();

            var fileHashes = new Dictionary<string, byte[]>();

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
                    File.Copy(jsonFileEnumerator.Current, blfFolder + fileRelativePath, true);
                    Console.WriteLine("Copied file: " + fileRelativePath);

                    continue;
                }

                IBLFChunk blfChunk = null;

                switch(fileName)
                {
                    case "game_set_006.json":
                        blfChunk = JsonConvert.DeserializeObject<GameSet>(File.ReadAllText(jsonFileEnumerator.Current));
                        break;
                    case "rsa_manifest.json":
                        blfChunk = JsonConvert.DeserializeObject<MapManifest>(File.ReadAllText(jsonFileEnumerator.Current));
                        break;
                    case "matchmaking_hopper_011.json":
                        continue; // Handle manually last.
                    case "dynamic_hopper_statistics.json":
                        blfChunk = JsonConvert.DeserializeObject<MatchmakingHopperStatistics>(File.ReadAllText(jsonFileEnumerator.Current));
                        break;
                    case "motd.json":
                    case "black_motd.json":
                    case "blue_motd.json":
                        blfChunk = JsonConvert.DeserializeObject<MessageOfTheDay>(File.ReadAllText(jsonFileEnumerator.Current));
                        break;
                    case "motd_popup.json":
                    case "black_motd_popup.json":
                    case "blue_motd_popup.json":
                        blfChunk = JsonConvert.DeserializeObject<MessageOfTheDayPopup>(File.ReadAllText(jsonFileEnumerator.Current));
                        break;
                    case "matchmaking_banhammer_messages.json":
                        blfChunk = JsonConvert.DeserializeObject<MatchmakingBanhammerMessages>(File.ReadAllText(jsonFileEnumerator.Current));
                        break;
                    case "matchmaking_hopper_descriptions_003.json":
                        blfChunk = JsonConvert.DeserializeObject<MatchmakingHopperDescriptions>(File.ReadAllText(jsonFileEnumerator.Current));
                        break;
                    case "matchmaking_tips.json":
                        blfChunk = JsonConvert.DeserializeObject<MatchmakingTips>(File.ReadAllText(jsonFileEnumerator.Current));
                        break;
                }

                if (blfChunk != null) {

                    if (blfChunk is GameSet)
                    {
                        foreach (GameSet.GameEntry entry in (blfChunk as GameSet).gameEntries)
                        {
                            entry.gameVariantHash = ComputeHash(jsonFolder + fileDirectoryRelativePath + "\\" + entry.gameVariantFileName + "_010.bin");
                            entry.mapVariantHash = ComputeHash(jsonFolder + fileDirectoryRelativePath + "\\map_variants\\" + entry.mapVariantFileName + "_012.bin");
                        }
                    }

                    BlfFile blfFile = new BlfFile();
                    blfFile.AddChunk(blfChunk);
                    blfFile.WriteFile(blfFolder + fileRelativePath.Replace(".json", ".bin"));

                    Console.WriteLine("Converted file: " + fileRelativePath);

                    if (blfChunk is MatchmakingHopperDescriptions 
                        || blfChunk is MatchmakingTips
                        || blfChunk is MapManifest
                        || blfChunk is MatchmakingBanhammerMessages)
                    {
                        fileHashes.Add("/title/default_hoppers/" + fileRelativePath.Replace("\\", "/").Replace(".json", ".bin"), blfFile.ComputeHash());
                    }
                } else
                {
                    Console.WriteLine("Unrecognized JSON file: " + fileRelativePath);
                }
            }

            // And now for the manual ones!
            // First up, matchmaking playlists.
            var mhcf = JsonConvert.DeserializeObject<HopperConfigurationTable>(File.ReadAllText(jsonFolder + "matchmaking_hopper_011.json"));

            //We need to calculate the hash of every gameset.
            foreach (HopperConfigurationTable.HopperConfiguration hopperConfiguration in mhcf.configurations)
            {
                BlfFile gameSetFile = new BlfFile();
                gameSetFile.ReadFile(blfFolder + hopperConfiguration.identifier.ToString("D5") + "\\game_set_006.bin");
                hopperConfiguration.gameSetHash = gameSetFile.ComputeHash();
            }

            BlfFile hoppersFile = new BlfFile();
            hoppersFile.AddChunk(mhcf);
            hoppersFile.WriteFile(blfFolder + "\\matchmaking_hopper_011.bin");

            Console.WriteLine("Converted file: matchmaking_hopper_011.json");

            fileHashes.Add("/title/default_hoppers/matchmaking_hopper_011.bin", hoppersFile.ComputeHash());
            fileHashes.Add("/title/default_hoppers/network_configuration_135.bin", Convert.FromHexString("9D5AF6BC38270765C429F4776A9639D1A0E87319"));
            Manifest.FileEntry[] fileEntries = new Manifest.FileEntry[fileHashes.Count];
            int i = 0;
            foreach (KeyValuePair<string, byte[]> fileEntry in fileHashes)
            {
                fileEntries[i] = new Manifest.FileEntry()
                {
                    filePath = fileEntry.Key,
                    fileHash = fileEntry.Value
                };
                i++;
            }

            var onfm = new Manifest()
            {
                files = fileEntries
            };

            BlfFile manifestFile = new BlfFile();
            manifestFile.AddChunk(onfm);
            manifestFile.WriteFile(blfFolder + "\\manifest_001.bin");

            Console.WriteLine("Created file: manifest_001.bin");
        }

        public static void ConvertBlfToJson(string titleStorageFolder, string jsonFolder)
        {
            Console.WriteLine("Converting BLF files to JSON...");

            var titleDirectoryEnumerator = Directory.EnumerateFiles(titleStorageFolder, "*.*", SearchOption.AllDirectories).GetEnumerator();

            var jsonSettings = new JsonSerializerSettings { Converters = { new ByteArrayConverter(), new HexStringConverter() },  Formatting = Formatting.Indented };

            while (titleDirectoryEnumerator.MoveNext())
            {
                // We remake the manifest on conversion back to BLF.
                if (titleDirectoryEnumerator.Current.EndsWith("manifest_001.bin"))
                    continue;

                string fileRelativePath = titleDirectoryEnumerator.Current.Replace(titleStorageFolder, "");
                if (fileRelativePath.Contains("\\"))
                {
                    string fileDirectoryRelativePath = fileRelativePath.Substring(0, fileRelativePath.LastIndexOf("\\"));
                    Directory.CreateDirectory(jsonFolder + fileDirectoryRelativePath);
                }

                if (titleDirectoryEnumerator.Current.EndsWith(".bin"))
                {
                    try
                    {
                        BlfFile blfFile = new BlfFile();
                        blfFile.ReadFile(titleDirectoryEnumerator.Current);
                        var blfChunk = blfFile.GetChunk(1);
                        string output = JsonConvert.SerializeObject((object)blfChunk, jsonSettings);

                        File.WriteAllText(jsonFolder + fileRelativePath.Replace(".bin", ".json"), output);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to convert file: " + titleDirectoryEnumerator.Current);
                        File.Copy(titleDirectoryEnumerator.Current, jsonFolder + fileRelativePath);
                        Console.WriteLine(ex.ToString());
                    }
                }
                else if (titleDirectoryEnumerator.Current.EndsWith(".jpg"))
                {
                    File.Copy(titleDirectoryEnumerator.Current, jsonFolder + fileRelativePath);
                }
            }
        }

        static byte[] halo3salt = Convert.FromHexString("EDD43009666D5C4A5C3657FAB40E022F535AC6C9EE471F01F1A44756B7714F1C36EC");

        public static byte[] ComputeHash(string path)
        {

            var memoryStream = new MemoryStream();
            var blfFileOut = new BitStream<StreamByteStream>(new StreamByteStream(memoryStream));
            foreach (byte saltByte in halo3salt)
            {
                blfFileOut.Write(saltByte, 8);
            }

            try
            {
                byte[] blfBytes = File.ReadAllBytes(path);
                foreach (byte blfByte in blfBytes)
                {
                    blfFileOut.Write(blfByte, 8);
                }

                byte[] saltedBlf = memoryStream.ToArray();
                return new SHA1Managed().ComputeHash(saltedBlf);
            } catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found: " + path);
                return new byte[20];
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("File Not Found: " + path);
                return new byte[20];
            }
        }
    }
}
