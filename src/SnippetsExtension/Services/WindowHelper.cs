// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace SnippetsExtension.Services;

internal static class WindowHelper
{
    public static HWND GetPreviousWindow()
    {
        var hwnd = PInvoke.GetForegroundWindow();
        if (hwnd.IsNull)
        {
            return HWND.Null;
        }

        var prevHwnd = PInvoke.GetWindow(hwnd, GET_WINDOW_CMD.GW_HWNDPREV);
        if (prevHwnd.IsNull)
        {
            return HWND.Null;
        }

        return prevHwnd;
    }

    public static bool SetForegroundWindow(HWND hwnd)
    {
        var targetThreadId = PInvoke.GetWindowThreadProcessId(hwnd, out _);
        if (targetThreadId == 0)
        {
            return false;
        }

        var currentThreadId = PInvoke.GetCurrentThreadId();
        if (currentThreadId == 0)
        {
            return false;
        }

        if (!PInvoke.AttachThreadInput(currentThreadId, targetThreadId, true))
        {
            return false;
        }

        try
        {
            PInvoke.SetForegroundWindow(hwnd); // Method doesn't return true
            return true;
        }
        finally
        {
            PInvoke.AttachThreadInput(currentThreadId, targetThreadId, false);
        }
    }
}
