using Sewer56.BitStream.ByteStreams;
using Sewer56.BitStream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool.BlfChunks
{
    internal class MatchmakingHopperDescriptions2 : IBLFChunk
    {
        public class HopperDescription
        {
            public ushort identifier;

            public bool type;

            public string description;
        }

        public HopperDescription[] descriptions;

        [JsonIgnore]
        public byte descriptionCount => (byte)descriptions.Length;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            var ms = new BitStream<StreamByteStream>(new StreamByteStream(new MemoryStream()));
            WriteChunk(ref ms);
            return (uint)ms.NextByteIndex;
        }

        public string GetName()
        {
            return "mhdf";
        }

        public ushort GetVersion()
        {
            return 2;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> stream)
        {
            var memoryStream = new MemoryStream();
            var hoppersStream = new BitStream<StreamByteStream>(new StreamByteStream(memoryStream));

            hoppersStream.WriteBitswapped(descriptionCount, 6);
            for (int i = 0; i < descriptionCount; i++)
            {
                HopperDescription hopperDescription = descriptions[i];
                hoppersStream.WriteBitswapped(hopperDescription.identifier, 16);
                hoppersStream.WriteBitswapped(hopperDescription.type ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswappedString(hopperDescription.description, 256, Encoding.UTF8);
            }
            if (hoppersStream.BitIndex % 8 != 0)
            {
                hoppersStream.WriteBitswapped((byte)0, 8 - hoppersStream.BitIndex % 8);
            }
            memoryStream.Seek(0L, SeekOrigin.Begin);
            while (memoryStream.Position < memoryStream.Length)
            {
                stream.WriteBitswapped((byte)memoryStream.ReadByte(), 8);
            }
        }
    }

}
