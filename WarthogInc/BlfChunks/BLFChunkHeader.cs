using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc.BlfChunks
{
    class BLFChunkHeader
    {
        string blfChunkName; // 4 characters
        int chunkLength;
        short version;
        short authentication;

        public BLFChunkHeader(IBLFChunk blfChunk)
        {
            blfChunkName = blfChunk.GetName();
            chunkLength = blfChunk.GetLength() + GetLength();
            version = blfChunk.GetVersion();
            authentication = blfChunk.GetAuthentication();
        }

        public int GetLength()
        {
            return 0xB;
        }

        public void WriteHeader(BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.WriteString(blfChunkName, 4);
            hoppersStream.Write(chunkLength, 32);
            hoppersStream.Write(version, 16);
            hoppersStream.Write(authentication, 16);
        }

    }
}
