using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Sunrise.BlfTool
{
    public class ObjectDatumRelativeIndexMap
    {
        public Dictionary<string, RelativeIndex> objectDatumRelativeIndexMap;
        public class RelativeIndex
        {
            public enum ObjectGroup : short
            {
                VEHICLES = 1,
                WEAPONS,
                EQUIPMENT,
                SCENERY,
                TELEPORTERS,
                GOALS,
                SPAWNS
            }

            public RelativeIndex()
            {

            }

            public RelativeIndex(short objectGroup, short objectIndex)
            {
                this.objectGroup = (ObjectGroup)objectGroup;
                this.objectIndex = objectIndex;
            }

            [JsonConverter(typeof(StringEnumConverter))]
            public ObjectGroup objectGroup;
            public short objectIndex;

            public override bool Equals(System.Object obj)
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                RelativeIndex p = obj as RelativeIndex;
                if ((System.Object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return (objectGroup == p.objectGroup) && (objectIndex == p.objectIndex);
            }

            public bool Equals(RelativeIndex p)
            {
                // If parameter is null return false:
                if ((object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return (objectGroup == p.objectGroup) && (objectIndex == p.objectIndex);
            }

            public override int GetHashCode()
            {
                return (short)objectGroup ^ objectIndex;
            }
        }
    }
}
