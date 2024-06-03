using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV10
{
    public class PackedTerritoriesGameVariant10 : PackedBaseGameVariant10
    {
        public PackedTerritoriesGameVariant10() { }

        public PackedTerritoriesGameVariant10(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public bool oneSided;
        public bool lockAfterFirstCapture;
        public byte respawnOnCapture; // 2
        public byte captureSeconds; // 7
        [JsonConverter(typeof(StringEnumConverter))]
        public SuddenDeathSeconds suddenDeathSeconds; // 10
        public PlayerTraits defenderTraits;
        public PlayerTraits attackerTraits;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Read(ref hoppersStream);
            oneSided = hoppersStream.Read<byte>(1) > 0;
            lockAfterFirstCapture = hoppersStream.Read<byte>(1) > 0;
            respawnOnCapture = hoppersStream.Read<byte>(2);
            captureSeconds = hoppersStream.Read<byte>(7);
            suddenDeathSeconds = (SuddenDeathSeconds)hoppersStream.Read<byte>(10);
            defenderTraits = new PlayerTraits(ref hoppersStream);
            attackerTraits = new PlayerTraits(ref hoppersStream);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Write(ref hoppersStream);
            hoppersStream.Write(oneSided ? 1 : 0, 1);
            hoppersStream.Write(lockAfterFirstCapture ? 1 : 0, 1);
            hoppersStream.Write(respawnOnCapture, 2);
            hoppersStream.Write(captureSeconds, 7);
            hoppersStream.Write((short)suddenDeathSeconds, 10);
            defenderTraits.Write(ref hoppersStream);
            attackerTraits.Write(ref hoppersStream);
        }

        public enum SuddenDeathSeconds : short
        {
            //NO_LIMIT = -1
        }
    }
}
