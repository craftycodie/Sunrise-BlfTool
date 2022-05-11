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
        public string blfChunkName; // 4 characters
        public uint chunkLength;
        public ushort version;
        public ushort authentication;

        public BLFChunkHeader(IBLFChunk blfChunk)
        {
            blfChunkName = blfChunk.GetName();
            chunkLength = blfChunk.GetLength() + GetLength();
            version = blfChunk.GetVersion();
            authentication = blfChunk.GetAuthentication();
        }

        public BLFChunkHeader()
        {

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

        public void ReadHeader(ref BitStream<StreamByteStream> hoppersStream)
        {
            blfChunkName = hoppersStream.ReadString(5);
            hoppersStream.SeekRelative(-1);
            chunkLength = hoppersStream.Read<uint>(32);
            version = hoppersStream.Read<ushort>(16);
            authentication = hoppersStream.Read<ushort>(16);
        }

    }
}
