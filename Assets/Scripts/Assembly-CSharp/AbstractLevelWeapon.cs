using UnityEngine;

public class AbstractLevelWeapon : AbstractPausableComponent
{
	[SerializeField]
	protected AbstractProjectile exPrefab;
	[SerializeField]
	protected Effect exEffectPrefab;
	[SerializeField]
	protected LevelPlayerWeaponFiringHitbox exFiringHitboxPrefab;
	[SerializeField]
	protected AbstractProjectile basicPrefab;
	[SerializeField]
	protected Effect basicEffectPrefab;
	[SerializeField]
	protected LevelPlayerWeaponFiringHitbox basicFiringHitboxPrefab;
}
