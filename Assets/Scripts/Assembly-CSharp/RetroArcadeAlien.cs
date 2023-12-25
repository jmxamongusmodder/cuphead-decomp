using System;
using UnityEngine;

// Token: 0x02000735 RID: 1845
public class RetroArcadeAlien : RetroArcadeEnemy
{
	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x0600282D RID: 10285 RVA: 0x00176F96 File Offset: 0x00175396
	// (set) Token: 0x0600282E RID: 10286 RVA: 0x00176F9E File Offset: 0x0017539E
	public int ColumnIndex { get; private set; }

	// Token: 0x0600282F RID: 10287 RVA: 0x00176FA8 File Offset: 0x001753A8
	public RetroArcadeAlien Create(Vector2 position, int columnIndex, RetroArcadeAlienManager manager, LevelProperties.RetroArcade.Aliens properties)
	{
		RetroArcadeAlien retroArcadeAlien = this.InstantiatePrefab<RetroArcadeAlien>();
		retroArcadeAlien.transform.position = position;
		retroArcadeAlien.properties = properties;
		retroArcadeAlien.manager = manager;
		retroArcadeAlien.hp = properties.hp;
		retroArcadeAlien.ColumnIndex = columnIndex;
		return retroArcadeAlien;
	}

	// Token: 0x06002830 RID: 10288 RVA: 0x00176FF1 File Offset: 0x001753F1
	protected override void Start()
	{
		base.PointsWorth = this.properties.pointsGained;
		base.PointsBonus = this.properties.pointsBonus;
	}

	// Token: 0x06002831 RID: 10289 RVA: 0x00177018 File Offset: 0x00175418
	protected override void FixedUpdate()
	{
		if (this.movingY)
		{
			return;
		}
		base.transform.AddPosition((float)((this.manager.direction != RetroArcadeAlien.Direction.Right) ? -1 : 1) * this.manager.moveSpeed * CupheadTime.FixedDelta, 0f, 0f);
	}

	// Token: 0x06002832 RID: 10290 RVA: 0x00177071 File Offset: 0x00175471
	public void MoveY(float moveAmount)
	{
		base.MoveY(moveAmount, 500f);
	}

	// Token: 0x06002833 RID: 10291 RVA: 0x0017707F File Offset: 0x0017547F
	public override void Dead()
	{
		base.Dead();
		this.manager.OnAlienDie(this);
	}

	// Token: 0x06002834 RID: 10292 RVA: 0x00177093 File Offset: 0x00175493
	public void Shoot()
	{
		this.bulletPrefab.Create(this.bulletRoot.position, -90f, this.properties.bulletSpeed);
	}

	// Token: 0x040030F5 RID: 12533
	private const float MOVE_Y_SPEED = 500f;

	// Token: 0x040030F6 RID: 12534
	[SerializeField]
	private BasicProjectile bulletPrefab;

	// Token: 0x040030F7 RID: 12535
	[SerializeField]
	private Transform bulletRoot;

	// Token: 0x040030F8 RID: 12536
	private LevelProperties.RetroArcade.Aliens properties;

	// Token: 0x040030F9 RID: 12537
	private RetroArcadeAlienManager manager;

	// Token: 0x02000736 RID: 1846
	public enum Direction
	{
		// Token: 0x040030FC RID: 12540
		Left,
		// Token: 0x040030FD RID: 12541
		Right
	}
}
