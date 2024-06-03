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
    class MatchmakingTips : IBLFChunk
    {
        [JsonIgnore]
        public uint tipCount { get { return (uint)tips.Length; } }
        public string[] tips;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            return (uint)(tipCount * 0x100) + 4;
        }

        public string GetName()
        {
            return "mmtp";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            int tipCount = hoppersStream.Read<int>(32);
            tips = new string[tipCount];
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

                tips[i] = Encoding.UTF8.GetString(tipBytes.Take(tipLength).ToArray());
            }
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(tipCount, 32);
            for (int i = 0; i < tipCount; i++)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(tips[i]);
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
