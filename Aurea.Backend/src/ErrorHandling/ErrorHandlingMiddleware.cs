using System.Diagnostics;
using System.Text.Json;

namespace Aurea.Backend.ErrorHandling;

public sealed class ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
{
	private readonly RequestDelegate _next = next;
	private readonly IWebHostEnvironment _env = env;
	private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

	public async Task Invoke(HttpContext ctx)
	{
		try
		{
			await _next(ctx);
		}
		catch (Exception ex)
		{
			await WriteExceptionAsync(ctx, ex);
		}
	}

	private async Task WriteExceptionAsync(HttpContext ctx, Exception ex)
	{
		var includeDetails = _env.IsDevelopment();
		var (status, code, message) = ExceptionMapping.Map(ex, includeDetails);

		// Prefer Activity.Current.Id, fallback to HttpContext.TraceIdentifier
		var requestId = Activity.Current?.Id ?? ctx.TraceIdentifier;

		var envelope = new ErrorEnvelope
		{
			Code = code,
			Message = message,
			Details = includeDetails ? ex.GetType().Name : null, // keep it minimal even in dev
			RequestId = requestId,
			Path = ctx.Request.Path
		};

		ctx.Response.Clear();
		ctx.Response.StatusCode = status;
		ctx.Response.ContentType = "application/json; charset=utf-8";

		await ctx.Response.WriteAsync(JsonSerializer.Serialize(envelope, JsonOpts));
	}
}
