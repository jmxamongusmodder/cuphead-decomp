using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200084F RID: 2127
public class VeggiesLevelOnionHomingHeart : AbstractProjectile
{
	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x0600314A RID: 12618 RVA: 0x001CDCBE File Offset: 0x001CC0BE
	// (set) Token: 0x0600314B RID: 12619 RVA: 0x001CDCC6 File Offset: 0x001CC0C6
	public VeggiesLevelOnionHomingHeart.State state { get; private set; }

	// Token: 0x0600314C RID: 12620 RVA: 0x001CDCD0 File Offset: 0x001CC0D0
	public VeggiesLevelOnionHomingHeart CreateRadish(Vector2 pos, float max, float acc, int hp, bool onLeft)
	{
		base.transform.position = pos;
		VeggiesLevelOnionHomingHeart veggiesLevelOnionHomingHeart = base.Create() as VeggiesLevelOnionHomingHeart;
		veggiesLevelOnionHomingHeart.maxSpeed = max;
		veggiesLevelOnionHomingHeart.acceletration = acc;
		veggiesLevelOnionHomingHeart.health = (float)hp;
		veggiesLevelOnionHomingHeart.isOnLeft = onLeft;
		return veggiesLevelOnionHomingHeart;
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x0600314D RID: 12621 RVA: 0x001CDD1A File Offset: 0x001CC11A
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x001CDD24 File Offset: 0x001CC124
	protected override void Start()
	{
		if (!this.isOnLeft)
		{
			base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
		}
		base.Start();
		this.sprite = base.GetComponent<SpriteRenderer>();
		this.homingMovement = base.GetComponent<GroundHomingMovement>();
		this.homingMovement.maxSpeed = this.maxSpeed;
		this.homingMovement.acceleration = this.acceletration;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x0600314F RID: 12623 RVA: 0x001CDDE4 File Offset: 0x001CC1E4
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x001CDE02 File Offset: 0x001CC202
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x001CDE20 File Offset: 0x001CC220
	private IEnumerator start_cr()
	{
		this.state = VeggiesLevelOnionHomingHeart.State.Alive;
		this.sprite.sortingLayerName = SpriteLayer.Enemies.ToString();
		this.sprite.sortingOrder = 0;
		yield return base.animator.WaitForAnimationToEnd(this, "Radish_Intro", false, true);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AudioManager.PlayLoop("level_veggies_raddish_loop");
		this.emitAudioFromObject.Add("level_veggies_raddish_loop");
		this.homingMovement.EnableHoming = true;
		base.StartCoroutine(this.loop_cr());
		yield break;
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x001CDE3B File Offset: 0x001CC23B
	private void ChangeLayer()
	{
		this.sprite.sortingOrder = 3;
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x001CDE4C File Offset: 0x001CC24C
	private IEnumerator loop_cr()
	{
		while (this.state != VeggiesLevelOnionHomingHeart.State.Dead)
		{
			this.homingMovement.TrackingPlayer = PlayerManager.GetNext();
			yield return CupheadTime.WaitForSeconds(this, 20f);
		}
		yield break;
	}

	// Token: 0x06003154 RID: 12628 RVA: 0x001CDE68 File Offset: 0x001CC268
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && this.state != VeggiesLevelOnionHomingHeart.State.Dead)
		{
			this.state = VeggiesLevelOnionHomingHeart.State.Dead;
			this.homingMovement.enabled = false;
			this.StopAllCoroutines();
			AudioManager.Stop("level_veggies_raddish_loop");
			AudioManager.Play("level_veggies_raddish_End");
			this.emitAudioFromObject.Add("level_veggies_raddish_End");
			base.animator.SetTrigger("Dead");
		}
	}

	// Token: 0x06003155 RID: 12629 RVA: 0x001CDEF1 File Offset: 0x001CC2F1
	private void CreateEffect()
	{
		this.deathPoof.Create(base.transform.position);
	}

	// Token: 0x06003156 RID: 12630 RVA: 0x001CDF0C File Offset: 0x001CC30C
	private void CreatePieces()
	{
		foreach (SpriteDeathParts spriteDeathParts in this.deathPieces)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x06003157 RID: 12631 RVA: 0x001CDF4A File Offset: 0x001CC34A
	private void Destroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003158 RID: 12632 RVA: 0x001CDF57 File Offset: 0x001CC357
	private void RaddishBonkSFX()
	{
		AudioManager.Play("level_veggies_raddish_bonk");
		this.emitAudioFromObject.Add("level_veggies_raddish_bonk");
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x001CDF73 File Offset: 0x001CC373
	private void RaddishLoopStartSFX()
	{
		AudioManager.Play("level_veggies_raddish_start");
		this.emitAudioFromObject.Add("level_veggies_raddish_start");
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x001CDF8F File Offset: 0x001CC38F
	private void RaddishDeathSFX()
	{
		AudioManager.Play("level_veggies_raddish_death");
		this.emitAudioFromObject.Add("level_veggies_raddish_death");
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x001CDFAB File Offset: 0x001CC3AB
	private void RaddishVoiceDeathSFX()
	{
		AudioManager.Play("veggies_Raddish_Voice_Death");
		this.emitAudioFromObject.Add("veggies_Raddish_Voice_Death");
	}

	// Token: 0x0600315C RID: 12636 RVA: 0x001CDFC7 File Offset: 0x001CC3C7
	private void RaddishVoiceIntroSFX()
	{
		AudioManager.Play("veggies_Raddish_Voice_Intro");
		this.emitAudioFromObject.Add("veggies_Raddish_Voice_Intro");
	}

	// Token: 0x040039DD RID: 14813
	[SerializeField]
	private Effect deathPoof;

	// Token: 0x040039DE RID: 14814
	[SerializeField]
	private SpriteDeathParts[] deathPieces;

	// Token: 0x040039DF RID: 14815
	private AbstractPlayerController player;

	// Token: 0x040039E0 RID: 14816
	private bool isOnLeft;

	// Token: 0x040039E2 RID: 14818
	private SpriteRenderer sprite;

	// Token: 0x040039E3 RID: 14819
	private GroundHomingMovement homingMovement;

	// Token: 0x040039E4 RID: 14820
	private DamageReceiver damageReceiver;

	// Token: 0x040039E5 RID: 14821
	private float maxSpeed;

	// Token: 0x040039E6 RID: 14822
	private float acceletration;

	// Token: 0x040039E7 RID: 14823
	private float health;

	// Token: 0x02000850 RID: 2128
	public enum State
	{
		// Token: 0x040039E9 RID: 14825
		Alive,
		// Token: 0x040039EA RID: 14826
		Dead
	}
}
