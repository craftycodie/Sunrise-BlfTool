using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc
{
    class HopperReader
    {

        public static UInt16 swap(UInt16 input)
        {
            return ((UInt16)(
            ((0xFF00 & input) >> 8) |
            ((0x00FF & input) << 8)));
        }

        public static UInt32 swap(UInt32 input)
        {
            return ((UInt32)(
            ((0xFF000000 & input) >> 24) |
            ((0x00FF0000 & input) >> 8) |
            ((0x0000FF00 & input) << 8) |
            ((0x000000FF & input) << 24)));
        }

        public static UInt64 swap(UInt64 input)
        {
            return ((UInt64)(
            ((0xFF00000000000000 & input) >> 56) |
            ((0x00FF000000000000 & input) >> 40) |
            ((0x0000FF0000000000 & input) >> 24) |
            ((0x000000FF00000000 & input) >> 8) |
            ((0x00000000FF000000 & input) << 8) |
            ((0x0000000000FF0000 & input) << 24) |
            ((0x000000000000FF00 & input) << 40) |
            ((0x00000000000000FF & input) << 56)));
        }

        public HopperDescriptions ReadHopperDescriptions(BitStream<StreamByteStream> hoppersStream)
        {
            HopperDescriptions descriptions = new HopperDescriptions();

            descriptions.descriptionCount = hoppersStream.Read<byte>(6);
            descriptions.descriptions = new HopperDescriptions.HopperDescription[descriptions.descriptionCount];

            for (int i = 0; i < descriptions.descriptionCount; i++)
            {
                hoppersStream.Read<byte>(1);
                HopperDescriptions.HopperDescription description = new HopperDescriptions.HopperDescription();
                description.identifier = hoppersStream.Read<ushort>(16);
                description.type = hoppersStream.Read<bool>(1);
                description.description = hoppersStream.ReadString(256, Encoding.UTF8);

                descriptions.descriptions[i] = description;
            }

            return descriptions;
        }

        public Hoppers ReadHoppers(BitStream<StreamByteStream> hoppersStream)
        {
            Hoppers res = new Hoppers();

            res.categoryCount = hoppersStream.Read<byte>(3);
            res.categories = new Hoppers.HopperCategory[res.categoryCount];
            bool validCategoriesCount = res.categoryCount >= 0 && res.categoryCount <= 4;

            for (int i = 0; validCategoriesCount && i < res.categoryCount; i++) {
                Hoppers.HopperCategory category = new Hoppers.HopperCategory();

                category.identifier = hoppersStream.Read<short>(16);
                category.image =  hoppersStream.Read<byte>(6);
                category.name = hoppersStream.ReadString(32);

                res.categories[i] = category;
            }

            bool validConfigurationsCount = res.configurationsCount >= 0 && res.configurationsCount <= 32;


            res.configurationsCount = hoppersStream.Read<byte>(6);
            res.configurations = new Hoppers.HopperConfiguration[res.configurationsCount];

            for (int i = 0; validConfigurationsCount && i < res.configurationsCount; i++)
            {
                //Console.WriteLine(hoppersStream.ByteOffset);

                Hoppers.HopperConfiguration configuration = new Hoppers.HopperConfiguration();

                configuration.name = hoppersStream.ReadString(32, Encoding.UTF8);
                Console.WriteLine(configuration.name);
                //configuration.hashSet = hoppersStreriam.Read<string>(160);
                //hoppersStream.SeekRelative(32);

                hoppersStream.Read<int>(160);
                configuration.identifier =  swap(hoppersStream.Read<ushort>(16));
                //hoppersStream.Read<int>(160);

                configuration.category = swap(hoppersStream.Read<ushort>(16));
                configuration.type = hoppersStream.Read<byte>(2);
                configuration.imageIndex = hoppersStream.Read<byte>(6);
                configuration.xLastIndex = hoppersStream.Read<byte>(5);
                configuration.richPresenceId = swap(hoppersStream.Read<ushort>(16));
                configuration.startTime = swap(hoppersStream.Read<ulong>(64));
                configuration.endTime = swap(hoppersStream.Read<ulong>(64));
                configuration.regions = swap(hoppersStream.Read<uint>(32));
                configuration.minimumBaseXp = swap(hoppersStream.Read<uint>(17));
                configuration.maximumBaseXp = hoppersStream.Read<uint>(17);
                configuration.minimumGamesPlayed = hoppersStream.Read<uint>(17);
                configuration.maximumGamesPlayed = hoppersStream.Read<uint>(17);
                configuration.minimumPartySize = hoppersStream.Read<byte>(4);
                configuration.maximumPartySize = hoppersStream.Read<byte>(4);
                configuration.hopperAccessBit = hoppersStream.Read<byte>(4);
                configuration.accountTypeAccess = hoppersStream.Read<byte>(2);
                configuration.require_all_party_members_meet_games_played_requirements = hoppersStream.Read<bool>(1);
                configuration.require_all_party_members_meet_base_xp_requirements = hoppersStream.Read<bool>(1);
                configuration.require_all_party_members_meet_access_requirements = hoppersStream.Read<bool>(1);
                configuration.require_all_party_members_meet_live_account_access_requirements = hoppersStream.Read<bool>(1);
                configuration.hide_hopper_from_games_played_restricted_players = hoppersStream.Read<bool>(1);
                configuration.hide_hopper_from_xp_restricted_players = hoppersStream.Read<bool>(1);
                configuration.hide_hopper_from_access_restricted_players = hoppersStream.Read<bool>(1);
                configuration.hide_hopper_from_live_account_access_restricted_players = hoppersStream.Read<bool>(1);
                configuration.hide_hopper_due_to_time_restriction = hoppersStream.Read<bool>(1);
                configuration.preMatchVoice = hoppersStream.Read<byte>(2);
                configuration.inMatchVoice = hoppersStream.Read<byte>(2);
                configuration.postMatchVoice = hoppersStream.Read<byte>(2);
                configuration.restrictOpenChannel = hoppersStream.Read<bool>(1);
                configuration.requires_all_downloadable_maps = hoppersStream.Read<bool>(1);
                configuration.veto_enabled = hoppersStream.Read<bool>(1);
                configuration.guests_allowed = hoppersStream.Read<bool>(1);
                configuration.require_hosts_on_multiple_teams = hoppersStream.Read<bool>(1);
                configuration.stats_write = hoppersStream.Read<byte>(2);
                configuration.language_filter = hoppersStream.Read<byte>(2);
                configuration.country_code_filter = hoppersStream.Read<byte>(2);
                configuration.gamerzone_filter = hoppersStream.Read<byte>(2);
                configuration.quitter_filter_percentage = hoppersStream.Read<byte>(7);
                configuration.quitter_filter_maximum_party_size = hoppersStream.Read<byte>(4);
                configuration.rematch_countdown_timer = hoppersStream.Read<ushort>(10);
                configuration.rematch_group_formation = hoppersStream.Read<byte>(2);
                configuration.repeated_opponents_to_consider_for_penalty = hoppersStream.Read<byte>(7);
                configuration.repeated_opponents_experience_threshold = hoppersStream.Read<byte>(4);
                configuration.repeated_opponents_skill_throttle_start = hoppersStream.Read<byte>(4);
                configuration.repeated_opponents_skill_throttle_stop = hoppersStream.Read<byte>(4);
                configuration.maximum_total_matchmaking_seconds = hoppersStream.Read<ushort>(10);
                configuration.gather_start_game_early_seconds = hoppersStream.Read<ushort>(10);
                configuration.gather_give_up_seconds = hoppersStream.Read<ushort>(10);

                for (int k = 0; k < 16; k++)
                {
                    hoppersStream.Read<int>(7);
                }
                //configuration.chance_of_gathering = hoppersStream.Read<byte>(7);

                configuration.experience_points_per_win = hoppersStream.Read<byte>(2);
                configuration.experience_penalty_per_drop = hoppersStream.Read<byte>(2);

                for (int l = 0; l < 49; l++)
                    hoppersStream.Read<int>(32);
                for (int m = 0; m < 50; m++)
                    hoppersStream.Read<int>(6);

                //configuration.minimum_mu_per_level = hoppersStream.Read<uint>(32);
                //configuration.maximum_skill_level_match_delta = hoppersStream.Read<byte>(6);


                configuration.trueskill_sigma_multiplier = hoppersStream.Read<uint>(32);
                configuration.trueskill_beta_performance_variation = hoppersStream.Read<uint>(32);
                configuration.trueskill_tau_dynamics_factor = hoppersStream.Read<uint>(32);
                configuration.trueskill_adjust_tau_with_update_weight = hoppersStream.Read<bool>(1);
                configuration.trueskill_draw_probability = hoppersStream.Read<byte>(7);
                configuration.trueskill_hillclimb_w0 = hoppersStream.Read<byte>(7);
                configuration.trueskill_hillclimb_w50 = hoppersStream.Read<byte>(7);
                configuration.trueskill_hillclimb_w100 = hoppersStream.Read<byte>(7);
                configuration.trueskill_hillclimb_w150 = hoppersStream.Read<byte>(7);
                configuration.skill_update_weight_s0 = hoppersStream.Read<byte>(7);
                configuration.skill_update_weight_s10 = hoppersStream.Read<byte>(7);
                configuration.skill_update_weight_s20 = hoppersStream.Read<byte>(7);
                configuration.skill_update_weight_s30 = hoppersStream.Read<byte>(7);
                configuration.skill_update_weight_s40 = hoppersStream.Read<byte>(7);
                configuration.skill_update_weight_s50 = hoppersStream.Read<byte>(7);
                configuration.quality_update_weight_q0 = hoppersStream.Read<byte>(7);
                configuration.quality_update_weight_q25 = hoppersStream.Read<byte>(7);
                configuration.quality_update_weight_q50 = hoppersStream.Read<byte>(7);
                configuration.quality_update_weight_q75 = hoppersStream.Read<byte>(7);
                configuration.quality_update_weight_q100 = hoppersStream.Read<byte>(7);

                //Console.WriteLine(configuration.type);

                if (configuration.type <= 1)
                {
                    hoppersStream.Read<int>(8 + 7);


                    configuration.minimum_player_count = hoppersStream.Read<byte>(4);
                    configuration.maximum_player_count = hoppersStream.Read<byte>(4);

                }
                else if (configuration.type == 2)
                {
                    hoppersStream.Read<int>(7);


                    configuration.team_count = hoppersStream.Read<byte>(3);
                    configuration.minimum_team_size = hoppersStream.Read<byte>(3);
                    configuration.maximum_team_size = hoppersStream.Read<byte>(3);
                    configuration.maximum_team_imbalance = hoppersStream.Read<byte>(3);
                    configuration.big_squad_size_threshold = hoppersStream.Read<byte>(4);
                    configuration.maximum_big_squad_imbalance = hoppersStream.Read<byte>(3);
                    configuration.enable_big_squad_mixed_skill_restrictions = hoppersStream.Read<bool>(1);
                }
                else
                {
                    hoppersStream.Read<int>(8 + 2 + 8 + 8);


                    configuration.team_count = hoppersStream.Read<byte>(3);
                    configuration.minimum_team_size = hoppersStream.Read<byte>(3);
                    configuration.maximum_team_size = hoppersStream.Read<byte>(3);
                    configuration.allow_uneven_teams = hoppersStream.Read<bool>(1);
                    configuration.allow_parties_to_split = hoppersStream.Read<bool>(1);
                }

                res.configurations[i] = configuration;
            }

            return res;
        }
    }
}
