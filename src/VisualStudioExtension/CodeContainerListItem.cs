﻿// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Community.PowerToys.Run.Plugin.VisualStudio.Core.Models;
using Microsoft.CommandPalette.Extensions.Toolkit;
using VisualStudioExtension.Commands;

namespace VisualStudioExtension
{
    internal partial class CodeContainerListItem : ListItem
    {
        public DateTime LastAccessed { get; }

        public CodeContainerListItem(CodeContainer codeContainer, SettingsManager settingsManager)
            : base(new OpenVisualStudioCommand(codeContainer, false))
        {
            Title = codeContainer.Name;
            Icon = new IconInfo(codeContainer.Instance.InstancePath);
            Subtitle = string.Format("Result_Subtitle".GetLocalized(), codeContainer.Instance.DisplayName, codeContainer.FullPath);
            MoreCommands =
            [
                new CommandContextItem(new OpenVisualStudioCommand(codeContainer, true)),
                new CommandContextItem(new CopyTextCommand(codeContainer.FullPath)),
                new CommandContextItem(new OpenFolderCommand(codeContainer)),
            ];

            LastAccessed = codeContainer.LastAccessed;
        }
    }
}
