using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WFPrimeJunkManager.Data;
using WFPrimeJunkManager.Data.Models;

namespace WFPrimeJunkManager.ViewModels
{
	public class EquipmentViewModel : INotifyPropertyChanged
	{
		public Equipment Equipment { get; }

		public string  Name             { get; }
		public string  Type             { get; }
		public bool    IsVaulted        { get; }
		public decimal Price            => LivePrices.Of ( $"{Name} Set" )?.CustomAvg ?? 0m;
		public int     Ducats           { get; }
		public int     TotalPartsNeeded { get; }

		public int PartsOwnedForNextSet =>
			Parts.Values.Sum ( x => Math.Min ( x.Owned - SetsOwned * x.Needed, x.Needed ) );

		public int SetsOwned => Parts.Values.Min ( x => x.SetsOwned );

		public Dictionary<string, EquipmentPartViewModel> Parts { get; set; } =
			new Dictionary<string, EquipmentPartViewModel> ( );

		public EquipmentViewModel ( Equipment equipment )
		{
			Equipment = equipment;
			Name      = equipment.Name;
			Type      = equipment.Type;
			IsVaulted = equipment.IsVaulted;

			Ducats           = equipment.Parts.Values.Sum ( x => x.Ducats * x.Needed );
			TotalPartsNeeded = equipment.Parts.Values.Sum ( x => x.Needed );

			foreach ( var (name, part) in equipment.Parts )
				Parts[name] = new EquipmentPartViewModel ( this, name, part );
		}
#pragma warning disable CS0067
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

		public void Invalidate ( )
		{
			PropertyChanged?.Invoke ( this, new PropertyChangedEventArgs ( nameof ( SetsOwned ) ) );
			PropertyChanged?.Invoke ( this, new PropertyChangedEventArgs ( nameof ( Parts ) ) );
			PropertyChanged?.Invoke ( this, new PropertyChangedEventArgs ( nameof ( PartsOwnedForNextSet ) ) );
		}
	}
}