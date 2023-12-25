using System;
using UnityEngine;

// Token: 0x020003AF RID: 943
public class MusicLoader : AssetLoader<AudioClip>
{
	// Token: 0x06000BA3 RID: 2979 RVA: 0x0008463B File Offset: 0x00082A3B
	protected override Coroutine loadAsset(string assetName, Action<AudioClip> completionHandler)
	{
		return AssetBundleLoader.LoadMusic(assetName, completionHandler);
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x00084644 File Offset: 0x00082A44
	protected override AudioClip loadAssetSynchronous(string assetName)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x0008464B File Offset: 0x00082A4B
	protected override void destroyAsset(AudioClip asset)
	{
		UnityEngine.Object.Destroy(asset);
	}
}
