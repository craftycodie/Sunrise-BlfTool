using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using Sunrise.BlfTool.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunrise.BlfTool.BlfChunks.GameEngineVariants
{
    public class PackedBaseGameVariant10 : IGameVariant
    {
        public VariantMetadata metadata;
        public bool builtIn; 
        public MiscellaneousOptions miscellaneousOptions;
        public RespawnOptions respawnOptions;
        public SocialOptions socialOptions;
        public MapOverrides mapOverrides;
        [JsonConverter(typeof(StringEnumConverter))]
        public TeamScoringMethod teamScoringMethod; // 3

        public enum Points : byte
        {
            ZERO_POINTS,
            ONE_POINT,
            TWO_POINTS,
            THREE_POINTS,
            FOUR_POINTS,
            FIVE_POINTS,
            SIX_POINTS,
            SEVEN_POINTS,
            EIGHT_POINTS,
            NINE_POINTS,
            TEN_POINTS,
            MINUS_TEN_POINTS = 22,
            MINUS_NINE_POINTS,
            MINUS_EIGHT_POINTS,
            MINUS_SEVEN_POINTS,
            MINUS_SIX_POINTS,
            MINUS_FIVE_POINTS,
            MINUS_FOUR_POINTS,
            MINUS_THREE_POINTS,
            MINUS_TWO_POINTS,
            MINUS_ONE_POINT,
        }

        public class VariantMetadata
        {
            public VariantMetadata() { }

            public VariantMetadata(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                uniqueId = hoppersStream.Read<long>(64);

                LinkedList<byte> nameBytes = new LinkedList<byte>();
                for (int i = 0; i < 16; i++)
                {
                    byte left = hoppersStream.Read<byte>(8);
                    byte right = hoppersStream.Read<byte>(8);
                    if (left == 0 && right == 0)
                    {
                        name = Encoding.BigEndianUnicode.GetString(nameBytes.ToArray());
                        break;
                    }
                    nameBytes.AddLast(left);
                    nameBytes.AddLast(right); 
                }

                //name = hoppersStream.ReadString(32, Encoding.BigEndianUnicode);
                description = hoppersStream.ReadString(128, Encoding.UTF8);
                author = hoppersStream.ReadString(16);
                fileType = (FileType)hoppersStream.Read<byte>(5);
                authorIsXuidOnline = hoppersStream.Read<byte>(1) > 0;
                authorXuid = hoppersStream.Read<ulong>(64);
                sizeInBytes = hoppersStream.Read<long>(64);
                date = hoppersStream.ReadDate(64);
                lengthSeconds = hoppersStream.Read<int>(32);
                campaignId = hoppersStream.Read<int>(32);
                mapId = hoppersStream.Read<int>(32);
                gameEngineType = (PackedGameVariant10.VariantGameEngine)hoppersStream.Read<byte>(4);
                campaignDifficulty = hoppersStream.Read<byte>(3);
                hopperId = hoppersStream.Read<short>(16);
                gameId = hoppersStream.Read<long>(64);
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.WriteLong(uniqueId, 64);
                hoppersStream.WriteString(name, 32, Encoding.BigEndianUnicode);
                hoppersStream.Write(0, 8);
                hoppersStream.WriteString(description, 128, Encoding.UTF8);
                hoppersStream.WriteString(author, 16);
                hoppersStream.Write((byte)fileType, 5);
                hoppersStream.Write(authorIsXuidOnline ? 1 : 0, 1);
                hoppersStream.WriteLong(authorXuid, 64);
                hoppersStream.WriteLong(sizeInBytes, 64);
                hoppersStream.WriteDate(date, 64);
                hoppersStream.Write(lengthSeconds, 32);
                hoppersStream.Write(campaignId, 32);
                hoppersStream.Write(mapId, 32);
                hoppersStream.Write((byte)gameEngineType, 4);
                hoppersStream.Write(campaignDifficulty, 3);
                hoppersStream.Write(hopperId, 16);
                hoppersStream.WriteLong(gameId, 64);
            }

            public long uniqueId;
            public string name;
            public string description;
            public string author;
            [JsonConverter(typeof(StringEnumConverter))]
            public FileType fileType;
            public bool authorIsXuidOnline;
            [JsonConverter(typeof(XUIDConverter))]
            public ulong authorXuid;
            public long sizeInBytes;
            [JsonConverter(typeof(IsoDateTimeConverter))]
            public DateTime date;
            public int lengthSeconds;
            public int campaignId;
            public int mapId;
            [JsonConverter(typeof(StringEnumConverter))]
            public PackedGameVariant10.VariantGameEngine gameEngineType;
            public byte campaignDifficulty;
            public short hopperId;
            public long gameId;

            public enum FileType : byte
            {
                GAME_VARIANT_CTF = 2,
                GAME_VARIANT_SLAYER,
                GAME_VARIANT_ODDBALL,
                GAME_VARIANT_KING,
                GAME_VARIANT_JUGGERNAUT,
                GAME_VARIANT_TERRITORIES,
                GAME_VARIANT_ASSAULT,
                GAME_VARIANT_INFECTION,
                GAME_VARIANT_VIP,
                MAP_VARIANT,
                FILM,
                FILM_CLIP,
                SCREENSHOT,
                INVALID

            }
        }

        public class PlayerTraits
        {
            public PlayerTraits() { }

            public PlayerTraits(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                damageResistance = (DamageResistance)hoppersStream.Read<byte>(4);
                shieldRechargeRate = (ShieldRechargeRate)hoppersStream.Read<byte>(4);
                vampirism = (Vampirism)hoppersStream.Read<byte>(3);
                headshotImmunity = (TraitBoolean)hoppersStream.Read<byte>(2);
                shieldMultiplier = (ShieldMultiplier)hoppersStream.Read<byte>(3);
                damageModifier = (DamageModifier)hoppersStream.Read<byte>(4);
                primaryWeapon = (Weapon)hoppersStream.Read<byte>(8);
                secondaryWeapon = (Weapon)hoppersStream.Read<byte>(8);
                grenadeCount = (GrenadeCount)hoppersStream.Read<byte>(2);
                infiniteAmmo = (TraitBoolean)hoppersStream.Read<byte>(2);
                rechargingGrenades = (TraitBoolean)hoppersStream.Read<byte>(2);
                weaponPickupAllowed = (TraitBoolean)hoppersStream.Read<byte>(2);
                playerSpeed = (PlayerSpeed)hoppersStream.Read<byte>(4);
                playerGravity = (PlayerGravity)hoppersStream.Read<byte>(3);
                vehicleUsage = (VehicleUsage)hoppersStream.Read<byte>(2);
                activeCamo = (ActiveCamo)hoppersStream.Read<byte>(3);
                waypoint = (PlayerWaypoint)hoppersStream.Read<byte>(2);
                playerAura = (PlayerAura)hoppersStream.Read<byte>(3);
                forcedColorChange = (ForcedColorChange)hoppersStream.Read<byte>(4);
                motionTacker = (MotionTacker)hoppersStream.Read<byte>(3);
                motionTrackerRange = (MotionTrackerRange)hoppersStream.Read<byte>(3);
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.Write((byte)damageResistance, 4);
                hoppersStream.Write((byte)shieldRechargeRate, 4);
                hoppersStream.Write((byte)vampirism, 3);
                hoppersStream.Write((byte)headshotImmunity, 2);
                hoppersStream.Write((byte)shieldMultiplier, 3);
                hoppersStream.Write((byte)damageModifier, 4);
                hoppersStream.Write((byte)primaryWeapon, 8);
                hoppersStream.Write((byte)secondaryWeapon, 8);
                hoppersStream.Write((byte)grenadeCount, 2);
                hoppersStream.Write((byte)infiniteAmmo, 2);
                hoppersStream.Write((byte)rechargingGrenades, 2);
                hoppersStream.Write((byte)weaponPickupAllowed, 2);
                hoppersStream.Write((byte)playerSpeed, 4);
                hoppersStream.Write((byte)playerGravity, 3);
                hoppersStream.Write((byte)vehicleUsage, 2);
                hoppersStream.Write((byte)activeCamo, 3);
                hoppersStream.Write((byte)waypoint, 2);
                hoppersStream.Write((byte)playerAura, 3);
                hoppersStream.Write((byte)forcedColorChange, 4);
                hoppersStream.Write((byte)motionTacker, 3);
                hoppersStream.Write((byte)motionTrackerRange, 3);
            }

            [JsonConverter(typeof(StringEnumConverter))]
            public DamageResistance damageResistance; // 4
            [JsonConverter(typeof(StringEnumConverter))]
            public ShieldRechargeRate shieldRechargeRate; // 4
            [JsonConverter(typeof(StringEnumConverter))]
            public Vampirism vampirism; // 3
            [JsonConverter(typeof(StringEnumConverter))]
            public TraitBoolean headshotImmunity; // 2
            [JsonConverter(typeof(StringEnumConverter))]
            public ShieldMultiplier shieldMultiplier; // 3
            [JsonConverter(typeof(StringEnumConverter))]
            public DamageModifier damageModifier; // 4
            [JsonConverter(typeof(StringEnumConverter))]
            public Weapon primaryWeapon;
            [JsonConverter(typeof(StringEnumConverter))]
            public Weapon secondaryWeapon;
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
                INVULNERABLE,
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
                PERCENT_200,
            }

            public enum ShieldMultiplier : byte
            {
                UNCHANGED,
                NO_SHIELDS,
                NORMAL_SHIELDS,
                TIMES_2,
                TIMES_3,
                TIMES_4,
            }

            public enum Vampirism : byte
            {
                UNCHANGED,
                DISABLED,
                PERCENT_10,
                PERCENT_25,
                PERCENT_50,
                PERCENT_100,
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
                INSTANT_KILL,
            }

            public enum Weapon : byte
            {
                BATTLE_RIFLE,
                ASSAULT_RIFLE,
                PLASMA_PISTOL,
                SPIKER,
                SMG,
                COVENANT_CARBINE,
                ENERGY_SWORD,
                MAGNUM,
                NEEDLER,
                PLASMA_RIFLE,
                ROCKET_LAUNCHER,
                SHOTGUN,
                SNIPER_RIFLE,
                BRUTE_SHOT,
                UNARMED,
                BEAM_RIFLE,
                SPARTAN_LASER,
                NONE,
                GRAVITY_HAMMER,
                MAULER,
                FLEMETHROWER,
                MISSILE_POD,
                RANDOM = 253,
                UNCHANGED = 254,
                MAP_DEFAULT = 255,
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
                PERCENT_300,
            }

            public enum PlayerGravity : byte
            {
                UNCHANGED,
                PERCENT_50,
                PERCENT_75,
                PERCENT_100,
                PERCENT_150,
                PERCENT_200,
            }

            public enum VehicleUsage : byte
            {
                UNCHANGED,
                NONE,
                PASSENGER_ONLY,
                FULL_USE,
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
                VISIBLE_TO_EVERYONE,
            }

            public enum PlayerAura : byte
            {
                UNCHANGED,
                OFF,
                TEAM_COLOR,
                BLACK,
                WHITE,
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
                ZOMBIE,
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
        }

        public class MiscellaneousOptions
        {
            public MiscellaneousOptions() { }

            public MiscellaneousOptions(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                teams = hoppersStream.Read<byte>(1) > 0;
                roundResetPlayers = hoppersStream.Read<byte>(1) > 0;
                roundResetMap = hoppersStream.Read<byte>(1) > 0;
                roundTimeLimitMinutes = hoppersStream.Read<byte>(8);
                roundLimit = hoppersStream.Read<byte>(4);
                earlyVictoryWinCount = hoppersStream.Read<byte>(4);
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.Write(teams ? 1 : 0, 1);
                hoppersStream.Write(roundResetPlayers ? 1 : 0, 1);
                hoppersStream.Write(roundResetMap ? 1 : 0, 1);
                hoppersStream.Write(roundTimeLimitMinutes, 8);
                hoppersStream.Write(roundLimit, 4);
                hoppersStream.Write(earlyVictoryWinCount, 4);
            }

            public bool teams;
            public bool roundResetPlayers;
            public bool roundResetMap;
            public byte roundTimeLimitMinutes;
            public byte roundLimit; // 4
            public byte earlyVictoryWinCount; // 4
        }

        public class RespawnOptions
        {
            public RespawnOptions() { }

            public RespawnOptions(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                inheritRespawnTime = hoppersStream.Read<byte>(1) > 0;
                respawnWithTeammate = hoppersStream.Read<byte>(1) > 0;
                respawnAtLocation = hoppersStream.Read<byte>(1) > 0;
                respawnOnKills = hoppersStream.Read<byte>(1) > 0;
                livesPerRound = hoppersStream.Read<byte>(6);
                teamLivesPerRound = hoppersStream.Read<byte>(7);
                respawnTime = hoppersStream.Read<byte>(8);
                suicideTime = hoppersStream.Read<byte>(8);
                betrayalTime = hoppersStream.Read<byte>(8);
                respawnGrowthTime = hoppersStream.Read<byte>(4);
                respawnOptionsPlayerTraitsDuration = hoppersStream.Read<byte>(6);
                respawnTraits = new PlayerTraits(ref hoppersStream);
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.Write(inheritRespawnTime ? 1 : 0, 1);
                hoppersStream.Write(respawnWithTeammate ? 1 : 0, 1);
                hoppersStream.Write(respawnAtLocation ? 1 : 0, 1);
                hoppersStream.Write(respawnOnKills ? 1 : 0, 1);
                hoppersStream.Write(livesPerRound, 6);
                hoppersStream.Write(teamLivesPerRound, 7);
                hoppersStream.Write(respawnTime, 8);
                hoppersStream.Write(suicideTime, 8);
                hoppersStream.Write(betrayalTime, 8);
                hoppersStream.Write(respawnGrowthTime, 4);
                hoppersStream.Write(respawnOptionsPlayerTraitsDuration, 6);
                respawnTraits.Write(ref hoppersStream);
            }

            public bool inheritRespawnTime;
            public bool respawnWithTeammate;
            public bool respawnAtLocation;
            public bool respawnOnKills;
            public byte livesPerRound; // 6
            public byte teamLivesPerRound; // 7
            public byte respawnTime;
            public byte suicideTime;
            public byte betrayalTime;
            public byte respawnGrowthTime; // 4
            public byte respawnOptionsPlayerTraitsDuration; // 6
            public PlayerTraits respawnTraits;
        }

        public class SocialOptions
        {
            public SocialOptions() { }

            public SocialOptions(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                observers = hoppersStream.Read<byte>(1) > 0;
                teamChanging = hoppersStream.Read<byte>(2);
                friendlyFire = hoppersStream.Read<byte>(1) > 0;
                betrayalBooting = hoppersStream.Read<byte>(1) > 0;
                enemyVoice = hoppersStream.Read<byte>(1) > 0;
                openChannelVoice = hoppersStream.Read<byte>(1) > 0;
                deadPlayerVoice = hoppersStream.Read<byte>(1) > 0;
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.Write(observers ? 1 : 0, 1);
                hoppersStream.Write(teamChanging, 2);
                hoppersStream.Write(friendlyFire ? 1 : 0, 1);
                hoppersStream.Write(betrayalBooting ? 1 : 0, 1);
                hoppersStream.Write(enemyVoice ? 1 : 0, 1);
                hoppersStream.Write(openChannelVoice ? 1 : 0, 1);
                hoppersStream.Write(deadPlayerVoice ? 1 : 0, 1);
            }

            public bool observers;
            public byte teamChanging; // 2
            public bool friendlyFire;
            public bool betrayalBooting;
            public bool enemyVoice;
            public bool openChannelVoice;
            public bool deadPlayerVoice;
        }

        public enum TeamScoringMethod : byte
        {
            SUM_OF_TEAM,
            MINIMUM_SCORE,
            MAXIMUM_SCORE,
        }

        public class MapOverrides
        {
            public MapOverrides() { }

            public MapOverrides(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                grenadesOnMap = hoppersStream.Read<byte>(1) > 0;
                indestructibleVehicles = hoppersStream.Read<byte>(1) > 0;
                baseTraits = new PlayerTraits(ref hoppersStream);
                weaponSet = (WeaponSet)hoppersStream.Read<byte>(8);
                vehicleSet = (VehicleSet)hoppersStream.Read<byte>(8);
                redPowerupTraits = new PlayerTraits(ref hoppersStream);
                bluePowerupTraits = new PlayerTraits(ref hoppersStream);
                yellowPowerupTraits = new PlayerTraits(ref hoppersStream);
                redPowerupDuration = hoppersStream.Read<byte>(7);
                bluePowerupDuration = hoppersStream.Read<byte>(7);
                yellowPowerupDuration = hoppersStream.Read<byte>(7);
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.Write(grenadesOnMap ? 1 : 0, 1);
                hoppersStream.Write(indestructibleVehicles ? 1 : 0, 1);
                baseTraits.Write(ref hoppersStream);
                hoppersStream.Write((byte)weaponSet, 8);
                hoppersStream.Write((byte)vehicleSet, 8);
                redPowerupTraits.Write(ref hoppersStream);
                bluePowerupTraits.Write(ref hoppersStream);
                yellowPowerupTraits.Write(ref hoppersStream);
                hoppersStream.Write(redPowerupDuration, 7);
                hoppersStream.Write(bluePowerupDuration, 7);
                hoppersStream.Write(yellowPowerupDuration, 7);
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
            public byte redPowerupDuration; // 7
            public byte bluePowerupDuration; // 7
            public byte yellowPowerupDuration; // 7

            public enum WeaponSet : byte
            {
                ASSAULT_RIFLES,
                DUALS,
                GRAVITY_HAMMERS,
                SPARTAN_LASER,
                ROCKET_LAUNCHERS,
                SHOTGUNS,
                SNIPER_RIFLES,
                ENERGY_SWORDS,
                RANDOM,
                NO_POWER_WEAPONS,
                NO_SNIPERS,
                NO_WEAPONS,
                MAP_DEFAULT = 255,
            }

            public enum VehicleSet : byte
            {
                NO_VEHICLES = 1,
                MONGOOSES_ONLY,
                LIGHT_GROUND_ONLY,
                TANKS_ONLY,
                AIRCRAFT_ONLY,
                NO_LIGHT_GROUND,
                NO_TANKS,
                NO_AIRCRAFT,
                ALL_VEHICLES,
                MAP_DEFAULT = 255,
            }
        }

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
            hoppersStream.Write(builtIn ? 1 : 0, 1);
            miscellaneousOptions.Write(ref hoppersStream);
            respawnOptions.Write(ref hoppersStream);
            socialOptions.Write(ref hoppersStream);
            mapOverrides.Write(ref hoppersStream);
            hoppersStream.Write((byte)teamScoringMethod, 3);
        }
    }
}
