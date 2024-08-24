using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.Extensions;
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
        public int drawProbability;
        public float beta;
        public float tau;
        public int expBaseIncrement;
        public int expPenaltyDecrement;

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

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            hopperIdentifier = hoppersStream.Read<ushort>(16);

            LinkedList<byte> nameBytes = new LinkedList<byte>();
            for (int si = 0; si < 32; si++)
            {
                byte left = hoppersStream.Read<byte>(8);
                byte right = hoppersStream.Read<byte>(8);
                if (((left == 0 && right == 0) || si == 32) && hopperName == null)
                {
                    hopperName = Encoding.BigEndianUnicode.GetString(nameBytes.ToArray());
                }
                nameBytes.AddLast(left);
                nameBytes.AddLast(right);
            }

            isRanked = hoppersStream.Read<byte>(8) > 0;
            teamsEnabled = hoppersStream.Read<byte>(8) > 0;
            xLastIndex = hoppersStream.Read<byte>(8);
            drawProbability = hoppersStream.Read<int>(32);
            beta = hoppersStream.ReadFloat(32);
            tau = hoppersStream.ReadFloat(32);
            expBaseIncrement = hoppersStream.Read<int>(32);
            expPenaltyDecrement = hoppersStream.Read<int>(32);

        }
        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }
    }
}
