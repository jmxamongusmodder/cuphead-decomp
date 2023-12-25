using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000622 RID: 1570
public class FlyingBirdLevelNursePill : AbstractProjectile
{
	// Token: 0x06001FF0 RID: 8176 RVA: 0x00125600 File Offset: 0x00123A00
	protected override void FixedUpdate()
	{
		if (this.gravity)
		{
			if (this.velocity.magnitude < this.properties.pillSpeed)
			{
			}
			this.velocity.y = this.velocity.y - 10f;
		}
		base.FixedUpdate();
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x00125650 File Offset: 0x00123A50
	public void InitPill(LevelProperties.FlyingBird.Nurses properties, PlayerId target, bool parryable)
	{
		this.SetParryable(parryable);
		this.target = target;
		this.properties = properties;
		this.velocity = base.transform.up.normalized * properties.pillSpeed;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x001256A3 File Offset: 0x00123AA3
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		if (parryable)
		{
			this.parryPill.SetActive(true);
		}
		else
		{
			this.normalPill.SetActive(true);
		}
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x001256CF File Offset: 0x00123ACF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x001256F0 File Offset: 0x00123AF0
	private IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.position += this.velocity * CupheadTime.Delta;
			if (base.transform.position.y >= this.properties.pillMaxHeight && !this.gravity)
			{
				this.gravity = true;
				base.StartCoroutine(this.detonate_cr());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x0012570C File Offset: 0x00123B0C
	private IEnumerator detonate_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.pillExplodeDelay);
		AbstractPlayerController player = PlayerManager.GetPlayer(this.target);
		if (player == null || player.IsDead)
		{
			player = PlayerManager.GetNext();
		}
		base.transform.right = (player.center - base.transform.position).normalized;
		FlyingBirdLevelNursePillProjectile top = this.topHalf.Create(base.transform.position, base.transform.eulerAngles.z, this.properties.bulletSpeed) as FlyingBirdLevelNursePillProjectile;
		FlyingBirdLevelNursePillProjectile bottom = this.bottomHalf.Create(base.transform.position, base.transform.eulerAngles.z + 180f, this.properties.bulletSpeed) as FlyingBirdLevelNursePillProjectile;
		if (base.CanParry)
		{
			top.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.LightPink);
			top.SetParryable(true);
			bottom.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.DarkPink);
			bottom.SetParryable(true);
		}
		else
		{
			top.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.Yellow);
			bottom.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.Blue);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400286B RID: 10347
	[SerializeField]
	private FlyingBirdLevelNursePillProjectile topHalf;

	// Token: 0x0400286C RID: 10348
	[SerializeField]
	private FlyingBirdLevelNursePillProjectile bottomHalf;

	// Token: 0x0400286D RID: 10349
	[SerializeField]
	private GameObject normalPill;

	// Token: 0x0400286E RID: 10350
	[SerializeField]
	private GameObject parryPill;

	// Token: 0x0400286F RID: 10351
	private bool gravity;

	// Token: 0x04002870 RID: 10352
	private Vector3 velocity;

	// Token: 0x04002871 RID: 10353
	private PlayerId target;

	// Token: 0x04002872 RID: 10354
	private LevelProperties.FlyingBird.Nurses properties;
}
