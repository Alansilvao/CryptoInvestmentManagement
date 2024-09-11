using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Application.Logging;

[ExcludeFromCodeCoverage]
public class CustomLooger : ILogger
{
    private readonly string _loggerName;
    private readonly CustomLoggerProviderConfiguration _configuration;

    public CustomLooger(string nome, CustomLoggerProviderConfiguration configuration)
    {
        _loggerName = nome;
        _configuration = configuration;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, 
                            EventId eventId, 
                            TState state, 
                            Exception exception, 
                            Func<TState, 
                            Exception, string> formatter)
    {
        var message = string.Format($"{logLevel}: {eventId} - {formatter(state, exception)}");
        CreateLogFile(message);

    }

    private void CreateLogFile (string message)
    {
        var archiveDirectory = @$"C:\repositorio\InvestimentPlatform\LOG-{DateTime.Now:yyyy-MM-dd}.txt";
        if (!File.Exists(archiveDirectory))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(archiveDirectory));
            File.Create(archiveDirectory).Dispose();
        }

        using StreamWriter streamWriter = new StreamWriter(archiveDirectory, true);
        streamWriter.WriteLine(message);
        streamWriter.Close();
    }
}
