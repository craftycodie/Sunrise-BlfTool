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
    public class TitleConverter_10015 : ITitleConverter
    {
        private static readonly AbstractBlfChunkNameMap chunkNameMap = new BlfChunkNameMap10015();

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
                    else if (text2.EndsWith(".json") && !(text2 == "game_set_003.json") && !(text2 == "matchmaking_hopper_009.json"))
                    {
                        BlfFile blfFile = BlfFile.FromJSON(File.ReadAllText(enumerator2.Current), chunkNameMap);
                        blfFile.WriteFile(blfFolder + text3.Replace(".json", ".bin"));
                        Console.WriteLine("Converted file: " + text3);
                        if (blfFile.HasChunk<MatchmakingHopperDescriptions2>() || blfFile.HasChunk<MatchmakingTips>() || blfFile.HasChunk<MapManifest>() || blfFile.HasChunk<MatchmakingBanhammerMessages>())
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
                    if (!text5.EndsWith(".json"))
                    {
                        continue;
                    }
                    BlfFile blfFile2 = BlfFile.FromJSON(File.ReadAllText(enumerator2.Current), chunkNameMap);
                    IBLFChunk iBLFChunk = null;
                    if (text5 == "game_set_003.json")
                    {
                        iBLFChunk = blfFile2.GetChunk<GameSet3>();
                    }
                    if (iBLFChunk != null)
                    {
                        GameSet3.GameEntry[] gameEntries = (iBLFChunk as GameSet3).gameEntries;
                        foreach (GameSet3.GameEntry gameEntry in gameEntries)
                        {
                            gameEntry.gameVariantHash = BlfFile.ComputeHash(blfFolder + text7 + "\\" + gameEntry.gameVariantFileName + "_005.bin");
                        }
                        blfFile2.WriteFile(blfFolder + text6.Replace(".json", ".bin"));
                        Console.WriteLine("Converted file: " + text6);
                    }
                }
                HopperConfigurationTable9 chunk = BlfFile.FromJSON(File.ReadAllText(jsonFolder + "default_hoppers\\matchmaking_hopper_009.json"), chunkNameMap).GetChunk<HopperConfigurationTable9>();
                HopperConfigurationTable9.HopperConfiguration[] configurations = chunk.configurations;
                foreach (HopperConfigurationTable9.HopperConfiguration hopperConfiguration in configurations)
                {
                    hopperConfiguration.gameSetHash = BlfFile.ComputeHash(blfFolder + "default_hoppers\\" + hopperConfiguration.identifier.ToString("D5") + "\\game_set_003.bin");
                }
                BlfFile blfFile3 = new BlfFile();
                blfFile3.AddChunk(chunk);
                blfFile3.WriteFile(blfFolder + "\\default_hoppers\\matchmaking_hopper_009.bin");
                Console.WriteLine("Converted file: default_hoppers\\matchmaking_hopper_009.json");
                dictionary.Add("/title/default_hoppers/matchmaking_hopper_009.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\matchmaking_hopper_009.bin"));
                dictionary.Add("/title/default_hoppers/network_configuration_088.bin", BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\network_configuration_088.bin"));
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
                Manifest chunk2 = new Manifest
                {
                    files = array
                };
                BlfFile blfFile4 = new BlfFile();
                blfFile4.AddChunk(chunk2);
                blfFile4.WriteFile(blfFolder + "\\default_hoppers\\manifest_001.bin");
                Console.WriteLine("Created file: manifest_001.bin");
            }
        }

        public virtual string GetVersion()
        {
            return "10015";
        }
    }
}
