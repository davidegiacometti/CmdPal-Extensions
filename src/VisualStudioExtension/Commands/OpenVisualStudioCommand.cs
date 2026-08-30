// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Diagnostics;
using Community.PowerToys.Run.Plugin.VisualStudio.Core.Models;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace VisualStudioExtension.Commands;

internal partial class OpenVisualStudioCommand : InvokableCommand
{
    private readonly bool _elevated;
    private readonly string _fileName;
    private readonly string _arguments;

    public OpenVisualStudioCommand(CodeContainer codeContainer, bool elevated)
    {
        _elevated = elevated;
        _fileName = codeContainer.Instance.InstancePath;
        _arguments = $"\"{codeContainer.FullPath}\"";

        Icon = new(_elevated ? "\uE7EF" : "\uE737");
        Name = _elevated
            ? "Command_OpenAsAdministrator".GetLocalized()
            : "Command_Open".GetLocalized();
    }

    public override ICommandResult Invoke()
    {
        using var process = new Process();
        process.StartInfo.FileName = _fileName;
        process.StartInfo.Arguments = _arguments;
        process.StartInfo.UseShellExecute = true;

        if (_elevated)
        {
            process.StartInfo.Verb = "runas";
        }

        try
        {
            process.Start();
        }
        catch (Win32Exception)
        {
        }

        return CommandResult.Dismiss();
    }
}
