// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Microsoft.CommandPalette.Extensions;
using Serilog;
using Shmuelie.WinRTServer;
using Shmuelie.WinRTServer.CsWinRT;

namespace SnippetsExtension;

public class Program
{
    [MTAThread]
    public static void Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;

        if (args.Length > 0 && args[0] == "-RegisterProcessAsComServer")
        {
            global::Shmuelie.WinRTServer.ComServer server = new();
            ManualResetEvent extensionDisposedEvent = new(false);
            SnippetsExtension extensionInstance = new(extensionDisposedEvent);
            server.RegisterClass<SnippetsExtension, IExtension>(() => extensionInstance);
            server.Start();
            extensionDisposedEvent.WaitOne();
            server.Stop();
            server.UnsafeDispose();
        }
    }

    private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = (Exception)e.ExceptionObject;
        Log.Fatal(exception, "Unhandled exception");
        Log.CloseAndFlush();
    }
}
