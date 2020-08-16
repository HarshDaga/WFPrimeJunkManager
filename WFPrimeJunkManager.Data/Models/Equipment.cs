using System.Collections.Generic;
using JetBrains.Annotations;

namespace WFPrimeJunkManager.Data.Models
{
	[PublicAPI]
	public class Equipment
	{
		public string Name { get; set; }

		public string Type { get; set; }

		public bool IsVaulted { get; set; }

		public Dictionary<string, EquipmentPart> Parts { get; set; } = new Dictionary<string, EquipmentPart> ( );

		public void Update ( EquipmentInfo info )
		{
			Type      = info.Type;
			IsVaulted = info.IsVaulted;

			foreach ( var part in info.Parts )
			{
				var name = part.Key.Replace ( Name ?? "", "" ).Trim ( );
				if ( !Parts.ContainsKey ( name ) )
					Parts[name] = new EquipmentPart ( );
				Parts[name].Update ( part.Value );
			}
		}

		public override string ToString ( )
		{
			return $"{IsVaulted} | {Name} | {Type} | {string.Join ( ", ", Parts.Keys )}";
		}
	}
}