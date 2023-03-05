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
    public class PackedKOTHGameVariant : PackedBaseGameVariant10
    {
        public PackedKOTHGameVariant() { }

        public PackedKOTHGameVariant(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public bool opaqueHill;
        public byte scoreToWin; // 10
        public byte movingHill; // 4
        public byte movingHillOrder; // 2
        [JsonConverter(typeof(StringEnumConverter))]
        public Points insideHillPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points outsideHillPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points uncontestedHillBonus; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points kingKillPoints; // 5 
        public PlayerTraits hillTraits;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Read(ref hoppersStream);
            opaqueHill = hoppersStream.Read<byte>(1) > 0;
            scoreToWin = hoppersStream.Read<byte>(10);
            movingHill = hoppersStream.Read<byte>(4);
            movingHillOrder = hoppersStream.Read<byte>(2);
            insideHillPoints = (Points)hoppersStream.Read<byte>(5);
            outsideHillPoints = (Points)hoppersStream.Read<byte>(5);
            uncontestedHillBonus = (Points)hoppersStream.Read<byte>(5);
            kingKillPoints = (Points)hoppersStream.Read<byte>(5);
            hillTraits = new PlayerTraits(ref hoppersStream);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Write(ref hoppersStream);
            hoppersStream.Write(opaqueHill ? 1 : 0, 1);
            hoppersStream.Write(scoreToWin, 10);
            hoppersStream.Write(movingHill, 4);
            hoppersStream.Write(movingHillOrder, 2);
            hoppersStream.Write((byte)insideHillPoints, 5);
            hoppersStream.Write((byte)outsideHillPoints, 5);
            hoppersStream.Write((byte)uncontestedHillBonus, 5);
            hoppersStream.Write((byte)kingKillPoints, 5);
            hillTraits.Write(ref hoppersStream);
        }
    }
}
