using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseBlfTool.BlfChunks;

namespace SunriseBlfTool
{
    class MapManifest : IBLFChunk
    {
        [JsonIgnore]
        public uint mapCount { get { return (uint)mapRSAs.Length; } }
        public MapEntry[] mapRSAs;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            return (mapCount * 256) + 4;
        }

        public string GetName()
        {
            return "mapm";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            int mapCount = hoppersStream.Read<int>(32);
            mapRSAs = new MapEntry[mapCount];
            for (int i = 0; i < mapCount; i++)
            {
                byte[] rsaBytes = new byte[0x100];
                for (int j = 0; j < 0x100; j++)
                {
                    rsaBytes[j] = hoppersStream.Read<byte>(8);
                }
                mapRSAs[i] = new MapEntry(rsaBytes);
            }
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(mapCount, 32);
            for (int i = 0; i < mapCount; i++)
            {
                byte[] rsaBytes = mapRSAs[i].rsaKey;
                for (int j = 0; j < 0x100; j++)
                {
                    hoppersStream.Write<byte>(rsaBytes[j], 8);
                }
            }
        }

        public class MapEntry
        {
            [JsonConverter(typeof(HexStringConverter))]
            public byte[] rsaKey;

            public MapEntry(byte[] rsaBytes)
            {
                rsaKey = rsaBytes;
            }
        }
    }
}
