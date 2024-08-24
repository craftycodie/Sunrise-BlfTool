using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks
{
    public class MessageOfTheDay : IBLFChunk
    {
        [JsonIgnore]
        public uint motdLength { get { return (uint)Encoding.UTF8.GetBytes(motdText).Length; } }
        public string motdText;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            return motdLength + 4;
        }

        public string GetName()
        {
            return "motd";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            uint motdLength = hoppersStream.Read<uint>(32);
            byte[] motdBytes = new byte[motdLength];
            for (int i = 0; i < motdLength; i++)
            {
                motdBytes[i] = hoppersStream.Read<byte>(8);
            }
            motdText = Encoding.UTF8.GetString(motdBytes);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(motdLength, 32);
            hoppersStream.WriteString(motdText);
            hoppersStream.SeekRelative(-1);
        }
    }
}
