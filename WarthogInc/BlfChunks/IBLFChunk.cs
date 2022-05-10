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
        public ushort GetVersion();
        public ushort GetAuthentication();
        public string GetName();
        public uint GetLength();
        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream);
        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream);
    }
}
