// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using EdgeFavoritesExtension.Pages;
using EdgeFavoritesExtension.Services;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace EdgeFavoritesExtension
{
    internal partial class CommandsProvider : CommandProvider
    {
        private readonly SettingsManager _settingsManager;
        private readonly EdgeManager _edgeManager;
        private readonly FavoriteQuery _favoriteQuery;
        private readonly ProfileManager _profileManager;
        private readonly ICommandItem[] _commands;

        public CommandsProvider(SettingsManager settingsManager, EdgeManager edgeManager, FavoriteQuery favoriteQuery, ProfileManager profileManager)
        {
            _settingsManager = settingsManager;
            _edgeManager = edgeManager;
            _favoriteQuery = favoriteQuery;
            _profileManager = profileManager;
            Settings = _settingsManager.Settings;
            DisplayName = "Name".GetLocalized();
#if DEBUG
            DisplayName += " (Dev)";
#endif
            Icon = Helper.ExtensionIcon;

            _commands = new ICommandItem[1];
            _settingsManager.Settings.SettingsChanged += OnSettingsChanged;

            SettingsChanged();
        }

        public override ICommandItem[] TopLevelCommands() => _commands;

        private void OnSettingsChanged(object sender, Settings args) => SettingsChanged();

        private void SettingsChanged()
        {
            _edgeManager.Initialize(_settingsManager.Channel);
            _profileManager.ReloadProfiles(_settingsManager.ExcludedProfiles);

            ICommand page = _settingsManager.SearchMode == SearchMode.Tree
                ? new TreePage(_edgeManager, _favoriteQuery, _settingsManager, _profileManager)
                : new SearchPage(_edgeManager, _favoriteQuery, _settingsManager, _profileManager);

            _commands[0] = new CommandItem(page)
            {
                Subtitle = "Description".GetLocalized(),
                MoreCommands =
                [
                    new CommandContextItem(Settings!.SettingsPage),
                ],
            };

            RaiseItemsChanged(0);
        }
    }
}
