using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.BlfTool.BlfChunks.GameEngineVariants
{
    public class PackedSlayerGameVariant10 : PackedBaseGameVariant10
    {
        public PackedSlayerGameVariant10() { }

        public PackedSlayerGameVariant10(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public short scoreToWin; // 10
        [JsonConverter(typeof(StringEnumConverter))]
        public Points killPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points assistPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points deathPoints; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points suicidePoints; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points betrayalPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points leaderKilledPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points eliminationPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points assassinationPoints; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points headshotPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points meleePoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points stickyPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points splatterPoints; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points killingSpreePoints; // 5
        public PlayerTraits leaderTraits;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Read(ref hoppersStream);
            scoreToWin = hoppersStream.Read<short>(10);
            killPoints = (Points)hoppersStream.Read<byte>(5);
            assistPoints = (Points)hoppersStream.Read<byte>(5);
            deathPoints = (Points)hoppersStream.Read<byte>(5);
            suicidePoints = (Points)hoppersStream.Read<byte>(5);
            betrayalPoints = (Points)hoppersStream.Read<byte>(5);
            leaderKilledPoints = (Points)hoppersStream.Read<byte>(5);
            eliminationPoints = (Points)hoppersStream.Read<byte>(5);
            assassinationPoints = (Points)hoppersStream.Read<byte>(5);
            headshotPoints = (Points)hoppersStream.Read<byte>(5);
            meleePoints = (Points)hoppersStream.Read<byte>(5);
            stickyPoints = (Points)hoppersStream.Read<byte>(5);
            splatterPoints = (Points)hoppersStream.Read<byte>(5);
            killingSpreePoints = (Points)hoppersStream.Read<byte>(5);
            leaderTraits = new PlayerTraits(ref hoppersStream);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Write(ref hoppersStream);
            hoppersStream.Write(scoreToWin, 10);
            hoppersStream.Write((byte)killPoints, 5);
            hoppersStream.Write((byte)assistPoints, 5);
            hoppersStream.Write((byte)deathPoints, 5);
            hoppersStream.Write((byte)suicidePoints, 5);
            hoppersStream.Write((byte)betrayalPoints, 5);
            hoppersStream.Write((byte)leaderKilledPoints, 5);
            hoppersStream.Write((byte)eliminationPoints, 5);
            hoppersStream.Write((byte)assassinationPoints, 5);
            hoppersStream.Write((byte)headshotPoints, 5);
            hoppersStream.Write((byte)meleePoints, 5);
            hoppersStream.Write((byte)stickyPoints, 5);
            hoppersStream.Write((byte)splatterPoints, 5);
            hoppersStream.Write((byte)killingSpreePoints, 5);
            leaderTraits.Write(ref hoppersStream);
        }
    }
}
