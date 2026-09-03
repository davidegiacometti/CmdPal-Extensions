// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SnippetsExtension.Persistence;
using SnippetsExtension.Services;

namespace SnippetsExtension.Commands;

internal partial class DeleteCommand : InvokableCommand
{
    private readonly SnippetData _snippet;
    private readonly SnippetsManager _snippetsManager;

    public DeleteCommand(SnippetData snippet, SnippetsManager snippetsManager)
    {
        _snippet = snippet;
        _snippetsManager = snippetsManager;

        Name = "Command_Delete".GetLocalized();
        Icon = Consts.DeleteIcon;
    }

    public override ICommandResult Invoke()
    {
        _snippetsManager.Remove(_snippet.Id);
        return CommandResult.GoHome();
    }
}
