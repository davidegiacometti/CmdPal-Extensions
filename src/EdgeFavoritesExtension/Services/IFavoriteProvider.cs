// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using EdgeFavoritesExtension.Models;

namespace EdgeFavoritesExtension.Services
{
    internal interface IFavoriteProvider
    {
        FavoriteItem Root { get; }
    }
}
