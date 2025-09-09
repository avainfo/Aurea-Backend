using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace Aurea.Backend.Tests;

public class SmokeTests : IClassFixture<CustomWebAppFactory>
{
	private readonly CustomWebAppFactory _fact;

	public SmokeTests(CustomWebAppFactory fact)
	{
		_fact = fact;
	}

	[Fact]
	public async Task Get_Root_Should_Return_200_And_Minimal_Json()
	{
		var client = _fact.CreateClient();

		var resp = await client.GetAsync("/");
		resp.StatusCode.Should().Be(HttpStatusCode.OK);
		resp.Content.Headers.ContentType!.MediaType.Should().Be("application/json");

		using var stream = await resp.Content.ReadAsStreamAsync();
		using var doc = await JsonDocument.ParseAsync(stream);

		var root = doc.RootElement;
		root.TryGetProperty("name", out _).Should().BeTrue();
		root.TryGetProperty("version", out _).Should().BeTrue();
		root.TryGetProperty("status", out _).Should().BeTrue();
	}

	[Fact]
	public async Task Get_Health_Should_Return_200()
	{
		var client = _fact.CreateClient();

		var resp = await client.GetAsync("/");
		resp.StatusCode.Should().Be(HttpStatusCode.OK);
	}
}
