// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CmdPal.Common.Commands;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SnippetsExtension.Commands;
using SnippetsExtension.Pages;
using SnippetsExtension.Persistence;
using SnippetsExtension.Services;
using Windows.System;

namespace SnippetsExtension;

internal partial class SnippetListItem : ListItem
{
    private readonly SnippetData _snippetData;
    private readonly SnippetsManager _snippetsManager;
    private readonly AddSnippetPage _updateSnippetPage;

    public SnippetListItem(SnippetData snippetData, SnippetsManager snippetsManager)
    {
        _snippetData = snippetData;
        _snippetsManager = snippetsManager;
        _updateSnippetPage = new AddSnippetPage(snippetData);
        _updateSnippetPage.AddedCommand += (_, e) => _snippetsManager.Update(e.Id, e.Name, e.Value);

        Title = snippetData.Name;
        Subtitle = snippetData.Value.Replace("\r\n", " ").Replace('\n', ' ');
        Command = new TypeCommand(snippetData);
        MoreCommands = GetMoreCommands();
        Icon = Consts.ResultIcon;
    }

    private IContextItem[] GetMoreCommands()
    {
        var confirmableCommand = new ConfirmableCommand
        {
            Command = new DeleteCommand(_snippetData, _snippetsManager),
            ConfirmationTitle = "Delete_Prompt_Title".GetLocalized(),
            ConfirmationMessage = "Delete_Prompt_Message".GetLocalized(),
        };

        var deleteCommandItem = new CommandContextItem(confirmableCommand)
        {
            RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.Delete, 0),
        };

        var editCommandItem = new CommandContextItem(_updateSnippetPage)
        {
            RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.E, 0),
        };

        var copyCommandItem = new CommandContextItem(new CopyTextCommand(_snippetData.Value))
        {
            RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.C, 0),
        };

        return [deleteCommandItem, editCommandItem, copyCommandItem];
    }
}
