using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using SunriseBlfTool.BlfChunks;
using SunriseBlfTool.BlfChunks.GameEngineVariants;
using SunriseBlfTool.Extensions;
using SunriseBlfTool.BlfChunks;
using SunriseBlfTool.Extensions;

namespace SunriseBlfTool.BlfChunks.GameVariants.PackedV2
{
    public class PackedBaseGameVariant2 : IGameVariant
    {
        public enum Points : byte
        {
            ZERO_POINTS = 0,
            ONE_POINT = 1,
            TWO_POINTS = 2,
            THREE_POINTS = 3,
            FOUR_POINTS = 4,
            FIVE_POINTS = 5,
            SIX_POINTS = 6,
            SEVEN_POINTS = 7,
            EIGHT_POINTS = 8,
            NINE_POINTS = 9,
            TEN_POINTS = 10,
            MINUS_TEN_POINTS = 22,
            MINUS_NINE_POINTS = 23,
            MINUS_EIGHT_POINTS = 24,
            MINUS_SEVEN_POINTS = 25,
            MINUS_SIX_POINTS = 26,
            MINUS_FIVE_POINTS = 27,
            MINUS_FOUR_POINTS = 28,
            MINUS_THREE_POINTS = 29,
            MINUS_TWO_POINTS = 30,
            MINUS_ONE_POINT = 31
        }

        public class VariantMetadata
        {
            public string name;

            public string description;

            public VariantMetadata()
            {
            }

            public VariantMetadata(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                throw new NotImplementedException();
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.WriteBitswappedString(name, 32, Encoding.BigEndianUnicode);
                hoppersStream.WriteBitswappedString(description, 32, Encoding.BigEndianUnicode);
            }
        }

        public class PlayerTraits
        {
            public enum DamageResistance : byte
            {
                UNCHANGED,
                PERCENT_10,
                PERCENT_50,
                PERCENT_90,
                NORMAL,
                PERCENT_110,
                PERCENT_150,
                PERCENT_200,
                PERCENT_300,
                PERCENT_500,
                PERCENT_1000,
                PERCENT_2000,
                INVULNERABLE
            }

            public enum ShieldRechargeRate : byte
            {
                UNCHANGED,
                NEGATIVE_25_PERCENT,
                NEGATIVE_10_PERCENT,
                NEGATIVE_5_PERCENT,
                NO_RECHARGE,
                PERCENT_50,
                PERCENT_90,
                PERCENT_100,
                PERCENT_110,
                PERCENT_200
            }

            public enum ShieldMultiplier : byte
            {
                UNCHANGED,
                NO_SHIELDS,
                NORMAL_SHIELDS,
                TIMES_2,
                TIMES_3,
                TIMES_4
            }

            public enum Vampirism : byte
            {
                UNCHANGED,
                DISABLED,
                PERCENT_10,
                PERCENT_25,
                PERCENT_50,
                PERCENT_100
            }

            public enum DamageModifier : byte
            {
                UNCHANGED,
                PERCENT_0,
                PERCENT_25,
                PERCENT_50,
                PERCENT_75,
                PERCENT_90,
                PERCENT_100,
                PERCENT_110,
                PERCENT_125,
                PERCEMT_150,
                PERCENT_200,
                PERCENT_300,
                INSTANT_KILL
            }

            public enum Weapon : byte
            {
                BATTLE_RIFLE = 0,
                ASSAULT_RIFLE = 1,
                PLASMA_PISTOL = 2,
                SPIKER = 3,
                SMG = 4,
                COVENANT_CARBINE = 5,
                ENERGY_SWORD = 6,
                MAGNUM = 7,
                MISSILE_POD = 8,
                NEEDLER = 9,
                PLASMA_RIFLE = 10,
                ROCKET_LAUNCHER = 11,
                SHOTGUN = 12,
                SNIPER_RIFLE = 13,
                BRUTE_SHOT = 14,
                ENERGY_SWORD_USELESS = 15,
                BEAM_RIFLE = 16,
                SPARTAN_LASER = 17,
                NONE = 18,
                RANDOM = 254,
                UNCHANGED = byte.MaxValue,
                MAP_DEFAULT = byte.MaxValue
            }

            public enum GrenadeCount : byte
            {
                UNCHANGED,
                MAP_DEFAULT,
                NONE
            }

            public enum TraitBoolean : byte
            {
                UNCHANGED,
                DISABLED,
                ENABLED
            }

            public enum PlayerSpeed : byte
            {
                UNCHANGED,
                PERCENT_25,
                PERCENT_50,
                PERCENT_75,
                PERCENT_90,
                PERCENT_100,
                PERCENT_110,
                PERCENT_125,
                PERCENT_150,
                PERCENT_200,
                PERCENT_300
            }

