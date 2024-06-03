using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks.GameEngineVariants;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV2
{
    public class PackedSlayerGameVariant2 : PackedBaseGameVariant2
    {
        public byte teamScoring;

        public short scoreToWin;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points killPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points assistPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points deathPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points suicidePoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points betrayalPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points leaderKilledPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points eliminationPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points assassinationPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points headshotPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points meleePoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points stickyPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points splatterPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points killingSpreePoints;

        public PlayerTraits leaderTraits;

        public PlayerTraits leadingTeamTraits;

        public PackedSlayerGameVariant2()
        {
        }

        public PackedSlayerGameVariant2(ref BitStream<StreamByteStream> hoppersStream)
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
            hoppersStream.WriteBitswapped(teamScoring, 2);
            hoppersStream.WriteBitswapped(scoreToWin, 10);
            hoppersStream.WriteBitswapped((byte)killPoints, 5);
            hoppersStream.WriteBitswapped((byte)assistPoints, 5);
            hoppersStream.WriteBitswapped((byte)deathPoints, 5);
            hoppersStream.WriteBitswapped((byte)suicidePoints, 5);
            hoppersStream.WriteBitswapped((byte)betrayalPoints, 5);
            hoppersStream.WriteBitswapped((byte)leaderKilledPoints, 5);
            hoppersStream.WriteBitswapped((byte)eliminationPoints, 5);
            hoppersStream.WriteBitswapped((byte)assassinationPoints, 5);
            hoppersStream.WriteBitswapped((byte)headshotPoints, 5);
            hoppersStream.WriteBitswapped((byte)meleePoints, 5);
            hoppersStream.WriteBitswapped((byte)stickyPoints, 5);
            hoppersStream.WriteBitswapped((byte)splatterPoints, 5);
            hoppersStream.WriteBitswapped((byte)killingSpreePoints, 5);
            leaderTraits.Inherit(mapOverrides.baseTraits);
            leaderTraits.Write(ref hoppersStream);
            leadingTeamTraits.Inherit(mapOverrides.baseTraits);
            leadingTeamTraits.Write(ref hoppersStream);
        }
    }

}
