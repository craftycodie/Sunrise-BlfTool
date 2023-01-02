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
        public IBLFChunk ReadChunk(ref BitStream<StreamByteStream> outputStream, AbstractBlfChunkNameMap chunkNameMap)
        {
            BLFChunkHeader header = new BLFChunkHeader();
            header.ReadHeader(ref outputStream);

            if (header.chunkLength <= header.GetLength())
                throw new InvalidDataException("Bad chunk length!");

            try
            {
                IBLFChunk chunk = chunkNameMap.GetChunk(header.blfChunkName);
                chunk.ReadChunk(ref outputStream);
                return chunk;
            } catch(KeyNotFoundException knf)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unrecognized chunk {header.blfChunkName}, skipping...");
                Console.ResetColor();
                outputStream.SeekRelative((int)(header.chunkLength - header.GetLength()));

                return null;
            }
        }
    }
}
