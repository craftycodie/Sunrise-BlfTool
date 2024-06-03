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
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool
{
    internal class MatchmakingHopperStatistics_PC : IBLFChunk
    {
        public class HopperPopulation
        {
            public uint hopperIdentifier;

            public uint playerCount;
        }

        public uint totalPlayers;

        public HopperPopulation[] statistics;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            return 260u;
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
                HopperPopulation hopperPopulation = new HopperPopulation();
                hopperPopulation.hopperIdentifier = hoppersStream.Read<uint>(32);
                hopperPopulation.playerCount = hoppersStream.Read<uint>(32);
                statistics[i] = hopperPopulation;
            }
            hoppersStream.Seek(hoppersStream.NextByteIndex, (byte)0);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write<uint>(totalPlayers, 32);
            for (int i = 0; i < 32; i++)
            {
                HopperPopulation hopperPopulation = statistics[i];
                hoppersStream.WriteBitswapped(hopperPopulation.hopperIdentifier, 32);
                hoppersStream.WriteBitswapped(hopperPopulation.playerCount, 32);
            }
        }
    }

}
