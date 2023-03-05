using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.BlfTool.BlfChunks.GameEngineVariants
{
    public class PackedForgeGameVariant : PackedBaseGameVariant10
    {
        public PackedForgeGameVariant() { }

        public PackedForgeGameVariant(ref BitStream<StreamByteStream> hoppersStream)
        {
            Read(ref hoppersStream);
        }

        public bool openChannelVoice;
        public byte editMode; // 2
        public byte respawnTime; // 6
        public PlayerTraits monitorTraits;

        public void Read(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Read(ref hoppersStream);
            openChannelVoice = hoppersStream.Read<byte>(1) > 0;
            editMode = hoppersStream.Read<byte>(2);
            respawnTime = hoppersStream.Read<byte>(6);
            monitorTraits = new PlayerTraits(ref hoppersStream);
        }

        public void Write(ref BitStream<StreamByteStream> hoppersStream)
        {
            base.Write(ref hoppersStream);
            hoppersStream.Write(openChannelVoice ? 1 : 0, 1);
            hoppersStream.Write(editMode, 2);
            hoppersStream.Write(respawnTime, 6);
            monitorTraits.Write(ref hoppersStream);
        }
    }
}
