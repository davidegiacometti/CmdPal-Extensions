// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace VisualStudioExtension;

internal class SettingsManager : JsonSettingsManager
{
    private readonly ToggleSetting _showPrerelease = new(
        nameof(ShowPrerelease),
        "Setting_ShowPrerelease_Label".GetLocalized(),
        "Setting_ShowPrerelease_Description".GetLocalized(),
        true);

    private readonly ToggleSetting _sortLastUsed = new(
        nameof(SortLastUsed),
        "Setting_SortLastUsed_Label".GetLocalized(),
        "Setting_SortLastUsed_Description".GetLocalized(),
        false);

    private readonly TextSetting _excludedVersions = new(
        nameof(ExcludedVersions),
        "Setting_ExcludedVersions_Label".GetLocalized(),
        "Setting_ExcludedVersions_Description".GetLocalized(),
        string.Empty);

    public bool ShowPrerelease => _showPrerelease.Value;

    public bool SortLastUsed => _sortLastUsed.Value;

    public string[] ExcludedVersions => _excludedVersions.Value?.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray() ?? [];

    public SettingsManager()
    {
        FilePath = SettingsJsonPath();

        _excludedVersions.Placeholder = "Setting_ExcludedVersions_Placeholder".GetLocalized();
        Settings.Add(_showPrerelease);
        Settings.Add(_sortLastUsed);
        Settings.Add(_excludedVersions);

        LoadSettings();

        Settings.SettingsChanged += (s, a) => SaveSettings();
    }

    private static string SettingsJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("VisualStudioExtension");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "settings.json");
    }
}
