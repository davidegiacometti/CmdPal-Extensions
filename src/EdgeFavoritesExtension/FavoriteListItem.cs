// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;
using Community.PowerToys.Run.Plugin.EdgeFavorite.Core.Models;
using Community.PowerToys.Run.Plugin.EdgeFavorite.Core.Services;
using EdgeFavoritesExtension.Commands;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.System;

namespace EdgeFavoritesExtension
{
    internal partial class FavoriteListItem : ListItem
    {
        public FavoriteListItem(FavoriteItem favorite, EdgeManager edgeManager, SettingsManager settingsManager, ProfileManager profileManager)
            : base(new NoOpCommand())
        {
            Title = favorite.Name;
            MoreCommands = GetMoreCommands(favorite, edgeManager, settingsManager);

            if (favorite.Type == FavoriteType.Folder)
            {
                Command = new NoOpCommand();
                Subtitle = settingsManager.SearchMode == SearchMode.Tree && profileManager.FavoriteProviders.Count > 1
                    ? string.Format(CultureInfo.CurrentCulture, "FolderResult_Profile_Subtitle".GetLocalized(), favorite.Path, favorite.Profile.Name)
                    : string.Format(CultureInfo.CurrentCulture, "FolderResult_Subtitle".GetLocalized(), favorite.Path);
                Icon = new IconInfo("\uE8B7");
            }
            else if (favorite.Type == FavoriteType.Url)
            {
                Command = new OpenEdgeCommand(edgeManager, favorite, false, false);
                Subtitle = settingsManager.SearchMode == SearchMode.Tree && profileManager.FavoriteProviders.Count > 1
                    ? string.Format(CultureInfo.CurrentCulture, "FavoriteResult_Profile_Subtitle".GetLocalized(), favorite.Path, favorite.Profile.Name)
                    : string.Format(CultureInfo.CurrentCulture, "FavoriteResult_Subtitle".GetLocalized(), favorite.Path);
                Icon = new IconInfo("\uE734");
            }
            else
            {
                throw new ArgumentException("Invalid favorite item", nameof(favorite));
            }
        }

        private static IContextItem[] GetMoreCommands(FavoriteItem favorite, EdgeManager edgeManager, SettingsManager settingsManager)
        {
            if (favorite.Type == FavoriteType.Folder)
            {
                var favorites = favorite.Children.Where(c => c.Type == FavoriteType.Url).ToArray();
                if (favorites.Length > 0)
                {
                    return
                    [
                        new CommandContextItem(new OpenEdgeCommand(edgeManager, favorites, false, false))
                        {
                            RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.O, 0),
                        },
                        new CommandContextItem(new OpenEdgeCommand(edgeManager, favorites, false, true))
                        {
                            RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.N, 0),
                        },
                        new CommandContextItem(new OpenEdgeCommand(edgeManager, favorites, true, false))
                        {
                            RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.P, 0),
                        },
                    ];
                }
            }
            else if (favorite.Type == FavoriteType.Url)
            {
                return
                [
                    new CommandContextItem(new CopyTextCommand(favorite.Url!))
                    {
                        RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.C, 0),
                    },
                    new CommandContextItem(new OpenEdgeCommand(edgeManager, favorite, false, true))
                    {
                        RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.N, 0),
                    },
                    new CommandContextItem(new OpenEdgeCommand(edgeManager, favorite, true, false))
                    {
                        RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.P, 0),
                    },
                ];
            }

            return [];
        }
    }
}
