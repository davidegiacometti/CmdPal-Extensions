// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.CompilerServices;
using Serilog;
using Windows.Storage;

namespace EdgeFavoritesExtension
{
    internal class Logger
    {
        public Logger()
        {
            var path = Path.Combine(ApplicationData.Current.TemporaryFolder.Path, "Logs", "Log.log");

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.File(path, rollingInterval: RollingInterval.Day)
               .CreateLogger();
        }

        public void LogDebug(string message, Type fullClassName, [CallerMemberName] string methodName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Debug(message);
        }

        public void LogError(string message, Type fullClassName, [CallerMemberName] string methodName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Error(message);
        }

        public void LogError(Exception exception, string message, Type fullClassName, [CallerMemberName] string methodName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Error(exception, message);
        }

        public void LogInformation(string message, Type fullClassName, [CallerMemberName] string methodName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Information(message);
        }

        public void LogWarning(string message, Type fullClassName, [CallerMemberName] string methodName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Warning(message);
        }
    }
}
