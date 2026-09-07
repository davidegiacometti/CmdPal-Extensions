// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SnippetsExtension.Persistence;

namespace SnippetsExtension.Services;

internal sealed partial class SnippetsManager
{
    private readonly Logger _logger;
    private readonly SnippetsDataSource _dataSource;
    private readonly SnippetJsonParser _parser = new();
    private readonly Lock _lock = new();
    private readonly SemaphoreSlim _saveLock = new(1, 1);
    private SnippetsData _snippetsData = new();

    public IReadOnlyCollection<SnippetData> Snippets
    {
        get
        {
            lock (_lock)
            {
                return _snippetsData.Data.ToList().AsReadOnly();
            }
        }
    }

    public SnippetsManager(Logger logger)
    {
        _logger = logger;
        _dataSource = new SnippetsDataSource(_logger, StateJsonPath());
        LoadSnippetsFromFile();
    }

    public SnippetData Add(string name, string value)
    {
        var newSnippet = new SnippetData(name, value);

        lock (_lock)
        {
            _snippetsData.Data.Add(newSnippet);
            _ = SaveChangesAsync();
            return newSnippet;
        }
    }

    public bool Remove(Guid id)
    {
        lock (_lock)
        {
            var snippet = _snippetsData.Data.FirstOrDefault(b => b.Id == id);
            if (snippet != null && _snippetsData.Data.Remove(snippet))
            {
                _ = SaveChangesAsync();
                return true;
            }

            return false;
        }
    }

    public SnippetData? Update(Guid id, string name, string value)
    {
        lock (_lock)
        {
            var existingSnippet = _snippetsData.Data.FirstOrDefault(b => b.Id == id);
            if (existingSnippet != null)
            {
                var updatedSnippet = existingSnippet with
                {
                    Name = name,
                    Value = value,
                };

                var index = _snippetsData.Data.IndexOf(existingSnippet);
                _snippetsData.Data[index] = updatedSnippet;

                _ = SaveChangesAsync();
                return updatedSnippet;
            }

            return null;
        }
    }

    private void LoadSnippetsFromFile()
    {
        try
        {
            var jsonData = _dataSource.GetSnippetData();
            var snippetsData = _parser.ParseSnippets(jsonData);

            lock (_lock)
            {
                _snippetsData = snippetsData;
            }
        }
        catch (Exception)
        {
        }
    }

    private void WriteData()
    {
        List<SnippetData> dataToSave;

        lock (_lock)
        {
            dataToSave = _snippetsData.Data.ToList();
        }

        try
        {
            var jsonData = _parser.SerializeSnippets(new SnippetsData { Data = dataToSave });
            _dataSource.SaveSnippetData(jsonData);
        }
        catch (Exception)
        {
        }
    }

    private async Task SaveChangesAsync()
    {
        await _saveLock.WaitAsync();

        try
        {
            await Task.Run(WriteData);
        }
        finally
        {
            _saveLock.Release();
        }
    }

    private static string StateJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("SnippetsExtension");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "snippets.json");
    }
}
