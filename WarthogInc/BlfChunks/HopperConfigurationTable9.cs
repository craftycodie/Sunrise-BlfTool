using Sewer56.BitStream.ByteStreams;
using Sewer56.BitStream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunriseBlfTool.Extensions;
using Newtonsoft.Json;

namespace SunriseBlfTool.BlfChunks
{
    internal class HopperConfigurationTable9 : IBLFChunk
    {
        public class HopperCategory
        {
            public ushort identifier;

            public string name;
        }

        public class HopperConfiguration
        {
            public string name;

            [JsonIgnore]
            public byte[] gameSetHash;

            public ushort identifier;

            public ushort category;

            public byte type;

            public ushort sortKey;

            public byte imageIndex;

            public byte xLastIndex;

            public ushort richPresenceId;

            public ulong startTime;

            public ulong endTime;

            public uint regions;

            public byte minimumExperienceRank;

            public byte maximumExperienceRank;

            public byte minimumPartySize;

            public byte maximumPartySize;

            public byte hopperAccessBit;

            public byte accountTypeAccess;

            public bool require_all_party_members_meet_base_xp_requirements;

            public bool require_all_party_members_meet_access_requirements;

            public bool require_all_party_members_meet_live_account_access_requirements;

            public bool hide_hopper_from_xp_restricted_players;

            public bool hide_hopper_from_access_restricted_players;

            public bool hide_hopper_from_live_account_access_restricted_players;

            public bool requires_beta_rights;

            public bool requires_all_downloadable_maps;

            public bool veto_enabled;

            public bool guests_allowed;

            public bool require_hosts_on_multiple_teams;

            public bool balance_network_bandwidth_between_teams;

            public byte stats_write;

            public byte language_filter;

            public byte country_code_filter;

            public byte gamerzone_filter;

            public byte quitter_filter_percentage;

            public byte quitter_filter_maximum_party_size;

            public ushort rematch_countdown_timer;

            public byte rematch_group_formation;

            public byte repeated_opponents_to_consider_for_penalty;

            public byte repeated_opponents_experience_threshold;

            public byte repeated_opponents_skill_throttle_start;

            public byte repeated_opponents_skill_throttle_stop;

            public ushort maximum_total_matchmaking_seconds;

            public ushort gather_start_game_early_seconds;

            public ushort gather_give_up_seconds;

            [JsonConverter(typeof(ByteArrayConverter))]
            public byte[] chance_of_gathering;

            public byte experience_points_per_win;

            public byte experience_penalty_per_drop;

            public float[] minimum_mu_per_level;

            [JsonConverter(typeof(ByteArrayConverter))]
            public byte[] maximum_skill_level_match_delta;

            public float trueskill_sigma_multiplier;

            public float trueskill_beta_performance_variation;

            public float trueskill_tau_dynamics_factor;

            public bool trueskill_adjust_tau_with_update_weight;

            public byte trueskill_draw_probability;

            public byte trueskill_hillclimb_w0;

            public byte trueskill_hillclimb_w50;

            public byte trueskill_hillclimb_w100;

            public byte trueskill_hillclimb_w150;

            public byte skill_update_weight_s0;

            public byte skill_update_weight_s10;

            public byte skill_update_weight_s20;

            public byte skill_update_weight_s30;

            public byte skill_update_weight_s40;

            public byte skill_update_weight_s50;

            public byte quality_update_weight_q0;

            public byte quality_update_weight_q25;

            public byte quality_update_weight_q50;

            public byte quality_update_weight_q75;

            public byte quality_update_weight_q100;

            public byte minimum_player_count;

            public byte maximum_player_count;

            public byte team_count;

            public byte minimum_team_size;

            public byte maximum_team_size;

            public byte maximum_team_imbalance;

            public byte big_squad_size_threshold;

            public byte maximum_big_squad_imbalance;

            public bool enable_big_squad_mixed_skill_restrictions;

            public bool allow_uneven_teams;

            public bool allow_parties_to_split;
        }

        public HopperCategory[] categories;

        public HopperConfiguration[] configurations;

        [JsonIgnore]
        public byte configurationsCount => (byte)configurations.Length;

