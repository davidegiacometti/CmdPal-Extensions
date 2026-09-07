// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SnippetsExtension.Persistence;
using Windows.Foundation;

namespace SnippetsExtension.Pages;

internal sealed partial class AddSnippetPage : ContentPage
{
    internal event TypedEventHandler<object, SnippetData>? AddedCommand
    {
        add => _addSnippetForm.AddedCommand += value;
        remove => _addSnippetForm.AddedCommand -= value;
    }

    private readonly AddSnippetForm _addSnippetForm;

    public AddSnippetPage(SnippetData? snippet)
    {
        var name = snippet?.Name ?? string.Empty;
        var value = snippet?.Value ?? string.Empty;
        var isAdd = string.IsNullOrEmpty(name) && string.IsNullOrEmpty(value);

        Icon = isAdd ? Consts.Icon : Consts.EditIcon;
        Name = isAdd ? "Command_Add".GetLocalized() : "Command_Edit".GetLocalized();

#if DEBUG
        if (isAdd)
        {
            Name += " (Dev)";
        }
#endif

        _addSnippetForm = new AddSnippetForm(snippet);
    }

    public override IContent[] GetContent() => [_addSnippetForm];
}
