using System.Collections.Generic;
using Newtonsoft.Json;

namespace WFPrimeJunkManager.Data.Models
{
	public class EquipmentInfo
	{
		[JsonProperty ( "type" )]
		public string Type { get; set; }

		[JsonProperty ( "vaulted" )]
		public bool IsVaulted { get; set; }

		[JsonProperty ( "parts" )]
		public Dictionary<string, EquipmentPartInfo> Parts { get; set; }

		public override string ToString ( )
		{
			return $"{IsVaulted} | {Type} | {string.Join ( ", ", Parts.Keys )}";
		}
	}
}