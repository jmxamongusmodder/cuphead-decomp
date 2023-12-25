using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000748 RID: 1864
public class RetroArcadeQShip : RetroArcadeEnemy
{
	// Token: 0x170003DB RID: 987
	// (get) Token: 0x0600289E RID: 10398 RVA: 0x0017B232 File Offset: 0x00179632
	// (set) Token: 0x0600289F RID: 10399 RVA: 0x0017B23A File Offset: 0x0017963A
	public float TileRotationSpeed { get; private set; }

	// Token: 0x060028A0 RID: 10400 RVA: 0x0017B243 File Offset: 0x00179643
	public void LevelInit(LevelProperties.RetroArcade properties)
	{
		this.properties = properties;
	}

	// Token: 0x060028A1 RID: 10401 RVA: 0x0017B24C File Offset: 0x0017964C
	public void StartQShip()
	{
		base.gameObject.SetActive(true);
		this.p = this.properties.CurrentState.qShip;
		this.hp = this.p.hp;
		this.TileRotationSpeed = this.p.tileRotationSpeed.min;
		base.PointsBonus = this.p.pointsGained;
		base.PointsWorth = this.p.pointsBonus;
		this.tiles = new List<RetroArcadeQShipOrbitingTile>();
		for (int i = 0; i < this.p.numSpinningTiles; i++)
		{
			RetroArcadeQShipOrbitingTile item = this.tilePrefab.Create(this, 360f * (float)i / (float)this.p.numSpinningTiles, this.p);
			this.tiles.Add(item);
		}
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.tentacle_cr());
	}

	// Token: 0x060028A2 RID: 10402 RVA: 0x0017B33C File Offset: 0x0017973C
	protected override void FixedUpdate()
	{
		this.TileRotationSpeed = this.p.tileRotationSpeed.min * Mathf.Pow(this.p.tileRotationSpeed.max / this.p.tileRotationSpeed.min, 1f - this.hp / this.p.hp);
	}

	// Token: 0x060028A3 RID: 10403 RVA: 0x0017B3A0 File Offset: 0x001797A0
	private IEnumerator move_cr()
	{
		base.transform.SetPosition(new float?(0f), new float?(350f + this.p.tileRotationDistance), null);
		base.MoveY(this.p.yPos - (350f + this.p.tileRotationDistance), 500f);
		while (this.movingY)
		{
			yield return new WaitForFixedUpdate();
		}
		float t = 0f;
		float moveTime = this.p.maxXPos * 2f / this.p.moveSpeed;
		for (;;)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Sin(t * 3.1415927f / moveTime) * this.p.maxXPos), null, null);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060028A4 RID: 10404 RVA: 0x0017B3BC File Offset: 0x001797BC
	public override void Dead()
	{
		this.StopAllCoroutines();
		foreach (Collider2D collider2D in base.GetComponentsInChildren<Collider2D>())
		{
			collider2D.enabled = false;
		}
		base.IsDead = true;
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.color = new Color(0f, 0f, 0f, 0.25f);
		}
		this.properties.DealDamageToNextNamedState();
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x060028A5 RID: 10405 RVA: 0x0017B45C File Offset: 0x0017985C
	private IEnumerator moveOffscreen_cr()
	{
		base.MoveY(350f + this.p.tileRotationDistance - base.transform.position.y, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060028A6 RID: 10406 RVA: 0x0017B478 File Offset: 0x00179878
	public void ShootProjectile()
	{
		this.projectilePrefab.Create(this.projectileRoot.position, -90f - this.p.shotSpreadAngle, this.p.shotSpeed);
		this.projectilePrefab.Create(this.projectileRoot.position, -90f, this.p.shotSpeed);
		this.projectilePrefab.Create(this.projectileRoot.position, -90f + this.p.shotSpreadAngle, this.p.shotSpeed);
	}

	// Token: 0x060028A7 RID: 10407 RVA: 0x0017B524 File Offset: 0x00179924
	private IEnumerator tentacle_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.p.tentacleSpawnRange.RandomFloat());
			bool left = Rand.Bool();
			base.animator.SetBool((!left) ? "RightTentacle" : "LeftTentacle", true);
			yield return CupheadTime.WaitForSeconds(this, this.p.tentacleWarningDuration);
			base.animator.SetBool((!left) ? "RightTentacle" : "LeftTentacle", false);
			this.tentaclePrefab.Create((!left) ? RetroArcadeQShipTentacle.Direction.Left : RetroArcadeQShipTentacle.Direction.Right, this.p);
		}
		yield break;
	}

	// Token: 0x04003176 RID: 12662
	private const float OFFSCREEN_Y = 350f;

	// Token: 0x04003177 RID: 12663
	private const float MOVE_Y_SPEED = 500f;

	// Token: 0x04003178 RID: 12664
	private LevelProperties.RetroArcade properties;

	// Token: 0x04003179 RID: 12665
	private LevelProperties.RetroArcade.QShip p;

	// Token: 0x0400317A RID: 12666
	[SerializeField]
	private RetroArcadeQShipOrbitingTile tilePrefab;

	// Token: 0x0400317B RID: 12667
	[SerializeField]
	private BasicProjectile projectilePrefab;

	// Token: 0x0400317C RID: 12668
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x0400317D RID: 12669
	[SerializeField]
	private RetroArcadeQShipTentacle tentaclePrefab;

	// Token: 0x0400317E RID: 12670
	private List<RetroArcadeQShipOrbitingTile> tiles;
}
