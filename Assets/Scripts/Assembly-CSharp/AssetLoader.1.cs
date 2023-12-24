using UnityEngine;

public class AssetLoader<T> : MonoBehaviour
{
	[SerializeField]
	private RuntimeSceneAssetDatabase sceneAssetDatabase;
	[SerializeField]
	private AssetLocationDatabase assetLocationDatabase;
}
