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
    public class UserBanhammer : IBLFChunk
    {
        [JsonIgnore]
        public int BanCount { get { return bans.Length; } }
        public int unknown4;
        public Ban[] bans;

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
            return "fubh";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            var count = BanCount;

            if (bans.Length > 32)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too many bans! I can only write the first 32 :(");
                Console.ResetColor();
                count = 32;
            }

            hoppersStream.Write(count, 32);
            hoppersStream.Write(unknown4, 32);

            for (int i = 0; i < count; i++)
            {
                Ban entry = bans[i];

                hoppersStream.Write(entry.banType, 32);
                hoppersStream.Write(entry.banMessageIndex, 32);
                hoppersStream.WriteDate(entry.startTime, 64);
                hoppersStream.WriteDate(entry.endTime, 64);
            }
        }

        public class Ban
        {

            // 0 = unk
            // 1 = matchmaking
            // 2 = unk
            // 3 = unk
            // 4 = unk
            // 5 = unk
            // 6 = unk
            // 7 = unk
            // 8 = xbox live
            // 9 = unk

            public int banType;
            public int banMessageIndex;
            public DateTime startTime;
            public DateTime endTime;
        }
    }
}
