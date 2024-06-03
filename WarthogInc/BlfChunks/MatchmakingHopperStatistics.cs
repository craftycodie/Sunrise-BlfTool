using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseBlfTool.BlfChunks;

namespace SunriseBlfTool
{
    class MatchmakingHopperStatistics : IBLFChunk
    {
        public uint totalPlayers;
        public HopperPopulation[] statistics;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            return 0x104;
        }

        public string GetName()
        {
            return "mmhs";
        }

        public ushort GetVersion()
        {
            return 3;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            totalPlayers = hoppersStream.Read<uint>(32);
            statistics = new HopperPopulation[32];
            for (int i = 0; i < 32; i++)
            {
                HopperPopulation entry = new HopperPopulation();

                entry.hopperIdentifier = hoppersStream.Read<uint>(32);
                entry.playerCount = hoppersStream.Read<uint>(32);

                statistics[i] = entry;
            }
            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write<uint>(totalPlayers, 32);
            for (int i = 0; i < 32; i++)
            {
                HopperPopulation entry = statistics[i];

                hoppersStream.Write<uint>(entry.hopperIdentifier, 32);
                hoppersStream.Write<uint>(entry.playerCount, 32);
            }
        }

        public class HopperPopulation
        {
            public uint hopperIdentifier;
            public uint playerCount;
        }
    }
}
