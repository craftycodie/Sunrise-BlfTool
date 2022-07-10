﻿using Newtonsoft.Json;
using Sewer56.BitStream;
using Sewer56.BitStream.ByteStreams;
using Sunrise.BlfTool.BlfChunks.GameEngineVariants;
using System;
using System.IO;
using WarthogInc.BlfChunks;
using Sunrise.BlfTool.Extensions;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;

namespace Sunrise.BlfTool
{
    public class PackedMapVariant : IBLFChunk
    {
        public BaseGameVariant.VariantMetadata metadata;
        public byte mapVariantVersion;
        [JsonConverter(typeof(ObjectIndexConverter))]
        public uint mapVariantChecksum; // 32
        public short numberOfScenarioObjects; // 10
        [JsonIgnore]
        public short numberOfVariantObjects { get { return (short)objects.Length; } } // 10
        [JsonIgnore]
        public short numberOfPlacableObjectQuotas { get { return (short)budget.Length; } } // 9
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
            return 12;
        }

        public void ReadChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            metadata = new BaseGameVariant.VariantMetadata(ref hoppersStream);
            mapVariantVersion = hoppersStream.Read<byte>(8);
            mapVariantChecksum = hoppersStream.Read<uint>(32);
            numberOfScenarioObjects = hoppersStream.Read<short>(10);
            short numberOfVariantObjects = hoppersStream.Read<short>(10);
            short numberOfPlacableObjectQuotas = hoppersStream.Read<short>(9);
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
            for (int i = 0; i < 14; ++i)
            {
                objectTypes[i] = hoppersStream.Read<short>(9);
            }

            budget = new VariantBudgetEntry[numberOfPlacableObjectQuotas];
            for (int i = 0; i < numberOfPlacableObjectQuotas; i++)
            {
                budget[i] = new VariantBudgetEntry(ref hoppersStream);
            }

            hoppersStream.Seek(hoppersStream.NextByteIndex, 0);

