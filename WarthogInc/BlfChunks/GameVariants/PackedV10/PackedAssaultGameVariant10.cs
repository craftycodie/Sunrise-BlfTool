using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV10
{
    public class PackedAssaultGameVariant10 : PackedBaseGameVariant10
    {
        public PackedAssaultGameVariant10() { }

        public PackedAssaultGameVariant10(ref BitStream<StreamByteStream> hoppersStream)
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
            base.Read(ref hoppersStream);
            resetBombOnDisarm = hoppersStream.Read<byte>(1) > 0;
            assaultMode = (AssaultGameType)hoppersStream.Read<byte>(2);
            assaultRespawn = (AssaultRespawn)hoppersStream.Read<byte>(3);
            enemyBombWaypoint = (EnemyBombWaypoint)hoppersStream.Read<byte>(3);
            scoreToWin = hoppersStream.Read<byte>(6);
            suddenDeathSeconds = (SuddenDeathSeconds)hoppersStream.Read<short>(9);
            bombArmingTime = hoppersStream.Read<byte>(5);
            bombDisarmingTime = hoppersStream.Read<byte>(5);
            bombFuseTime = hoppersStream.Read<byte>(5);
            bombResetTime = hoppersStream.Read<byte>(6);
            bombCarrierTraits = new PlayerTraits(ref hoppersStream);
            unknownPlayerTraits = new PlayerTraits(ref hoppersStream);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Write(ref hoppersStream);
            hoppersStream.Write(resetBombOnDisarm ? 1 : 0, 1);
            hoppersStream.Write((byte)assaultMode, 2);
            hoppersStream.Write((byte)assaultRespawn, 3);
            hoppersStream.Write((byte)enemyBombWaypoint, 3);
            hoppersStream.Write(scoreToWin, 6);
            hoppersStream.Write((short)suddenDeathSeconds, 9);
            hoppersStream.Write(bombArmingTime, 5);
            hoppersStream.Write(bombDisarmingTime, 5);
            hoppersStream.Write(bombFuseTime, 5);
            hoppersStream.Write(bombResetTime, 6);
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
