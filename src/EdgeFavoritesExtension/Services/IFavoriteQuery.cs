// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using EdgeFavoritesExtension.Models;

namespace EdgeFavoritesExtension.Services
{
    internal interface IFavoriteQuery
    {
        IEnumerable<FavoriteItem> GetAll();

        IEnumerable<FavoriteItem> Search(string query);
    }
}
