using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks.GameEngineVariants;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV2
{
    public class PackedCTFGameVariant2 : PackedBaseGameVariant2
    {
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

        }

        public enum FlagReturnTime : short
        {

        }

        public enum HomeFlagWaypoint : byte
        {

        }

        public bool flagAtHomeToScore;

        [JsonConverter(typeof(StringEnumConverter))]
        public HomeFlagWaypoint homeFlagWaypoint;

        [JsonConverter(typeof(StringEnumConverter))]
        public CTFGameType flagCount;

        public CTFRespawn ctfRespawn;

        public byte scoreToWin;

        [JsonConverter(typeof(StringEnumConverter))]
        public SuddenDeathSeconds suddenDeathSeconds;

        public short flagResetSeconds;

        [JsonConverter(typeof(StringEnumConverter))]
        public FlagReturnTime flagReturnSeconds;

        public PlayerTraits flagCarrierTraits;

        public PackedCTFGameVariant2()
        {
        }

        public PackedCTFGameVariant2(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public new void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public new void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Write(ref hoppersStream);
            hoppersStream.WriteBitswapped(flagAtHomeToScore ? 1 : 0, 1);
            hoppersStream.WriteBitswapped((byte)homeFlagWaypoint, 2);
            hoppersStream.WriteBitswapped((byte)flagCount, 2);
            hoppersStream.WriteBitswapped((byte)ctfRespawn, 2);
            hoppersStream.WriteBitswapped(scoreToWin, 6);
            hoppersStream.WriteBitswapped((short)suddenDeathSeconds, 9);
            hoppersStream.WriteBitswapped(flagResetSeconds, 9);
            hoppersStream.WriteBitswapped((short)flagReturnSeconds, 6);
            flagCarrierTraits.Write(ref hoppersStream);
        }
    }

}
