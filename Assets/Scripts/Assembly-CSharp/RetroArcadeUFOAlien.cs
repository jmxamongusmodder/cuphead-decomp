using System;
using UnityEngine;

// Token: 0x02000764 RID: 1892
public class RetroArcadeUFOAlien : RetroArcadeEnemy
{
	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x0600293B RID: 10555 RVA: 0x00180870 File Offset: 0x0017EC70
	public float NormalizedHpRemaining
	{
		get
		{
			return this.hp / this.properties.hp;
		}
	}

	// Token: 0x0600293C RID: 10556 RVA: 0x00180884 File Offset: 0x0017EC84
	public RetroArcadeUFOAlien Create(RetroArcadeUFO parent, LevelProperties.RetroArcade.UFO properties)
	{
		RetroArcadeUFOAlien retroArcadeUFOAlien = this.InstantiatePrefab<RetroArcadeUFOAlien>();
		retroArcadeUFOAlien.properties = properties;
		retroArcadeUFOAlien.parent = parent;
		retroArcadeUFOAlien.hp = properties.hp;
		retroArcadeUFOAlien.transform.parent = parent.transform;
		retroArcadeUFOAlien.transform.position = parent.transform.position;
		this.cyclePositionIndex = UnityEngine.Random.Range(0, retroArcadeUFOAlien.properties.cyclePositionX.Length);
		return retroArcadeUFOAlien;
	}

	// Token: 0x0600293D RID: 10557 RVA: 0x001808F4 File Offset: 0x0017ECF4
	protected override void Start()
	{
		base.transform.position = base.transform.position + new Vector3(this.properties.initialPositionX, (float)this.properties.alienYPosition, 0f) * this.parent.transform.localScale.y;
		base.PointsWorth = this.properties.pointsGained;
		base.PointsWorth = this.properties.pointsBonus;
	}

	// Token: 0x0600293E RID: 10558 RVA: 0x0018097C File Offset: 0x0017ED7C
	public override void Dead()
	{
		base.Dead();
		this.parent.OnAlienDie();
	}

	// Token: 0x0600293F RID: 10559 RVA: 0x00180990 File Offset: 0x0017ED90
	protected override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.OnDamageTaken(info);
		base.transform.position = new Vector3(this.properties.cyclePositionX[this.cyclePositionIndex], base.transform.position.y, 0f);
		this.cyclePositionIndex = (this.cyclePositionIndex + 1) % this.properties.cyclePositionX.Length;
	}

	// Token: 0x04003231 RID: 12849
	private LevelProperties.RetroArcade.UFO properties;

	// Token: 0x04003232 RID: 12850
	private RetroArcadeUFO parent;

	// Token: 0x04003233 RID: 12851
	private int cyclePositionIndex;
}
