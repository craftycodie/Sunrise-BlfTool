using Sunrise.BlfTool;
using System;
using System.Collections.Generic;
using WarthogInc.BlfChunks;

namespace SunriseBlfTool.BlfChunks
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
            RegisterChunk<NagMessage>();
            RegisterChunk<GameSet6>();
            RegisterChunk<Manifest>();
            RegisterChunk<MatchmakingTips>();
            RegisterChunk<MatchmakingBanhammerMessages>();
            RegisterChunk<MapManifest>();
            RegisterChunk<MatchmakingHopperStatistics>();
            //RegisterChunk<NetworkConfiguration>();
            RegisterChunk<PackedGameVariant>();
            RegisterChunk<PackedMapVariant>();
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
            return "12070";
        }
    }
}
