using SunriseBlfTool.BlfChunks.ChunkNameMaps;
using SunriseBlfTool.BlfChunks;
using SunriseBlfTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BlfChunkNameMap08117 : AbstractBlfChunkNameMap
{
    public static BlfChunkNameMap08117 singleton = new BlfChunkNameMap08117();

    public BlfChunkNameMap08117()
    {
        RegisterChunks();
    }

    private void RegisterChunks()
    {
        RegisterChunk<StartOfFile>();
        RegisterChunk<EndOfFile>();
        RegisterChunk<HopperConfigurationTable8>();
        RegisterChunk<MatchmakingHopperDescriptions1>();
        RegisterChunk<UserBanhammer>();
        RegisterChunk<Author>();
        RegisterChunk<ServiceRecordIdentity>();
        RegisterChunk<GameSet1>();
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
        return "08117";
    }
}
