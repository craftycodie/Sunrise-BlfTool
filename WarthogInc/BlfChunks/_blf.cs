using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc.BlfChunks
{
    class _blf : IBLFChunk
    {
        public short GetAuthentication()
        {
            return 2;
        }

        public string GetName()
        {
            return "_blf";
        }

        public short GetVersion()
        {
            return 1;
        }

        public enum ByteOrder
        {
            BIG = 0xFFFE,
            LITTLE = 0xFEFF
        }

        ByteOrder byteOrder;
        byte[] unknown = new byte[0x22];

        public static _blf GetDefaultHeader()
        {
            _blf blf = new _blf();
            blf.byteOrder = ByteOrder.BIG;
            blf.unknown = new byte[0x22];
            return blf;
        }

        public int GetLength()
        {
            return 0x24;
        }

        public void WriteChunk(BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(byteOrder, 16);
            hoppersStream.Write(unknown, 0x22 * 8);
        }

        public void ReadChunk(BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }
    }
}
