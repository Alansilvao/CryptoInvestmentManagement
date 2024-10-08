﻿using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Application.Logging;

[ExcludeFromCodeCoverage]
public class CustomLoggerProviderConfiguration
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
    public int EventId { get; set; } = 0;
}
