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

        public static float ReadFloat(this ref BitStream<StreamByteStream> bitStream, int length)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(bitStream.Read<int>(length)), 0);
        }

        public static void WriteFloat(this ref BitStream<StreamByteStream> bitStream, float value, int length)
        {
            bitStream.Write(BitConverter.ToInt32(BitConverter.GetBytes(value)), length);
        }

        static uint[] long2doubleInt(ulong a)
        {
            uint a1 = (uint)(a & uint.MaxValue);
            uint a2 = (uint)(a >> 32);
            return new uint[] { a1, a2 };
        }

        static int[] long2doubleInt(long a)
        {
            int a1 = (int)(a & int.MaxValue);
            int a2 = (int)(a >> 32);
            return new int[] { a1, a2 };
        }

        public static void WriteLong(this ref BitStream<StreamByteStream> bitStream, long value, int length)
        {
            if (length != 64)
                throw new NotImplementedException();

            bitStream.Write(long2doubleInt(value)[1], 32);
            bitStream.Write(long2doubleInt(value)[0], 32);
        }

        public static void WriteLong(this ref BitStream<StreamByteStream> bitStream, ulong value, int length)
        {
            if (length != 64)
                throw new NotImplementedException();

            bitStream.Write(long2doubleInt(value)[1], 32);
            bitStream.Write(long2doubleInt(value)[0], 32);
        }
    }
}
