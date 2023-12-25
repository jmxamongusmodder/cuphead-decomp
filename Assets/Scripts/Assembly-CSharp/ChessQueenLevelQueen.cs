using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054E RID: 1358
public class ChessQueenLevelQueen : LevelProperties.ChessQueen.Entity
{
	// Token: 0x17000340 RID: 832
	// (get) Token: 0x0600191C RID: 6428 RVA: 0x000E389F File Offset: 0x000E1C9F
	// (set) Token: 0x0600191D RID: 6429 RVA: 0x000E38A7 File Offset: 0x000E1CA7
	public ChessQueenLevelQueen.States state { get; private set; }

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x0600191E RID: 6430 RVA: 0x000E38B0 File Offset: 0x000E1CB0
	// (set) Token: 0x0600191F RID: 6431 RVA: 0x000E38B8 File Offset: 0x000E1CB8
	public ChessQueenLevelLightning activeLightning { get; private set; }

	// Token: 0x06001920 RID: 6432 RVA: 0x000E38C4 File Offset: 0x000E1CC4
	public override void LevelInit(LevelProperties.ChessQueen properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.onIntroEventHandler;
		LevelProperties.ChessQueen.Turret turret = properties.CurrentState.turret;
		this.cannons = new List<ChessQueenLevelCannon>();
		this.cannonLeft.SetProperties(turret.leftTurretRange.min, turret.leftTurretRange.max, turret.leftTurretRotationTime, ChessQueenLevelCannon.CannonPosition.Side, turret, this);
		this.cannons.Add(this.cannonLeft);
		this.cannonMiddle.SetProperties(turret.middleTurretRange.min, turret.middleTurretRange.max, turret.middleTurretRotationTime, ChessQueenLevelCannon.CannonPosition.Center, turret, this);
		this.cannons.Add(this.cannonMiddle);
		this.cannonRight.SetProperties(turret.rightTurretRange.min, turret.rightTurretRange.max, turret.rightTurretRotationTime, ChessQueenLevelCannon.CannonPosition.Side, turret, this);
		this.cannons.Add(this.cannonRight);
		this.cannonCycleDirection = MathUtils.PlusOrMinus();
		this.activeCannonIndex = ((this.cannonCycleDirection != -1) ? 0 : 2);
		this.cannons[this.activeCannonIndex].IsActive = true;
		base.StartCoroutine(this.check_cannons_cr());
		this.delayPattern = new PatternString(properties.CurrentState.queen.queenAttackDelayString, true, true);
		this.lightningPositionPattern = new PatternString(properties.CurrentState.lightning.lightningPositionString, true, true);
		this.flipPositionString = Rand.Bool();
		this.positionPattern = new PatternString(properties.CurrentState.movement.queenPositionString, true);
		this.SFX_KOG_QUEEN_IntroTypeWriter();
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x000E3A68 File Offset: 0x000E1E68
	protected override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemyProjectile(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			ChessQueenLevelCannonball component = hit.GetComponent<ChessQueenLevelCannonball>();
			if (component)
			{
				this.receiveDamage();
				this.SFX_KOG_QUEEN_CannonHitQueenDing();
				component.HitQueen();
			}
		}
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x000E3AA8 File Offset: 0x000E1EA8
	private void receiveDamage()
	{
		base.properties.DealDamage((!PlayerManager.BothPlayersActive()) ? 10f : ChessKingLevelKing.multiplayerDamageNerf);
		this.hitFlash.Flash(0.7f);
		if (base.properties.CurrentHealth <= 0f)
		{
			this.die();
		}
		else
		{
			this.mouse.HitQueen();
		}
		if (!base.animator.GetBool("OnLightning"))
		{
			this.headWobbleCurrentAmplitude = this.headWobbleAmplitude;
			this.headWobbleTimer = 0f;
		}
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x000E3B40 File Offset: 0x000E1F40
	public void StateChanged()
	{
		this.delayPattern = new PatternString(base.properties.CurrentState.queen.queenAttackDelayString, true, true);
		this.lightningPositionPattern = new PatternString(base.properties.CurrentState.lightning.lightningPositionString, true, true);
		this.positionPattern = new PatternString(base.properties.CurrentState.movement.queenPositionString, true);
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x000E3BB4 File Offset: 0x000E1FB4
	private void LateUpdate()
	{
		foreach (SpriteRenderer spriteRenderer in this.dressRenderers)
		{
			spriteRenderer.enabled = (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || base.animator.GetCurrentAnimatorStateInfo(0).IsTag("Egg"));
		}
		this.head.localPosition = new Vector3(Mathf.Sin(this.headWobbleTimer) * this.headWobbleCurrentAmplitude, 0f);
		this.headWobbleTimer += CupheadTime.Delta * this.headWobbleSpeed;
		if (this.headWobbleCurrentAmplitude > 0f)
		{
			this.headWobbleCurrentAmplitude -= CupheadTime.Delta * this.headWobbleDecay;
			if (this.headWobbleCurrentAmplitude < 0f)
			{
				this.headWobbleCurrentAmplitude = 0f;
			}
		}
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x000E3CAF File Offset: 0x000E20AF
	private void onIntroEventHandler()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x000E3CC0 File Offset: 0x000E20C0
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.4f);
		base.animator.SetTrigger("Intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro.Start", false, true);
		base.StartCoroutine(this.moving_cr());
		yield break;
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x000E3CDC File Offset: 0x000E20DC
	private IEnumerator check_cannons_cr()
	{
		for (;;)
		{
			while (this.cannons[this.activeCannonIndex].IsActive)
			{
				yield return null;
			}
			this.activeCannonIndex = ((this.cannonCycleDirection <= 0) ? MathUtilities.PreviousIndex(this.activeCannonIndex, this.cannons.Count) : MathUtilities.NextIndex(this.activeCannonIndex, this.cannons.Count));
			this.cannons[this.activeCannonIndex].SetActive(true);
		}
		yield break;
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x000E3CF8 File Offset: 0x000E20F8
	private IEnumerator moving_cr()
	{
		LevelProperties.ChessQueen.Queen p = base.properties.CurrentState.queen;
		float moveSpeed = 0f;
		this.lastXPos = base.transform.position.x;
		base.animator.Play("MoveSlow", 1, 0f);
		base.animator.Update(0f);
		this.isMoving = true;
		bool firstMove = true;
		bool inLightning = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			while (!this.isMoving)
			{
				yield return null;
			}
			float elapsed = 0f;
			float startX = base.transform.position.x;
			float endX = Mathf.Lerp((float)Level.Current.Left + 150f, (float)Level.Current.Right - 150f, (this.positionPattern.PopFloat() * (float)((!this.flipPositionString) ? 1 : -1) + 1f) / 2f);
			if (firstMove && endX > 0f)
			{
				endX = -endX;
			}
			firstMove = false;
			this.movingLeft = (endX < startX);
			float distance = Mathf.Abs(endX - startX);
			float time = distance / p.queenMovementSpeed;
			while (elapsed <= time)
			{
				if (!this.isMoving)
				{
					inLightning = true;
				}
				if (inLightning && this.isMoving)
				{
					inLightning = false;
					startX = base.transform.position.x;
					elapsed = 0f;
					distance = Mathf.Abs(endX - startX);
					int num = 0;
					while (distance < this.minMoveDistanceAfterLightning && num < this.positionPattern.SubStringLength())
					{
						endX = Mathf.Lerp((float)Level.Current.Left + 150f, (float)Level.Current.Right - 150f, (this.positionPattern.PopFloat() * (float)((!this.flipPositionString) ? 1 : -1) + 1f) / 2f);
						distance = Mathf.Abs(endX - startX);
						num++;
					}
					this.movingLeft = (endX < startX);
					time = distance / p.queenMovementSpeed;
					moveSpeed = 0f;
				}
				if ((base.animator.GetCurrentAnimatorStateInfo(1).IsName("MoveSlow") && base.animator.GetCurrentAnimatorStateInfo(1).normalizedTime % 1f < 0.16666667f) || base.animator.GetCurrentAnimatorStateInfo(1).IsName("MoveEaseOut"))
				{
					foreach (SpriteRenderer spriteRenderer in this.dressRenderers)
					{
						spriteRenderer.flipX = !this.movingLeft;
					}
				}
				base.animator.SetBool("Fast", this.isMoving && Mathf.Abs(this.lastXPos - base.transform.position.x) > this.speedThresholdForFastAnimation && this.dressRenderers[0].flipX != this.lastXPos - base.transform.position.x > 0f);
				moveSpeed = Mathf.Clamp(moveSpeed + CupheadTime.FixedDelta * ((!this.isMoving) ? (-this.attackDecel) : this.accel), 0f, 1f);
				float t = elapsed / time;
				float val = (!this.useSineEasing) ? EaseUtils.EaseInOutArbitraryCoefficient(startX, endX, t, this.easeCoefficient) : EaseUtils.EaseInOutSine(startX, endX, t);
				this.lastXPos = base.transform.position.x;
				base.transform.SetPosition(new float?(val), null, null);
				elapsed += CupheadTime.FixedDelta * moveSpeed;
				yield return wait;
			}
		}
		yield break;
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x000E3D13 File Offset: 0x000E2113
	public void StartLightning()
	{
		this.state = ChessQueenLevelQueen.States.Lightning;
		base.StartCoroutine(this.SFX_KOG_QUEEN_VocalAttack_cr());
		base.StartCoroutine(this.lightning_cr());
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x000E3D38 File Offset: 0x000E2138
	private IEnumerator lightning_cr()
	{
		LevelProperties.ChessQueen.Lightning p = base.properties.CurrentState.lightning;
		this.isMoving = false;
		base.animator.SetBool("Fast", false);
		base.animator.SetBool("OnLightning", true);
		this.headWobbleDecay *= 2f;
		yield return CupheadTime.WaitForSeconds(this, p.lightningAnticipationTime);
		base.animator.SetTrigger("OnAttack");
		while (this.activeLightning != null && !this.activeLightning.isGone)
		{
			yield return null;
		}
		base.animator.SetBool("OnLightning", false);
		base.animator.Play("MoveEaseOutHold", 1, 0f);
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "Lightning.Exit", false, true);
		base.animator.Play("MoveSlow", 1, 0f);
		base.animator.Update(0f);
		this.isMoving = true;
		this.headWobbleDecay *= 0.5f;
		yield return CupheadTime.WaitForSeconds(this, this.delayPattern.PopFloat());
		this.state = ChessQueenLevelQueen.States.Idle;
		yield break;
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x000E3D54 File Offset: 0x000E2154
	private void AniEvent_CreateLightning()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		this.lightningPositionPattern.IncrementString();
		this.activeLightning = this.lightningPrefab.Spawn<ChessQueenLevelLightning>();
		this.activeLightning.Create((this.lightningPositionPattern.GetString()[0] != 'P') ? this.lightningPositionPattern.GetFloat() : next.transform.position.x, base.properties.CurrentState.lightning);
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x000E3DDA File Offset: 0x000E21DA
	public void StartEgg()
	{
		this.state = ChessQueenLevelQueen.States.Egg;
		base.animator.SetBool("Egg", true);
		base.StartCoroutine(this.egg_cr());
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x000E3E04 File Offset: 0x000E2204
	private IEnumerator egg_cr()
	{
		LevelProperties.ChessQueen.Egg p = base.properties.CurrentState.egg;
		yield return base.animator.WaitForAnimationToStart(this, "Egg.AttackLoop", false);
		this.SFX_KOG_QUEEN_FabergeEggLoop();
		this.SFX_KOG_QUEEN_FabergeEggTeethLoop();
		float rateTime = 0f;
		float attackTime = 0f;
		float attackDuration = p.eggAttackDuration.RandomFloat();
		this.eggRootLeft.SetPosition(null, null, new float?(5E-07f));
		this.eggRootRight.SetPosition(null, null, new float?(0f));
		while (attackTime < attackDuration)
		{
			attackTime += CupheadTime.Delta;
			if (rateTime > p.eggFireRate)
			{
				this.fireProjectiles();
				rateTime = 0f;
			}
			else
			{
				rateTime += CupheadTime.Delta;
			}
			yield return null;
		}
		float delay = this.delayPattern.PopFloat();
		if (p.eggCooldownDuration + delay < this.maxTimeToHoldForTwoEggAttacks && ((ChessQueenLevel)Level.Current).NextPatternIsEgg())
		{
			if (p.eggCooldownDuration + delay > this.maxTimeToStayOpenForTwoEggAttacks)
			{
				base.animator.SetTrigger("ResetEgg");
				base.animator.SetTrigger("EndAttack");
			}
			yield return CupheadTime.WaitForSeconds(this, p.eggCooldownDuration + delay);
			this.StartEgg();
			yield break;
		}
		base.animator.SetTrigger("EndAttack");
		this.SFX_KOG_QUEEN_FabergeEggLoopStopShort();
		yield return CupheadTime.WaitForSeconds(this, p.eggCooldownDuration);
		base.animator.SetBool("Egg", false);
		this.SFX_KOG_QUEEN_FabergeEggClose();
		this.SFX_KOG_QUEEN_FabergeEggLoopStopEnd();
		this.SFX_KOG_QUEEN_FabergeEggTeethLoopStop();
		yield return base.animator.WaitForAnimationToStart(this, "Egg.End", false);
		yield return CupheadTime.WaitForSeconds(this, delay);
		this.state = ChessQueenLevelQueen.States.Idle;
		yield break;
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x000E3E20 File Offset: 0x000E2220
	private void fireProjectiles()
	{
		LevelProperties.ChessQueen.Egg egg = base.properties.CurrentState.egg;
		Vector2 zero = Vector2.zero;
		float num = (float)((!this.movingLeft) ? -200 : 200);
		zero.y = egg.eggVelocityY.RandomFloat();
		zero.x = egg.eggVelocityX.RandomFloat() + num;
		this.eggRootLeft.transform.position += Vector3.forward * 1E-06f;
		ChessQueenLevelEgg chessQueenLevelEgg = this.eggPrefab.Spawn<ChessQueenLevelEgg>();
		chessQueenLevelEgg.Create(this.eggRootLeft.position, zero, egg.eggGravity, egg.eggSpawnCollisionTimer);
		zero.y = egg.eggVelocityY.RandomFloat();
		zero.x = egg.eggVelocityX.RandomFloat() + num;
		this.eggRootLeft.transform.position += Vector3.forward * 1E-06f;
		ChessQueenLevelEgg chessQueenLevelEgg2 = this.eggPrefab.Spawn<ChessQueenLevelEgg>();
		chessQueenLevelEgg2.Create(this.eggRootRight.position, zero, egg.eggGravity, egg.eggSpawnCollisionTimer);
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x000E3F64 File Offset: 0x000E2364
	private void die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.mouse.Win();
		this.headWobbleCurrentAmplitude = 0f;
		this.StopAllCoroutines();
		if (base.transform.position.x > 0f)
		{
			base.transform.SetScale(new float?(-1f), null, null);
		}
		LevelBossDeathExploder component = base.GetComponent<LevelBossDeathExploder>();
		component.offset.x = component.offset.x * base.transform.localScale.x;
		base.animator.Play("Death");
		base.StartCoroutine(this.SFX_KOG_QUEEN_Death_cr());
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x000E402B File Offset: 0x000E242B
	private void SFX_KOG_QUEEN_IntroTypeWriter()
	{
		AudioManager.Play("sfx_dlc_kog_queen_introtypewriter");
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x000E4037 File Offset: 0x000E2437
	private void AnimationEvent_SFX_KOG_QUEEN_IntroTableFlip()
	{
		AudioManager.Play("sfx_dlc_kog_queen_introtableflip");
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x000E4043 File Offset: 0x000E2443
	private void AnimationEvent_SFX_KOG_QUEEN_FabergeEggOpen()
	{
		AudioManager.Play("sfx_dlc_kog_queen_fabergeegg_open");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeegg_open");
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x000E405F File Offset: 0x000E245F
	private void SFX_KOG_QUEEN_FabergeEggClose()
	{
		AudioManager.Play("sfx_dlc_kog_queen_fabergeegg_close");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeegg_close");
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x000E407B File Offset: 0x000E247B
	private void SFX_KOG_QUEEN_FabergeEggTeethLoop()
	{
		AudioManager.Play("sfx_dlc_kog_queen_fabergeeggteeth_loop");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeeggteeth_loop");
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x000E4097 File Offset: 0x000E2497
	private void SFX_KOG_QUEEN_FabergeEggTeethLoopStop()
	{
		AudioManager.Stop("sfx_dlc_kog_queen_fabergeeggteeth_loop");
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x000E40A3 File Offset: 0x000E24A3
	private void SFX_KOG_QUEEN_FabergeEggLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_kog_queen_fabergeegg_loop");
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_queen_fabergeegg_loop", 0.7f, 2f);
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeegg_loop");
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x000E40D3 File Offset: 0x000E24D3
	private void SFX_KOG_QUEEN_FabergeEggLoopStopShort()
	{
		AudioManager.Stop("sfx_dlc_kog_queen_fabergeegg_loop");
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x000E40DF File Offset: 0x000E24DF
	private void SFX_KOG_QUEEN_FabergeEggLoopStopEnd()
	{
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_queen_fabergeegg_loop", 0f, 1f);
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x000E40F5 File Offset: 0x000E24F5
	private void AnimationEvent_SFX_KOG_QUEEN_SpawnChessPieces()
	{
		AudioManager.Play("sfx_dlc_kog_queen_spawnchesspieces");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_spawnchesspieces");
	}

	// Token: 0x0600193A RID: 6458 RVA: 0x000E4114 File Offset: 0x000E2514
	private IEnumerator SFX_KOG_QUEEN_Death_cr()
	{
		this.SFX_KOG_QUEEN_FabergeEggLoopStopShort();
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		AudioManager.Play("sfx_dlc_kog_queen_death");
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("sfx_dlc_kog_queen_vocal_death");
		yield return CupheadTime.WaitForSeconds(this, 0.7f);
		AudioManager.PlayLoop("sfx_dlc_kog_queen_deathcrownspin_loop");
		yield break;
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x000E4130 File Offset: 0x000E2530
	private IEnumerator SFX_KOG_QUEEN_VocalAttack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0f);
		AudioManager.Play("sfx_dlc_kog_queen_vocal_attack");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_attack");
		yield break;
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x000E414B File Offset: 0x000E254B
	private void AnimationEvent_SFX_KOG_QUEEN_VocalHurt()
	{
		AudioManager.Play("sfx_dlc_kog_queen_vocal_hurt");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_hurt");
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x000E4167 File Offset: 0x000E2567
	private void AnimationEvent_SFX_KOG_QUEEN_VocalLaughLrg()
	{
		AudioManager.Play("sfx_dlc_kog_queen_vocal_laughlrg");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_laughlrg");
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x000E4183 File Offset: 0x000E2583
	private void AnimationEvent_SFX_KOG_QUEEN_VocalLaughSml()
	{
		AudioManager.Play("sfx_dlc_kog_queen_vocal_laughSml");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_laughSml");
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x000E419F File Offset: 0x000E259F
	private void SFX_KOG_QUEEN_CannonHitQueenDing()
	{
		AudioManager.Play("sfx_dlc_kog_queen_cannonhitqueending");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_cannonhitqueending");
	}

	// Token: 0x04002235 RID: 8757
	[SerializeField]
	private SpriteRenderer[] dressRenderers;

	// Token: 0x04002236 RID: 8758
	[SerializeField]
	private float easeCoefficient;

	// Token: 0x04002237 RID: 8759
	[SerializeField]
	private float accel = 2f;

	// Token: 0x04002238 RID: 8760
	[SerializeField]
	private float attackDecel = 2f;

	// Token: 0x04002239 RID: 8761
	[SerializeField]
	private bool useSineEasing;

	// Token: 0x0400223A RID: 8762
	[SerializeField]
	private float minMoveDistanceAfterLightning = 100f;

	// Token: 0x0400223B RID: 8763
	private bool flipPositionString;

	// Token: 0x0400223C RID: 8764
	[SerializeField]
	private HitFlash hitFlash;

	// Token: 0x0400223D RID: 8765
	[SerializeField]
	private ChessQueenLevelCannon cannonLeft;

	// Token: 0x0400223E RID: 8766
	[SerializeField]
	private ChessQueenLevelCannon cannonMiddle;

	// Token: 0x0400223F RID: 8767
	[SerializeField]
	private ChessQueenLevelCannon cannonRight;

	// Token: 0x04002240 RID: 8768
	[SerializeField]
	private Transform head;

	// Token: 0x04002241 RID: 8769
	[SerializeField]
	private ChessQueenLevelLooseMouse mouse;

	// Token: 0x04002242 RID: 8770
	private float headWobbleCurrentAmplitude;

	// Token: 0x04002243 RID: 8771
	private float headWobbleTimer;

	// Token: 0x04002244 RID: 8772
	[SerializeField]
	private float headWobbleSpeed = 50f;

	// Token: 0x04002245 RID: 8773
	[SerializeField]
	private float headWobbleAmplitude = 25f;

	// Token: 0x04002246 RID: 8774
	[SerializeField]
	private float headWobbleDecay = 50f;

	// Token: 0x04002247 RID: 8775
	[Header("Egg")]
	[SerializeField]
	private ChessQueenLevelEgg eggPrefab;

	// Token: 0x04002248 RID: 8776
	[SerializeField]
	private Transform eggRootRight;

	// Token: 0x04002249 RID: 8777
	[SerializeField]
	private Transform eggRootLeft;

	// Token: 0x0400224A RID: 8778
	[SerializeField]
	private float maxTimeToHoldForTwoEggAttacks;

	// Token: 0x0400224B RID: 8779
	[SerializeField]
	private float maxTimeToStayOpenForTwoEggAttacks = 0.7f;

	// Token: 0x0400224C RID: 8780
	[Header("Lightning")]
	[SerializeField]
	private ChessQueenLevelLightning lightningPrefab;

	// Token: 0x0400224D RID: 8781
	[SerializeField]
	public float lightningDisableRange = 150f;

	// Token: 0x0400224E RID: 8782
	private float lastXPos;

	// Token: 0x0400224F RID: 8783
	private int cannonCycleDirection;

	// Token: 0x04002252 RID: 8786
	private List<ChessQueenLevelCannon> cannons;

	// Token: 0x04002253 RID: 8787
	private int activeCannonIndex;

	// Token: 0x04002254 RID: 8788
	private PatternString delayPattern;

	// Token: 0x04002255 RID: 8789
	private PatternString lightningPositionPattern;

	// Token: 0x04002256 RID: 8790
	private PatternString positionPattern;

	// Token: 0x04002257 RID: 8791
	private bool movingLeft;

	// Token: 0x04002258 RID: 8792
	private bool isMoving;

	// Token: 0x04002259 RID: 8793
	private bool dead;

	// Token: 0x0400225A RID: 8794
	public float speedThresholdForFastAnimation;

	// Token: 0x0200054F RID: 1359
	public enum States
	{
		// Token: 0x0400225C RID: 8796
		Idle,
		// Token: 0x0400225D RID: 8797
		Lightning,
		// Token: 0x0400225E RID: 8798
		Egg
	}
}