        [JsonIgnore]
        public byte categoryCount => (byte)categories.Length;

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
            return 9;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> stream)
        {
            var memoryStream = new MemoryStream();
            var hoppersStream = new BitStream<StreamByteStream>(new StreamByteStream(memoryStream));

            hoppersStream.WriteBitswapped(categoryCount, 3);
            bool flag = categoryCount >= 0 && categoryCount < 4;
            if (!flag)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Too many hopper categories to convert! ${categoryCount}/4");
                throw new InvalidDataException("Too many hopper categories to convert!");
            }
            int num = 0;
            while (flag && num < categoryCount)
            {
                HopperCategory hopperCategory = categories[num];
                hoppersStream.WriteBitswapped(hopperCategory.identifier, 16);
                hoppersStream.WriteBitswappedString(hopperCategory.name, 32, Encoding.UTF8);
                categories[num] = hopperCategory;
                num++;
            }
            bool flag2 = configurationsCount >= 0 && configurationsCount <= 32;
            if (!flag2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Too many hopper configurations to convert! ${configurationsCount}/32");
                throw new InvalidDataException("Too many hopper configurations to convert!");
            }
            hoppersStream.WriteBitswapped(configurationsCount, 6);
            int num2 = 0;
            while (flag2 && num2 < configurationsCount)
            {
                HopperConfiguration hopperConfiguration = configurations[num2];
                hoppersStream.WriteBitswappedString(hopperConfiguration.name, 32, Encoding.UTF8);
                for (int i = 0; i < 20; i++)
                {
                    hoppersStream.Write<byte>(hopperConfiguration.gameSetHash[i], 8);
                }
                hoppersStream.WriteBitswapped(hopperConfiguration.identifier, 16);
                hoppersStream.WriteBitswapped(hopperConfiguration.category, 16);
                hoppersStream.WriteBitswapped(hopperConfiguration.type, 2);
                hoppersStream.WriteBitswapped(hopperConfiguration.sortKey, 10);
                hoppersStream.WriteBitswapped(hopperConfiguration.imageIndex, 6);
                hoppersStream.WriteBitswapped(hopperConfiguration.xLastIndex, 5);
                hoppersStream.WriteBitswapped(hopperConfiguration.richPresenceId, 16);
                hoppersStream.WriteBitswapped((long)hopperConfiguration.startTime, 64);
                hoppersStream.WriteBitswapped((long)hopperConfiguration.endTime, 64);
                hoppersStream.WriteBitswapped(hopperConfiguration.regions, 32);
                hoppersStream.WriteBitswapped(hopperConfiguration.minimumExperienceRank, 4);
                hoppersStream.WriteBitswapped(hopperConfiguration.maximumExperienceRank, 4);
                hoppersStream.WriteBitswapped(hopperConfiguration.minimumPartySize, 4);
                hoppersStream.WriteBitswapped(hopperConfiguration.maximumPartySize, 4);
                hoppersStream.WriteBitswapped(hopperConfiguration.hopperAccessBit, 4);
                hoppersStream.WriteBitswapped(hopperConfiguration.accountTypeAccess, 2);
                hoppersStream.WriteBitswapped(hopperConfiguration.require_all_party_members_meet_base_xp_requirements ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.require_all_party_members_meet_access_requirements ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.require_all_party_members_meet_live_account_access_requirements ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.hide_hopper_from_xp_restricted_players ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.hide_hopper_from_access_restricted_players ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.hide_hopper_from_live_account_access_restricted_players ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.requires_beta_rights ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.requires_all_downloadable_maps ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.veto_enabled ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.guests_allowed ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.require_hosts_on_multiple_teams ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.balance_network_bandwidth_between_teams ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.stats_write, 2);
                hoppersStream.WriteBitswapped(hopperConfiguration.language_filter, 2);
                hoppersStream.WriteBitswapped(hopperConfiguration.country_code_filter, 2);
                hoppersStream.WriteBitswapped(hopperConfiguration.gamerzone_filter, 2);
                hoppersStream.WriteBitswapped(hopperConfiguration.quitter_filter_percentage, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.quitter_filter_maximum_party_size, 4);
                hoppersStream.WriteBitswapped(hopperConfiguration.rematch_countdown_timer, 10);
                hoppersStream.WriteBitswapped(hopperConfiguration.rematch_group_formation, 2);
                hoppersStream.WriteBitswapped(hopperConfiguration.repeated_opponents_to_consider_for_penalty, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.repeated_opponents_experience_threshold, 4);
                hoppersStream.WriteBitswapped(hopperConfiguration.repeated_opponents_skill_throttle_start, 4);
                hoppersStream.WriteBitswapped(hopperConfiguration.repeated_opponents_skill_throttle_stop, 4);
                hoppersStream.WriteBitswapped(hopperConfiguration.maximum_total_matchmaking_seconds, 10);
                hoppersStream.WriteBitswapped(hopperConfiguration.gather_start_game_early_seconds, 10);
                hoppersStream.WriteBitswapped(hopperConfiguration.gather_give_up_seconds, 10);
                for (int j = 0; j < 8; j++)
                {
                    hoppersStream.WriteBitswapped(hopperConfiguration.chance_of_gathering[j], 7);
                }
                hoppersStream.WriteBitswapped(hopperConfiguration.experience_points_per_win, 2);
                hoppersStream.WriteBitswapped(hopperConfiguration.experience_penalty_per_drop, 2);
                for (int k = 0; k < 49; k++)
                {
                    hoppersStream.WriteBitswappedFloat(hopperConfiguration.minimum_mu_per_level[k], 32);
                }
                for (int l = 0; l < 50; l++)
                {
                    hoppersStream.WriteBitswapped(hopperConfiguration.maximum_skill_level_match_delta[l], 6);
                }
                hoppersStream.WriteBitswappedFloat(hopperConfiguration.trueskill_sigma_multiplier, 32);
                hoppersStream.WriteBitswappedFloat(hopperConfiguration.trueskill_beta_performance_variation, 32);
                hoppersStream.WriteBitswappedFloat(hopperConfiguration.trueskill_tau_dynamics_factor, 32);
                hoppersStream.WriteBitswapped(hopperConfiguration.trueskill_adjust_tau_with_update_weight ? ((byte)1) : ((byte)0), 1);
                hoppersStream.WriteBitswapped(hopperConfiguration.trueskill_draw_probability, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.trueskill_hillclimb_w0, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.trueskill_hillclimb_w50, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.trueskill_hillclimb_w100, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.trueskill_hillclimb_w150, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.skill_update_weight_s0, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.skill_update_weight_s10, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.skill_update_weight_s20, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.skill_update_weight_s30, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.skill_update_weight_s40, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.skill_update_weight_s50, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.quality_update_weight_q0, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.quality_update_weight_q25, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.quality_update_weight_q50, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.quality_update_weight_q75, 7);
                hoppersStream.WriteBitswapped(hopperConfiguration.quality_update_weight_q100, 7);
                if (hopperConfiguration.type <= 1)
                {
                    hoppersStream.WriteBitswapped(hopperConfiguration.minimum_player_count, 4);
                    hoppersStream.WriteBitswapped(hopperConfiguration.maximum_player_count, 4);
                }
                else if (hopperConfiguration.type == 3)
                {
                    hoppersStream.WriteBitswapped(hopperConfiguration.team_count, 3);
                    hoppersStream.WriteBitswapped(hopperConfiguration.minimum_team_size, 3);
                    hoppersStream.WriteBitswapped(hopperConfiguration.maximum_team_size, 3);
                    hoppersStream.WriteBitswapped(hopperConfiguration.maximum_team_imbalance, 3);
                    hoppersStream.WriteBitswapped(hopperConfiguration.big_squad_size_threshold, 4);
                    hoppersStream.WriteBitswapped(hopperConfiguration.maximum_big_squad_imbalance, 3);
                    hoppersStream.WriteBitswapped(hopperConfiguration.enable_big_squad_mixed_skill_restrictions ? ((byte)1) : ((byte)0), 1);
                }
                else
                {
                    hoppersStream.WriteBitswapped(hopperConfiguration.team_count, 3);
                    hoppersStream.WriteBitswapped(hopperConfiguration.minimum_team_size, 3);
                    hoppersStream.WriteBitswapped(hopperConfiguration.maximum_team_size, 3);
                    hoppersStream.WriteBitswapped(hopperConfiguration.allow_uneven_teams ? ((byte)1) : ((byte)0), 1);
                    hoppersStream.WriteBitswapped(hopperConfiguration.allow_parties_to_split ? ((byte)1) : ((byte)0), 1);
                }
                num2++;
            }
            if (hoppersStream.BitIndex % 8 != 0)
            {
                hoppersStream.WriteBitswapped((byte)0, 8 - hoppersStream.BitIndex % 8);
            }
            memoryStream.Seek(0L, SeekOrigin.Begin);
            while (memoryStream.Position < memoryStream.Length)
            {
                stream.WriteBitswapped((byte)memoryStream.ReadByte(), 8);
            }
        }
    }

}
