using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace WFPrimeJunkManager.Data.Persistence
{
	[PublicAPI]
	public static class Persist
	{
		public static void Save<T> ( T data )
			where T : IPersistent
		{
			var json = JsonConvert.SerializeObject ( data, Formatting.Indented );
			var dir  = Path.GetDirectoryName ( data.FilePath );
			if ( dir != null )
				Directory.CreateDirectory ( dir );
			File.WriteAllText ( data.FilePath, json );
		}

		public static bool Load<T> ( ref T data )
			where T : IPersistent
		{
			if ( data is null || !File.Exists ( data.FilePath ) )
				return false;

			var json = File.ReadAllText ( data.FilePath );
			data = JsonConvert.DeserializeObject<T> ( json );

			return true;
		}

		public static T Load<T> ( string path )
			where T : IPersistent
		{
			if ( !File.Exists ( path ) )
				return default;

			var json = File.ReadAllText ( path );
			var data = JsonConvert.DeserializeObject<T> ( json );

			return data;
		}
	}
}