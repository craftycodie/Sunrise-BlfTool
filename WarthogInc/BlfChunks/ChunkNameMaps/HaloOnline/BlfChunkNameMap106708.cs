using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks.ChunkNameMaps.HaloOnline
{
    public class BlfChunkNameMap106708 : AbstractBlfChunkNameMap
    {
        public BlfChunkNameMap106708()
        {
            RegisterChunks();
        }

        private void RegisterChunks()
        {
            RegisterChunk<StartOfFile>();
            RegisterChunk<EndOfFile>();
            RegisterChunk<HopperConfigurationTable11>();
            RegisterChunk<MatchmakingHopperDescriptions3>();
            RegisterChunk<UserBanhammer>();
            RegisterChunk<Author>();
            RegisterChunk<ServiceRecordIdentity>();
            RegisterChunk<MessageOfTheDayPopup_PC>();
            RegisterChunk<NagMessage>();
            RegisterChunk<GameSet6>();
            RegisterChunk<Manifest_PC>();
            RegisterChunk<MatchmakingTips>();
            RegisterChunk<MatchmakingBanhammerMessages>();
            RegisterChunk<MapManifest>();
            RegisterChunk<MatchmakingHopperStatistics_PC>();
            RegisterChunk<PackedGameVariant10>();
            RegisterChunk<PackedMapVariant>();
            RegisterChunk<ContentHeader>();
            RegisterChunk<FileQueue>();
            RegisterChunk<UserPlayerData>();
            RegisterChunk<RecentPlayers>();
            RegisterChunk<MachineNetworkStatistics>();
            RegisterChunk<MessageOfTheDay>();
            RegisterChunk<MultiplayerPlayers>();
            RegisterChunk<MatchmakingOptions>();
        }

        public override string GetVersion()
        {
            return "106708";
        }
    }

}
