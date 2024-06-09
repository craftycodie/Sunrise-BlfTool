using SunriseBlfTool.TitleConverters;

namespace SunriseBlfTool
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
                else
                {
                    Console.WriteLine("Invalid command.");
                }
            }
            else
            {
                Console.WriteLine("Not enough arguments provided.");
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
