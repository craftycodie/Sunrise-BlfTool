using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class XUIDConverter : JsonConverter
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

        writer.WriteValue("0x" + Convert.ToHexString(BitConverter.GetBytes(ReverseBytes((ulong)value))));
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
    {
        string hexString = (string)reader.Value;
        return ReverseBytes(BitConverter.ToUInt64(Convert.FromHexString(hexString.Substring(2))));
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ulong);
    }

    public static ulong ReverseBytes(ulong value)
    {
        return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
               (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
               (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
               (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
    }
}