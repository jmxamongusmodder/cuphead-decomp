using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020007D4 RID: 2004
public class SaltbakerLevelSaltbaker : LevelProperties.Saltbaker.Entity
{
	// Token: 0x1400004C RID: 76
	// (add) Token: 0x06002D86 RID: 11654 RVA: 0x001ADC10 File Offset: 0x001AC010
	// (remove) Token: 0x06002D87 RID: 11655 RVA: 0x001ADC48 File Offset: 0x001AC048
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x06002D88 RID: 11656 RVA: 0x001ADC80 File Offset: 0x001AC080
	private void Start()
	{
		this.scale = base.transform.localScale.x;
		this.startPos = base.transform.position;
		this.damageReceiver = this.phaseOneCollider.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.doughFXAnimator.transform.parent = null;
		base.animator.SetBool("IntroCubes", Rand.Bool());
	}

	// Token: 0x06002D89 RID: 11657 RVA: 0x001ADD08 File Offset: 0x001AC108
	public override void LevelInit(LevelProperties.Saltbaker properties)
	{
		base.LevelInit(properties);
		this.strawberriesSpawnString = new PatternString(properties.CurrentState.strawberries.locationSpawnString, true, true);
		this.strawberriesDelayString = new PatternString(properties.CurrentState.strawberries.bulletDelayString, true, true);
		this.sugarcubesPhaseString = new PatternString(properties.CurrentState.sugarcubes.phaseString, true, true);
		this.sugarcubesDelayString = new PatternString(properties.CurrentState.sugarcubes.bulletDelayString, true, true);
		this.sugarcubesParryString = new PatternString(properties.CurrentState.sugarcubes.parryString, true);
		this.doughSpawnSidePatternString = new PatternString(properties.CurrentState.dough.doughSpawnSideString, true, true);
		this.doughSpawnDelayString = new PatternString(properties.CurrentState.dough.doughDelayString, true, true);
		this.doughSpawnTypeString = new PatternString(properties.CurrentState.dough.doughSpawnTypeString, true, true);
		this.limeHeightString = new PatternString(properties.CurrentState.limes.boomerangHeightString, true, true);
		this.limesDelayString = new PatternString(properties.CurrentState.limes.boomerangDelayString, true, true);
		this.timeToNextAttack[0] = properties.CurrentState.strawberries.startNextAtk;
		this.timeToNextAttack[1] = properties.CurrentState.sugarcubes.startNextAttack;
		this.timeToNextAttack[2] = properties.CurrentState.dough.startNextAttack;
		this.timeToNextAttack[3] = properties.CurrentState.limes.startNextAttack;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002D8A RID: 11658 RVA: 0x001ADEAC File Offset: 0x001AC2AC
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		((SaltbakerLevel)Level.Current).SpawnSwoopers();
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.currentAttack = ((!base.animator.GetBool("IntroCubes")) ? SaltbakerLevelSaltbaker.State.Limes : SaltbakerLevelSaltbaker.State.Sugarcubes);
		this.prevAttack = this.currentAttack;
		this.AniEvent_StartProjectiles();
		this.attackCoroutines.Add(base.StartCoroutine(this.pattern_cr()));
		yield break;
	}

