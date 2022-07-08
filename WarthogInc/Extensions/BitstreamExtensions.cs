using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;

namespace Sunrise.BlfTool.Extensions
{
    static class BitstreamExtensions
    {
        public static void WriteArray<T>(this BitStream<StreamByteStream> bitStream, T[] array, int bitLength)
        {
            foreach(T item in array)
            {
                bitStream.Write(item, bitLength);
            }
        }

        public static float ReadFloat(this BitStream<StreamByteStream> bitStream, int length)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(bitStream.Read<int>(length)), 0);
        }

        public static void WriteFloat(this BitStream<StreamByteStream> bitStream, float value, int length)
        {
            bitStream.Write(BitConverter.ToInt32(BitConverter.GetBytes(value)), length);
        }
    }
}
