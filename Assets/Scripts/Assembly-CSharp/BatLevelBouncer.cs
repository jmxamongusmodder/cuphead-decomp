using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000503 RID: 1283
public class BatLevelBouncer : AbstractCollidableObject
{
	// Token: 0x060016B0 RID: 5808 RVA: 0x000CC47D File Offset: 0x000CA87D
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.lastSideHit = BatLevelBouncer.SideHit.None;
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x000CC497 File Offset: 0x000CA897
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x000CC4B0 File Offset: 0x000CA8B0
	public void Init(LevelProperties.Bat.BatBouncer properties, Vector2 pos, float angle)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.angle = angle;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(angle));
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060016B3 RID: 5811 RVA: 0x000CC510 File Offset: 0x000CA910
	protected IEnumerator move_cr()
	{
		this.velocity = -base.transform.right;
		for (;;)
		{
			base.transform.position += this.velocity * this.properties.mainBounceSpeed * CupheadTime.FixedDelta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060016B4 RID: 5812 RVA: 0x000CC52B File Offset: 0x000CA92B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x000CC544 File Offset: 0x000CA944
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != BatLevelBouncer.SideHit.Top)
		{
			Vector3 position = base.transform.position;
			Vector3 a = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
			this.collisionPoint = a - position;
			this.lastSideHit = BatLevelBouncer.SideHit.Top;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x000CC5C8 File Offset: 0x000CA9C8
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			if (base.transform.position.x > 0f)
			{
				if (this.lastSideHit != BatLevelBouncer.SideHit.Right)
				{
					Vector3 a = new Vector3((float)Level.Current.Left, base.transform.position.y, 0f);
					Vector3 position = base.transform.position;
					this.collisionPoint = a - position;
					this.lastSideHit = BatLevelBouncer.SideHit.Right;
					base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
				}
			}
			else if (this.lastSideHit != BatLevelBouncer.SideHit.Left)
			{
				Vector3 position2 = base.transform.position;
				Vector3 a2 = new Vector3((float)Level.Current.Right, base.transform.position.y, 0f);
				this.collisionPoint = a2 - position2;
				this.lastSideHit = BatLevelBouncer.SideHit.Left;
				base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
			}
		}
	}

	// Token: 0x060016B7 RID: 5815 RVA: 0x000CC6DC File Offset: 0x000CAADC
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != BatLevelBouncer.SideHit.Bottom)
		{
			Vector3 position = base.transform.position;
			Vector3 a = new Vector3(base.transform.position.x, (float)Level.Current.Ceiling, 0f);
			this.collisionPoint = a - position;
			this.lastSideHit = BatLevelBouncer.SideHit.Bottom;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x060016B8 RID: 5816 RVA: 0x000CC760 File Offset: 0x000CAB60
	protected IEnumerator change_direction_cr(Vector3 collisionPoint)
	{
		this.velocity = 1f * (-2f * Vector3.Dot(this.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + this.velocity);
		this.counter++;
		if ((float)this.counter >= this.properties.breakCounter && !this.isPink)
		{
			this.SpawnPink();
			this.Die();
		}
		yield return null;
		yield break;
	}

	// Token: 0x060016B9 RID: 5817 RVA: 0x000CC784 File Offset: 0x000CAB84
	protected void SpawnPink()
	{
		BatLevelPinkCore batLevelPinkCore = UnityEngine.Object.Instantiate<BatLevelPinkCore>(this.pinkPrefab);
		batLevelPinkCore.Init(this.properties, base.transform.position, this.angle);
	}

	// Token: 0x060016BA RID: 5818 RVA: 0x000CC7BF File Offset: 0x000CABBF
	protected void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001FFE RID: 8190
	[SerializeField]
	private BatLevelPinkCore pinkPrefab;

	// Token: 0x04001FFF RID: 8191
	private LevelProperties.Bat.BatBouncer properties;

	// Token: 0x04002000 RID: 8192
	private DamageDealer damageDealer;

	// Token: 0x04002001 RID: 8193
	private Vector3 velocity;

	// Token: 0x04002002 RID: 8194
	private Vector3 collisionPoint;

	// Token: 0x04002003 RID: 8195
	public BatLevelBouncer.SideHit lastSideHit;

	// Token: 0x04002004 RID: 8196
	private bool isPink;

	// Token: 0x04002005 RID: 8197
	private float angle;

	// Token: 0x04002006 RID: 8198
	private int counter;

	// Token: 0x02000504 RID: 1284
	public enum SideHit
	{
		// Token: 0x04002008 RID: 8200
		Top,
		// Token: 0x04002009 RID: 8201
		Bottom,
		// Token: 0x0400200A RID: 8202
		Left,
		// Token: 0x0400200B RID: 8203
		Right,
		// Token: 0x0400200C RID: 8204
		None
	}
}
