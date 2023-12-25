using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006B8 RID: 1720
public class FrogsLevelShort : LevelProperties.Frogs.Entity
{
	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x06002477 RID: 9335 RVA: 0x00155C70 File Offset: 0x00154070
	// (set) Token: 0x06002478 RID: 9336 RVA: 0x00155C78 File Offset: 0x00154078
	public FrogsLevelShort.State state { get; private set; }

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x06002479 RID: 9337 RVA: 0x00155C81 File Offset: 0x00154081
	// (set) Token: 0x0600247A RID: 9338 RVA: 0x00155C89 File Offset: 0x00154089
	public FrogsLevelShort.Direction direction { get; private set; }

	// Token: 0x0600247B RID: 9339 RVA: 0x00155C94 File Offset: 0x00154094
	protected override void Awake()
	{
		base.Awake();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 0.3f, DamageDealer.DamageSource.Enemy, true, false, false);
	}

	// Token: 0x0600247C RID: 9340 RVA: 0x00155CEF File Offset: 0x001540EF
	private void Start()
	{
		Level.Current.OnIntroEvent += this.OnLevelIntro;
	}

	// Token: 0x0600247D RID: 9341 RVA: 0x00155D07 File Offset: 0x00154107
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x0600247E RID: 9342 RVA: 0x00155D14 File Offset: 0x00154114
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600247F RID: 9343 RVA: 0x00155D32 File Offset: 0x00154132
	public override void LevelInit(LevelProperties.Frogs properties)
	{
		base.LevelInit(properties);
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x06002480 RID: 9344 RVA: 0x00155D4D File Offset: 0x0015414D
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (FrogsLevel.FINAL_FORM)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002481 RID: 9345 RVA: 0x00155D6C File Offset: 0x0015416C
	private void OnBossDeath()
	{
		AudioManager.Play("level_frogs_short_death");
		this.emitAudioFromObject.Add("level_frogs_short_death");
		AudioManager.PlayLoop("level_frogs_short_death_loop");
		this.emitAudioFromObject.Add("level_frogs_short_death_loop");
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x00155DC3 File Offset: 0x001541C3
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.introDust = null;
		this.rageFireball = null;
		this.rageFireballSpark = null;
		this.clapBullet = null;
		this.clapEffect = null;
	}

	// Token: 0x06002483 RID: 9347 RVA: 0x00155DEE File Offset: 0x001541EE
	private void SfxClap()
	{
		AudioManager.Play("level_frogs_short_clap_shock");
		this.emitAudioFromObject.Add("level_frogs_short_clap_shock");
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x00155E0A File Offset: 0x0015420A
	private void SfxEndIntro()
	{
		AudioManager.Stop("level_frogs_short_intro_loop");
		AudioManager.Play("level_frogs_short_intro_start");
		this.emitAudioFromObject.Add("level_frogs_short_intro_start");
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x00155E30 File Offset: 0x00154230
	private void OnLevelIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x00155E40 File Offset: 0x00154240
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AudioManager.PlayLoop("level_frogs_short_intro_loop");
		this.emitAudioFromObject.Add("level_frogs_short_intro_loop");
		base.animator.Play("Intro");
		yield break;
	}

	// Token: 0x06002487 RID: 9351 RVA: 0x00155E5B File Offset: 0x0015425B
	private void PlayIntroEffect()
	{
		this.introDust.Create(base.transform.position);
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x00155E74 File Offset: 0x00154274
	public void StartRage()
	{
		if (this.state != FrogsLevelShort.State.Idle && this.state != FrogsLevelShort.State.Complete)
		{
			return;
		}
		this.state = FrogsLevelShort.State.Rage;
		base.StartCoroutine(this.rage_cr());
	}

	// Token: 0x06002489 RID: 9353 RVA: 0x00155EA8 File Offset: 0x001542A8
	private void Shoot(LevelProperties.Frogs.ShortRage properties, Vector3 pos, bool parry)
	{
		int num = (this.direction != FrogsLevelShort.Direction.Left) ? 1 : -1;
		AudioManager.Play("level_frogs_short_fireball");
		this.emitAudioFromObject.Add("level_frogs_short_fireball");
		this.rageFireballSpark.Create(pos, new Vector3((float)num, (float)num, 1f));
		BasicProjectile basicProjectile = this.rageFireball.Create(pos, 0f, new Vector2((float)num, (float)num), properties.shotSpeed * (float)num);
		basicProjectile.SetParryable(parry);
		basicProjectile.CollisionDeath.OnlyPlayer();
	}

	// Token: 0x0600248A RID: 9354 RVA: 0x00155F38 File Offset: 0x00154338
	private IEnumerator rage_cr()
	{
		LevelProperties.Frogs.ShortRage p = base.properties.CurrentState.shortRage;
		base.animator.SetTrigger("OnRage");
		yield return base.animator.WaitForAnimationToEnd(this, "Rage", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.anticipationDelay);
		base.animator.SetTrigger("OnRageAttack");
		yield return base.animator.WaitForAnimationToEnd(this, "Rage_Anticipate_End", false, true);
		AudioManager.PlayLoop("level_frogs_short_ragefist_attack_loop");
		this.emitAudioFromObject.Add("level_frogs_short_ragefist_attack_loop");
		int shotCount = p.shotCount;
		int root = 0;
		string parryString = p.parryPatterns[UnityEngine.Random.Range(0, p.parryPatterns.Length)].ToLower();
		int parryIndex = 0;
		while (shotCount > 0)
		{
			yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
			shotCount--;
			this.Shoot(p, this.rageRoots[root].position, parryString[parryIndex] == 'p');
			root = (int)Mathf.Repeat((float)(root + 1), (float)this.rageRoots.Length);
			parryIndex = (int)Mathf.Repeat((float)(parryIndex + 1), (float)parryString.Length);
		}
		yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
		base.animator.SetTrigger("OnRageEnd");
		AudioManager.Stop("level_frogs_short_ragefist_attack_loop");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = FrogsLevelShort.State.Complete;
		yield break;
	}

	// Token: 0x0600248B RID: 9355 RVA: 0x00155F53 File Offset: 0x00154353
	public void StartClap()
	{
		if (this.state != FrogsLevelShort.State.Idle && this.state != FrogsLevelShort.State.Complete)
		{
			return;
		}
		this.state = FrogsLevelShort.State.Clap;
		base.StartCoroutine(this.clap_cr());
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x00155F88 File Offset: 0x00154388
	private void ShootClap()
	{
		this.SfxClap();
		this.clapEffect.Create(this.clapRoot.position);
		this.clapBullet.Create(this.direction, this.clapDirection, this.clapRoot.position, this.clapRoot.right * this.clapProperties.bulletSpeed);
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x00155FFC File Offset: 0x001543FC
	private IEnumerator clap_cr()
	{
		this.clapProperties = base.properties.CurrentState.shortClap;
		this.clapDirection = ((!Rand.Bool()) ? FrogsLevelShortClapBullet.Direction.Up : FrogsLevelShortClapBullet.Direction.Down);
		this.clapRoot.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.clapProperties.angles.GetRandom<float>()));
		string patternString = this.clapProperties.patterns[UnityEngine.Random.Range(0, this.clapProperties.patterns.Length)];
		KeyValue[] pattern = KeyValue.ListFromString(patternString, new char[]
		{
			'S',
			'D'
		});
		base.animator.SetTrigger("OnClap");
		base.animator.SetBool("Clapping", true);
		yield return CupheadTime.WaitForSeconds(this, 1f + this.clapProperties.shotDelay);
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].key == "S")
			{
				int ii = 0;
				while ((float)ii < pattern[i].value)
				{
					this.clapDirection = ((this.clapDirection != FrogsLevelShortClapBullet.Direction.Down) ? FrogsLevelShortClapBullet.Direction.Down : FrogsLevelShortClapBullet.Direction.Up);
					if (i >= pattern.Length - 1 && (float)ii >= pattern[i].value - 1f)
					{
						base.animator.Play("Clap_End");
						yield return base.animator.WaitForAnimationToEnd(this, "Clap_End", false, true);
					}
					else
					{
						base.animator.Play("Clap_Shoot");
					}
					yield return CupheadTime.WaitForSeconds(this, 0.5f);
					ii++;
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
		}
		base.animator.Play("Idle");
		base.animator.ResetTrigger("OnClap");
		base.animator.SetBool("Clapping", false);
		yield return CupheadTime.WaitForSeconds(this, this.clapProperties.hesitate);
		this.state = FrogsLevelShort.State.Complete;
		yield break;
	}

	// Token: 0x0600248E RID: 9358 RVA: 0x00156017 File Offset: 0x00154417
	public void StartRoll()
	{
		this.StopAllCoroutines();
		base.animator.Play("Idle");
		this.state = FrogsLevelShort.State.Roll;
		base.StartCoroutine(this.roll_cr());
	}

	// Token: 0x0600248F RID: 9359 RVA: 0x00156043 File Offset: 0x00154443
	private bool CheckRollable()
	{
		return this.state == FrogsLevelShort.State.Complete || this.state == FrogsLevelShort.State.Idle;
	}

	// Token: 0x06002490 RID: 9360 RVA: 0x00156064 File Offset: 0x00154464
	private IEnumerator roll_cr()
	{
		yield return null;
		float startX = base.transform.position.x;
		float endX = -(startX + 240f);
		LevelProperties.Frogs.ShortRoll p = base.properties.CurrentState.shortRoll;
		base.animator.SetTrigger("OnRoll");
		yield return CupheadTime.WaitForSeconds(this, 1.2f + p.delay);
		base.animator.SetTrigger("OnRollContinue");
		CupheadLevelCamera.Current.StartShake(4f);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield return base.animator.WaitForAnimationToStart(this, "Roll_Loop", false);
		float t = 0f;
		while (t < p.time)
		{
			float val = t / p.time;
			float x = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startX, endX, val);
			base.transform.SetPosition(new float?(x), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(new float?(endX), null, null);
		yield return null;
		CupheadLevelCamera.Current.EndShake(0.5f);
		AudioManager.Stop("level_frogs_short_rolling_loop");
		AudioManager.Play("level_frogs_short_rolling_crash");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_crash");
		this.spriteRenderer.enabled = false;
		this.direction = FrogsLevelShort.Direction.Right;
		yield return CupheadTime.WaitForSeconds(this, p.returnDelay);
		base.transform.SetScale(new float?(-1f), null, null);
		base.transform.SetPosition(new float?(-(startX + 140f)), null, null);
		base.animator.SetTrigger("OnRollContinue");
		AudioManager.Play("level_frogs_short_rolling_end");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_end");
		this.spriteRenderer.enabled = true;
		yield return CupheadTime.WaitForSeconds(this, 1f + p.hesitate);
		this.state = FrogsLevelShort.State.Complete;
		AudioManager.Stop("level_frogs_short_rolling_loop");
		yield break;
	}

	// Token: 0x06002491 RID: 9361 RVA: 0x0015607F File Offset: 0x0015447F
	public void PlayRollSfx()
	{
		AudioManager.PlayLoop("level_frogs_short_rolling_loop");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_loop");
		AudioManager.Play("level_frogs_short_rolling_start");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_start");
	}

	// Token: 0x06002492 RID: 9362 RVA: 0x001560B5 File Offset: 0x001544B5
	public void StartMorph()
	{
		this.StopAllCoroutines();
		base.animator.Play("Idle");
		this.state = FrogsLevelShort.State.Morphing;
		base.StartCoroutine(this.morphRoll_cr());
	}

	// Token: 0x06002493 RID: 9363 RVA: 0x001560E4 File Offset: 0x001544E4
	private IEnumerator morphRoll_cr()
	{
		Vector2 start = base.transform.position;
		Vector2 end = FrogsLevelTall.Current.shortMorphRoot.position;
		base.animator.SetTrigger("OnRoll");
		yield return base.animator.WaitForAnimationToEnd(this, "Roll", false, true);
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		base.animator.SetTrigger("OnRollContinue");
		CupheadLevelCamera.Current.StartShake(4f);
		AudioManager.PlayLoop("level_frogs_short_rolling_loop");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_loop");
		AudioManager.Play("level_frogs_short_rolling_start");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_start");
		float t = 0f;
		while (t < 1f)
		{
			float val = t / 1f;
			float x = EaseUtils.Ease(EaseUtils.EaseType.linear, start.x, end.x, val);
			float y = EaseUtils.Ease(EaseUtils.EaseType.linear, start.y, end.y, val);
			base.transform.SetPosition(new float?(x), new float?(y), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		AudioManager.Stop("level_frogs_short_rolling_loop");
		base.transform.SetPosition(new float?(end.x), new float?(end.y), null);
		CupheadLevelCamera.Current.EndShake(0.5f);
		yield return null;
		FrogsLevelTall.Current.ContinueMorph();
		this.spriteRenderer.enabled = false;
		base.properties.OnBossDeath -= this.OnBossDeath;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x04002D26 RID: 11558
	[SerializeField]
	private Effect introDust;

	// Token: 0x04002D27 RID: 11559
	[SerializeField]
	private Transform[] rageRoots;

	// Token: 0x04002D28 RID: 11560
	[SerializeField]
	private FrogsLevelShortRageBullet rageFireball;

	// Token: 0x04002D29 RID: 11561
	[SerializeField]
	private Effect rageFireballSpark;

	// Token: 0x04002D2A RID: 11562
	[SerializeField]
	private FrogsLevelShortClapBullet clapBullet;

	// Token: 0x04002D2B RID: 11563
	[SerializeField]
	private Effect clapEffect;

	// Token: 0x04002D2C RID: 11564
	[SerializeField]
	private Transform clapRoot;

	// Token: 0x04002D2F RID: 11567
	private SpriteRenderer spriteRenderer;

	// Token: 0x04002D30 RID: 11568
	private DamageReceiver damageReceiver;

	// Token: 0x04002D31 RID: 11569
	private DamageDealer damageDealer;

	// Token: 0x04002D32 RID: 11570
	private LevelProperties.Frogs.ShortClap clapProperties;

	// Token: 0x04002D33 RID: 11571
	private FrogsLevelShortClapBullet.Direction clapDirection;

	// Token: 0x04002D34 RID: 11572
	private const float MORPH_ROLL_TIME = 1f;

	// Token: 0x020006B9 RID: 1721
	public enum State
	{
		// Token: 0x04002D36 RID: 11574
		Idle,
		// Token: 0x04002D37 RID: 11575
		Rage,
		// Token: 0x04002D38 RID: 11576
		Roll,
		// Token: 0x04002D39 RID: 11577
		Clap,
		// Token: 0x04002D3A RID: 11578
		Morphing,
		// Token: 0x04002D3B RID: 11579
		Complete = 1000,
		// Token: 0x04002D3C RID: 11580
		Morphed
	}

	// Token: 0x020006BA RID: 1722
	public enum Direction
	{
		// Token: 0x04002D3E RID: 11582
		Left,
		// Token: 0x04002D3F RID: 11583
		Right
	}
}
