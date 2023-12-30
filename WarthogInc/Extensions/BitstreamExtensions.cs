using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.Text;

namespace Sunrise.BlfTool.Extensions
{
    static class BitstreamExtensions
    {
        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static ushort SwapBytes(ushort x)
        {
            return (ushort)((ushort)((x & 0xff) << 8) | ((x >> 8) & 0xff));
        }
        private static sbyte SwapBits(sbyte value, int numBits)
        {
            int bitcount = 8;
            string binaryString = Convert.ToString(value, 2).PadLeft(bitcount, '0');
            string swapSect = binaryString.Substring(binaryString.Length - numBits);
            string swappedSect = swapSect.Reverse();
            string output = swappedSect.PadLeft(bitcount, '0');
            return Convert.ToSByte(output, 2);
        }

        private static byte SwapBits(byte value, int numBits)
        {
            int bitcount = 8;
            string binaryString = Convert.ToString(value, 2).PadLeft(bitcount, '0');
            string swapSect = binaryString.Substring(binaryString.Length - numBits);
            string swappedSect = swapSect.Reverse();
            string output = swappedSect.PadLeft(bitcount, '0');
            return Convert.ToByte(output, 2);
        }

        private static short SwapBits(short value, int numBits)
        {
            int bitcount = 16;
            string binaryString = Convert.ToString(value, 2).PadLeft(bitcount, '0');
            string swapSect = binaryString.Substring(binaryString.Length - numBits);
            string swappedSect = swapSect.Reverse();
            string output = swappedSect.PadLeft(bitcount, '0');
            return Convert.ToInt16(output, 2);
        }

        private static ushort SwapBits(ushort value, int numBits)
        {
            int bitcount = 16;
            string binaryString = Convert.ToString(value, 2).PadLeft(bitcount, '0');
            string swapSect = binaryString.Substring(binaryString.Length - numBits);
            string swappedSect = swapSect.Reverse();
            string output = swappedSect.PadLeft(bitcount, '0');
            return Convert.ToUInt16(output, 2);
        }

        private static int SwapBits(int value, int numBits)
        {
            int bitcount = 32;
            string binaryString = Convert.ToString(value, 2).PadLeft(bitcount, '0');
            string swapSect = binaryString.Substring(binaryString.Length - numBits);
            string swappedSect = swapSect.Reverse();
            string output = swappedSect.PadLeft(bitcount, '0');
            return Convert.ToInt32(output, 2);
        }

        private static uint SwapBits(uint value, int numBits)
        {
            int bitcount = 32;
            string binaryString = Convert.ToString(value, 2).PadLeft(bitcount, '0');
            string swapSect = binaryString.Substring(binaryString.Length - numBits);
            string swappedSect = swapSect.Reverse();
            string output = swappedSect.PadLeft(bitcount, '0');
            return Convert.ToUInt32(output, 2);
        }

        private static long SwapBits(long value, int numBits)
        {
            int bitcount = 64;
            string binaryString = Convert.ToString(value, 2).PadLeft(bitcount, '0');
            string swapSect = binaryString.Substring(binaryString.Length - numBits);
            string swappedSect = swapSect.Reverse();
            string output = swappedSect.PadLeft(bitcount, '0');
            return Convert.ToInt64(output, 2);
        }

        //private static ulong SwapBits(ulong value, int numBits)
        //{
        //    int bitcount = 64;
        //    string binaryString = Convert.ToString(value, 2).PadLeft(bitcount, '0');
        //    string swapSect = binaryString.Substring(binaryString.Length - numBits);
        //    string swappedSect = swapSect.Reverse();
        //    string output = swappedSect.PadLeft(bitcount, '0');
        //    return Convert.ToUInt64(output, 2);
        //}

        public static void WriteBitswappedString(ref this BitStream<StreamByteStream> hoppersStream, string text, int maxLengthCharacters, Encoding encoding)
        {
            if (encoding == Encoding.UTF8) { 
                char[] characters = text.ToCharArray();

                for (int characterIndex = 0; characterIndex < characters.Length && characterIndex < maxLengthCharacters; characterIndex++)
                    hoppersStream.Write(SwapBits((byte)characters[characterIndex], 8));

                if (text.Length < maxLengthCharacters)
                    hoppersStream.Write(0, 8);
            }
            else if (encoding == Encoding.BigEndianUnicode) {
                char[] characters = text.ToCharArray();

                for (int characterIndex = 0; characterIndex < characters.Length && characterIndex < maxLengthCharacters; characterIndex++) {
                    ushort bitswapped = SwapBits((ushort)characters[characterIndex], 16);
                    hoppersStream.Write(bitswapped, 16);
                }

                if (text.Length < maxLengthCharacters)
                    hoppersStream.Write(0, 16);
            }
            else throw new NotImplementedException("Bitswapped encoding not implemented");
        }

        public static void WriteBitswapped<T>(ref this BitStream<StreamByteStream> hoppersStream, T value, int numBits)
        {
            if (typeof(T) == typeof(byte)) hoppersStream.Write(SwapBits((byte)Convert.ChangeType(value, typeof(byte)), numBits), numBits);
            else if (typeof(T) == typeof(sbyte)) hoppersStream.Write<sbyte>(SwapBits((sbyte)Convert.ChangeType(value, typeof(sbyte)), numBits), numBits);

            else if (typeof(T) == typeof(short)) hoppersStream.Write<short>(SwapBits((short)Convert.ChangeType(value, typeof(short)), numBits), numBits);
            else if (typeof(T) == typeof(ushort)) hoppersStream.Write<ushort>(SwapBits((ushort)Convert.ChangeType(value, typeof(ushort)), numBits), numBits);

            else if (typeof(T) == typeof(int)) hoppersStream.Write<int>(SwapBits((int)Convert.ChangeType(value, typeof(int)), numBits), numBits);
            else if (typeof(T) == typeof(uint)) hoppersStream.Write<uint>(SwapBits((uint)Convert.ChangeType(value, typeof(uint)), numBits), numBits);

            else if (typeof(T) == typeof(long)) hoppersStream.Write<long>(SwapBits((long)Convert.ChangeType(value, typeof(long)), numBits), numBits);
            //else if (typeof(T) == typeof(ulong)) hoppersStream.Write<ulong>(SwapBits((ulong)Convert.ChangeType(value, typeof(ulong)), numBits));

            // Debug-only because exceptions prevent inlining.
            else throw new InvalidCastException();
        }

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

        public static void WriteBitswappedFloat(this ref BitStream<StreamByteStream> bitStream, float value, int length)
        {
            bitStream.WriteBitswapped(BitConverter.ToInt32(BitConverter.GetBytes(value)), length);
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

        public static void WriteDate(this ref BitStream<StreamByteStream> bitStream, DateTime value, int length)
        {
            if (length != 64)
                throw new NotImplementedException();

            bitStream.WriteLong((long)value.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds, length);
        }

        public static DateTime ReadDate(this ref BitStream<StreamByteStream> bitStream, int length)
        {
            if (length != 64)
                throw new NotImplementedException();

            long seconds = bitStream.Read<long>(length);

            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);
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
