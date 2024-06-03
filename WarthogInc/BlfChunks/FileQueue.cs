using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseBlfTool.BlfChunks;

namespace SunriseBlfTool
{
    public class FileQueue : IBLFChunk
    {
        public FileQueueTransfer[] transfers = new FileQueueTransfer[8];

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            return 8 * 80;
        }

        public string GetName()
        {
            return "filq";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            if (transfers.Length > 8)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too many bans! I can only write the first 8 :(");
                Console.ResetColor();
            }

            for (int i = 0; i < 8; i++)
            {
                FileQueueTransfer entry = i < transfers.Length ? transfers[i] : new FileQueueTransfer();

                hoppersStream.WriteLong(entry.playerXuid, 64);
                hoppersStream.Write(entry.slot, 32);
                hoppersStream.Write(entry.unknownC, 32);
                hoppersStream.WriteLong(entry.serverId, 64);

                byte[] messageBytes = Encoding.BigEndianUnicode.GetBytes("" + entry.fileName);
                int messageLength = messageBytes.Length;
                for (int j = 0; j < 32; j++)
                {
                    if (j < messageLength)
                    {
                        hoppersStream.Write(messageBytes[j], 8);
                    }
                    else
                    {
                        hoppersStream.Write(0, 8);
                    }
                }

                hoppersStream.Write(entry.fileType, 32);
                hoppersStream.Write(entry.unknown3C, 32);
                hoppersStream.Write(entry.mapId, 32);
                hoppersStream.Write(entry.unknown44, 32);
                hoppersStream.Write(entry.unknown48, 32);
                hoppersStream.Write(entry.sizeBytes, 32);
            }
        }

        public class FileQueueTransfer
        {
            [JsonConverter(typeof(XUIDConverter))]
            public ulong playerXuid;
            public int slot;
            public int unknownC;
            public long serverId;
            public string fileName;
            public int fileType;
            public int unknown3C;
            public int mapId;
            public int unknown44;
            public int unknown48;
            public int sizeBytes;
        }
    }
}
