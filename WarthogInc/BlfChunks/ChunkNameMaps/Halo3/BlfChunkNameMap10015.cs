using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks.ChunkNameMaps.Halo3
{
    public class BlfChunkNameMap10015 : AbstractBlfChunkNameMap
    {
        public BlfChunkNameMap10015()
        {
            RegisterChunks();
        }

        private void RegisterChunks()
        {
            RegisterChunk<StartOfFile>();
            RegisterChunk<EndOfFile>();
            RegisterChunk<HopperConfigurationTable9>();
            RegisterChunk<MatchmakingHopperDescriptions2>();
            RegisterChunk<UserBanhammer>();
            RegisterChunk<Author>();
            RegisterChunk<ServiceRecordIdentity>();
            RegisterChunk<MessageOfTheDayPopup>();
            RegisterChunk<GameSet3>();
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
            return "10015";
        }
    }

}
