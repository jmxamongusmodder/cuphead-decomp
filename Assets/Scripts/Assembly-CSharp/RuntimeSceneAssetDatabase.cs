using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003B0 RID: 944
public class RuntimeSceneAssetDatabase : ScriptableObject
{
	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0008465B File Offset: 0x00082A5B
	public HashSet<string> persistentAssets
	{
		get
		{
			if (this._persistentAssets == null)
			{
				this._persistentAssets = new HashSet<string>(this.INTERNAL_persistentAssetNames);
			}
			return this._persistentAssets;
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x0008467F File Offset: 0x00082A7F
	public HashSet<string> persistentAssetsDLC
	{
		get
		{
			if (this._persistentAssetsDLC == null)
			{
				this._persistentAssetsDLC = new HashSet<string>(this.INTERNAL_persistentAssetNamesDLC);
			}
			return this._persistentAssetsDLC;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x000846A4 File Offset: 0x00082AA4
	public Dictionary<string, string[]> sceneAssetMappings
	{
		get
		{
			if (this._sceneAssetMappings == null)
			{
				this._sceneAssetMappings = new Dictionary<string, string[]>();
				foreach (RuntimeSceneAssetDatabase.SceneAssetMapping sceneAssetMapping in this.INTERNAL_sceneAssetMappings)
				{
					this._sceneAssetMappings.Add(sceneAssetMapping.sceneName, sceneAssetMapping.assetNames);
				}
			}
			return this._sceneAssetMappings;
		}
	}

	// Token: 0x04001562 RID: 5474
	public string[] INTERNAL_persistentAssetNames;

	// Token: 0x04001563 RID: 5475
	public string[] INTERNAL_persistentAssetNamesDLC;

	// Token: 0x04001564 RID: 5476
	public RuntimeSceneAssetDatabase.SceneAssetMapping[] INTERNAL_sceneAssetMappings;

	// Token: 0x04001565 RID: 5477
	private HashSet<string> _persistentAssets;

	// Token: 0x04001566 RID: 5478
	private HashSet<string> _persistentAssetsDLC;

	// Token: 0x04001567 RID: 5479
	private Dictionary<string, string[]> _sceneAssetMappings;

	// Token: 0x020003B1 RID: 945
	[Serializable]
	public class SceneAssetMapping
	{
		// Token: 0x04001568 RID: 5480
		public string sceneName;

		// Token: 0x04001569 RID: 5481
		public string[] assetNames;
	}
}