            public enum PlayerGravity : byte
            {
                UNCHANGED,
                PERCENT_50,
                PERCENT_75,
                PERCENT_100,
                PERCENT_150,
                PERCENT_200
            }

            public enum VehicleUsage : byte
            {
                UNCHANGED,
                NONE,
                PASSENGER_ONLY,
                FULL_USE
            }

            public enum ActiveCamo : byte
            {
                UNCHANGED,
                OFF,
                VERY_POOR,
                POOR,
                GOOD
            }

            public enum PlayerWaypoint : byte
            {
                UNCHANGED,
                NO_WAYPOINT,
                VISIBLE_TO_ALLIES,
                VISIBLE_TO_EVERYONE
            }

            public enum PlayerAura : byte
            {
                UNCHANGED,
                OFF,
                TEAM_COLOR,
                BLACK,
                WHITE
            }

            public enum ForcedColorChange : byte
            {
                UNCHANGED,
                OFF,
                RED,
                BLUE,
                GREEN,
                ORANGE,
                PURPLE,
                GOLD,
                BROWN,
                PINK,
                WHITE,
                BLACK,
                ZOMBIE
            }

            public enum MotionTacker : byte
            {
                UNCHANGED,
                OFF,
                ALLIES_ONLY,
                NORMAL,
                ENHANCED
            }

            public enum MotionTrackerRange : byte
            {
                UNCHANGED,
                METERS_10,
                METERS_15,
                METERS_25,
                METERS_50,
                METERS_75,
                METERS_100,
                METERS_150
            }

            [JsonConverter(typeof(StringEnumConverter))]
            public DamageResistance damageResistance;

            [JsonConverter(typeof(StringEnumConverter))]
            public ShieldRechargeRate shieldRechargeRate;

            [JsonConverter(typeof(StringEnumConverter))]
            public Vampirism vampirism;

            [JsonConverter(typeof(StringEnumConverter))]
            public TraitBoolean headshotImmunity;

            [JsonConverter(typeof(StringEnumConverter))]
            public ShieldMultiplier shieldMultiplier;

            [JsonConverter(typeof(StringEnumConverter))]
            public DamageModifier damageModifier;

            [JsonConverter(typeof(StringEnumConverter))]
            public Weapon primaryWeapon;

            [JsonConverter(typeof(StringEnumConverter))]
            public Weapon secondaryWeapon;

            public byte grenadeType;

            [JsonConverter(typeof(StringEnumConverter))]
            public GrenadeCount grenadeCount;

            [JsonConverter(typeof(StringEnumConverter))]
            public TraitBoolean infiniteAmmo;

            [JsonConverter(typeof(StringEnumConverter))]
            public TraitBoolean rechargingGrenades;

            [JsonConverter(typeof(StringEnumConverter))]
            public TraitBoolean weaponPickupAllowed;

            [JsonConverter(typeof(StringEnumConverter))]
            public PlayerSpeed playerSpeed;

            [JsonConverter(typeof(StringEnumConverter))]
            public PlayerGravity playerGravity;

            [JsonConverter(typeof(StringEnumConverter))]
            public VehicleUsage vehicleUsage;

            [JsonConverter(typeof(StringEnumConverter))]
            public ActiveCamo activeCamo;

            [JsonConverter(typeof(StringEnumConverter))]
            public PlayerWaypoint waypoint;

            [JsonConverter(typeof(StringEnumConverter))]
            public PlayerAura playerAura;

            [JsonConverter(typeof(StringEnumConverter))]
            public ForcedColorChange forcedColorChange;

            [JsonConverter(typeof(StringEnumConverter))]
            public MotionTacker motionTacker;

            [JsonConverter(typeof(StringEnumConverter))]
            public MotionTrackerRange motionTrackerRange;

            public PlayerTraits()
            {
            }

            public PlayerTraits(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                throw new NotImplementedException();
            }

