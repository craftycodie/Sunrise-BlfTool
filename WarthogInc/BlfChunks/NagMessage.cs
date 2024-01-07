using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc.BlfChunks
{
    public class NagMessage : IBLFChunk
    {
        public uint motdIdentifier;
        public uint acceptWaitMilliseconds;

        [JsonIgnore]
        public uint titleLength { get { return (uint)title.Length; } }
        public string title;

        [JsonIgnore]
        public uint headingLength { get { return (uint)heading.Length; } }
        public string heading;

        [JsonIgnore]
        public uint acceptLength { get { return (uint)accept.Length; } }
        public string accept;

        [JsonIgnore]
        public uint waitLength { get { return (uint)wait.Length; } }
        public string wait;

        [JsonIgnore]
        public uint bodyLength { get { return (uint)body.Length; } }
        public string body;

        public int unknown1;
        public int unknown2;
        public int unknown3;

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
            return "nagm";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            motdIdentifier = hoppersStream.Read<uint>(32);
            acceptWaitMilliseconds = hoppersStream.Read<uint>(32);

            uint titleLength = hoppersStream.Read<uint>(32);
            byte[] titleBytes = new byte[titleLength];
            for (int i = 0; i < 0x60; i++)
            {
                if (i < titleLength)
                    titleBytes[i] = hoppersStream.Read<byte>(8);
                else
                    hoppersStream.SeekRelative(1);
            }
            title = Encoding.BigEndianUnicode.GetString(titleBytes);

            uint headingLength = hoppersStream.Read<uint>(32);
            byte[] headingBytes = new byte[headingLength];
            for (int i = 0; i < 0x60; i++)
            {
                if (i < headingLength)
                    headingBytes[i] = hoppersStream.Read<byte>(8);
                else
                    hoppersStream.SeekRelative(1);
            }
            heading = Encoding.BigEndianUnicode.GetString(headingBytes);

            uint acceptLength = hoppersStream.Read<uint>(32);
            byte[] acceptBytes = new byte[acceptLength];
            for (int i = 0; i < 0x60; i++)
            {
                if (i < acceptLength)
                    acceptBytes[i] = hoppersStream.Read<byte>(8);
                else
                    hoppersStream.SeekRelative(1);
            }
            accept = Encoding.BigEndianUnicode.GetString(acceptBytes);

            uint waitLength = hoppersStream.Read<uint>(32);
            byte[] waitBytes = new byte[waitLength];
            for (int i = 0; i < 0x60; i++)
            {
                if (i < waitLength)
                    waitBytes[i] = hoppersStream.Read<byte>(8);
                else
                    hoppersStream.SeekRelative(1);
            }
            wait = Encoding.BigEndianUnicode.GetString(waitBytes);

            uint bodyLength = hoppersStream.Read<uint>(32);
            byte[] bodyBytes = new byte[bodyLength];
            for (int i = 0; i < 0x800; i++)
            {
                if (i < bodyLength)
                    bodyBytes[i] = hoppersStream.Read<byte>(8);
                else
                    hoppersStream.SeekRelative(1);
            }
            body = Encoding.BigEndianUnicode.GetString(bodyBytes);

            unknown1 = hoppersStream.Read<int>(32);
            unknown2 = hoppersStream.Read<int>(32);
            unknown3 = hoppersStream.Read<int>(32);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(motdIdentifier, 32);
            hoppersStream.Write(acceptWaitMilliseconds, 32);

            if (title.Length > 0x30) title = title.Substring(0, 0x30);
            if (heading.Length > 0x30) heading = heading.Substring(0, 0x30);
            if (accept.Length > 0x30) accept = accept.Substring(0, 0x30);
            if (wait.Length > 0x30) wait = wait.Substring(0, 0x30);
            if (body.Length > 0x400) body = body.Substring(0, 0x400);


            hoppersStream.Write(titleLength * 2, 32);
            hoppersStream.WriteString(title, (int)titleLength * 2, Encoding.BigEndianUnicode);
            hoppersStream.SeekRelative((int)(0x60 - (titleLength * 2)) - 1);

            hoppersStream.Write(headingLength * 2, 32);
            hoppersStream.WriteString(heading, (int)headingLength * 2, Encoding.BigEndianUnicode);
            hoppersStream.SeekRelative((int)(0x60 - (headingLength * 2)) - 1);

            hoppersStream.Write(acceptLength * 2, 32);
            hoppersStream.WriteString(accept, (int)acceptLength * 2, Encoding.BigEndianUnicode);
            hoppersStream.SeekRelative((int)(0x60 - (acceptLength * 2)) - 1);

            hoppersStream.Write(waitLength * 2, 32);
            hoppersStream.WriteString(wait, (int)waitLength * 2, Encoding.BigEndianUnicode);
            hoppersStream.SeekRelative((int)(0x60 - (waitLength * 2)) - 1);

            hoppersStream.Write(bodyLength * 2, 32);
            hoppersStream.WriteString(body, (int)bodyLength * 2, Encoding.BigEndianUnicode);
            hoppersStream.SeekRelative((int)(0x800 - (bodyLength * 2)) - 1);

            hoppersStream.Write(unknown1, 32);
            hoppersStream.Write(unknown2, 32);
            hoppersStream.Write(unknown3, 32);
        }
    }
}
