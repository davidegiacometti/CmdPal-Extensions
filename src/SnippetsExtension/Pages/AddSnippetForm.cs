// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SnippetsExtension.Persistence;
using Windows.Foundation;

namespace SnippetsExtension.Pages;

internal sealed partial class AddSnippetForm : FormContent
{
    private readonly SnippetData? _snippet;

    internal event TypedEventHandler<object, SnippetData>? AddedCommand;

    public AddSnippetForm(SnippetData? snippet)
    {
        _snippet = snippet;
        var name = snippet?.Name ?? string.Empty;
        var value = snippet?.Value ?? string.Empty;
        TemplateJson = $$"""
{
    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
    "type": "AdaptiveCard",
    "version": "1.5",
    "body": [
        {
            "type": "Input.Text",
            "style": "text",
            "id": "name",
            "label": {{EncodeString("Form_Name".GetLocalized())}},
            "value": {{EncodeString(name)}},
            "isRequired": true
        },
        {
            "type": "Input.Text",
            "style": "text",
            "id": "value",
            "label": {{EncodeString("Form_Value".GetLocalized())}},
            "value": {{EncodeString(value)}},
            "isRequired": true,
            "isMultiline": true
        },
        {
            "type": "TextBlock",
            "text": {{EncodeString("Form_Warning".GetLocalized())}},
            "isSubtle": true,
            "size": "Small"
        }
    ],
    "actions": [
        {
            "type": "Action.Submit",
            "title": {{EncodeString("Form_Save".GetLocalized())}},
            "data": {
                "name": "name",
                "value": "value"
            }
        }
    ]
}
""";
    }

    public override CommandResult SubmitForm(string payload)
    {
        var formInput = JsonNode.Parse(payload);
        if (formInput is null)
        {
            return CommandResult.GoHome();
        }

        var formName = formInput["name"] ?? string.Empty;
        var formValue = formInput["value"] ?? string.Empty;
        AddedCommand?.Invoke(this, new SnippetData(formName.ToString(), formValue.ToString()) { Id = _snippet?.Id ?? Guid.Empty });
        return CommandResult.GoHome();
    }

    private static string EncodeString(string s) => JsonSerializer.Serialize(s, SnippetSerializationContext.Default.String);
}
