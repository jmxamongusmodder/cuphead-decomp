using System;

// Token: 0x020003A7 RID: 935
public class AssetLoaderOption
{
	// Token: 0x06000B88 RID: 2952 RVA: 0x00083F68 File Offset: 0x00082368
	public AssetLoaderOption(AssetLoaderOption.Type option, object context)
	{
		this.type = option;
		this.context = context;
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00083F7E File Offset: 0x0008237E
	// (set) Token: 0x06000B8A RID: 2954 RVA: 0x00083F86 File Offset: 0x00082386
	public AssetLoaderOption.Type type { get; private set; }

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06000B8B RID: 2955 RVA: 0x00083F8F File Offset: 0x0008238F
	// (set) Token: 0x06000B8C RID: 2956 RVA: 0x00083F97 File Offset: 0x00082397
	public object context { get; private set; }

	// Token: 0x06000B8D RID: 2957 RVA: 0x00083FA0 File Offset: 0x000823A0
	public static AssetLoaderOption None()
	{
		return new AssetLoaderOption(AssetLoaderOption.Type.None, null);
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00083FA9 File Offset: 0x000823A9
	public static AssetLoaderOption PersistInCache()
	{
		return new AssetLoaderOption(AssetLoaderOption.Type.PersistInCache, null);
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x00083FB2 File Offset: 0x000823B2
	public static AssetLoaderOption DontDestroyOnUnload()
	{
		return new AssetLoaderOption(AssetLoaderOption.Type.DontDestroyOnUnload, null);
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x00083FBB File Offset: 0x000823BB
	public static AssetLoaderOption PersistInCacheTagged(string tag)
	{
		return new AssetLoaderOption(AssetLoaderOption.Type.PersistInCacheTagged, tag);
	}

	// Token: 0x020003A8 RID: 936
	[Flags]
	public enum Type
	{
		// Token: 0x0400151C RID: 5404
		None = 0,
		// Token: 0x0400151D RID: 5405
		PersistInCache = 1,
		// Token: 0x0400151E RID: 5406
		DontDestroyOnUnload = 2,
		// Token: 0x0400151F RID: 5407
		PersistInCacheTagged = 4
	}
}
