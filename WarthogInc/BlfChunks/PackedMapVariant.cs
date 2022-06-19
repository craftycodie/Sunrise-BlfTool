using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using Sunrise.BlfTool.BlfChunks.GameEngineVariants;
using System;
using System.IO;
using WarthogInc.BlfChunks;

namespace Sunrise.BlfTool
{
    public class PackedMapVariant : IBLFChunk
    {
        public BaseGameVariant.VariantMetadata metadata;
        public byte mapVariantVersion;
        public int mapVariantChecksum; // 2
        public short numberOfScenarioObjects; // 10
        public short numberOfVariantObjects; // 10
        public short numberOfPlacableObjectQuotas; // 9
        public int mapID;
        public bool builtIn;
        public WorldBounds worldBounds;
        public byte gameEngineSubtype;
        public int maximumBudget;
        public int spentBudget;
        public VariantObject[] objects; // * 640
        public short[] objectTypes; // 9 * 14
        public VariantBudgetEntry[] budget; // * 256

        public ushort GetAuthentication()
        {
            return 1;
        }

        public uint GetLength()
        {
            var ms = new BitStream<StreamByteStream>(new StreamByteStream(new MemoryStream()));
            WriteChunk(ref ms);
            return (uint)ms.NextByteIndex;
        }

        public string GetName()
        {
            return "mvar";
        }

        public ushort GetVersion()
        {
            return 10;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            metadata = new BaseGameVariant.VariantMetadata(ref hoppersStream);
            throw new NotImplementedException();
            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public class WorldBounds
        {
            public int xMin;
            public int xMax;
            public int yMin;
            public int yMax;
            public int zMin;
            public int zMax;

            public WorldBounds() { }

            public WorldBounds(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                xMin = hoppersStream.Read<int>(32);
                xMax = hoppersStream.Read<int>(32);
                yMin = hoppersStream.Read<int>(32);
                yMax = hoppersStream.Read<int>(32);
                zMin = hoppersStream.Read<int>(32);
                zMax = hoppersStream.Read<int>(32);
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.Write<int>(xMin, 32);
                hoppersStream.Write<int>(xMax, 32);
                hoppersStream.Write<int>(yMin, 32);
                hoppersStream.Write<int>(yMax, 32);
                hoppersStream.Write<int>(zMin, 32);
                hoppersStream.Write<int>(zMax, 32);
            }
        }
        public class VariantObject {
            public bool objectExists;
            public ushort flags;
            public int definitionIndex;
            public bool parentObjectExists;
            public long parentObjectIdentifier;
            public bool positionExists;
            public Position position;
            public Axes axis;
            public byte propertiesCachedObjectType;
            public byte propertiesFlags;
            public short propertiesGameEngineFlags;
            public byte propertiesSharedStorage;
            public byte propertiesSpawnTime;
            public byte propertiesTeamAffiliation;
            public byte propertiesShapeType;
            public float propertiesShapeRadiusWidth;
            public float propertiesShapeLength;
            public float propertiesShapePositiveHeight;
        }

        public class VariantBudgetEntry
        {
            public int objectDefinitionIndex;
            public byte minimumCount;
            public byte maximumCount;
            public byte placedOnMap;
            public byte maximumAllowed;
            public float pricePerItem;
        }

        public class Position
        {
            public Half x;
            public Half y;
            public Half z;
        }

        public class Axes
        {
            public bool upIsGlobalUp;
            public int upQuantization;
            public float forwardAngle;
        }
    }
}
