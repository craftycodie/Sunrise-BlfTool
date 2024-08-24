using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Text;

namespace SunriseBlfTool.BlfChunks
{
    public class UserPlayerData : IBLFChunk
    {
        [Flags]
        public enum BungieUserRole : int
        {
            None = 0,
            SeventhColumn = 1 << 0,
            Pro = 1 << 1,
            Bungie = 1 << 2,
            Recon = 1 << 3
        }

        public int hopperAccess;
        public BungieUserRole bungieUserRole;
        public int highestSkill;
        public string hopperDirectory; // 32 characters

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            return 0x2C;
        }

        public string GetName()
        {
            return "fupd";
        }

        public ushort GetVersion()
        {
            return 3;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(hopperAccess, 32);
            hoppersStream.Write((int)bungieUserRole, 32);
            hoppersStream.Write(highestSkill, 32);

            byte[] hopperDirectoryOut = Encoding.UTF8.GetBytes(hopperDirectory);

            for (int i = 0; i < 0x20; i++)
            {
                if (i < hopperDirectoryOut.Length)
                    hoppersStream.Write(hopperDirectoryOut[i], 8);
                else
                    hoppersStream.Write(0, 8);
            }
        }
    }
}
