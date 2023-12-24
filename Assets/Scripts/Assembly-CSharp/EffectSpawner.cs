using UnityEngine;

public class EffectSpawner : AbstractMonoBehaviour
{
	[SerializeField]
	private Effect effectPrefab;
	public Vector2 offset;
	public float delay;
}
