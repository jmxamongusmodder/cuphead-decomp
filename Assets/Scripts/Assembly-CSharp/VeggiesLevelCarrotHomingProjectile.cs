using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000848 RID: 2120
public class VeggiesLevelCarrotHomingProjectile : HomingProjectile
{
	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x0600310F RID: 12559 RVA: 0x001CCAF2 File Offset: 0x001CAEF2
	protected override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06003110 RID: 12560 RVA: 0x001CCAF9 File Offset: 0x001CAEF9
	// (set) Token: 0x06003111 RID: 12561 RVA: 0x001CCB01 File Offset: 0x001CAF01
	public VeggiesLevelCarrotHomingProjectile.State state { get; private set; }

	// Token: 0x06003112 RID: 12562 RVA: 0x001CCB0C File Offset: 0x001CAF0C
	public VeggiesLevelCarrotHomingProjectile Create(AbstractPlayerController player, VeggiesLevelCarrot parent, Vector2 pos, float speed, float rotationSpeed, float health)
	{
		VeggiesLevelCarrotHomingProjectile veggiesLevelCarrotHomingProjectile = base.Create(pos, -90f, speed, speed, rotationSpeed, this.DestroyLifetime, 0f, player) as VeggiesLevelCarrotHomingProjectile;
		veggiesLevelCarrotHomingProjectile.CollisionDeath.OnlyPlayer();
		veggiesLevelCarrotHomingProjectile.DamagesType.OnlyPlayer();
		veggiesLevelCarrotHomingProjectile.Init(parent, health);
		return veggiesLevelCarrotHomingProjectile;
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x001CCB5E File Offset: 0x001CAF5E
	private void LateUpdate()
	{
		this.UpdateHitBox();
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x001CCB66 File Offset: 0x001CAF66
	private void UpdateHitBox()
	{
		this.hitBox.position = base.transform.position;
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x001CCB7E File Offset: 0x001CAF7E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.parent.OnDeathEvent -= this.OnDeath;
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x001CCB9D File Offset: 0x001CAF9D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x001CCBA7 File Offset: 0x001CAFA7
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		this.Die();
	}

	// Token: 0x06003118 RID: 12568 RVA: 0x001CCBB8 File Offset: 0x001CAFB8
	protected override void Die()
	{
		if (!base.GetComponent<Collider2D>().enabled)
		{
			return;
		}
		base.Die();
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<Collider2D>().enabled = false;
		this.hitBox.gameObject.SetActive(false);
		this.StopAllCoroutines();
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(-90f));
	}

	// Token: 0x06003119 RID: 12569 RVA: 0x001CCC38 File Offset: 0x001CB038
	private void Init(VeggiesLevelCarrot parent, float health)
	{
		this.hitBox.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.parent = parent;
		this.health = health;
		parent.OnDeathEvent += this.OnDeath;
		base.transform.localScale = Vector3.one * (1f + UnityEngine.Random.Range(0.1f, -0.1f));
	}

	// Token: 0x0600311A RID: 12570 RVA: 0x001CCCAC File Offset: 0x001CB0AC
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f && this.state != VeggiesLevelCarrotHomingProjectile.State.Dead)
		{
			this.state = VeggiesLevelCarrotHomingProjectile.State.Dead;
			AudioManager.Play("level_veggies_carrot_projectile_death");
			this.emitAudioFromObject.Add("level_veggies_carrot_projectile_death");
			base.StartCoroutine(this.dying_cr());
		}
	}

	// Token: 0x0600311B RID: 12571 RVA: 0x001CCD18 File Offset: 0x001CB118
	private IEnumerator dying_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x0600311C RID: 12572 RVA: 0x001CCD33 File Offset: 0x001CB133
	private void OnDeath()
	{
		this.hitBox.gameObject.SetActive(false);
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.parentDied_cr());
	}

	// Token: 0x0600311D RID: 12573 RVA: 0x001CCD5F File Offset: 0x001CB15F
	private void End()
	{
		this.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x0600311E RID: 12574 RVA: 0x001CCD70 File Offset: 0x001CB170
	private IEnumerator parentDied_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0f, 0.5f));
		this.End();
		yield break;
	}

	// Token: 0x040039B2 RID: 14770
	public const float SCALE_RAND = 0.1f;

	// Token: 0x040039B4 RID: 14772
	[SerializeField]
	private Transform hitBox;

	// Token: 0x040039B5 RID: 14773
	private VeggiesLevelCarrot parent;

	// Token: 0x040039B6 RID: 14774
	private float health;

	// Token: 0x02000849 RID: 2121
	public enum State
	{
		// Token: 0x040039B8 RID: 14776
		In,
		// Token: 0x040039B9 RID: 14777
		InComplete,
		// Token: 0x040039BA RID: 14778
		Firing,
		// Token: 0x040039BB RID: 14779
		Dead
	}
}
