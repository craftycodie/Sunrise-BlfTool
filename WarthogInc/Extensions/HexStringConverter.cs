using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class HexStringConverter : JsonConverter
{
    public override void WriteJson(
        JsonWriter writer,
        object value,
        JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        byte[] data = (byte[])value;

        // Compose an array.
        writer.WriteValue(Convert.ToHexString(data));

        //for (var i = 0; i < data.Length; i++)
        //{
        //    writer.WriteValue(data[i]);
        //}

        //writer.WriteEndArray();
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
    {
        string hexString = (string)reader.Value;
        return Convert.FromHexString(hexString);
        //if (reader.TokenType == JsonToken.StartArray)
        //{
        //    var byteList = new List<byte>();

        //    while (reader.Read())
        //    {
        //        switch (reader.TokenType)
        //        {
        //            case JsonToken.Integer:
        //                byteList.Add(Convert.ToByte(reader.Value));
        //                break;
        //            case JsonToken.EndArray:
        //                return byteList.ToArray();
        //            case JsonToken.Comment:
        //                // skip
        //                break;
        //            default:
        //                throw new Exception(
        //                string.Format(
        //                    "Unexpected token when reading bytes: {0}",
        //                    reader.TokenType));
        //        }
        //    }

        //    throw new Exception("Unexpected end when reading bytes.");
        //}
        //else
        //{
        //    throw new Exception(
        //        string.Format(
        //            "Unexpected token parsing binary. "
        //            + "Expected StartArray, got {0}.",
        //            reader.TokenType));
        //}
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(byte[]);
    }
}