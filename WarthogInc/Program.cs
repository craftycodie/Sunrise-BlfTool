using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using System;
using System.IO;

namespace WarthogInc
{
    class Program
    {
        static void Main(string[] args)
        {
            BitStream<StreamByteStream> streamHelper = new BitStream<StreamByteStream>(new StreamByteStream(new FileStream("C:\\Users\\codie\\Desktop\\hoppers", FileMode.Open)));
            Hoppers hoppers = new HopperReader().ReadHoppers(streamHelper);
            Console.WriteLine(hoppers.categoryCount);
            Console.WriteLine(hoppers.configurationsCount);
        }
    }
}
