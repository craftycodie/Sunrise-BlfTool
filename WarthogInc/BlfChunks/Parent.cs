using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLibDotNet;

namespace SunriseBlfTool.BlfChunks
{
    class Parent : IBLFChunk
    {
        public Dictionary<string, IBLFChunk> chunks;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public string GetName()
        {
            return "_par";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public uint GetLength()
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> stream)
        {
            throw new NotImplementedException();
        }

        public void ReadChunk(ref BitStream<StreamByteStream> stream, BLFChunkReader reader)
        {
            chunks = new Dictionary<string, IBLFChunk>();
            int chunksCount = stream.Read<int>(32);
            for (int i = 0; i < chunksCount; i++)
            {
                IBLFChunk chunk = reader.ReadChunk(ref stream);
                if (chunk != null)
                    chunks.Add(chunk.GetName(), chunk);
            }
        }

        public Parent()
        {

        }
    }
}
