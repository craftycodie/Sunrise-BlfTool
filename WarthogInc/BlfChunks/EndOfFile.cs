using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks
{
    class EndOfFile : IBLFChunk
    {
        public ushort GetAuthentication()
        {
            return 1;
        }

        public string GetName()
        {
            return "_eof";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public uint GetLength()
        {
            return 5;
        }
            
        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(lengthUpToEOF, 32);
            hoppersStream.Write(unknown, 8);
            //hoppersStream.Seek(hoppersStream.NextByteIndex);
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            lengthUpToEOF = hoppersStream.Read<int>(32);
            unknown = hoppersStream.Read<byte>(8);
        }

        int lengthUpToEOF;
        byte unknown;

        public EndOfFile()
        {

        }

        public EndOfFile(int _lengthUpToEOF)
        {
            lengthUpToEOF = _lengthUpToEOF;
        }
    }
}
