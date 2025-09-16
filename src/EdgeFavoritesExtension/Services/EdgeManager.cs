// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using EdgeFavoritesExtension.Models;
using Windows.Management.Deployment;

namespace EdgeFavoritesExtension.Services
{
    internal class EdgeManager : IEdgeManager
    {
        private readonly Logger _logger;
        private readonly PackageManager _packageManager;

        private readonly Dictionary<Channel, (string PackageName, string UserDataParentFolder)> _packages = new()
        {
            { Channel.Stable, ("Microsoft.MicrosoftEdge.Stable", "Edge") },
            { Channel.Beta, ("Microsoft.MicrosoftEdge.Beta", "Edge Beta") },
            { Channel.Dev, ("Microsoft.MicrosoftEdge.Dev", "Edge Dev") },
            { Channel.Canary, ("Microsoft.MicrosoftEdge.Canary", "Edge SxS") },
        };

        private string? _openCommand;
        private string? _userDataPath;

        public string UserDataPath => _userDataPath ?? string.Empty;

        public bool ChannelDetected => _openCommand != null && _userDataPath != null;

        public EdgeManager(Logger logger)
        {
            _logger = logger;
            _packageManager = new PackageManager();
        }

        public void Initialize(Channel channel)
        {
            _openCommand = null;
            _userDataPath = null;

            var user = WindowsIdentity.GetCurrent().User;

            if (user == null)
            {
                return;
            }

            var (packageName, userDataParentFolder) = _packages[channel];

            foreach (var p in _packageManager.FindPackagesForUser(user.Value))
            {
                if (p.Id.Name.Equals(packageName, StringComparison.OrdinalIgnoreCase))
                {
                    _openCommand = $@"shell:AppsFolder\{p.Id.FamilyName}!App";
                    _userDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", userDataParentFolder, "User Data");
                    break;
                }
            }
        }

        public bool Open(FavoriteItem favorite, bool inPrivate, bool newWindow)
        {
            if (favorite.Type == FavoriteType.Url)
            {
                return OpenInternal(favorite.Profile, favorite.Url, null, inPrivate, newWindow);
            }
            else if (favorite.Type == FavoriteType.Workspace)
            {
                return OpenInternal(favorite.Profile, null, favorite.WorkspaceId, inPrivate, newWindow);
            }

            throw new ArgumentException("Invalid favorite item", nameof(favorite));
        }

        public bool Open(FavoriteItem[] favorites, bool inPrivate, bool newWindow)
        {
            if (favorites.Length == 0)
            {
                throw new InvalidOperationException("Favorites cannot be empty.");
            }

            // If there is no need to open in a new window, starting multiple processes is preferred to avoid long command line arguments
            if (newWindow)
            {
                return OpenInternal(favorites[0].Profile, string.Join(" ", favorites.Select(f => f.Url!)), null, inPrivate, newWindow);
            }
            else
            {
                var result = true;

                foreach (var favorite in favorites)
                {
                    if (!Open(favorite, inPrivate, false))
                    {
                        result = false;
                    }
                }

                return result;
            }
        }

        private bool OpenInternal(ProfileInfo profileInfo, string? urls, string? workspaceId, bool inPrivate, bool newWindow)
        {
            var args = string.Empty;

            if (urls != null)
            {
                args += $" {urls}";
            }

            if (workspaceId != null)
            {
                args += $" -launch-workspace=\"{workspaceId}\"";
            }

            if (inPrivate)
            {
                args += " -inprivate";
            }

            if (newWindow)
            {
                args += " -new-window";
            }

            args += $" -profile-directory=\"{profileInfo.Directory}\"";

            try
            {
                OpenInShell(_openCommand!, args);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to launch Microsoft Edge", typeof(EdgeManager));
                return false;
            }
        }

        private static bool OpenInShell(string path, string args)
        {
            using var process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = true;

            try
            {
                process.Start();
                return true;
            }
            catch (Win32Exception)
            {
                return false;
            }
        }
    }
}
