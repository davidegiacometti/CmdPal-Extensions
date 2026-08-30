// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Community.PowerToys.Run.Plugin.VisualStudio.Core.Services;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace VisualStudioExtension.Pages;

internal sealed partial class SearchPage : ListPage
{
    private readonly VisualStudioService _visualStudioService;
    private readonly SettingsManager _settingsManager;

    public SearchPage(SettingsManager settingsManager, VisualStudioService visualStudioService)
    {
        _settingsManager = settingsManager;
        _visualStudioService = visualStudioService;

        Name = "Name".GetLocalized();
#if DEBUG
        Name += " (Dev)";
#endif
        Icon = Consts.Icon;
    }

    public override IListItem[] GetItems()
    {
        var items = Search();
        if (_settingsManager.SortLastUsed)
        {
            items = items.OrderByDescending(i => i.LastAccessed);
        }
        else
        {
            items = items.OrderBy(i => i.Title);
        }

        return items.ToArray();
    }

    private IEnumerable<CodeContainerListItem> Search()
    {
        foreach (var r in _visualStudioService.GetResults(_settingsManager.ShowPrerelease))
        {
            yield return new CodeContainerListItem(r, _settingsManager);
        }
    }
}
