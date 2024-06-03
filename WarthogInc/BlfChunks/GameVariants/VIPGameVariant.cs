using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;

namespace SunriseBlfTool.BlfChunks.GameEngineVariants
{
    public class VIPGameVariant : BaseGameVariant
    {
        public VIPGameVariant() { }

        public VIPGameVariant(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public bool singleVip;
        public bool zonesEnabled;
        public bool endRoundOnVipDeath;
        public short scoreToWin; // 10
        [JsonConverter(typeof(StringEnumConverter))]
        public Points killPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points takedownPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points killAsVipPoints; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points vipDeathPoints; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points destinationArrivalPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points suicidePoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points vipSuicidePoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points betrayalPoints; // 5
        public byte vipSelection; // 2
        public byte zoneMovement; // 4
        public byte zoneOrder; // 1
        public byte influenceRadius; // 6
        public PlayerTraits vipTraits;
        public PlayerTraits vipProximityTraits;
        public PlayerTraits vipTeamTraits;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();

            base.Read(ref hoppersStream);
            singleVip = hoppersStream.Read<byte>(1) > 0;
            zonesEnabled = hoppersStream.Read<byte>(1) > 0;
            endRoundOnVipDeath = hoppersStream.Read<byte>(1) > 0;
            scoreToWin = hoppersStream.Read<short>(10);
            killPoints = (Points)hoppersStream.Read<byte>(5);
            takedownPoints = (Points)hoppersStream.Read<byte>(5);
            killAsVipPoints = (Points)hoppersStream.Read<byte>(5);
            vipDeathPoints = (Points)hoppersStream.Read<byte>(5);
            destinationArrivalPoints = (Points)hoppersStream.Read<byte>(5);
            suicidePoints = (Points)hoppersStream.Read<byte>(5);
            vipSuicidePoints = (Points)hoppersStream.Read<byte>(5);
            betrayalPoints = (Points)hoppersStream.Read<byte>(5);
            vipSelection = hoppersStream.Read<byte>(2);
            zoneMovement = hoppersStream.Read<byte>(4);
            zoneOrder = hoppersStream.Read<byte>(1);
            influenceRadius = hoppersStream.Read<byte>(6);
            vipTraits = new PlayerTraits(ref hoppersStream);
            vipProximityTraits = new PlayerTraits(ref hoppersStream);
            vipTeamTraits = new PlayerTraits(ref hoppersStream);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();

            base.Write(ref hoppersStream);
            hoppersStream.Write(singleVip ? 1 : 0, 1);
            hoppersStream.Write(zonesEnabled ? 1 : 0, 1);
            hoppersStream.Write(endRoundOnVipDeath ? 1 : 0, 1);
            hoppersStream.Write(scoreToWin, 10);
            hoppersStream.Write((byte)killPoints, 5);
            hoppersStream.Write((byte)takedownPoints, 5);
            hoppersStream.Write((byte)killAsVipPoints, 5);
            hoppersStream.Write((byte)vipDeathPoints, 5);
            hoppersStream.Write((byte)destinationArrivalPoints, 5);
            hoppersStream.Write((byte)suicidePoints, 5);
            hoppersStream.Write((byte)vipSuicidePoints, 5);
            hoppersStream.Write((byte)betrayalPoints, 5);
            hoppersStream.Write(vipSelection, 2);
            hoppersStream.Write(zoneMovement, 4);
            hoppersStream.Write(zoneOrder, 1);
            hoppersStream.Write(influenceRadius, 6);
            vipTraits.Write(ref hoppersStream);
            vipProximityTraits.Write(ref hoppersStream);
            vipTeamTraits.Write(ref hoppersStream);
        }
    }
}