            public void Inherit(PlayerTraits from)
            {
                if (damageResistance == DamageResistance.UNCHANGED)
                {
                    damageResistance = from.damageResistance;
                }
                if (shieldRechargeRate == ShieldRechargeRate.UNCHANGED)
                {
                    shieldRechargeRate = from.shieldRechargeRate;
                }
                if (vampirism == Vampirism.UNCHANGED)
                {
                    vampirism = from.vampirism;
                }
                if (headshotImmunity == TraitBoolean.UNCHANGED)
                {
                    headshotImmunity = from.headshotImmunity;
                }
                if (shieldMultiplier == ShieldMultiplier.UNCHANGED)
                {
                    shieldMultiplier = from.shieldMultiplier;
                }
                if (damageModifier == DamageModifier.UNCHANGED)
                {
                    damageModifier = from.damageModifier;
                }
                if (primaryWeapon == Weapon.UNCHANGED)
                {
                    primaryWeapon = from.primaryWeapon;
                }
                if (secondaryWeapon == Weapon.UNCHANGED)
                {
                    secondaryWeapon = from.secondaryWeapon;
                }
                if (grenadeCount == GrenadeCount.UNCHANGED)
                {
                    grenadeCount = from.grenadeCount;
                }
                if (infiniteAmmo == TraitBoolean.UNCHANGED)
                {
                    infiniteAmmo = from.infiniteAmmo;
                }
                if (rechargingGrenades == TraitBoolean.UNCHANGED)
                {
                    rechargingGrenades = from.rechargingGrenades;
                }
                if (weaponPickupAllowed == TraitBoolean.UNCHANGED)
                {
                    weaponPickupAllowed = from.weaponPickupAllowed;
                }
                if (playerSpeed == PlayerSpeed.UNCHANGED)
                {
                    playerSpeed = from.playerSpeed;
                }
                if (playerGravity == PlayerGravity.UNCHANGED)
                {
                    playerGravity = from.playerGravity;
                }
                if (vehicleUsage == VehicleUsage.UNCHANGED)
                {
                    vehicleUsage = from.vehicleUsage;
                }
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.WriteBitswapped((byte)damageResistance, 4);
                hoppersStream.WriteBitswapped((byte)shieldRechargeRate, 4);
                hoppersStream.WriteBitswapped((byte)vampirism, 3);
                hoppersStream.WriteBitswapped((byte)headshotImmunity, 2);
                hoppersStream.WriteBitswapped((byte)shieldMultiplier, 3);
                hoppersStream.WriteBitswapped((byte)damageModifier, 4);
                hoppersStream.WriteBitswapped((byte)primaryWeapon, 8);
                hoppersStream.WriteBitswapped((byte)secondaryWeapon, 8);
                hoppersStream.WriteBitswapped(grenadeType, 8);
                hoppersStream.WriteBitswapped((byte)grenadeCount, 3);
                hoppersStream.WriteBitswapped((byte)infiniteAmmo, 2);
                hoppersStream.WriteBitswapped((byte)rechargingGrenades, 2);
                hoppersStream.WriteBitswapped((byte)weaponPickupAllowed, 2);
                hoppersStream.WriteBitswapped((byte)playerSpeed, 4);
                hoppersStream.WriteBitswapped((byte)playerGravity, 3);
                hoppersStream.WriteBitswapped((byte)vehicleUsage, 2);
            }
        }

        public class MiscellaneousOptions
        {
            public bool teams;

            public bool roundResetPlayers;

            public bool roundResetMap;

            public byte roundTimeLimitMinutes;

            public byte roundLimit;

            public byte earlyVictoryWinCount;

            public MiscellaneousOptions()
            {
            }

