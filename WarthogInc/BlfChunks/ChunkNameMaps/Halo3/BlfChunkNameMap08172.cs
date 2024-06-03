using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks.ChunkNameMaps.Halo3
{
    public class BlfChunkNameMap08172 : AbstractBlfChunkNameMap
    {
        public static BlfChunkNameMap08172 singleton = new BlfChunkNameMap08172();

        public BlfChunkNameMap08172()
        {
            RegisterChunks();
        }

        private void RegisterChunks()
        {
            RegisterChunk<StartOfFile>();
            RegisterChunk<EndOfFile>();
            RegisterChunk<HopperConfigurationTable8>();
            RegisterChunk<MatchmakingHopperDescriptions2>();
            RegisterChunk<UserBanhammer>();
            RegisterChunk<Author>();
            RegisterChunk<ServiceRecordIdentity>();
            RegisterChunk<GameSet2>();
            RegisterChunk<Manifest>();
            RegisterChunk<MatchmakingTips>();
            RegisterChunk<MatchmakingBanhammerMessages>();
            RegisterChunk<MapManifest>();
            RegisterChunk<MatchmakingHopperStatistics>();
            RegisterChunk<ContentHeader>();
            RegisterChunk<FileQueue>();
            RegisterChunk<UserPlayerData>();
            RegisterChunk<RecentPlayers>();
            RegisterChunk<MachineNetworkStatistics>();
            RegisterChunk<MessageOfTheDay>();
            RegisterChunk<MultiplayerPlayers>();
        }

        public override string GetVersion()
        {
            return "08172";
        }
    }

}
