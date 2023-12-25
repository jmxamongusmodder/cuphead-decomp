using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000642 RID: 1602
public class FlyingBlimpLevelStars : AbstractProjectile
{
	// Token: 0x17000388 RID: 904
	// (get) Token: 0x060020E3 RID: 8419 RVA: 0x0012FF6F File Offset: 0x0012E36F
	// (set) Token: 0x060020E4 RID: 8420 RVA: 0x0012FF77 File Offset: 0x0012E377
	public FlyingBlimpLevelStars.State state { get; private set; }

	// Token: 0x060020E5 RID: 8421 RVA: 0x0012FF80 File Offset: 0x0012E380
	public FlyingBlimpLevelStars Create(Vector2 pos, LevelProperties.FlyingBlimp.Stars properties)
	{
		FlyingBlimpLevelStars flyingBlimpLevelStars = base.Create() as FlyingBlimpLevelStars;
		flyingBlimpLevelStars.properties = properties;
		flyingBlimpLevelStars.transform.position = pos;
		return flyingBlimpLevelStars;
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x0012FFB2 File Offset: 0x0012E3B2
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x0012FFD0 File Offset: 0x0012E3D0
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x0012FFF0 File Offset: 0x0012E3F0
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
		float num = (float)UnityEngine.Random.Range(0, 2);
		this.starFx = UnityEngine.Object.Instantiate<Transform>(this.starFXPrefab);
		this.starFx.transform.parent = base.transform;
		Vector3 position = base.transform.position;
		if (num == 0f)
		{
			base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
			this.starFx.SetScale(new float?(-1f), new float?(1f), new float?(1f));
			position.x = base.transform.position.x + 70f;
		}
		else
		{
			position.x = base.transform.position.x - 10f;
			this.starFx.SetScale(new float?(1f), new float?(-1f), new float?(1f));
		}
		this.starFx.transform.position = position;
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x00130128 File Offset: 0x0012E528
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float speed = this.properties.speedX.RandomFloat();
		float angle = 0f;
		while (base.transform.position.x > -840f)
		{
			angle += this.properties.speedY * CupheadTime.FixedDelta;
			if (CupheadTime.Delta != 0f)
			{
				Vector3 moveY = new Vector3(0f, Mathf.Sin(angle) * this.properties.sineSize);
				Vector3 moveX = base.transform.right * -speed * CupheadTime.FixedDelta;
				base.transform.position += moveX + moveY;
			}
			yield return wait;
		}
		this.Die();
		yield return wait;
		yield break;
	}

	// Token: 0x060020EA RID: 8426 RVA: 0x00130143 File Offset: 0x0012E543
	protected override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x0400297C RID: 10620
	[SerializeField]
	private Transform starFXPrefab;

	// Token: 0x0400297D RID: 10621
	private Transform starFx;

	// Token: 0x0400297E RID: 10622
	private Vector3 spawnPoint;

	// Token: 0x0400297F RID: 10623
	private LevelProperties.FlyingBlimp.Stars properties;

	// Token: 0x02000643 RID: 1603
	public enum State
	{
		// Token: 0x04002981 RID: 10625
		Unspawned,
		// Token: 0x04002982 RID: 10626
		Spawned
	}
}
