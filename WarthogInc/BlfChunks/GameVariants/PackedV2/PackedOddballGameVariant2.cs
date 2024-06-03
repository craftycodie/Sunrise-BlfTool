using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks.GameEngineVariants;
using SunriseBlfTool.Extensions;


namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV2
{
    public class PackedOddballGameVariant2 : PackedBaseGameVariant2
    {
        public bool autoBallPickup;

        public bool ballEffectEnabled;

        public byte teamScoring;

        public byte oddballWaypoint;

        public short scoreToWin;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points carryingPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points killPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points ballKillPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points carrierKillPoints;

        public byte ballCount;

        public byte ballSpawnSeconds;

        public byte ballRespawnSeconds;

        public PlayerTraits ballCarrierTraits;

        public PackedOddballGameVariant2()
        {
        }

        public PackedOddballGameVariant2(ref BitStream<StreamByteStream> hoppersStream)
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
            hoppersStream.WriteBitswapped(autoBallPickup ? 1 : 0, 1);
            hoppersStream.WriteBitswapped(ballEffectEnabled ? 1 : 0, 1);
            hoppersStream.WriteBitswapped(teamScoring, 2);
            hoppersStream.WriteBitswapped(oddballWaypoint, 2);
            hoppersStream.WriteBitswapped(scoreToWin, 11);
            hoppersStream.WriteBitswapped((byte)carryingPoints, 5);
            hoppersStream.WriteBitswapped((byte)killPoints, 5);
            hoppersStream.WriteBitswapped((byte)ballKillPoints, 5);
            hoppersStream.WriteBitswapped((byte)carrierKillPoints, 5);
            hoppersStream.WriteBitswapped(ballCount, 2);
            hoppersStream.WriteBitswapped(ballSpawnSeconds, 7);
            hoppersStream.WriteBitswapped(ballRespawnSeconds, 7);
            ballCarrierTraits.Write(ref hoppersStream);
        }
    }

}
