using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WFPrimeJunkManager.Data.Persistence;

namespace WFPrimeJunkManager.Data.Models
{
	public class FetchResult : IPersistent
	{
		private static readonly string FilePathInternal =
			Path.Combine ( Globals.AppDataPath, "FetchResult.json" );

		[JsonProperty ( "errors" )]
		public List<string> Errors { get; set; }

		[JsonProperty ( "timestamp" )]
		public DateTimeOffset Timestamp { get; set; }

		[JsonProperty ( "relics" )]
		public Dictionary<string, Dictionary<string, RelicInfo>> Relics { get; set; }

		// ReSharper disable once StringLiteralTypo
		[JsonProperty ( "eqmt" )]
		public Dictionary<string, EquipmentInfo> Equipment { get; set; }

		public string FilePath => FilePathInternal;

		public override string ToString ( )
		{
			return $"{Timestamp:G} | {Relics.Values.Sum ( x => x.Count )} relics | {Equipment.Count} items";
		}
	}
}