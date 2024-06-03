using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks.ChunkNameMaps.Halo3
{
    public class BlfChunkNameMap06481 : AbstractBlfChunkNameMap
    {
        public BlfChunkNameMap06481()
        {
            RegisterChunks();
        }

        private void RegisterChunks()
        {
            RegisterChunk<StartOfFile>();
            RegisterChunk<EndOfFile>();
            RegisterChunk<HopperConfigurationTable2>();
            RegisterChunk<MatchmakingHopperDescriptions1>();
            RegisterChunk<Author>();
            RegisterChunk<GameSet1>();
            RegisterChunk<MatchmakingHopperStatistics>();
            RegisterChunk<PackedGameVariant2>();
        }

        public override string GetVersion()
        {
            return "06481";
        }
    }

}
