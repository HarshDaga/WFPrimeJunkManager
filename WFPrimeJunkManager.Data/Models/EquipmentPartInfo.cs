using Newtonsoft.Json;

namespace WFPrimeJunkManager.Data.Models
{
	public class EquipmentPartInfo
	{
		[JsonProperty ( "count" )]
		public int Count { get; set; }

		[JsonProperty ( "ducats" )]
		public int Ducats { get; set; }

		[JsonProperty ( "vaulted" )]
		public bool IsVaulted { get; set; }

		public override string ToString ( )
		{
			return $"{IsVaulted} | {Count} | {Ducats} ducats";
		}
	}
}