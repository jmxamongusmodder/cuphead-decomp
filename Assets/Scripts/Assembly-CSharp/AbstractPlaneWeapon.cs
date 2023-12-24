using UnityEngine;

public class AbstractPlaneWeapon : AbstractPausableComponent
{
	[SerializeField]
	protected AbstractProjectile exPrefab;
	[SerializeField]
	protected Effect exEffectPrefab;
	[SerializeField]
	protected AbstractProjectile basicPrefab;
	[SerializeField]
	protected Effect basicEffectPrefab;
	[SerializeField]
	protected AbstractProjectile shrunkPrefab;
	[SerializeField]
	protected float shrunkDamageMultiplier;
}
