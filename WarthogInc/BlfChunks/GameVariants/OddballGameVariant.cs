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
    public class OddballGameVariant : BaseGameVariant
    {
        public OddballGameVariant() { }

        public OddballGameVariant(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public bool autoBallPickup;
        public bool ballEffectEnabled; // no idea
        public short scoreToWin; // 11
        [JsonConverter(typeof(StringEnumConverter))]
        public Points carryingPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points killPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points ballKillPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points carrierKillPoints; // 5
        public byte ballCount; // 2
        public byte ballSpawnSeconds; // 7
        public byte ballRespawnSeconds; // 7
        public PlayerTraits ballCarrierTraits;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();

            base.Read(ref hoppersStream);
            autoBallPickup = hoppersStream.Read<byte>(1) > 0;
            ballEffectEnabled = hoppersStream.Read<byte>(1) > 0;
            scoreToWin = hoppersStream.Read<short>(11);
            carryingPoints = (Points)hoppersStream.Read<byte>(5);
            killPoints = (Points)hoppersStream.Read<byte>(5);
            ballKillPoints = (Points)hoppersStream.Read<byte>(5);
            carrierKillPoints = (Points)hoppersStream.Read<byte>(5);
            ballCount = hoppersStream.Read<byte>(2);
            ballSpawnSeconds = hoppersStream.Read<byte>(7);
            ballRespawnSeconds = hoppersStream.Read<byte>(7);
            ballCarrierTraits = new PlayerTraits(ref hoppersStream);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();

            base.Write(ref hoppersStream);
            hoppersStream.Write(autoBallPickup ? 1 : 0, 1);
            hoppersStream.Write(ballEffectEnabled ? 1 : 0, 1);
            hoppersStream.Write(scoreToWin, 11);
            hoppersStream.Write((byte)carryingPoints, 5);
            hoppersStream.Write((byte)killPoints, 5);
            hoppersStream.Write((byte)ballKillPoints, 5);
            hoppersStream.Write((byte)carrierKillPoints, 5);
            hoppersStream.Write(ballCount, 2);
            hoppersStream.Write(ballSpawnSeconds, 7);
            hoppersStream.Write(ballRespawnSeconds, 7);
            ballCarrierTraits.Write(ref hoppersStream);
        }
    }
}
