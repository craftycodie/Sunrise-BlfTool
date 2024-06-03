using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks.GameEngineVariants;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV2
{
    public class PackedKOTHGameVariant2 : PackedBaseGameVariant2
    {
        public bool opaqueHill;

        public byte scoreToWin;

        public byte teamScoring;

        public byte movingHill;

        public byte movingHillOrder;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points insideHillPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points outsideHillPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points uncontestedHillBonus;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points kingKillPoints;

        public PlayerTraits hillTraits;

        public PackedKOTHGameVariant2()
        {
        }

        public PackedKOTHGameVariant2(ref BitStream<StreamByteStream> hoppersStream)
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
            hoppersStream.WriteBitswapped(opaqueHill ? 1 : 0, 1);
            hoppersStream.WriteBitswapped(scoreToWin, 4);
            hoppersStream.WriteBitswapped(teamScoring, 3);
            hoppersStream.WriteBitswapped(movingHill, 4);
            hoppersStream.WriteBitswapped(movingHillOrder, 2);
            hoppersStream.WriteBitswapped((byte)insideHillPoints, 5);
            hoppersStream.WriteBitswapped((byte)outsideHillPoints, 5);
            hoppersStream.WriteBitswapped((byte)uncontestedHillBonus, 5);
            hoppersStream.WriteBitswapped((byte)kingKillPoints, 5);
            hillTraits.Write(ref hoppersStream);
        }
    }
}
