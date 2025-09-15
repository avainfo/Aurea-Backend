using System.Diagnostics;
using System.Text.Json;

namespace Aurea.Backend.ErrorHandling;

public sealed class ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
{
	private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

	public async Task Invoke(HttpContext ctx)
	{
		try
		{
			await next(ctx);
		}
		catch (Exception ex)
		{
			await WriteExceptionAsync(ctx, ex);
		}
	}

	private async Task WriteExceptionAsync(HttpContext ctx, Exception ex)
	{
		var includeDetails = env.IsDevelopment();
		var (status, code, message) = ExceptionMapping.Map(ex, includeDetails);

		var requestId = Activity.Current?.Id ?? ctx.TraceIdentifier;

		var envelope = new ErrorEnvelope
		{
			Code = code,
			Message = message,
			Details = includeDetails ? ex.GetType().Name : null,
			RequestId = requestId,
			Path = ctx.Request.Path
		};

		ctx.Response.Clear();
		ctx.Response.StatusCode = status;
		ctx.Response.ContentType = "application/json; charset=utf-8";

		await ctx.Response.WriteAsync(JsonSerializer.Serialize(envelope, JsonOpts));
	}
}
