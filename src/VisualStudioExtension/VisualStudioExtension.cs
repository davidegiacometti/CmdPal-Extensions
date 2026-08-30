// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Community.PowerToys.Run.Plugin.VisualStudio.Core;
using Community.PowerToys.Run.Plugin.VisualStudio.Core.Services;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace VisualStudioExtension;

[ComVisible(true)]
#if DEBUG
[Guid("2133155f-511c-4e4e-b7df-810f42d10a30")]
#else
[Guid("b6f2e125-fa86-4e65-b787-5f98b672bff3")]
#endif
[ComDefaultInterface(typeof(IExtension))]
public sealed partial class VisualStudioExtension : IExtension, IDisposable
{
    private readonly ManualResetEvent _extensionDisposedEvent;
    private readonly SettingsManager _settingsManager;
    private readonly Settings _settings;
    private readonly ILogger _logger;
    private readonly VisualStudioService _visualStudioService;

    private readonly CommandsProvider _provider;

    public VisualStudioExtension(ManualResetEvent extensionDisposedEvent)
    {
        _extensionDisposedEvent = extensionDisposedEvent;

        _settingsManager = new SettingsManager();
        _logger = new Logger();
        _visualStudioService = new VisualStudioService(_logger);
        _provider = new CommandsProvider(_settingsManager, _visualStudioService);

        _settings = _settingsManager.Settings;
        _settings.SettingsChanged += SettingsChanged;

        InitInstances();
    }

    public object? GetProvider(ProviderType providerType)
    {
        return providerType switch
        {
            ProviderType.Commands => _provider,
            _ => null,
        };
    }

    public void Dispose() => _extensionDisposedEvent.Set();

    private void SettingsChanged(object sender, Settings args)
    {
        InitInstances();
    }

    private void InitInstances() => _visualStudioService.InitInstances(_settingsManager.ExcludedVersions);
}
