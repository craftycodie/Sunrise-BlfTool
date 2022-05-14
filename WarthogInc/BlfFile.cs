using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WarthogInc.BlfChunks;

namespace Sunrise.BlfTool
{
    public class BlfFile
    {
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

        public IBLFChunk GetChunk(int index)
        {
            return chunks.ElementAt(index);
        }

        public void ReadFile(string path)
        {
            var blfFileIn = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream(path, FileMode.Open)));

            BLFChunkReader chunkReader = new BLFChunkReader();

            while (true) {
                IBLFChunk chunk = chunkReader.ReadChunk(ref blfFileIn);

                if (chunk is EndOfFile)
                    break;
                if (chunk is StartOfFile)
                    continue;

                chunks.AddLast(chunk);
            }

        }

        public void WriteFile(string path)
        {
            var fileStream = new FileStream(path, FileMode.Create);
            var blfFileOut = new BitStream<StreamByteStream>(new StreamByteStream(fileStream));

            BLFChunkWriter blfChunkWriter = new BLFChunkWriter();
            blfChunkWriter.WriteChunk(ref blfFileOut, new StartOfFile());

            foreach (IBLFChunk chunk in chunks)
            {
                blfChunkWriter.WriteChunk(ref blfFileOut, chunk);
            }

            blfChunkWriter.WriteChunk(ref blfFileOut, new EndOfFile(blfFileOut.ByteOffset));
            fileStream.Flush();
            fileStream.Close();
        }
    }
}
