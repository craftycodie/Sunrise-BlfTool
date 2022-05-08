using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc.BlfChunks
{
    interface IBLFChunk
    {
        public short GetVersion();
        public short GetAuthentication();
        public string GetName();
        public int GetLength();
        public void WriteChunk(BitStream<StreamByteStream> hoppersStream);
        public void ReadChunk(BitStream<StreamByteStream> hoppersStream);
    }
}
