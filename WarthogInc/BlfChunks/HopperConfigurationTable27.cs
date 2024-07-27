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
using SunriseBlfTool.Extensions;
using ZLibDotNet;

namespace SunriseBlfTool
{
    class HopperConfigurationTable27 : IBLFChunk
    {
        [JsonIgnore]
        public byte configurationsCount { get { return (byte)configurations.Length; } }
        [JsonIgnore]
        public byte categoryCount { get { return (byte)categories.Length; } }
        public HopperCategory[] categories;
        public HopperConfiguration[] configurations;

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
            return "mhcf";
        }

        public ushort GetVersion()
        {
            return 27;
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
            for(int i = 0; i < compressedHopperTableLength; i++)
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

            uint hopperCount = decompressedStream.Read<uint>(32);
            uint categoryCount = decompressedStream.Read<uint>(32);

            categories = new HopperCategory[categoryCount];

            for (int i = 0; i < categoryCount; i++)
            {
                HopperCategory category = new HopperCategory();

                category.unknown1 = decompressedStream.Read<ushort>(16);
                category.category_name = decompressedStream.ReadString(32);
                decompressedStream.SeekRelative(32 - category.category_name.Length - 1);
                category.unknown2 = decompressedStream.Read<ushort>(16);
                category.unknown_string = decompressedStream.ReadString(32);
                decompressedStream.SeekRelative(32 - category.unknown_string.Length - 1);


                categories[i] = category;
            }

            decompressedStream.Seek(0x448, 0);

            configurations = new HopperConfiguration[hopperCount];

            for (int i = 0; i < hopperCount; i++)
            {
                HopperConfiguration hopper = new HopperConfiguration();

                hopper.name = decompressedStream.ReadString(32);
                decompressedStream.SeekRelative(32 - hopper.name.Length - 1);
                hopper.gameSetHash = new byte[20];
                for (int j = 0; j < 20; j++)
                    hopper.gameSetHash[j] = decompressedStream.Read<byte>(8);
                hopper.identifier = decompressedStream.Read<ushort>(16);
                hopper.category = decompressedStream.Read<ushort>(16);
                hopper.gap38 = decompressedStream.Read<uint>(32);
                hopper.dword3C = decompressedStream.Read<uint>(32);
                hopper.dword40 = decompressedStream.Read<uint>(32);
                hopper.gap44 = decompressedStream.Read<uint>(32);
                hopper.startTime = decompressedStream.Read<ulong>(64);
                hopper.endTime = decompressedStream.Read<ulong>(64);
                hopper.dword58 = decompressedStream.Read<uint>(32);
                hopper.dword5C = decompressedStream.Read<uint>(32);
                hopper.dword60 = decompressedStream.Read<uint>(32);
                hopper.dword64 = decompressedStream.Read<uint>(32);
                hopper.dword68 = decompressedStream.Read<uint>(32);
                hopper.dword6C = decompressedStream.Read<uint>(32);
                hopper.dword70 = decompressedStream.Read<uint>(32);
                hopper.dword74 = decompressedStream.Read<uint>(32);
                hopper.dword78 = decompressedStream.Read<uint>(32);
                hopper.dword7C = decompressedStream.Read<uint>(32);
                hopper.hopper_access_bit = decompressedStream.Read<uint>(32);
                hopper.account_type_access = decompressedStream.Read<uint>(32);
                hopper.gap88_1 = decompressedStream.Read<uint>(32);
                hopper.gap88_2 = decompressedStream.Read<uint>(32);
                hopper.gap88_3 = decompressedStream.Read<uint>(32);
                hopper.gap88_4 = decompressedStream.Read<uint>(32);
                hopper.dword98 = decompressedStream.Read<uint>(32);
                hopper.dword9C = decompressedStream.Read<uint>(32);
                hopper.dwordA0 = decompressedStream.Read<uint>(32);
                hopper.dwordA4 = decompressedStream.Read<uint>(32);
                hopper.dwordA8 = decompressedStream.Read<uint>(32);
                hopper.dwordAC = decompressedStream.Read<uint>(32);
                hopper.gapB0_1 = decompressedStream.Read<uint>(32);
                hopper.gapB0_2 = decompressedStream.Read<uint>(32);
                hopper.gapB0_3 = decompressedStream.Read<uint>(32);
                hopper.dwordBC = decompressedStream.Read<uint>(32);
                hopper.dwordC0 = decompressedStream.Read<uint>(32);
                hopper.gapC4 = decompressedStream.Read<uint>(32);
                hopper.dwordC8 = decompressedStream.Read<uint>(32);
                hopper.post_match_countdown_time_seconds = decompressedStream.Read<uint>(32);
                hopper.gapD0 = decompressedStream.Read<uint>(32);
                hopper.dwordD4 = decompressedStream.Read<uint>(32);
                hopper.dwordD8 = decompressedStream.Read<uint>(32);
                hopper.dwordDC = decompressedStream.Read<uint>(32);
                hopper.gapE0 = decompressedStream.Read<uint>(32);
                hopper.dwordE4 = decompressedStream.Read<uint>(32);
                hopper.dwordE8 = decompressedStream.Read<uint>(32);
                hopper.dwordEC = decompressedStream.Read<uint>(32);
                hopper.gapF0_1 = decompressedStream.Read<uint>(32);
                hopper.gapF0_2 = decompressedStream.Read<uint>(32);
                hopper.gapF0_3 = decompressedStream.Read<uint>(32);
                hopper.gapF0_4 = decompressedStream.Read<uint>(32);
                hopper.gapF0_5 = decompressedStream.Read<uint>(32);
                hopper.dword104 = decompressedStream.Read<uint>(32);
                hopper.dword108 = decompressedStream.Read<uint>(32);
                hopper.gap10C = decompressedStream.Read<uint>(32);
                hopper.dword110 = decompressedStream.Read<uint>(32);
                hopper.dword114 = decompressedStream.Read<uint>(32);
                hopper.dword118 = decompressedStream.Read<uint>(32);
                hopper.dword11C = decompressedStream.Read<uint>(32);
                hopper.pre_match_voice_configuration = decompressedStream.Read<uint>(32);
                hopper.in_match_voice_configuration = decompressedStream.Read<uint>(32);
                hopper.post_match_voice_configuration = decompressedStream.Read<uint>(32);
                hopper.gap12C = decompressedStream.Read<uint>(32);
                hopper.dword130 = decompressedStream.Read<uint>(32);

                hopper.query_configurations = new HopperQueryConfiguration[4];
                for (int j =  0; j < 4; j++)
                {
                    HopperQueryConfiguration queryConfiguration = new HopperQueryConfiguration();
                    queryConfiguration.dword0 = decompressedStream.Read<uint>(32);
                    queryConfiguration.gap4 = decompressedStream.Read<uint>(32);
                    queryConfiguration.dword8 = decompressedStream.Read<uint>(32);
                    queryConfiguration.gapC = decompressedStream.Read<uint>(32);
                    queryConfiguration.dword10 = decompressedStream.Read<uint>(32);
                    queryConfiguration.gap14 = decompressedStream.Read<uint>(32);
                    queryConfiguration.dword18 = decompressedStream.Read<uint>(32);
                    queryConfiguration.dword1C = decompressedStream.Read<uint>(32);
                    queryConfiguration.dword20 = decompressedStream.Read<uint>(32);
                    queryConfiguration.gap24 = decompressedStream.Read<uint>(32);
                    queryConfiguration.dword28 = decompressedStream.Read<uint>(32);
                    queryConfiguration.dword2C = decompressedStream.Read<uint>(32);
                    queryConfiguration.dword30 = decompressedStream.Read<uint>(32);
                    queryConfiguration.unknown1 = decompressedStream.Read<uint>(32);

                    queryConfiguration.latency_desirability_configurations = new HopperQueryLatencyDesirabilityConfiguration[2];
                    for (int k = 0; k < 2; k++)
                    {
                        HopperQueryLatencyDesirabilityConfiguration queryLatencyDesirabilityConfiguration = new HopperQueryLatencyDesirabilityConfiguration();
                        queryLatencyDesirabilityConfiguration.unknown1 = decompressedStream.Read<uint>(32);
                        queryLatencyDesirabilityConfiguration.unknown2 = decompressedStream.Read<uint>(32);
                        queryLatencyDesirabilityConfiguration.unknown3 = decompressedStream.Read<uint>(32);
                        queryConfiguration.latency_desirability_configurations[k] = queryLatencyDesirabilityConfiguration;
                    }

                    queryConfiguration.unknown2 = new float[17];
                    for (int k = 0; k < 17; k++)
                    {
                        queryConfiguration.unknown2[k] = decompressedStream.ReadFloat(32);
                    }

                    hopper.query_configurations[j] = queryConfiguration;
                }

                hopper.unknown = decompressedStream.Read<uint>(32);
                hopper.dword388 = decompressedStream.Read<uint>(32);
                hopper.dword38C = decompressedStream.Read<uint>(32);
                hopper.dword390 = decompressedStream.Read<uint>(32);
                hopper.dword394 = decompressedStream.Read<uint>(32);
                hopper.dword398 = decompressedStream.Read<uint>(32);

                hopper.per_team_data = new PerTeamData[8];
                for (int j = 0; j < 8; j++)
                {
                    PerTeamData perTeamData = new PerTeamData();
                    perTeamData.unknown1 = decompressedStream.Read<uint>(32);
                    perTeamData.unknown2 = decompressedStream.Read<uint>(32);
                    perTeamData.unknown3 = decompressedStream.Read<uint>(32);
                    perTeamData.unknown4 = decompressedStream.Read<uint>(32);
                    hopper.per_team_data[j] = perTeamData;
                }

                hopper.dword41C = decompressedStream.Read<uint>(32);
                hopper.dword420 = decompressedStream.Read<uint>(32);
                hopper.dword424 = decompressedStream.Read<uint>(32);
                hopper.gap428 = decompressedStream.Read<uint>(32);
                hopper.dword42C = decompressedStream.Read<uint>(32);
                hopper.dword430 = decompressedStream.Read<uint>(32);

                hopper.jackpot_configurations = new JackpotConfiguration[3];
                for (int j = 0; j < 3; j++)
                {
                    JackpotConfiguration perTeamData = new JackpotConfiguration();
                    perTeamData.unknown1 = decompressedStream.Read<uint>(32);
                    perTeamData.unknown2 = decompressedStream.Read<uint>(32);
                    perTeamData.unknown3 = decompressedStream.Read<uint>(32);
                    hopper.jackpot_configurations[j] = perTeamData;
                }

                configurations[i] = hopper;
            }

            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> chunkStream)
        {
            byte[] hopperConfigurationTableData = new byte[0x8F48];
            MemoryStream ms = new MemoryStream(hopperConfigurationTableData);
            StreamByteStream sbs = new StreamByteStream(ms);
            BitStream<StreamByteStream> hopperStream = new BitStream<StreamByteStream>(sbs);

            hopperStream.Write((uint)configurations.Length, 32);
            hopperStream.Write((uint)categories.Length, 32);

            for (int i = 0; i < categories.Length; i++)
            {
                var category = categories[i];

                hopperStream.Write(category.unknown1, 16);
                hopperStream.WriteString(category.category_name, 32);
                hopperStream.SeekRelative(32 - category.category_name.Length - 1);
                hopperStream.Write(category.unknown2, 16);
                hopperStream.WriteString(category.unknown_string, 32);
                hopperStream.SeekRelative(32 - category.unknown_string.Length - 1);
            }

            hopperStream.Seek(0x448, 0);

            for (int i = 0; i < configurations.Length; i++)
            {
                var hopper = configurations[i];

                hopperStream.WriteString(hopper.name, 32);
                hopperStream.SeekRelative(32 - hopper.name.Length - 1);

                for (int j = 0; j < hopper.gameSetHash.Length; j++)
                    hopperStream.Write(hopper.gameSetHash[j], 8);

                hopperStream.Write(hopper.identifier, 16);
                hopperStream.Write(hopper.category, 16);
                hopperStream.Write(hopper.gap38, 32);
                hopperStream.Write(hopper.dword3C, 32);
                hopperStream.Write(hopper.dword40, 32);
                hopperStream.Write(hopper.gap44, 32);
                hopperStream.WriteLong(hopper.startTime, 64);
                hopperStream.WriteLong(hopper.endTime, 64);
                hopperStream.Write(hopper.dword58, 32);
                hopperStream.Write(hopper.dword5C, 32);
                hopperStream.Write(hopper.dword60, 32);
                hopperStream.Write(hopper.dword64, 32);
                hopperStream.Write(hopper.dword68, 32);
                hopperStream.Write(hopper.dword6C, 32);
                hopperStream.Write(hopper.dword70, 32);
                hopperStream.Write(hopper.dword74, 32);
                hopperStream.Write(hopper.dword78, 32);
                hopperStream.Write(hopper.dword7C, 32);
                hopperStream.Write(hopper.hopper_access_bit, 32);
                hopperStream.Write(hopper.account_type_access, 32);
                hopperStream.Write(hopper.gap88_1, 32);
                hopperStream.Write(hopper.gap88_2, 32);
                hopperStream.Write(hopper.gap88_3, 32);
                hopperStream.Write(hopper.gap88_4, 32);
                hopperStream.Write(hopper.dword98, 32);
                hopperStream.Write(hopper.dword9C, 32);
                hopperStream.Write(hopper.dwordA0, 32);
                hopperStream.Write(hopper.dwordA4, 32);
                hopperStream.Write(hopper.dwordA8, 32);
                hopperStream.Write(hopper.dwordAC, 32);
                hopperStream.Write(hopper.gapB0_1, 32);
                hopperStream.Write(hopper.gapB0_2, 32);
                hopperStream.Write(hopper.gapB0_3, 32);
                hopperStream.Write(hopper.dwordBC, 32);
                hopperStream.Write(hopper.dwordC0, 32);
                hopperStream.Write(hopper.gapC4, 32);
                hopperStream.Write(hopper.dwordC8, 32);
                hopperStream.Write(hopper.post_match_countdown_time_seconds, 32);
                hopperStream.Write(hopper.gapD0, 32);
                hopperStream.Write(hopper.dwordD4, 32);
                hopperStream.Write(hopper.dwordD8, 32);
                hopperStream.Write(hopper.dwordDC, 32);
                hopperStream.Write(hopper.gapE0, 32);
                hopperStream.Write(hopper.dwordE4, 32);
                hopperStream.Write(hopper.dwordE8, 32);
                hopperStream.Write(hopper.dwordEC, 32);
                hopperStream.Write(hopper.gapF0_1, 32);
                hopperStream.Write(hopper.gapF0_2, 32);
                hopperStream.Write(hopper.gapF0_3, 32);
                hopperStream.Write(hopper.gapF0_4, 32);
                hopperStream.Write(hopper.gapF0_5, 32);
                hopperStream.Write(hopper.dword104, 32);
                hopperStream.Write(hopper.dword108, 32);
                hopperStream.Write(hopper.gap10C, 32);
                hopperStream.Write(hopper.dword110, 32);
                hopperStream.Write(hopper.dword114, 32);
                hopperStream.Write(hopper.dword118, 32);
                hopperStream.Write(hopper.dword11C, 32);
                hopperStream.Write(hopper.pre_match_voice_configuration, 32);
                hopperStream.Write(hopper.in_match_voice_configuration, 32);
                hopperStream.Write(hopper.post_match_voice_configuration, 32);
                hopperStream.Write(hopper.gap12C, 32);
                hopperStream.Write(hopper.dword130, 32);

                for (int j = 0; j < hopper.query_configurations.Length; j++)
                {
                    var queryConfiguration = hopper.query_configurations[j];
                    hopperStream.Write(queryConfiguration.dword0, 32);
                    hopperStream.Write(queryConfiguration.gap4, 32);
                    hopperStream.Write(queryConfiguration.dword8, 32);
                    hopperStream.Write(queryConfiguration.gapC, 32);
                    hopperStream.Write(queryConfiguration.dword10, 32);
                    hopperStream.Write(queryConfiguration.gap14, 32);
                    hopperStream.Write(queryConfiguration.dword18, 32);
                    hopperStream.Write(queryConfiguration.dword1C, 32);
                    hopperStream.Write(queryConfiguration.dword20, 32);
                    hopperStream.Write(queryConfiguration.gap24, 32);
                    hopperStream.Write(queryConfiguration.dword28, 32);
                    hopperStream.Write(queryConfiguration.dword2C, 32);
                    hopperStream.Write(queryConfiguration.dword30, 32);
                    hopperStream.Write(queryConfiguration.unknown1, 32);

                    for (int k = 0; k < queryConfiguration.latency_desirability_configurations.Length; k++)
                    {
                        var queryLatencyDesirabilityConfiguration = queryConfiguration.latency_desirability_configurations[k];
                        hopperStream.Write(queryLatencyDesirabilityConfiguration.unknown1, 32);
                        hopperStream.Write(queryLatencyDesirabilityConfiguration.unknown2, 32);
                        hopperStream.Write(queryLatencyDesirabilityConfiguration.unknown3, 32);
                    }

                    for (int k = 0; k < queryConfiguration.unknown2.Length; k++)
                    {
                        hopperStream.WriteFloat(queryConfiguration.unknown2[k], 32);
                    }
                }

                hopperStream.Write(hopper.unknown, 32);
                hopperStream.Write(hopper.dword388, 32);
                hopperStream.Write(hopper.dword38C, 32);
                hopperStream.Write(hopper.dword390, 32);
                hopperStream.Write(hopper.dword394, 32);
                hopperStream.Write(hopper.dword398, 32);

                for (int j = 0; j < hopper.per_team_data.Length; j++)
                {
                    var perTeamData = hopper.per_team_data[j];
                    hopperStream.Write(perTeamData.unknown1, 32);
                    hopperStream.Write(perTeamData.unknown2, 32);
                    hopperStream.Write(perTeamData.unknown3, 32);
                    hopperStream.Write(perTeamData.unknown4, 32);
                }

                hopperStream.Write(hopper.dword41C, 32);
                hopperStream.Write(hopper.dword420, 32);
                hopperStream.Write(hopper.dword424, 32);
                hopperStream.Write(hopper.gap428, 32);
                hopperStream.Write(hopper.dword42C, 32);
                hopperStream.Write(hopper.dword430, 32);

                for (int j = 0; j < hopper.jackpot_configurations.Length; j++)
                {
                    var jackpotConfiguration = hopper.jackpot_configurations[j];
                    hopperStream.Write(jackpotConfiguration.unknown1, 32);
                    hopperStream.Write(jackpotConfiguration.unknown2, 32);
                    hopperStream.Write(jackpotConfiguration.unknown3, 32);
                }
            }


            WriteCompressedHopperData(ref chunkStream, ms.ToArray());
        }