            if (mapVariantVersion == 13)
                ConvertMCCMap();
        }

        public void WriteChunk(ref BitStream<StreamByteStream> hoppersStream)
        {
            metadata.Write(ref hoppersStream);
            hoppersStream.Write(mapVariantVersion, 8);
            hoppersStream.Write(mapVariantChecksum, 32);
            hoppersStream.Write(numberOfScenarioObjects, 10);
            hoppersStream.Write(numberOfVariantObjects, 10);
            hoppersStream.Write(numberOfPlacableObjectQuotas, 9);
            hoppersStream.Write(mapID, 32);
            hoppersStream.Write(builtIn ? 1 : 0, 1);
            worldBounds.Write(ref hoppersStream);
            hoppersStream.Write(gameEngineSubtype, 4);
            hoppersStream.WriteFloat(maximumBudget, 32);
            hoppersStream.WriteFloat(spentBudget, 32);

            foreach (var variantObject in objects)
            {
                variantObject.Write(ref hoppersStream);
            }

            foreach (var objectType in objectTypes)
            {
                hoppersStream.Write(objectType, 9);
            }

            foreach (var budgetEntry in budget)
            {
                budgetEntry.Write(ref hoppersStream);
            }
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
                hoppersStream.Write(xMin, 32);
                hoppersStream.Write(xMax, 32);
                hoppersStream.Write(yMin, 32);
                hoppersStream.Write(yMax, 32);
                hoppersStream.Write(zMin, 32);
                hoppersStream.Write(zMax, 32);
            }
        }
        public class VariantObject {
            public enum EShapeType : byte
            {
                UNKNOWN_1 = 1,
                CYLINDER,
                BOX,
            }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public short? flags;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int? definitionIndex;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public long? parentObjectIdentifier;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Position position;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Axes axis;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public byte? propertiesCachedObjectType;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public byte? propertiesFlags;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public short? propertiesGameEngineFlags;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public byte? propertiesSharedStorage;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public byte? propertiesSpawnTime;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public byte? propertiesTeamAffiliation;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            [JsonConverter(typeof(StringEnumConverter))]
            public EShapeType? propertiesShapeType;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public short? propertiesShapeRadiusWidth;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public short? propertiesShapeDepth;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public short? propertiesShapeTop;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public short? propertiesShapeBottom;

            public VariantObject() { }

            public VariantObject(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                if (hoppersStream.Read<byte>(1) == 0)
                    return;

                flags = hoppersStream.Read<short>(16);
                definitionIndex = hoppersStream.Read<int>(32);
                bool parentObjectExists = hoppersStream.Read<byte>(1) > 0;

                if (parentObjectExists)
                    parentObjectIdentifier = hoppersStream.Read<long>(64);

                bool positionExists = hoppersStream.Read<byte>(1) > 0;

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
                    propertiesShapeType = (EShapeType)hoppersStream.Read<byte>(8);

                    if (propertiesShapeType == 0)
                        propertiesShapeType = null;

                    switch(propertiesShapeType)
                    {
                        case EShapeType.UNKNOWN_1:
                            propertiesShapeRadiusWidth = hoppersStream.Read<short>(16);
                            break;
                        case EShapeType.CYLINDER:
                            propertiesShapeRadiusWidth = hoppersStream.Read<short>(16);
                            propertiesShapeDepth = hoppersStream.Read<short>(16);
                            propertiesShapeTop = hoppersStream.Read<short>(16);
                            break;
                        case EShapeType.BOX:
                            propertiesShapeRadiusWidth = hoppersStream.Read<short>(16);
                            propertiesShapeDepth = hoppersStream.Read<short>(16);
                            propertiesShapeTop = hoppersStream.Read<short>(16);
                            propertiesShapeBottom = hoppersStream.Read<short>(16);
                            break;
                    }
                }
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {

                hoppersStream.Write(flags == null ? 0 : 1, 1);

                if (flags == null)
                    return;

                hoppersStream.Write(flags.Value, 16);
                hoppersStream.Write(definitionIndex.Value, 32);

                hoppersStream.Write(parentObjectIdentifier != null ? 1 : 0, 1);

                if (parentObjectIdentifier != null)
                    hoppersStream.Write(parentObjectIdentifier.Value, 64);

                hoppersStream.Write(position != null ? 1 : 0, 1);

                if (position != null)
                {
                    position.Write(ref hoppersStream);
                    axis.Write(ref hoppersStream);
                    hoppersStream.Write(propertiesCachedObjectType.Value, 8);
                    hoppersStream.Write(propertiesFlags.Value, 8);
                    hoppersStream.Write(propertiesGameEngineFlags.Value, 16);
                    hoppersStream.Write(propertiesSharedStorage.Value, 8);
                    hoppersStream.Write(propertiesSpawnTime.Value, 8);
                    hoppersStream.Write(propertiesTeamAffiliation.Value, 8);
                    hoppersStream.Write(((byte?)propertiesShapeType ?? 0), 8);

                    switch (propertiesShapeType)
                    {
                        case EShapeType.UNKNOWN_1:
                            hoppersStream.Write(propertiesShapeRadiusWidth.Value, 16);
                            break;
                        case EShapeType.CYLINDER:
                            hoppersStream.Write(propertiesShapeRadiusWidth.Value, 16);
                            hoppersStream.Write(propertiesShapeDepth.Value, 16);
                            hoppersStream.Write(propertiesShapeTop.Value, 16);
                            break;
                        case EShapeType.BOX:
                            hoppersStream.Write(propertiesShapeRadiusWidth.Value, 16);
                            hoppersStream.Write(propertiesShapeDepth.Value, 16);
                            hoppersStream.Write(propertiesShapeTop.Value, 16);
                            hoppersStream.Write(propertiesShapeBottom.Value, 16);
                            break;
                    }
                }
            }
        }

        public class VariantBudgetEntry
        {
            [JsonConverter(typeof(ObjectIndexConverter))]
            public uint objectDefinitionIndex;
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
                objectDefinitionIndex = hoppersStream.Read<uint>(32);
                minimumCount = hoppersStream.Read<byte>(8);
                maximumCount = hoppersStream.Read<byte>(8);
                placedOnMap = hoppersStream.Read<byte>(8);
                maximumAllowed = hoppersStream.Read<byte>(8);
                pricePerItem = hoppersStream.ReadFloat(32);
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.Write(objectDefinitionIndex, 32);
                hoppersStream.Write(minimumCount, 8);
                hoppersStream.Write(maximumCount, 8);
                hoppersStream.Write(placedOnMap, 8);
                hoppersStream.Write(maximumAllowed, 8);
                hoppersStream.WriteFloat(pricePerItem, 32);
            }
        }

        public class Position
        {
            public short x;
            public short y;
            public short z;

            public Position() { }

            public Position(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                x = hoppersStream.Read<short>(16);
                y = hoppersStream.Read<short>(16);
                z = hoppersStream.Read<short>(16);
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                hoppersStream.Write(x, 16);
                hoppersStream.Write(y, 16);
                hoppersStream.Write(z, 16);
            }
        }

        public class Axes
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int? upQuantization;
            public byte forwardAngle;

            public Axes() { }

            public Axes(ref BitStream<StreamByteStream> hoppersStream)
            {
                Read(ref hoppersStream);
            }

            public void Read(ref BitStream<StreamByteStream> hoppersStream)
            {
                bool upIsGlobalUp3d = hoppersStream.Read<byte>(1) > 0;

                if (!upIsGlobalUp3d)
                    upQuantization = hoppersStream.Read<int>(19);

                forwardAngle = hoppersStream.Read<byte>(8);
            }

            public void Write(ref BitStream<StreamByteStream> hoppersStream)
            {
                if (upQuantization == null)
                    hoppersStream.Write(1, 1);
                else
                {
                    hoppersStream.Write(0, 1);
                    hoppersStream.Write(upQuantization.Value, 19);
                }

                hoppersStream.Write(forwardAngle, 8);
            }
        }

        private void ConvertMCCMap()
        {
            BudgetObjectIndexConverter objectIndexMap = new BudgetObjectIndexConverter(mapID);

            List<VariantBudgetEntry> newBudget = new List<VariantBudgetEntry>();
            List<VariantObject> newObjects = new List<VariantObject>(objects);

            for (int i = 0; i < budget.Length; i++)
            {
                VariantBudgetEntry entry = budget[i];
                short objectGroup = (short)(entry.objectDefinitionIndex >> 16);
                short objectIndex = (short)(entry.objectDefinitionIndex & 0xffff);

                if (objectGroup > 7)
                    continue;

                try
                {
                    entry.objectDefinitionIndex = objectIndexMap.Get360ObjectIndex(objectGroup, objectIndex);
                    newBudget.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine($"Unknown 360 object index for [{objectGroup},{objectIndex}], removing placed objects.");
                    newObjects = newObjects.Where(variantObject => variantObject.definitionIndex != i + 1).ToList();
                }
            }

            budget = newBudget.ToArray();
            objects = newObjects.ToArray();

            mapVariantVersion = 12;
        }
    }
}
