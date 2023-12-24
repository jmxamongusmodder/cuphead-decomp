using UnityEngine;
using System;

public class RuntimeSceneAssetDatabase : ScriptableObject
{
	[Serializable]
	public class SceneAssetMapping
	{
		public string sceneName;
		public string[] assetNames;
	}

	public string[] INTERNAL_persistentAssetNames;
	public string[] INTERNAL_persistentAssetNamesDLC;
	public SceneAssetMapping[] INTERNAL_sceneAssetMappings;
}
