using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.BlfTool.TitleConverters
{
    public interface ITitleConverter
    {
        public void ConvertBlfToJson(string blfFolder, string jsonFolder);
        public void ConvertJsonToBlf(string jsonFolder, string blfFolder);

        public string GetVersion();
    }
}
