﻿using System.Runtime.CompilerServices;
using System.Text;

namespace Cysharp.Web;

public static class WebSerializer
{
    public static string ToQueryString<T>(in T value)
    {
        var sb = new StringBuilder();
        ToQueryString<T>(sb, value);
        return sb.ToString();
    }

    public static string ToQueryString<T>(string urlBase, in T value)
    {
        var sb = new StringBuilder();
        sb.Append(urlBase);
        sb.Append("?");

        var beforeLength = sb.Length;
        ToQueryString<T>(sb, value);

        if (sb.Length == beforeLength)
        {
            // trim last '?'
            sb.Remove(sb.Length - 1, 1);
        }

        return sb.ToString();
    }

    public static void ToQueryString<T>(StringBuilder stringBuilder, in T value, WebSerializerOptions? options = default)
    {
        var writer = new WebSerializerWriter(stringBuilder);
        ToQueryString(writer, value, options);
    }

    public static void ToQueryString<T>(in WebSerializerWriter writer, in T value, WebSerializerOptions? options = default)
    {
        options ??= new WebSerializerOptions(WebSerializerProvider.Default);
        var serialzier = options.GetRequiredSerializer<T>();
        serialzier.Serialize(ref Unsafe.AsRef(writer), Unsafe.AsRef(value), options);
    }

    public static HttpContent ToHttpContent<T>(in T value)
    {
        throw new NotImplementedException();
    }
}