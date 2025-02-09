// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Community.PowerToys.Run.Plugin.EdgeFavorite.Core.Services;
using Microsoft.CmdPal.Extensions;
using Microsoft.CmdPal.Extensions.Helpers;

namespace EdgeFavoritesExtension.Pages
{
    internal sealed partial class SearchPage : ListPage
    {
        private readonly EdgeManager _edgeManager;
        private readonly FavoriteQuery _favoriteQuery;
        private readonly SettingsManager _settingsManager;

        public SearchPage(EdgeManager edgeManager, FavoriteQuery favoriteQuery, SettingsManager settingsManager)
        {
            _edgeManager = edgeManager;
            _favoriteQuery = favoriteQuery;
            _settingsManager = settingsManager;

            Name = "Name".GetLocalized();
            Icon = new("\uE728");
        }

        public override IListItem[] GetItems()
        {
            if (_edgeManager.ChannelDetected)
            {
                return Search().OrderBy(r => r.Title).ToArray();
            }
            else
            {
                return [];
            }
        }

        private IEnumerable<FavoriteListItem> Search()
        {
            var emptyQuery = string.IsNullOrWhiteSpace(SearchText);

            foreach (var f in _favoriteQuery.GetAll().Where(f => !f.IsEmptySpecialFolder))
            {
                var score = StringMatcher.FuzzySearch(SearchText, f.Name);

                if (emptyQuery || score.Score > 0)
                {
                    yield return new FavoriteListItem(f, _edgeManager, _settingsManager);
                }
            }
        }
    }
}
