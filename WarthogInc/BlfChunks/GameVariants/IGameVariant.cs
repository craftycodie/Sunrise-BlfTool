using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;

namespace Sunrise.BlfTool.BlfChunks
{
    interface IGameVariant
    {
        public void Write(ref BitStream<StreamByteStream> hoppersStream);
        public void Read(ref BitStream<StreamByteStream> hoppersStream);
    }
}
