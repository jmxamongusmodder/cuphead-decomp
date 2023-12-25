using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

// Token: 0x020003B2 RID: 946
public class SpriteAtlasLoader : AssetLoader<SpriteAtlas>
{
	// Token: 0x06000BAC RID: 2988 RVA: 0x0008471E File Offset: 0x00082B1E
	private void OnEnable()
	{
		SpriteAtlasManager.atlasRequested += this.atlasRequestedHandler;
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00084731 File Offset: 0x00082B31
	private void OnDisable()
	{
		SpriteAtlasManager.atlasRequested -= this.atlasRequestedHandler;
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x00084744 File Offset: 0x00082B44
	protected override Coroutine loadAsset(string assetName, Action<SpriteAtlas> completionHandler)
	{
		Action<SpriteAtlas> completionHandler2 = delegate(SpriteAtlas atlas)
		{
			this.resolveDeferredRequests(assetName, atlas);
			completionHandler(atlas);
		};
		return AssetBundleLoader.LoadSpriteAtlas(assetName, completionHandler2);
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x00084785 File Offset: 0x00082B85
	protected override SpriteAtlas loadAssetSynchronous(string assetName)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x0008478C File Offset: 0x00082B8C
	protected override void destroyAsset(SpriteAtlas asset)
	{
		UnityEngine.Object.Destroy(asset);
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x00084794 File Offset: 0x00082B94
	private void addDeferredRequest(string assetName, Action<SpriteAtlas> action)
	{
		List<Action<SpriteAtlas>> list;
		if (!this.deferredAtlastRequests.TryGetValue(assetName, out list))
		{
			list = new List<Action<SpriteAtlas>>();
			this.deferredAtlastRequests.Add(assetName, list);
		}
		list.Add(action);
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x000847D0 File Offset: 0x00082BD0
	private void resolveDeferredRequests(string assetName, SpriteAtlas atlas)
	{
		if (atlas == null)
		{
			return;
		}
		List<Action<SpriteAtlas>> list;
		if (this.deferredAtlastRequests.TryGetValue(assetName, out list))
		{
			foreach (Action<SpriteAtlas> action in list)
			{
				action(atlas);
			}
			this.deferredAtlastRequests.Remove(assetName);
		}
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00084854 File Offset: 0x00082C54
	private void atlasRequestedHandler(string atlasTag, Action<SpriteAtlas> action)
	{
		Action<SpriteAtlas> completionAction = delegate(SpriteAtlas atlas)
		{
			if (atlas == null)
			{
				this.addDeferredRequest(atlasTag, action);
			}
			else
			{
				action(atlas);
			}
		};
		base.loadAssetFromAssetBundle(atlasTag, AssetLoaderOption.None(), completionAction);
	}

	// Token: 0x0400156A RID: 5482
	private Dictionary<string, List<Action<SpriteAtlas>>> deferredAtlastRequests = new Dictionary<string, List<Action<SpriteAtlas>>>();
}
