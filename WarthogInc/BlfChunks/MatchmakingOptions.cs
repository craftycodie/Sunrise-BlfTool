using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks
{
    public class MatchmakingOptions : IBLFChunk
    {
        public ushort hopperIdentifier;
        public byte xLastIndex;
        public bool isRanked;
        public bool teamsEnabled;
        public string hopperName;
        public ulong drawProbability;
        public ulong beta;
        public ulong tau;
        public ulong expBaseIncrement;
        public ulong expPenaltyDecrement;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public string GetName()
        {
            return "mpmo";
        }

        public ushort GetVersion()
        {
            return 2;
        }

        public uint GetLength()
        {
            return 0x5C;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Warning: mpmo chunk definition is incomplete.");
            Console.ResetColor();

            hopperIdentifier = hoppersStream.Read<ushort>(16);

            LinkedList<byte> nameBytes = new LinkedList<byte>();
            for (int si = 0; si < 16; si++)
            {
                byte left = hoppersStream.Read<byte>(8);
                byte right = hoppersStream.Read<byte>(8);
                if (((left == 0 && right == 0) || si == 16) && hopperName == null)
                {
                    hopperName = Encoding.BigEndianUnicode.GetString(nameBytes.ToArray());
                }
                nameBytes.AddLast(left);
                nameBytes.AddLast(right);
            }

            hoppersStream.SeekRelative(0x3A);
        }
        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }
    }
}