        public class HopperCategory
        {
            public ushort unknown1;
            public string category_name;
            public ushort unknown2;
            public string unknown_string;
        }

        public class HopperQueryLatencyDesirabilityConfiguration
        {
            public uint unknown1;
            public uint unknown2;
            public uint unknown3;
        }

        public class PerTeamData
        {
            public uint unknown1;
            public uint unknown2;
            public uint unknown3;
            public uint unknown4;
        }

        public class JackpotConfiguration
        {
            public uint unknown1;
            public uint unknown2;
            public uint unknown3;
        }

        public class HopperQueryConfiguration
        {
            public uint dword0;
            public uint gap4;
            public uint dword8;
            public uint gapC;
            public uint dword10;
            public uint gap14;
            public uint dword18;
            public uint dword1C;
            public uint dword20;
            public uint gap24;
            public uint dword28;
            public uint dword2C;
            public uint dword30;
            public uint unknown1;
            public HopperQueryLatencyDesirabilityConfiguration[] latency_desirability_configurations; // 2
            public float[] unknown2; // 17
        }

        public class HopperConfiguration
        {
            public string name;
            [JsonIgnore]
            public byte[] gameSetHash;
            public ushort identifier;
            public ushort category;
            public uint gap38; // not sure on type.
            public uint dword3C;
            public uint dword40;
            public uint gap44; // not sure on type.
            public ulong startTime;
            public ulong endTime;
            public uint dword58;
            public uint dword5C;
            public uint dword60;
            public uint dword64;
            public uint dword68;
            public uint dword6C;
            public uint dword70;
            public uint dword74;
            public uint dword78;
            public uint dword7C;
            public uint hopper_access_bit;
            public uint account_type_access;
            public uint gap88_1; // not sure on type for any of these.
            public uint gap88_2;
            public uint gap88_3;
            public uint gap88_4;
            public uint dword98;
            public uint dword9C;
            public uint dwordA0;
            public uint dwordA4;
            public uint dwordA8;
            public uint dwordAC;
            public uint gapB0_1; // not sure on type for any of these.
            public uint gapB0_2;
            public uint gapB0_3;
            public uint dwordBC;
            public uint dwordC0;
            public uint gapC4; // not sure on type.
            public uint dwordC8;
            public uint post_match_countdown_time_seconds;
            public uint gapD0; // not sure on type.
            public uint dwordD4;
            public uint dwordD8;
            public uint dwordDC;
            public uint gapE0; // not sure on type.
            public uint dwordE4;
            public uint dwordE8;
            public uint dwordEC;
            public uint gapF0_1; // not sure on type for any of these.
            public uint gapF0_2;
            public uint gapF0_3;
            public uint gapF0_4;
            public uint gapF0_5;
            public uint dword104;
            public uint dword108;
            public uint gap10C;  // not sure on type.
            public uint dword110;
            public uint dword114;
            public uint dword118;
            public uint dword11C;
            public uint pre_match_voice_configuration;
            public uint in_match_voice_configuration;
            public uint post_match_voice_configuration;
            public uint gap12C;  // not sure on type.
            public uint dword130;
            public HopperQueryConfiguration[] query_configurations; // 4 of them
            public uint unknown;  // not sure on type.
            public uint dword388;
            public uint dword38C;
            public uint dword390;
            public uint dword394;
            public uint dword398;
            public PerTeamData[] per_team_data; // 8
            public uint dword41C;
            public uint dword420;
            public uint dword424;
            public uint gap428; // not sure on type.
            public uint dword42C;
            public uint dword430;
            public JackpotConfiguration[] jackpot_configurations; // 3
        }
    }
}
