using System.Diagnostics.CodeAnalysis;

namespace Infra.Database.Config;

[ExcludeFromCodeCoverage]
public class DatabaseOptions
{
	public string SqlServer { get; init; } = null!;
}
