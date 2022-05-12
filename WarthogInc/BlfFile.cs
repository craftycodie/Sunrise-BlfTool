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
            var blfFileOut = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream(path, FileMode.Create)));

            BLFChunkWriter blfChunkWriter = new BLFChunkWriter();
            blfChunkWriter.WriteChunk(ref blfFileOut, new StartOfFile());

            foreach (IBLFChunk chunk in chunks)
            {
                blfChunkWriter.WriteChunk(ref blfFileOut, chunk);
            }

            blfChunkWriter.WriteChunk(ref blfFileOut, new EndOfFile((uint)(CHUNK_HEAD_SIZE * (chunks.Count + 1))));
            blfFileOut.Write(0, 8);
        }

        byte[] halo3salt = Convert.FromHexString("EDD43009666D5C4A5C3657FAB40E022F535AC6C9EE471F01F1A44756B7714F1C36EC");

        public byte[] ComputeHash()
        {

            var memoryStream = new MemoryStream();
            var blfFileOut = new BitStream<StreamByteStream>(new StreamByteStream(memoryStream));
            foreach(byte saltByte in halo3salt) {
                blfFileOut.Write(saltByte, 8);
            }

            BLFChunkWriter blfChunkWriter = new BLFChunkWriter();
            blfChunkWriter.WriteChunk(ref blfFileOut, new StartOfFile());

            foreach (IBLFChunk chunk in chunks)
            {
                blfChunkWriter.WriteChunk(ref blfFileOut, chunk);
            }

            blfChunkWriter.WriteChunk(ref blfFileOut, new EndOfFile((uint)(CHUNK_HEAD_SIZE * (chunks.Count + 1))));
            blfFileOut.Write(0, 8);

            byte[] saltedBlf = memoryStream.ToArray();
            return new SHA1Managed().ComputeHash(saltedBlf);
        }
    }
}
