namespace Aurea.Backend.ErrorHandling;

public static class ErrorCodes
{
	public const string BadRequest = "bad_request";
	public const string Unauthorized = "unauthorized";
	public const string Forbidden = "forbidden";
	public const string NotFound = "not_found";
	public const string MethodNotAllowed = "method_not_allowed";
	public const string UnsupportedMediaType = "unsupported_media_type";
	public const string Conflict = "conflict";
	public const string TooManyRequests = "rate_limited";
	public const string Unprocessable = "unprocessable_entity";
	public const string Internal = "internal_error";
}
