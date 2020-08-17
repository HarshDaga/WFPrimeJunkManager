using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using WFPrimeJunkManager.Data;
using WFPrimeJunkManager.Data.Models;
using WFPrimeJunkManager.Data.Persistence;

namespace WFPrimeJunkManager.ViewModels
{
	public class EquipmentPartViewModel : INotifyPropertyChanged
	{
		public string  Name           { get; set; }
		public int     Owned          { get; set; }
		public int     Needed         { get; set; }
		public int     Ducats         { get; set; }
		public bool    IsVaulted      { get; set; }
		public decimal Price          => LivePrices.Of ( _parent.Name, Name )?.CustomAvg ?? 0m;
		public int     SetsOwned      => Owned / Needed;
		public decimal DucanatorRatio => Price == 0 ? Ducats : Ducats / Price;

		public           ICommand           IncrementCommand { get; }
		public           ICommand           DecrementCommand { get; }
		private readonly EquipmentViewModel _parent;

		public EquipmentPartViewModel ( EquipmentViewModel parent, string partName, EquipmentPart part )
		{
			_parent = parent;

			Name      = partName;
			Owned     = part.Owned;
			Needed    = part.Needed;
			Ducats    = part.Ducats;
			IsVaulted = part.IsVaulted;

			IncrementCommand = new ActionCommand ( IncrementPartOwnedCount );
			DecrementCommand = new ActionCommand ( DecrementPartOwnedCount );
		}

#pragma warning disable CS0067
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

		private void IncrementPartOwnedCount ( )
		{
			Owned++;
			_parent.Equipment.Parts[Name].Owned++;
			_parent.Invalidate ( );
			Persist.Save ( Globals.PersonalData );
		}

		private void DecrementPartOwnedCount ( )
		{
			if ( Owned == 0 )
				return;

			Owned--;
			_parent.Equipment.Parts[Name].Owned--;
			_parent.Invalidate ( );
			Persist.Save ( Globals.PersonalData );
		}
	}
}