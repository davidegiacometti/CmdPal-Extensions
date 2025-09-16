// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EdgeFavoritesExtension.Models
{
    internal class FavoriteItem
    {
        private readonly List<FavoriteItem> _children = new();
        private readonly bool _special;

        public string Name { get; }

        public string? Url { get; }

        public string Path { get; }

        public FavoriteType Type { get; }

        public string? WorkspaceId { get; }

        public ProfileInfo Profile { get; }

        public ReadOnlyCollection<FavoriteItem> Children => _children.AsReadOnly();

        public bool IsEmptySpecialFolder => _special && Children.Count == 0;

        private FavoriteItem(string name, string? url, string path, FavoriteType type, string? workspaceId, ProfileInfo profile, bool special)
        {
            Name = name;
            Url = url;
            Path = path;
            Type = type;
            WorkspaceId = workspaceId;
            Profile = profile;
            _special = special;
        }

        public static FavoriteItem CreateRoot(ProfileInfo profile)
        {
            return new FavoriteItem(string.Empty, null, string.Empty, FavoriteType.Folder, null, profile, false);
        }

        public static FavoriteItem CreateFolder(string name, string path, ProfileInfo profile, bool special = false)
        {
            return new FavoriteItem(name, null, path, FavoriteType.Folder, null, profile, special);
        }

        public static FavoriteItem CreateUrl(string name, string url, string path, ProfileInfo profile)
        {
            return new FavoriteItem(name, url, path, FavoriteType.Url, null, profile, false);
        }

        public static FavoriteItem CreateWorkspace(string name, string path, string workspaceId, ProfileInfo profile)
        {
            return new FavoriteItem(name, null, path, FavoriteType.Workspace, workspaceId, profile, false);
        }

        public void AddChildren(FavoriteItem item)
        {
            _children.Add(item);
        }
    }
}
