using System.Text.Json;

namespace Aurea.Backend.ErrorHandling;

public static class StatusCodePageWriter
{
	private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

	public static Task WriteJsonAsync(HttpContext ctx)
	{
		if (ctx.Response.HasStarted) return Task.CompletedTask;

		var code = ctx.Response.StatusCode switch
		{
			StatusCodes.Status401Unauthorized => ErrorCodes.Unauthorized,
			StatusCodes.Status403Forbidden => ErrorCodes.Forbidden,
			StatusCodes.Status404NotFound => ErrorCodes.NotFound,
			StatusCodes.Status405MethodNotAllowed => ErrorCodes.MethodNotAllowed,
			StatusCodes.Status409Conflict => ErrorCodes.Conflict,
			StatusCodes.Status415UnsupportedMediaType => ErrorCodes.UnsupportedMediaType,
			StatusCodes.Status422UnprocessableEntity => ErrorCodes.Unprocessable,
			StatusCodes.Status429TooManyRequests => ErrorCodes.TooManyRequests,
			_ => ErrorCodes.Internal
		};

		var message = ctx.Response.StatusCode switch
		{
			StatusCodes.Status401Unauthorized => "Authentication is required.",
			StatusCodes.Status403Forbidden => "You are not allowed to access this resource.",
			StatusCodes.Status404NotFound => "The requested resource was not found.",
			StatusCodes.Status405MethodNotAllowed => "The method is not allowed on this endpoint.",
			StatusCodes.Status409Conflict => "Operation conflict.",
			StatusCodes.Status415UnsupportedMediaType => "Unsupported media type.",
			StatusCodes.Status422UnprocessableEntity => "Unprocessable entity.",
			StatusCodes.Status429TooManyRequests => "Too many requests. Please retry later.",
			_ => "An unexpected error occurred."
		};

		var envelope = new ErrorEnvelope
		{
			Code = code,
			Message = message,
			RequestId = ctx.TraceIdentifier,
			Path = ctx.Request.Path
		};

		ctx.Response.ContentType = "application/json; charset=utf-8";
		return ctx.Response.WriteAsync(JsonSerializer.Serialize(envelope, JsonOpts));
	}
}
