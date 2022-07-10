using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class ObjectIndexConverter : JsonConverter
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

        writer.WriteValue("0x" + Convert.ToHexString(BitConverter.GetBytes(ReverseBytes((uint)value))));
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
    {
        string hexString = (string)reader.Value;
        return ReverseBytes(BitConverter.ToUInt32(Convert.FromHexString(hexString.Substring(2))));
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(uint);
    }

    private static uint ReverseBytes(uint value)
    {
        return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
               (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
    }
}