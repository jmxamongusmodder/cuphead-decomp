using UnityEngine;

public class RetroArcadeQShip : RetroArcadeEnemy
{
	[SerializeField]
	private RetroArcadeQShipOrbitingTile tilePrefab;
	[SerializeField]
	private BasicProjectile projectilePrefab;
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private RetroArcadeQShipTentacle tentaclePrefab;
}
