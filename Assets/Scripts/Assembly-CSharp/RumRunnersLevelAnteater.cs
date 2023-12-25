using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000783 RID: 1923
public class RumRunnersLevelAnteater : LevelProperties.RumRunners.Entity
{
	// Token: 0x06002A42 RID: 10818 RVA: 0x0018B6AE File Offset: 0x00189AAE
	private void OnEnable()
	{
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.onDamageTakenEventHandler;
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x0018B6C7 File Offset: 0x00189AC7
	private void OnDisable()
	{
		base.GetComponent<DamageReceiver>().OnDamageTaken -= this.onDamageTakenEventHandler;
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x0018B6E0 File Offset: 0x00189AE0
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		foreach (CollisionChild collisionChild in this.collChildren)
		{
			collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		}
		this.tongue.OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x0018B742 File Offset: 0x00189B42
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x0018B75A File Offset: 0x00189B5A
	public void DoDamage(float damage)
	{
		base.properties.DealDamage(damage);
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x0018B768 File Offset: 0x00189B68
	private void onDamageTakenEventHandler(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x0018B77B File Offset: 0x00189B7B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x0018B7A4 File Offset: 0x00189BA4
	public override void LevelInit(LevelProperties.RumRunners properties)
	{
		base.LevelInit(properties);
		this.snoutAttackPattern = new PatternString(properties.CurrentState.anteaterSnout.snoutActionArray, true, false);
		this.snoutPositionPattern = new PatternString(properties.CurrentState.anteaterSnout.snoutPosString, true, true);
		this.snout.Setup(properties);
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x0018B7FE File Offset: 0x00189BFE
	public void StartAnteater()
	{
		base.StartCoroutine(this.snout_cr());
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x0018B810 File Offset: 0x00189C10
	private IEnumerator snout_cr()
	{
		LevelProperties.RumRunners.AnteaterSnout p = base.properties.CurrentState.anteaterSnout;
		for (;;)
		{
			RumRunnersLevelAnteater.AttackIntro attackIntro = (!Rand.Bool()) ? RumRunnersLevelAnteater.AttackIntro.B : RumRunnersLevelAnteater.AttackIntro.A;
			base.animator.SetInteger("AttackIntro", (int)attackIntro);
			int count = this.snoutAttackPattern.SubStringLength();
			for (int i = 0; i < count; i++)
			{
				RumRunnersLevelSnout.AttackType attackType;
				RumRunnersLevelAnteater.AttackIntroLength introLength;
				this.parseAttackString(this.snoutAttackPattern.GetString(), out attackType, out introLength);
				this.snoutAttackPattern.IncrementString();
				RumRunnersLevelSnout.AttackType nextAttackType;
				RumRunnersLevelAnteater.AttackIntroLength nextIntroLength;
				this.parseAttackString(this.snoutAttackPattern.GetString(), out nextAttackType, out nextIntroLength);
				this.queuedAttack = attackType;
				if (p.anticipationBoilDelay > 0f)
				{
					yield return CupheadTime.WaitForSeconds(this, p.anticipationBoilDelay);
				}
				base.animator.SetTrigger("AnticipationComplete");
				while (!this.snout.isAttacking)
				{
					yield return null;
				}
				string endAnimationName;
				if (i == 0)
				{
					endAnimationName = "AttackInitialEnd";
				}
				else
				{
					string str = (attackIntro != RumRunnersLevelAnteater.AttackIntro.A) ? "AttackB." : "AttackA.";
					endAnimationName = str + "AttackEnd";
				}
				bool nextAttackIsFinal = nextAttackType == RumRunnersLevelSnout.AttackType.Tongue || i == count - 1;
				if (!nextAttackIsFinal)
				{
					attackIntro = ((attackIntro != RumRunnersLevelAnteater.AttackIntro.A) ? RumRunnersLevelAnteater.AttackIntro.A : RumRunnersLevelAnteater.AttackIntro.B);
				}
				else
				{
					if (i < count - 1 && nextAttackType == RumRunnersLevelSnout.AttackType.Tongue)
					{
						this.snoutAttackPattern.IncrementString();
					}
					attackIntro = RumRunnersLevelAnteater.AttackIntro.Final;
				}
				base.animator.SetInteger("AttackIntro", (int)attackIntro);
				base.animator.SetBool("LongIntro", nextIntroLength == RumRunnersLevelAnteater.AttackIntroLength.Long);
				while (this.snout.isAttacking)
				{
					yield return null;
				}
				base.animator.SetTrigger("EndAttack");
				yield return base.animator.WaitForAnimationToEnd(this, endAnimationName, false, true);
				if (nextAttackIsFinal)
				{
					break;
				}
			}
			this.queuedAttack = RumRunnersLevelSnout.AttackType.Tongue;
			if (p.anticipationBoilDelay > 0f)
			{
				yield return CupheadTime.WaitForSeconds(this, p.anticipationBoilDelay);
			}
			base.animator.SetTrigger("AnticipationComplete");
			base.StartCoroutine(this.finalAttackEyes_cr());
			while (!this.snout.isAttacking)
			{
				yield return null;
			}
			while (this.snout.isAttacking)
			{
				yield return null;
			}
			base.animator.SetTrigger("EndAttack");
			base.animator.Play("Off", RumRunnersLevelAnteater.EyesAnimationLayer, 0f);
			base.animator.Update(0f);
			yield return base.animator.WaitForAnimationToStart(this, "AttackFinal.AttackEndHold", false);
			yield return CupheadTime.WaitForSeconds(this, p.finalAttackTauntDuration);
			base.animator.SetTrigger("EndTaunt");
			yield return base.animator.WaitForAnimationToEnd(this, "AttackFinal.AttackEnd", false, true);
		}
		yield break;
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x0018B82C File Offset: 0x00189C2C
	private IEnumerator finalAttackEyes_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "AttackFinal.AttackStart", false, true);
		base.animator.Play("Hold", RumRunnersLevelAnteater.EyesAnimationLayer, 0f);
		base.animator.Update(0f);
		yield break;
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x0018B848 File Offset: 0x00189C48
	public void FakeDeathStart()
	{
		this.StopAllCoroutines();
		LevelProperties.RumRunners.Boss boss = base.properties.CurrentState.boss;
		this.mobBoss.Setup(base.properties, this, this.mobBossHelperTransform);
		foreach (SpriteRenderer spriteRenderer in this.flipRenderers)
		{
			spriteRenderer.flipX = false;
		}
		this.snout.Death();
		base.animator.Play("Off", RumRunnersLevelAnteater.EyesAnimationLayer);
		base.animator.Play("Off", RumRunnersLevelAnteater.HandsAnimatorLayer);
		base.animator.Play("Death");
		base.animator.Update(0f);
		base.GetComponent<AnimationHelper>().Speed = 0f;
		this.eyes.transform.localPosition = new Vector3(2f, -62f);
		base.GetComponent<LevelBossDeathExploder>().StartExplosion(true);
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x0018B93A File Offset: 0x00189D3A
	public void FakeDeathContinue()
	{
		base.GetComponent<AnimationHelper>().Speed = 1f;
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x0018B94C File Offset: 0x00189D4C
	private IEnumerator fakeDeathEyes_cr()
	{
		PatternString eyesPattern = new PatternString(base.properties.CurrentState.boss.anteaterEyeClosedOpenString, true);
		for (;;)
		{
			string[] currentPattern = eyesPattern.PopString().Split(new char[]
			{
				':'
			});
			if (currentPattern.Length != 2)
			{
				break;
			}
			int closedCount;
			Parser.IntTryParse(currentPattern[0], out closedCount);
			int openCount;
			Parser.IntTryParse(currentPattern[1], out openCount);
			yield return base.StartCoroutine(this.eyeHandler_cr(closedCount, "DeathLoop"));
			yield return base.animator.WaitForAnimationToStart(this, "DeathLoopEyeOpen", false);
			yield return base.StartCoroutine(this.eyeHandler_cr(openCount, "DeathLoopOpen"));
			yield return base.animator.WaitForAnimationToStart(this, "DeathLoopEyeClose", false);
		}
		throw new Exception("Invalid anteater eye pattern");
		yield break;
	}

	// Token: 0x06002A50 RID: 10832 RVA: 0x0018B968 File Offset: 0x00189D68
	private IEnumerator eyeHandler_cr(int count, string loopAnimationName)
	{
		if (count == 0)
		{
			base.animator.SetTrigger("DeathLoopEyeChange");
		}
		else
		{
			yield return base.animator.WaitForAnimationToStart(this, loopAnimationName, false);
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < (float)count - 0.5f)
			{
				yield return null;
			}
			base.animator.SetTrigger("DeathLoopEyeChange");
		}
		yield break;
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x0018B991 File Offset: 0x00189D91
	public void RealDeath()
	{
		this.StopAllCoroutines();
		base.animator.Play("ActualDeath", 0, 0.2631579f);
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x0018B9B0 File Offset: 0x00189DB0
	private void animationEvent_LeftDirt()
	{
		CupheadLevelCamera.Current.Shake(20f, 0.3f, true);
		((RumRunnersLevel)Level.Current).FullscreenDirt(1, new float?(-640f + UnityEngine.Random.Range(100f, 200f)), -1f, -1f);
	}

	// Token: 0x06002A53 RID: 10835 RVA: 0x0018BA08 File Offset: 0x00189E08
	private void animationEvent_RightDirt()
	{
		CupheadLevelCamera.Current.Shake(20f, 0.3f, true);
		((RumRunnersLevel)Level.Current).FullscreenDirt(1, new float?(640f - UnityEngine.Random.Range(100f, 200f)), -1f, -1f);
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x0018BA5E File Offset: 0x00189E5E
	private void animationEvent_MiddleBridgeDestroy()
	{
		((RumRunnersLevel)Level.Current).DestroyMiddleBridge();
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x0018BA6F File Offset: 0x00189E6F
	private void animationEvent_UpperBridgeDestroy()
	{
		((RumRunnersLevel)Level.Current).DestroyUpperBridge();
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x0018BA80 File Offset: 0x00189E80
	private void animationEvent_BridgeShatter()
	{
		((RumRunnersLevel)Level.Current).ShatterBridges();
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x0018BA94 File Offset: 0x00189E94
	private void animationEvent_InitialAttackStarted()
	{
		if (this.firstAttack)
		{
			this.firstAttack = false;
			return;
		}
		this.onLeft = !this.onLeft;
		foreach (SpriteRenderer spriteRenderer in this.flipRenderers)
		{
			spriteRenderer.flipX = !this.onLeft;
		}
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x0018BAF4 File Offset: 0x00189EF4
	private void animationEvent_SnoutAttack()
	{
		int num = this.snoutPositionPattern.PopInt();
		Vector2 v;
		v.x = ((!this.onLeft) ? RumRunnersLevelAnteater.SNOUT_SPAWN_X : (-RumRunnersLevelAnteater.SNOUT_SPAWN_X));
		v.y = this.spawnPoints[num].position.y;
		this.snout.Attack(v, this.snoutShadowPositions[num], this.onLeft, this.queuedAttack);
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x0018BB7C File Offset: 0x00189F7C
	private void animationEvent_HandsUp()
	{
		int num;
		if (this.onLeft)
		{
			num = RumRunnersLevelAnteater.HandsUpAnimatorHash;
		}
		else
		{
			num = RumRunnersLevelAnteater.HandsDownAnimatorHash;
		}
		AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(RumRunnersLevelAnteater.HandsAnimatorLayer);
		if (currentAnimatorStateInfo.shortNameHash != num || currentAnimatorStateInfo.normalizedTime >= 1f)
		{
			base.animator.Play(num, RumRunnersLevelAnteater.HandsAnimatorLayer, 0f);
		}
		base.animator.Update(0f);
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x0018BBFC File Offset: 0x00189FFC
	private void animationEvent_HandsDown()
	{
		int num;
		if (this.onLeft)
		{
			num = RumRunnersLevelAnteater.HandsDownAnimatorHash;
		}
		else
		{
			num = RumRunnersLevelAnteater.HandsUpAnimatorHash;
		}
		AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(RumRunnersLevelAnteater.HandsAnimatorLayer);
		if (currentAnimatorStateInfo.shortNameHash != num || currentAnimatorStateInfo.normalizedTime >= 1f)
		{
			base.animator.Play(num, RumRunnersLevelAnteater.HandsAnimatorLayer, 0f);
		}
		base.animator.Update(0f);
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x0018BC7C File Offset: 0x0018A07C
	private void animationEvent_HandsUpHalfway()
	{
		int stateNameHash;
		if (this.onLeft)
		{
			stateNameHash = RumRunnersLevelAnteater.HandsUpAnimatorHash;
		}
		else
		{
			stateNameHash = RumRunnersLevelAnteater.HandsDownAnimatorHash;
		}
		AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(RumRunnersLevelAnteater.HandsAnimatorLayer);
		base.animator.Play(stateNameHash, RumRunnersLevelAnteater.HandsAnimatorLayer, 0.5f);
		base.animator.Update(0f);
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x0018BCDC File Offset: 0x0018A0DC
	private void animationEvent_HandsStartTaunt()
	{
		if (this.onLeft)
		{
			base.animator.Play("HandsTaunt", 2, 0f);
		}
		else
		{
			base.animator.Play("HandsTauntRight", 2, 0f);
		}
		base.animator.Update(0f);
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x0018BD35 File Offset: 0x0018A135
	private void animationEvent_HandsEndTaunt()
	{
		base.animator.SetTrigger("HandsEndTaunt");
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x0018BD47 File Offset: 0x0018A147
	private void animationEvent_FalseDeathDust()
	{
		base.animator.Play("DeathDust", RumRunnersLevelAnteater.DeathDustAnimatorLayer);
		CupheadLevelCamera.Current.Shake(35f, 0.5f, false);
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x0018BD73 File Offset: 0x0018A173
	private void animationEvent_FalseDeathEnded()
	{
		this.mobBoss.Begin();
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		base.StartCoroutine(this.fakeDeathEyes_cr());
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x0018BD98 File Offset: 0x0018A198
	private void parseAttackString(string attackString, out RumRunnersLevelSnout.AttackType attackType, out RumRunnersLevelAnteater.AttackIntroLength introLength)
	{
		char c;
		char c2;
		if (attackString.Length == 2)
		{
			c = attackString[1];
			c2 = attackString[0];
		}
		else
		{
			c = attackString[0];
			c2 = '0';
		}
		if (c == 'Q')
		{
			attackType = RumRunnersLevelSnout.AttackType.Quick;
		}
		else if (c == 'F')
		{
			attackType = RumRunnersLevelSnout.AttackType.Fake;
		}
		else
		{
			if (c != 'T')
			{
				throw new Exception("Invalid attack string: " + attackString);
			}
			attackType = RumRunnersLevelSnout.AttackType.Tongue;
		}
		if (c2 == 'L')
		{
			introLength = RumRunnersLevelAnteater.AttackIntroLength.Long;
		}
		else
		{
			introLength = RumRunnersLevelAnteater.AttackIntroLength.Standard;
		}
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x0018BE28 File Offset: 0x0018A228
	public void TriggerEyesTurnaround()
	{
		base.animator.SetTrigger((this.snout.transform.position.y >= 150f) ? "EyesLookUp" : "EyesLookDown");
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x0018BE74 File Offset: 0x0018A274
	public void SetEyeSide(bool onLeft)
	{
		this.eyes.transform.localPosition = new Vector3((!onLeft) ? this.eyePositionAttack.x : (-this.eyePositionAttack.x), this.eyePositionAttack.y);
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x0018BEC3 File Offset: 0x0018A2C3
	private void AnimationEvent_SFX_RUMRUN_P3_Anteater_Intro()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_intro");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_intro");
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x0018BEDF File Offset: 0x0018A2DF
	private void AnimationEvent_SFX_RUMRUN_P3_Anteater_HandSlamFirst()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_handslamfirst");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_handslamfirst");
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x0018BEFB File Offset: 0x0018A2FB
	private void AnimationEvent_SFX_RUMRUN_P3_Anteater_HandSlamSecond()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_handslamsecond");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_handslamsecond");
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x0018BF17 File Offset: 0x0018A317
	private void AnimationEvent_SFX_RUMRUN_P3_Anteater_Intro_Hat_Off()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_intro_hat_off");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_intro_hat_off");
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x0018BF33 File Offset: 0x0018A333
	private void AnimationEvent_SFX_RUMRUN_P3_Anteater_Intro_Hatton()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_intro_hatton");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_intro_hatton");
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x0018BF4F File Offset: 0x0018A34F
	private void AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_Start()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_initial_start");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_initial_start");
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x0018BF6B File Offset: 0x0018A36B
	private void AnimationEvent_SFX_RUMRUN_P4_IntroSnailLaugh()
	{
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_laughing");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_vx_fakeannouncer_laughing");
	}

	// Token: 0x04003319 RID: 13081
	private static readonly float SNOUT_SPAWN_X = 0f;

	// Token: 0x0400331A RID: 13082
	private static readonly int EyesAnimationLayer = 1;

	// Token: 0x0400331B RID: 13083
	private static readonly int HandsAnimatorLayer = 2;

	// Token: 0x0400331C RID: 13084
	private static readonly int DeathDustAnimatorLayer = 3;

	// Token: 0x0400331D RID: 13085
	private static readonly int HandsUpAnimatorHash = Animator.StringToHash("HandsUp");

	// Token: 0x0400331E RID: 13086
	private static readonly int HandsDownAnimatorHash = Animator.StringToHash("HandsDown");

	// Token: 0x0400331F RID: 13087
	private Vector2 eyePositionAttack = new Vector2(348f, 145f);

	// Token: 0x04003320 RID: 13088
	[SerializeField]
	private Transform[] spawnPoints;

	// Token: 0x04003321 RID: 13089
	[SerializeField]
	private Vector2[] snoutShadowPositions;

	// Token: 0x04003322 RID: 13090
	[SerializeField]
	private RumRunnersLevelSnout snout;

	// Token: 0x04003323 RID: 13091
	[SerializeField]
	private RumRunnersLevelMobBoss mobBoss;

	// Token: 0x04003324 RID: 13092
	[SerializeField]
	private Transform mobBossHelperTransform;

	// Token: 0x04003325 RID: 13093
	[SerializeField]
	private CollisionChild[] collChildren;

	// Token: 0x04003326 RID: 13094
	[SerializeField]
	private RumRunnersLevelSnoutTongue tongue;

	// Token: 0x04003327 RID: 13095
	[SerializeField]
	private SpriteRenderer[] flipRenderers;

	// Token: 0x04003328 RID: 13096
	[SerializeField]
	private float blinkProbability;

	// Token: 0x04003329 RID: 13097
	[SerializeField]
	private GameObject eyes;

	// Token: 0x0400332A RID: 13098
	private DamageDealer damageDealer;

	// Token: 0x0400332B RID: 13099
	private PatternString snoutAttackPattern;

	// Token: 0x0400332C RID: 13100
	private PatternString snoutPositionPattern;

	// Token: 0x0400332D RID: 13101
	private bool onLeft = true;

	// Token: 0x0400332E RID: 13102
	private bool firstAttack = true;

	// Token: 0x0400332F RID: 13103
	private RumRunnersLevelSnout.AttackType queuedAttack;

	// Token: 0x02000784 RID: 1924
	private enum AttackIntro
	{
		// Token: 0x04003331 RID: 13105
		Initial = -1,
		// Token: 0x04003332 RID: 13106
		A,
		// Token: 0x04003333 RID: 13107
		B,
		// Token: 0x04003334 RID: 13108
		Final
	}

	// Token: 0x02000785 RID: 1925
	private enum AttackIntroLength
	{
		// Token: 0x04003336 RID: 13110
		Standard,
		// Token: 0x04003337 RID: 13111
		Long
	}
}
