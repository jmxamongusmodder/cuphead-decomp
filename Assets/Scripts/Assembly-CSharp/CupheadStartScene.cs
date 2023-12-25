using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

// Token: 0x020003EE RID: 1006
public class CupheadStartScene : AbstractMonoBehaviour
{
	// Token: 0x06000D9B RID: 3483 RVA: 0x0008E649 File Offset: 0x0008CA49
	protected override void Awake()
	{
		Application.targetFrameRate = 60;
		Cuphead.Init(true);
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x0008E658 File Offset: 0x0008CA58
	private void Start()
	{
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x0008E668 File Offset: 0x0008CA68
	private IEnumerator start_cr()
	{
		yield return null;
		yield return null;
		AssetLoader<Texture2D[]>.LoadAssetSynchronous("screen_fx", AssetLoaderOption.DontDestroyOnUnload());
		UnityEngine.Object.FindObjectOfType<ChromaticAberrationFilmGrain>().Initialize(AssetLoader<Texture2D[]>.GetCachedAsset("screen_fx"));
		if (PlatformHelper.ForceAdditionalHeapMemory)
		{
			HeapAllocator.Allocate(100);
			yield return null;
			yield return null;
		}
		if (PlatformHelper.PreloadSettingsData)
		{
			OnlineManager.Instance.Init();
			SettingsData.LoadFromCloud(new SettingsData.SettingsDataLoadFromCloudHandler(this.OnSettingsDataLoaded));
			while (!this.settingsDataLoaded)
			{
				yield return null;
			}
		}
		StartScreen.InitialLoadData startScreenLoadData = new StartScreen.InitialLoadData();
		PlatformHandlingTitleScreenOverride titleScreenOverride = new PlatformHandlingTitleScreenOverride(startScreenLoadData);
		yield return base.StartCoroutine(titleScreenOverride.GetTitleScreenOverrideStatus_cr(this));
		StartScreen.initialLoadData = startScreenLoadData;
		titleScreenOverride = null;
		Coroutine[] fontCoroutines = FontLoader.Initialize();
		foreach (Coroutine coroutine in fontCoroutines)
		{
			yield return coroutine;
		}
		while (AssetBundleLoader.loadCounter > 0 || !AssetLoader<SpriteAtlas>.persistentAssetsLoaded || !AssetLoader<AudioClip>.persistentAssetsLoaded || !AssetLoader<Texture2D[]>.persistentAssetsLoaded)
		{
			yield return null;
		}
		yield return null;
		Cuphead.Init(false);
		yield return new WaitForSeconds(0.1f);
		DLCManager.RefreshDLC();
		yield return null;
		yield return null;
		Coroutine[] coroutines = DLCManager.LoadPersistentAssets();
		if (coroutines != null)
		{
			foreach (Coroutine coroutine2 in coroutines)
			{
				yield return coroutine2;
			}
			yield return null;
			yield return null;
		}
		string titleSceneName = "scene_title";
		string[] preloadAtlases = AssetLoader<SpriteAtlas>.GetPreloadAssetNames(titleSceneName);
		foreach (string atlas in preloadAtlases)
		{
			yield return AssetLoader<SpriteAtlas>.LoadAsset(atlas, AssetLoaderOption.None());
		}
		string[] preloadMusic = AssetLoader<AudioClip>.GetPreloadAssetNames(titleSceneName);
		foreach (string clip in preloadMusic)
		{
			yield return AssetLoader<AudioClip>.LoadAsset(clip, AssetLoaderOption.None());
		}
		yield return null;
		yield return null;
		SceneManager.LoadSceneAsync(1);
		yield break;
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x0008E683 File Offset: 0x0008CA83
	private void OnSettingsDataLoaded(bool success)
	{
		if (!success)
		{
			SettingsData.LoadFromCloud(new SettingsData.SettingsDataLoadFromCloudHandler(this.OnSettingsDataLoaded));
			return;
		}
		this.settingsDataLoaded = true;
	}

	// Token: 0x04001713 RID: 5907
	private bool settingsDataLoaded;
}
