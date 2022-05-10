using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.BlfTool.Extensions
{
    static class BitstreamExtensions
    {
        public static void WriteArray<T>(this BitStream<StreamByteStream> bitStream, T[] array, int bitLength)
        {
            foreach(T item in array)
            {
                bitStream.Write(item, bitLength);
                bitStream.SeekRelative((int)Math.Floor((decimal)(bitLength / 8)), (byte)(bitLength % 8));
            }
        }
    }
}
