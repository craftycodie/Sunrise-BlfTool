using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SunriseBlfTool.BlfChunks;
using System;
using System.Collections;
using System.Collections.Generic;
using WarthogInc.BlfChunks;

public class BlfFileConverter : JsonConverter
{
    private object PopulateDictionary(IDictionary<string, IBLFChunk> dictionary, JsonReader reader)
    {
        //object dictionary;

        //OnDeserializing(reader, contract, underlyingDictionary);

        //if (contract.KeyContract == null)
        //{
        //    contract.KeyContract = GetContractSafe(contract.DictionaryKeyType);
        //}

        //if (contract.ItemContract == null)
        //{
        //    contract.ItemContract = GetContractSafe(contract.DictionaryValueType);
        //}

        //JsonConverter? dictionaryValueConverter = contract.ItemConverter ?? GetConverter(contract.ItemContract, null, contract, containerProperty);
        //PrimitiveTypeCode keyTypeCode = (contract.KeyContract is JsonPrimitiveContract keyContract) ? keyContract.TypeCode : PrimitiveTypeCode.Empty;

        bool finished = false;
        do
        {
            switch (reader.TokenType)
            {
                case JsonToken.PropertyName:
                    string keyValue = (string)reader.Value!;
                    //if (CheckPropertyName(reader, keyValue.ToString()!))
                    //{
                    //    continue;
                    //}

                    //try
                    //{
                        //try
                        //{
                        //    keyValue = contract.KeyContract != null && contract.KeyContract.IsEnum
                        //        ? EnumUtils.ParseEnum(contract.KeyContract.NonNullableUnderlyingType, (Serializer._contractResolver as DefaultContractResolver)?.NamingStrategy, keyValue.ToString()!, false)
                        //        : EnsureType(reader, keyValue, CultureInfo.InvariantCulture, contract.KeyContract, contract.DictionaryKeyType)!;
                        //    break;
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw JsonSerializationException.Create(reader, "Could not convert string '{0}' to dictionary key type '{1}'. Create a TypeConverter to convert from the string to the key type object.".FormatWith(CultureInfo.InvariantCulture, reader.Value, contract.DictionaryKeyType), ex);
                        //}

                        //if (!reader.ReadForType(contract.ItemContract, dictionaryValueConverter != null))
                        //{
                        //    throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
                        //}

                        IBLFChunk itemValue;
                        BlfChunkNameMap chunkNameMap = new BlfChunkNameMap();
                    reader.Read();

                    
                    itemValue = (IBLFChunk)new JsonSerializer().Deserialize(reader, chunkNameMap.GetChunk(keyValue).GetType());

                    //if (dictionaryValueConverter != null && dictionaryValueConverter.CanRead)
                    //{
                    //    itemValue = DeserializeConvertable(dictionaryValueConverter, reader, contract.DictionaryValueType!, null);
                    //}
                    //else
                    //{
                    //    itemValue = CreateValueInternal(reader, contract.DictionaryValueType, contract.ItemContract, null, contract, containerProperty, null);
                    //}

                    dictionary[keyValue] = itemValue;
                    //}
                    //catch (Exception ex)
                    //{
                    //    if (IsErrorHandled(underlyingDictionary, contract, keyValue, reader as IJsonLineInfo, reader.Path, ex))
                    //    {
                    //        HandleError(reader, true, initialDepth);
                    //    }
                    //    else
                    //    {
                    //        throw;
                    //    }
                    //}
                    break;
                case JsonToken.Comment:
                    break;
                case JsonToken.EndObject:
                    finished = true;
                    break;
                default:
                    throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
            }
        } while (!finished && reader.Read());

        if (!finished)
        {
            //ThrowUnexpectedEndException(reader, contract, underlyingDictionary, "Unexpected end when deserializing object.");
        }

        //OnDeserialized(reader, contract, underlyingDictionary);
        return dictionary;
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
    {
        IDictionary<string, IBLFChunk> result = new Dictionary<string, IBLFChunk>();

        if (reader.TokenType == JsonToken.StartObject)
        {
            reader.Read();

            PopulateDictionary(result, reader);
        }

        return result;
    }

    public override void WriteJson(
        JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(IDictionary<string, IBLFChunk>).IsAssignableFrom(objectType);
    }

    public override bool CanWrite
    {
        get { return false; }
    }
}