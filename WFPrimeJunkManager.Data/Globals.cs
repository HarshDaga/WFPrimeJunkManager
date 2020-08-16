using System;
using System.IO;
using JetBrains.Annotations;
using WFPrimeJunkManager.Data.Models;

namespace WFPrimeJunkManager.Data
{
	[PublicAPI]
	public static class Globals
	{
		public const string AppFolderName = "WFPrimeJunkManager";

		public static readonly string AppDataPath =
			Path.Combine ( Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData ), AppFolderName );

		public static FetchResult  FetchResult  { get; set; }
		public static PersonalData PersonalData { get; set; }
	}
}