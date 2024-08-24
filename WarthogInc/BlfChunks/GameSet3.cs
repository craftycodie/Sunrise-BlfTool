using Sewer56.BitStream.ByteStreams;
using Sewer56.BitStream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool.BlfChunks
{
    public class GameSet3 : IBLFChunk
    {
        public class GameEntry
        {
            public int gameEntryWeight;

            public byte minimumPlayerCount;

            public bool skipAfterVeto;

            public int mapID;

            public string gameVariantFileName;

            [JsonIgnore]
            public byte[] gameVariantHash;
        }

        public GameEntry[] gameEntries;

        [JsonIgnore]
        public byte gameEntryCount => (byte)gameEntries.Length;

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
            return 3;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> stream)
        {
            var memoryStream = new MemoryStream();
            var hoppersStream = new BitStream<StreamByteStream>(new StreamByteStream(memoryStream));

            byte b = gameEntryCount;
            if (gameEntries.Length > 63)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too many game entries! I can only write the first 63 :(");
                Console.ResetColor();
                b = 63;
            }
            hoppersStream.WriteBitswapped(b, 6);
            for (int i = 0; i < b; i++)
            {
                GameEntry gameEntry = gameEntries[i];
                hoppersStream.WriteBitswapped(gameEntry.gameEntryWeight, 32);
                hoppersStream.WriteBitswapped(gameEntry.minimumPlayerCount, 4);
                hoppersStream.WriteBitswapped(gameEntry.skipAfterVeto ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(gameEntry.mapID, 32);
                hoppersStream.WriteBitswappedString(gameEntry.gameVariantFileName, 32, Encoding.UTF8);
                for (int j = 0; j < 20; j++)
                {
                    hoppersStream.WriteBitswapped(gameEntry.gameVariantHash[j], 8);
                }
                hoppersStream.WriteBitswappedString("", 32, Encoding.UTF8);
                for (int k = 0; k < 20; k++)
                {
                    hoppersStream.WriteBitswapped((byte)0, 8);
                }
            }
            if (hoppersStream.BitIndex % 8 != 0)
            {
                hoppersStream.WriteBitswapped((byte)0, 8 - hoppersStream.BitIndex % 8);
            }
            memoryStream.Seek(0L, SeekOrigin.Begin);
            while (memoryStream.Position < memoryStream.Length)
            {
                stream.WriteBitswapped((byte)memoryStream.ReadByte(), 8);
            }
        }
    }

}
