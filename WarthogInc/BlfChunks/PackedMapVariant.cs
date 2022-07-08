using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using Sunrise.BlfTool.BlfChunks.GameEngineVariants;
using System;
using System.IO;
using WarthogInc.BlfChunks;
using Sunrise.BlfTool.Extensions;

namespace Sunrise.BlfTool
{
    public class PackedMapVariant : IBLFChunk
    {
        public BaseGameVariant.VariantMetadata metadata;
        public byte mapVariantVersion;
        public int mapVariantChecksum; // 32
        public short numberOfScenarioObjects; // 10
        public short numberOfVariantObjects; // 10
        public short numberOfPlacableObjectQuotas; // 9
        public int mapID;
        public bool builtIn;
        public WorldBounds worldBounds;
        public byte gameEngineSubtype;
        public float maximumBudget;
        public float spentBudget;
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
            mapVariantVersion = hoppersStream.Read<byte>(8);
            mapVariantChecksum = hoppersStream.Read<int>(32);
            numberOfScenarioObjects = hoppersStream.Read<short>(10);
            numberOfVariantObjects = hoppersStream.Read<short>(10);
            numberOfPlacableObjectQuotas = hoppersStream.Read<short>(9);
            mapID = hoppersStream.Read<int>(32);
            builtIn = hoppersStream.Read<byte>(1) > 0;
            worldBounds = new WorldBounds(ref hoppersStream);
            gameEngineSubtype = hoppersStream.Read<byte>(4);
            maximumBudget = hoppersStream.ReadFloat(32);
            spentBudget = hoppersStream.ReadFloat(32);

            objects = new VariantObject[numberOfVariantObjects];
            for (int i = 0; i < numberOfVariantObjects; i++)
            {
                objects[i] = new VariantObject(ref hoppersStream);
            }

            objectTypes = new short[14];
            for (int i = 0; i < 14; i++)
            {
                objectTypes[i] = hoppersStream.Read<short>(9);
            }

            budget = new VariantBudgetEntry[numberOfPlacableObjectQuotas];
            for (int i = 0; i < numberOfPlacableObjectQuotas; i++)
            {
                budget[i] = new VariantBudgetEntry(ref hoppersStream);
            }

            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            throw new NotImplementedException();
        }

        public class WorldBounds
        {
            public float xMin;
            public float xMax;
            public float yMin;
            public float yMax;
            public float zMin;
            public float zMax;

            public WorldBounds() { }

            public WorldBounds(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                xMin = hoppersStream.ReadFloat(32);
                xMax = hoppersStream.ReadFloat(32);
                yMin = hoppersStream.ReadFloat(32);
                yMax = hoppersStream.ReadFloat(32);
                zMin = hoppersStream.ReadFloat(32);
                zMax = hoppersStream.ReadFloat(32);
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.WriteFloat(xMin, 32);
                hoppersStream.WriteFloat(xMax, 32);
                hoppersStream.WriteFloat(yMin, 32);
                hoppersStream.WriteFloat(yMax, 32);
                hoppersStream.WriteFloat(zMin, 32);
                hoppersStream.WriteFloat(zMax, 32);
            }
        }
        public class VariantObject {
            public enum SHAPE_TYPE : byte
            {
                UNKNOWN_1 = 1,
                UNKNOWN_2,
                UNKNOWN_3,
            }

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
            public SHAPE_TYPE propertiesShapeType;
            public float propertiesShapeRadiusWidth;
            public float propertiesShapeDepth;
            public float propertiesShapeTop;
            public float propertiesShapeBottom;

            public VariantObject() { }

            public VariantObject(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                objectExists = hoppersStream.Read<byte>(1) > 0;
                flags = hoppersStream.Read<ushort>(16);
                definitionIndex = hoppersStream.Read<int>(32);
                parentObjectExists = hoppersStream.Read<byte>(1) > 0;

                if (parentObjectExists)
                    parentObjectIdentifier = hoppersStream.Read<long>(64);
                else
                    parentObjectIdentifier = -1;

                positionExists = hoppersStream.Read<byte>(1) > 0;

                if (positionExists)
                {
                    position = new Position(ref hoppersStream);
                    axis = new Axes(ref hoppersStream);
                    propertiesCachedObjectType = hoppersStream.Read<byte>(8);
                    propertiesFlags = hoppersStream.Read<byte>(8);
                    propertiesGameEngineFlags = hoppersStream.Read<short>(16);
                    propertiesSharedStorage = hoppersStream.Read<byte>(8);
                    propertiesSpawnTime = hoppersStream.Read<byte>(8);
                    propertiesTeamAffiliation = hoppersStream.Read<byte>(8);
                    propertiesShapeType = (SHAPE_TYPE)hoppersStream.Read<byte>(8);
                    switch(propertiesShapeType)
                    {
                        case SHAPE_TYPE.UNKNOWN_1:
                            propertiesShapeRadiusWidth = hoppersStream.ReadFloat(16);
                            break;
                        case SHAPE_TYPE.UNKNOWN_2:
                            propertiesShapeRadiusWidth = hoppersStream.ReadFloat(16);
                            propertiesShapeDepth = hoppersStream.ReadFloat(16);
                            propertiesShapeTop = hoppersStream.ReadFloat(16);
                            break;
                        case SHAPE_TYPE.UNKNOWN_3:
                            propertiesShapeRadiusWidth = hoppersStream.ReadFloat(16);
                            propertiesShapeDepth = hoppersStream.ReadFloat(16);
                            propertiesShapeTop = hoppersStream.ReadFloat(16);
                            propertiesShapeBottom = hoppersStream.ReadFloat(16);
                            break;
                    }
                }
            }
        }

        public class VariantBudgetEntry
        {
            public int objectDefinitionIndex;
            public byte minimumCount;
            public byte maximumCount;
            public byte placedOnMap;
            public byte maximumAllowed;
            public float pricePerItem;

            public VariantBudgetEntry() { }

            public VariantBudgetEntry(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                objectDefinitionIndex = hoppersStream.Read<int>(32);
                minimumCount = hoppersStream.Read<byte>(8);
                maximumCount = hoppersStream.Read<byte>(8);
                placedOnMap = hoppersStream.Read<byte>(8);
                maximumAllowed = hoppersStream.Read<byte>(8);
                pricePerItem = hoppersStream.Read<float>(32);
            }
        }

        public class Position
        {
            public float x;
            public float y;
            public float z;

            public Position() { }

            public Position(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                x = hoppersStream.ReadFloat(16);
                y = hoppersStream.ReadFloat(16);
                z = hoppersStream.ReadFloat(16);
            }
        }

        public class Axes
        {
            public bool upIsGlobalUp3d;
            public int upQuantization;
            public float forwardAngle;

            public Axes() { }

            public Axes(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                upIsGlobalUp3d = hoppersStream.Read<byte>(1) > 0;

                if (!upIsGlobalUp3d)
                {
                    upQuantization = hoppersStream.Read<int>(19);
                }
                forwardAngle = hoppersStream.ReadFloat(8);
            }
        }
    }
}
