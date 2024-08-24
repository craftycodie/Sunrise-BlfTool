using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseBlfTool.BlfChunks
{
    class ContentHeader : IBLFChunk
    {
        public ushort GetAuthentication()
        {
            return 2;
        }

        public string GetName()
        {
            return "chdr";
        }

        public ushort GetVersion()
        {
            return 9;
        }

        public short buildNumber;
        public short mapVersion;
        public long uniqueId;
        public string filename;
        public string description;
        public string author;
        public int filetype;
        public bool authorXuidIsOnline;
        [JsonConverter(typeof(XUIDConverter))]
        public ulong authorXuid;
        public long size;
        public DateTime date;
        public int lengthSeconds;
        public int campaignId;
        public int mapId;
        public int gameEngineType;
        public int campaignDifficulty;
        public short hopperId;
        public long gameId;

        public uint GetLength()
        {
            return 0x108;
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(buildNumber, 16);
            hoppersStream.Write(mapVersion, 16);
            hoppersStream.WriteLong(uniqueId, 64);

            var filenameWideBytes = Encoding.Unicode.GetBytes(filename);

            for (int i = 0; i < 0x20; i++)
            {
                if (i < filenameWideBytes.Length)
                    hoppersStream.Write((byte)filenameWideBytes[i], 8);
                else
                    hoppersStream.Write(0, 8);
            }

            for (int i = 0; i < 0x80; i++)
            {
                if (i < description.Length)
                    hoppersStream.Write((byte)description[i], 8);
                else
                    hoppersStream.Write(0, 8);
            }

            for (int i = 0; i < 0x10; i++)
            {
                if (i < author.Length)
                    hoppersStream.Write((byte)author[i], 8);
                else
                    hoppersStream.Write(0, 8);
            }

            hoppersStream.Write(filetype, 32);
            hoppersStream.Write(authorXuidIsOnline ? (byte)1 : (byte)0, 8);
            hoppersStream.Write(0, 24);
            hoppersStream.WriteLong(authorXuid, 64);
            hoppersStream.WriteLong(size, 64);
            hoppersStream.WriteDate(date, 64);
            hoppersStream.Write(lengthSeconds, 32);
            hoppersStream.Write(campaignId, 32);
            hoppersStream.Write(mapId, 32);
            hoppersStream.Write(gameEngineType, 32);
            hoppersStream.Write(campaignDifficulty, 32);
            hoppersStream.Write(hopperId, 16);
            hoppersStream.Write(0, 16);
            hoppersStream.WriteLong(gameId, 64);
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            buildNumber = hoppersStream.Read<short>(16);
            mapVersion = hoppersStream.Read<short>(16);
            uniqueId = hoppersStream.Read<long>(64);

            var filenameBytes = new byte[0x20];
            var filenameLen = -1;
            var descriptionBytes = new byte[0x80];
            var descriptionLen = -1;
            var authorBytes = new byte[0x10];
            var authorLen = -1;

            for (int i = 0; i < filenameBytes.Length; i++)
            {
                filenameBytes[i] = hoppersStream.Read<byte>(8);
                if (filenameBytes[i] == 0 && filenameLen == -1)
                    filenameLen = i;
            }

            filename = Encoding.Unicode.GetString(filenameBytes).Substring(0, filenameLen);

            for (int i = 0; i < descriptionBytes.Length; i++)
            {
                descriptionBytes[i] = hoppersStream.Read<byte>(8);
                if (descriptionBytes[i] == 0 && descriptionLen == -1)
                    descriptionLen = i;
            }

            description = Encoding.UTF8.GetString(descriptionBytes).Substring(0, descriptionLen);

            for (int i = 0; i < authorBytes.Length; i++)
            {
                authorBytes[i] = hoppersStream.Read<byte>(8);
                if (authorBytes[i] == 0 && authorLen == -1)
                    authorLen = i;
            }

            author = Encoding.UTF8.GetString(authorBytes).Substring(0, authorLen);

            filetype = hoppersStream.Read<int>(32);
            authorXuidIsOnline = hoppersStream.Read<byte>(8) > 0;
            hoppersStream.SeekRelative(3);
            authorXuid = hoppersStream.Read<ulong>(64);
            size = hoppersStream.Read<long>(64);
            date = hoppersStream.ReadDate(64);
            lengthSeconds = hoppersStream.Read<int>(32);
            campaignId = hoppersStream.Read<int>(32);
            mapId = hoppersStream.Read<int>(32);
            gameEngineType = hoppersStream.Read<int>(32);
            campaignDifficulty = hoppersStream.Read<int>(32);
            hopperId = hoppersStream.Read<short>(16);
            hoppersStream.SeekRelative(2);
            gameId = hoppersStream.Read<long>(64);
        }
    }
}
