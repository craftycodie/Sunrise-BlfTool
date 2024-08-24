using Newtonsoft.Json;
using Sewer56.BitStream.ByteStreams;
using Sewer56.BitStream;
using SunriseBlfTool.BlfChunks.GameVariants.PackedV2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool.BlfChunks
{
    public class PackedGameVariant2 : IBLFChunk
    {
        public enum VariantGameEngine : byte
        {
            CTF = 1,
            SLAYER,
            ODDBALL,
            KOTH,
            FORGE,
            VIP,
            JUGGERNAUT,
            TERRITORIES,
            ASSAULT,
            INFECTION
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedSlayerGameVariant2 slayer;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedCTFGameVariant2 captureTheFlag;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedOddballGameVariant2 oddball;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedAssaultGameVariant2 assault;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedInfectionGameVariant2 infection;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedKOTHGameVariant2 kingOfTheHill;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedTerritoriesGameVariant2 territories;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedVIPGameVariant2 vip;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedJuggernautGameVariant2 juggernaut;

        public byte descriptionIndex;

        [JsonIgnore]
        public VariantGameEngine variantGameEngineIndex
        {
            get
            {
                if (slayer != null)
                {
                    return VariantGameEngine.SLAYER;
                }
                if (captureTheFlag != null)
                {
                    return VariantGameEngine.CTF;
                }
                if (oddball != null)
                {
                    return VariantGameEngine.ODDBALL;
                }
                if (assault != null)
                {
                    return VariantGameEngine.ASSAULT;
                }
                if (infection != null)
                {
                    return VariantGameEngine.INFECTION;
                }
                if (kingOfTheHill != null)
                {
                    return VariantGameEngine.KOTH;
                }
                if (territories != null)
                {
                    return VariantGameEngine.TERRITORIES;
                }
                if (vip != null)
                {
                    return VariantGameEngine.VIP;
                }
                if (juggernaut != null)
                {
                    return VariantGameEngine.JUGGERNAUT;
                }
                throw new Exception("No variant found.");
            }
        }

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            var ms = new BitStream<StreamByteStream>(new StreamByteStream(new MemoryStream()));
            WriteChunk(ref ms);
            return (uint)ms.NextByteIndex;
        }

        public string GetName()
        {
            return "gvar";
        }

        public ushort GetVersion()
        {
            return 2;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> stream)
        {
            var memoryStream = new MemoryStream();
            var hoppersStream = new BitStream<StreamByteStream>(new StreamByteStream(memoryStream));

            hoppersStream.WriteBitswapped((byte)variantGameEngineIndex, 4);
            hoppersStream.WriteBitswapped(descriptionIndex, 8);
            switch (variantGameEngineIndex)
            {
                case VariantGameEngine.SLAYER:
                    slayer.Write(ref hoppersStream);
                    break;
                case VariantGameEngine.CTF:
                    captureTheFlag.Write(ref hoppersStream);
                    break;
                case VariantGameEngine.ODDBALL:
                    oddball.Write(ref hoppersStream);
                    break;
                case VariantGameEngine.ASSAULT:
                    assault.Write(ref hoppersStream);
                    break;
                case VariantGameEngine.INFECTION:
                    infection.Write(ref hoppersStream);
                    break;
                case VariantGameEngine.KOTH:
                    kingOfTheHill.Write(ref hoppersStream);
                    break;
                case VariantGameEngine.TERRITORIES:
                    territories.Write(ref hoppersStream);
                    break;
                case VariantGameEngine.VIP:
                    vip.Write(ref hoppersStream);
                    break;
                case VariantGameEngine.JUGGERNAUT:
                    juggernaut.Write(ref hoppersStream);
                    break;
                default:
                    throw new Exception("Unsupported game engine " + variantGameEngineIndex);
            }
            memoryStream.Seek(0L, SeekOrigin.Begin);
            while (memoryStream.Position < memoryStream.Length)
            {
                stream.WriteBitswapped((byte)memoryStream.ReadByte(), 8);
            }
        }
    }

}
