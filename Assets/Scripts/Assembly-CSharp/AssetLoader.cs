using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003A4 RID: 932
public abstract class AssetLoader<T> : MonoBehaviour where T : class
{
	// Token: 0x06000B70 RID: 2928 RVA: 0x000836FF File Offset: 0x00081AFF
	private void Awake()
	{
		if (AssetLoader<T>.Instance != null)
		{
			throw new Exception("More than one instance found");
		}
		AssetLoader<T>.Instance = this;
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x00083722 File Offset: 0x00081B22
	private void Start()
	{
		base.StartCoroutine(this.loadPersistentAssets());
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x00083734 File Offset: 0x00081B34
	private IEnumerator loadPersistentAssets()
	{
		if (this.sceneAssetDatabase != null)
		{
			foreach (string assetName in this.sceneAssetDatabase.persistentAssets)
			{
				yield return this.loadAssetFromAssetBundle(assetName, AssetLoaderOption.PersistInCache(), null);
			}
		}
		AssetLoader<T>.persistentAssetsLoaded = true;
		yield break;
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x00083750 File Offset: 0x00081B50
	public static string[] GetPreloadAssetNames(string sceneName)
	{
		string[] result;
		if (!AssetLoader<T>.Instance.sceneAssetDatabase.sceneAssetMappings.TryGetValue(sceneName, out result))
		{
			return new string[0];
		}
		return result;
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x00083781 File Offset: 0x00081B81
	public static Coroutine LoadAsset(string assetName, AssetLoaderOption option)
	{
		return AssetLoader<T>.Instance.loadAssetFromAssetBundle(assetName, option, null);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x00083790 File Offset: 0x00081B90
	public static T LoadAssetSynchronous(string assetName, AssetLoaderOption option)
	{
		return AssetLoader<T>.Instance.loadAssetFromAssetBundleSynchronous(assetName, option);
	}

	// Token: 0x06000B76 RID: 2934
	protected abstract Coroutine loadAsset(string assetName, Action<T> completionHandler);

	// Token: 0x06000B77 RID: 2935
	protected abstract T loadAssetSynchronous(string assetName);

	// Token: 0x06000B78 RID: 2936 RVA: 0x0008379E File Offset: 0x00081B9E
	public static Coroutine LoadPersistentAssetsDLC()
	{
		return AssetLoader<T>.Instance.loadPersistentAssetsDLC();
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x000837AA File Offset: 0x00081BAA
	private Coroutine loadPersistentAssetsDLC()
	{
		return base.StartCoroutine(this.loadPersistentAssetsDLC_cr());
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x000837B8 File Offset: 0x00081BB8
	private IEnumerator loadPersistentAssetsDLC_cr()
	{
		foreach (string assetName in this.sceneAssetDatabase.persistentAssetsDLC)
		{
			yield return this.loadAssetFromAssetBundle(assetName, AssetLoaderOption.PersistInCache(), null);
		}
		yield break;
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x000837D4 File Offset: 0x00081BD4
	public static T GetCachedAsset(string assetName)
	{
		T result;
		if (AssetLoader<T>.Instance.tryGetAsset(assetName, out result))
		{
			return result;
		}
		throw new Exception("Asset not cached: " + assetName);
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x00083808 File Offset: 0x00081C08
	public static void UnloadAssets(params string[] persistentTagsToUnload)
	{
		List<string> list = new List<string>(AssetLoader<T>.Instance.loadedAssets.Keys);
		for (int i = list.Count - 1; i >= 0; i--)
		{
			AssetLoader<T>.AssetContainer<T> assetContainer = AssetLoader<T>.Instance.loadedAssets[list[i]];
			if ((assetContainer.assetOption.type & AssetLoaderOption.Type.PersistInCache) != AssetLoaderOption.Type.None)
			{
				list.RemoveAt(i);
			}
			else if ((assetContainer.assetOption.type & AssetLoaderOption.Type.PersistInCacheTagged) != AssetLoaderOption.Type.None && Array.IndexOf<string>(persistentTagsToUnload, (string)assetContainer.assetOption.context) < 0)
			{
				list.RemoveAt(i);
			}
			else if ((assetContainer.assetOption.type & AssetLoaderOption.Type.DontDestroyOnUnload) == AssetLoaderOption.Type.None)
			{
				AssetLoader<T>.Instance.destroyAsset(assetContainer.asset);
			}
		}
		foreach (string key in list)
		{
			AssetLoader<T>.Instance.loadedAssets.Remove(key);
		}
	}

	// Token: 0x06000B7D RID: 2941
	protected abstract void destroyAsset(T asset);

	// Token: 0x06000B7E RID: 2942 RVA: 0x00083930 File Offset: 0x00081D30
	private void cacheAsset(string assetName, AssetLoader<T>.AssetContainer<T> container)
	{
		this.loadedAssets.Add(assetName, container);
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00083940 File Offset: 0x00081D40
	protected bool tryGetAsset(string assetName, out T asset)
	{
		asset = (T)((object)null);
		AssetLoader<T>.AssetContainer<T> assetContainer;
		if (this.loadedAssets.TryGetValue(assetName, out assetContainer))
		{
			asset = assetContainer.asset;
			return true;
		}
		return false;
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x0008397C File Offset: 0x00081D7C
	protected Coroutine loadAssetFromAssetBundle(string assetName, AssetLoaderOption option, Action<T> completionAction)
	{
		T obj;
		if (this.tryGetAsset(assetName, out obj))
		{
			if (completionAction != null)
			{
				completionAction(obj);
			}
			return null;
		}
		if (!DLCManager.DLCEnabled() && this.assetLocationDatabase.dlcAssets.Contains(assetName))
		{
			if (completionAction != null)
			{
				completionAction((T)((object)null));
			}
			return null;
		}
		AssetLoader<T>.LoadOperation loadOperation;
		if (!this.loadOperations.TryGetValue(assetName, out loadOperation))
		{
			loadOperation = new AssetLoader<T>.LoadOperation();
			this.loadOperations.Add(assetName, loadOperation);
			loadOperation.coroutine = this.loadAsset(assetName, delegate(T asset)
			{
				this.cacheAsset(assetName, new AssetLoader<T>.AssetContainer<T>(asset, option));
				AssetLoader<T>.LoadOperation loadOperation2 = this.loadOperations[assetName];
				foreach (Action<T> action in loadOperation2.completionHandlers)
				{
					if (action != null)
					{
						action(asset);
					}
				}
				this.loadOperations.Remove(assetName);
			});
		}
		loadOperation.completionHandlers.Add(completionAction);
		return loadOperation.coroutine;
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x00083A60 File Offset: 0x00081E60
	protected T loadAssetFromAssetBundleSynchronous(string assetName, AssetLoaderOption option)
	{
		T result;
		if (this.tryGetAsset(assetName, out result))
		{
			return result;
		}
		T t = this.loadAssetSynchronous(assetName);
		this.cacheAsset(assetName, new AssetLoader<T>.AssetContainer<T>(t, option));
		return t;
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x00083A94 File Offset: 0x00081E94
	public static bool IsDLCAsset(string assetName)
	{
		return AssetLoader<T>.Instance.assetLocationDatabase.dlcAssets.Contains(assetName);
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x00083AAC File Offset: 0x00081EAC
	public static List<string> DEBUG_GetLoadedAssets()
	{
		List<string> list = new List<string>(AssetLoader<T>.Instance.loadedAssets.Count);
		foreach (KeyValuePair<string, AssetLoader<T>.AssetContainer<T>> keyValuePair in AssetLoader<T>.Instance.loadedAssets)
		{
			AssetLoader<T>.AssetContainer<T> value = keyValuePair.Value;
			string text = string.Format("{0} ({1})", keyValuePair.Key, value.assetOption.type.ToString());
			if ((value.assetOption.type & AssetLoaderOption.Type.PersistInCacheTagged) != AssetLoaderOption.Type.None)
			{
				text += string.Format(" [Tag={0}]", value.assetOption.context);
			}
			list.Add(text);
		}
		return list;
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06000B84 RID: 2948 RVA: 0x00083B88 File Offset: 0x00081F88
	// (set) Token: 0x06000B85 RID: 2949 RVA: 0x00083B94 File Offset: 0x00081F94
	public static bool persistentAssetsLoaded
	{
		get
		{
			return AssetLoader<T>.Instance._persistentAssetsLoaded;
		}
		private set
		{
			AssetLoader<T>.Instance._persistentAssetsLoaded = value;
		}
	}

	// Token: 0x0400150F RID: 5391
	protected static AssetLoader<T> Instance;

	// Token: 0x04001510 RID: 5392
	[SerializeField]
	private RuntimeSceneAssetDatabase sceneAssetDatabase;

	// Token: 0x04001511 RID: 5393
	[SerializeField]
	private AssetLocationDatabase assetLocationDatabase;

	// Token: 0x04001512 RID: 5394
	private Dictionary<string, AssetLoader<T>.LoadOperation> loadOperations = new Dictionary<string, AssetLoader<T>.LoadOperation>();

	// Token: 0x04001513 RID: 5395
	private Dictionary<string, AssetLoader<T>.AssetContainer<T>> loadedAssets = new Dictionary<string, AssetLoader<T>.AssetContainer<T>>();

	// Token: 0x04001514 RID: 5396
	private bool _persistentAssetsLoaded;

	// Token: 0x020003A5 RID: 933
	private class AssetContainer<U>
	{
		// Token: 0x06000B86 RID: 2950 RVA: 0x00083BA1 File Offset: 0x00081FA1
		public AssetContainer(U asset, AssetLoaderOption assetOption)
		{
			this.asset = asset;
			this.assetOption = assetOption;
		}

		// Token: 0x04001515 RID: 5397
		public U asset;

		// Token: 0x04001516 RID: 5398
		public AssetLoaderOption assetOption;
	}

	// Token: 0x020003A6 RID: 934
	private class LoadOperation
	{
		// Token: 0x04001517 RID: 5399
		public Coroutine coroutine;

		// Token: 0x04001518 RID: 5400
		public List<Action<T>> completionHandlers = new List<Action<T>>();
	}
}
