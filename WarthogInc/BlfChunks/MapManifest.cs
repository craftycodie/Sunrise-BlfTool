using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarthogInc.BlfChunks;

namespace Sunrise.BlfTool
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

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
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
            //hoppersStream.Write<byte>(gameEntryCount, 6);

            //for (int i = 0; i < gameEntryCount; i++)
            //{
            //    FileEntry entry = gameEntries[i];
            //    //hoppersStream.Write<ushort>(entry.identifier, 16);
            //    //hoppersStream.Write<byte>(description.type ? (byte)1 : (byte)0, 1);
            //    //hoppersStream.WriteString(description.description, 256, Encoding.UTF8);
            //}

            //hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
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
