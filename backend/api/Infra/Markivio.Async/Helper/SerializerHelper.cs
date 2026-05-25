using System.Text.Json;

namespace Markivio.Infra.Async.Helper;

internal static class SerializerHelper {
	private readonly static JsonSerializerOptions opts = new JsonSerializerOptions() 
	{
		PropertyNameCaseInsensitive = true,
		IncludeFields = false,
		PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.KebabCaseLower,
	};

	internal static string Serialize<T>(T obj) {
		return JsonSerializer.Serialize<T> (obj, opts);
	}
}
