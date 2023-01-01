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
    public class TitleConverter_06481 : ITitleConverter
    {
        private static readonly AbstractBlfChunkNameMap chunkNameMap = new BlfChunkNameMap06481();
        public void ConvertBlfToJson(string blfFolder, string jsonFolder)
        {
            throw new NotImplementedException();
        }

        public string GetVersion()
        {
            return "06481";
        }

        public void ConvertJsonToBlf(string jsonFolder, string blfFolder)
        {
            jsonFolder += "\\";
            blfFolder += "\\";

            Console.WriteLine("Converting JSON files to BLF...");

            var jsonFileEnumerator = Directory.EnumerateFiles(jsonFolder, "*.*", SearchOption.AllDirectories).GetEnumerator();

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

                if (fileName.EndsWith(".bin") || fileName.EndsWith(".jpg") || fileName.EndsWith(".txt"))
                {
                    File.Copy(jsonFileEnumerator.Current, blfFolder + fileRelativePath, true);
                    Console.WriteLine("Copied file: " + fileRelativePath);

                    continue;
                }

                BlfFile blfFile = BlfFile.FromJSON(File.ReadAllText(jsonFileEnumerator.Current), chunkNameMap);

                if (fileName == "game_set_006.json")
                    continue; // handle after variants

                if (fileName == "matchmaking_hopper_002.json")
                    continue; // Handle after game sets


                blfFile.WriteFile(blfFolder + fileRelativePath.Replace(".json", ".bin"));

                Console.WriteLine("Converted file: " + fileRelativePath);
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

                if (fileName.EndsWith(".bin") || fileName.EndsWith(".jpg") || fileName.EndsWith(".txt"))
                {
                    continue;
                }

                BlfFile blfFile = BlfFile.FromJSON(File.ReadAllText(jsonFileEnumerator.Current), chunkNameMap);

                IBLFChunk blfChunk = null;

                if (fileName == "game_set_006.json")
                    blfChunk = blfFile.GetChunk<GameSet>();

                if (blfChunk != null)
                {

                    foreach (GameSet.GameEntry entry in (blfChunk as GameSet).gameEntries)
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
            var hopperConfigurationTableBlfFile = BlfFile.FromJSON(File.ReadAllText(jsonFolder + "matchmaking_hopper_002.json"), chunkNameMap);
            var mhcf = hopperConfigurationTableBlfFile.GetChunk<HopperConfigurationTable2>();

            BlfFile hoppersFile = new BlfFile();
            hoppersFile.AddChunk(mhcf);
            hoppersFile.WriteFile(blfFolder + "\\matchmaking_hopper_002.bin");

            Console.WriteLine("Converted file: matchmaking_hopper_002.json");
        }
    }
}
