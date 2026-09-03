// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CommandPalette.Extensions.Toolkit;

namespace SnippetsExtension;

public static class Consts
{
    public static readonly IconInfo Icon = IconHelpers.FromRelativePath(@"Assets\Snippets.svg");

    public static readonly IconInfo TypeIcon = new("\uE765");

    public static readonly IconInfo EditIcon = new("\uE70F");

    public static readonly IconInfo DeleteIcon = new("\uE74D");

    public static readonly IconInfo ResultIcon = new("\uE8C1");
}