            public MiscellaneousOptions(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                throw new NotImplementedException();
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.WriteBitswapped(teams ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(roundResetPlayers ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(roundResetMap ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(roundTimeLimitMinutes, 8);
                hoppersStream.WriteBitswapped(roundLimit, 4);
                hoppersStream.WriteBitswapped(earlyVictoryWinCount, 4);
            }
        }

        public class RespawnOptions
        {
            public bool inheritRespawnTime;

            public bool respawnWithTeammate;

            public bool respawnAtLocation;

            public bool respawnOnKills;

            public byte livesPerRound;

            public byte teamLivesPerRound;

            public byte respawnTime;

            public byte suicideTime;

            public byte betrayalTime;

            public byte respawnGrowthTime;

            public byte respawnOptionsPlayerTraitsDuration;

            public PlayerTraits respawnTraits;

            public RespawnOptions()
            {
            }

            public RespawnOptions(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                throw new NotImplementedException();
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.WriteBitswapped(inheritRespawnTime ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(respawnWithTeammate ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(respawnAtLocation ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(respawnOnKills ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(livesPerRound, 6);
                hoppersStream.WriteBitswapped(teamLivesPerRound, 7);
                hoppersStream.WriteBitswapped(respawnTime, 8);
                hoppersStream.WriteBitswapped(suicideTime, 8);
                hoppersStream.WriteBitswapped(betrayalTime, 8);
                hoppersStream.WriteBitswapped(respawnGrowthTime, 4);
                hoppersStream.WriteBitswapped(respawnOptionsPlayerTraitsDuration, 6);
                respawnTraits.Write(ref hoppersStream);
            }
        }

        public class SocialOptions
        {
            public bool observers;

            public bool teamChanging;

            public bool teamChangingBalancingOnly;

            public bool friendlyFire;

            public bool betrayalBooting;

            public bool enemyVoice;

            public bool openChannelVoice;

            public bool deadPlayerVoice;

            public SocialOptions()
            {
            }

            public SocialOptions(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                throw new NotImplementedException();
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.WriteBitswapped(observers ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(teamChanging ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(teamChangingBalancingOnly ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(friendlyFire ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(betrayalBooting ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(enemyVoice ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(openChannelVoice ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(deadPlayerVoice ? 1 : 0, 1);
            }
        }

        public enum TeamScoringMethod : byte
        {
            SUM_OF_TEAM,
            MINIMUM_SCORE,
            MAXIMUM_SCORE
        }

        public class MapOverrides
        {
            public enum WeaponSet : byte
            {
                ASSAULT_RIFLES = 0,
                DUALS = 1,
                GRAVITY_HAMMERS = 2,
                SPARTAN_LASER = 3,
                ROCKET_LAUNCHERS = 4,
                SHOTGUNS = 5,
                SNIPER_RIFLES = 6,
                ENERGY_SWORDS = 7,
                RANDOM = 8,
                NO_POWER_WEAPONS = 9,
                NO_SNIPERS = 10,
                NO_WEAPONS = 11,
                MAP_DEFAULT = byte.MaxValue
            }

            public enum VehicleSet : byte
            {
                NO_VEHICLES = 1,
                MONGOOSES_ONLY = 2,
                LIGHT_GROUND_ONLY = 3,
                TANKS_ONLY = 4,
                AIRCRAFT_ONLY = 5,
                NO_LIGHT_GROUND = 6,
                NO_TANKS = 7,
                NO_AIRCRAFT = 8,
                ALL_VEHICLES = 9,
                MAP_DEFAULT = byte.MaxValue
            }

            public bool grenadesOnMap;

            public bool indestructibleVehicles;

            public PlayerTraits baseTraits;

            [JsonConverter(typeof(StringEnumConverter))]
            public WeaponSet weaponSet;

            [JsonConverter(typeof(StringEnumConverter))]
            public VehicleSet vehicleSet;

            public PlayerTraits redPowerupTraits;

            public PlayerTraits bluePowerupTraits;

            public PlayerTraits yellowPowerupTraits;

            public byte redPowerupDuration;

            public byte bluePowerupDuration;

            public byte yellowPowerupDuration;

            public MapOverrides()
            {
            }

            public MapOverrides(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                throw new NotImplementedException();
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.WriteBitswapped(grenadesOnMap ? 1 : 0, 1);
                hoppersStream.WriteBitswapped(indestructibleVehicles ? 1 : 0, 1);
                baseTraits.Write(ref hoppersStream);
                hoppersStream.WriteBitswapped((byte)weaponSet, 8);
                hoppersStream.WriteBitswapped((byte)vehicleSet, 8);
                redPowerupTraits.Write(ref hoppersStream);
                bluePowerupTraits.Write(ref hoppersStream);
                yellowPowerupTraits.Write(ref hoppersStream);
                hoppersStream.WriteBitswapped(redPowerupDuration, 7);
                hoppersStream.WriteBitswapped(bluePowerupDuration, 7);
                hoppersStream.WriteBitswapped(yellowPowerupDuration, 7);
            }
        }

        public VariantMetadata metadata;

        public bool builtIn;

        public MiscellaneousOptions miscellaneousOptions;

        public RespawnOptions respawnOptions;

        public SocialOptions socialOptions;

        public MapOverrides mapOverrides;

        [JsonConverter(typeof(StringEnumConverter))]
        public TeamScoringMethod teamScoringMethod;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            metadata = new VariantMetadata(ref hoppersStream);
            builtIn = hoppersStream.Read<byte>(1) > 0;
            miscellaneousOptions = new MiscellaneousOptions(ref hoppersStream);
            respawnOptions = new RespawnOptions(ref hoppersStream);
            socialOptions = new SocialOptions(ref hoppersStream);
            mapOverrides = new MapOverrides(ref hoppersStream);
            teamScoringMethod = (TeamScoringMethod)hoppersStream.Read<byte>(3);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            metadata.Write(ref hoppersStream);
            miscellaneousOptions.Write(ref hoppersStream);
            respawnOptions.Write(ref hoppersStream);
            socialOptions.Write(ref hoppersStream);
            mapOverrides.Write(ref hoppersStream);
        }
    }
}