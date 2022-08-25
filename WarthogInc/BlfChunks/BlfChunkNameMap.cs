using Sunrise.BlfTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WarthogInc.BlfChunks;

namespace SunriseBlfTool.BlfChunks
{
    public class BlfChunkNameMap
    {
        public static BlfChunkNameMap singleton = new BlfChunkNameMap();

        private Dictionary<string, Type> chunkTypes = new Dictionary<string, Type>();

        private BlfChunkNameMap()
        {
            RegisterChunks();
        }

        private void RegisterChunks()
        {
            RegisterChunk<StartOfFile>();
            RegisterChunk<EndOfFile>();
            RegisterChunk<HopperConfigurationTable>();
            RegisterChunk<MatchmakingHopperDescriptions>();
            RegisterChunk<UserBanhammer>();
            RegisterChunk<Author>();
            RegisterChunk<ServiceRecordIdentity>();
            RegisterChunk<MessageOfTheDayPopup>();
            RegisterChunk<NagMessage>();
            RegisterChunk<GameSet>();
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

        private void RegisterChunk<T>() where T : IBLFChunk, new()
        {
            chunkTypes.Add(new T().GetName(), typeof(T));
        }

        public IBLFChunk GetChunk(string chunkName)
        {
            return (IBLFChunk)Activator.CreateInstance(chunkTypes[chunkName]);
        }
    }
}
