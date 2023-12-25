using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public class AirplaneLevelRocket : HomingProjectile
{
	// Token: 0x06001454 RID: 5204 RVA: 0x000B64A0 File Offset: 0x000B48A0
	public AirplaneLevelRocket Create(AbstractPlayerController player, Vector2 pos, float speed, float rotationSpeed, float health, float homingTime)
	{
		AirplaneLevelRocket airplaneLevelRocket = base.Create(pos, -90f, speed, speed, rotationSpeed, this.DestroyLifetime, 0f, player) as AirplaneLevelRocket;
		airplaneLevelRocket.DamagesType.OnlyPlayer();
		airplaneLevelRocket.Init(health);
		airplaneLevelRocket.homingTimer = homingTime;
		return airplaneLevelRocket;
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x000B64EC File Offset: 0x000B48EC
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.sfx_rocket_spawn_cr());
		base.StartCoroutine(this.spawn_effect_cr());
	}

	// Token: 0x06001456 RID: 5206 RVA: 0x000B650E File Offset: 0x000B490E
	private void Init(float health)
	{
		this.health = health;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001457 RID: 5207 RVA: 0x000B652E File Offset: 0x000B492E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_Impact");
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x000B6544 File Offset: 0x000B4944
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health <= 0f)
		{
			return;
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Die();
		}
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x000B6598 File Offset: 0x000B4998
	private IEnumerator continue_without_homing_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x000B65B4 File Offset: 0x000B49B4
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		this.sprite.GetComponent<SpriteRenderer>().enabled = false;
		GameObject gameObject = GameObject.Find("BullDogPlane");
		if (gameObject && Mathf.Abs(gameObject.transform.position.x - base.transform.position.x) < 800f && Mathf.Abs(gameObject.transform.position.y - base.transform.position.y) < 175f)
		{
			this.deathOnPlaneFX.Create(base.transform.position);
		}
		else
		{
			this.deathFX.Create(base.transform.position);
		}
		AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_DeathExplode");
	}

	// Token: 0x0600145B RID: 5211 RVA: 0x000B66AC File Offset: 0x000B4AAC
	private IEnumerator spawn_effect_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.fxSpawnRate.RandomFloat());
			this.effectFX.Create(this.effectRoot.position);
			AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_Chuff");
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600145C RID: 5212 RVA: 0x000B66C8 File Offset: 0x000B4AC8
	protected override void Update()
	{
		base.Update();
		if (this.homingTimer > 0f)
		{
			this.homingTimer -= CupheadTime.Delta;
			if (this.homingTimer <= 0f)
			{
				this.StopAllCoroutines();
				base.StartCoroutine(this.continue_without_homing_cr());
			}
		}
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x000B6728 File Offset: 0x000B4B28
	private IEnumerator sfx_rocket_spawn_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("sfx_DLC_Dogfight_P1_HydrantMissile_Entrance");
		yield break;
	}

	// Token: 0x04001DA3 RID: 7587
	private float homingTimer;

	// Token: 0x04001DA4 RID: 7588
	private float health;

	// Token: 0x04001DA5 RID: 7589
	[SerializeField]
	private Transform effectRoot;

	// Token: 0x04001DA6 RID: 7590
	[SerializeField]
	private Effect effectFX;

	// Token: 0x04001DA7 RID: 7591
	[SerializeField]
	private Effect deathFX;

	// Token: 0x04001DA8 RID: 7592
	[SerializeField]
	private Effect deathOnPlaneFX;

	// Token: 0x04001DA9 RID: 7593
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x04001DAA RID: 7594
	[SerializeField]
	private MinMax fxSpawnRate;
}
