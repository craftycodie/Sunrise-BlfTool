using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks.GameEngineVariants;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV2
{
    public class PackedVIPGameVariant2 : PackedBaseGameVariant2
    {
        public bool singleVip;

        public short scoreToWin;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points killPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points takedownPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points killAsVipPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points vipDeathPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points destinationArrivalPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points suicidePoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points vipSuicidePoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points betrayalPoints;

        public byte vipSelection;

        public byte activeZoneCount;

        public byte zoneMovement;

        public byte zoneOrder;

        public PlayerTraits vipTraits;

        public PlayerTraits vipProximityTraits;

        public PlayerTraits vipTeamTraits;

        public PackedVIPGameVariant2()
        {
        }

        public PackedVIPGameVariant2(ref BitStream<StreamByteStream> hoppersStream)
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
            hoppersStream.WriteBitswapped(singleVip ? 1 : 0, 1);
            hoppersStream.WriteBitswapped(scoreToWin, 10);
            hoppersStream.WriteBitswapped((byte)killPoints, 5);
            hoppersStream.WriteBitswapped((byte)takedownPoints, 5);
            hoppersStream.WriteBitswapped((byte)killAsVipPoints, 5);
            hoppersStream.WriteBitswapped((byte)vipDeathPoints, 5);
            hoppersStream.WriteBitswapped((byte)destinationArrivalPoints, 5);
            hoppersStream.WriteBitswapped((byte)suicidePoints, 5);
            hoppersStream.WriteBitswapped((byte)vipSuicidePoints, 5);
            hoppersStream.WriteBitswapped((byte)betrayalPoints, 5);
            hoppersStream.WriteBitswapped(vipSelection, 2);
            hoppersStream.WriteBitswapped(activeZoneCount, 2);
            hoppersStream.WriteBitswapped(zoneMovement, 4);
            hoppersStream.WriteBitswapped(zoneOrder, 1);
            vipTraits.Write(ref hoppersStream);
            vipProximityTraits.Write(ref hoppersStream);
            vipTeamTraits.Write(ref hoppersStream);
        }
    }

}
