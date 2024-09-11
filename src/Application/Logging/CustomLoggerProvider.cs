using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Application.Logging;

[ExcludeFromCodeCoverage]
public class CustomLoggerProvider : ILoggerProvider
{
    private readonly CustomLoggerProviderConfiguration _loggerConfig;
    private readonly ConcurrentDictionary<string, CustomLooger> loggers = 
        new ConcurrentDictionary<string, CustomLooger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration loggerConfig)
    {
        _loggerConfig = loggerConfig;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, nome => new CustomLooger(nome, _loggerConfig));
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
