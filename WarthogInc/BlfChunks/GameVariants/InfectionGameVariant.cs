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
    public class InfectionGameVariant : BaseGameVariant
    {
        public InfectionGameVariant() { }

        public InfectionGameVariant(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public bool respawnOnHavenMode;
        public byte safeHavens; // 2
        public byte nextZombie; // 2
        public byte initialZombieCount; // 5
        public byte safeHavenMovementSeconds; // 7
        [JsonConverter(typeof(StringEnumConverter))]
        public Points zombieKillPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points infectionPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points safeHavenArrivalPoints; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points suicidePoints; // 5 
        [JsonConverter(typeof(StringEnumConverter))]
        public Points betrayalPoints; // 5
        [JsonConverter(typeof(StringEnumConverter))]
        public Points lastManBonusPoints; // 5
        public PlayerTraits zombieTraits;
        public PlayerTraits alphaZombieTraits;
        public PlayerTraits lastManTraits;
        public PlayerTraits havenTraits;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();

            base.Read(ref hoppersStream);
            respawnOnHavenMode = hoppersStream.Read<byte>(1) > 0;
            safeHavens = hoppersStream.Read<byte>(2);
            nextZombie = hoppersStream.Read<byte>(2);
            initialZombieCount = hoppersStream.Read<byte>(5);
            safeHavenMovementSeconds = hoppersStream.Read<byte>(7);
            zombieKillPoints = (Points)hoppersStream.Read<byte>(5);
            infectionPoints = (Points)hoppersStream.Read<byte>(5);
            safeHavenArrivalPoints = (Points)hoppersStream.Read<byte>(5);
            suicidePoints = (Points)hoppersStream.Read<byte>(5);
            betrayalPoints = (Points)hoppersStream.Read<byte>(5);
            lastManBonusPoints = (Points)hoppersStream.Read<byte>(5);
            zombieTraits = new PlayerTraits(ref hoppersStream);
            alphaZombieTraits = new PlayerTraits(ref hoppersStream);
            lastManTraits = new PlayerTraits(ref hoppersStream);
            havenTraits = new PlayerTraits(ref hoppersStream);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();

            base.Write(ref hoppersStream);
            hoppersStream.Write(respawnOnHavenMode ? 1 : 0, 1);
            hoppersStream.Write(safeHavens, 2);
            hoppersStream.Write(nextZombie, 2);
            hoppersStream.Write(initialZombieCount, 5);
            hoppersStream.Write(safeHavenMovementSeconds, 7);
            hoppersStream.Write((byte)zombieKillPoints, 5);
            hoppersStream.Write((byte)infectionPoints, 5);
            hoppersStream.Write((byte)safeHavenArrivalPoints, 5);
            hoppersStream.Write((byte)suicidePoints, 5);
            hoppersStream.Write((byte)betrayalPoints, 5);
            hoppersStream.Write((byte)lastManBonusPoints, 5);
            zombieTraits.Write(ref hoppersStream);
            alphaZombieTraits.Write(ref hoppersStream);
            lastManTraits.Write(ref hoppersStream);
            havenTraits.Write(ref hoppersStream);
        }
    }
}
