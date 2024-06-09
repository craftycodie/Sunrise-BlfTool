using SunriseBlfTool.TitleConverters;
using SunriseBlfTool;
using SunriseBlfTool.BlfChunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseBlfTool.BlfChunks.ChunkNameMaps;
using SunriseBlfTool.BlfChunks.ChunkNameMaps.Halo3;

namespace SunriseBlfTool.TitleConverters.Halo3
{
    public class TitleConverter_11856 : TitleConverter_11855
    {
        public virtual string GetVersion()
        {
            return "11856";
        }
    }
}
