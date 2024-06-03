using SunriseBlfTool;
using SunriseBlfTool.BlfChunks;
using SunriseBlfTool.BlfChunks.ChunkNameMaps;
using SunriseBlfTool.BlfChunks.ChunkNameMaps.Halo3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.TitleConverters.Halo3
{
    public class TitleConverter_08172 : ITitleConverter
    {
        private static readonly AbstractBlfChunkNameMap chunkNameMap = new BlfChunkNameMap08172();

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
            File.Copy(jsonFolder + "\\matchmaking_nightmap.jpg", blfFolder + "\\matchmaking_nightmap.jpg", overwrite: true);
            File.Copy(jsonFolder + "\\network_configuration_080.bin", blfFolder + "\\network_configuration_080.bin", overwrite: true);
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                string text = current.Substring(current.LastIndexOf("\\") + 1);
                Console.WriteLine("Converting " + text);
                IEnumerator<string> enumerator2 = Directory.EnumerateFiles(current, "*.*", SearchOption.AllDirectories).GetEnumerator();
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
                        continue;
                    }
                    BlfFile blfFile = BlfFile.FromJSON(File.ReadAllText(enumerator2.Current), chunkNameMap);
                    if (!(text2 == "matchmaking_hopper_008.json"))
                    {
                        blfFile.WriteFile(blfFolder + text3.Replace(".json", ".bin"));
                        Console.WriteLine("Converted file: " + text3);
                    }
                }
                HopperConfigurationTable8 chunk = BlfFile.FromJSON(File.ReadAllText(jsonFolder + "default_hoppers\\matchmaking_hopper_008.json"), chunkNameMap).GetChunk<HopperConfigurationTable8>();
                BlfFile blfFile2 = new BlfFile();
                blfFile2.AddChunk(chunk);
                blfFile2.WriteFile(blfFolder + "\\default_hoppers\\matchmaking_hopper_008.bin");
                Console.WriteLine("Converted file: default_hoppers\\matchmaking_hopper_008.json");
            }
            Dictionary<string, byte[]> obj = new Dictionary<string, byte[]>
        {
            {
                "/title/default_hoppers/matchmaking_hopper_008.bin",
                BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\matchmaking_hopper_008.bin")
            },
            {
                "/title/default_hoppers/matchmaking_hopper_descriptions_002.bin",
                BlfFile.ComputeHash(blfFolder + "\\default_hoppers\\matchmaking_hopper_descriptions_002.bin")
            },
            {
                "/title/network_configuration_080.bin",
                BlfFile.ComputeHash(blfFolder + "\\network_configuration_080.bin")
            }
        };
            Manifest.FileEntry[] array = new Manifest.FileEntry[obj.Count];
            int num = 0;
            foreach (KeyValuePair<string, byte[]> item in obj)
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
            BlfFile blfFile3 = new BlfFile();
            blfFile3.AddChunk(chunk2);
            blfFile3.WriteFile(blfFolder + "\\manifest_001.bin");
            Console.WriteLine("Created file: manifest_001.bin");
        }

        public virtual string GetVersion()
        {
            return "08172";
        }
    }
}
