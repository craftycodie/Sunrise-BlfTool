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
using ZLibDotNet;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool
{
    public class GameSet15 : IBLFChunk
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
            return 15;
        }

        private BitStream<StreamByteStream> ReadCompressedHopperData(ref BitStream<StreamByteStream> hoppersStream)
        {
            ushort compressedHopperTableLength = (ushort)(hoppersStream.Read<ushort>(14) - 4);
            int decompressedHopperTableLength = hoppersStream.Read<int>(32);
            byte[] compressedHopperTable = new byte[compressedHopperTableLength];
            byte[] decompressedHopperTable = new byte[decompressedHopperTableLength];

            for (ushort j = 0; j < compressedHopperTableLength; j++)
                compressedHopperTable[j] = hoppersStream.Read<byte>(8);

            new ZLib().Uncompress(decompressedHopperTable, out decompressedHopperTableLength, compressedHopperTable, compressedHopperTableLength);

            MemoryStream ms = new MemoryStream(decompressedHopperTable);
            StreamByteStream sbs = new StreamByteStream(ms);

            return new BitStream<StreamByteStream>(sbs);
        }

        private void WriteCompressedHopperData(ref BitStream<StreamByteStream> hoppersStream, byte[] hopperData)
        {
            // save this for later...
            int chunkStartPosition = hoppersStream.ByteOffset;
            hoppersStream.Write(0, 14);
            int compressedHopperTableLength;
            hoppersStream.Write(hopperData.Length, 32);
            byte[] compressedHopperTable = new byte[hopperData.Length];
            new ZLib().Compress(compressedHopperTable, out compressedHopperTableLength, hopperData, hopperData.Length, 9);
            for (int i = 0; i < compressedHopperTableLength; i++)
            {
                hoppersStream.Write(compressedHopperTable[i], 8);
            }
            hoppersStream.Seek(chunkStartPosition);
            hoppersStream.Write(compressedHopperTableLength + 4, 14);
            hoppersStream.SeekRelative(compressedHopperTableLength + 4);
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            var decompressedStream = ReadCompressedHopperData(ref hoppersStream);

            int gameEntryCount = Math.Min(100, decompressedStream.Read<int>(32)); // Read the count of game sets, with a maximum of 100
            gameEntries = new GameEntry[gameEntryCount];

            for (int i = 0; i < gameEntryCount; i++)
            {
                GameEntry gameSet = new GameEntry();

                gameSet.dword0 = decompressedStream.Read<uint>(32);
                gameSet.dword4 = decompressedStream.Read<uint>(32);
                gameSet.dword8 = decompressedStream.Read<uint>(32);
                gameSet.dwordC = decompressedStream.Read<uint>(32);
                gameSet.dword10 = decompressedStream.Read<uint>(32);
                gameSet.dword14 = decompressedStream.Read<uint>(32);
                gameSet.dword18 = decompressedStream.Read<uint>(32);
                gameSet.dword1C = decompressedStream.Read<uint>(32);
                gameSet.dword20 = decompressedStream.Read<uint>(32);
                gameSet.dword24 = decompressedStream.Read<uint>(32);
                gameSet.dword28 = decompressedStream.Read<uint>(32);
                gameSet.gap2C = decompressedStream.Read<ushort>(16);
                gameSet.word2E = decompressedStream.Read<ushort>(16);
                gameSet.dword30 = decompressedStream.Read<uint>(32);
                gameSet.dword34 = decompressedStream.Read<uint>(32);
                gameSet.float38 = decompressedStream.ReadFloat(32);
                gameSet.float3C = decompressedStream.ReadFloat(32);
                gameSet.byte40 = decompressedStream.Read<byte>(8);
                gameSet.gap41 = decompressedStream.Read<byte>(8);
                gameSet.gap42 = decompressedStream.Read<byte>(8);
                gameSet.gap43 = decompressedStream.Read<byte>(8);
                gameSet.mapId = decompressedStream.Read<uint>(32);
                bool hasGameVariant = decompressedStream.Read<byte>(8) > 0;
                gameSet.gameName = decompressedStream.ReadString(16);
                decompressedStream.SeekRelative(16 - gameSet.gameName.Length - 1);
                gameSet.gameVariantFileName = decompressedStream.ReadString(32);
                decompressedStream.SeekRelative(32 - gameSet.gameVariantFileName.Length - 1);
                gameSet.gameVariantHash = new byte[20];
                for (int j = 0; j < 20; j++)
                    gameSet.gameVariantHash[j] = decompressedStream.Read<byte>(8);
                bool hasMapVariant = decompressedStream.Read<byte>(8) > 0;
                gameSet.mapName = decompressedStream.ReadString(16);
                decompressedStream.SeekRelative(16 - gameSet.mapName.Length - 1);
                gameSet.mapVariantFileName = decompressedStream.ReadString(32);
                decompressedStream.SeekRelative(32 - gameSet.mapVariantFileName.Length - 1);
                gameSet.mapVariantHash = new byte[20];
                for (int j = 0; j < 20; j++)
                    gameSet.mapVariantHash[j] = decompressedStream.Read<byte>(8);
                gameSet.unknown3 = decompressedStream.Read<ushort>(16);

                gameEntries[i] = gameSet;
            }
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            byte[] hopperConfigurationTableData = new byte[0xD404];
            MemoryStream ms = new MemoryStream(hopperConfigurationTableData);
            StreamByteStream sbs = new StreamByteStream(ms);
            BitStream<StreamByteStream> compressedStream = new BitStream<StreamByteStream>(sbs);

            int gameEntryCount = Math.Min(100, gameEntries.Length);
            compressedStream.Write(gameEntryCount, 32); // Write the count of game sets, with a maximum of 100

            for (int i = 0; i < gameEntryCount; i++)
            {
                GameEntry gameSet = gameEntries[i];

                compressedStream.Write(gameSet.dword0, 32);
                compressedStream.Write(gameSet.dword4, 32);
                compressedStream.Write(gameSet.dword8, 32);
                compressedStream.Write(gameSet.dwordC, 32);
                compressedStream.Write(gameSet.dword10, 32);
                compressedStream.Write(gameSet.dword14, 32);
                compressedStream.Write(gameSet.dword18, 32);
                compressedStream.Write(gameSet.dword1C, 32);
                compressedStream.Write(gameSet.dword20, 32);
                compressedStream.Write(gameSet.dword24, 32);
                compressedStream.Write(gameSet.dword28, 32);
                compressedStream.Write(gameSet.gap2C, 16);
                compressedStream.Write(gameSet.word2E, 16);
                compressedStream.Write(gameSet.dword30, 32);
                compressedStream.Write(gameSet.dword34, 32);
                compressedStream.WriteFloat(gameSet.float38, 32);
                compressedStream.WriteFloat(gameSet.float3C, 32);
                compressedStream.Write(gameSet.byte40, 8);
                compressedStream.Write(gameSet.gap41, 8);
                compressedStream.Write(gameSet.gap42, 8);
                compressedStream.Write(gameSet.gap43, 8);
                compressedStream.Write(gameSet.mapId, 32);
                compressedStream.Write(gameSet.hasGameVariant ? (byte)1 : (byte)0, 8);
                compressedStream.WriteString(gameSet.gameName, 16);
                compressedStream.SeekRelative(16 - gameSet.gameName.Length - 1);
                compressedStream.WriteString(gameSet.gameVariantFileName, 32);
                compressedStream.SeekRelative(32 - gameSet.gameVariantFileName.Length - 1);

                for (int j = 0; j < 20; j++)
                    compressedStream.Write(gameSet.gameVariantHash[j], 8);

                compressedStream.Write(gameSet.hasMapVariant ? (byte)1 : (byte)0, 8);
                compressedStream.WriteString(gameSet.mapName, 16);
                compressedStream.SeekRelative(16 - gameSet.mapName.Length - 1);
                compressedStream.WriteString(gameSet.mapVariantFileName, 32);
                compressedStream.SeekRelative(32 - gameSet.mapVariantFileName.Length - 1);

                for (int j = 0; j < 20; j++)
                    compressedStream.Write(gameSet.mapVariantHash[j], 8);

                compressedStream.Write(gameSet.unknown3, 16);
            }

            WriteCompressedHopperData(ref hoppersStream, ms.ToArray());
        }

        public class GameEntry
        {
            public uint dword0;
            public uint dword4;
            public uint dword8;
            public uint dwordC;
            public uint dword10;
            public uint dword14;
            public uint dword18;
            public uint dword1C;
            public uint dword20;
            public uint dword24;
            public uint dword28;
            public ushort gap2C;
            public ushort word2E;
            public uint dword30;
            public uint dword34;
            public float float38;
            public float float3C;
            public byte byte40;
            public byte gap41;
            public byte gap42;
            public byte gap43;
            public uint mapId;
            [JsonIgnore]
            public bool hasGameVariant { get { return gameVariantFileName != null && gameVariantFileName.Length > 0; } }
            [JsonIgnore]
            public bool hasMapVariant { get { return mapVariantFileName != null && mapVariantFileName.Length > 0; } }
            public string gameName;
            [JsonIgnore]
            //[JsonConverter(typeof(HexStringConverter))]
            public byte[] gameVariantHash;
            public string gameVariantFileName;
            public string mapName;
            [JsonIgnore]
            //[JsonConverter(typeof(HexStringConverter))]
            public byte[] mapVariantHash;
            public string mapVariantFileName;
            public ushort unknown3;
        }
    }
}
