using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using JetBrains.Annotations;
using WFPrimeJunkManager.Data.Models;

namespace WFPrimeJunkManager.Data
{
	[PublicAPI]
	public static class LivePrices
	{
		[SuppressMessage ( "ReSharper", "StringLiteralTypo" )]
		private const string SheetId = "1uAbqfwBYrcqlWCJad4juankmu9-_UqzBmd7XtrrrwkM";

		[SuppressMessage ( "ReSharper", "StringLiteralTypo" )]
		private const string CredsJson = @"
{
	""type"": ""service_account"",
	""project_id"": ""wfinfo"",
	""private_key_id"": ""a99126b110941194abf09b95afb357799464265d"",
	""private_key"":
		""-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC4wcaLYg0Njl5Y\nDIwzv7itzNezVtlgOhByiC0O2WQRikUVGorpkafRjFxJ0g38E5QfeC4QFycIUik5\nT3ilmzZZzDUCFD5s1RLEl6vMZvb+vbWZq0DlzZHZcBhirslLTD5HMsI2v2Hl5SeS\n9rEAYVzNHZqB1E+zpGfdd7YL3ZSceOvIAKzmeTiQ485PN6f6Udsxp6jx23epYTr0\nzfaj6AYr3+EL7drQwn1DM4AQDp1ixggL08CETZwj76youV2MsuYZYQuq4hwNC0yZ\ndwfqAbUhWSeHmYbmc8JD9Y/hXRXRGDTGnl/jZjBSAge9Oqd78XpqMfpNujVdCxJ5\n/lDpTfrlAgMBAAECggEACfUHikOCiJR/qScj9zYB8fv0NAoeNP1sHYARTZeRuRBG\noNiJXAUpkjWKU8AqWMeFWBzV/rAAvoYPCrOpwOMYjsZepPoKUzs4g7fY3m8+KIR4\n4QtFuBzYG2vbJJZe6tv1D0lBBkpSMF2lQFpjN64HnlZWGWEj2n6lyl3FReZvN++E\n4rT1jNAooIjzFJ0tWwm3rjP8kmPUHpXkwv3ok2+v3p7g/pYKX54poexWrT/Wn4qw\nq2XzEhWfT54lqwL0wkrjGpC1zLgdqbnNTWriowwFfAaVaTyJqkfNx0RNgVRgFZ/g\nJoYl4HSsrczDUxrVrZcjUBeK71N7qp21QQXPswGDAQKBgQDq2H4E5VOhX1HMw/EU\nFu62Ugpey9kE/8hlEsJs5yyVOFZWZ2FNSpbLnGpH5aHJuTvIQrfjo4I0KDtTZvAk\n+OgKwO44n5Jm9O2o3b6Gyk/5hP6aMeTzdEtKbJvS51Xr8b8YvAkJh1H+PsvcwH/F\nDvXfTtFVklAZPW7F8sIauqSR1QKBgQDJZj6Bm0LEqTF9hEyF9NmGSujCAN7lS86E\npBkyFW3K8wxS3y7WwPxYo75y54+gZCde8hZ3HQw/Te+wcGsDbmsU7ziGHJFYxBPa\nS4H+zzGMo+MAKAvr9RiEqkMRg+TZtZo2ikWUgQD6hVX6SkWzI0ioyf4kFtqsD9ru\nZWHZWTE80QKBgConulc+owhwh8pt0bR9eVQY3euuQ8J1947NE0FhLcuLVVQlMn4h\nSXg7F4jYW3ZOXcDQ2RlvnEuofR23eJvqYhysDRb07d61UIPjafPgFQMBMIKVOjfJ\nREqTvFTbWb0Eo/zYo/al573vn8B8fXLuAIyZJVJq9R3SvTOjI9yQbEIRAoGBAKgv\nixle2BY1GNAx0Fm5jIH6Qn/ojDs94Buikivh+0sVRwBZwtqyVMmNDHkWaTnPCZXf\nYuVby6N96SEV8DfwWNoln7VXXAWd0NpmXgu1aTKClgnGZ5ZHmo8HRHT3CQDKCBtt\nwwdt56xN9uvKZIRhfcb+0A3BTCGOKA0Xeuwv8M9RAoGBAIe+G2TDfwJrNBK6T+Bq\nSf9sdr8rI5M69AZTJeBrBP3C8ZmqES4EKWTG3XoNtkTZAT2JxIWoWFTokKWs7vMF\n2NE6WqikpeoPeYthQpo1lCCcv5NbdiDS6BKy77vDrNCbUJZpkeA2BWr7xdunO/AH\nyddU15rz/DoRfyT/6tK+ktMT\n-----END PRIVATE KEY-----\n"",
	""client_email"": ""wfinfo-reader@wfinfo.iam.gserviceaccount.com"",
	""client_id"": ""115531611173584956181"",
	""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
	""token_uri"": ""https://oauth2.googleapis.com/token"",
	""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
	""client_x509_cert_url"":
		""https://www.googleapis.com/robot/v1/metadata/x509/wfinfo-reader%40wfinfo.iam.gserviceaccount.com""
}";

		private static readonly SheetsService Service;

		public static Dictionary<string, MarketItem> MarketItems;

#pragma warning disable CA1810 // Initialize reference type static fields inline
		static LivePrices ( )
#pragma warning restore CA1810 // Initialize reference type static fields inline
		{
			var cred = GoogleCredential
					   .FromJson ( CredsJson )
					   .CreateScoped ( SheetsService.Scope.SpreadsheetsReadonly );
			Service = new SheetsService ( new BaseClientService.Initializer
			{
				HttpClientInitializer = cred,
				ApplicationName       = "WFInfo"
			} );

			MarketItems = new Dictionary<string, MarketItem> ( );
		}

		public static IList<IList<object>> GetRange ( string range )
		{
			var response = Service.Spreadsheets.Values.Get ( SheetId, range ).Execute ( );

			return response.Values;
		}

		public static void Init ( )
		{
			var rows = GetRange ( "prices!A:I" );
			foreach ( var row in rows.Skip ( 1 ) )
			{
				var item = new MarketItem ( );
				item.PopulateFromSheetRow ( row );
				MarketItems[item.Name] = item;
			}
		}

		public static MarketItem Of ( string name )
		{
			return MarketItems.TryGetValue ( name, out var item ) ? item : null;
		}

		public static MarketItem Of ( string equipmentName, string partName )
		{
			return Of ( $"{equipmentName} {partName}" );
		}
	}
}