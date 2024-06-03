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
    public class MessageOfTheDayPopup_PC : IBLFChunk
    {
        public uint motdIdentifier;

        public uint acceptWaitMilliseconds;

        public string title;

        public string heading;

        public string accept;

        public string wait;

        public string body;

        [JsonIgnore]
        public uint titleLength => (uint)title.Length;

        [JsonIgnore]
        public uint headingLength => (uint)heading.Length;

        [JsonIgnore]
        public uint acceptLength => (uint)accept.Length;

        [JsonIgnore]
        public uint waitLength => (uint)wait.Length;

        [JsonIgnore]
        public uint bodyLength => (uint)body.Length;

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
            return "mtdp";
        }

        public ushort GetVersion()
        {
            return 4;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write<uint>(motdIdentifier, 32);
            hoppersStream.Write<uint>(acceptWaitMilliseconds, 32);
            hoppersStream.WriteBitswapped(titleLength * 2, 32);
            hoppersStream.WriteString(title, (int)(titleLength * 2), Encoding.Unicode);
            hoppersStream.SeekRelative((int)(96 - titleLength * 2 - 1), (byte)0);
            hoppersStream.WriteBitswapped(headingLength * 2, 32);
            hoppersStream.WriteString(heading, (int)(headingLength * 2), Encoding.Unicode);
            hoppersStream.SeekRelative((int)(96 - headingLength * 2 - 1), (byte)0);
            hoppersStream.WriteBitswapped(acceptLength * 2, 32);
            hoppersStream.WriteString(accept, (int)(acceptLength * 2), Encoding.Unicode);
            hoppersStream.SeekRelative((int)(96 - acceptLength * 2 - 1), (byte)0);
            hoppersStream.WriteBitswapped(waitLength * 2, 32);
            hoppersStream.WriteString(wait, (int)(waitLength * 2), Encoding.Unicode);
            hoppersStream.SeekRelative((int)(96 - waitLength * 2 - 1), (byte)0);
            hoppersStream.WriteBitswapped(bodyLength * 2, 32);
            hoppersStream.WriteString(body, (int)(bodyLength * 2), Encoding.Unicode);
            hoppersStream.SeekRelative((int)(2048 - bodyLength * 2 - 1), (byte)0);
        }
    }

}
