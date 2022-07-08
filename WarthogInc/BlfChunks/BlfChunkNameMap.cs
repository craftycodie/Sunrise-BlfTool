using Sunrise.BlfTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarthogInc.BlfChunks;

namespace SunriseBlfTool.BlfChunks
{
    public class BlfChunkNameMap
    {
        public IBLFChunk GetChunk(string chunkName)
        {
            switch (chunkName)
            {
                case "_blf":
                    return new StartOfFile();
                case "_eof":
                    return new EndOfFile();
                case "mhcf":
                    return new HopperConfigurationTable();
                case "mhdf":
                    return new MatchmakingHopperDescriptions();
                case "fupd":
                    return new fupd();
                case "motd":
                    return new MessageOfTheDay();
                case "athr":
                    return new Author();
                case "srid":
                    return new ServiceRecordIdentity();
                case "mtdp":
                    return new MessageOfTheDayPopup();
                case "nagm":
                    return new NagMessage();
                case "gset":
                    return new GameSet();
                case "onfm":
                    return new Manifest();
                case "mmtp":
                    return new MatchmakingTips();
                case "bhms":
                    return new MatchmakingBanhammerMessages();
                case "mapm":
                    return new MapManifest();
                case "mmhs":
                    return new MatchmakingHopperStatistics();
                case "netc":
                    return new NetworkConfiguration();
                case "gvar":
                    return new PackedGameVariant();
                case "mvar":
                    return new PackedMapVariant();
                case "fubh":
                case "funs":
                case "fcma":
                case "fpre":
                case "dlcd":
                default:
                    throw new KeyNotFoundException("Chunk not found.");
            }
        }
    }
}
