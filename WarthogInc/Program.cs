using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using Sunrise.BlfTool;
using Sunrise.BlfTool.TitleConverters;
using SunriseBlfTool.BlfChunks;
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
            if (args.Length == 4) {
                if (args[0].Equals("json"))
                {
                    ConvertBlfToJson(args[1], args[2]);
                }
                if (args[0].Equals("blf"))
                {
                    ConvertJsonToBlf(args[1], args[2], args[3]);
                }
            }
            else
            {
                //ConvertJsonToBlf(@"D:\Projects\GitHub\Sunrise-Content\Title Storage\json\", @"D:\Projects\GitHub\Sunrise-Content\Title Storage\blf\");
            }
        }

        public static void ConvertJsonToBlf(string jsonFolder, string blfFolder, string version)
        {
            var titleConverter = TitleConverterVersionMap.singleton.GetConverter(version);

            if (titleConverter == null)
            {
                throw new NotImplementedException("No converter for version " + version);
            }

            titleConverter.ConvertJsonToBlf(jsonFolder, blfFolder);
        }

        public static void ConvertBlfToJson(string titleStorageFolder, string jsonFolder)
        {
            Console.WriteLine("Converting BLF files to JSON...");

            var titleDirectoryEnumerator = Directory.EnumerateFiles(titleStorageFolder, "*.*", SearchOption.AllDirectories).GetEnumerator();

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

                if (titleDirectoryEnumerator.Current.EndsWith(".bin") 
                    || titleDirectoryEnumerator.Current.EndsWith(".mvar") 
                    || titleDirectoryEnumerator.Current.EndsWith(".blf")
                    || !titleDirectoryEnumerator.Current.Contains('.')
                ) {
                    Console.WriteLine("Converting file: " + fileRelativePath);

                    try
                    {
                        BlfFile blfFile = new BlfFile();
                        blfFile.ReadFile(titleDirectoryEnumerator.Current);
                        string output = blfFile.ToJSON();

                        File.WriteAllText(jsonFolder + fileRelativePath.Replace(".bin", "").Replace(".mvar", "").Replace(".blf", "") + ".json", output);
                        //Console.WriteLine("Converted file: " + fileRelativePath);
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
    }
}
