using UnityEngine;

public class RumRunnersLevelSnout : AbstractCollidableObject
{
	[SerializeField]
	private Transform copballLaunchOrigin;
	[SerializeField]
	private RumRunnersLevelCopBall copBallPrefab;
	[SerializeField]
	private Effect dirtEffect;
	[SerializeField]
	private RumRunnersLevelAnteater parent;
	[SerializeField]
	private Transform shadowTransform;
	[SerializeField]
	private DamageReceiver[] damageReceivers;
	[SerializeField]
	private Transform tonguePokeFXTransform;
	[SerializeField]
	private Effect fakeTongueSpittleEffect;
}
