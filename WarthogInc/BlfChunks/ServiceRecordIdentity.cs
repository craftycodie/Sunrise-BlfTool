﻿using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarthogInc.BlfChunks
{
    class ServiceRecordIdentity : IBLFChunk
    {
        public ushort GetAuthentication()
        {
            return 1;
        }

        public string GetName()
        {
            return "srid";
        }

        public ushort GetVersion()
        {
            return 2;
        }

        public uint GetLength()
        {
            return 0x5C;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public enum PlayerModel : byte
        {
            SPARTAN,
            ELITE
        }

        public enum SpartanHelmet : byte
        {
            DEFAULT,
            COBRA,
            INTRUDER,
            NINJA, // recon
            REGULATOR,
            RYU, // hyabusa
            MARATHON,
            SCOUT,
            ODST,
            MARKV,
            ROGUE,
        }

        public enum SpartanShoulder : byte
        {
            DEFAULT,
            COBRA,
            INTRUDER,
            NINJA, // recon
            REGULATOR,
            RYU, // hyabusa
            MARATHON,
            SCOUT
        }

        public enum SpartanBody : byte
        {
            DEFAULT,
            COBRA,
            INTRUDER,
            NINJA, // recon
            RYU, // hyabusa
            REGULATOR,
            SCOUT,
            KATANA,
            BUNGIE
        }

        public enum EliteArmour : byte
        {
            DEFAULT,
            PREDATOR,
            RAPTOR,
            BLADES,
            SCYTHE,
        }

        public enum Rank : int
        {
            RECRUIT,
            APPRENTICE,
            PRIVATE,
            CORPORAL,
            SERGEANT,
            GUNNERY_SERGEANT,
            LIEUTENANT,
            CAPTAIN,
            MAJOR,
            COMMANDER,
            COLNEL,
            BRIGADIER,
            GENERAL
        }

        public enum Grade : int
        {
            GRADE_1,
            GRADE_2,
            GRADE_3,
            GRADE_4,
        }

        public enum Color : byte
        {
            STEEL,
            SILVER,
            WHITE,
            RED,
            MAUVE,
            SALMON,
            ORANGE,
            CORAL,
            PEACH,
            GOLD,
            YELLOW,
            PALE,
            SAGE,
            GREEN,
            OLIVE,
            TEAL,
            AQUA,
            CYAN,
            BLUE,
            COBALT,
            SAPPHIRE,
            VIOLET,
            ORCHID,
            LAVENDER,
            CRIMSON,
            RUBY_WINE,
            PINK,
            BROWN,
            RAN,
            KHAKI
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            byte[] playerNameOut = Encoding.BigEndianUnicode.GetBytes(playerName);

            for (int i = 0; i < 32; i++)
            {
                if (i < playerNameOut.Length)
                    hoppersStream.Write(playerNameOut[i], 8);
                else
                    hoppersStream.Write(0, 8);
            }

            //hoppersStream.WriteString(playerName, 32, Encoding.Unicode);
            hoppersStream.Write(appearanceFlags, 8);
            hoppersStream.Write((byte)primaryColor, 8);
            hoppersStream.Write((byte)secondaryColor, 8);
            hoppersStream.Write((byte)tertiaryColor, 8);
            hoppersStream.Write((byte)isElite, 8);
            hoppersStream.Write(foregroundEmblem, 8);
            hoppersStream.Write(backgroundEmblem, 8);
            hoppersStream.Write(emblemFlags, 8);
            hoppersStream.Write(0, 8);
            hoppersStream.Write((byte)emblemPrimaryColor, 8);
            hoppersStream.Write((byte)emblemSecondaryColor, 8);
            hoppersStream.Write((byte)emblemBackgroundColor, 8);
            hoppersStream.Write(0, 16);
            hoppersStream.Write((byte)spartanHelmet, 8);
            hoppersStream.Write((byte)spartanLeftShounder, 8);
            hoppersStream.Write((byte)spartanRightShoulder, 8);
            hoppersStream.Write((byte)spartanBody, 8);
            hoppersStream.Write((byte)eliteHelmet, 8);
            hoppersStream.Write((byte)eliteLeftShoulder, 8);
            hoppersStream.Write((byte)eliteRightShoulder, 8);
            hoppersStream.Write((byte)eliteBody, 8);
            //hoppersStream.WriteString(serviceTag, 10, Encoding.BigEndianUnicode);

            byte[] serviceTagOut = Encoding.BigEndianUnicode.GetBytes(playerName);

            for (int i = 0; i < 10; i++)
            {
                if (i < serviceTagOut.Length)
                    hoppersStream.Write(serviceTagOut[i], 8);
                else
                    hoppersStream.Write(0, 8);
            }

            hoppersStream.Write(campaignProgress, 32);
            hoppersStream.Write(highestSkill, 32);
            hoppersStream.Write(totalEXP, 32);
            hoppersStream.Write(unknownInsignia, 32);
            hoppersStream.Write((byte)rank, 32);
            hoppersStream.Write((byte)grade, 32);
            hoppersStream.Write(unknownInsignia2, 32);
        }

        public string playerName; // wide, 16 chars
        public byte appearanceFlags; // includes gender i think
        public Color primaryColor;
        public Color secondaryColor;
        public Color tertiaryColor;
        public PlayerModel isElite;
        public byte foregroundEmblem;
        public byte backgroundEmblem;
        public byte emblemFlags; // Whether the secondary layer is shown or not.
        public Color emblemPrimaryColor;
        public Color emblemSecondaryColor;
        public Color emblemBackgroundColor;
        public SpartanHelmet spartanHelmet;
        public SpartanShoulder spartanLeftShounder;
        public SpartanShoulder spartanRightShoulder;
        public SpartanBody spartanBody;
        public EliteArmour eliteHelmet;
        public EliteArmour eliteLeftShoulder;
        public EliteArmour eliteRightShoulder;
        public EliteArmour eliteBody;
        public string serviceTag; // wide, 5 characters long for some reason
        public int campaignProgress;
        public int highestSkill;
        public int totalEXP;
        public int unknownInsignia;
        public Rank rank;
        public Grade grade;
        public int unknownInsignia2;
    }
}
