// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CommandPalette.Extensions;

namespace SnippetsExtension;

[ComVisible(true)]
#if DEBUG
[Guid("801cc76d-4a30-446c-89bc-5f5b16d43169")]
#else
[Guid("f8ed2e3e-ae92-4390-b5bf-7fa65a3af316")]
#endif
[ComDefaultInterface(typeof(IExtension))]
public sealed partial class SnippetsExtension : IExtension, IDisposable
{
    private readonly ManualResetEvent _extensionDisposedEvent;

    private readonly CommandsProvider _provider;

    public SnippetsExtension(ManualResetEvent extensionDisposedEvent)
    {
        _extensionDisposedEvent = extensionDisposedEvent;

        _provider = new CommandsProvider();
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
}
