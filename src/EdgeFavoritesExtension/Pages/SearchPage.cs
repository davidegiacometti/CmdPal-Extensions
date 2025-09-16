// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using EdgeFavoritesExtension.Models;
using EdgeFavoritesExtension.Services;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace EdgeFavoritesExtension.Pages
{
    internal sealed partial class SearchPage : ListPage
    {
        private readonly EdgeManager _edgeManager;
        private readonly FavoriteQuery _favoriteQuery;
        private readonly SettingsManager _settingsManager;
        private readonly ProfileManager _profileManger;

        public SearchPage(EdgeManager edgeManager, FavoriteQuery favoriteQuery, SettingsManager settingsManager, ProfileManager profileManger)
        {
            _edgeManager = edgeManager;
            _favoriteQuery = favoriteQuery;
            _settingsManager = settingsManager;
            _profileManger = profileManger;

            Name = "Name".GetLocalized();
#if DEBUG
            Name += " (Dev)";
#endif
            Icon = Consts.Icon;
        }

        public override IListItem[] GetItems()
        {
            if (!_edgeManager.ChannelDetected)
            {
                return [];
            }

            return Search().OrderBy(r => r.Title).ToArray();
        }

        private IEnumerable<FavoriteListItem> Search()
        {
            foreach (var f in _favoriteQuery.GetAll().Where(f => !f.IsEmptySpecialFolder))
            {
                if (f.Type == FavoriteType.Folder && _settingsManager.SearchMode == SearchMode.FlatFavorites)
                {
                    continue;
                }

                yield return new FavoriteListItem(f, _edgeManager, _settingsManager, _profileManger);
            }
        }
    }
}
