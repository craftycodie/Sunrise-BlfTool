using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.Extensions;
using System;
using System.Text;

namespace SunriseBlfTool.BlfChunks
{
    public class MachineNetworkStatistics : IBLFChunk
    {
        public ulong unknown1;
        public ulong unknown2;
        public ulong unknown3;
        public ulong unknown4;
        public ulong unknown5;
        public ulong unknown6;
        public ulong unknown7;
        public ulong unknown8;
        public ulong unknown9;
        public ulong unknown10;
        public ulong unknown11;
        public ulong unknown12;
        public ulong unknown13;
        public ulong unknown14;
        public ulong unknown15;
        public ulong unknown16;
        public ulong unknown17;
        public ulong unknown18;
        public ulong unknown19;
        public ulong unknown20;
        public ulong unknown21;
        public ulong unknown22;
        public ulong unknown23;
        public ulong unknown24;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            return 0xC0;
        }

        public string GetName()
        {
            return "funs";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.WriteLong(unknown1, 64);
            hoppersStream.WriteLong(unknown2, 64);
            hoppersStream.WriteLong(unknown3, 64);
            hoppersStream.WriteLong(unknown4, 64);
            hoppersStream.WriteLong(unknown5, 64);
            hoppersStream.WriteLong(unknown6, 64);
            hoppersStream.WriteLong(unknown7, 64);
            hoppersStream.WriteLong(unknown8, 64);
            hoppersStream.WriteLong(unknown9, 64);
            hoppersStream.WriteLong(unknown10, 64);
            hoppersStream.WriteLong(unknown11, 64);
            hoppersStream.WriteLong(unknown12, 64);
            hoppersStream.WriteLong(unknown13, 64);
            hoppersStream.WriteLong(unknown14, 64);
            hoppersStream.WriteLong(unknown15, 64);
            hoppersStream.WriteLong(unknown16, 64);
            hoppersStream.WriteLong(unknown17, 64);
            hoppersStream.WriteLong(unknown18, 64);
            hoppersStream.WriteLong(unknown19, 64);
            hoppersStream.WriteLong(unknown20, 64);
            hoppersStream.WriteLong(unknown21, 64);
            hoppersStream.WriteLong(unknown22, 64);
            hoppersStream.WriteLong(unknown23, 64);
            hoppersStream.WriteLong(unknown24, 64);
        }
    }
}
