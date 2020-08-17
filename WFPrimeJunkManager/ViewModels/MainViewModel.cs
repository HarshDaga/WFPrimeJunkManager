using System;
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
		public IDictionary<string, List<EquipmentViewModel>> Equipments =>
			_equipments
				.ToDictionary ( x => x.Key, x => x.Value.Where ( FilterEquipment ) )
				.Where ( x => x.Value.Any ( ) )
				.ToDictionary ( x => x.Key, x => x.Value.ToList ( ) );

		public bool   ShowOnlyOwned { get; set; }
		public bool   ShowVaulted   { get; set; }
		public string SearchText    { get; set; } = "";

		public int TotalDucats => Equipments.Values
											.SelectMany ( x => x.SelectMany ( y => y.Parts.Values ) )
											.Sum ( x => x.Ducats * x.Owned );

		private readonly Dictionary<string, List<EquipmentViewModel>> _equipments;

		public MainViewModel ( )
		{
			Fetch ( ).Wait ( );
			LoadPersonalData ( );
			LivePrices.Init ( );
			ShowVaulted = true;

			var ungrouped = new Dictionary<string, EquipmentViewModel> ( );
			foreach ( var (name, equipment) in Globals.PersonalData.Equipments )
				ungrouped[name] = new EquipmentViewModel ( this, equipment );

			_equipments = ungrouped
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

		public void Invalidate ( )
		{
			PropertyChanged?.Invoke ( this, new PropertyChangedEventArgs ( nameof ( Equipments ) ) );
			PropertyChanged?.Invoke ( this, new PropertyChangedEventArgs ( nameof ( TotalDucats ) ) );
		}

		private bool FilterEquipment ( EquipmentViewModel equipment )
		{
			var result = true;
			if ( ShowOnlyOwned )
				result &= equipment.PartsOwnedForNextSet > 0;
			result &= ShowVaulted || !equipment.IsVaulted;

			if ( !string.IsNullOrWhiteSpace ( SearchText ) )
				result &= equipment.Name.Contains ( SearchText, StringComparison.OrdinalIgnoreCase ) ||
						  equipment.Parts.Values.Any (
							  x => x.Name.Contains ( SearchText, StringComparison.OrdinalIgnoreCase ) );

			return result;
		}
	}
}