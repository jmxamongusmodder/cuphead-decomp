using UnityEngine;

public class LevelBossDeathExploder : AbstractMonoBehaviour
{
	public Effect ExplosionPrefabOverride;
	[SerializeField]
	private float STEADY_DELAY;
	[SerializeField]
	private float MIN_DELAY;
	[SerializeField]
	private float MAX_DELAY;
	public Vector2 offset;
	[SerializeField]
	private float radius;
	[SerializeField]
	private Vector2 scaleFactor;
	[SerializeField]
	private bool disableSound;
}
