using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarthogInc.BlfChunks;

namespace Sunrise.BlfTool
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
            //hoppersStream.Write<byte>(gameEntryCount, 6);

            //for (int i = 0; i < gameEntryCount; i++)
            //{
            //    HopperPopulation entry = gameEntries[i];
            //    //hoppersStream.Write<ushort>(entry.identifier, 16);
            //    //hoppersStream.Write<byte>(description.type ? (byte)1 : (byte)0, 1);
            //    //hoppersStream.WriteString(description.description, 256, Encoding.UTF8);
            //}

            //hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public class HopperPopulation
        {
            public uint hopperIdentifier;
            public uint playerCount;
        }
    }
}
