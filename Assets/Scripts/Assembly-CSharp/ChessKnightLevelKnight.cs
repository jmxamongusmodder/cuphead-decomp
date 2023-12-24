using UnityEngine;

public class ChessKnightLevelKnight : LevelProperties.ChessKnight.Entity
{
	[SerializeField]
	private ParrySwitch pink;
	[SerializeField]
	private CollisionChild swordHitbox;
	[SerializeField]
	private CollisionChild upHitbox;
	[SerializeField]
	private Rangef positionBoundaryInset;
	[SerializeField]
	private Transform smokeSpawnPoint;
	[SerializeField]
	private Effect backDashSmoke;
	[SerializeField]
	private Rangef legSpeedMultiplierRange;
	[SerializeField]
	private float maximumLegVelocity;
	[SerializeField]
	private SpriteDeathPartsDLC[] deathArmor;
	[SerializeField]
	private Transform[] deathArmorSpawns;
	[SerializeField]
	private HitFlash hitFlash;
	public int tauntAttackCounter;
}
