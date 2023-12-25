using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007B8 RID: 1976
public class SallyStagePlayLevelShurikenBomb : AbstractProjectile
{
	// Token: 0x06002CA5 RID: 11429 RVA: 0x001A52E0 File Offset: 0x001A36E0
	public void InitShuriken(LevelProperties.SallyStagePlay properties, int direction, AbstractPlayerController target)
	{
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.properties = properties;
		this.speed = properties.CurrentState.shuriken.InitialMovementSpeed;
		this.target = target;
		this.isActive = true;
		this.childSpawnCount = 0;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002CA6 RID: 11430 RVA: 0x001A5338 File Offset: 0x001A3738
	public void InitChildShuriken(int direction, int childSpawnCount, AbstractPlayerController target, LevelProperties.SallyStagePlay properties)
	{
		this.properties = properties;
		base.GetComponent<SpriteRenderer>().sprite = this.shuriken;
		if (childSpawnCount > 1)
		{
			this.currentYVelocity = properties.CurrentState.shuriken.ArcTwoVerticalVelocity;
			this.horizontalVelocity = properties.CurrentState.shuriken.ArcTwoHorizontalVelocity * Mathf.Sign((float)direction);
			this.gravity = properties.CurrentState.shuriken.ArcTwoGravity;
		}
		else
		{
			this.currentYVelocity = properties.CurrentState.shuriken.ArcOneVerticalVelocity;
			this.horizontalVelocity = properties.CurrentState.shuriken.ArcOneHorizontalVelocity * Mathf.Sign((float)direction);
			this.gravity = properties.CurrentState.shuriken.ArcOneGravity;
		}
		this.target = target;
		this.isActive = true;
		this.childSpawnCount = childSpawnCount;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002CA7 RID: 11431 RVA: 0x001A5428 File Offset: 0x001A3828
	private IEnumerator move_cr()
	{
		if (this.target == null || this.target.IsDead)
		{
			this.target = PlayerManager.GetNext();
		}
		Vector3 direction = (new Vector3(this.target.center.x, (float)Level.Current.Ground, 0f) - base.transform.position).normalized;
		for (;;)
		{
			if (this.boxCollider != null)
			{
				this.boxCollider.enabled = true;
			}
			if (this.childSpawnCount > 0)
			{
				base.transform.position += (Vector3.right * this.horizontalVelocity + Vector3.up * this.currentYVelocity) * CupheadTime.Delta;
				this.currentYVelocity -= this.gravity;
			}
			else
			{
				base.transform.position += direction * this.speed * CupheadTime.Delta;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CA8 RID: 11432 RVA: 0x001A5444 File Offset: 0x001A3844
	private void Explode()
	{
		base.GetComponent<SpriteRenderer>().sprite = this.explosion;
		if (this.childSpawnCount < this.properties.CurrentState.shuriken.NumberOfChildSpawns)
		{
			this.childSpawnCount++;
			float x = base.GetComponent<SpriteRenderer>().bounds.size.x;
			for (int i = -1; i < 1; i++)
			{
				AbstractProjectile abstractProjectile = this.Create(base.transform.position + Vector3.right * x / 2f * Mathf.Sign((float)i) + Vector3.up * 50f);
				abstractProjectile.GetComponent<SallyStagePlayLevelShurikenBomb>().InitChildShuriken(i, this.childSpawnCount, this.target, this.properties);
			}
		}
	}

	// Token: 0x06002CA9 RID: 11433 RVA: 0x001A552F File Offset: 0x001A392F
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (this.isActive)
		{
			this.isActive = false;
			this.Explode();
			this.StopAllCoroutines();
			UnityEngine.Object.Destroy(base.gameObject, 0.1f);
		}
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x06002CAA RID: 11434 RVA: 0x001A5567 File Offset: 0x001A3967
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x04003526 RID: 13606
	[SerializeField]
	private Sprite shuriken;

	// Token: 0x04003527 RID: 13607
	[SerializeField]
	private Sprite explosion;

	// Token: 0x04003528 RID: 13608
	private float currentYVelocity;

	// Token: 0x04003529 RID: 13609
	private float horizontalVelocity;

	// Token: 0x0400352A RID: 13610
	private float gravity;

	// Token: 0x0400352B RID: 13611
	private AbstractPlayerController target;

	// Token: 0x0400352C RID: 13612
	private float speed;

	// Token: 0x0400352D RID: 13613
	private int childSpawnCount;

	// Token: 0x0400352E RID: 13614
	private bool isActive;

	// Token: 0x0400352F RID: 13615
	private LevelProperties.SallyStagePlay properties;

	// Token: 0x04003530 RID: 13616
	private BoxCollider2D boxCollider;
}
