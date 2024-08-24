using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseBlfTool.BlfChunks;

namespace SunriseBlfTool
{
    public class RecentPlayers : IBLFChunk
    {
        [JsonIgnore]
        public int PlayerCount { get { return recentPlayers.Length; } }
        public RecentPlayer[] recentPlayers;

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
            return "furp";
        }

        public ushort GetVersion()
        {
            return 2;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            var count = PlayerCount;

            if (recentPlayers.Length > 384)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too many players! I can only write the first 384 :(");
                Console.ResetColor();
                count = 384;
            }

            hoppersStream.Write(count, 32);

            for (int i = 0; i < count; i++)
            {
                RecentPlayer entry = recentPlayers[i];

                hoppersStream.Write(entry.unknown, 16);
                hoppersStream.WriteLong(entry.playerXuid, 64);
            }
        }

        public class RecentPlayer
        {

            public short unknown; // could be cheat/ban flags
            public ulong playerXuid;
        }
    }
}
