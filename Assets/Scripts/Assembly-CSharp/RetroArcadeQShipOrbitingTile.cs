using System;

// Token: 0x02000749 RID: 1865
public class RetroArcadeQShipOrbitingTile : AbstractCollidableObject
{
	// Token: 0x060028A9 RID: 10409 RVA: 0x0017B974 File Offset: 0x00179D74
	public RetroArcadeQShipOrbitingTile Create(RetroArcadeQShip parent, float angle, LevelProperties.RetroArcade.QShip properties)
	{
		RetroArcadeQShipOrbitingTile retroArcadeQShipOrbitingTile = this.InstantiatePrefab<RetroArcadeQShipOrbitingTile>();
		retroArcadeQShipOrbitingTile.transform.position = parent.transform.position + properties.tileRotationDistance * MathUtils.AngleToDirection(angle);
		retroArcadeQShipOrbitingTile.properties = properties;
		retroArcadeQShipOrbitingTile.transform.parent = parent.transform;
		retroArcadeQShipOrbitingTile.parent = parent;
		retroArcadeQShipOrbitingTile.angle = angle;
		DamageReceiver component = retroArcadeQShipOrbitingTile.GetComponent<DamageReceiver>();
		component.OnDamageTaken += retroArcadeQShipOrbitingTile.OnDamageTaken;
		return retroArcadeQShipOrbitingTile;
	}

	// Token: 0x060028AA RID: 10410 RVA: 0x0017BA00 File Offset: 0x00179E00
	private void FixedUpdate()
	{
		this.angle += CupheadTime.FixedDelta * this.parent.TileRotationSpeed;
		base.transform.position = this.parent.transform.position + MathUtils.AngleToDirection(this.angle) * this.properties.tileRotationDistance;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.angle));
	}

	// Token: 0x060028AB RID: 10411 RVA: 0x0017BA9A File Offset: 0x00179E9A
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.parent.ShootProjectile();
	}

	// Token: 0x04003180 RID: 12672
	private float angle;

	// Token: 0x04003181 RID: 12673
	private RetroArcadeQShip parent;

	// Token: 0x04003182 RID: 12674
	private LevelProperties.RetroArcade.QShip properties;
}
