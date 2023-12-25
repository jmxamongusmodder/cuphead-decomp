using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004D1 RID: 1233
public class AirshipCrabLevelGems : ParrySwitch
{
	// Token: 0x06001502 RID: 5378 RVA: 0x000BCB38 File Offset: 0x000BAF38
	public void Init(LevelProperties.AirshipCrab.Gems properties, Vector2 pos, float angle)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.pink = base.GetComponent<SpriteRenderer>().color;
		this.startPos = base.transform.position;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(-angle));
		this.velocity = -base.transform.right;
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x000BCBBB File Offset: 0x000BAFBB
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.lastSideHit = AirshipCrabLevelGems.SideHit.None;
		this.parried = false;
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x000BCBDC File Offset: 0x000BAFDC
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001505 RID: 5381 RVA: 0x000BCBF4 File Offset: 0x000BAFF4
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, CollisionPhase.Enter);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06001506 RID: 5382 RVA: 0x000BCC0C File Offset: 0x000BB00C
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != AirshipCrabLevelGems.SideHit.Top)
		{
			Vector3 position = base.transform.position;
			Vector3 a = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
			this.collisionPoint = a - position;
			this.lastSideHit = AirshipCrabLevelGems.SideHit.Top;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x06001507 RID: 5383 RVA: 0x000BCC90 File Offset: 0x000BB090
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			if (base.transform.position.x > 0f)
			{
				if (this.lastSideHit != AirshipCrabLevelGems.SideHit.Right)
				{
					Vector3 a = new Vector3((float)Level.Current.Left, base.transform.position.y, 0f);
					Vector3 position = base.transform.position;
					this.collisionPoint = a - position;
					this.lastSideHit = AirshipCrabLevelGems.SideHit.Right;
					base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
				}
			}
			else if (this.lastSideHit != AirshipCrabLevelGems.SideHit.Left)
			{
				Vector3 position2 = base.transform.position;
				Vector3 a2 = new Vector3((float)Level.Current.Right, base.transform.position.y, 0f);
				this.collisionPoint = a2 - position2;
				this.lastSideHit = AirshipCrabLevelGems.SideHit.Left;
				base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
			}
		}
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x000BCDA4 File Offset: 0x000BB1A4
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != AirshipCrabLevelGems.SideHit.Bottom)
		{
			Vector3 position = base.transform.position;
			Vector3 a = new Vector3(base.transform.position.x, (float)Level.Current.Ceiling, 0f);
			this.collisionPoint = a - position;
			this.lastSideHit = AirshipCrabLevelGems.SideHit.Bottom;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x000BCE28 File Offset: 0x000BB228
	public void PickMovement()
	{
		if (!this.parried)
		{
			if (this.currentMovement != null)
			{
				base.StopCoroutine(this.currentMovement);
			}
			this.currentMovement = base.StartCoroutine(this.move_cr());
		}
		else
		{
			if (this.currentMovement != null)
			{
				base.StopCoroutine(this.currentMovement);
			}
			this.currentMovement = base.StartCoroutine(this.go_back_cr());
		}
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x000BCE98 File Offset: 0x000BB298
	private IEnumerator move_cr()
	{
		this.moving = true;
		this.parried = false;
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().color = this.pink;
		while (this.moving)
		{
			base.transform.position += this.velocity * this.properties.bulletSpeed * CupheadTime.FixedDelta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x000BCEB4 File Offset: 0x000BB2B4
	private IEnumerator change_direction_cr(Vector3 collisionPoint)
	{
		this.velocity = 1f * (-2f * Vector3.Dot(this.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + this.velocity);
		yield return null;
		yield break;
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x000BCED8 File Offset: 0x000BB2D8
	private IEnumerator go_back_cr()
	{
		this.velocity = -base.transform.right;
		while (base.transform.position != this.startPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.startPos, this.properties.bulletSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600150D RID: 5389 RVA: 0x000BCEF3 File Offset: 0x000BB2F3
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.ParryOneQuarter();
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x000BCF08 File Offset: 0x000BB308
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		this.startTimer = true;
		this.parried = true;
		this.moving = false;
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		this.PickMovement();
	}

	// Token: 0x04001E61 RID: 7777
	public bool parried;

	// Token: 0x04001E62 RID: 7778
	public bool startTimer;

	// Token: 0x04001E63 RID: 7779
	public bool moving;

	// Token: 0x04001E64 RID: 7780
	public AirshipCrabLevelGems.SideHit lastSideHit;

	// Token: 0x04001E65 RID: 7781
	private LevelProperties.AirshipCrab.Gems properties;

	// Token: 0x04001E66 RID: 7782
	private DamageDealer damageDealer;

	// Token: 0x04001E67 RID: 7783
	private Color pink;

	// Token: 0x04001E68 RID: 7784
	private Vector3 velocity;

	// Token: 0x04001E69 RID: 7785
	private Vector3 startPos;

	// Token: 0x04001E6A RID: 7786
	private Vector3 collisionPoint;

	// Token: 0x04001E6B RID: 7787
	private bool getCollisionPoint;

	// Token: 0x04001E6C RID: 7788
	private Coroutine currentMovement;

	// Token: 0x020004D2 RID: 1234
	public enum SideHit
	{
		// Token: 0x04001E6E RID: 7790
		Top,
		// Token: 0x04001E6F RID: 7791
		Bottom,
		// Token: 0x04001E70 RID: 7792
		Left,
		// Token: 0x04001E71 RID: 7793
		Right,
		// Token: 0x04001E72 RID: 7794
		None
	}
}
