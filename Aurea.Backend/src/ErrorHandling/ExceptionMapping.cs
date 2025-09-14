using System.ComponentModel.DataAnnotations;

namespace Aurea.Backend.ErrorHandling;

public static class ExceptionMapping
{
	public static (int Status, string Code, string Message) Map(Exception ex, bool includeDetails = false)
	{
		return ex switch
		{
			ValidationException => (StatusCodes.Status400BadRequest, ErrorCodes.BadRequest,
				"The request is invalid."),
			UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, ErrorCodes.Unauthorized,
				"Authentication is required."),
			KeyNotFoundException => (StatusCodes.Status404NotFound, ErrorCodes.NotFound,
				"The requested resource was not found."),
			NotSupportedException => (StatusCodes.Status415UnsupportedMediaType, ErrorCodes.UnsupportedMediaType,
				"Unsupported media type."),
			InvalidOperationException => (StatusCodes.Status409Conflict, ErrorCodes.Conflict,
				"Operation conflict."),
			_ => (StatusCodes.Status500InternalServerError, ErrorCodes.Internal,
				includeDetails ? ex.Message : "An unexpected error occurred.")
		};
	}
}
