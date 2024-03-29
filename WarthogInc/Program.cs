﻿using Newtonsoft.Json;
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
            if (args.Length == 4)
            {
                if (args[0].Equals("json"))
                {
                    ConvertBlfToJson(args[1], args[2], args[3]);
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

        public static void ConvertBlfToJson(string titleStorageFolder, string jsonFolder, string version)
        {
            var titleConverter = TitleConverterVersionMap.singleton.GetConverter(version);

            if (titleConverter == null)
            {
                throw new NotImplementedException("No converter for version " + version);
            }

            titleConverter.ConvertBlfToJson(titleStorageFolder, jsonFolder);
        }
    }
}
