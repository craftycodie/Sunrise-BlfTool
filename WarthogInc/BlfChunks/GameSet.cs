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
    public class GameSet : IBLFChunk
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
            return (uint)ms.ByteOffset;
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
            int gameEntryCount = hoppersStream.Read<byte>(6);
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
                //hoppersStream.SeekRelative(-1);
                entry.gameVariantHash = new byte[20];
                for (int j = 0; j < 20; j++)
                    entry.gameVariantHash[j] = hoppersStream.Read<byte>(8);

                entry.mapVariantFileName = hoppersStream.ReadString(32);
                //hoppersStream.SeekRelative(-1);
                entry.mapVariantHash = new byte[20];
                for (int j = 0; j < 20; j++)
                    entry.mapVariantHash[j] = hoppersStream.Read<byte>(8);




                //byte[] unknown = new byte[200];
                //for (int j = 0; j < 200; j++)
                //    unknown[j] = hoppersStream.Read<byte>(8);

                //entry.gameVariantHash = new byte[32];
                //for (int j = 0; j < 32; j++)
                //{
                //    entry.gameVariantHash[j] = hoppersStream.Read<byte>(8);
                //    if (entry.gameVariantHash[j] == 0)
                //        break;
                //}

                //entry.gameVariantFileName = hoppersStream.ReadString(160);

                //entry.mapVariantHash = new byte[32];
                //for (int j = 0; j < 32; j++)
                //{
                //    entry.mapVariantHash[j] = hoppersStream.Read<byte>(8);
                //    if (entry.mapVariantHash[j] == 0)
                //        break;
                //}

                //entry.mapVariantFileName = hoppersStream.ReadString(160);

                //var tmp = Convert.ToHexString(unknown);
                gameEntries[i] = entry;
            }
            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write<byte>(gameEntryCount, 6);

            for (int i = 0; i < gameEntryCount; i++)
            {
                GameEntry entry = gameEntries[i];
                //hoppersStream.Write<ushort>(entry.identifier, 16);
                //hoppersStream.Write<byte>(description.type ? (byte)1 : (byte)0, 1);
                //hoppersStream.WriteString(description.description, 256, Encoding.UTF8);
            }

            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public class GameEntry
        {
            public int gameEntryWeight;
            public byte minimumPlayerCount;
            public bool skipAfterVeto;
            public bool optional; // not in the beta!
            public int mapID;
            [JsonConverter(typeof(HexStringConverter))]
            public byte[] gameVariantHash;
            public string gameVariantFileName;
            [JsonConverter(typeof(HexStringConverter))]
            public byte[] mapVariantHash;
            public string mapVariantFileName;
        }
    }
}
