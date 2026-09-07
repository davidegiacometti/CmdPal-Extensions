// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace SnippetsExtension.Persistence;

internal sealed class SnippetJsonParser
{
    public SnippetJsonParser()
    {
    }

    public SnippetsData ParseSnippets(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new SnippetsData();
        }

        try
        {
            var snippets = JsonSerializer.Deserialize(json, SnippetSerializationContext.Default.SnippetsData);
            return snippets ?? new SnippetsData();
        }
        catch (JsonException)
        {
            return new SnippetsData();
        }
    }

    public string SerializeSnippets(SnippetsData? snippets)
    {
        if (snippets == null)
        {
            return string.Empty;
        }

        return JsonSerializer.Serialize(snippets, SnippetSerializationContext.Default.SnippetsData);
    }
}
