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
    public class GameSet6 : IBLFChunk
    {
        [JsonIgnore]
        public byte gameEntryCount { get { return (byte)gameEntries.Length; } }
        public GameEntry[] gameEntries;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            var ms = new BitStream<StreamByteStream>(new StreamByteStream(new MemoryStream()));
            WriteChunk(ref ms);
            return (uint)ms.NextByteIndex;
        }

        public string GetName()
        {
            return "gset";
        }

        public ushort GetVersion()
        {
            return 6;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            byte gameEntryCount = hoppersStream.Read<byte>(6);
            gameEntries = new GameEntry[gameEntryCount];
            for (int i = 0; i < gameEntryCount; i++)
            {
                GameEntry entry = new GameEntry();

                entry.gameEntryWeight = hoppersStream.Read<int>(32);
                entry.minimumPlayerCount = hoppersStream.Read<byte>(4);
                entry.skipAfterVeto = hoppersStream.Read<byte>(1) > 0;
                entry.optional = hoppersStream.Read<byte>(1) > 0;
                entry.mapID = hoppersStream.Read<int>(32);

                entry.gameVariantFileName = hoppersStream.ReadString(32);
                entry.gameVariantHash = new byte[20];
                for (int j = 0; j < 20; j++)
                    entry.gameVariantHash[j] = hoppersStream.Read<byte>(8);

                entry.mapVariantFileName = hoppersStream.ReadString(32);
                entry.mapVariantHash = new byte[20];
                for (int j = 0; j < 20; j++)
                    entry.mapVariantHash[j] = hoppersStream.Read<byte>(8);

                gameEntries[i] = entry;
            }
            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            var count = gameEntryCount;

            if (gameEntries.Length > 63)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too many game entries! I can only write the first 63 :(");
                Console.ResetColor();
                count = 63;
            }

            hoppersStream.Write(count, 6);

            for (int i = 0; i < count; i++)
            {
                GameEntry entry = gameEntries[i];

                hoppersStream.Write(entry.gameEntryWeight, 32);
                hoppersStream.Write(entry.minimumPlayerCount, 4);
                hoppersStream.Write(entry.skipAfterVeto ? (byte) 1 : (byte) 0, 1);
                hoppersStream.Write(entry.optional ? (byte)1 : (byte)0, 1);
                hoppersStream.Write(entry.mapID, 32);

                hoppersStream.WriteString(entry.gameVariantFileName, 32, Encoding.UTF8);

                for (int j = 0; j < 20; j++)
                    hoppersStream.Write(entry.gameVariantHash[j], 8);

                hoppersStream.WriteString(entry.mapVariantFileName, 32, Encoding.UTF8);

                for (int j = 0; j < 20; j++)
                    hoppersStream.Write<byte>(entry.mapVariantHash[j], 8);
            }
        }

        public class GameEntry
        {
            public int gameEntryWeight;
            public byte minimumPlayerCount;
            public bool skipAfterVeto;
            public bool optional; // not in the beta!
            [JsonIgnore]
            public int mapID;
            [JsonIgnore]
            //[JsonConverter(typeof(HexStringConverter))]
            public byte[] gameVariantHash;
            public string gameVariantFileName;
            [JsonIgnore]
            //[JsonConverter(typeof(HexStringConverter))]
            public byte[] mapVariantHash;
            public string mapVariantFileName;
        }
    }
}
