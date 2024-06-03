using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks
{
    class StartOfFile : IBLFChunk
    {
        public ushort GetAuthentication()
        {
            return 2;
        }

        public string GetName()
        {
            return "_blf";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public enum ByteOrder : ushort
        {
            BIG = 0xFFFE,
            LITTLE = 0xFEFF
        }

        ByteOrder byteOrder;
        byte[] unknown = new byte[0x22];

        public StartOfFile()
        {
            byteOrder = ByteOrder.BIG;
            unknown = new byte[0x22];
        }

        public uint GetLength()
        {
            return 0x24;
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write((ushort)byteOrder, 16);
            foreach (byte item in unknown)
            {
                hoppersStream.Write(item, 8);
            }
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            byteOrder = (ByteOrder)hoppersStream.Read<ushort>(16);
            for (int i = 0; i < 0x22; i++)
            {
                unknown[i] = hoppersStream.Read<byte>(8);
            }
        }
    }
}
