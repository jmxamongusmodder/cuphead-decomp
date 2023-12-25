using System;
using UnityEngine;

// Token: 0x020008C9 RID: 2249
public class HarbourPlatformingLevelIceberg : AbstractCollidableObject
{
	// Token: 0x06003490 RID: 13456 RVA: 0x001E8647 File Offset: 0x001E6A47
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x001E8654 File Offset: 0x001E6A54
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x001E866C File Offset: 0x001E6A6C
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<HarbourPlatformingLevelOctoProjectile>())
		{
			this.SmashSFX();
			UnityEngine.Object.Destroy(hit.gameObject);
			this.DeathParts();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003493 RID: 13459 RVA: 0x001E86A8 File Offset: 0x001E6AA8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003494 RID: 13460 RVA: 0x001E86C8 File Offset: 0x001E6AC8
	public void DeathParts()
	{
		this.explosion.Create(base.transform.position);
		foreach (SpriteDeathParts spriteDeathParts in this.deathParts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x001E871D File Offset: 0x001E6B1D
	private void SmashSFX()
	{
		AudioManager.Play("harbour_iceberg_smash");
		this.emitAudioFromObject.Add("harbour_iceberg_smash");
	}

	// Token: 0x04003CBE RID: 15550
	[SerializeField]
	private Effect explosion;

	// Token: 0x04003CBF RID: 15551
	[SerializeField]
	private SpriteDeathParts[] deathParts;

	// Token: 0x04003CC0 RID: 15552
	private DamageDealer damageDealer;
}
