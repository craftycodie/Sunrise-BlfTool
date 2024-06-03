using SunriseBlfTool.TitleConverters;
using SunriseBlfTool;
using SunriseBlfTool.BlfChunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseBlfTool.BlfChunks.ChunkNameMaps;
using SunriseBlfTool.BlfChunks.ChunkNameMaps.Halo3;

namespace SunriseBlfTool.TitleConverters.Halo3
{
    public class TitleConverter_11637 : ITitleConverter
    {
        private static readonly AbstractBlfChunkNameMap chunkNameMap = new BlfChunkNameMap12070();

        public void ConvertBlfToJson(string blfFolder, string jsonFolder)
        {
            throw new NotImplementedException();
        }

        public void ConvertJsonToBlf(string jsonFolder, string blfFolder)
        {
            jsonFolder += "\\";
            blfFolder += "\\";
            Console.WriteLine("Converting JSON files to BLF...");
            IEnumerator<string> enumerator = Directory.EnumerateDirectories(jsonFolder, "*", SearchOption.TopDirectoryOnly).GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                string text = current.Substring(current.LastIndexOf("\\") + 1);
                Console.WriteLine("Converting " + text);
                IEnumerator<string> enumerator2 = Directory.EnumerateFiles(current, "*.*", SearchOption.AllDirectories).GetEnumerator();
                Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
                while (enumerator2.MoveNext())
                {
                    string text2 = enumerator2.Current;
                    if (text2.Contains("\\"))
                    {
                        text2 = text2.Substring(text2.LastIndexOf("\\") + 1);
                    }
                    string text3 = enumerator2.Current.Replace(jsonFolder, "");
                    if (text3.Contains("\\"))
                    {
                        string text4 = text3.Substring(0, text3.LastIndexOf("\\"));
                        Directory.CreateDirectory(blfFolder + text4);
                    }
                    if (text2.EndsWith(".bin") || text2.EndsWith(".jpg"))
                    {
                        File.Copy(enumerator2.Current, blfFolder + text3, overwrite: true);
                        Console.WriteLine("Copied file: " + text3);
                    }
                    else if (!(text2 == "game_set_006.json") && !(text2 == "matchmaking_hopper_011.json"))
                    {
                        BlfFile blfFile = BlfFile.FromJSON(File.ReadAllText(enumerator2.Current), chunkNameMap);
                        blfFile.WriteFile(blfFolder + text3.Replace(".json", ".bin"));
                        Console.WriteLine("Converted file: " + text3);
                        if (blfFile.HasChunk<MatchmakingHopperDescriptions3>() || blfFile.HasChunk<MatchmakingTips>() || blfFile.HasChunk<MapManifest>() || blfFile.HasChunk<MatchmakingBanhammerMessages>())
                        {
                            dictionary.Add("/title/default_hoppers/" + text3.Replace("\\", "/").Replace(".json", ".bin"), BlfFile.ComputeHash(blfFolder + text3.Replace(".json", ".bin")));
                        }
                    }
                }
                enumerator2 = Directory.EnumerateFiles(jsonFolder, "*.*", SearchOption.AllDirectories).GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    string text5 = enumerator2.Current;
                    if (text5.Contains("\\"))
                    {
                        text5 = text5.Substring(text5.LastIndexOf("\\") + 1);
                    }
                    string text6 = enumerator2.Current.Replace(jsonFolder, "");
                    string text7 = "";
                    if (text6.Contains("\\"))
                    {
                        text7 = text6.Substring(0, text6.LastIndexOf("\\"));
                        Directory.CreateDirectory(blfFolder + text7);
                    }
                    if (text5.EndsWith(".bin") || text5.EndsWith(".jpg"))
                    {
                        continue;
                    }
                    BlfFile blfFile2 = BlfFile.FromJSON(File.ReadAllText(enumerator2.Current), chunkNameMap);
                    IBLFChunk iBLFChunk = null;
                    if (text5 == "game_set_006.json")
                    {
                        iBLFChunk = blfFile2.GetChunk<GameSet6>();
                    }
                    if (iBLFChunk == null)
                    {
                        continue;
                    }
                    GameSet6.GameEntry[] gameEntries = (iBLFChunk as GameSet6).gameEntries;
                    foreach (GameSet6.GameEntry gameEntry in gameEntries)
                    {
                        gameEntry.gameVariantHash = BlfFile.ComputeHash(blfFolder + text7 + "\\" + gameEntry.gameVariantFileName + "_010.bin");
                        gameEntry.mapVariantHash = BlfFile.ComputeHash(blfFolder + text7 + "\\map_variants\\" + gameEntry.mapVariantFileName + "_012.bin");
                        string text8 = jsonFolder + text7 + "\\map_variants\\" + gameEntry.mapVariantFileName + "_012.json";
                        try
                        {
                            PackedMapVariant chunk = BlfFile.FromJSON(File.ReadAllText(text8), chunkNameMap).GetChunk<PackedMapVariant>();
                            gameEntry.mapID = chunk.mapID;
                        }
                        catch (FileNotFoundException)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("File Not Found: " + text8, ConsoleColor.Red);
                            Console.ResetColor();
                        }
                    }
                    blfFile2.WriteFile(blfFolder + text6.Replace(".json", ".bin"));
                    Console.WriteLine("Converted file: " + text6);
                }
                HopperConfigurationTable11 chunk2 = BlfFile.FromJSON(File.ReadAllText(jsonFolder + "default_hoppers\\matchmaking_hopper_011.json"), chunkNameMap).GetChunk<HopperConfigurationTable11>();
                HopperConfigurationTable11.HopperConfiguration[] configurations = chunk2.configurations;
                foreach (HopperConfigurationTable11.HopperConfiguration hopperConfiguration in configurations)
                {
                    hopperConfiguration.gameSetHash = BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\" + hopperConfiguration.identifier.ToString("D5") + "\\game_set_006.bin");
                }
                BlfFile blfFile3 = new BlfFile();
                blfFile3.AddChunk(chunk2);
                blfFile3.WriteFile(blfFolder + "\\default_hoppers\\matchmaking_hopper_011.bin");
                Console.WriteLine("Converted file: default_hoppers\\matchmaking_hopper_011.json");
                dictionary.Add("/title/default_hoppers/matchmaking_hopper_011.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\matchmaking_hopper_011.bin"));
                dictionary.Add("/title/default_hoppers/network_configuration_121.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\network_configuration_121.bin"));
                Manifest.FileEntry[] array = new Manifest.FileEntry[dictionary.Count];
                int num = 0;
                foreach (KeyValuePair<string, byte[]> item in dictionary)
                {
                    array[num] = new Manifest.FileEntry
                    {
                        filePath = item.Key,
                        fileHash = item.Value
                    };
                    num++;
                }
                Manifest chunk3 = new Manifest
                {
                    files = array
                };
                BlfFile blfFile4 = new BlfFile();
                blfFile4.AddChunk(chunk3);
                blfFile4.WriteFile(blfFolder + "\\default_hoppers\\manifest_001.bin");
                Console.WriteLine("Created file: manifest_001.bin");
            }
        }

        public string GetVersion()
        {
            return "11637";
        }
    }

}
