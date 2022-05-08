using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc.BlfChunks
{
    class _eof : IBLFChunk
    {
        public short GetAuthentication()
        {
            return 1;
        }

        public string GetName()
        {
            return "_eof";
        }

        public short GetVersion()
        {
            return 1;
        }

        public int GetLength()
        {
            return 5;
        }

        public void WriteChunk(BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(lengthUpToEOF, 32);
            hoppersStream.Write(unknown, 8);
        }

        public void ReadChunk(BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        int lengthUpToEOF;
        byte unknown;

        public _eof(int _lengthUpToEOF)
        {
            lengthUpToEOF = _lengthUpToEOF;
        }
    }
}
