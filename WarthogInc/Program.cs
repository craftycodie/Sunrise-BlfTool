using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.IO;
using WarthogInc.BlfChunks;

namespace WarthogInc
{
    class Program
    {
        static void Main(string[] args)
        {
            //BitStream<StreamByteStream> hoppersIn = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream("C:\\Users\\codie\\Desktop\\hoppers", FileMode.Open)));

            //HopperConfigurationTable hoppers = new HopperReader().ReadHoppers(hoppersIn);

            //hoppers.categories[2].identifier = 10;
            //hoppers.categories[2].name = "Sunrise";
            //hoppers.categories[2].image = hoppers.categories[3].image;

            //hoppers.categories[3].identifier = 11;
            //hoppers.categories[3].name = "Sunrise Launch";
            //hoppers.categories[3].image += 1;

            //ulong startTime = 0;
            //ulong endTime = 0;

            //hoppers.configurations[15].startTime = startTime;
            //hoppers.configurations[16].startTime = startTime;
            //hoppers.configurations[22].startTime = startTime;
            //hoppers.configurations[24].startTime = startTime;

            //hoppers.configurations[15].endTime = endTime;
            //hoppers.configurations[16].endTime = endTime;
            //hoppers.configurations[22].endTime = endTime;
            //hoppers.configurations[24].endTime = endTime;

            //hoppers.configurations[16].require_all_party_members_meet_live_account_access_requirements = false;
            //hoppers.configurations[22].require_all_party_members_meet_live_account_access_requirements = false;
            //hoppers.configurations[24].require_all_party_members_meet_live_account_access_requirements = false;

            //hoppers.configurations[15].name = "Quick Match";
            //hoppers.configurations[15].minimum_player_count = 1;
            //hoppers.configurations[15].maximum_player_count = 15;
            //hoppers.configurations[15].type = 0;
            //hoppers.configurations[15].maximumPartySize = 15;
            //hoppers.configurations[15].identifier = 200;
            //hoppers.configurations[15].hashSet = hoppers.configurations[6].hashSet;
            //hoppers.configurations[15].guests_allowed = true;
            //hoppers.configurations[15].category = 10;
            //hoppers.configurations[15].xLastIndex = 200;
            //hoppers.configurations[15].richPresenceId = 78;
            //hoppers.configurations[15].experience_points_per_win = 1;

            //BitStream <StreamByteStream> hoppersOut = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream("C:\\Users\\codie\\Desktop\\hoppers_out", FileMode.OpenOrCreate)));
            //new HopperReader().WriteHoppers(hoppersOut, hoppers);

            var streamHelper = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream("C:\\Users\\codie\\Desktop\\descriptions", FileMode.Open)));
            HopperDescriptions descriptions = new HopperReader().ReadHopperDescriptions(streamHelper);

            descriptions.descriptions[27].description = "Sunrise Launch|r|nWelcome back to matchmaking, we're throwing a launch party!|r|nWinners receive double EXP points.      ";
            descriptions.descriptions[27].identifier = 11;

            Array.Resize(ref descriptions.descriptions, 29);

            descriptions.descriptionCount = 29;
            descriptions.descriptions[28] = new HopperDescriptions.HopperDescription();
            descriptions.descriptions[28].description = "Sunrise|r|nCelebrate the revival of Halo 3 with all new playlists!";
            descriptions.descriptions[28].identifier = 10;


            descriptions.descriptions[15].description = "<color argb=#FF8692d4>In a rush? You've got time for one more game right?|r|nFind some players and give em hell!</color>|r|nMax Party Size: 16";
            descriptions.descriptions[15].identifier = 200;

            BitStream<StreamByteStream> descriptionsOut = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream("C:\\Users\\codie\\Desktop\\descriptions_out", FileMode.OpenOrCreate)));
            new HopperReader().WriteHopperDescriptions(descriptionsOut, descriptions);

            File.Delete("C:\\Users\\codie\\Desktop\\user.bin");
            streamHelper = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream("C:\\Users\\codie\\Desktop\\user.bin", FileMode.CreateNew)));

            _blf blfFileHeader = new _blf();
            ServiceRecordIdentity srid = new ServiceRecordIdentity()
            {
                appearanceFlags = 0xFF,
                primaryColor = ServiceRecordIdentity.Color.SILVER,
                secondaryColor = ServiceRecordIdentity.Color.SILVER,
                tertiaryColor = ServiceRecordIdentity.Color.SILVER,
                isElite = ServiceRecordIdentity.PlayerModel.SPARTAN,
                foregroundEmblem = 2,
                backgroundEmblem = 2,
                emblemFlags = 0,
                playerName = "codietest",
                serviceTag = "CODIE",
                rank = ServiceRecordIdentity.Rank.GENERAL,
                grade = ServiceRecordIdentity.Grade.GRADE_4,
                totalEXP = 50000,
                campaignProgress = 5,
            };

            fupd fupd = new fupd()
            {
                bungieUserRole = 1,
                highestSkill = 1,
                unknown0 = 1,
                hopperDirectory = "default_hoppers"
            };

            BLFChunkWriter bLFChunkWriter = new BLFChunkWriter();
            bLFChunkWriter.WriteChunk(ref streamHelper, blfFileHeader);

            bLFChunkWriter.WriteChunk(ref streamHelper, srid);
            bLFChunkWriter.WriteChunk(ref streamHelper, fupd);
            bLFChunkWriter.WriteChunk(ref streamHelper, new _eof(blfFileHeader.GetLength() + srid.GetLength() + fupd.GetLength() + (0xC * 3)));
            streamHelper.Write(0, 8);


            var mhcf = JsonConvert.DeserializeObject<Sunrise.BlfTool.HopperConfigurationTable>(File.ReadAllText("../../../../json/matchmaking_hopper_011.json"));

            //hoppersIn.Seek(0);
            //File.Delete("C:\\Users\\codie\\Desktop\\matchmaking_hopper_011.bin");
            var hoppersOut = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream("../../../../blf/matchmaking_hopper_011.bin", FileMode.CreateNew)));
            //mhcf.configurations[4].name = "Hello, World!";
            bLFChunkWriter.WriteChunk(ref hoppersOut, blfFileHeader);
            bLFChunkWriter.WriteChunk(ref hoppersOut, mhcf);
            bLFChunkWriter.WriteChunk(ref hoppersOut, new _eof(blfFileHeader.GetLength() + mhcf.GetLength() + (0xC * 2)));
            hoppersOut.Write(0, 8);

            var setting = new JsonSerializerSettings { Converters = { new ByteArrayConverter(), new HexStringConverter() }, Formatting = Formatting.Indented };
            string output = JsonConvert.SerializeObject(mhcf, setting);

            File.WriteAllText("./out/matchmaking_hopper_011.json", output);
            //streamHelper.Write(0, 8);
        }
    }
}
