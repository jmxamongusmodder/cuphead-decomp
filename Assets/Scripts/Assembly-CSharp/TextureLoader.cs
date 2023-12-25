using System;
using UnityEngine;

// Token: 0x020003B3 RID: 947
public class TextureLoader : AssetLoader<Texture2D[]>
{
	// Token: 0x06000BB5 RID: 2997 RVA: 0x0008490A File Offset: 0x00082D0A
	protected override Coroutine loadAsset(string assetName, Action<Texture2D[]> completionHandler)
	{
		return AssetBundleLoader.LoadTextures(assetName, completionHandler);
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00084913 File Offset: 0x00082D13
	protected override Texture2D[] loadAssetSynchronous(string assetName)
	{
		return AssetBundleLoader.LoadTexturesSynchronous(assetName);
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x0008491C File Offset: 0x00082D1C
	protected override void destroyAsset(Texture2D[] asset)
	{
		for (int i = 0; i < asset.Length; i++)
		{
			UnityEngine.Object.Destroy(asset[i]);
		}
	}
}
