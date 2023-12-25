using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005A3 RID: 1443
public class DicePalaceBoozeLevelTumbler : DicePalaceBoozeLevelBossBase
{
	// Token: 0x06001BC5 RID: 7109 RVA: 0x000FD30A File Offset: 0x000FB70A
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x000FD340 File Offset: 0x000FB740
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x000FD350 File Offset: 0x000FB750
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		float health = this.health;
		this.health -= info.damage;
		if (health > 0f)
		{
			Level.Current.timeline.DealDamage(Mathf.Clamp(health - this.health, 0f, health));
		}
		if (this.health < 0f && !base.isDead)
		{
			this.StartDying();
			this.TumblerDeathSFX();
		}
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x000FD3CC File Offset: 0x000FB7CC
	public override void LevelInit(LevelProperties.DicePalaceBooze properties)
	{
		this.attackDelayIndex = UnityEngine.Random.Range(0, properties.CurrentState.tumbler.beamDelayString.Split(new char[]
		{
			','
		}).Length);
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		Level.Current.OnWinEvent += this.HandleDead;
		this.health = properties.CurrentState.tumbler.tumblerHP;
		AudioManager.Play("booze_tumbler_intro");
		this.emitAudioFromObject.Add("booze_tumbler_intro");
		base.LevelInit(properties);
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x000FD46B File Offset: 0x000FB86B
	private void OnIntroEnd()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001BCA RID: 7114 RVA: 0x000FD47C File Offset: 0x000FB87C
	private IEnumerator attack_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.tumbler.beamDelayString.Split(new char[]
			{
				','
			})[this.attackDelayIndex]) - DicePalaceBoozeLevelBossBase.ATTACK_DELAY);
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Start", false, true);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.tumbler.beamWarningDuration);
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
			AudioManager.Play("booze_tumbler_attack");
			this.emitAudioFromObject.Add("booze_tumbler_attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
			this.attackDelayIndex = (this.attackDelayIndex + 1) % base.properties.CurrentState.tumbler.beamDelayString.Split(new char[]
			{
				','
			}).Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x000FD497 File Offset: 0x000FB897
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x000FD4B5 File Offset: 0x000FB8B5
	private void EnableSpray()
	{
		AudioManager.Play("booze_tumbler_attack_spray");
		this.emitAudioFromObject.Add("booze_tumbler_attack_spray");
		base.animator.Play("Attack_Spray");
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x000FD4E1 File Offset: 0x000FB8E1
	private void TumblerDeathSFX()
	{
		AudioManager.Play("tumbler_death_vox");
		this.emitAudioFromObject.Add("tumbler_death_vox");
	}

	// Token: 0x040024D4 RID: 9428
	[SerializeField]
	private BoxCollider2D sprayCollider;

	// Token: 0x040024D5 RID: 9429
	private int attackDelayIndex;

	// Token: 0x040024D6 RID: 9430
	private DamageDealer damageDealer;

	// Token: 0x040024D7 RID: 9431
	private DamageReceiver damageReceiver;
}
