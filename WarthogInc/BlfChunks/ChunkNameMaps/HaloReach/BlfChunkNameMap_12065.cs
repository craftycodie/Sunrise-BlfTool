using System;
using System.Collections.Generic;

namespace SunriseBlfTool.BlfChunks.ChunkNameMaps.HaloReach
{
    public class BlfChunkNameMap_12065 : AbstractBlfChunkNameMap
    {
        public static BlfChunkNameMap_12065 singleton = new BlfChunkNameMap_12065();

        public BlfChunkNameMap_12065()
        {
            RegisterChunks();
        }

        private void RegisterChunks()
        {
            RegisterChunk<StartOfFile>();
            RegisterChunk<EndOfFile>();
            RegisterChunk<Author>();
            RegisterChunk<NagMessage>();
            RegisterChunk<Manifest>();
            RegisterChunk<MatchmakingTips>();
            RegisterChunk<MatchmakingBanhammerMessages>();
            RegisterChunk<MatchmakingHopperDescriptions3>();
            RegisterChunk<MapManifest>();
        }

        public override string GetVersion()
        {
            return "12065";
        }
    }
}
