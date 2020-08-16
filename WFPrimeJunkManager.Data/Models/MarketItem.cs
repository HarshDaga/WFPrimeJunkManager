using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace WFPrimeJunkManager.Data.Models
{
	[PublicAPI]
	public class MarketItem
	{
		public string  Name      { get; set; }
		public string  UrlName   { get; set; }
		public string  Timestamp { get; set; }
		public decimal PrevAvg   { get; set; }
		public decimal PrevVol   { get; set; }
		public decimal CurAvg    { get; set; }
		public decimal CurVol    { get; set; }
		public decimal MovingAvg { get; set; }
		public decimal CustomAvg { get; set; }

		public void PopulateFromSheetRow ( IList<object> row )
		{
			Name      = Convert.ToString ( row[0] );
			UrlName   = Convert.ToString ( row[1] );
			Timestamp = Convert.ToString ( row[2] );
			PrevAvg   = Convert.ToDecimal ( row[3] );
			PrevVol   = Convert.ToDecimal ( row[4] );
			CurAvg    = Convert.ToDecimal ( row[5] );
			CurVol    = Convert.ToDecimal ( row[6] );
			MovingAvg = Convert.ToDecimal ( row[7] );
			CustomAvg = Convert.ToDecimal ( row[8] );
		}
	}
}