// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SnippetsExtension.Persistence;

public sealed record SnippetData
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Value { get; init; }

    [JsonConstructor]
    [SetsRequiredMembers]
    public SnippetData(Guid id, string? name, string? value)
    {
        Id = id;
        Name = name ?? string.Empty;
        Value = value ?? string.Empty;
    }

    [SetsRequiredMembers]
    public SnippetData(string? name, string? value)
        : this(Guid.NewGuid(), name, value)
    {
    }

    [SetsRequiredMembers]
    public SnippetData()
        : this(Guid.NewGuid(), string.Empty, string.Empty)
    {
    }
}
