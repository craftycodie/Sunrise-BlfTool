using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks.GameEngineVariants;
using SunriseBlfTool.Extensions;


namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV2
{
    public class PackedTerritoriesGameVariant2 : PackedBaseGameVariant2
    {
        public enum SuddenDeathSeconds : short
        {

        }

        public bool oneSided;

        public bool lockAfterFirstCapture;

        public byte respawnOnCapture;

        public byte captureSeconds;

        [JsonConverter(typeof(StringEnumConverter))]
        public SuddenDeathSeconds suddenDeathSeconds;

        public PlayerTraits defenderTraits;

        public PlayerTraits attackerTraits;

        public PackedTerritoriesGameVariant2()
        {
        }

        public PackedTerritoriesGameVariant2(ref BitStream<StreamByteStream> hoppersStream)
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
            hoppersStream.WriteBitswapped(oneSided ? 1 : 0, 1);
            hoppersStream.WriteBitswapped(lockAfterFirstCapture ? 1 : 0, 1);
            hoppersStream.WriteBitswapped(respawnOnCapture, 2);
            hoppersStream.WriteBitswapped(captureSeconds, 7);
            hoppersStream.WriteBitswapped((short)suddenDeathSeconds, 10);
            defenderTraits.Write(ref hoppersStream);
            attackerTraits.Write(ref hoppersStream);
        }
    }

}