	// Token: 0x06002D8B RID: 11659 RVA: 0x001ADEC7 File Offset: 0x001AC2C7
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002D8C RID: 11660 RVA: 0x001ADEDC File Offset: 0x001AC2DC
	private IEnumerator pattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		do
		{
			this.currentAttack = (SaltbakerLevelSaltbaker.State)(base.properties.CurrentState.NextPattern + 1);
		}
		while (this.currentAttack == SaltbakerLevelSaltbaker.State.Strawberries || this.currentAttack == this.prevAttack);
		while (!this.phaseOneEnded)
		{
			SaltbakerLevelSaltbaker.State state = this.currentAttack;
			if (state != SaltbakerLevelSaltbaker.State.Sugarcubes)
			{
				if (state != SaltbakerLevelSaltbaker.State.Dough)
				{
					if (state == SaltbakerLevelSaltbaker.State.Limes)
					{
						base.animator.Play("Limes");
					}
				}
				else
				{
					base.animator.Play("Dough");
				}
			}
			else
			{
				base.animator.Play("Sugarcubes");
				this.sugarTextReversed.enabled = (base.transform.localScale.x == -1f);
			}
			this.prevAttack = this.currentAttack;
			while (this.currentAttack != SaltbakerLevelSaltbaker.State.Idle)
			{
				yield return null;
			}
			if (!this.phaseOneEnded)
			{
				this.currentAttack = (SaltbakerLevelSaltbaker.State)(base.properties.CurrentState.NextPattern + 1);
				base.animator.SetBool("NextStrawberries", this.currentAttack == SaltbakerLevelSaltbaker.State.Strawberries);
				float timeToIdle = this.timeToNextAttack[this.prevAttack - SaltbakerLevelSaltbaker.State.Strawberries] - this.postAttackTime[this.prevAttack - SaltbakerLevelSaltbaker.State.Strawberries] - this.preAttackTime[this.currentAttack - SaltbakerLevelSaltbaker.State.Strawberries];
				yield return CupheadTime.WaitForSeconds(this, timeToIdle);
			}
			yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		}
		yield break;
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x001ADEF8 File Offset: 0x001AC2F8
	private void AniEvent_StartProjectiles()
	{
		if (this.phaseOneEnded)
		{
			return;
		}
		switch (this.currentAttack)
		{
		case SaltbakerLevelSaltbaker.State.Strawberries:
			this.attackCoroutines.Add(base.StartCoroutine(this.strawberries_cr()));
			break;
		case SaltbakerLevelSaltbaker.State.Sugarcubes:
			this.attackCoroutines.Add(base.StartCoroutine(this.sugarcubes_cr()));
			break;
		case SaltbakerLevelSaltbaker.State.Dough:
			this.attackCoroutines.Add(base.StartCoroutine(this.dough_cr()));
			break;
		case SaltbakerLevelSaltbaker.State.Limes:
			this.attackCoroutines.Add(base.StartCoroutine(this.limes_cr()));
			break;
		}
		if (this.currentAttack == SaltbakerLevelSaltbaker.State.Strawberries)
		{
			this.prevAttack = SaltbakerLevelSaltbaker.State.Strawberries;
			if (!this.phaseOneEnded)
			{
				this.currentAttack = (SaltbakerLevelSaltbaker.State)(base.properties.CurrentState.NextPattern + 1);
			}
		}
		else
		{
			this.currentAttack = SaltbakerLevelSaltbaker.State.Idle;
		}
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x001ADFE8 File Offset: 0x001AC3E8
	private void AniEvent_FinishMove()
	{
		base.transform.localScale = new Vector3(-base.transform.localScale.x, base.transform.localScale.y);
		this.onLeft = !this.onLeft;
	}

	// Token: 0x06002D8F RID: 11663 RVA: 0x001AE03C File Offset: 0x001AC43C
	private IEnumerator strawberries_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		LevelProperties.Saltbaker.Strawberries p = base.properties.CurrentState.strawberries;
		float delay = p.firstDelay;
		float attackTime = 0f;
		float delayTime = 0f;
		int anim = UnityEngine.Random.Range(0, 4);
		while (attackTime <= p.diagAtkDuration)
		{
			attackTime += CupheadTime.FixedDelta;
			delayTime += CupheadTime.FixedDelta;
			if (delayTime > delay)
			{
				delayTime -= delay;
				delay = this.strawberriesDelayString.PopFloat();
				this.destroyOnPhaseEnd.Add(this.strawberryPrefab.Create(new Vector3(this.strawberriesSpawnString.PopFloat(), (float)Level.Current.Ceiling + 100f), p.diagAngle + 90f, p.bulletSpeed, anim).gameObject);
				anim = (anim + 1) % 4;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002D90 RID: 11664 RVA: 0x001AE057 File Offset: 0x001AC457
	private void AniEvent_StrawberryBasketStart()
	{
		this.strawberryBasket.StartRunIn(this.onLeft);
	}

	// Token: 0x06002D91 RID: 11665 RVA: 0x001AE06A File Offset: 0x001AC46A
	private void AniEvent_StrawberryBasketGrab()
	{
		this.strawberryBasket.GetGrabbed();
	}

	// Token: 0x06002D92 RID: 11666 RVA: 0x001AE077 File Offset: 0x001AC477
	private void AniEvent_StrawberryBasketExit()
	{
		this.strawberryBasket.StartRunOut();
	}

	// Token: 0x06002D93 RID: 11667 RVA: 0x001AE084 File Offset: 0x001AC484
	private IEnumerator sugarcubes_cr()
	{
		LevelProperties.Saltbaker.Sugarcubes p = base.properties.CurrentState.sugarcubes;
		bool side = this.onLeft;
		float delay = p.firstDelay;
		float phase = this.sugarcubesPhaseString.PopFloat();
		float delayTime = 0f;
		float attackTime = 0f;
		int anim = UnityEngine.Random.Range(0, 3);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (attackTime <= p.sineAttackDuration)
		{
			attackTime += CupheadTime.FixedDelta;
			delayTime += CupheadTime.FixedDelta;
			if (delayTime > delay)
			{
				delayTime -= delay;
				delay = this.sugarcubesDelayString.PopFloat();
				phase = this.sugarcubesPhaseString.PopFloat();
				SaltbakerLevelSugarcube saltbakerLevelSugarcube = this.sugarcubePrefab.Spawn<SaltbakerLevelSugarcube>();
				saltbakerLevelSugarcube.Init(new Vector3((float)((!side) ? (Level.Current.Right + 100) : (Level.Current.Left - 100)), p.centerHeight), side, p, phase, this, anim, this.sugarcubesParryString.PopLetter() == 'P');
				this.destroyOnPhaseEnd.Add(saltbakerLevelSugarcube.gameObject);
				anim = (anim + 1) % 3;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002D94 RID: 11668 RVA: 0x001AE0A0 File Offset: 0x001AC4A0
	private IEnumerator dough_cr()
	{
		LevelProperties.Saltbaker.Dough p = base.properties.CurrentState.dough;
		bool side = this.onLeft;
		float attackTime = 0f;
		float delayTime = 0f;
		float delay = p.firstDelay;
		Vector3 left = new Vector3((float)Level.Current.Left - 100f, -300f);
		Vector3 right = new Vector3((float)Level.Current.Right + 100f, -300f);
		YieldInstruction wait = new WaitForFixedUpdate();
		int count = 0;
		int startAnimalType = UnityEngine.Random.Range(0, 3);
		while (attackTime <= p.doughAttackDuration)
		{
			attackTime += CupheadTime.FixedDelta;
			delayTime += CupheadTime.FixedDelta;
			if (delayTime > delay)
			{
				delayTime -= delay;
				delay = this.doughSpawnDelayString.PopFloat();
				int num = this.doughSpawnTypeString.PopInt();
				char c = this.doughSpawnSidePatternString.PopLetter();
				bool flag = (c != 'P') ? (!side) : side;
				SaltbakerLevelDough saltbakerLevelDough = this.doughPrefab.Spawn<SaltbakerLevelDough>();
				saltbakerLevelDough.Init((!flag) ? right : left, (!flag) ? (-p.doughXSpeed[num]) : p.doughXSpeed[num], p.doughYSpeed[num], p.doughGravity[num], p.doughHealth, count, (startAnimalType + count) % 3);
				this.destroyOnPhaseEnd.Add(saltbakerLevelDough.gameObject);
				count++;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x001AE0BB File Offset: 0x001AC4BB
	private void AniEvent_FinishDough()
	{
		this.doughFXAnimator.transform.localScale = base.transform.localScale;
		this.doughFXAnimator.Play("DoughFX");
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x001AE0E8 File Offset: 0x001AC4E8
	private IEnumerator limes_cr()
	{
		LevelProperties.Saltbaker.Limes p = base.properties.CurrentState.limes;
		bool side = this.onLeft;
		float attackTime = 0f;
		float delayTime = 0f;
		float delay = p.firstDelay;
		int sfxID = 0;
		int anim = UnityEngine.Random.Range(0, 4);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (attackTime <= p.boomerangAttackDuration)
		{
			attackTime += CupheadTime.FixedDelta;
			delayTime += CupheadTime.FixedDelta;
			if (delayTime > delay)
			{
				delayTime -= delay;
				delay = this.limesDelayString.PopFloat();
				SaltbakerLevelLime saltbakerLevelLime = this.limePrefab.Spawn<SaltbakerLevelLime>();
				saltbakerLevelLime.Init(new Vector3((float)((!side) ? Level.Current.Right : Level.Current.Left), 0f), side, this.limeHeightString.PopLetter() == 'H', base.properties.CurrentState.limes, sfxID, anim);
				this.destroyOnPhaseEnd.Add(saltbakerLevelLime.gameObject);
				sfxID = (sfxID + 1) % 3;
				anim = (anim + 1) % 4;
			}
			yield return wait;
		}
		yield return wait;
		yield break;
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x001AE104 File Offset: 0x001AC504
	public IEnumerator phase_one_to_two_cr()
	{
		this.phaseOneEnded = true;
		base.animator.SetTrigger("EndPhaseOne");
		yield return base.animator.WaitForAnimationToStart(this, "PhaseOneToTwo", false);
		this.phaseTwoHPPrediction = (float)((int)(base.properties.CurrentHealth - base.properties.GetNextStateHealthTrigger() * base.properties.TotalHealth));
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.weaponManager.EnableSuper(false);
			}
		}
		yield return base.animator.WaitForAnimationToEnd(this, "PhaseTwoIntro", false, true);
		this.phaseTwoStarted = true;
		this.Phase2SwitchOnPatterns();
		yield break;
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x001AE120 File Offset: 0x001AC520
	private void AniEvent_HitTable()
	{
		this.phaseOneCollider.enabled = false;
		CupheadLevelCamera.Current.Shake(55f, 0.5f, false);
		base.transform.localScale = new Vector3(1f, 1f);
		AudioManager.StartBGMAlternate(0);
	}

	// Token: 0x06002D99 RID: 11673 RVA: 0x001AE16E File Offset: 0x001AC56E
	private void AniEvent_KillFires()
	{
		((SaltbakerLevel)Level.Current).KillFires();
	}

	// Token: 0x06002D9A RID: 11674 RVA: 0x001AE180 File Offset: 0x001AC580
	private void AniEvent_HandsClosed()
	{
		this.ClearPhaseOneObjects();
		((SaltbakerLevel)Level.Current).ClearFires();
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.weaponManager.InterruptSuper();
			}
		}
		foreach (AbstractPlayerController abstractPlayerController2 in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController2 = (LevelPlayerController)abstractPlayerController2;
			if (levelPlayerController2 != null)
			{
				levelPlayerController2.DisableInput();
				levelPlayerController2.motor.ClearBufferedInput();
				Level.Current.SetBounds(new int?(10780), new int?(-9220), new int?(446), new int?(370));
				levelPlayerController2.transform.position = this.playerDefrostPositions[(int)levelPlayerController2.id].position + Vector3.left * 10000f;
			}
		}
		this.transitionCamera.SetActive(true);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		LevelPauseGUI.OnUnpauseEvent += this.SuppressPlayerJoin;
		if (((SaltbakerLevel)Level.Current).playerLost)
		{
			base.animator.speed = 0f;
		}
		else
		{
			base.StartCoroutine(this.scroll_bg_cr());
		}
	}

	// Token: 0x06002D9B RID: 11675 RVA: 0x001AE328 File Offset: 0x001AC728
	private void AniEvent_ShakeScreen()
	{
		CupheadLevelCamera.Current.Shake(55f, 0.5f, false);
	}

	// Token: 0x06002D9C RID: 11676 RVA: 0x001AE33F File Offset: 0x001AC73F
	private void AniEvent_FadeInReflection()
	{
		base.StartCoroutine(this.fade_in_reflection_cr());
	}

	// Token: 0x06002D9D RID: 11677 RVA: 0x001AE350 File Offset: 0x001AC750
	private void ClearPhaseOneObjects()
	{
		this.attackCoroutines.RemoveAll((Coroutine c) => c == null);
		foreach (Coroutine routine in this.attackCoroutines)
		{
			base.StopCoroutine(routine);
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("PlayerProjectile");
		for (int m = 0; m < array.Length; m++)
		{
			UnityEngine.Object.Destroy(array[m]);
		}
		Effect[] array2 = (Effect[])UnityEngine.Object.FindObjectsOfType(typeof(Effect));
		for (int j = 0; j < array2.Length; j++)
		{
			UnityEngine.Object.Destroy(array2[j].gameObject);
		}
		this.destroyOnPhaseEnd.RemoveAll((GameObject i) => i == null);
		for (int k = 0; k < this.destroyOnPhaseEnd.Count; k++)
		{
			UnityEngine.Object.Destroy(this.destroyOnPhaseEnd[k]);
		}
		UnityEngine.Object.Destroy(this.strawberryBasket.gameObject);
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.weaponManager.AbortEX();
			}
		}
		foreach (PlayerSuperChaliceShieldHeart playerSuperChaliceShieldHeart in UnityEngine.Object.FindObjectsOfType<PlayerSuperChaliceShieldHeart>())
		{
			playerSuperChaliceShieldHeart.transform.parent = playerSuperChaliceShieldHeart.player.transform;
		}
	}

	// Token: 0x06002D9E RID: 11678 RVA: 0x001AE548 File Offset: 0x001AC948
	private IEnumerator scroll_bg_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.transitionDelayAfterHandsClose);
		SaltbakerLevel level = Level.Current as SaltbakerLevel;
		float t = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		CupheadLevelCamera.Current.Shake(8f, this.transitionDuration, false);
		Vector3 shadowOffset = this.shadow.transform.position - this.table.transform.position;
		while (t < this.transitionDuration)
		{
			level.yScrollPos = EaseUtils.EaseInOut(EaseUtils.EaseType.easeInSine, EaseUtils.EaseType.easeOutBack, 0f, 1f, Mathf.InverseLerp(0f, this.transitionDuration, t));
			this.shadow.transform.position = shadowOffset + this.table.transform.position + Vector3.up * level.yScrollPos * 1500f;
			t += CupheadTime.FixedDelta;
			yield return wait;
		}
		level.yScrollPos = 1f;
		yield break;
	}

	// Token: 0x06002D9F RID: 11679 RVA: 0x001AE564 File Offset: 0x001AC964
	private IEnumerator fade_in_reflection_cr()
	{
		this.reflectionCamera.SetActive(true);
		yield return null;
		this.reflectionTexture.SetActive(true);
		float c = 0f;
		while (c < 0.5f)
		{
			c = Mathf.Clamp(c + CupheadTime.Delta * 5f, 0f, 0.5f);
			this.reflectionMaterial.color = new Color(1f, 1f, 1f, c);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002DA0 RID: 11680 RVA: 0x001AE57F File Offset: 0x001AC97F
	private void SuppressPlayerJoin()
	{
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
	}

	// Token: 0x06002DA1 RID: 11681 RVA: 0x001AE58C File Offset: 0x001AC98C
	private void AniEvent_HandsOpen()
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.EnableInput();
				levelPlayerController.weaponManager.DisableInput();
				levelPlayerController.transform.position = this.playerDefrostPositions[(int)levelPlayerController.id].position + Vector3.left * 10000f;
				levelPlayerController.motor.DoPostSuperHop();
			}
		}
	}

	// Token: 0x06002DA2 RID: 11682 RVA: 0x001AE640 File Offset: 0x001ACA40
	private void AniEvent_SFX_MagicDough()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_magiccookie");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_magiccookie");
	}

	// Token: 0x06002DA3 RID: 11683 RVA: 0x001AE65C File Offset: 0x001ACA5C
	private void AniEvent_SpawnJumpers()
	{
		((SaltbakerLevel)Level.Current).SpawnJumpers();
	}

	// Token: 0x06002DA4 RID: 11684 RVA: 0x001AE66D File Offset: 0x001ACA6D
	private void AniEvent_ShakeScreenSaltFall()
	{
		CupheadLevelCamera.Current.Shake(20f, 2f, false);
	}

	// Token: 0x06002DA5 RID: 11685 RVA: 0x001AE684 File Offset: 0x001ACA84
	private void AniEvent_RestorePlayers()
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.weaponManager.EnableSuper(true);
				levelPlayerController.weaponManager.EnableInput();
				Level.Current.SetBounds(new int?(780), new int?(780), new int?(446), new int?(370));
				levelPlayerController.transform.position += Vector3.right * 10000f;
			}
		}
		foreach (PlayerSuperChaliceShieldHeart playerSuperChaliceShieldHeart in UnityEngine.Object.FindObjectsOfType<PlayerSuperChaliceShieldHeart>())
		{
			playerSuperChaliceShieldHeart.transform.parent = null;
		}
		this.transitionCamera.SetActive(false);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		LevelPauseGUI.OnUnpauseEvent -= this.SuppressPlayerJoin;
	}

	// Token: 0x06002DA6 RID: 11686 RVA: 0x001AE7AC File Offset: 0x001ACBAC
	private void AniEvent_StartHandIdle()
	{
		this.phaseOneRenderer.enabled = false;
		this.handAnimator.Play("Idle");
		this.handAnimator.Update(0f);
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x06002DA7 RID: 11687 RVA: 0x001AE7E6 File Offset: 0x001ACBE6
	private void Phase2SwitchOnPatterns()
	{
		if (base.properties.CurrentState.leaf.leavesOn)
		{
			base.StartCoroutine(this.leaf_fall_cr());
		}
	}

	// Token: 0x06002DA8 RID: 11688 RVA: 0x001AE80F File Offset: 0x001ACC0F
	public bool PreDamagePhaseTwoAndReturnWhetherDoomed(float damage)
	{
		this.phaseTwoHPPrediction -= damage;
		if (this.phaseTwoHPPrediction < 0f)
		{
			AudioManager.StopBGM();
			AudioManager.StartBGMAlternate(1);
		}
		return this.phaseTwoHPPrediction < 0f;
	}

	// Token: 0x06002DA9 RID: 11689 RVA: 0x001AE848 File Offset: 0x001ACC48
	public void DamageSaltbaker(float damage, int turretIndex)
	{
		base.properties.DealDamage(damage);
		if (base.properties.CurrentState.stateName != LevelProperties.Saltbaker.States.PhaseThree)
		{
			base.animator.Play(this.turretHitAnimName[turretIndex]);
			base.animator.Update(0f);
			this.handAnimator.Play("Hit", 0, 0f);
			this.mintHandAnimator.Play((turretIndex != 3) ? "HitA" : "HitB", 1, 0f);
			CupheadLevelCamera.Current.Shake(30f, 0.5f, false);
		}
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x001AE8EC File Offset: 0x001ACCEC
	private void AniEvent_SpawnPepperShaker()
	{
		this.turrets[this.turretIndex] = UnityEngine.Object.Instantiate<SaltbakerLevelFeistTurret>(this.feistTurretPrefab);
		this.turrets[this.turretIndex].transform.position = this.turretRoots[this.turretIndex].position;
		this.turrets[this.turretIndex].transform.localScale = new Vector3(Mathf.Sign(-this.turrets[this.turretIndex].transform.position.x), 1f);
		this.turrets[this.turretIndex].Setup(base.properties.CurrentState.turrets, this, this.turretIndex);
		this.turretIndex++;
		if (this.turretIndex == 4)
		{
			base.StartCoroutine(this.turret_cr());
		}
	}

	// Token: 0x06002DAB RID: 11691 RVA: 0x001AE9D0 File Offset: 0x001ACDD0
	private IEnumerator turret_cr()
	{
		LevelProperties.Saltbaker.Turrets p = base.properties.CurrentState.turrets;
		this.turretIndex = UnityEngine.Random.Range(0, 4);
		this.turretFiringDir = Rand.PosOrNeg();
		PatternString bulletTypeString = new PatternString(p.bulletTypeString, true, true);
		yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
		for (;;)
		{
			if (this.turrets != null && this.turrets[this.turretFiringOrder[this.turretIndex]].IsActivated)
			{
				bool isPink = bulletTypeString.PopLetter() == 'P';
				this.turrets[this.turretFiringOrder[this.turretIndex]].Shoot(isPink, p.warningTime);
				yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
			}
			this.turretIndex = (this.turretIndex + this.turretFiringDir + 4) % this.turrets.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002DAC RID: 11692 RVA: 0x001AE9EC File Offset: 0x001ACDEC
	private IEnumerator leaf_fall_cr()
	{
		LevelProperties.Saltbaker.Leaf p = base.properties.CurrentState.leaf;
		PatternString leavesCountString = new PatternString(p.leavesCountString, true, true);
		bool animA = Rand.Bool();
		float posX = 0f;
		float posY = (float)Level.Current.Ceiling + 20f;
		for (;;)
		{
			animA = !animA;
			base.animator.SetTrigger((!animA) ? "MintB" : "MintA");
			yield return base.animator.WaitForAnimationToStart(this, (!animA) ? "PhaseTwoMintB" : "PhaseTwoMintA", false);
			this.mintHandAnimator.Play((!animA) ? "MintB" : "MintA");
			yield return this.mintHandAnimator.WaitForAnimationToEnd(this, (!animA) ? "MintB" : "MintA", false, true);
			yield return CupheadTime.WaitForSeconds(this, p.reenterDelay);
			int leavesCount = leavesCountString.PopInt();
			float offset = (float)(Level.Current.Width / leavesCount);
			float extraOffset = p.leavesOffset.RandomFloat();
			List<int> animIDs = new List<int>
			{
				0,
				1,
				2,
				3
			};
			for (int i = 0; i < animIDs.Count; i++)
			{
				int index = UnityEngine.Random.Range(0, animIDs.Count);
				int value = animIDs[i];
				animIDs[i] = animIDs[index];
				animIDs[index] = value;
			}
			for (int j = 0; j < leavesCount; j++)
			{
				posX = offset * ((float)j - (float)(leavesCount - 1) / 2f) - p.xDistance / 2f;
				Vector3 pos = new Vector3(posX + extraOffset, posY);
				SaltBakerLevelLeaf saltBakerLevelLeaf = this.leafFallPrefab.Spawn<SaltBakerLevelLeaf>();
				saltBakerLevelLeaf.Init(pos, p.xTime, p.xDistance, p.yConstantSpeed, p.ySpeed, this, animIDs[j % 4]);
			}
			for (int k = 0; k < UnityEngine.Random.Range(4, 8); k++)
			{
				SaltbakerLevelBGMint saltbakerLevelBGMint = UnityEngine.Object.Instantiate<SaltbakerLevelBGMint>(this.bgMintPrefab, new Vector3((float)UnityEngine.Random.Range(Level.Current.Left, 0), (float)(Level.Current.Ceiling + UnityEngine.Random.Range(250, 500))), Quaternion.identity, null);
				saltbakerLevelBGMint.StartAnimation(k % 4);
			}
			AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_leafdescend");
			yield return CupheadTime.WaitForSeconds(this, p.leavesDelay - 1.75f);
		}
		yield break;
	}

	// Token: 0x06002DAD RID: 11693 RVA: 0x001AEA07 File Offset: 0x001ACE07
	public void OnPhaseThree()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.phase_two_to_three_cr());
	}

	// Token: 0x06002DAE RID: 11694 RVA: 0x001AEA1C File Offset: 0x001ACE1C
	private IEnumerator phase_two_to_three_cr()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		base.animator.Play("PhaseTwoDeath");
		this.handAnimator.Play("Death");
		this.mintHandAnimator.Play("None");
		yield return base.animator.WaitForAnimationToStart(this, "PhaseTwoDeath", false);
		this.transitionFader.gameObject.SetActive(true);
		this.turretIndex = 0;
		while (this.turretIndex < 4)
		{
			this.turrets[this.turretIndex].Die();
			this.turretIndex++;
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.975f)
		{
			this.transitionFader.color = new Color(1f, 1f, 1f, Mathf.InverseLerp(0.8f, 0.975f, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
			CupheadLevelCamera.Current.Shake(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * this.endPhaseTwoShakeAmount, this.startPhaseThreeShakeHoldover, false);
			yield return null;
		}
		CupheadLevelCamera.Current.Shake(this.endPhaseTwoShakeAmount, this.startPhaseThreeShakeHoldover, false);
		foreach (SaltbakerLevelFeistTurret saltbakerLevelFeistTurret in this.turrets)
		{
			UnityEngine.Object.Destroy(saltbakerLevelFeistTurret.gameObject);
		}
		this.transitionFader.color = Color.white;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		((SaltbakerLevel)Level.Current).StartPhase3();
		this.BG.SetActive(false);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002DAF RID: 11695 RVA: 0x001AEA37 File Offset: 0x001ACE37
	protected override void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.reflectionCamera);
		UnityEngine.Object.Destroy(this.reflectionTexture);
		base.OnDestroy();
	}

	// Token: 0x06002DB0 RID: 11696 RVA: 0x001AEA55 File Offset: 0x001ACE55
	private void AnimationEvent_SFX_SALTBAKER_P1_DoughAttack_RollAndKnead()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_doughattack_rollandknead");
	}

	// Token: 0x06002DB1 RID: 11697 RVA: 0x001AEA61 File Offset: 0x001ACE61
	private void AnimationEvent_SFX_SALTBAKER_P1_DoughAttack_RollingPinAppear()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_doughattack_rollingpinappear");
	}

	// Token: 0x06002DB2 RID: 11698 RVA: 0x001AEA6D File Offset: 0x001ACE6D
	private void AnimationEvent_SFX_SALTBAKER_P1_Intro_BowTiePull()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_intro_bowtiepull");
	}

	// Token: 0x06002DB3 RID: 11699 RVA: 0x001AEA79 File Offset: 0x001ACE79
	private void AnimationEvent_SFX_SALTBAKER_P1_Intro_HandSwipeLimesSugar()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_intro_handswipe_limessugar");
	}

	// Token: 0x06002DB4 RID: 11700 RVA: 0x001AEA85 File Offset: 0x001ACE85
	private void AnimationEvent_SFX_SALTBAKER_P1_Limes_Knife_ChopCut()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_limes_knife_chopcut");
	}

	// Token: 0x06002DB5 RID: 11701 RVA: 0x001AEA91 File Offset: 0x001ACE91
	private void AnimationEvent_SFX_SALTBAKER_P1_Limes_Knife_Scrape()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_limes_knife_scrape");
	}

	// Token: 0x06002DB6 RID: 11702 RVA: 0x001AEA9D File Offset: 0x001ACE9D
	private void AnimationEvent_SFX_SALTBAKER_P1_Limes_Knife_SliceSwing()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_limes_knife_sliceswing");
	}

	// Token: 0x06002DB7 RID: 11703 RVA: 0x001AEAA9 File Offset: 0x001ACEA9
	private void AnimationEvent_SFX_SALTBAKER_P1_StrawberrySqueeze_Attack()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_strawberrysqueeze_attack");
	}

	// Token: 0x06002DB8 RID: 11704 RVA: 0x001AEAB5 File Offset: 0x001ACEB5
	private void AnimationEvent_SFX_SALTBAKER_P1_SugarCube_Blow()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_sugarcube_blow");
	}

	// Token: 0x06002DB9 RID: 11705 RVA: 0x001AEAC1 File Offset: 0x001ACEC1
	private void AnimationEvent_SFX_SALTBAKER_P1_SugarCube_KnockAndBreak()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_sugarcube_knockandbreak");
	}

	// Token: 0x06002DBA RID: 11706 RVA: 0x001AEACD File Offset: 0x001ACECD
	private void AnimationEvent_SFX_SALTBAKER_P1_SugarCube_PlaceOnTable()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_sugarcube_placeontable");
	}

	// Token: 0x06002DBB RID: 11707 RVA: 0x001AEAD9 File Offset: 0x001ACED9
	private void AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_A_TableSlam()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_a_tableslam");
	}

	// Token: 0x06002DBC RID: 11708 RVA: 0x001AEAE5 File Offset: 0x001ACEE5
	private void AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_B_HatRemove()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_b_hatremove");
	}

	// Token: 0x06002DBD RID: 11709 RVA: 0x001AEAF1 File Offset: 0x001ACEF1
	private void AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_C_ShroomInsert()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_c_shroominsert");
	}

	// Token: 0x06002DBE RID: 11710 RVA: 0x001AEAFD File Offset: 0x001ACEFD
	private void AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_D_BakerPowerup()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_d_bakerpowerup");
	}

	// Token: 0x06002DBF RID: 11711 RVA: 0x001AEB09 File Offset: 0x001ACF09
	private void AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_E_GrabandRise()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_e_grabandrise");
	}

	// Token: 0x06002DC0 RID: 11712 RVA: 0x001AEB15 File Offset: 0x001ACF15
	private void AnimationEvent_SFX_SALTBAKER_P2_MintLeafAttack_LaunchThrow()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_launchthrow");
	}

	// Token: 0x06002DC1 RID: 11713 RVA: 0x001AEB21 File Offset: 0x001ACF21
	private void AnimationEvent_SFX_SALTBAKER_P2_MintLeafAttack_LeafRustle()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_leafrustle");
	}

	// Token: 0x06002DC2 RID: 11714 RVA: 0x001AEB2D File Offset: 0x001ACF2D
	private void AnimationEvent_SFX_SALTBAKER_P2_Intro_Fingersnap()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_intro_fingersnap");
	}

	// Token: 0x06002DC3 RID: 11715 RVA: 0x001AEB39 File Offset: 0x001ACF39
	private void AnimationEvent_SFX_SALTBAKER_P2_Intro_Fingersnap_Laugh()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_intro_fingersnap_laugh");
	}

	// Token: 0x06002DC4 RID: 11716 RVA: 0x001AEB45 File Offset: 0x001ACF45
	private void AnimationEvent_SFX_SALTBAKER_P2_VocalPain()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_vocal_pain");
	}

	// Token: 0x06002DC5 RID: 11717 RVA: 0x001AEB51 File Offset: 0x001ACF51
	private void AnimationEvent_SFX_SALTBAKER_P2_Death()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_death");
	}

	// Token: 0x06002DC6 RID: 11718 RVA: 0x001AEB5D File Offset: 0x001ACF5D
	public void SFXLeafRustle()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_leafrustle");
	}

	// Token: 0x06002DC7 RID: 11719 RVA: 0x001AEB69 File Offset: 0x001ACF69
	public void SFXLaunchThrow()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_launchthrow");
	}

	// Token: 0x0400360E RID: 13838
	private const float MOVE_POS_X = 370f;

	// Token: 0x0400360F RID: 13839
	private const float MINT_ANIMATION_LENGTH = 1.75f;

	// Token: 0x04003610 RID: 13840
	private const float PHASE_TWO_REFLECTION_OPACITY = 0.5f;

	// Token: 0x04003611 RID: 13841
	public SaltbakerLevelSaltbaker.State currentAttack;

	// Token: 0x04003612 RID: 13842
	private SaltbakerLevelSaltbaker.State prevAttack;

	// Token: 0x04003613 RID: 13843
	private float[] preAttackTime = new float[]
	{
		1.75f,
		4.5416665f,
		5.2083335f,
		4.2083335f
	};

	// Token: 0x04003614 RID: 13844
	private float[] postAttackTime = new float[]
	{
		1.125f,
		1.5833334f,
		0.33333334f,
		1.9166666f
	};

	// Token: 0x04003616 RID: 13846
	[SerializeField]
	private SpriteRenderer sugarTextReversed;

	// Token: 0x04003617 RID: 13847
	[SerializeField]
	private Transform[] playerDefrostPositions;

	// Token: 0x04003618 RID: 13848
	[SerializeField]
	private GameObject shadow;

	// Token: 0x04003619 RID: 13849
	[SerializeField]
	private GameObject table;

	// Token: 0x0400361A RID: 13850
	[Header("Prefabs")]
	[SerializeField]
	private SaltbakerLevelStrawberry strawberryPrefab;

	// Token: 0x0400361B RID: 13851
	[SerializeField]
	private SaltbakerLevelSugarcube sugarcubePrefab;

	// Token: 0x0400361C RID: 13852
	[SerializeField]
	private SaltbakerLevelDough doughPrefab;

	// Token: 0x0400361D RID: 13853
	[SerializeField]
	private SaltbakerLevelLime limePrefab;

	// Token: 0x0400361E RID: 13854
	[SerializeField]
	private SaltbakerLevelStrawberryBasket strawberryBasket;

	// Token: 0x0400361F RID: 13855
	[SerializeField]
	private SaltbakerLevelFeistTurret feistTurretPrefab;

	// Token: 0x04003620 RID: 13856
	private SaltbakerLevelFeistTurret[] turrets = new SaltbakerLevelFeistTurret[4];

	// Token: 0x04003621 RID: 13857
	private int turretIndex;

	// Token: 0x04003622 RID: 13858
	private int[] turretFiringOrder = new int[]
	{
		2,
		1,
		3,
		0
	};

	// Token: 0x04003623 RID: 13859
	private int turretFiringDir;

	// Token: 0x04003624 RID: 13860
	private string[] turretHitAnimName = new string[]
	{
		"PhaseTwoHitB",
		"PhaseTwoHitA",
		"PhaseTwoHitD",
		"PhaseTwoHitC"
	};

	// Token: 0x04003625 RID: 13861
	[SerializeField]
	private SaltBakerLevelLeaf leafFallPrefab;

	// Token: 0x04003626 RID: 13862
	[SerializeField]
	private SaltbakerLevelBGMint bgMintPrefab;

	// Token: 0x04003627 RID: 13863
	[SerializeField]
	private Transform[] turretRoots;

	// Token: 0x04003628 RID: 13864
	[SerializeField]
	private Animator handAnimator;

	// Token: 0x04003629 RID: 13865
	[SerializeField]
	private GameObject transitionCamera;

	// Token: 0x0400362A RID: 13866
	[SerializeField]
	private float transitionDelayAfterHandsClose;

	// Token: 0x0400362B RID: 13867
	[SerializeField]
	private float transitionDuration = 2.5f;

	// Token: 0x0400362C RID: 13868
	[SerializeField]
	private SpriteRenderer transitionFader;

	// Token: 0x0400362D RID: 13869
	[SerializeField]
	private float endPhaseTwoShakeAmount = 75f;

	// Token: 0x0400362E RID: 13870
	[SerializeField]
	private float startPhaseThreeShakeHoldover = 4f;

	// Token: 0x0400362F RID: 13871
	private DamageReceiver damageReceiver;

	// Token: 0x04003630 RID: 13872
	private Vector3 startPos;

	// Token: 0x04003631 RID: 13873
	private bool onLeft;

	// Token: 0x04003632 RID: 13874
	private float scale;

	// Token: 0x04003633 RID: 13875
	private bool phaseOneEnded;

	// Token: 0x04003634 RID: 13876
	public bool phaseTwoStarted;

	// Token: 0x04003635 RID: 13877
	public bool preventAdditionalTurretLaunch;

	// Token: 0x04003636 RID: 13878
	private float phaseTwoHPPrediction;

	// Token: 0x04003637 RID: 13879
	private PatternString strawberriesSpawnString;

	// Token: 0x04003638 RID: 13880
	private PatternString strawberriesDelayString;

	// Token: 0x04003639 RID: 13881
	private PatternString sugarcubesPhaseString;

	// Token: 0x0400363A RID: 13882
	private PatternString sugarcubesDelayString;

	// Token: 0x0400363B RID: 13883
	private PatternString sugarcubesParryString;

	// Token: 0x0400363C RID: 13884
	private PatternString doughSpawnSidePatternString;

	// Token: 0x0400363D RID: 13885
	private PatternString doughSpawnTypeString;

	// Token: 0x0400363E RID: 13886
	private PatternString doughSpawnDelayString;

	// Token: 0x0400363F RID: 13887
	[SerializeField]
	private Animator doughFXAnimator;

	// Token: 0x04003640 RID: 13888
	private PatternString limeHeightString;

	// Token: 0x04003641 RID: 13889
	private PatternString limesDelayString;

	// Token: 0x04003642 RID: 13890
	[SerializeField]
	private GameObject BG;

	// Token: 0x04003643 RID: 13891
	[SerializeField]
	private Collider2D phaseOneCollider;

	// Token: 0x04003644 RID: 13892
	[SerializeField]
	private SpriteRenderer phaseOneRenderer;

	// Token: 0x04003645 RID: 13893
	[SerializeField]
	private float[] timeToNextAttack = new float[4];

	// Token: 0x04003646 RID: 13894
	[SerializeField]
	private Animator mintHandAnimator;

	// Token: 0x04003647 RID: 13895
	private List<GameObject> destroyOnPhaseEnd = new List<GameObject>();

	// Token: 0x04003648 RID: 13896
	private List<Coroutine> attackCoroutines = new List<Coroutine>();

	// Token: 0x04003649 RID: 13897
	[SerializeField]
	private GameObject reflectionCamera;

	// Token: 0x0400364A RID: 13898
	[SerializeField]
	private Material reflectionMaterial;

	// Token: 0x0400364B RID: 13899
	[SerializeField]
	private GameObject reflectionTexture;

	// Token: 0x020007D5 RID: 2005
	public enum State
	{
		// Token: 0x0400364F RID: 13903
		Idle,
		// Token: 0x04003650 RID: 13904
		Strawberries,
		// Token: 0x04003651 RID: 13905
		Sugarcubes,
		// Token: 0x04003652 RID: 13906
		Dough,
		// Token: 0x04003653 RID: 13907
		Limes
	}
}
