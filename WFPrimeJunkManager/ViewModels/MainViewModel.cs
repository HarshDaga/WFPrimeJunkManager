using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WFPrimeJunkManager.Data;
using WFPrimeJunkManager.Data.Models;
using WFPrimeJunkManager.Data.Persistence;

namespace WFPrimeJunkManager.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public Dictionary<string, List<EquipmentViewModel>> Equipments { get; set; }

		public MainViewModel ( )
		{
			Fetch ( ).Wait ( );
			LoadPersonalData ( );
			LivePrices.Init ( );

			var ungrouped = new Dictionary<string, EquipmentViewModel> ( );
			foreach ( var (name, equipment) in Globals.PersonalData.Equipments )
				ungrouped[name] = new EquipmentViewModel ( equipment );

			Equipments = ungrouped
						 .GroupBy ( x => x.Value.Type )
						 .ToDictionary ( x => x.Key, x => x.Select ( y => y.Value ).ToList ( ) );
		}
#pragma warning disable CS0067
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

		public static async Task Fetch ( )
		{
			var result = new FetchResult ( );
			if ( !Persist.Load ( ref result ) )
				result = await Fetcher.FetchAll ( );

			Persist.Save ( result );
			Globals.FetchResult = result;
		}

		public static void LoadPersonalData ( )
		{
			var data = new PersonalData ( );
			Persist.Load ( ref data );
			data.Update ( Globals.FetchResult );

			Persist.Save ( data );
			Globals.PersonalData = data;
		}
	}
}