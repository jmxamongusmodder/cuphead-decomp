using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

// Token: 0x020003A1 RID: 929
public class AssetBundleLoader : MonoBehaviour
{
	// Token: 0x06000B5A RID: 2906 RVA: 0x00082E32 File Offset: 0x00081232
	private void Awake()
	{
		if (AssetBundleLoader.Instance != null)
		{
			throw new Exception("Should only be one instance");
		}
		AssetBundleLoader.Instance = this;
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00082E58 File Offset: 0x00081258
	public static void UnloadAssetBundles()
	{
		foreach (KeyValuePair<string, AssetBundleLoader.AssetBundleContainer> keyValuePair in AssetBundleLoader.Instance.loadedBundles)
		{
			keyValuePair.Value.assetBundle.Unload(false);
		}
		AssetBundleLoader.Instance.loadedBundles.Clear();
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00082ED4 File Offset: 0x000812D4
	public static Coroutine LoadSpriteAtlas(string atlasName, Action<SpriteAtlas> completionHandler)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		if (AssetBundleLoader.Instance.atlasLocationDatabase.dlcAssets.Contains(atlasName))
		{
			location = AssetBundleLoader.AssetBundleLocation.DLC;
		}
		string spriteAtlasBundleName = AssetBundleLoader.GetSpriteAtlasBundleName(atlasName);
		return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAsset<SpriteAtlas>(spriteAtlasBundleName, location, atlasName, completionHandler));
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x00082F20 File Offset: 0x00081320
	public static Coroutine LoadMusic(string audioClipName, Action<AudioClip> completionHandler)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		if (AssetBundleLoader.Instance.musicLocationDatabase.dlcAssets.Contains(audioClipName))
		{
			location = AssetBundleLoader.AssetBundleLocation.DLC;
		}
		string musicBundleName = AssetBundleLoader.GetMusicBundleName(audioClipName);
		return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAsset<AudioClip>(musicBundleName, location, audioClipName, completionHandler));
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x00082F6C File Offset: 0x0008136C
	public static Coroutine LoadFont(string bundleName, string assetName, Action<Font> completionHandler)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		bundleName = AssetBundleLoader.AssetBundlePrefixFont + bundleName.ToLowerInvariant();
		return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAsset<Font>(bundleName, location, assetName, completionHandler));
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00082FA8 File Offset: 0x000813A8
	public static Coroutine LoadTMPFont(string bundleName, Action<UnityEngine.Object[]> completionHandler)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		bundleName = AssetBundleLoader.AssetBundlePrefixTMPFont + bundleName.ToLowerInvariant();
		return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAllAssets<UnityEngine.Object>(bundleName, location, completionHandler));
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x00082FE0 File Offset: 0x000813E0
	public static Coroutine LoadTextures(string bundleName, Action<Texture2D[]> completionHandler)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		bundleName = AssetBundleLoader.AssetBundlePrefixTexture + bundleName.ToLowerInvariant();
		return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAllAssets<Texture2D>(bundleName, location, completionHandler));
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x00083018 File Offset: 0x00081418
	public static Texture2D[] LoadTexturesSynchronous(string bundleName)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		bundleName = AssetBundleLoader.AssetBundlePrefixTexture + bundleName.ToLowerInvariant();
		return AssetBundleLoader.Instance.loadAllAssetsSynchronous<Texture2D>(bundleName, location);
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x00083048 File Offset: 0x00081448
	private IEnumerator loadAssetBundle(string assetBundleName, AssetBundleLoader.AssetBundleLocation location)
	{
		AssetBundleLoader.loadCounter++;
		string path = AssetBundleLoader.getBasePath(location);
		path = Path.Combine(path, "AssetBundles");
		path = Path.Combine(path, assetBundleName);
		AssetBundle assetBundle;
		if (location == AssetBundleLoader.AssetBundleLocation.DLC && DLCManager.UsesAlternateBundleLoadingMechanism())
		{
			DLCManager.AssetBundleLoadWaitInstruction waitInstruction = DLCManager.LoadAssetBundle(path);
			yield return waitInstruction;
			assetBundle = waitInstruction.assetBundle;
		}
		else
		{
			AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
			yield return request;
			assetBundle = request.assetBundle;
		}
		this.loadedBundles.Add(assetBundleName, new AssetBundleLoader.AssetBundleContainer(assetBundle, location));
		AssetBundleLoader.loadCounter--;
		yield break;
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00083074 File Offset: 0x00081474
	private AssetBundleLoader.AssetBundleContainer loadAssetBundleSynchronous(string assetBundleName, AssetBundleLoader.AssetBundleLocation location)
	{
		string text = AssetBundleLoader.getBasePath(location);
		text = Path.Combine(text, "AssetBundles");
		text = Path.Combine(text, assetBundleName);
		AssetBundle assetBundle = AssetBundle.LoadFromFile(text);
		AssetBundleLoader.AssetBundleContainer assetBundleContainer = new AssetBundleLoader.AssetBundleContainer(assetBundle, location);
		this.loadedBundles.Add(assetBundleName, assetBundleContainer);
		return assetBundleContainer;
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x000830BC File Offset: 0x000814BC
	private IEnumerator loadAsset<T>(string assetBundleName, AssetBundleLoader.AssetBundleLocation location, string assetName, Action<T> completionHandler) where T : UnityEngine.Object
	{
		AssetBundleLoader.loadCounter++;
		AssetBundleLoader.AssetBundleContainer assetBundleContainer;
		if (!this.loadedBundles.TryGetValue(assetBundleName, out assetBundleContainer))
		{
			yield return base.StartCoroutine(this.loadAssetBundle(assetBundleName, location));
			assetBundleContainer = this.loadedBundles[assetBundleName];
		}
		AssetBundleRequest assetRequest = assetBundleContainer.assetBundle.LoadAssetAsync<T>(assetName);
		yield return assetRequest;
		completionHandler(assetRequest.asset as T);
		if (assetBundleContainer.location == AssetBundleLoader.AssetBundleLocation.DLC && DLCManager.UnloadBundlesImmediately() && typeof(T) == typeof(SpriteAtlas))
		{
			this.loadedBundles.Remove(assetBundleContainer.assetBundle.name);
			assetBundleContainer.assetBundle.Unload(false);
		}
		AssetBundleLoader.loadCounter--;
		yield break;
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x000830F4 File Offset: 0x000814F4
	private IEnumerator loadAllAssets<T>(string assetBundleName, AssetBundleLoader.AssetBundleLocation location, Action<T[]> completionHandler) where T : UnityEngine.Object
	{
		AssetBundleLoader.loadCounter++;
		AssetBundleLoader.AssetBundleContainer assetBundleContainer;
		if (!this.loadedBundles.TryGetValue(assetBundleName, out assetBundleContainer))
		{
			yield return base.StartCoroutine(this.loadAssetBundle(assetBundleName, location));
			assetBundleContainer = this.loadedBundles[assetBundleName];
		}
		AssetBundleRequest assetRequest = assetBundleContainer.assetBundle.LoadAllAssetsAsync<T>();
		yield return assetRequest;
		UnityEngine.Object[] allAssets = assetRequest.allAssets;
		T[] castAssets = new T[allAssets.Length];
		for (int i = 0; i < allAssets.Length; i++)
		{
			castAssets[i] = (T)((object)allAssets[i]);
		}
		completionHandler(castAssets);
		AssetBundleLoader.loadCounter--;
		yield break;
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x00083124 File Offset: 0x00081524
	private T[] loadAllAssetsSynchronous<T>(string assetBundleName, AssetBundleLoader.AssetBundleLocation location) where T : UnityEngine.Object
	{
		AssetBundleLoader.AssetBundleContainer assetBundleContainer;
		if (!this.loadedBundles.TryGetValue(assetBundleName, out assetBundleContainer))
		{
			assetBundleContainer = this.loadAssetBundleSynchronous(assetBundleName, location);
		}
		return assetBundleContainer.assetBundle.LoadAllAssets<T>();
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00083158 File Offset: 0x00081558
	private static string getBasePath(AssetBundleLoader.AssetBundleLocation location)
	{
		if (location == AssetBundleLoader.AssetBundleLocation.DLC)
		{
			return DLCManager.AssetBundlePath();
		}
		return Application.streamingAssetsPath;
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x0008316C File Offset: 0x0008156C
	public static string GetSpriteAtlasBundleName(string atlasName)
	{
		return AssetBundleLoader.AssetBundlePrefixSpriteAtlas + atlasName.ToLowerInvariant();
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x0008317E File Offset: 0x0008157E
	public static string GetMusicBundleName(string audioClipName)
	{
		return AssetBundleLoader.AssetBundlePrefixMusic + audioClipName.ToLowerInvariant();
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x00083190 File Offset: 0x00081590
	public static List<string> DEBUG_LoadedAssetBundles()
	{
		return new List<string>(AssetBundleLoader.Instance.loadedBundles.Keys);
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06000B6B RID: 2923 RVA: 0x000831A6 File Offset: 0x000815A6
	// (set) Token: 0x06000B6C RID: 2924 RVA: 0x000831AD File Offset: 0x000815AD
	public static int loadCounter { get; private set; }

	// Token: 0x04001500 RID: 5376
	public static readonly string AssetBundlePrefixSpriteAtlas = "atlas_";

	// Token: 0x04001501 RID: 5377
	public static readonly string AssetBundlePrefixMusic = "music_";

	// Token: 0x04001502 RID: 5378
	public static readonly string AssetBundlePrefixFont = "font_";

	// Token: 0x04001503 RID: 5379
	public static readonly string AssetBundlePrefixTMPFont = "tmpfont_";

	// Token: 0x04001504 RID: 5380
	public static readonly string AssetBundlePrefixTexture = "tex_";

	// Token: 0x04001505 RID: 5381
	private static AssetBundleLoader Instance;

	// Token: 0x04001506 RID: 5382
	[SerializeField]
	private AssetLocationDatabase atlasLocationDatabase;

	// Token: 0x04001507 RID: 5383
	[SerializeField]
	private AssetLocationDatabase musicLocationDatabase;

	// Token: 0x04001508 RID: 5384
	private Dictionary<string, AssetBundleLoader.AssetBundleContainer> loadedBundles = new Dictionary<string, AssetBundleLoader.AssetBundleContainer>();

	// Token: 0x020003A2 RID: 930
	private enum AssetBundleLocation
	{
		// Token: 0x0400150B RID: 5387
		StreamingAssets,
		// Token: 0x0400150C RID: 5388
		DLC
	}

	// Token: 0x020003A3 RID: 931
	private class AssetBundleContainer
	{
		// Token: 0x06000B6E RID: 2926 RVA: 0x000831E9 File Offset: 0x000815E9
		public AssetBundleContainer(AssetBundle assetBundle, AssetBundleLoader.AssetBundleLocation location)
		{
			this.assetBundle = assetBundle;
			this.location = location;
		}

		// Token: 0x0400150D RID: 5389
		public AssetBundle assetBundle;

		// Token: 0x0400150E RID: 5390
		public AssetBundleLoader.AssetBundleLocation location;
	}
}
