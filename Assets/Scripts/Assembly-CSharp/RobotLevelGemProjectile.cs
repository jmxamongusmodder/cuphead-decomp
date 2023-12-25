using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200077C RID: 1916
public class RobotLevelGemProjectile : AbstractProjectile
{
	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x060029FC RID: 10748 RVA: 0x00188F83 File Offset: 0x00187383
	// (set) Token: 0x060029FD RID: 10749 RVA: 0x00188F8B File Offset: 0x0018738B
	public float Speed { get; private set; }

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x060029FE RID: 10750 RVA: 0x00188F94 File Offset: 0x00187394
	protected override float DestroyLifetime
	{
		get
		{
			return this.lifeTime;
		}
	}

	// Token: 0x060029FF RID: 10751 RVA: 0x00188F9C File Offset: 0x0018739C
	public virtual AbstractProjectile Init(MinMax speed, float acceleration, float waveLength, float waveSpeedMultiplier, float lifeTime, bool isBlue, bool isParryable)
	{
		base.ResetLifetime();
		base.ResetDistance();
		float num = UnityEngine.Random.Range(0.92f, 1.08f);
		this.minSpeed = speed.min * num;
		this.maxSpeed = speed.max * num;
		this.Speed = this.minSpeed;
		this.acceleration = acceleration;
		this.waveLength = waveLength;
		this.waveSpeedMultiplier = waveSpeedMultiplier;
		this.lifeTime = lifeTime;
		base.animator.SetFloat("Gem", (float)((!isBlue) ? 0 : 1));
		this.time = 0f;
		this.originalPosition = base.transform.position;
		this.SetParryable(isParryable);
		if (isParryable)
		{
			base.animator.Play("GemParry", 0, UnityEngine.Random.value);
		}
		else
		{
			base.animator.Play("Gem", 0, UnityEngine.Random.value);
		}
		base.StartCoroutine(this.speed_cr());
		base.StartCoroutine(this.fadeIn_cr());
		return this;
	}

	// Token: 0x06002A00 RID: 10752 RVA: 0x001890A4 File Offset: 0x001874A4
	protected override void Update()
	{
		base.Update();
		this.originalPosition += -base.transform.right * this.Speed * CupheadTime.Delta;
		base.transform.position = this.originalPosition + Mathf.Sin(this.time * this.waveSpeedMultiplier) * this.waveLength * base.transform.up;
		this.time += CupheadTime.Delta;
	}

	// Token: 0x06002A01 RID: 10753 RVA: 0x00189148 File Offset: 0x00187548
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002A02 RID: 10754 RVA: 0x00189166 File Offset: 0x00187566
	private void SetCollider(bool c)
	{
		base.GetComponent<CircleCollider2D>().enabled = c;
	}

	// Token: 0x06002A03 RID: 10755 RVA: 0x00189174 File Offset: 0x00187574
	private IEnumerator effect_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0f, 0.3f));
		for (;;)
		{
			this.effectPrefab.Create(this.effectRoot.position);
			yield return CupheadTime.WaitForSeconds(this, 0.3f);
		}
		yield break;
	}

	// Token: 0x06002A04 RID: 10756 RVA: 0x00189190 File Offset: 0x00187590
	private IEnumerator speed_cr()
	{
		this.Speed = this.minSpeed;
		while (this.Speed < this.maxSpeed)
		{
			this.Speed += this.acceleration;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x001891AC File Offset: 0x001875AC
	private IEnumerator fadeIn_cr()
	{
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		while (sprite.color.a < 1f)
		{
			Color c = sprite.color;
			c.a += 1f * CupheadTime.Delta;
			sprite.color = c;
			yield return null;
		}
		Color color = sprite.color;
		color.a = 1f;
		sprite.color = color;
		yield break;
	}

	// Token: 0x06002A06 RID: 10758 RVA: 0x001891C7 File Offset: 0x001875C7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.effectPrefab = null;
	}

	// Token: 0x06002A07 RID: 10759 RVA: 0x001891D6 File Offset: 0x001875D6
	public override void OnParryDie()
	{
		this.Recycle<RobotLevelGemProjectile>();
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x001891DE File Offset: 0x001875DE
	protected override void OnDieDistance()
	{
		this.Recycle<RobotLevelGemProjectile>();
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x001891E6 File Offset: 0x001875E6
	protected override void OnDieLifetime()
	{
		this.Recycle<RobotLevelGemProjectile>();
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x001891EE File Offset: 0x001875EE
	protected override void OnDieAnimationComplete()
	{
		this.Recycle<RobotLevelGemProjectile>();
	}

	// Token: 0x040032DE RID: 13022
	private const string GemParameterName = "Gem";

	// Token: 0x040032DF RID: 13023
	private const string GemParryParameterName = "GemParry";

	// Token: 0x040032E0 RID: 13024
	private const float SpeedVariation = 0.08f;

	// Token: 0x040032E1 RID: 13025
	private const float FadeTime = 0.3f;

	// Token: 0x040032E2 RID: 13026
	private const float FadeRate = 0.3f;

	// Token: 0x040032E4 RID: 13028
	[SerializeField]
	private Effect effectPrefab;

	// Token: 0x040032E5 RID: 13029
	[SerializeField]
	private Transform effectRoot;

	// Token: 0x040032E6 RID: 13030
	private Vector3 originalPosition;

	// Token: 0x040032E7 RID: 13031
	private Vector3 originalScale;

	// Token: 0x040032E8 RID: 13032
	private float minSpeed;

	// Token: 0x040032E9 RID: 13033
	private float maxSpeed;

	// Token: 0x040032EA RID: 13034
	private float acceleration;

	// Token: 0x040032EB RID: 13035
	private float waveLength;

	// Token: 0x040032EC RID: 13036
	private float waveSpeedMultiplier;

	// Token: 0x040032ED RID: 13037
	private float time;

	// Token: 0x040032EE RID: 13038
	private float lifeTime;
}
