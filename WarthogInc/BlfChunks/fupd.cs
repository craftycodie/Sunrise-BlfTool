using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc.BlfChunks
{
    public class fupd : IBLFChunk
    {
        public int unknown0;
        public int bungieUserRole;
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

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(unknown0, 32);
            hoppersStream.Write(bungieUserRole, 32);
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
