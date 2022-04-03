using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc
{
    class Hoppers
    {
        public byte categoryCount;
        public HopperCategory[] categories;
        public byte configurationsCount;
        public HopperConfiguration[] configurations;


        public class HopperCategory
        {
            public short identifier;
            public byte image;
            public string name;
        }

        public class HopperConfiguration
        {
            public string name;
            public string hashSet;
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
            public byte chance_of_gathering;
            public byte experience_points_per_win;
            public byte experience_penalty_per_drop;
            public uint minimum_mu_per_level;
            public byte maximum_skill_level_match_delta;
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
