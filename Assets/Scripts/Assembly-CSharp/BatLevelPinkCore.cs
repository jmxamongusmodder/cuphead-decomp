using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200050B RID: 1291
public class BatLevelPinkCore : ParrySwitch
{
	// Token: 0x060016E5 RID: 5861 RVA: 0x000CDEB1 File Offset: 0x000CC2B1
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.lastSideHit = BatLevelPinkCore.SideHit.None;
	}

	// Token: 0x060016E6 RID: 5862 RVA: 0x000CDECB File Offset: 0x000CC2CB
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060016E7 RID: 5863 RVA: 0x000CDEE4 File Offset: 0x000CC2E4
	public void Init(LevelProperties.Bat.BatBouncer properties, Vector2 pos, float angle)
	{
		this.properties = properties;
		base.transform.position = pos;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(angle));
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060016E8 RID: 5864 RVA: 0x000CDF3C File Offset: 0x000CC33C
	protected IEnumerator move_cr()
	{
		this.velocity = -base.transform.right;
		for (;;)
		{
			base.transform.position += this.velocity * this.properties.mainBounceSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060016E9 RID: 5865 RVA: 0x000CDF57 File Offset: 0x000CC357
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060016EA RID: 5866 RVA: 0x000CDF70 File Offset: 0x000CC370
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != BatLevelPinkCore.SideHit.Top)
		{
			Vector3 position = base.transform.position;
			Vector3 a = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
			this.collisionPoint = a - position;
			this.lastSideHit = BatLevelPinkCore.SideHit.Top;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x060016EB RID: 5867 RVA: 0x000CDFF4 File Offset: 0x000CC3F4
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			if (base.transform.position.x > 0f)
			{
				if (this.lastSideHit != BatLevelPinkCore.SideHit.Right)
				{
					Vector3 a = new Vector3((float)Level.Current.Left, base.transform.position.y, 0f);
					Vector3 position = base.transform.position;
					this.collisionPoint = a - position;
					this.lastSideHit = BatLevelPinkCore.SideHit.Right;
					base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
				}
			}
			else if (this.lastSideHit != BatLevelPinkCore.SideHit.Left)
			{
				Vector3 position2 = base.transform.position;
				Vector3 a2 = new Vector3((float)Level.Current.Right, base.transform.position.y, 0f);
				this.collisionPoint = a2 - position2;
				this.lastSideHit = BatLevelPinkCore.SideHit.Left;
				base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
			}
		}
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x000CE108 File Offset: 0x000CC508
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != BatLevelPinkCore.SideHit.Bottom)
		{
			Vector3 position = base.transform.position;
			Vector3 a = new Vector3(base.transform.position.x, (float)Level.Current.Ceiling, 0f);
			this.collisionPoint = a - position;
			this.lastSideHit = BatLevelPinkCore.SideHit.Bottom;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x060016ED RID: 5869 RVA: 0x000CE18C File Offset: 0x000CC58C
	protected IEnumerator change_direction_cr(Vector3 collisionPoint)
	{
		this.velocity = 1f * (-2f * Vector3.Dot(this.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + this.velocity);
		yield return null;
		yield break;
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x000CE1AE File Offset: 0x000CC5AE
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400202D RID: 8237
	private LevelProperties.Bat.BatBouncer properties;

	// Token: 0x0400202E RID: 8238
	private DamageDealer damageDealer;

	// Token: 0x0400202F RID: 8239
	private Vector3 velocity;

	// Token: 0x04002030 RID: 8240
	private Vector3 collisionPoint;

	// Token: 0x04002031 RID: 8241
	public BatLevelPinkCore.SideHit lastSideHit;

	// Token: 0x0200050C RID: 1292
	public enum SideHit
	{
		// Token: 0x04002033 RID: 8243
		Top,
		// Token: 0x04002034 RID: 8244
		Bottom,
		// Token: 0x04002035 RID: 8245
		Left,
		// Token: 0x04002036 RID: 8246
		Right,
		// Token: 0x04002037 RID: 8247
		None
	}
}
