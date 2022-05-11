using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc.BlfChunks
{
    public class BLFChunkReader
    {
        BlfChunkNameMap chunkNameMap = new BlfChunkNameMap();

        public IBLFChunk ReadChunk(ref BitStream<StreamByteStream> outputStream)
        {
            BLFChunkHeader header = new BLFChunkHeader();
            header.ReadHeader(ref outputStream);
            IBLFChunk chunk = chunkNameMap.GetChunk(header.blfChunkName);
            chunk.ReadChunk(ref outputStream);
            return chunk;
        }
    }
}
