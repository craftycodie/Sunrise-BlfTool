using System;
using System.Collections.Generic;

namespace SunriseBlfTool.BlfChunks.ChunkNameMaps.Halo3
{
    public class BlfChunkNameMap13895 : AbstractBlfChunkNameMap
    {
        public static BlfChunkNameMap13895 singleton = new BlfChunkNameMap13895();

        public BlfChunkNameMap13895()
        {
            RegisterChunks();
        }

        private void RegisterChunks()
        {
            RegisterChunk<StartOfFile>();
            RegisterChunk<EndOfFile>();
            RegisterChunk<UserBanhammer>();
            RegisterChunk<Author>();
            ///RegisterChunk<ServiceRecordIdentity>(); // osri instead of srid.
            RegisterChunk<MessageOfTheDayPopup>();
            RegisterChunk<Manifest>();
            RegisterChunk<MatchmakingTips>();
            RegisterChunk<MatchmakingBanhammerMessages>();
            RegisterChunk<MapManifest>();
            //RegisterChunk<NetworkConfiguration>();
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
            return "13895";
        }
    }
}
