using System.Collections.Generic;
using System.IO;
using WFPrimeJunkManager.Data.Persistence;

namespace WFPrimeJunkManager.Data.Models
{
	public class PersonalData : IPersistent
	{
		private static readonly string FilePathInternal =
			Path.Combine ( Globals.AppDataPath, "PersonalData.json" );

		public Dictionary<string, Equipment> Equipments { get; set; } = new Dictionary<string, Equipment> ( );

		public string FilePath => FilePathInternal;

		public void Update ( FetchResult result )
		{
			foreach ( var name in result.Equipment.Keys )
			{
				if ( !Equipments.TryGetValue ( name, out var equipment ) )
					equipment = new Equipment ( );

				equipment.Name = name;
				equipment.Update ( result.Equipment[name] );
				Equipments[name] = equipment;
			}
		}
	}
}