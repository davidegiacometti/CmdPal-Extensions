// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace SnippetsExtension.Persistence;

internal sealed class SnippetsData
{
    public List<SnippetData> Data { get; set; } = [];
}
