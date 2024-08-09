using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks;
using SunriseBlfTool.BlfChunks.ChunkNameMaps;
using SunriseBlfTool.BlfChunks.ChunkNameMaps.HaloReach;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool
{
    public class ReachVariantConverter
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

        class FakeChdr : IBLFChunk
        {
            public FakeChdr()
            {

            }


            public ushort GetAuthentication()
            {
                return 1;
            }

            public uint GetLength()
            {
                return 0x2C0;
            }

            public string GetName()
            {
                return "chdr";
            }

            public ushort GetVersion()
            {
                return 10;
            }

            public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
            {
                //
            }

            public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
            {
                //
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

        class MvarChunk : IBLFChunk
        {
            public byte[] mapHash;
            public byte[] mapData;

            public MvarChunk()
            {

            }

            public MvarChunk(byte[] mapData)
            {
                this.mapData = mapData;
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
                return "mvar";
            }

            public ushort GetVersion()
            {
                return 31;
            }

            public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
            {
                mapHash = new byte[20];
                for (uint i = 0; i < 20; i++)
                {
                    mapHash[i] = hoppersStream.Read<byte>(8);
                }

                uint length = hoppersStream.Read<uint>(32);
                mapData = new byte[length];
                for (uint i = 0; i < length; i++)
                {
                    mapData[i] = hoppersStream.Read<byte>(8);
                }
            }

            public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
            {
                foreach (byte b in mapHash)
                {
                    hoppersStream.Write(b, 8);
                }

                hoppersStream.Write(mapData.Length, 32);

                foreach (byte _byte in mapData)
                {
                    hoppersStream.Write(_byte, 8);
                }
            }
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
                RegisterChunk<MvarChunk>();
                RegisterChunk<FakeChdr>();
            }

            public override string GetVersion()
            {
                return "reach_12065";
            }
        }

        static AbstractBlfChunkNameMap chunkNameMap = new ConversionChunkNameMap();

        public static void ConvertVariant(string inputPath, string outputPath)
        {
            try
            {
                BlfFile blfFile = new BlfFile();
                blfFile.ReadFile(inputPath, chunkNameMap);

                BlfFile convertedBlf = new BlfFile();
                convertedBlf.AddChunk(new Author()
                {
                    buildName = "blftool variant",
                    buildNumber = 12065,
                    shellVersion = "",
                    unknown40 = "",
                });
                
                if (blfFile.HasChunk<MpvrChunk>())
                {
                    convertedBlf.AddChunk(new GvarChunk(blfFile.GetChunk<MpvrChunk>().gametypeData));
                }

                if (blfFile.HasChunk<MvarChunk>())
                {
                    convertedBlf.AddChunk(blfFile.GetChunk<MvarChunk>());
                }

                string fileName = Path.GetFileName(outputPath);
                if (!fileName.Contains("."))
                {
                    IBLFChunk convertedChunk = null;
                    // if there's no extension, make one.
                    if (convertedBlf.HasChunk<GvarChunk>())
                    {
                        convertedChunk = convertedBlf.GetChunk<GvarChunk>();
                    }
                    if (convertedBlf.HasChunk<MvarChunk>())
                    {
                        convertedChunk = convertedBlf.GetChunk<MvarChunk>();
                    }

                    if (convertedChunk != null)
                    {
                        outputPath = outputPath + $"_{convertedChunk.GetVersion().ToString("D3")}.bin";
                    }
                }

                convertedBlf.WriteFile(outputPath);

            }
            catch (NoConversionNecessaryException)
            {
                Console.WriteLine("Attempted to convert a gvar to a gvar. Copying instead...");
                File.Copy(inputPath, outputPath);
            }
        }

        public static void ConvertVariantFolder(string inputPath, string outputPath)
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
                if (fileName.EndsWith(".bak"))
                {
                    Console.WriteLine("Skipping .bak file " + fileName);
                }

                ConvertVariant(filePath, outputPath + Path.DirectorySeparatorChar + fileName.Replace(".mvar", ".bin"));
                Console.WriteLine("Successfully converted file: " + fileName);
                succeededCount++;
            }
            Console.WriteLine($"Converted {succeededCount} files.");
        }
    }
}
