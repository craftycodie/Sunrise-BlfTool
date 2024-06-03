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
    public class PackedCTFGameVariant10 : PackedBaseGameVariant10
    {
        public PackedCTFGameVariant10() { }

        public PackedCTFGameVariant10(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public bool flagAtHomeToScore;
        [JsonConverter(typeof(StringEnumConverter))]
        public HomeFlagWaypoint homeFlagWaypoint; // 2
        [JsonConverter(typeof(StringEnumConverter))]
        public CTFGameType flagCount; // 2
        public CTFRespawn ctfRespawn; // 2
        public byte scoreToWin; // 6
        [JsonConverter(typeof(StringEnumConverter))]
        public SuddenDeathSeconds suddenDeathSeconds; // 9
        public short flagResetSeconds; // 9
        [JsonConverter(typeof(StringEnumConverter))]
        public FlagReturnTime flagReturnSeconds; // 9
        public PlayerTraits flagCarrierTraits;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Read(ref hoppersStream);
            flagAtHomeToScore = hoppersStream.Read<byte>(1) > 0;
            homeFlagWaypoint = (HomeFlagWaypoint)hoppersStream.Read<byte>(2);
            flagCount = (CTFGameType)hoppersStream.Read<byte>(2);
            ctfRespawn = (CTFRespawn)hoppersStream.Read<byte>(2);
            scoreToWin = hoppersStream.Read<byte>(6);
            suddenDeathSeconds = (SuddenDeathSeconds)hoppersStream.Read<short>(9);
            flagResetSeconds = hoppersStream.Read<short>(9);
            flagReturnSeconds = (FlagReturnTime)hoppersStream.Read<short>(9);
            flagCarrierTraits = new PlayerTraits(ref hoppersStream);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Write(ref hoppersStream);
            hoppersStream.Write(flagAtHomeToScore ? 1 : 0, 1);
            hoppersStream.Write((byte)homeFlagWaypoint, 2);
            hoppersStream.Write((byte)flagCount, 2);
            hoppersStream.Write((byte)ctfRespawn, 2);
            hoppersStream.Write(scoreToWin, 6);
            hoppersStream.Write((short)suddenDeathSeconds, 9);
            hoppersStream.Write(flagResetSeconds, 9);
            hoppersStream.Write((short)flagReturnSeconds, 9);
            flagCarrierTraits.Write(ref hoppersStream);
        }

        public enum CTFGameType : byte
        {
            MULTI_FLAG,
            ONE_FLAG
        }

        public enum CTFRespawn : byte
        {

        }

        public enum SuddenDeathSeconds : short
        {
            //NO_LIMIT = -1
        }

        public enum FlagReturnTime : short
        {
            //DISABLED = -1
        }

        public enum HomeFlagWaypoint : byte
        {
            //DISABLED = -1
        }
    }
}
