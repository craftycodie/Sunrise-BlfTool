using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks.GameVariants.PackedV10;
using System;
using System.IO;
using SunriseBlfTool.BlfChunks;

namespace SunriseBlfTool
{
    public class PackedGameVariant10 : IBLFChunk
    {
        public enum VariantGameEngine : byte {
            CTF = 1,
            SLAYER = 2,
            ODDBALL = 3,
            KOTH = 4,
            FORGE = 5,
            VIP = 6,
            JUGGERNAUT = 7,
            TERRITORIES = 8,
            ASSAULT = 9,
            INFECTION = 10,
        }

        [JsonIgnore]
        public VariantGameEngine variantGameEngineIndex { 
            get {
                if (slayer != null)
                    return VariantGameEngine.SLAYER;
                else if (captureTheFlag != null)
                    return VariantGameEngine.CTF;
                else if (oddball != null)
                    return VariantGameEngine.ODDBALL;
                else if (assault != null)
                    return VariantGameEngine.ASSAULT;
                else if (infection != null)
                    return VariantGameEngine.INFECTION;
                else if (kingOfTheHill != null)
                    return VariantGameEngine.KOTH;
                else if (territories != null)
                    return VariantGameEngine.TERRITORIES;
                else if (vip != null)
                    return VariantGameEngine.VIP;
                else if (juggernaut != null)
                    return VariantGameEngine.JUGGERNAUT;
                else if (forge != null)
                    return VariantGameEngine.FORGE;
                else
                    throw new Exception("No variant found.");
            } 
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedSlayerGameVariant10 slayer;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedCTFGameVariant10 captureTheFlag;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedOddballGameVariant10 oddball;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedAssaultGameVariant10 assault;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedInfectionGameVariant10 infection;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedKOTHGameVariant10 kingOfTheHill;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedTerritoriesGameVariant10 territories;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedVIPGameVariant10 vip;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedJuggernautGameVariant10 juggernaut;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PackedForgeGameVariant10 forge;

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
            return 10;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            VariantGameEngine variantGameEngineIndex = (VariantGameEngine)hoppersStream.Read<byte>(4);
            switch (variantGameEngineIndex)
            {
                case VariantGameEngine.SLAYER:
                    slayer = new PackedSlayerGameVariant10(ref hoppersStream);
                    break;
                case VariantGameEngine.CTF:
                    captureTheFlag = new PackedCTFGameVariant10(ref hoppersStream);
                    break;
                case VariantGameEngine.ODDBALL:
                    oddball = new PackedOddballGameVariant10(ref hoppersStream);
                    break;
                case VariantGameEngine.ASSAULT:
                    assault = new PackedAssaultGameVariant10(ref hoppersStream);
                    break;
                case VariantGameEngine.INFECTION:
                    infection = new PackedInfectionGameVariant10(ref hoppersStream);
                    break;
                case VariantGameEngine.KOTH:
                    kingOfTheHill = new PackedKOTHGameVariant10(ref hoppersStream);
                    break;
                case VariantGameEngine.TERRITORIES:
                    territories = new PackedTerritoriesGameVariant10(ref hoppersStream);
                    break;
                case VariantGameEngine.VIP:
                    vip = new PackedVIPGameVariant10(ref hoppersStream);
                    break;
                case VariantGameEngine.JUGGERNAUT:
                    juggernaut = new PackedJuggernautGameVariant10(ref hoppersStream);
                    break;
                case VariantGameEngine.FORGE:
                    forge = new PackedForgeGameVariant10(ref hoppersStream);
                    break;
                default:
                    throw new Exception("Unsupported game engine " + variantGameEngineIndex.ToString());
            }
            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write((byte)variantGameEngineIndex, 4);
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
                case VariantGameEngine.FORGE:
                    forge.Write(ref hoppersStream);
                    break;
                default:
                    throw new Exception("Unsupported game engine " + variantGameEngineIndex.ToString());
            }
        }
    }
}
