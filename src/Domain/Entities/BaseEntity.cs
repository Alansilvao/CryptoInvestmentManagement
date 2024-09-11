using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class BaseEntity
{
	public string Id { get; }

	public BaseEntity(string id)
	{
		Id = id;
	}

	public BaseEntity()
	{
		Id = Guid.NewGuid().ToString();
	}
}