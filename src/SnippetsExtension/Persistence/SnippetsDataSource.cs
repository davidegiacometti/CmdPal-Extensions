// Copyright (c) Davide Giacometti. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace SnippetsExtension.Persistence;

internal sealed partial class SnippetsDataSource
{
    private readonly Logger _logger;

    private readonly string _filePath;

    public SnippetsDataSource(Logger logger, string filePath)
    {
        _logger = logger;
        _filePath = filePath;
    }

    public string GetSnippetData()
    {
        if (!File.Exists(_filePath))
        {
            return string.Empty;
        }

        try
        {
            return File.ReadAllText(_filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read snippet data", typeof(SnippetsDataSource));
            return string.Empty;
        }
    }

    public void SaveSnippetData(string jsonData)
    {
        try
        {
            File.WriteAllText(_filePath, jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write snippet data", typeof(SnippetsDataSource));
        }
    }
}
