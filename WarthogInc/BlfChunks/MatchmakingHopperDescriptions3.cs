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
    class MatchmakingHopperDescriptions3 : IBLFChunk
    {
        [JsonIgnore]
        public byte descriptionCount { get { return (byte)descriptions.Length; } }
        public HopperDescription[] descriptions;

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
            return 3;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            byte descriptionCount = hoppersStream.Read<byte>(6);
            descriptions = new HopperDescription[descriptionCount];

            for (int i = 0; i < descriptionCount; i++)
            {
                HopperDescription description = new HopperDescription();
                description.identifier = hoppersStream.Read<ushort>(16);
                description.type = hoppersStream.Read<byte>(1) > 0;
                description.description = hoppersStream.ReadString(256, Encoding.UTF8);
                descriptions[i] = description;
            }

            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
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
        }

        public class HopperDescription
        {
            public ushort identifier;
            public bool type;
            public string description;
        }
    }
}
