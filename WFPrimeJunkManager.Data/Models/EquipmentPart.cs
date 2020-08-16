namespace WFPrimeJunkManager.Data.Models
{
	public class EquipmentPart
	{
		public int  Owned     { get; set; }
		public int  Needed    { get; set; }
		public int  Ducats    { get; set; }
		public bool IsVaulted { get; set; }

		public int SetsOwned => Owned / Needed;

		public void Update ( EquipmentPartInfo info )
		{
			Needed    = info.Count;
			Ducats    = info.Ducats;
			IsVaulted = info.IsVaulted;
		}

		public override string ToString ( )
		{
			return $"{IsVaulted} | {Owned}/{Needed} | {Ducats} ducats";
		}
	}
}