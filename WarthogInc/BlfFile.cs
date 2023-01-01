using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using WarthogInc.BlfChunks;

namespace Sunrise.BlfTool
{
    public class BlfFile 
    {
        [JsonConverter(typeof(BlfFileConverter))]
        protected Dictionary<string, IBLFChunk> chunks;

        public string ToJSON()
        {
            if (chunks.Count < 1)
                throw new Exception("There are no chunks to convert.");

            var jsonSettings = new JsonSerializerSettings { Converters = { new ByteArrayConverter(), new HexStringConverter() }, Formatting = Formatting.Indented };
            return JsonConvert.SerializeObject(chunks, jsonSettings);
        }

        public static BlfFile FromJSON(string json, AbstractBlfChunkNameMap chunkNameMap)
        {
            BlfFile blfFile = new BlfFile();
            var jsonSettings = new JsonSerializerSettings { Converters = { new ByteArrayConverter(), new HexStringConverter(), new BlfFileConverter(chunkNameMap) }, Formatting = Formatting.Indented };
            blfFile.chunks = JsonConvert.DeserializeObject<Dictionary<string, IBLFChunk>>(json, jsonSettings);
            return blfFile;
        }

        public BlfFile()
        {
            chunks = new Dictionary<string, IBLFChunk>();
        }

        public void AddChunk(IBLFChunk chunk)
        {
            chunks.Add(chunk.GetName(), chunk);
        }

        public T GetChunk<T>() where T : IBLFChunk, new() {
            IBLFChunk chunk = new T();
            return (T)chunks[chunk.GetName()];
        }

        public bool HasChunk<T>() where T : IBLFChunk, new()
        {
            IBLFChunk chunk = new T();
            return chunks.ContainsKey(chunk.GetName());
        }

        public void ReadFile(string path)
        {
            var fs = new FileStream(path, FileMode.Open);
            var blfFileIn = new BitStream<StreamByteStream>(new StreamByteStream(fs));

            BLFChunkReader chunkReader = new BLFChunkReader();

            // If it has an x360 save content header skip to blf
            if (blfFileIn.Read<uint>(32) == 0x434F4E20)
            {
                blfFileIn.Seek(0xD000);
            } else
            {
                blfFileIn.Seek(0);
            }

            while (fs.Position < fs.Length) {
                IBLFChunk chunk = chunkReader.ReadChunk(ref blfFileIn);

                if (chunk == null)
                    continue;
                if (chunk is EndOfFile)
                    break;
                if (chunk is StartOfFile)
                    continue;
                if (chunk is Author)
                    continue;

                chunks.Add(chunk.GetName(), chunk);
            }

        }

        public void WriteFile(string path)
        {
            var fileStream = new FileStream(path, FileMode.Create);
            var blfFileOut = new BitStream<StreamByteStream>(new StreamByteStream(fileStream));

            BLFChunkWriter blfChunkWriter = new BLFChunkWriter();
            blfChunkWriter.WriteChunk(ref blfFileOut, new StartOfFile());

            blfChunkWriter.WriteChunk(ref blfFileOut, new Author()
            {
                buildName = "Sunrise",
                buildNumber = 12070,
                shellVersion = "12070.08.09.05.2031.halo3_ship"
            });

            foreach (IBLFChunk chunk in chunks.Values)
            {
                blfChunkWriter.WriteChunk(ref blfFileOut, chunk);
            }

            blfChunkWriter.WriteChunk(ref blfFileOut, new EndOfFile(blfFileOut.ByteOffset));
            fileStream.Flush();
            fileStream.Close();
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
                memoryStream.Flush();

                byte[] saltedBlf = memoryStream.ToArray();
                memoryStream.Close();
                return new SHA1Managed().ComputeHash(saltedBlf);
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File Not Found: " + path);
                Console.ResetColor();
                return new byte[20];
            }
            catch (DirectoryNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File Not Found: " + path);
                Console.ResetColor();
                return new byte[20];
            }
        }
    }
}
