using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.IO;
using System.Text;
using WarthogInc.BlfChunks;
using Sunrise.BlfTool.Extensions;

namespace Sunrise.BlfTool
{
    class HopperConfigurationTable7 : IBLFChunk
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
            return 7;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> stream)
        {
            var ms = new MemoryStream();
            var hoppersStream = new BitStream<StreamByteStream>(new StreamByteStream(ms));

            hoppersStream.WriteBitswapped<byte>(categoryCount, 3);
            bool validCategoriesCount = categoryCount >= 0 && categoryCount < 4;

            if (!validCategoriesCount)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Too many hopper categories to convert! ${categoryCount}/4");
                throw new InvalidDataException("Too many hopper categories to convert!");
            }

            for (int i = 0; validCategoriesCount && i < categoryCount; i++)
            {
                HopperCategory category = categories[i];

                hoppersStream.WriteBitswapped<ushort>(category.identifier, 16);
                hoppersStream.WriteBitswappedString(category.name, 32, Encoding.UTF8);

                categories[i] = category;
            }

            bool validConfigurationsCount = configurationsCount >= 0 && configurationsCount <= 32;

            if (!validConfigurationsCount)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Too many hopper configurations to convert! ${configurationsCount}/32");
                throw new InvalidDataException("Too many hopper configurations to convert!");
            }

            hoppersStream.WriteBitswapped<byte>(configurationsCount, 6);

            for (int i = 0; validConfigurationsCount && i < configurationsCount; i++)
            {
                HopperConfiguration configuration = configurations[i];

                hoppersStream.WriteBitswappedString(configuration.name, 32, Encoding.UTF8);

                hoppersStream.WriteBitswapped<ushort>(configuration.identifier, 16);

                hoppersStream.WriteBitswapped<ushort>(configuration.category, 16);
                hoppersStream.WriteBitswapped<byte>(configuration.type, 2);
                hoppersStream.WriteBitswapped<ushort>(configuration.sortKey, 10);
                hoppersStream.WriteBitswapped<byte>(configuration.imageIndex, 6);
                hoppersStream.WriteBitswapped<byte>(configuration.xLastIndex, 5);
                hoppersStream.WriteBitswapped<ushort>(configuration.richPresenceId, 16);
                hoppersStream.WriteBitswapped<long>((long)configuration.startTime, 64);
                hoppersStream.WriteBitswapped<long>((long)configuration.endTime, 64);
                hoppersStream.WriteBitswapped<uint>(configuration.regions, 32);
                hoppersStream.WriteBitswapped<byte>(configuration.minimumExperienceRank, 4);
                hoppersStream.WriteBitswapped<byte>(configuration.maximumExperienceRank, 4);
                hoppersStream.WriteBitswapped<byte>(configuration.minimumPartySize, 4);
                hoppersStream.WriteBitswapped<byte>(configuration.maximumPartySize, 4); //ok
                hoppersStream.WriteBitswapped<byte>(configuration.hopperAccessBit, 4);
                hoppersStream.WriteBitswapped<byte>(configuration.accountTypeAccess, 2);
                hoppersStream.WriteBitswapped<byte>(configuration.require_all_party_members_meet_base_xp_requirements ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.require_all_party_members_meet_access_requirements ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.require_all_party_members_meet_live_account_access_requirements ? (byte)1 : (byte)0, 1); // seems wrong
                hoppersStream.WriteBitswapped<byte>(configuration.hide_hopper_from_xp_restricted_players ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.hide_hopper_from_access_restricted_players ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.hide_hopper_from_live_account_access_restricted_players ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.requires_beta_rights? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.requires_all_downloadable_maps ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.veto_enabled ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.guests_allowed ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.require_hosts_on_multiple_teams ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.balance_network_bandwidth_between_teams ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.stats_write, 2);
                hoppersStream.WriteBitswapped<byte>(configuration.language_filter, 2);
                hoppersStream.WriteBitswapped<byte>(configuration.country_code_filter, 2);
                hoppersStream.WriteBitswapped<byte>(configuration.gamerzone_filter, 2);
                hoppersStream.WriteBitswapped<byte>(configuration.quitter_filter_percentage, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.quitter_filter_maximum_party_size, 4);
                hoppersStream.WriteBitswapped<ushort>(configuration.rematch_countdown_timer, 10);
                hoppersStream.WriteBitswapped<byte>(configuration.rematch_group_formation, 2);
                hoppersStream.WriteBitswapped<byte>(configuration.repeated_opponents_to_consider_for_penalty, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.repeated_opponents_experience_threshold, 4);
                hoppersStream.WriteBitswapped<byte>(configuration.repeated_opponents_skill_throttle_start, 4);
                hoppersStream.WriteBitswapped<byte>(configuration.repeated_opponents_skill_throttle_stop, 4);
                hoppersStream.WriteBitswapped<ushort>(configuration.maximum_total_matchmaking_seconds, 10);
                hoppersStream.WriteBitswapped<ushort>(configuration.gather_start_game_early_seconds, 10);
                hoppersStream.WriteBitswapped<ushort>(configuration.gather_give_up_seconds, 10);

                for (int k = 0; k < 8; k++)
                    hoppersStream.WriteBitswapped<byte>(configuration.chance_of_gathering[k], 7);

                hoppersStream.WriteBitswapped<byte>(configuration.experience_points_per_win, 2);
                hoppersStream.WriteBitswapped<byte>(configuration.experience_penalty_per_drop, 2);

                for (int l = 0; l < 49; l++)
                    hoppersStream.WriteBitswappedFloat(configuration.minimum_mu_per_level[l], 32);

                for (int m = 0; m < 50; m++)
                    hoppersStream.WriteBitswapped<byte>(configuration.maximum_skill_level_match_delta[m], 6);

                hoppersStream.WriteBitswappedFloat(configuration.trueskill_sigma_multiplier, 32);
                hoppersStream.WriteBitswappedFloat(configuration.trueskill_beta_performance_variation, 32);
                hoppersStream.WriteBitswappedFloat(configuration.trueskill_tau_dynamics_factor, 32);
                hoppersStream.WriteBitswapped<byte>(configuration.trueskill_adjust_tau_with_update_weight ? (byte)1 : (byte)0, 1);
                hoppersStream.WriteBitswapped<byte>(configuration.trueskill_draw_probability, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.trueskill_hillclimb_w0, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.trueskill_hillclimb_w50, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.trueskill_hillclimb_w100, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.trueskill_hillclimb_w150, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.skill_update_weight_s0, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.skill_update_weight_s10, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.skill_update_weight_s20, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.skill_update_weight_s30, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.skill_update_weight_s40, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.skill_update_weight_s50, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.quality_update_weight_q0, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.quality_update_weight_q25, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.quality_update_weight_q50, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.quality_update_weight_q75, 7);
                hoppersStream.WriteBitswapped<byte>(configuration.quality_update_weight_q100, 7);

                if (configuration.type <= 1)
                {
                    hoppersStream.WriteBitswapped<byte>(configuration.minimum_player_count, 4);
                    hoppersStream.WriteBitswapped<byte>(configuration.maximum_player_count, 4);

                }
                else if (configuration.type == 3)
                {
                    hoppersStream.WriteBitswapped<byte>(configuration.team_count, 3);
                    hoppersStream.WriteBitswapped<byte>(configuration.minimum_team_size, 3);
                    hoppersStream.WriteBitswapped<byte>(configuration.maximum_team_size, 3);
                    hoppersStream.WriteBitswapped<byte>(configuration.maximum_team_imbalance, 3);
                    hoppersStream.WriteBitswapped<byte>(configuration.big_squad_size_threshold, 4);
                    hoppersStream.WriteBitswapped<byte>(configuration.maximum_big_squad_imbalance, 3);
                    hoppersStream.WriteBitswapped<byte>(configuration.enable_big_squad_mixed_skill_restrictions ? (byte)1 : (byte)0, 1);
                }
                else
                {
                    hoppersStream.WriteBitswapped<byte>(configuration.team_count, 3);
                    hoppersStream.WriteBitswapped<byte>(configuration.minimum_team_size, 3);
                    hoppersStream.WriteBitswapped<byte>(configuration.maximum_team_size, 3);
                    hoppersStream.WriteBitswapped<byte>(configuration.allow_uneven_teams ? (byte)1 : (byte)0, 1);
                    hoppersStream.WriteBitswapped<byte>(configuration.allow_parties_to_split ? (byte)1 : (byte)0, 1);
                }

                if (hoppersStream.BitIndex % 8 != 0)
                    hoppersStream.WriteBitswapped((byte)0, 8 - (hoppersStream.BitIndex % 8));

                ms.Seek(0, SeekOrigin.Begin);
                while (ms.Position < ms.Length)
                {
                    stream.WriteBitswapped((byte)ms.ReadByte(), 8);
                }
            }
        }

        public class HopperCategory
        {
            public ushort identifier;
            public string name;
        }

        public class HopperConfiguration
        {
            public string name;
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
    }
}
