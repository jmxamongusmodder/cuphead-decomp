using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000679 RID: 1657
public class FlyingGenieLevelSpawner : AbstractProjectile
{
	// Token: 0x1700039C RID: 924
	// (get) Token: 0x060022E8 RID: 8936 RVA: 0x00147A08 File Offset: 0x00145E08
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x00147A10 File Offset: 0x00145E10
	public FlyingGenieLevelSpawner Create(Vector2 pos, AbstractPlayerController player, LevelProperties.FlyingGenie.Bullets properties)
	{
		FlyingGenieLevelSpawner flyingGenieLevelSpawner = base.Create(pos) as FlyingGenieLevelSpawner;
		flyingGenieLevelSpawner.properties = properties;
		flyingGenieLevelSpawner.player = player;
		return flyingGenieLevelSpawner;
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x00147A39 File Offset: 0x00145E39
	protected override void Start()
	{
		base.Start();
		this.SetUpSpawnPoints();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x00147A54 File Offset: 0x00145E54
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		for (int i = 0; i < this.points.Length; i++)
		{
			this.points[i].transform.Rotate(0f, 0f, this.properties.spawnerRotateSpeed * CupheadTime.Delta);
		}
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x00147AC8 File Offset: 0x00145EC8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060022ED RID: 8941 RVA: 0x00147AE0 File Offset: 0x00145EE0
	private void SetUpSpawnPoints()
	{
		float num = UnityEngine.Random.Range(0f, 6.2831855f);
		float rotation = 0f;
		float x = base.transform.position.x;
		float y = base.transform.position.y;
		this.points = new FlyingGenieLevelSpawnerPoint[this.properties.spawnerCount];
		for (int i = 0; i < this.properties.spawnerCount; i++)
		{
			if (i == 0)
			{
				rotation = num * 57.29578f + 90f;
			}
			else if (i == 1)
			{
				rotation = num * 57.29578f - 90f;
			}
			else if (i == 2)
			{
				rotation = num * 57.29578f + 360f;
			}
			else if (i == 3)
			{
				rotation = num * 57.29578f - 180f;
			}
			this.points[i] = this.pointPrefab.Create(new Vector3(x, y), rotation, this.properties);
			this.points[i].transform.parent = base.transform;
		}
	}

	// Token: 0x060022EE RID: 8942 RVA: 0x00147C0C File Offset: 0x0014600C
	private IEnumerator move_cr()
	{
		float offset = 200f;
		float size = base.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
		int count = 0;
		int maxCount = this.properties.spawnerMoveCountRange.RandomInt();
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 startDir = Vector3.zero - base.transform.position;
		while (base.transform.position.y > 0f)
		{
			base.transform.position += startDir.normalized * this.properties.spawnerSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		for (;;)
		{
			Vector3 start = base.transform.position;
			Vector3 dir = this.player.transform.position - base.transform.position;
			Vector3 endDist = start + dir.normalized * this.properties.spawnerDistance;
			if (this.isDead)
			{
				for (;;)
				{
					base.transform.position += dir.normalized * this.properties.spawnerSpeed * CupheadTime.FixedDelta;
					if (base.transform.position.x < -640f - offset || base.transform.position.x > 640f + offset || base.transform.position.y > 360f + offset || base.transform.position.y < -360f - offset)
					{
						break;
					}
					yield return wait;
				}
				this.Kill();
				this.StopAllCoroutines();
			}
			while (base.transform.position != endDist)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, endDist, this.properties.spawnerSpeed * CupheadTime.FixedDelta);
				if (base.transform.position.x < -640f + size || base.transform.position.x > 640f - size || base.transform.position.y > 360f - size || base.transform.position.y < -360f + size)
				{
					break;
				}
				yield return wait;
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.spawnerMoveDelay);
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
			count++;
			if (count >= maxCount)
			{
				while (this.attackCount < this.properties.spawnerShotCount)
				{
					base.animator.SetTrigger("Attack");
					yield return CupheadTime.WaitForSeconds(this, this.properties.spawnerShotDelay);
				}
				yield return CupheadTime.WaitForSeconds(this, this.properties.spawnerHesitate);
				count = 0;
				this.attackCount = 0;
				maxCount = this.properties.spawnerMoveCountRange.RandomInt();
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x00147C28 File Offset: 0x00146028
	private void Kill()
	{
		foreach (FlyingGenieLevelSpawnerPoint flyingGenieLevelSpawnerPoint in this.points)
		{
			flyingGenieLevelSpawnerPoint.Dead();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x00147C68 File Offset: 0x00146068
	public void Attack()
	{
		foreach (FlyingGenieLevelSpawnerPoint flyingGenieLevelSpawnerPoint in this.points)
		{
			flyingGenieLevelSpawnerPoint.Shoot();
		}
		this.attackCount++;
		if (this.attackCount >= this.properties.spawnerShotCount)
		{
			base.animator.SetTrigger("End");
		}
	}

	// Token: 0x04002B84 RID: 11140
	private const string AttackParameterName = "Attack";

	// Token: 0x04002B85 RID: 11141
	private const string EndAttackParameterName = "End";

	// Token: 0x04002B86 RID: 11142
	[SerializeField]
	private FlyingGenieLevelSpawnerPoint pointPrefab;

	// Token: 0x04002B87 RID: 11143
	private FlyingGenieLevelSpawnerPoint[] points;

	// Token: 0x04002B88 RID: 11144
	private LevelProperties.FlyingGenie.Bullets properties;

	// Token: 0x04002B89 RID: 11145
	private AbstractPlayerController player;

	// Token: 0x04002B8A RID: 11146
	private float speed;

	// Token: 0x04002B8B RID: 11147
	private int attackCount;

	// Token: 0x04002B8C RID: 11148
	public bool isDead;
}
