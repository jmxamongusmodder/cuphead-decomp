using System;
using UnityEngine;

// Token: 0x02000752 RID: 1874
public class RetroArcadeOrbiterRobot : RetroArcadeEnemy
{
	// Token: 0x060028D8 RID: 10456 RVA: 0x0017C834 File Offset: 0x0017AC34
	public RetroArcadeOrbiterRobot Create(RetroArcadeBigRobot parent, LevelProperties.RetroArcade.Robots properties, float angle)
	{
		RetroArcadeOrbiterRobot retroArcadeOrbiterRobot = this.InstantiatePrefab<RetroArcadeOrbiterRobot>();
		retroArcadeOrbiterRobot.transform.position = parent.transform.position + properties.smallRobotRotationDistance * MathUtils.AngleToDirection(angle);
		retroArcadeOrbiterRobot.properties = properties;
		retroArcadeOrbiterRobot.parent = parent;
		retroArcadeOrbiterRobot.angle = angle;
		retroArcadeOrbiterRobot.hp = properties.smallRobotHp;
		return retroArcadeOrbiterRobot;
	}

	// Token: 0x060028D9 RID: 10457 RVA: 0x0017C8A0 File Offset: 0x0017ACA0
	protected override void Start()
	{
		base.PointsWorth = this.properties.pointsGained;
		base.PointsBonus = this.properties.pointsBonus;
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x0017C8C4 File Offset: 0x0017ACC4
	protected override void FixedUpdate()
	{
		this.angle += CupheadTime.FixedDelta * this.properties.smallRobotRotationSpeed;
		base.transform.position = this.parent.transform.position + MathUtils.AngleToDirection(this.angle) * this.properties.smallRobotRotationDistance;
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x0017C934 File Offset: 0x0017AD34
	public void Shoot()
	{
		this.projectilePrefab.Create(this.projectileRoot.position, -90f, this.properties.smallRobotShootSpeed);
	}

	// Token: 0x040031B5 RID: 12725
	[SerializeField]
	private BasicProjectile projectilePrefab;

	// Token: 0x040031B6 RID: 12726
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x040031B7 RID: 12727
	private LevelProperties.RetroArcade.Robots properties;

	// Token: 0x040031B8 RID: 12728
	private float angle;

	// Token: 0x040031B9 RID: 12729
	private RetroArcadeBigRobot parent;
}
