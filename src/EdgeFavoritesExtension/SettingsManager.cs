// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using EdgeFavoritesExtension.Models;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace EdgeFavoritesExtension
{
    internal class SettingsManager : JsonSettingsManager
    {
        private readonly ChoiceSetSetting _searchMode = new(
            nameof(SearchMode),
            "Setting_SearchMode_Label".GetLocalized(),
            "Setting_SearchMode_Description".GetLocalized(),
            [
                new("SearchMode_FlatAll".GetLocalized(), "Flat"),
                new("SearchMode_FlatNoFolders".GetLocalized(), "FlatFavorites"),
                new("SearchMode_Tree".GetLocalized(), "Tree"),
            ]);

        private readonly TextSetting _excludedProfiles = new(
           nameof(ExcludedProfiles),
           "Setting_ExcludedProfiles_Label".GetLocalized(),
           "Setting_ExcludedProfiles_Description".GetLocalized(),
           string.Empty);

        private readonly ChoiceSetSetting _channel = new(
            nameof(Channel),
            "Setting_Channel_Label".GetLocalized(),
            "Setting_Channel_Description".GetLocalized(),
            [
                new("Channel_Stable".GetLocalized(), "Stable"),
                new("Channel_Beta".GetLocalized(), "Beta"),
                new("Channel_Dev".GetLocalized(), "Dev"),
                new("Channel_Canary".GetLocalized(), "Canary"),
            ]);

        public SearchMode SearchMode => _searchMode.Value != null && Enum.TryParse(_searchMode.Value.ToString(), out SearchMode searchMode) ? searchMode : SearchMode.Flat;

        public string[] ExcludedProfiles => _excludedProfiles.Value?.Split(["\r\n", "\r"], StringSplitOptions.RemoveEmptyEntries).ToArray() ?? [];

        public Channel Channel => _channel.Value != null && Enum.TryParse(_channel.Value.ToString(), out Channel channel) ? channel : Channel.Stable;

        public SettingsManager()
        {
            FilePath = SettingsJsonPath();

            _excludedProfiles.Placeholder = "Setting_ExcludedProfiles_Placeholder".GetLocalized();
            _excludedProfiles.Multiline = true;
            Settings.Add(_searchMode);
            Settings.Add(_excludedProfiles);
            Settings.Add(_channel);

            LoadSettings();

            Settings.SettingsChanged += (s, a) => SaveSettings();
        }

        private static string SettingsJsonPath()
        {
            var directory = Utilities.BaseSettingsPath("EdgeFavoritesExtension");
            Directory.CreateDirectory(directory);
            return Path.Combine(directory, "settings.json");
        }
    }
}
