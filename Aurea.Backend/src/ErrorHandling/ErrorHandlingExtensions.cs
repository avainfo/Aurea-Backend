using Microsoft.AspNetCore.Diagnostics;

namespace Aurea.Backend.ErrorHandling;

public static class ErrorHandlingExtensions
{
	public static void UseJsonExceptionHandler(this WebApplication app)
	{
		app.UseExceptionHandler("/error");

		app.Map("/error", (HttpContext ctx) =>
		{
			var feature = ctx.Features.Get<IExceptionHandlerFeature>();
			var ex = feature?.Error;

			ctx.Response.StatusCode = 500;
			ctx.Response.ContentType = "application/json";

			return Results.Json(new
			{
				error = new
				{
					code = "internal_error",
					message = "Something went wrong",
					details = app.Environment.IsDevelopment() ? ex?.Message : null
				}
			});
		});
	}
}
