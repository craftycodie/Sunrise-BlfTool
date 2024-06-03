using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks.GameEngineVariants;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV2
{
    public class PackedInfectionGameVariant2 : PackedBaseGameVariant2
    {
        public bool respawnOnHavenMove;

        public byte safeHavens;

        public byte nextZombie;

        public byte initialZombieCount;

        public byte safeHavenMovementSeconds;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points zombieKillPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points infectionPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points safeHavenArrivalPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points suicidePoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points betrayalPoints;

        [JsonConverter(typeof(StringEnumConverter))]
        public Points lastManBonusPoints;

        public PlayerTraits zombieTraits;

        public PlayerTraits alphaZombieTraits;

        public PlayerTraits lastManTraits;

        public PlayerTraits havenTraits;

        public PackedInfectionGameVariant2()
        {
        }

        public PackedInfectionGameVariant2(ref BitStream<StreamByteStream> hoppersStream)
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
            hoppersStream.WriteBitswapped(respawnOnHavenMove ? 1 : 0, 1);
            hoppersStream.WriteBitswapped(safeHavens, 2);
            hoppersStream.WriteBitswapped(nextZombie, 2);
            hoppersStream.WriteBitswapped(initialZombieCount, 5);
            hoppersStream.WriteBitswapped(safeHavenMovementSeconds, 7);
            hoppersStream.WriteBitswapped((byte)zombieKillPoints, 5);
            hoppersStream.WriteBitswapped((byte)infectionPoints, 5);
            hoppersStream.WriteBitswapped((byte)safeHavenArrivalPoints, 5);
            hoppersStream.WriteBitswapped((byte)suicidePoints, 5);
            hoppersStream.WriteBitswapped((byte)betrayalPoints, 5);
            hoppersStream.WriteBitswapped((byte)lastManBonusPoints, 5);
            zombieTraits.Write(ref hoppersStream);
            alphaZombieTraits.Write(ref hoppersStream);
            lastManTraits.Write(ref hoppersStream);
            havenTraits.Write(ref hoppersStream);
        }
    }
}
