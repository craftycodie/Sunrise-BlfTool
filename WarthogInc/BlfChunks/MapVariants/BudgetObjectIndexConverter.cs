using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks.MapVariants
{
    class BudgetObjectIndexConverter
    {
        private readonly int mapID;

        private Dictionary<ObjectDatumRelativeIndexMap.RelativeIndex, uint> activeTable;

        private void LoadIndexTable()
        {
            var jsonStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Sunrise-BlfTool.ObjectTables.{mapID}.json");
            var sr = new StreamReader(jsonStream);
            ObjectDatumRelativeIndexMap indexMap = JsonConvert.DeserializeObject<ObjectDatumRelativeIndexMap>(sr.ReadToEnd());

            activeTable = new Dictionary<ObjectDatumRelativeIndexMap.RelativeIndex, uint>();

            foreach (KeyValuePair<string, ObjectDatumRelativeIndexMap.RelativeIndex> mapEntry in indexMap.objectDatumRelativeIndexMap)
            {
                activeTable.Add(mapEntry.Value, Convert.ToUInt32(mapEntry.Key, 16));
            } 
        }

        public BudgetObjectIndexConverter(int mapID)
        {
            this.mapID = mapID;
            LoadIndexTable();
        }

        public uint Get360ObjectIndex(short scnrGroup, short scnrIndex)
        {
            return activeTable[new ObjectDatumRelativeIndexMap.RelativeIndex(scnrGroup, scnrIndex)];
        }
    }
}
