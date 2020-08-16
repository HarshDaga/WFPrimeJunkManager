using Newtonsoft.Json;

namespace WFPrimeJunkManager.Data.Models
{
	public class RelicInfo
	{
		[JsonProperty ( "vaulted" )]
		public bool IsVaulted { get; set; }

		[JsonProperty ( "rare1" )]
		public string Rare { get; set; }

		[JsonProperty ( "uncommon1" )]
		public string Uncommon1 { get; set; }

		[JsonProperty ( "uncommon2" )]
		public string Uncommon2 { get; set; }

		[JsonProperty ( "common1" )]
		public string Common1 { get; set; }

		[JsonProperty ( "common2" )]
		public string Common2 { get; set; }

		[JsonProperty ( "common3" )]
		public string Common3 { get; set; }

		public override string ToString ( )
		{
			return $"{IsVaulted} | {Rare} | {Uncommon1}, {Uncommon2} | {Common1}, {Common2}, {Common3}";
		}
	}
}