using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006BE RID: 1726
public class FrogsLevelTall : LevelProperties.Frogs.Entity
{
	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x0600249D RID: 9373 RVA: 0x001574EA File Offset: 0x001558EA
	// (set) Token: 0x0600249E RID: 9374 RVA: 0x001574F2 File Offset: 0x001558F2
	public FrogsLevelTall.State state { get; private set; }

	// Token: 0x0600249F RID: 9375 RVA: 0x001574FC File Offset: 0x001558FC
	protected override void Awake()
	{
		base.Awake();
		FrogsLevelTall.Current = this;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 0.3f, DamageDealer.DamageSource.Enemy, true, false, false);
	}

	// Token: 0x060024A0 RID: 9376 RVA: 0x00157551 File Offset: 0x00155951
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (FrogsLevelTall.Current == this)
		{
			FrogsLevelTall.Current = null;
		}
		this.fireflyPrefab = null;
	}

	// Token: 0x060024A1 RID: 9377 RVA: 0x00157576 File Offset: 0x00155976
	private void Start()
	{
		Level.Current.OnIntroEvent += this.OnLevelIntro;
	}

	// Token: 0x060024A2 RID: 9378 RVA: 0x0015758E File Offset: 0x0015598E
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x060024A3 RID: 9379 RVA: 0x0015759B File Offset: 0x0015599B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060024A4 RID: 9380 RVA: 0x001575B9 File Offset: 0x001559B9
	public override void LevelInit(LevelProperties.Frogs properties)
	{
		base.LevelInit(properties);
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x060024A5 RID: 9381 RVA: 0x001575D4 File Offset: 0x001559D4
	public void AddFanForce(AbstractPlayerController player)
	{
		if (this.fanForce == null)
		{
			this.fanForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, 0f);
			this.fanForce.enabled = false;
		}
		player.GetComponent<LevelPlayerMotor>().AddForce(this.fanForce);
	}

	// Token: 0x060024A6 RID: 9382 RVA: 0x0015760F File Offset: 0x00155A0F
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (FrogsLevel.FINAL_FORM)
		{
			return;
		}
		if (base.properties.CurrentState.stateName != LevelProperties.Frogs.States.Main)
		{
			base.properties.DealDamage(info.damage);
		}
	}

	// Token: 0x060024A7 RID: 9383 RVA: 0x00157642 File Offset: 0x00155A42
	private void OnBossDeath()
	{
		this.StopAllCoroutines();
		this.fanForce.enabled = false;
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("level_frogs_tall_death");
	}

	// Token: 0x060024A8 RID: 9384 RVA: 0x00157670 File Offset: 0x00155A70
	private void OnLevelIntro()
	{
		AudioManager.Play("level_frogs_tall_intro_full");
		base.animator.Play("Intro");
	}

	// Token: 0x060024A9 RID: 9385 RVA: 0x0015768C File Offset: 0x00155A8C
	public void StartFan()
	{
		if (this.state != FrogsLevelTall.State.Idle && this.state != FrogsLevelTall.State.Complete)
		{
			return;
		}
		this.state = FrogsLevelTall.State.Fan;
		base.StartCoroutine(this.fan_cr());
	}

	// Token: 0x060024AA RID: 9386 RVA: 0x001576C0 File Offset: 0x00155AC0
	private IEnumerator fan_cr()
	{
		LevelProperties.Frogs.TallFan p = base.properties.CurrentState.tallFan;
		float time = p.duration;
		base.animator.Play("Fan");
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("level_frogs_tall_fan_start");
		this.emitAudioFromObject.Add("level_frogs_tall_fan_start");
		base.StartCoroutine(this.fanAccelerate_cr(p));
		yield return CupheadTime.WaitForSeconds(this, 2f);
		AudioManager.PlayLoop("level_frogs_tall_fan_attack_loop");
		this.emitAudioFromObject.Add("level_frogs_tall_fan_attack_loop");
		if (this.firstFan)
		{
			this.firstFan = false;
			float startX = base.transform.position.x;
			yield return CupheadTime.WaitForSeconds(this, 0.25f);
			float t = 0f;
			while (t < 0.5f)
			{
				float val = t / 0.5f;
				float x = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startX, startX + 60f, val);
				base.transform.SetPosition(new float?(x), null, null);
				t += CupheadTime.Delta;
				yield return null;
			}
			base.transform.SetPosition(new float?(startX + 60f), null, null);
			yield return CupheadTime.WaitForSeconds(this, p.duration.RandomFloat() - 0.75f);
		}
		else
		{
			yield return CupheadTime.WaitForSeconds(this, p.duration.RandomFloat());
		}
		yield return CupheadTime.WaitForSeconds(this, time);
		AudioManager.Play("level_frogs_tall_fan_end");
		this.emitAudioFromObject.Add("level_frogs_tall_fan_end");
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AudioManager.Stop("level_frogs_tall_fan_attack_loop");
		base.animator.SetTrigger("OnFanEnd");
		base.StartCoroutine(this.fanDecelerate_cr(p));
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.state = FrogsLevelTall.State.Complete;
		yield break;
	}

	// Token: 0x060024AB RID: 9387 RVA: 0x001576DC File Offset: 0x00155ADC
	private IEnumerator fanAccelerate_cr(LevelProperties.Frogs.TallFan p)
	{
		this.fanForce.enabled = true;
		yield return base.StartCoroutine(this.fanPowerTween_cr(0f, p.power, (float)p.accelerationTime));
		yield break;
	}

	// Token: 0x060024AC RID: 9388 RVA: 0x00157700 File Offset: 0x00155B00
	private IEnumerator fanDecelerate_cr(LevelProperties.Frogs.TallFan p)
	{
		yield return base.StartCoroutine(this.fanPowerTween_cr(p.power, 0f, 0.75f));
		this.fanForce.enabled = false;
		yield break;
	}

	// Token: 0x060024AD RID: 9389 RVA: 0x00157724 File Offset: 0x00155B24
	private IEnumerator fanPowerTween_cr(float start, float end, float time)
	{
		this.fanForce.value = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.fanForce.value = Mathf.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.fanForce.value = end;
		yield break;
	}

	// Token: 0x060024AE RID: 9390 RVA: 0x00157754 File Offset: 0x00155B54
	public void StartFireflies()
	{
		this.layer = 0;
		if (this.state != FrogsLevelTall.State.Idle && this.state != FrogsLevelTall.State.Complete)
		{
			return;
		}
		this.state = FrogsLevelTall.State.Fireflies;
		this.fireflyCount = 0;
		base.animator.SetBool("EndFirefly", false);
		base.StartCoroutine(this.fireflies_cr());
	}

	// Token: 0x060024AF RID: 9391 RVA: 0x001577B0 File Offset: 0x00155BB0
	private void ResetFireflyRoots()
	{
		this.tempRoots = new List<FrogsLevelTallFireflyRoot>(this.fireflyRoots);
	}

	// Token: 0x060024B0 RID: 9392 RVA: 0x001577C4 File Offset: 0x00155BC4
	private void ShootFirefly()
	{
		AudioManager.Play("level_frogs_tall_spit_shoot");
		this.emitAudioFromObject.Add("level_frogs_tall_spit_shoot");
		FrogsLevelTallFireflyRoot frogsLevelTallFireflyRoot = this.tempRoots[UnityEngine.Random.Range(0, this.tempRoots.Count)];
		this.tempRoots.Remove(frogsLevelTallFireflyRoot);
		Vector2 vector = frogsLevelTallFireflyRoot.transform.position;
		Vector2 a = vector;
		Vector2 vector2 = new Vector2(UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1), UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1));
		vector = a + vector2.normalized * frogsLevelTallFireflyRoot.radius * UnityEngine.Random.value;
		this.fireflyPrefab.Create(this.spitRoot.position, vector, this.fireflyProperties.speed, this.fireflyProperties.hp, this.fireflyProperties.followDelay, this.fireflyProperties.followTime, this.fireflyProperties.followDistance, this.fireflyProperties.invincibleDuration, PlayerManager.GetNext(), this.layer++);
		this.fireflyCount--;
	}

	// Token: 0x060024B1 RID: 9393 RVA: 0x00157904 File Offset: 0x00155D04
	private IEnumerator fireflies_cr()
	{
		this.fireflyProperties = base.properties.CurrentState.tallFireflies;
		string patternString = this.fireflyProperties.patterns[UnityEngine.Random.Range(0, this.fireflyProperties.patterns.Length)];
		KeyValue[] pattern = KeyValue.ListFromString(patternString, new char[]
		{
			'S',
			'D'
		});
		base.animator.SetTrigger("OnFirefly");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].key == "S")
			{
				this.ResetFireflyRoots();
				this.fireflyCount = (int)pattern[i].value;
				base.animator.SetInteger("FireflyCount", this.fireflyCount);
				base.animator.SetTrigger("OnFireflyStart");
				base.animator.SetBool("EndFirefly", i >= pattern.Length - 1);
				while (this.fireflyCount > 0)
				{
					base.animator.SetInteger("FireflyCount", this.fireflyCount);
					yield return null;
				}
				base.animator.SetInteger("FireflyCount", this.fireflyCount);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
		}
		yield return CupheadTime.WaitForSeconds(this, this.fireflyProperties.hesitate);
		this.state = FrogsLevelTall.State.Complete;
		yield break;
	}

	// Token: 0x060024B2 RID: 9394 RVA: 0x0015791F File Offset: 0x00155D1F
	private void MorphSFX()
	{
		AudioManager.Play("level_frogs_tall_morph_end");
		this.emitAudioFromObject.Add("level_frogs_tall_morph_end");
	}

	// Token: 0x060024B3 RID: 9395 RVA: 0x0015793B File Offset: 0x00155D3B
	public void StartMorph()
	{
		this.StopAllCoroutines();
		this.fanForce.value = 0f;
		this.fanForce.enabled = false;
		this.state = FrogsLevelTall.State.Morphing;
		base.animator.Play("Morph");
	}

	// Token: 0x060024B4 RID: 9396 RVA: 0x00157976 File Offset: 0x00155D76
	public void ContinueMorph()
	{
		base.StartCoroutine(this.morph_cr());
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x00157988 File Offset: 0x00155D88
	private IEnumerator morph_cr()
	{
		base.animator.SetTrigger("OnMorphContinue");
		Vector2 start = base.transform.position;
		Vector2 end = new Vector2(631f, -314f);
		float t = 0f;
		while (t < 1f)
		{
			float val = t / 1f;
			float x = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.x, end.x, val);
			float y = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.y, end.y, val);
			base.transform.SetPosition(new float?(x), new float?(y), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = end;
		this.state = FrogsLevelTall.State.Morphed;
		yield break;
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x001579A4 File Offset: 0x00155DA4
	private void OnMorphAnimationComplete()
	{
		FrogsLevelMorphed.Current.Enable(FrogsLevel.DEMON_TRIGGERED);
		CupheadLevelCamera.Current.Shake(20f, 0.6f, false);
		base.properties.OnBossDeath -= this.OnBossDeath;
		base.gameObject.SetActive(false);
	}

	// Token: 0x04002D48 RID: 11592
	public static FrogsLevelTall Current;

	// Token: 0x04002D49 RID: 11593
	[SerializeField]
	private FrogsLevelTallFirefly fireflyPrefab;

	// Token: 0x04002D4A RID: 11594
	[SerializeField]
	private FrogsLevelTallFireflyRoot[] fireflyRoots;

	// Token: 0x04002D4B RID: 11595
	[SerializeField]
	private Transform spitRoot;

	// Token: 0x04002D4C RID: 11596
	[Space(10f)]
	public Transform shortMorphRoot;

	// Token: 0x04002D4E RID: 11598
	private LevelPlayerMotor.VelocityManager.Force fanForce;

	// Token: 0x04002D4F RID: 11599
	private DamageReceiver damageReceiver;

	// Token: 0x04002D50 RID: 11600
	private DamageDealer damageDealer;

	// Token: 0x04002D51 RID: 11601
	private int layer;

	// Token: 0x04002D52 RID: 11602
	private const float FAN_START_TIME = 2f;

	// Token: 0x04002D53 RID: 11603
	private const float FAN_END_TIME = 0.5f;

	// Token: 0x04002D54 RID: 11604
	private const float FIRST_FAN_MOVE_OFFSET = 60f;

	// Token: 0x04002D55 RID: 11605
	private const float FAN_DECELERATE_TIME = 0.75f;

	// Token: 0x04002D56 RID: 11606
	private bool firstFan = true;

	// Token: 0x04002D57 RID: 11607
	private int fireflyCount;

	// Token: 0x04002D58 RID: 11608
	private List<FrogsLevelTallFireflyRoot> tempRoots;

	// Token: 0x04002D59 RID: 11609
	private LevelProperties.Frogs.TallFireflies fireflyProperties;

	// Token: 0x04002D5A RID: 11610
	private const float MORPH_MOVE_TIME = 1f;

	// Token: 0x020006BF RID: 1727
	public enum State
	{
		// Token: 0x04002D5C RID: 11612
		Idle,
		// Token: 0x04002D5D RID: 11613
		Fan,
		// Token: 0x04002D5E RID: 11614
		Fireflies,
		// Token: 0x04002D5F RID: 11615
		Morphing,
		// Token: 0x04002D60 RID: 11616
		Complete = 1000000,
		// Token: 0x04002D61 RID: 11617
		Morphed
	}
}
