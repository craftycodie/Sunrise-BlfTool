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

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream, BLFChunkReader reader)
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
                hopper.identifier = decompressedStream.Read<ushort>(16); // 34

                hopper.category_identifier = decompressedStream.Read<ushort>(16); // 36
                hopper.category_index = decompressedStream.Read<byte>(8); // 38
                hopper.player_investment_category = decompressedStream.Read<byte>(8);
                hopper.pad3A = decompressedStream.Read<byte>(8);
                hopper.pad3B = decompressedStream.Read<byte>(8);
                hopper.image_index = decompressedStream.Read<uint>(32);
                hopper.xlast_index = decompressedStream.Read<uint>(32);
                hopper.equivalency_id = decompressedStream.Read<byte>(8); // 44
                hopper.pad45 = decompressedStream.Read<byte>(8);
                hopper.pad46 = decompressedStream.Read<byte>(8);
                hopper.pad47 = decompressedStream.Read<byte>(8);
                hopper.startTime = decompressedStream.Read<ulong>(64);
                hopper.endTime = decompressedStream.Read<ulong>(64);
                hopper.minimum_games_won = decompressedStream.Read<uint>(32);
                hopper.maximum_games_won = decompressedStream.Read<uint>(32);
                hopper.minimum_games_played = decompressedStream.Read<uint>(32);
                hopper.maximum_games_played = decompressedStream.Read<uint>(32);
                hopper.minimum_grade = decompressedStream.Read<uint>(32);
                hopper.maximum_grade = decompressedStream.Read<uint>(32);
                hopper.min_party_size = decompressedStream.Read<uint>(32);
                hopper.max_party_size = decompressedStream.Read<uint>(32);
                hopper.min_local_players = decompressedStream.Read<uint>(32);
                hopper.max_local_players = decompressedStream.Read<uint>(32);
                hopper.hopper_access_bit = decompressedStream.Read<uint>(32);
                hopper.account_type_access = decompressedStream.Read<uint>(32);
                hopper.require_all_party_members_meet_games_played_requirements = decompressedStream.Read<byte>(8);
                hopper.byte89 = decompressedStream.Read<byte>(8);
                hopper.require_all_party_members_meet_grade_requirements = decompressedStream.Read<byte>(8);
                hopper.require_all_party_members_meet_access_requirements = decompressedStream.Read<byte>(8);
                hopper.require_all_party_members_meet_live_account_access_requirements = decompressedStream.Read<byte>(8);
                hopper.hide_hopper_from_games_played_restricted_players = decompressedStream.Read<byte>(8);
                hopper.byte8E = decompressedStream.Read<byte>(8);
                hopper.hide_hopper_from_grade_restricted_players = decompressedStream.Read<byte>(8);
                hopper.hide_hopper_from_access_restricted_players = decompressedStream.Read<byte>(8);
                hopper.hide_hopper_from_live_account_access_restricted_players = decompressedStream.Read<byte>(8);
                hopper.hide_hopper_due_to_time_restriction = decompressedStream.Read<byte>(8);
                hopper.requires_hard_drive = decompressedStream.Read<byte>(8);
                hopper.requires_local_party = decompressedStream.Read<byte>(8);
                hopper.pad95 = decompressedStream.Read<byte>(8);
                hopper.pad96 = decompressedStream.Read<byte>(8);
                hopper.pad97 = decompressedStream.Read<byte>(8);
                hopper.dword98 = decompressedStream.Read<uint>(32);
                hopper.dword9C = decompressedStream.Read<uint>(32);
                hopper.dwordA0 = decompressedStream.Read<uint>(32);
                hopper.dwordA4 = decompressedStream.Read<uint>(32);
                hopper.dwordA8 = decompressedStream.Read<uint>(32);
                hopper.dwordAC = decompressedStream.Read<uint>(32);
                hopper.gapB0_1 = decompressedStream.Read<uint>(32);
                hopper.is_ranked = decompressedStream.Read<byte>(8);
                hopper.is_arbitrated = decompressedStream.Read<byte>(8);
                hopper.are_guests_allowed = decompressedStream.Read<byte>(8);
                hopper.are_opponents_visible = decompressedStream.Read<byte>(8);
                hopper.uses_arena_lsp_stats = decompressedStream.Read<byte>(8);
                hopper.gapB9 = decompressedStream.Read<byte>(8);
                hopper.gapBA = decompressedStream.Read<byte>(8);
                hopper.gapBB = decompressedStream.Read<byte>(8);
                hopper.dwordBC = decompressedStream.Read<uint>(32);
                hopper.dwordC0 = decompressedStream.Read<uint>(32);
                hopper.gapC4 = decompressedStream.Read<byte>(8);
                hopper.uses_high_score_leaderboard = decompressedStream.Read<byte>(8);
                hopper.gapC6 = decompressedStream.Read<byte>(8);
                hopper.gapC7 = decompressedStream.Read<byte>(8);
                hopper.posse_formation = decompressedStream.Read<uint>(32);
                hopper.post_match_countdown_time_seconds = decompressedStream.Read<uint>(32);
                hopper.require_hosts_on_multiple_teams = decompressedStream.Read<uint>(32);
                hopper.repeated_opponents_to_consider_for_penalty = decompressedStream.Read<uint>(32);
                hopper.repeated_opponents_skill_throttle_start = decompressedStream.Read<uint>(32);
                hopper.repeated_opponents_skill_throttle_stop = decompressedStream.Read<uint>(32);
                hopper.is_team_matching_enabled = decompressedStream.Read<uint>(32);
                hopper.gather_start_threshold_seconds = decompressedStream.Read<uint>(32);
                hopper.get_gather_start_game_early_seconds = decompressedStream.Read<uint>(32);
                hopper.get_gather_give_up_seconds = decompressedStream.Read<uint>(32);

                hopper.chance_of_gathering = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    hopper.chance_of_gathering[j] = decompressedStream.Read<byte>(8);
                }

                hopper.gapF0_5 = decompressedStream.Read<uint>(32);
                hopper.dword104 = decompressedStream.Read<uint>(32);
                hopper.dword108 = decompressedStream.Read<uint>(32);
                hopper.uses_ffa_scoring_for_leaderboard_writes = decompressedStream.Read<byte>(8);
                hopper.should_modify_skill_update_weight_with_game_quality = decompressedStream.Read<byte>(8);
                hopper.gap10E = decompressedStream.Read<byte>(8);
                hopper.gap10F = decompressedStream.Read<byte>(8);
                hopper.trueskill_sigma_multiplier = decompressedStream.ReadFloat(32);
                hopper.dword114 = decompressedStream.Read<uint>(32);
                hopper.trueskill_tau_dynamics_factor = decompressedStream.Read<uint>(32);
                hopper.trueskill_draw_probability = decompressedStream.Read<uint>(32);
                hopper.pre_match_voice_configuration = decompressedStream.Read<uint>(32);
                hopper.in_match_voice_configuration = decompressedStream.Read<uint>(32);
                hopper.post_match_voice_configuration = decompressedStream.Read<uint>(32);
                hopper.restrict_open_channel = decompressedStream.Read<uint>(32);
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

                hopper.games_game_type = decompressedStream.Read<uint>(32);
                hopper.maximum_player_count = decompressedStream.Read<uint>(32);
                hopper.maximum_player_count = decompressedStream.Read<uint>(32);
                hopper.ffa_model_override = decompressedStream.Read<uint>(32);
                hopper.minimum_team_count = decompressedStream.Read<uint>(32);
                hopper.maximum_team_count = decompressedStream.Read<uint>(32);

                hopper.per_team_data = new PerTeamData[8];
                for (int j = 0; j < 8; j++)
                {
                    PerTeamData perTeamData = new PerTeamData();
                    perTeamData.minimum_team_size = decompressedStream.Read<uint>(32);
                    perTeamData.maximum_team_size = decompressedStream.Read<uint>(32);
                    perTeamData.team_model_override = decompressedStream.Read<uint>(32);
                    perTeamData.team_allegiance = decompressedStream.Read<uint>(32);
                    hopper.per_team_data[j] = perTeamData;
                }

                hopper.maximum_team_imbalance = decompressedStream.Read<uint>(32);
                hopper.big_squad_size_threshold = decompressedStream.Read<uint>(32);
                hopper.dword424 = decompressedStream.Read<uint>(32);
                hopper.gap428 = decompressedStream.Read<uint>(32);
                hopper.undersized_party_split_permissions = decompressedStream.Read<uint>(32);
                hopper.jackpot_minimum_time_seconds = decompressedStream.Read<uint>(32);

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

            bool validConfigurationsCount = configurationsCount >= 0 && configurationsCount <= 32;

            if (!validConfigurationsCount)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Too many hopper configurations to convert! ${configurationsCount}/32");
                throw new InvalidDataException("Too many hopper configurations to convert!");
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
                hopperStream.Write(hopper.category_identifier, 16);
                hopperStream.Write(hopper.category_index, 8);
                hopperStream.Write(hopper.player_investment_category, 8);
                hopperStream.Write(hopper.pad3A, 8);
                hopperStream.Write(hopper.pad3B, 8);
                hopperStream.Write(hopper.image_index, 32);
                hopperStream.Write(hopper.xlast_index, 32);
                hopperStream.Write(hopper.equivalency_id, 8);
                hopperStream.Write(hopper.pad45, 8);
                hopperStream.Write(hopper.pad46, 8);
                hopperStream.Write(hopper.pad47, 8);
                hopperStream.WriteLong(hopper.startTime, 64);
                hopperStream.WriteLong(hopper.endTime, 64);
                hopperStream.Write(hopper.minimum_games_won, 32);
                hopperStream.Write(hopper.maximum_games_won, 32);
                hopperStream.Write(hopper.minimum_games_played, 32);
                hopperStream.Write(hopper.maximum_games_played, 32);
                hopperStream.Write(hopper.minimum_grade, 32);
                hopperStream.Write(hopper.maximum_grade, 32);
                hopperStream.Write(hopper.min_party_size, 32);
                hopperStream.Write(hopper.max_party_size, 32);
                hopperStream.Write(hopper.min_local_players, 32);
                hopperStream.Write(hopper.max_local_players, 32);
                hopperStream.Write(hopper.hopper_access_bit, 32);
                hopperStream.Write(hopper.account_type_access, 32);
                hopperStream.Write(hopper.require_all_party_members_meet_games_played_requirements, 8);
                hopperStream.Write(hopper.byte89, 8);
                hopperStream.Write(hopper.require_all_party_members_meet_grade_requirements, 8);
                hopperStream.Write(hopper.require_all_party_members_meet_access_requirements, 8);
                hopperStream.Write(hopper.require_all_party_members_meet_live_account_access_requirements, 8);
                hopperStream.Write(hopper.hide_hopper_from_games_played_restricted_players, 8);
                hopperStream.Write(hopper.byte8E, 8);
                hopperStream.Write(hopper.hide_hopper_from_grade_restricted_players, 8);
                hopperStream.Write(hopper.hide_hopper_from_access_restricted_players, 8);
                hopperStream.Write(hopper.hide_hopper_from_live_account_access_restricted_players, 8);
                hopperStream.Write(hopper.hide_hopper_due_to_time_restriction, 8);
                hopperStream.Write(hopper.requires_hard_drive, 8);
                hopperStream.Write(hopper.requires_local_party, 8);
                hopperStream.Write(hopper.pad95, 8);
                hopperStream.Write(hopper.pad96, 8);
                hopperStream.Write(hopper.pad97, 8);
                hopperStream.Write(hopper.dword98, 32);
                hopperStream.Write(hopper.dword9C, 32);
                hopperStream.Write(hopper.dwordA0, 32);
                hopperStream.Write(hopper.dwordA4, 32);
                hopperStream.Write(hopper.dwordA8, 32);
                hopperStream.Write(hopper.dwordAC, 32);
                hopperStream.Write(hopper.gapB0_1, 32);
                hopperStream.Write(hopper.is_ranked, 8);
                hopperStream.Write(hopper.is_arbitrated, 8);
                hopperStream.Write(hopper.are_guests_allowed, 8);
                hopperStream.Write(hopper.are_opponents_visible, 8);
                hopperStream.Write(hopper.uses_arena_lsp_stats, 8);
                hopperStream.Write(hopper.gapB9, 8);
                hopperStream.Write(hopper.gapBA, 8);
                hopperStream.Write(hopper.gapBB, 8);
                hopperStream.Write(hopper.dwordBC, 32);
                hopperStream.Write(hopper.dwordC0, 32);
                hopperStream.Write(hopper.gapC4, 8);
                hopperStream.Write(hopper.uses_high_score_leaderboard, 8);
                hopperStream.Write(hopper.gapC6, 8);
                hopperStream.Write(hopper.gapC7, 8);
                hopperStream.Write(hopper.posse_formation, 32);
                hopperStream.Write(hopper.post_match_countdown_time_seconds, 32);
                hopperStream.Write(hopper.require_hosts_on_multiple_teams, 32);
                hopperStream.Write(hopper.repeated_opponents_to_consider_for_penalty, 32);
                hopperStream.Write(hopper.repeated_opponents_skill_throttle_start, 32);
                hopperStream.Write(hopper.repeated_opponents_skill_throttle_stop, 32);
                hopperStream.Write(hopper.is_team_matching_enabled, 32);
                hopperStream.Write(hopper.gather_start_threshold_seconds, 32);
                hopperStream.Write(hopper.get_gather_start_game_early_seconds, 32);
                hopperStream.Write(hopper.get_gather_give_up_seconds, 32);

                for (int j = 0; j < 16; j++)
                    hopperStream.Write(hopper.chance_of_gathering[j], 8);

                hopperStream.Write(hopper.gapF0_5, 32);
                hopperStream.Write(hopper.dword104, 32);
                hopperStream.Write(hopper.dword108, 32);
                hopperStream.Write(hopper.uses_ffa_scoring_for_leaderboard_writes, 8);
                hopperStream.Write(hopper.should_modify_skill_update_weight_with_game_quality, 8);
                hopperStream.Write(hopper.gap10E, 8);
                hopperStream.Write(hopper.gap10F, 8);
                hopperStream.WriteFloat(hopper.trueskill_sigma_multiplier, 32);
                hopperStream.Write(hopper.dword114, 32);
                hopperStream.Write(hopper.trueskill_tau_dynamics_factor, 32);
                hopperStream.Write(hopper.trueskill_draw_probability, 32);
                hopperStream.Write(hopper.pre_match_voice_configuration, 32);
                hopperStream.Write(hopper.in_match_voice_configuration, 32);
                hopperStream.Write(hopper.post_match_voice_configuration, 32);
                hopperStream.Write(hopper.restrict_open_channel, 32);
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

                hopperStream.Write(hopper.games_game_type, 32);
                hopperStream.Write(hopper.maximum_player_count, 32);
                hopperStream.Write(hopper.maximum_player_count, 32);
                hopperStream.Write(hopper.ffa_model_override, 32);
                hopperStream.Write(hopper.minimum_team_count, 32);
                hopperStream.Write(hopper.maximum_team_count, 32);

                for (int j = 0; j < hopper.per_team_data.Length; j++)
                {
                    var perTeamData = hopper.per_team_data[j];
                    hopperStream.Write(perTeamData.minimum_team_size, 32);
                    hopperStream.Write(perTeamData.maximum_team_size, 32);
                    hopperStream.Write(perTeamData.team_model_override, 32);
                    hopperStream.Write(perTeamData.team_allegiance, 32);
                }

                hopperStream.Write(hopper.maximum_team_imbalance, 32);
                hopperStream.Write(hopper.big_squad_size_threshold, 32);
                hopperStream.Write(hopper.dword424, 32);
                hopperStream.Write(hopper.gap428, 32);
                hopperStream.Write(hopper.undersized_party_split_permissions, 32);
                hopperStream.Write(hopper.jackpot_minimum_time_seconds, 32);

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
            public uint minimum_team_size;
            public uint maximum_team_size;
            public uint team_model_override;
            public uint team_allegiance;
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
            public ushort category_identifier;
            public byte category_index;
            public byte player_investment_category;
            public byte pad3A;
            public byte pad3B;
            public uint image_index;
            public uint xlast_index;
            public byte equivalency_id; // not sure on type.
            public byte pad45;
            public byte pad46;
            public byte pad47;
            public ulong startTime;
            public ulong endTime;
            public uint minimum_games_won;
            public uint maximum_games_won;
            public uint minimum_games_played;
            public uint maximum_games_played;
            public uint minimum_grade;
            public uint maximum_grade;
            public uint min_party_size;
            public uint max_party_size;
            public uint min_local_players;
            public uint max_local_players;
            public uint hopper_access_bit;
            public uint account_type_access;
            public byte require_all_party_members_meet_games_played_requirements;
            public byte byte89;
            public byte require_all_party_members_meet_grade_requirements;
            public byte require_all_party_members_meet_access_requirements;
            public byte require_all_party_members_meet_live_account_access_requirements;
            public byte hide_hopper_from_games_played_restricted_players;
            public byte byte8E;
            public byte hide_hopper_from_grade_restricted_players;
            public byte hide_hopper_from_access_restricted_players;
            public byte hide_hopper_from_live_account_access_restricted_players;
            public byte hide_hopper_due_to_time_restriction;
            public byte pad92;
            public byte requires_hard_drive;
            public byte requires_local_party;
            public byte pad95;
            public byte pad96;
            public byte pad97;
            public uint dword98;
            public uint dword9C;
            public uint dwordA0;
            public uint dwordA4;
            public uint dwordA8;
            public uint dwordAC;
            public uint gapB0_1; // not sure on type for any of these.
            public byte is_ranked;
            public byte is_arbitrated;
            public byte are_guests_allowed;
            public byte are_opponents_visible;
            public byte uses_arena_lsp_stats;
            public byte gapB9; // arena related
            public byte gapBA;
            public byte gapBB;
            public uint dwordBC; // arena related
            public uint dwordC0; // arena related
            public byte gapC4; // arena related?
            public byte uses_high_score_leaderboard;
            public byte gapC6;
            public byte gapC7;
            public uint posse_formation;
            public uint post_match_countdown_time_seconds;
            public uint require_hosts_on_multiple_teams; // not sure on type.
            public uint repeated_opponents_to_consider_for_penalty;
            public uint repeated_opponents_skill_throttle_start;
            public uint repeated_opponents_skill_throttle_stop;
            public uint is_team_matching_enabled; // not sure on type.
            public uint gather_start_threshold_seconds;
            public uint get_gather_start_game_early_seconds;
            public uint get_gather_give_up_seconds;
            public byte[] chance_of_gathering;// 16
            public uint gapF0_5;
            public uint dword104;
            public uint dword108;
            public byte uses_ffa_scoring_for_leaderboard_writes;  // not sure on type.
            public byte should_modify_skill_update_weight_with_game_quality;  // not sure on type.
            public byte gap10E;  // not sure on type.
            public byte gap10F;  // not sure on type.
            public float trueskill_sigma_multiplier;
            public uint dword114;
            public uint trueskill_tau_dynamics_factor;
            public uint trueskill_draw_probability;
            public uint pre_match_voice_configuration;
            public uint in_match_voice_configuration;
            public uint post_match_voice_configuration;
            public uint restrict_open_channel;  // not sure on type.
            public uint dword130;
            public HopperQueryConfiguration[] query_configurations; // 4 of them
            public uint games_game_type;  // 0 == teams, 1 == firefight, 2 == campaign? // this is also min player count for FFA
            public uint maximum_player_count; // maximum_player_count
            public uint ffa_model_override;
            public uint minimum_team_count;
            public uint maximum_team_count;
            public PerTeamData[] per_team_data; // 8
            public uint maximum_team_imbalance;
            public uint big_squad_size_threshold;
            public uint dword424;
            public uint gap428; // not sure on type.
            public uint undersized_party_split_permissions;
            public uint jackpot_minimum_time_seconds;
            public JackpotConfiguration[] jackpot_configurations; // 3
        }
    }
}
