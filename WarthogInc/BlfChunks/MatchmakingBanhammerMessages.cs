using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseBlfTool.BlfChunks;

namespace SunriseBlfTool
{
    class MatchmakingBanhammerMessages : IBLFChunk
    {
        [JsonIgnore]
        public uint messageCount { get { return (uint)messages.Length; } }
        public string[] messages;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            return (uint)(messageCount * 0x100) + 4;
        }

        public string GetName()
        {
            return "bhms";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            int tipCount = hoppersStream.Read<int>(32);
            messages = new string[tipCount];
            for (int i = 0; i < tipCount; i++)
            {
                byte[] tipBytes = new byte[0x100];
                int tipLength = tipBytes.Length;
                for (int j = 0; j < tipBytes.Length; j++)
                {
                    byte tipByte = hoppersStream.Read<byte>(8);
                    if (tipByte == 0)
                    {
                        tipLength = j;
                        hoppersStream.SeekRelative(tipBytes.Length - j - 1);
                        break;
                    } 
                    else
                    {
                        tipBytes[j] = tipByte;
                    }
                }

                messages[i] = Encoding.UTF8.GetString(tipBytes.Take(tipLength).ToArray());
            }
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(messageCount, 32);
            for (int i = 0; i < messageCount; i++)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(messages[i]);
                int messageLength = messageBytes.Length;
                for (int j = 0; j < 0x100; j++)
                {
                    if (j < messageLength)
                    {
                        hoppersStream.Write(messageBytes[j], 8);
                    }
                    else
                    {
                        hoppersStream.Write(0, 8);
                    }
                }
            }
        }
    }
}
