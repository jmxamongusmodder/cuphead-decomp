public class AssetLoaderOption
{
	public enum Type
	{
		None = 0,
		PersistInCache = 1,
		DontDestroyOnUnload = 2,
		PersistInCacheTagged = 4,
	}

	public AssetLoaderOption(AssetLoaderOption.Type option, object context)
	{
	}

}
