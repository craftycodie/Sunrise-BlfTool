using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarthogInc.BlfChunks;

namespace Sunrise.BlfTool
{
    public class BlfFile
    {
        private static readonly int CHUNK_HEAD_SIZE = 0xC;

        protected LinkedList<IBLFChunk> chunks;

        public BlfFile()
        {
            chunks = new LinkedList<IBLFChunk>();
        }

        public void AddChunk(IBLFChunk chunk)
        {
            chunks.AddLast(chunk);
        }

        public T GetChunk<T>() {
            foreach (IBLFChunk chunk in chunks)
            {
                if (chunk is T) return (T)chunk;
            }
            throw new Exception("Chunk not found.");
        }

        public void ReadFile(string path)
        {
            var blfFileIn = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream(path, FileMode.Open)));

            BLFChunkReader chunkReader = new BLFChunkReader();

            while (true) {
                IBLFChunk chunk = chunkReader.ReadChunk(ref blfFileIn);

                if (chunk is EndOfFile)
                    break;
                if (chunk is _blf)
                    continue;

                chunks.AddLast(chunk);
            }

        }

        public void WriteFile(string path)
        {
            var blfFileOut = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream(path, FileMode.Create)));

            BLFChunkWriter blfChunkWriter = new BLFChunkWriter();
            blfChunkWriter.WriteChunk(ref blfFileOut, new _blf());

            foreach (IBLFChunk chunk in chunks)
            {
                blfChunkWriter.WriteChunk(ref blfFileOut, chunk);
            }

            blfChunkWriter.WriteChunk(ref blfFileOut, new EndOfFile((uint)(CHUNK_HEAD_SIZE * (chunks.Count + 1))));
            blfFileOut.Write(0, 8);
        }
    }
}
