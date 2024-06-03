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

namespace SunriseBlfTool
{
    class Manifest : IBLFChunk
    {
        [JsonIgnore]
        public uint fileCount { get { return (uint)files.Length; } }
        public FileEntry[] files;

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            return (uint)(fileCount * 0x64) + 4;
        }

        public string GetName()
        {
            return "onfm";
        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void SetFileHash(string filePath, byte[] hash)
        {
            foreach(FileEntry file in files)
            {
                if (file.filePath == filePath)
                {
                    file.fileHash = hash;
                    return;
                }
            }
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            int fileCount = hoppersStream.Read<int>(32);
            files = new FileEntry[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                FileEntry entry = new FileEntry();

                byte[] fileNameBytes = new byte[0x50];
                int filePathLength = fileNameBytes.Length;
                for (int j = 0; j < fileNameBytes.Length; j++)
                {
                    byte filePathByte = hoppersStream.Read<byte>(8);
                    if (filePathByte == 0)
                    {
                        filePathLength = j;
                        hoppersStream.SeekRelative(fileNameBytes.Length - j - 1);
                        break;
                    } 
                    else
                    {
                        fileNameBytes[j] = filePathByte;
                    }
                }

                entry.filePath = Encoding.UTF8.GetString(fileNameBytes.Take(filePathLength).ToArray());

                entry.fileHash = new byte[20];
                for (int j = 0; j < 20; j++)
                    entry.fileHash[j] = hoppersStream.Read<byte>(8);

                files[i] = entry;
            }
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write(files.Length, 32);
            foreach (FileEntry file in files)
            {
                byte[] filePathBytes = Encoding.UTF8.GetBytes(file.filePath);
                for (int j = 0; j < 0x50; j++)
                {
                    if (j < filePathBytes.Length)
                    {
                        hoppersStream.Write(filePathBytes[j], 8);
                    }
                    else
                    {
                        hoppersStream.Write(0, 8);
                    }
                }

                foreach (byte hashByte in file.fileHash)
                {
                    hoppersStream.Write(hashByte, 8);
                }
            }
        }

        public class FileEntry
        {
            public string filePath;
            [JsonConverter(typeof(HexStringConverter))]
            public byte[] fileHash;
        }
    }
}
