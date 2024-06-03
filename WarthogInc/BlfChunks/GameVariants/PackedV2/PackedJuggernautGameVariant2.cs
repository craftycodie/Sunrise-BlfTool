using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks.GameEngineVariants;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV2
{
    public class PackedJuggernautGameVariant2 : PackedBaseGameVariant2
    {
        public bool alliedAgainstJuggernaut;

        public bool respawnOnLoneJuggernaut;

        public bool juggernautDestinationZonesEnabled;

        public short scoreToWin;

        public byte initialJuggernaut;

        public byte nextJuggernaut;

        public byte zoneMovement;

        public byte zoneOrder;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points killPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points juggernautKillPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points killAsJuggernautPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points destinationArrivalPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points suicidePoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points betrayalPoints;

        public byte juggernautDelay;

        public PlayerTraits juggernautTraits;

        public PackedJuggernautGameVariant2()
        {
        }

        public PackedJuggernautGameVariant2(ref BitStream<StreamByteStream> hoppersStream)
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
            hoppersStream.WriteBitswapped(alliedAgainstJuggernaut ? 1 : 0, 1);
            hoppersStream.WriteBitswapped(respawnOnLoneJuggernaut ? 1 : 0, 1);
            hoppersStream.WriteBitswapped(juggernautDestinationZonesEnabled ? 1 : 0, 1);
            hoppersStream.WriteBitswapped(scoreToWin, 9);
            hoppersStream.WriteBitswapped(initialJuggernaut, 2);
            hoppersStream.WriteBitswapped(nextJuggernaut, 2);
            hoppersStream.WriteBitswapped(zoneMovement, 4);
            hoppersStream.WriteBitswapped(zoneOrder, 1);
            hoppersStream.WriteBitswapped((byte)killPoints, 5);
            hoppersStream.WriteBitswapped((byte)juggernautKillPoints, 5);
            hoppersStream.WriteBitswapped((byte)killAsJuggernautPoints, 5);
            hoppersStream.WriteBitswapped((byte)destinationArrivalPoints, 5);
            hoppersStream.WriteBitswapped((byte)suicidePoints, 5);
            hoppersStream.WriteBitswapped((byte)betrayalPoints, 5);
            hoppersStream.WriteBitswapped(juggernautDelay, 4);
            juggernautTraits.Write(ref hoppersStream);
        }
    }

}
