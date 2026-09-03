// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SnippetsExtension.Services;

namespace SnippetsExtension.Pages;

internal sealed partial class SearchPage : ListPage
{
    private readonly SnippetsManager _snippetsManager;

    public SearchPage(SnippetsManager snippetsManager)
    {
        _snippetsManager = snippetsManager;

        Name = "Name".GetLocalized();
#if DEBUG
        Name += " (Dev)";
#endif
        Icon = Consts.Icon;
    }

    public override IListItem[] GetItems() => [.. _snippetsManager.Snippets.OrderBy(s => s.Name).Select(s => new SnippetListItem(s, _snippetsManager))];
}
