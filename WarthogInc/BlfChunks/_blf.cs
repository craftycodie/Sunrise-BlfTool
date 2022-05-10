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
    class _blf : IBLFChunk
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
        char[] unknown = new char[0x22];

        public _blf()
        {
            byteOrder = ByteOrder.BIG;
            unknown = new char[0x22];
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
            throw new NotImplementedException();
        }
    }
}
