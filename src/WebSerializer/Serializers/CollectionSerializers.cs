﻿namespace Cysharp.Web.Serializers;

public sealed class EnumerableWebSerializer<TCollection, TElement> : IWebSerializer<TCollection>
    where TCollection : IEnumerable<TElement>
{
    public void Serialize(ref WebSerializerWriter writer, TCollection value, WebSerializerOptions options)
    {
        if (value == null) return;

        writer.EnterAndValidate(options);

        var serializer = options.GetRequiredSerializer<TElement>();
        var first = true;
        foreach (var item in value)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                writer.AppendRaw(options.CollectionSeparator);
            }

            serializer.Serialize(ref writer, item, options);
        }

        writer.Exit();
    }
}

public sealed class DictionaryWebSerializer<TDictionary, TKey, TValue> : IWebSerializer<TDictionary>
    where TDictionary : IDictionary<TKey, TValue>
{
    public void Serialize(ref WebSerializerWriter writer, TDictionary value, WebSerializerOptions options)
    {
        if (value == null) return;

        writer.EnterAndValidate(options);

        var keySerializer = options.GetRequiredSerializer<TKey>();
        var valueSerializer = options.GetRequiredSerializer<TValue>();

        var first = true;
        foreach (var item in value)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                writer.AppendConcatenate();
            }

            // Name
            writer.AppendNamePrefix();
            keySerializer.Serialize(ref writer, item.Key, options);

            writer.AppendRaw("=");

            // Value
            valueSerializer.Serialize(ref writer, item.Value, options);
        }

        writer.Exit();
    }
}