using System.Linq;
using System.Threading.Tasks;
using WFPrimeJunkManager.Data;
using WFPrimeJunkManager.Data.Models;
using WFPrimeJunkManager.Data.Persistence;

namespace ScratchPad
{
	public static class Program
	{
		public static async Task Main ( )
		{
			await Fetch ( );
			LoadPersonalData ( );
			var result = Globals.FetchResult;
			var types = result.Equipment.Values
							  .GroupBy ( x => x.Type )
							  .ToDictionary ( x => x.Key, x => x.ToList ( ) );
		}

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