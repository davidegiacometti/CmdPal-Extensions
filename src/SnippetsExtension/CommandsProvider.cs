// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SnippetsExtension.Pages;
using SnippetsExtension.Services;

namespace SnippetsExtension;

internal partial class CommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private readonly SnippetsManager _snippetsManager;
    private readonly CommandItem _addSnippetCommand;
    private readonly CommandItem _searchCommand;

    public CommandsProvider()
    {
        var logger = new Logger();
        _snippetsManager = new SnippetsManager(logger);

        DisplayName = "Name".GetLocalized();
#if DEBUG
        DisplayName += " (Dev)";
#endif
        Icon = Consts.Icon;

        var addSnippetPage = new AddSnippetPage(null);
        _addSnippetCommand = new ListItem(addSnippetPage)
        {
            Subtitle = "Add_Description".GetLocalized(),
        };
        addSnippetPage.AddedCommand += (_, e) => _snippetsManager.Add(e.Name, e.Value);
        _searchCommand = new CommandItem(new SearchPage(_snippetsManager))
        {
            Subtitle = "Description".GetLocalized(),
        };

        _commands = [_addSnippetCommand, _searchCommand];
    }

    public override ICommandItem[] TopLevelCommands() => _commands;
}
