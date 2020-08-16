using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using JetBrains.Annotations;
using Newtonsoft.Json;
using WFPrimeJunkManager.Data.Models;

namespace WFPrimeJunkManager.Data
{
	[PublicAPI]
	public static class Fetcher
	{
		public const string AllDataUrl =
			"https://docs.google.com/uc?id=1zqI55GqcXMfbvZgBjASC34ad71GDTkta&export=download";

		public static async Task<FetchResult> FetchAll ( CancellationToken ct = default )
		{
			var json   = await AllDataUrl.GetStringAsync ( ct );
			var result = JsonConvert.DeserializeObject<FetchResult> ( json );
			return result;
		}
	}
}