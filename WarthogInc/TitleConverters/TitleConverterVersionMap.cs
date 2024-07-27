using SunriseBlfTool.TitleConverters;
using SunriseBlfTool.TitleConverters.Halo3;
using SunriseBlfTool.TitleConverters.Halo3ODST;
using SunriseBlfTool.TitleConverters.HaloOnline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.TitleConverters
{
    public class TitleConverterVersionMap
    {
        public static TitleConverterVersionMap singleton = new TitleConverterVersionMap();

        private Dictionary<string, Type> converters = new Dictionary<string, Type>();

        private TitleConverterVersionMap()
        {
            RegisterTitles();
        }

        private void RegisterTitles()
        {
            RegisterTitle<Halo3.TitleConverter_06481>();
            RegisterTitle<Halo3.TitleConverter_08117>();
            RegisterTitle<Halo3.TitleConverter_08172>();
            RegisterTitle<Halo3.TitleConverter_Untracked_090307_221540>();
            RegisterTitle<Halo3.TitleConverter_09699>();
            RegisterTitle<Halo3.TitleConverter_10015>();
            RegisterTitle<Halo3.TitleConverter_11637>();
            RegisterTitle<Halo3.TitleConverter_11729>();
            RegisterTitle<Halo3.TitleConverter_11855>();
            RegisterTitle<Halo3.TitleConverter_11856>();
            RegisterTitle<Halo3.TitleConverter_11902>();
            RegisterTitle<Halo3.TitleConverter_12065>();
            RegisterTitle<Halo3.TitleConverter_12070>();
            RegisterTitle<Halo3ODST.TitleConverter_13895>();
            RegisterTitle<HaloReach.TitleConverter_11860>();
            RegisterTitle<HaloReach.TitleConverter_12065>();
            RegisterTitle<HaloOnline.TitleConverter_106708>();
        }

        private void RegisterTitle<T>() where T : ITitleConverter, new()
        {
            converters.Add(new T().GetVersion(), typeof(T));
        }

        public ITitleConverter GetConverter(string version)
        {
            return (ITitleConverter)Activator.CreateInstance(converters[version]);
        }
    }

}
