using UnityEngine;

public class TestLevelShootableTimer : AbstractCollidableObject
{
	[SerializeField]
	private float maxTime;
	[SerializeField]
	private DamageReceiver child;
}
