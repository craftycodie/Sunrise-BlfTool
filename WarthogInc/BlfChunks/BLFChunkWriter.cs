using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks
{
    class BLFChunkWriter
    {
        public void WriteChunk(ref BitStream<StreamByteStream> outputStream, IBLFChunk blfChunk)
        {
            BLFChunkHeader header = new BLFChunkHeader(blfChunk);
            header.WriteHeader(ref outputStream);
            blfChunk.WriteChunk(ref outputStream);

            if (outputStream.BitIndex % 8 != 0)
                outputStream.Write((byte)0, 8 - (outputStream.BitIndex % 8));
        }
    }
}
