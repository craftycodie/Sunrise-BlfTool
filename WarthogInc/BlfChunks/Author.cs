using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using Sunrise.BlfTool.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc.BlfChunks
{
    class Author : IBLFChunk
    {
        public ushort GetAuthentication()
        {
            return 1;
        }

        public string GetName()
        {
            return "athr";
        }

        public ushort GetVersion()
        {
            return 3;
        }

        byte[] unknown = new byte[0x44];

        public Author()
        {
            unknown = new byte[0x44];
        }

        public uint GetLength()
        {
            return 0x44;
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            foreach (byte item in unknown)
            {
                hoppersStream.Write(item, 8);
            }
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            for (int i = 0; i < 0x44; i++)
            {
                unknown[i] = hoppersStream.Read<byte>(8);
            }
        }
    }
}
