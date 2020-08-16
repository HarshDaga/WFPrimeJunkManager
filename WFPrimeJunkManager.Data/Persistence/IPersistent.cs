using Newtonsoft.Json;

namespace WFPrimeJunkManager.Data.Persistence
{
	public interface IPersistent
	{
		[JsonIgnore]
		string FilePath { get; }
	}
}