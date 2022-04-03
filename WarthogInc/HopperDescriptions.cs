using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc
{
    class HopperDescriptions
    {
        public byte descriptionCount;
        public HopperDescription[] descriptions;


        public class HopperDescription
        {
            public ushort identifier;
            public bool type;
            public string description;
        }
    }
}
