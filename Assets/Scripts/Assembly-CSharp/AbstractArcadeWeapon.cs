using UnityEngine;

public class AbstractArcadeWeapon : AbstractPausableComponent
{
	[SerializeField]
	protected AbstractProjectile exPrefab;
	[SerializeField]
	protected Effect exEffectPrefab;
	[SerializeField]
	protected AbstractProjectile basicPrefab;
	[SerializeField]
	protected Effect basicEffectPrefab;
}
