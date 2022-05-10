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
        uint chunkLength;
        ushort version;
        ushort authentication;

        public BLFChunkHeader(IBLFChunk blfChunk)
        {
            blfChunkName = blfChunk.GetName();
            chunkLength = blfChunk.GetLength() + GetLength();
            version = blfChunk.GetVersion();
            authentication = blfChunk.GetAuthentication();
        }

        public uint GetLength()
        {
            return 0xC;
        }

        public void WriteHeader(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.WriteString(blfChunkName, 4 * 8);
            hoppersStream.SeekRelative(-1);
            hoppersStream.Write(chunkLength, 32);
            hoppersStream.Write(version, 16);
            hoppersStream.Write(authentication, 16);
        }

    }
}
