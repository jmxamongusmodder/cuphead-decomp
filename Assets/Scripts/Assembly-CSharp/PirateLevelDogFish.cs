using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000722 RID: 1826
public class PirateLevelDogFish : AbstractProjectile
{
	// Token: 0x060027C0 RID: 10176 RVA: 0x0017451C File Offset: 0x0017291C
	protected override void Awake()
	{
		base.Awake();
		base.transform.position = PirateLevelDogFish.START_POS;
		this.normalHitBox.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.normalHitBox.GetComponent<DamageReceiver>().OnDamageTaken += this.onDamageTaken;
		this.secretHitBox.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTakenFromBehind;
	}

	// Token: 0x060027C1 RID: 10177 RVA: 0x0017459C File Offset: 0x0017299C
	protected override void Update()
	{
		base.Update();
		if (this.state == PirateLevelDogFish.State.Slide)
		{
			base.transform.AddPosition(-this.speedY * CupheadTime.Delta, 0f, 0f);
			float num = this.slideTime / this.dogfish.speedFalloffTime;
			if (num < 1f)
			{
				this.speedY = EaseUtils.EaseOutQuart(this.dogfish.startSpeed, this.dogfish.endSpeed, num);
				this.slideTime += CupheadTime.Delta;
			}
			else
			{
				this.speedY = this.dogfish.endSpeed;
			}
			if (base.transform.position.x < -1000f)
			{
				this.properties.OnBossDeath -= this.OnBossDeath;
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (this.bossDied)
			{
				this.Die();
			}
			if (PirateLevelDogFish.dogKilled && this.isSecret)
			{
				this.isSecret = false;
				this.OnEnableCollider();
			}
		}
	}

	// Token: 0x060027C2 RID: 10178 RVA: 0x001746C1 File Offset: 0x00172AC1
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.state != PirateLevelDogFish.State.Death)
		{
			base.OnCollisionPlayer(hit, phase);
			if (phase != CollisionPhase.Exit)
			{
				this.damageDealer.DealDamage(hit);
			}
		}
	}

	// Token: 0x060027C3 RID: 10179 RVA: 0x001746EC File Offset: 0x00172AEC
	public void Init(LevelProperties.Pirate properties, bool isSecret)
	{
		this.properties = properties;
		this.dogfish = properties.CurrentState.dogFish;
		this.isSecret = isSecret;
		this.hp = (float)this.dogfish.hp;
		this.state = PirateLevelDogFish.State.Jump;
		this.normalHitBox.GetComponent<DamageReceiver>().enabled = false;
		AudioManager.Play("level_pirate_dogfish_jump");
		this.emitAudioFromObject.Add("level_pirate_dogfish_jump");
		this.splashEffect.Create(this.splashRoot.position);
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x060027C4 RID: 10180 RVA: 0x00174788 File Offset: 0x00172B88
	private void onDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f & this.state != PirateLevelDogFish.State.Death)
		{
			this.OnDying();
			this.secretHitBox.GetComponent<Collider2D>().enabled = false;
			this.Die();
		}
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x001747E4 File Offset: 0x00172BE4
	private void OnDamageTakenFromBehind(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != PirateLevelDogFish.State.Death)
		{
			this.OnDying();
			this.SetParryable(true);
			this.Die();
		}
	}

	// Token: 0x060027C6 RID: 10182 RVA: 0x00174834 File Offset: 0x00172C34
	private void OnDying()
	{
		AudioManager.Stop("level_pirate_dogfish_jump");
		AudioManager.Play("level_pirate_dogfish_death_poof");
		this.emitAudioFromObject.Add("level_pirate_dogfish_death_poof");
		PirateLevelDogFish.dogKilled = true;
		this.normalHitBox.GetComponent<Collider2D>().enabled = false;
		this.secretHitBox.GetComponent<DamageReceiver>().enabled = false;
	}

	// Token: 0x060027C7 RID: 10183 RVA: 0x0017488D File Offset: 0x00172C8D
	private void OnEnableCollider()
	{
		this.normalHitBox.GetComponent<DamageReceiver>().enabled = true;
		this.secretHitBox.GetComponent<DamageReceiver>().enabled = false;
		base.gameObject.layer = 0;
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x001748C0 File Offset: 0x00172CC0
	private void OnJumpAnimationComplete()
	{
		if (this.state != PirateLevelDogFish.State.Death)
		{
			this.state = PirateLevelDogFish.State.Slide;
			AudioManager.Play("level_pirate_dogfish_slide");
			this.emitAudioFromObject.Add("level_pirate_dogfish_slide");
			this.slideTime = 0f;
			this.speedY = this.dogfish.startSpeed;
		}
	}

	// Token: 0x060027C9 RID: 10185 RVA: 0x00174918 File Offset: 0x00172D18
	protected override void Die()
	{
		this.state = PirateLevelDogFish.State.Death;
		this.properties.OnBossDeath -= this.OnBossDeath;
		base.animator.SetTrigger("OnDeath");
		this.deathEffect.Create(base.transform.position);
		base.StartCoroutine(this.deathFloat_cr());
	}

	// Token: 0x060027CA RID: 10186 RVA: 0x00174977 File Offset: 0x00172D77
	private void OnBossDeath()
	{
		this.bossDied = true;
		if (this.state == PirateLevelDogFish.State.Slide)
		{
			this.Die();
		}
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x00174994 File Offset: 0x00172D94
	private IEnumerator deathFloat_cr()
	{
		AudioManager.Play("level_pirate_dogfish_death_flap");
		this.emitAudioFromObject.Add("level_pirate_dogfish_death_flap");
		while (base.transform.position.y < 360f)
		{
			base.transform.AddPosition(0f, this.properties.CurrentState.dogFish.deathSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060027CC RID: 10188 RVA: 0x001749AF File Offset: 0x00172DAF
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.splashEffect = null;
		this.deathEffect = null;
	}

	// Token: 0x0400307D RID: 12413
	private static readonly Vector2 START_POS = new Vector2(235f, -245f);

	// Token: 0x0400307E RID: 12414
	private const float DEATH_Y = 450f;

	// Token: 0x0400307F RID: 12415
	[SerializeField]
	private Collider2D secretHitBox;

	// Token: 0x04003080 RID: 12416
	[SerializeField]
	private Collider2D normalHitBox;

	// Token: 0x04003081 RID: 12417
	[SerializeField]
	private Effect splashEffect;

	// Token: 0x04003082 RID: 12418
	[SerializeField]
	private Transform splashRoot;

	// Token: 0x04003083 RID: 12419
	[SerializeField]
	private Effect deathEffect;

	// Token: 0x04003084 RID: 12420
	private PirateLevelDogFish.State state;

	// Token: 0x04003085 RID: 12421
	private float hp;

	// Token: 0x04003086 RID: 12422
	private float speedY;

	// Token: 0x04003087 RID: 12423
	private float slideTime;

	// Token: 0x04003088 RID: 12424
	private LevelProperties.Pirate properties;

	// Token: 0x04003089 RID: 12425
	private LevelProperties.Pirate.DogFish dogfish;

	// Token: 0x0400308A RID: 12426
	private bool bossDied;

	// Token: 0x0400308B RID: 12427
	private bool isSecret;

	// Token: 0x0400308C RID: 12428
	public static bool dogKilled = false;

	// Token: 0x02000723 RID: 1827
	public enum State
	{
		// Token: 0x0400308E RID: 12430
		Init,
		// Token: 0x0400308F RID: 12431
		Jump,
		// Token: 0x04003090 RID: 12432
		Slide,
		// Token: 0x04003091 RID: 12433
		Death
	}
}
