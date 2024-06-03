using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks.GameEngineVariants
{
    public class JuggernautGameVariant : BaseGameVariant
    {
        public JuggernautGameVariant() { }

        public JuggernautGameVariant(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public bool alliedAgainstJuggernaut;
        public bool respawnOnLoneJuggernaut;
        public bool juggernautDestinationZonesEnabled;
        public short scoreToWin; // 9
        public byte initialJuggernaut; // 2
        public byte nextJuggernaut; // 2
        public byte zoneMovement; // 4
        public byte zoneOrder; // 1
        [JsonConverter(typeof(StringEnumConverter))]
        public Points killPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points juggernautKillPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points killAsJuggernautPoints; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points destinationArrivalPoints; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points suicidePoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points betrayalPoints; // 5
        public byte juggernautDelay; // 4
        public PlayerTraits juggernautTraits;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();

            base.Read(ref hoppersStream);
            alliedAgainstJuggernaut = hoppersStream.Read<byte>(1) > 0;
            respawnOnLoneJuggernaut = hoppersStream.Read<byte>(1) > 0;
            juggernautDestinationZonesEnabled = hoppersStream.Read<byte>(1) > 0;
            scoreToWin = hoppersStream.Read<short>(9);
            initialJuggernaut = hoppersStream.Read<byte>(2);
            nextJuggernaut = hoppersStream.Read<byte>(2);
            zoneMovement = hoppersStream.Read<byte>(4);
            zoneOrder = hoppersStream.Read<byte>(1);
            killPoints = (Points)hoppersStream.Read<byte>(5);
            juggernautKillPoints = (Points)hoppersStream.Read<byte>(5);
            killAsJuggernautPoints = (Points)hoppersStream.Read<byte>(5);
            destinationArrivalPoints = (Points)hoppersStream.Read<byte>(5);
            suicidePoints = (Points)hoppersStream.Read<byte>(5);
            betrayalPoints = (Points)hoppersStream.Read<byte>(5);
            juggernautDelay = hoppersStream.Read<byte>(4);
            juggernautTraits = new PlayerTraits(ref hoppersStream);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();

            base.Write(ref hoppersStream);
            hoppersStream.Write(alliedAgainstJuggernaut ? 1 : 0, 1);
            hoppersStream.Write(respawnOnLoneJuggernaut ? 1 : 0, 1);
            hoppersStream.Write(juggernautDestinationZonesEnabled ? 1 : 0, 1);
            hoppersStream.Write(scoreToWin, 9);
            hoppersStream.Write(initialJuggernaut, 2);
            hoppersStream.Write(nextJuggernaut, 2);
            hoppersStream.Write(zoneMovement, 4);
            hoppersStream.Write(zoneOrder, 1);
            hoppersStream.Write((byte)killPoints, 5);
            hoppersStream.Write((byte)juggernautKillPoints, 5);
            hoppersStream.Write((byte)killAsJuggernautPoints, 5);
            hoppersStream.Write((byte)destinationArrivalPoints, 5);
            hoppersStream.Write((byte)suicidePoints, 5);
            hoppersStream.Write((byte)betrayalPoints, 5);
            hoppersStream.Write(juggernautDelay, 4);
            juggernautTraits.Write(ref hoppersStream);
        }
    }
}
