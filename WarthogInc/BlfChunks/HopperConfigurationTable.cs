﻿using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarthogInc.BlfChunks;

namespace Sunrise.BlfTool
{
    class HopperConfigurationTable : IBLFChunk
    {
        public byte configurationsCount;
        public byte categoryCount;
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
            return (uint)ms.ByteOffset;
        }

        public string GetName()
        {
            return "mhcf";
        }

        public ushort GetVersion()
        {
            return 11;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            hoppersStream.Write<byte>(categoryCount, 3);
            bool validCategoriesCount = categoryCount >= 0 && categoryCount <= 4;

            for (int i = 0; validCategoriesCount && i < categoryCount; i++)
            {
                HopperCategory category = categories[i];

                hoppersStream.Write<ushort>(category.identifier, 16);
                hoppersStream.Write<byte>(category.image, 6);
                hoppersStream.WriteString(category.name, 32, Encoding.UTF8);
                //hoppersStream.Write(0, 32 * 8)

                categories[i] = category;
            }

            bool validConfigurationsCount = configurationsCount >= 0 && configurationsCount <= 32;

            hoppersStream.Write<byte>(configurationsCount, 6);

            for (int i = 0; validConfigurationsCount && i < configurationsCount; i++)
            {
                HopperConfiguration configuration = configurations[i];

                hoppersStream.WriteString(configuration.name, 32, Encoding.UTF8);


                //configuration.hashSet = hoppersStream.Read<byte[]>(160);
                for (int j = 0; j < 20; j++)
                    hoppersStream.Write<byte>(configuration.hashSet[j], 8);
                hoppersStream.Write<ushort>(configuration.identifier, 16);

                hoppersStream.Write<ushort>(configuration.category, 16);
                hoppersStream.Write<byte>(configuration.type, 2);
                hoppersStream.Write<byte>(configuration.imageIndex, 6);
                hoppersStream.Write<byte>(configuration.xLastIndex, 5);
                hoppersStream.Write<ushort>(configuration.richPresenceId, 16);
                hoppersStream.Write<ulong>(configuration.startTime, 64);
                hoppersStream.Write<ulong>(configuration.endTime, 64);
                hoppersStream.Write<uint>(configuration.regions, 32);
                hoppersStream.Write<uint>(configuration.minimumBaseXp, 17);
                hoppersStream.Write<uint>(configuration.maximumBaseXp, 17);
                hoppersStream.Write<uint>(configuration.minimumGamesPlayed, 17);
                hoppersStream.Write<uint>(configuration.maximumGamesPlayed, 17);
                hoppersStream.Write<byte>(configuration.minimumPartySize, 4);
                hoppersStream.Write<byte>(configuration.maximumPartySize, 4); //ok
                hoppersStream.Write<byte>(configuration.hopperAccessBit, 4);
                hoppersStream.Write<byte>(configuration.accountTypeAccess, 2);
                hoppersStream.Write<byte>(configuration.require_all_party_members_meet_games_played_requirements ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.require_all_party_members_meet_base_xp_requirements ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.require_all_party_members_meet_access_requirements ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.require_all_party_members_meet_live_account_access_requirements ? (byte)1 : (byte)0, 1); // seems wrong
                hoppersStream.Write<byte>(configuration.hide_hopper_from_games_played_restricted_players ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.hide_hopper_from_xp_restricted_players ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.hide_hopper_from_access_restricted_players ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.hide_hopper_from_live_account_access_restricted_players ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.hide_hopper_due_to_time_restriction ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.preMatchVoice, 2);
                hoppersStream.Write<byte>(configuration.inMatchVoice, 2);
                hoppersStream.Write<byte>(configuration.postMatchVoice, 2);
                hoppersStream.Write<byte>(configuration.restrictOpenChannel ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.requires_all_downloadable_maps ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.veto_enabled ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.guests_allowed ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.require_hosts_on_multiple_teams ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.stats_write, 2);
                hoppersStream.Write<byte>(configuration.language_filter, 2);
                hoppersStream.Write<byte>(configuration.country_code_filter, 2);
                hoppersStream.Write<byte>(configuration.gamerzone_filter, 2);
                hoppersStream.Write<byte>(configuration.quitter_filter_percentage, 7);
                hoppersStream.Write<byte>(configuration.quitter_filter_maximum_party_size, 4);
                hoppersStream.Write<ushort>(configuration.rematch_countdown_timer, 10);
                hoppersStream.Write<byte>(configuration.rematch_group_formation, 2);
                hoppersStream.Write<byte>(configuration.repeated_opponents_to_consider_for_penalty, 7);
                hoppersStream.Write<byte>(configuration.repeated_opponents_experience_threshold, 4);
                hoppersStream.Write<byte>(configuration.repeated_opponents_skill_throttle_start, 4);
                hoppersStream.Write<byte>(configuration.repeated_opponents_skill_throttle_stop, 4);
                hoppersStream.Write<ushort>(configuration.maximum_total_matchmaking_seconds, 10);
                hoppersStream.Write<ushort>(configuration.gather_start_game_early_seconds, 10);
                hoppersStream.Write<ushort>(configuration.gather_give_up_seconds, 10);

                for (int k = 0; k < 16; k++)
                    hoppersStream.Write<byte>(configuration.chance_of_gathering[k], 7);

                hoppersStream.Write<byte>(configuration.experience_points_per_win, 2);
                hoppersStream.Write<byte>(configuration.experience_penalty_per_drop, 2);

                for (int l = 0; l < 49; l++)
                    hoppersStream.Write<uint>(configuration.minimum_mu_per_level[l], 32);

                for (int m = 0; m < 50; m++)
                    hoppersStream.Write<byte>(configuration.maximum_skill_level_match_delta[m], 6);

                hoppersStream.Write<uint>(configuration.trueskill_sigma_multiplier, 32);
                hoppersStream.Write<uint>(configuration.trueskill_beta_performance_variation, 32);
                hoppersStream.Write<uint>(configuration.trueskill_tau_dynamics_factor, 32);
                hoppersStream.Write<byte>(configuration.trueskill_adjust_tau_with_update_weight ? (byte)1 : (byte)0, 1);
                hoppersStream.Write<byte>(configuration.trueskill_draw_probability, 7);
                hoppersStream.Write<byte>(configuration.trueskill_hillclimb_w0, 7);
                hoppersStream.Write<byte>(configuration.trueskill_hillclimb_w50, 7);
                hoppersStream.Write<byte>(configuration.trueskill_hillclimb_w100, 7);
                hoppersStream.Write<byte>(configuration.trueskill_hillclimb_w150, 7);
                hoppersStream.Write<byte>(configuration.skill_update_weight_s0, 7);
                hoppersStream.Write<byte>(configuration.skill_update_weight_s10, 7);
                hoppersStream.Write<byte>(configuration.skill_update_weight_s20, 7);
                hoppersStream.Write<byte>(configuration.skill_update_weight_s30, 7);
                hoppersStream.Write<byte>(configuration.skill_update_weight_s40, 7);
                hoppersStream.Write<byte>(configuration.skill_update_weight_s50, 7);
                hoppersStream.Write<byte>(configuration.quality_update_weight_q0, 7);
                hoppersStream.Write<byte>(configuration.quality_update_weight_q25, 7);
                hoppersStream.Write<byte>(configuration.quality_update_weight_q50, 7);
                hoppersStream.Write<byte>(configuration.quality_update_weight_q75, 7);
                hoppersStream.Write<byte>(configuration.quality_update_weight_q100, 7);

                if (configuration.type <= 1)
                {
                    hoppersStream.Write<byte>(configuration.minimum_player_count, 4);
                    hoppersStream.Write<byte>(configuration.maximum_player_count, 4);

                }
                else if (((configuration.type - 1) & 0xFFFFFFFD) == 0)
                {
                    hoppersStream.Write<byte>(configuration.team_count, 3);
                    hoppersStream.Write<byte>(configuration.minimum_team_size, 3);
                    hoppersStream.Write<byte>(configuration.maximum_team_size, 3);
                    hoppersStream.Write<byte>(configuration.maximum_team_imbalance, 3);
                    hoppersStream.Write<byte>(configuration.big_squad_size_threshold, 4);
                    hoppersStream.Write<byte>(configuration.maximum_big_squad_imbalance, 3);
                    hoppersStream.Write<byte>(configuration.enable_big_squad_mixed_skill_restrictions ? (byte)1 : (byte)0, 1);
                }
                else
                {
                    hoppersStream.Write<byte>(configuration.team_count, 3);
                    hoppersStream.Write<byte>(configuration.minimum_team_size, 3);
                    hoppersStream.Write<byte>(configuration.maximum_team_size, 3);
                    hoppersStream.Write<byte>(configuration.allow_uneven_teams ? (byte)1 : (byte)0, 1);
                    hoppersStream.Write<byte>(configuration.allow_parties_to_split ? (byte)1 : (byte)0, 1);
                }
            }
            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public class HopperCategory
        {
            public ushort identifier;
            public byte image;
            public string name;
        }

        public class HopperConfiguration
        {
            public string name;
            [JsonConverter(typeof(HexStringConverter))]
            public byte[] hashSet;
            public ushort identifier;
            public ushort category;
            public byte type;
            public byte imageIndex;
            public byte xLastIndex;
            public ushort richPresenceId;
            public ulong startTime;
            public ulong endTime;
            public uint regions;
            public uint minimumBaseXp;
            public uint maximumBaseXp;
            public uint minimumGamesPlayed;
            public uint maximumGamesPlayed;
            public byte minimumPartySize;
            public byte maximumPartySize;
            public byte hopperAccessBit;
            public byte accountTypeAccess;
            public bool require_all_party_members_meet_games_played_requirements;
            public bool require_all_party_members_meet_base_xp_requirements;
            public bool require_all_party_members_meet_access_requirements;
            public bool require_all_party_members_meet_live_account_access_requirements;
            public bool hide_hopper_from_games_played_restricted_players;
            public bool hide_hopper_from_xp_restricted_players;
            public bool hide_hopper_from_access_restricted_players;
            public bool hide_hopper_from_live_account_access_restricted_players;
            public bool hide_hopper_due_to_time_restriction;
            public byte preMatchVoice;
            public byte inMatchVoice;
            public byte postMatchVoice;
            public bool restrictOpenChannel;
            public bool requires_all_downloadable_maps;
            public bool veto_enabled;
            public bool guests_allowed;
            public bool require_hosts_on_multiple_teams;
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
            public uint[] minimum_mu_per_level;
            [JsonConverter(typeof(ByteArrayConverter))]
            public byte[] maximum_skill_level_match_delta;
            public uint trueskill_sigma_multiplier;
            public uint trueskill_beta_performance_variation;
            public uint trueskill_tau_dynamics_factor;
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