using SunriseBlfTool.TitleConverters;
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
    public class TitleConverter_06481 : ITitleConverter
    {
        private static readonly AbstractBlfChunkNameMap chunkNameMap = new BlfChunkNameMap06481();
        public void ConvertBlfToJson(string blfFolder, string jsonFolder)
        {
            Console.WriteLine("Converting BLF files to JSON...");

            var titleDirectoryEnumerator = Directory.EnumerateFiles(blfFolder, "*.*", SearchOption.AllDirectories).GetEnumerator();

            while (titleDirectoryEnumerator.MoveNext())
            {
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
                if (fileRelativePath.Contains("\\"))
                {
                    string fileDirectoryRelativePath = fileRelativePath.Substring(0, fileRelativePath.LastIndexOf("\\"));
                    Directory.CreateDirectory(blfFolder + fileDirectoryRelativePath);
                }

                if (fileName.EndsWith(".bin") || fileName.EndsWith(".jpg") || fileName.EndsWith(".txt"))
                {
                    File.Copy(jsonFileEnumerator.Current, blfFolder + fileRelativePath, true);
                    Console.WriteLine("Copied file: " + fileRelativePath);

                    continue;
                }

                BlfFile blfFile = BlfFile.FromJSON(File.ReadAllText(jsonFileEnumerator.Current), chunkNameMap);

                blfFile.WriteFile(blfFolder + fileRelativePath.Replace(".json", ".bin"));

                Console.WriteLine("Converted file: " + fileRelativePath);
            }
        }
    }
}
