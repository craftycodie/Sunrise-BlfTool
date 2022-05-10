using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarthogInc.BlfChunks;

namespace Sunrise.BlfTool
{
    class HopperDescriptions : IBLFChunk
    {
        public byte descriptionCount;
        public HopperDescription[] descriptions;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            var ms = new BitStream<StreamByteStream>(new StreamByteStream(new MemoryStream()));
            WriteChunk(ref ms);
            return (uint)ms.ByteOffset;
        }

        public string GetName()
        {
            return "mhdf";
        }

        public ushort GetVersion()
        {
            return 3;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write<byte>(descriptionCount, 6);

            for (int i = 0; i < descriptionCount; i++)
            {
                HopperDescription description = descriptions[i];
                hoppersStream.Write<ushort>(description.identifier, 16);
                hoppersStream.Write<byte>(description.type ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteString(description.description, 256, Encoding.UTF8);
            }

            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public class HopperDescription
        {
            public ushort identifier;
            public bool type;
            public string description;
        }
    }
}
