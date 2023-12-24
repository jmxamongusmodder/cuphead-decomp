using UnityEngine;

public class DicePalaceCigarLevelCigar : LevelProperties.DicePalaceCigar.Entity
{
	[SerializeField]
	private GameObject leftAshTray;
	[SerializeField]
	private GameObject rightAshTray;
	[SerializeField]
	private GameObject leftAsh;
	[SerializeField]
	private GameObject rightAsh;
	[SerializeField]
	private Transform leftSpawnPointFacingRight;
	[SerializeField]
	private Transform leftSpawnPointFacingLeft;
	[SerializeField]
	private Transform rightSpawnPointFacingLeft;
	[SerializeField]
	private Transform rightSpawnPointFacingRight;
	[SerializeField]
	private Transform smokeSpawnPoint;
	[SerializeField]
	private Effect smokeA;
	[SerializeField]
	private Effect smokeB;
	[SerializeField]
	private CollisionChild collisionChild;
	[SerializeField]
	private DicePalaceCigarLevelCigarSpit spitPrefab;
	[SerializeField]
	private Transform spitSpawnPoint;
	[SerializeField]
	private DicePalaceCigarLevelCigaretteGhost ghostPrefab;
	[SerializeField]
	private Transform ghostSpawnPoint;
	[SerializeField]
	private float ghostOffset;
}
