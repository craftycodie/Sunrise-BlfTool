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

namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV2
{
    public class PackedAssaultGameVariant2 : PackedBaseGameVariant2
    {
        public PackedAssaultGameVariant2() { }

        public PackedAssaultGameVariant2(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public bool resetBombOnDisarm;
        [JsonConverter(typeof(StringEnumConverter))]
        public AssaultGameType assaultMode; // 2
        [JsonConverter(typeof(StringEnumConverter))]
        public AssaultRespawn assaultRespawn; // 2
        [JsonConverter(typeof(StringEnumConverter))]
        public EnemyBombWaypoint enemyBombWaypoint;
        public byte scoreToWin; // 6
        [JsonConverter(typeof(StringEnumConverter))]
        public SuddenDeathSeconds suddenDeathSeconds; // 9
        public byte bombArmingTime; // 5
        public byte bombDisarmingTime; // 5
        public byte bombFuseTime; // 5
        public byte bombResetTime; // 6
        public PlayerTraits bombCarrierTraits;
        public PlayerTraits unknownPlayerTraits;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Write(ref hoppersStream);
            hoppersStream.WriteBitswapped(resetBombOnDisarm ? 1 : 0, 1);
            hoppersStream.WriteBitswapped((byte)assaultMode, 2);
            hoppersStream.WriteBitswapped((byte)assaultRespawn, 3);
            hoppersStream.WriteBitswapped((byte)enemyBombWaypoint, 3);
            hoppersStream.WriteBitswapped(scoreToWin, 6);
            hoppersStream.WriteBitswapped((short)suddenDeathSeconds, 9);
            hoppersStream.WriteBitswapped(bombArmingTime, 5);
            hoppersStream.WriteBitswapped(bombDisarmingTime, 5);
            hoppersStream.WriteBitswapped(bombFuseTime, 5);
            hoppersStream.WriteBitswapped(bombResetTime, 6);
            bombCarrierTraits.Write(ref hoppersStream);
            unknownPlayerTraits.Write(ref hoppersStream);
        }

        public enum AssaultGameType : byte
        {
            MULTI,
            SINGLE,
            NEUTRAL
        }

        public enum EnemyBombWaypoint : byte
        {

        }

        public enum AssaultRespawn : byte
        {

        }

        public enum SuddenDeathSeconds : short
        {
            //NO_LIMIT = -1
        }
    }
}
