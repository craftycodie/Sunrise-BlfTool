using Newtonsoft.Json;
using SunriseBlfTool.BlfChunks;
using System;
using System.Collections.Generic;
using WarthogInc.BlfChunks;

public class BlfFileConverter : JsonConverter
{
    private readonly AbstractBlfChunkNameMap chunkNameMap;

    public BlfFileConverter(AbstractBlfChunkNameMap _chunkNameMap)
    {
        this.chunkNameMap = _chunkNameMap;
    }

    private object PopulateDictionary(IDictionary<string, IBLFChunk> dictionary, JsonReader reader)
    {
        bool finished = false;
        do
        {
            switch (reader.TokenType)
            {
                case JsonToken.PropertyName:
                    string keyValue = (string)reader.Value!;

                    IBLFChunk itemValue;
                    reader.Read();

                    itemValue = (IBLFChunk)new JsonSerializer().Deserialize(reader, chunkNameMap.GetChunk(keyValue).GetType());

                    dictionary[keyValue] = itemValue;
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
            throw new JsonSerializationException("Unexpected end when deserializing object.");

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