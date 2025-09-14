namespace Aurea.Backend.ErrorHandling;

public sealed class ErrorEnvelope
{
	public required string Code { get; init; }
	public required string Message { get; init; }
	public string? Details { get; init; }
	public string RequestId { get; init; } = string.Empty;
	public string Path { get; init; } = string.Empty;
	public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}
