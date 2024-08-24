using System;
using System.Collections.Generic;

namespace SunriseBlfTool.BlfChunks.ChunkNameMaps.Halo3
{
    public class BlfChunkNameMap12070 : AbstractBlfChunkNameMap
    {
        public static BlfChunkNameMap12070 singleton = new BlfChunkNameMap12070();

        public BlfChunkNameMap12070()
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
            RegisterChunk<MessageOfTheDayPopup>();
            RegisterChunk<GameSet6>();
            RegisterChunk<Manifest>();
            RegisterChunk<MatchmakingTips>();
            RegisterChunk<MatchmakingBanhammerMessages>();
            RegisterChunk<MapManifest>();
            RegisterChunk<MatchmakingHopperStatistics>();
            //RegisterChunk<NetworkConfiguration>();
            RegisterChunk<PackedGameVariant10>();
            RegisterChunk<PackedMapVariant>();
            RegisterChunk<ContentHeader>();
            RegisterChunk<FileQueue>();
            RegisterChunk<UserPlayerData>();
            RegisterChunk<RecentPlayers>();
            RegisterChunk<MachineNetworkStatistics>();
            RegisterChunk<MessageOfTheDay>();
            RegisterChunk<MultiplayerPlayers>();
            RegisterChunk<MultiplayerPlayerStatistics>();
            RegisterChunk<MultiplayerPlayerVsPlayerStatistics>();
            RegisterChunk<MultiplayerTeamStatistics>();
            RegisterChunk<MultiplayerTeams>();
            RegisterChunk<MatchmakingOptions>();
            RegisterChunk<Parent>();
        }

        public override string GetVersion()
        {
            return "12070";
        }
    }
}
