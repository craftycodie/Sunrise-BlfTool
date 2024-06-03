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
    class Author : IBLFChunk
    {
        public ushort GetAuthentication()
        {
            return 1;
        }

        public string GetName()
        {
            return "athr";
        }

        public ushort GetVersion()
        {
            return 3;
        }

        public string buildName = "";
        public ulong buildNumber;
        public string shellVersion = "";
        public string unknown40 = "";

        public uint GetLength()
        {
            return 0x44;
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            for (int i = 0; i < 16; i++)
            {
                if (i < buildName.Length)
                    hoppersStream.Write((byte)buildName[i], 8);
                else
                    hoppersStream.Write(0, 8);
            }

            hoppersStream.Write(buildNumber, 64);

            for (int i = 0; i < 28; i++)
            {
                if (i < shellVersion.Length)
                    hoppersStream.Write((byte)shellVersion[i], 8);
                else
                    hoppersStream.Write(0, 8);
            }

            for (int i = 0; i < 16; i++)
            {
                if (i < unknown40.Length)
                    hoppersStream.Write((byte)unknown40[i], 8);
                else
                    hoppersStream.Write(0, 8);
            }
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            var buildNameBytes = new byte[16];
            var buildNameLen = -1;
            var shellVersionBytes = new byte[28];
            var shellVersionLen = -1;
            var unknown40Bytes = new byte[16];
            var unknown40Len = -1;

            

            for (int i = 0; i < buildNameBytes.Length; i++)
            {
                buildNameBytes[i] = hoppersStream.Read<byte>(8);
                if (buildNameBytes[i] == 0 && buildNameLen == -1)
                    buildNameLen = i;
            }

            if (buildNameLen == -1)
                buildNameLen = buildNameBytes.Length;

            buildName = Encoding.UTF8.GetString(buildNameBytes).Substring(0, buildNameLen);

            buildNumber = hoppersStream.Read<ulong>(64);

            for (int i = 0; i < shellVersionBytes.Length; i++)
            {
                shellVersionBytes[i] = hoppersStream.Read<byte>(8);
                if (shellVersionBytes[i] == 0 && shellVersionLen == -1)
                    shellVersionLen = i;
            }

            if (shellVersionLen == -1)
                shellVersionLen = shellVersionBytes.Length;

            shellVersion = Encoding.UTF8.GetString(shellVersionBytes).Substring(0, shellVersionLen);

            for (int i = 0; i < unknown40Bytes.Length; i++)
            {
                unknown40Bytes[i] = hoppersStream.Read<byte>(8);
                if (unknown40Bytes[i] == 0 && unknown40Len == -1)
                    unknown40Len = i;
            }

            if (unknown40Len == -1)
                unknown40Len = unknown40Bytes.Length;

            unknown40 = Encoding.UTF8.GetString(unknown40Bytes).Substring(0, unknown40Len);
        }
    }
}
