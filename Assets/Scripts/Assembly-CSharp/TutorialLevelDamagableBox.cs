using UnityEngine;

public class TutorialLevelDamagableBox : AbstractCollidableObject
{
	[SerializeField]
	private float boxHealth;
	[SerializeField]
	private GameObject toDisable;
	[SerializeField]
	private GameObject toEnable;
	[SerializeField]
	private PlatformingLevelGenericExplosion explosionPrefab;
	[SerializeField]
	private Vector3 explosionPosition;
}
