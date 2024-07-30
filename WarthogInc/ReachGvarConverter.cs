using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks;
using SunriseBlfTool.BlfChunks.ChunkNameMaps;
using SunriseBlfTool.BlfChunks.ChunkNameMaps.HaloReach;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool
{
    public class ReachGvarConverter
    {
        class NoConversionNecessaryException : Exception
        {
            public NoConversionNecessaryException(string v) : base(v)
            {
                
            }
        }

        class GvarChunk : IBLFChunk
        {
            readonly byte[] gametypeData;

            public GvarChunk()
            {
                
            }

            public GvarChunk(byte[] gametypeData)
            {
                this.gametypeData = gametypeData;
            }


            public ushort GetAuthentication()
            {
                return 1;
            }

            public uint GetLength()
            {
                var ms = new BitStream<StreamByteStream>(new StreamByteStream(new MemoryStream()));
                WriteChunk(ref ms);
                return (uint)ms.NextByteIndex;
            }

            public string GetName()
            {
                return "gvar";
            }

            public ushort GetVersion()
            {
                return 54;
            }

            public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
            {
                throw new NoConversionNecessaryException("Attempted to convert a Gvar to a Gvar.");
            }

            public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
            {
                foreach (byte _byte in gametypeData)
                {
                    hoppersStream.Write(_byte, 8);
                }
            }
        }

        class MpvrChunk : IBLFChunk
        {
            public byte[] gametypeData;

            public ushort GetAuthentication()
            {
                return 1;
            }

            public uint GetLength()
            {
                var ms = new BitStream<StreamByteStream>(new StreamByteStream(new MemoryStream()));
                WriteChunk(ref ms);
                return (uint)ms.NextByteIndex;
            }

            public string GetName()
            {
                return "mpvr";
            }

            public ushort GetVersion()
            {
                return 54;
            }

            public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.SeekRelative(24);
                uint gametypeDataLength = hoppersStream.Read<uint>(32);
                gametypeData = new byte[gametypeDataLength];
                for (uint i = 0; i < gametypeDataLength; i++)
                {
                    gametypeData[i] = hoppersStream.Read<byte>(8);
                }
            }

            public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
            {
                throw new InvalidOperationException();
            }
        }

        public static BlfFile CreateGvarFile(byte[] gvarData)
        {
            BlfFile blfFile = new BlfFile();
            blfFile.AddChunk(new Author()
            {
                buildName = "blftool mcc mpvr",
                buildNumber = 12065,
                shellVersion = "",
                unknown40 = "",
            });
            blfFile.AddChunk(new GvarChunk(gvarData));
            return blfFile;
        }

        class ConversionChunkNameMap : AbstractBlfChunkNameMap
        {
            public ConversionChunkNameMap()
            {
                RegisterChunks();
            }

            private void RegisterChunks()
            {
                RegisterChunk<StartOfFile>();
                RegisterChunk<EndOfFile>();
                RegisterChunk<Author>();
                RegisterChunk<GvarChunk>();
                RegisterChunk<MpvrChunk>();
            }

            public override string GetVersion()
            {
                return "reach_12065";
            }
        }

        static AbstractBlfChunkNameMap chunkNameMap = new ConversionChunkNameMap();

        public static void ConvertMpvrToGvar(string inputPath, string outputPath)
        {
            try
            {
                BlfFile mpvrFile = new BlfFile();
                mpvrFile.ReadFile(inputPath, chunkNameMap);
                BlfFile converted = CreateGvarFile(mpvrFile.GetChunk<MpvrChunk>().gametypeData);
                converted.WriteFile(outputPath);
            }
            catch (NoConversionNecessaryException)
            {
                Console.WriteLine("Attempted to convert a gvar to a gvar. Copying instead...");
                File.Copy(inputPath, outputPath);
            }
        }

        public static void ConvertMpvrFolder(string inputPath, string outputPath)
        {
            int succeededCount = 0;
            foreach (string filePath in Directory.EnumerateFiles(inputPath))
            {
                string fileName = Path.GetFileName(filePath);

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Warning: Tried to convert a non-existent file somehow - " + fileName);
                    continue;
                }
                if (!filePath.EndsWith("_054.bin"))
                {
                    Console.WriteLine("Skipping non-variant file - " + fileName);
                    continue;
                }

                ConvertMpvrToGvar(filePath, outputPath + Path.DirectorySeparatorChar + fileName);
                Console.WriteLine("Successfully converted file: " + fileName);
                succeededCount++;
            }
            Console.WriteLine($"Converted {succeededCount} files.");
        }
    }
}
