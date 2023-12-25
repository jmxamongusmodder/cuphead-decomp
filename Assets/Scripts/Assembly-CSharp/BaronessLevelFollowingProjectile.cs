using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004E3 RID: 1251
public class BaronessLevelFollowingProjectile : AbstractProjectile
{
	// Token: 0x060015A0 RID: 5536 RVA: 0x000C1D77 File Offset: 0x000C0177
	protected override void Awake()
	{
		base.Awake();
		this.isActive = true;
	}

	// Token: 0x060015A1 RID: 5537 RVA: 0x000C1D88 File Offset: 0x000C0188
	public void Init(Vector2 pos, Vector3 target, LevelProperties.Baroness.BaronessVonBonbon properties, AbstractPlayerController player, BaronessLevelCastle parent)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.target = target;
		this.player = player;
		this.parent = parent;
		this.parent.OnDeathEvent += this.KillProjectile;
	}

	// Token: 0x060015A2 RID: 5538 RVA: 0x000C1DDB File Offset: 0x000C01DB
	private void KillProjectile()
	{
		this.isActive = false;
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x000C1DE4 File Offset: 0x000C01E4
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x000C1DF9 File Offset: 0x000C01F9
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (!this.isActive)
		{
			this.Die();
		}
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x000C1E28 File Offset: 0x000C0228
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x000C1E48 File Offset: 0x000C0248
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float count = 0f;
		for (;;)
		{
			Vector2 start = base.transform.position;
			this.target = this.player.transform.position;
			float followTime = this.properties.finalProjectileMoveDuration;
			float t = 0f;
			while (t < followTime)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, this.target, this.properties.finalProjectileSpeed * CupheadTime.FixedDelta);
				t += CupheadTime.FixedDelta;
				yield return wait;
			}
			this.player = PlayerManager.GetNext();
			count += 1f;
			if (count > this.properties.finalProjectileRedirectCount)
			{
				break;
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.finalProjectileRedirectDelay);
		}
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 dir = this.player.transform.position - base.transform.position;
		for (;;)
		{
			base.transform.position += dir.normalized * this.properties.finalProjectileSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060015A7 RID: 5543 RVA: 0x000C1E63 File Offset: 0x000C0263
	protected override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
		base.Die();
	}

	// Token: 0x04001EFE RID: 7934
	public LevelProperties.Baroness.BaronessVonBonbon properties;

	// Token: 0x04001EFF RID: 7935
	private AbstractPlayerController player;

	// Token: 0x04001F00 RID: 7936
	private Vector3 target;

	// Token: 0x04001F01 RID: 7937
	private BaronessLevelCastle parent;

	// Token: 0x04001F02 RID: 7938
	private bool timesUp;

	// Token: 0x04001F03 RID: 7939
	private bool isActive;
}
