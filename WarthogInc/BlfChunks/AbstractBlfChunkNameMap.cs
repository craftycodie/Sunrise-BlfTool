using Sunrise.BlfTool;
using System;
using System.Collections.Generic;
using WarthogInc.BlfChunks;

namespace SunriseBlfTool.BlfChunks
{
    public abstract class AbstractBlfChunkNameMap
    {
        protected Dictionary<string, Type> chunkTypes = new Dictionary<string, Type>();

        protected void RegisterChunk<T>() where T : IBLFChunk, new()
        {
            chunkTypes.Add(new T().GetName(), typeof(T));
        }

        public IBLFChunk GetChunk(string chunkName)
        {
            return (IBLFChunk)Activator.CreateInstance(chunkTypes[chunkName]);
        }

        public abstract string GetVersion();
    }
}
