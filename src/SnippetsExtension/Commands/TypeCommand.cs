// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Timers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SnippetsExtension.Persistence;
using SnippetsExtension.Services;
using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace SnippetsExtension.Commands;

internal partial class TypeCommand : InvokableCommand
{
    private readonly SnippetData _snippetData;

    public TypeCommand(SnippetData snippetData)
    {
        _snippetData = snippetData;

        Name = "Command_Type".GetLocalized();
        Icon = Consts.TypeIcon;
    }

    public override ICommandResult Invoke()
    {
        var hwnd = WindowHelper.GetPreviousWindow();
        if (hwnd.IsNull)
        {
            return CommandResult.KeepOpen();
        }

        var result = CommandResult.Hide();
        var throttleTimer = new Timer(TimeSpan.FromMilliseconds(500))
        {
            AutoReset = false,
        };

        throttleTimer.Elapsed += (_, _) =>
        {
            if (WindowHelper.SetForegroundWindow(hwnd))
            {
                var inputString = NormalizeLineEndings(_snippetData.Value);

                Span<INPUT> inputs = stackalloc INPUT[2];

                foreach (var rune in inputString.EnumerateRunes())
                {
                    foreach (var c in rune.ToString())
                    {
                        System.Threading.Thread.Sleep(20);

                        if (IsLineEnding(c))
                        {
                            AddLineBreak(ref inputs);
                        }
                        else
                        {
                            inputs[0] = new INPUT
                            {
                                type = INPUT_TYPE.INPUT_KEYBOARD,
                                Anonymous =
                                {
                                    ki = new KEYBDINPUT
                                    {
                                        wVk = 0,
                                        wScan = c,
                                        dwFlags = KEYBD_EVENT_FLAGS.KEYEVENTF_UNICODE,
                                    },
                                },
                            };

                            inputs[1] = new INPUT
                            {
                                type = INPUT_TYPE.INPUT_KEYBOARD,
                                Anonymous =
                                {
                                    ki = new KEYBDINPUT
                                    {
                                        wVk = 0,
                                        wScan = c,
                                        dwFlags = KEYBD_EVENT_FLAGS.KEYEVENTF_UNICODE | KEYBD_EVENT_FLAGS.KEYEVENTF_KEYUP,
                                    },
                                },
                            };
                        }

                        PInvoke.SendInput(inputs, Marshal.SizeOf<INPUT>());
                    }
                }
            }
        };

        throttleTimer.Start();
        return result;
    }

    private static string NormalizeLineEndings(string value) => value.Replace("\r\n", "\r").Replace('\n', '\r');

    private static bool IsLineEnding(char c) => c is '\r';

    private static void AddLineBreak(ref Span<INPUT> inputs)
    {
        inputs[0] = new INPUT
        {
            type = INPUT_TYPE.INPUT_KEYBOARD,
            Anonymous =
            {
                ki = new KEYBDINPUT
                {
                    wVk = VIRTUAL_KEY.VK_RETURN,
                    dwFlags = KEYBD_EVENT_FLAGS.KEYEVENTF_UNICODE,
                },
            },
        };

        inputs[1] = new INPUT
        {
            type = INPUT_TYPE.INPUT_KEYBOARD,
            Anonymous =
            {
                ki = new KEYBDINPUT
                {
                    wVk = VIRTUAL_KEY.VK_RETURN,
                    dwFlags = KEYBD_EVENT_FLAGS.KEYEVENTF_UNICODE | KEYBD_EVENT_FLAGS.KEYEVENTF_KEYUP,
                },
            },
        };
    }
}
