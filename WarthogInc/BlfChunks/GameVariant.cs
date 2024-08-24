using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks.GameEngineVariants;
using System;
using System.IO;
using SunriseBlfTool.BlfChunks;

namespace SunriseBlfTool
{
    public class GameVariant : IBLFChunk
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
        public SlayerGameVariant slayer;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CTFGameVariant captureTheFlag;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OddballGameVariant oddball;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AssaultGameVariant assault;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public InfectionGameVariant infection;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public KOTHGameVariant kingOfTheHill;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TerritoriesGameVariant territories;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public VIPGameVariant vip;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JuggernautGameVariant juggernaut;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForgeGameVariant forge;

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
            return "mpvr";
        }

        public ushort GetVersion()
        {
            return 3;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            throw new NotImplementedException();

            VariantGameEngine variantGameEngineIndex = (VariantGameEngine)hoppersStream.Read<byte>(4);
            switch (variantGameEngineIndex)
            {
                case VariantGameEngine.SLAYER:
                    slayer = new SlayerGameVariant(ref hoppersStream);
                    break;
                case VariantGameEngine.CTF:
                    captureTheFlag = new CTFGameVariant(ref hoppersStream);
                    break;
                case VariantGameEngine.ODDBALL:
                    oddball = new OddballGameVariant(ref hoppersStream);
                    break;
                case VariantGameEngine.ASSAULT:
                    assault = new AssaultGameVariant(ref hoppersStream);
                    break;
                case VariantGameEngine.INFECTION:
                    infection = new InfectionGameVariant(ref hoppersStream);
                    break;
                case VariantGameEngine.KOTH:
                    kingOfTheHill = new KOTHGameVariant(ref hoppersStream);
                    break;
                case VariantGameEngine.TERRITORIES:
                    territories = new TerritoriesGameVariant(ref hoppersStream);
                    break;
                case VariantGameEngine.VIP:
                    vip = new VIPGameVariant(ref hoppersStream);
                    break;
                case VariantGameEngine.JUGGERNAUT:
                    juggernaut = new JuggernautGameVariant(ref hoppersStream);
                    break;
                case VariantGameEngine.FORGE:
                    forge = new ForgeGameVariant(ref hoppersStream);
                    break;
                default:
                    throw new Exception("Unsupported game engine " + variantGameEngineIndex.ToString());
            }
            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();

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
