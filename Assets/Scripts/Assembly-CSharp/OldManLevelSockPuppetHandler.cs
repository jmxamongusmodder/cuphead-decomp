using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200070F RID: 1807
public class OldManLevelSockPuppetHandler : LevelProperties.OldMan.Entity
{
	// Token: 0x06002726 RID: 10022 RVA: 0x0016E7FD File Offset: 0x0016CBFD
	private void Start()
	{
		this.transState = OldManLevelSockPuppetHandler.TransitionState.None;
		this.dwarvesObject.gameObject.SetActive(false);
	}

	// Token: 0x06002727 RID: 10023 RVA: 0x0016E817 File Offset: 0x0016CC17
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06002728 RID: 10024 RVA: 0x0016E828 File Offset: 0x0016CC28
	public void StartPhase2()
	{
		this.dwarvesObject.gameObject.SetActive(true);
		this.sockPuppetLeft.gameObject.SetActive(true);
		this.sockPuppetRight.gameObject.SetActive(true);
		this.damageDealer = DamageDealer.NewEnemy();
		this.sockPuppetRight.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.sockPuppetLeft.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.sockPuppetRight.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.sockPuppetLeft.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.bounce_ball_cr());
		base.StartCoroutine(this.dwarves_arc_cr());
		((LevelPlayerController)PlayerManager.GetPlayer(PlayerId.PlayerOne)).motor.OnHitEvent += this.TriggerLaugh;
		if (PlayerManager.Multiplayer)
		{
			((LevelPlayerController)PlayerManager.GetPlayer(PlayerId.PlayerTwo)).motor.OnHitEvent += this.TriggerLaugh;
		}
	}

	// Token: 0x06002729 RID: 10025 RVA: 0x0016E94B File Offset: 0x0016CD4B
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600272A RID: 10026 RVA: 0x0016E963 File Offset: 0x0016CD63
	public override void LevelInit(LevelProperties.OldMan properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x0600272B RID: 10027 RVA: 0x0016E96C File Offset: 0x0016CD6C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600272C RID: 10028 RVA: 0x0016E97F File Offset: 0x0016CD7F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600272D RID: 10029 RVA: 0x0016E9A0 File Offset: 0x0016CDA0
	private IEnumerator dwarves_arc_cr()
	{
		LevelProperties.OldMan.Dwarf p = base.properties.CurrentState.dwarf;
		PatternString arcAttackDelay = new PatternString(p.arcAttackDelayString, true, true);
		PatternString arcAttackPos = new PatternString(p.arcAttackPosString, true, true);
		PatternString arcShootHeight = new PatternString(p.arcShootHeightString, true, true);
		PatternString parryableString = new PatternString(p.parryString, true);
		int posIndex = 0;
		bool typeA = Rand.Bool();
		yield return CupheadTime.WaitForSeconds(this, 2f);
		for (;;)
		{
			posIndex = arcAttackPos.PopInt();
			if (this.dwarves[posIndex].inPlace)
			{
				this.dwarves[posIndex].ShootInArc(arcShootHeight.PopFloat(), p.arcApex, p.arcHealth, typeA, parryableString.PopLetter() == 'P', p.arcAttackWarningTime);
				typeA = !typeA;
				yield return CupheadTime.WaitForSeconds(this, arcAttackDelay.PopFloat());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600272E RID: 10030 RVA: 0x0016E9BC File Offset: 0x0016CDBC
	private IEnumerator bounce_ball_cr()
	{
		LevelProperties.OldMan.Hands p = base.properties.CurrentState.hands;
		PatternString leftHandPosString = new PatternString(p.leftHandPosString, true, true);
		PatternString rightHandPosString = new PatternString(p.rightHandPosString, true, true);
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		this.fromLeft = Rand.Bool();
		if (this.fromLeft)
		{
			this.sockPuppetLeft.AnIEvent_HoldingBall();
		}
		else
		{
			this.sockPuppetRight.AnIEvent_HoldingBall();
		}
		this.sockPuppetLeft.MoveToPos(this.KDpuppetYPositions[this.leftHandPos].position.y, 1f);
		base.StartCoroutine(this.move_level_borders_time_cr(1060, Level.Current.Right, 0.5f));
		yield return CupheadTime.WaitForSeconds(this, 0.45f);
		base.animator.Play("Ph2_Enter");
		yield return base.animator.WaitForAnimationToStart(this, "LookUpLeftAndBack", false);
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		this.sockPuppetRight.MoveToPos(this.DpuppetYPositions[this.rightHandPos].position.y, 1f);
		base.StartCoroutine(this.move_level_borders_time_cr(-Level.Current.Left, 152, 0.5f));
		yield return CupheadTime.WaitForSeconds(this, 0.7f);
		base.animator.Play("LookUpRightAndBack");
		yield return null;
		base.animator.SetTrigger("EndIntroLook");
		bool first = true;
		base.StartCoroutine(this.animate_face_cr());
		for (;;)
		{
			if (!first)
			{
				this.rightHandPosOld = this.rightHandPos;
				this.leftHandPosOld = this.leftHandPos;
				this.rightHandPos = rightHandPosString.PopInt();
				this.leftHandPos = leftHandPosString.PopInt();
				this.sockPuppetLeft.MoveToPos(this.KDpuppetYPositions[this.leftHandPos].position.y, (float)Mathf.Abs(this.leftHandPosOld - this.leftHandPos));
				this.sockPuppetRight.MoveToPos(this.DpuppetYPositions[this.rightHandPos].position.y, (float)Mathf.Abs(this.rightHandPosOld - this.rightHandPos));
			}
			first = false;
			this.sockPuppetRight.animator.SetBool("CanTaunt", this.fromLeft);
			this.sockPuppetLeft.animator.SetBool("CanTaunt", !this.fromLeft);
			while (!this.sockPuppetLeft.ready || !this.sockPuppetRight.ready)
			{
				yield return null;
			}
			this.sockPuppetRight.animator.SetBool("IsCatching", this.fromLeft);
			this.sockPuppetLeft.animator.SetBool("IsCatching", !this.fromLeft);
			yield return CupheadTime.WaitForSeconds(this, p.throwDelay);
			OldManLevelSockPuppet throwingPuppet = (!this.fromLeft) ? this.sockPuppetRight : this.sockPuppetLeft;
			throwingPuppet.animator.SetTrigger("IsThrowing");
			this.sockPuppetRight.animator.SetBool("CanTaunt", false);
			this.sockPuppetLeft.animator.SetBool("CanTaunt", false);
			this.sockPuppetRight.StopTaunt();
			this.sockPuppetLeft.StopTaunt();
			yield return throwingPuppet.animator.WaitForAnimationToEnd(this, "Throw_Start", false, true);
			yield return CupheadTime.WaitForSeconds(this, p.throwWarningTime);
			throwingPuppet.animator.Play("Throw");
			yield return null;
			while (throwingPuppet.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
			{
				yield return null;
			}
			Vector3 startPos = (!this.fromLeft) ? this.sockPuppetRight.throwPosition() : this.sockPuppetLeft.throwPosition();
			Vector3 endPos = (!this.fromLeft) ? this.sockPuppetLeft.catchPosition() : this.sockPuppetRight.catchPosition();
			Vector3 pos = new Vector3(this.mainPlatformCollider.transform.position.x + (float)(this.leftHandPos - this.rightHandPos) * p.bouncePositionSpacing, this.mainPlatformCollider.bounds.max.y);
			this.puppetBall = this.puppetBallPrefab.Spawn<OldManLevelPuppetBall>();
			this.puppetBall.Init(startPos, pos, endPos, p);
			throwingPuppet.animator.Play("Throw_End");
			throwingPuppet.animator.Update(0f);
			throwingPuppet.AnIEvent_NotHoldingBall();
			while (!this.puppetBall.readyToCatch)
			{
				yield return null;
			}
			if (this.fromLeft)
			{
				this.sockPuppetRight.animator.Play("Catch");
				this.sockPuppetRight.animator.Update(0f);
				this.sockPuppetRight.animator.SetBool("IsCatching", false);
			}
			else
			{
				this.sockPuppetLeft.animator.Play("Catch");
				this.sockPuppetLeft.animator.Update(0f);
				this.sockPuppetLeft.animator.SetBool("IsCatching", false);
			}
			this.fromLeft = !this.fromLeft;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600272F RID: 10031 RVA: 0x0016E9D8 File Offset: 0x0016CDD8
	private IEnumerator animate_face_cr()
	{
		float t = 0f;
		float waitTime = 0f;
		bool lookLeft = Rand.Bool();
		PatternString laughString = new PatternString("L,N,N,N,N,N,N,N,L,N,N,N,N,L,N,N,N,N,N,N", true);
		for (;;)
		{
			yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
			if (this.triggerLaugh || laughString.PopLetter() == 'L')
			{
				base.animator.Play("Laugh");
				this.triggerLaugh = false;
				yield return null;
			}
			else
			{
				waitTime = this.idleHoldRange.RandomFloat();
				t = 0f;
				while (t < waitTime && !this.triggerLaugh)
				{
					t += CupheadTime.Delta;
					yield return null;
				}
				int curLook = (!lookLeft) ? this.rightHandPos : this.leftHandPos;
				if (!lookLeft && !this.sockPuppetRight.ready && this.rightHandPos == 2)
				{
					curLook = 1;
				}
				base.animator.Play((!lookLeft) ? ((curLook <= 0) ? "LookRight" : ((curLook <= 1) ? "LookMidRight" : "LookUpRight")) : ((curLook <= 0) ? "LookLeft" : "LookUpLeft"));
				yield return base.animator.WaitForAnimationToStart(this, (!lookLeft) ? ((curLook <= 0) ? "LookRightHold" : ((curLook <= 1) ? "LookMidRightHold" : "LookUpRightHold")) : ((curLook <= 0) ? "LookLeftHold" : "LookUpLeftHold"), false);
				waitTime = this.lookHoldRange.RandomFloat();
				t = 0f;
				while (t < waitTime && (t < this.lookHoldRange.min || ((!lookLeft) ? this.sockPuppetRight.ready : this.sockPuppetLeft.ready)) && !this.triggerLaugh)
				{
					t += CupheadTime.Delta;
					yield return null;
				}
				base.animator.SetTrigger("Continue");
				if (UnityEngine.Random.Range(0f, 1f) < this.chanceToSwitchLookSides)
				{
					lookLeft = !lookLeft;
				}
			}
		}
		yield break;
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x0016E9F4 File Offset: 0x0016CDF4
	private void TriggerLaugh()
	{
		if (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Laugh"))
		{
			this.triggerLaugh = true;
		}
	}

	// Token: 0x06002731 RID: 10033 RVA: 0x0016EA26 File Offset: 0x0016CE26
	public void CatchBall()
	{
		this.puppetBall.GetCaught();
	}

	// Token: 0x06002732 RID: 10034 RVA: 0x0016EA33 File Offset: 0x0016CE33
	public void OnPhase3()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.deathAnimation_cr());
	}

	// Token: 0x06002733 RID: 10035 RVA: 0x0016EA60 File Offset: 0x0016CE60
	private IEnumerator deathAnimation_cr()
	{
		((LevelPlayerController)PlayerManager.GetPlayer(PlayerId.PlayerOne)).motor.OnHitEvent -= this.TriggerLaugh;
		if (PlayerManager.Multiplayer)
		{
			((LevelPlayerController)PlayerManager.GetPlayer(PlayerId.PlayerTwo)).motor.OnHitEvent -= this.TriggerLaugh;
		}
		this.sockPuppetLeft.Die();
		this.sockPuppetRight.Die();
		if (this.puppetBall == null)
		{
			this.puppetBall = this.puppetBallPrefab.Spawn<OldManLevelPuppetBall>();
			this.puppetBall.transform.position = ((!this.fromLeft) ? this.sockPuppetRight.throwPosition() : this.sockPuppetLeft.throwPosition());
		}
		this.puppetBall.Explode();
		foreach (OldManLevelDwarf oldManLevelDwarf in this.dwarves)
		{
			oldManLevelDwarf.Death(true);
		}
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		base.animator.Play("Angry");
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 startPos = this.oldManAngry.localPosition;
		Vector3 endPos = new Vector3(this.oldManAngry.localPosition.x, 200f);
		Vector3 sockPuppetLeftStart = this.sockPuppetLeft.rootPosition;
		Vector3 sockPuppetRightStart = this.sockPuppetRight.rootPosition;
		Vector3 sockPuppetLeftEnd = (Level.Current.mode != Level.Mode.Easy) ? new Vector3(this.sockPuppetLeft.rootPosition.x - 300f, -1100f) : new Vector3(this.sockPuppetLeft.rootPosition.x, this.KDpuppetYPositions[1].position.y);
		Vector3 sockPuppetRightEnd = (Level.Current.mode != Level.Mode.Easy) ? new Vector3(this.sockPuppetRight.rootPosition.x + 300f, -1100f) : new Vector3(this.sockPuppetRight.rootPosition.x, this.DpuppetYPositions[1].position.y);
		yield return CupheadTime.WaitForSeconds(this, (Level.Current.mode != Level.Mode.Easy) ? 2f : 0.1f);
		float t = 0f;
		float time = (Level.Current.mode != Level.Mode.Easy) ? base.properties.CurrentState.hands.endSlideUpTime : 2f;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			if (Level.Current.mode != Level.Mode.Easy)
			{
				this.oldManAngry.SetPosition(null, new float?(Mathf.Lerp(startPos.y, endPos.y, t / time)), null);
				this.oldManAngryNoseShadow.localPosition = new Vector3(this.oldManAngryNoseShadow.localPosition.x, Mathf.Lerp(0f, 10f, t / time));
			}
			this.sockPuppetLeft.rootPosition = new Vector3(Mathf.Lerp(sockPuppetLeftStart.x, sockPuppetLeftEnd.x, EaseUtils.EaseInSine(0f, 1f, t / time)), Mathf.Lerp(sockPuppetLeftStart.y, sockPuppetLeftEnd.y, EaseUtils.EaseInSine(0f, 1f, t / time)));
			this.sockPuppetRight.rootPosition = new Vector3(Mathf.Lerp(sockPuppetRightStart.x, sockPuppetRightEnd.x, EaseUtils.EaseInSine(0f, 1f, t / time)), Mathf.Lerp(sockPuppetRightStart.y, sockPuppetRightEnd.y, EaseUtils.EaseInSine(0f, 1f, t / time)));
			if (t <= 0.5f && t + CupheadTime.FixedDelta > 0.5f)
			{
				this.sockPuppetLeft.GetComponent<LevelBossDeathExploder>().StopExplosions();
				this.sockPuppetRight.GetComponent<LevelBossDeathExploder>().StopExplosions();
			}
			yield return wait;
		}
		UnityEngine.Object.Destroy(this.dwarvesObject.gameObject);
		if (Level.Current.mode != Level.Mode.Easy)
		{
			base.animator.SetTrigger("ContinueDeath");
			while (this.transState != OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed)
			{
				yield return null;
			}
			yield return base.StartCoroutine(this.move_level_borders_anim_sync_cr(925, 93, 56f));
		}
		yield break;
	}

	// Token: 0x06002734 RID: 10036 RVA: 0x0016EA7C File Offset: 0x0016CE7C
	private IEnumerator move_level_borders_time_cr(int left, int right, float time)
	{
		float t = 0f;
		float startLeft = (float)(-(float)Level.Current.Left);
		float startRight = (float)Level.Current.Right;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float tm = Mathf.InverseLerp(0f, time, t);
			Level.Current.SetBounds(new int?((int)Mathf.Lerp(startLeft, (float)left, tm)), new int?((int)Mathf.Lerp(startRight, (float)right, tm)), null, null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002735 RID: 10037 RVA: 0x0016EAA8 File Offset: 0x0016CEA8
	private IEnumerator move_level_borders_anim_sync_cr(int left, int right, float endFrame)
	{
		float startTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		float startLeft = (float)(-(float)Level.Current.Left);
		float startRight = (float)Level.Current.Right;
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < (endFrame + 1f) / 79f)
		{
			float tm = Mathf.InverseLerp(startTime, endFrame / 79f, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			Level.Current.SetBounds(new int?((int)Mathf.Lerp(startLeft, (float)left, tm)), new int?((int)Mathf.Lerp(startRight, (float)right, tm)), null, null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002736 RID: 10038 RVA: 0x0016EAD8 File Offset: 0x0016CED8
	private IEnumerator shake_platform_cr()
	{
		float amount = 1.5f;
		while (this.transState != OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed)
		{
			this.handsParent.transform.localPosition = new Vector3(UnityEngine.Random.Range(-amount, amount), UnityEngine.Random.Range(-amount, amount));
			amount += 0.25f;
			yield return CupheadTime.WaitForSeconds(this, 0.016666668f);
		}
		yield break;
	}

	// Token: 0x06002737 RID: 10039 RVA: 0x0016EAF4 File Offset: 0x0016CEF4
	private void AniEvent_HandsGrip()
	{
		CupheadLevelCamera.Current.Shake(5f, 0.2f, false);
		this.beardObject.transform.parent = this.handsParent.transform;
		this.rocksUnderBeardObject.transform.parent = this.handsParent.transform;
		base.StartCoroutine(this.shake_platform_cr());
	}

	// Token: 0x06002738 RID: 10040 RVA: 0x0016EB5C File Offset: 0x0016CF5C
	private void AniEvent_PlatformDestroyed()
	{
		this.beardObject.transform.parent = this.BGParent.transform;
		this.rocksUnderBeardObject.transform.parent = this.BGParent.transform;
		this.transState = OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed;
		CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
	}

	// Token: 0x06002739 RID: 10041 RVA: 0x0016EBBB File Offset: 0x0016CFBB
	private void AniEvent_FinishedSwallow()
	{
		this.transState = OldManLevelSockPuppetHandler.TransitionState.InStomach;
	}

	// Token: 0x0600273A RID: 10042 RVA: 0x0016EBC4 File Offset: 0x0016CFC4
	public void SwallowedPlayers()
	{
		base.animator.SetTrigger("SwallowedPlayers");
	}

	// Token: 0x0600273B RID: 10043 RVA: 0x0016EBD6 File Offset: 0x0016CFD6
	public void FinishPuppet()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600273C RID: 10044 RVA: 0x0016EBE3 File Offset: 0x0016CFE3
	private void AnimationEvent_SFX_OMM_P2_EndBreakPlatformEat()
	{
		AudioManager.Play("sfx_dlc_omm_p2_end_breakplatformeat");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_breakplatformeat");
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x0016EBFF File Offset: 0x0016CFFF
	private void AnimationEvent_SFX_OMM_P2_EndBurp()
	{
		AudioManager.Play("sfx_dlc_omm_p2_end_burp");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_burp");
	}

	// Token: 0x0600273E RID: 10046 RVA: 0x0016EC1C File Offset: 0x0016D01C
	private void WORKAROUND_NullifyFields()
	{
		this.OnDeathEvent = null;
		this.idleHoldRange = null;
		this.lookHoldRange = null;
		this.oldManAngry = null;
		this.oldManAngryNoseShadow = null;
		this.mainPlatformCollider = null;
		this.puppetBallPrefab = null;
		this.sockPuppetLeft = null;
		this.sockPuppetRight = null;
		this.platformManager = null;
		this.KDpuppetYPositions = null;
		this.DpuppetYPositions = null;
		this.dwarves = null;
		this.dwarvesObject = null;
		this.handsParent = null;
		this.BGParent = null;
		this.beardObject = null;
		this.rocksUnderBeardObject = null;
		this.damageDealer = null;
		this.puppetBall = null;
	}

	// Token: 0x04002FDA RID: 12250
	private const float POST_DEATH_PRE_MOVE_TIME = 2f;

	// Token: 0x04002FDB RID: 12251
	[SerializeField]
	private MinMax idleHoldRange = new MinMax(0.01f, 0.1f);

	// Token: 0x04002FDC RID: 12252
	[SerializeField]
	private MinMax lookHoldRange = new MinMax(0.75f, 1.5f);

	// Token: 0x04002FDD RID: 12253
	[SerializeField]
	private float chanceToSwitchLookSides = 0.75f;

	// Token: 0x04002FDE RID: 12254
	[SerializeField]
	private float chanceToLaugh = 0.25f;

	// Token: 0x04002FDF RID: 12255
	public OldManLevelSockPuppetHandler.TransitionState transState;

	// Token: 0x04002FE0 RID: 12256
	[SerializeField]
	private Transform oldManAngry;

	// Token: 0x04002FE1 RID: 12257
	[SerializeField]
	private Transform oldManAngryNoseShadow;

	// Token: 0x04002FE2 RID: 12258
	[SerializeField]
	private Collider2D mainPlatformCollider;

	// Token: 0x04002FE3 RID: 12259
	[SerializeField]
	private OldManLevelPuppetBall puppetBallPrefab;

	// Token: 0x04002FE4 RID: 12260
	[SerializeField]
	private OldManLevelSockPuppet sockPuppetLeft;

	// Token: 0x04002FE5 RID: 12261
	[SerializeField]
	private OldManLevelSockPuppet sockPuppetRight;

	// Token: 0x04002FE6 RID: 12262
	[SerializeField]
	private OldManLevelPlatformManager platformManager;

	// Token: 0x04002FE7 RID: 12263
	[SerializeField]
	private Transform[] KDpuppetYPositions;

	// Token: 0x04002FE8 RID: 12264
	[SerializeField]
	private Transform[] DpuppetYPositions;

	// Token: 0x04002FE9 RID: 12265
	[SerializeField]
	private OldManLevelDwarf[] dwarves;

	// Token: 0x04002FEA RID: 12266
	[SerializeField]
	private GameObject dwarvesObject;

	// Token: 0x04002FEB RID: 12267
	[SerializeField]
	private GameObject handsParent;

	// Token: 0x04002FEC RID: 12268
	[SerializeField]
	private GameObject BGParent;

	// Token: 0x04002FED RID: 12269
	[SerializeField]
	private GameObject beardObject;

	// Token: 0x04002FEE RID: 12270
	[SerializeField]
	private GameObject rocksUnderBeardObject;

	// Token: 0x04002FEF RID: 12271
	private DamageDealer damageDealer;

	// Token: 0x04002FF0 RID: 12272
	public Action OnDeathEvent;

	// Token: 0x04002FF1 RID: 12273
	private OldManLevelPuppetBall puppetBall;

	// Token: 0x04002FF2 RID: 12274
	private int leftHandPos = 1;

	// Token: 0x04002FF3 RID: 12275
	private int rightHandPos = 1;

	// Token: 0x04002FF4 RID: 12276
	private int rightHandPosOld;

	// Token: 0x04002FF5 RID: 12277
	private int leftHandPosOld;

	// Token: 0x04002FF6 RID: 12278
	private bool triggerLaugh;

	// Token: 0x04002FF7 RID: 12279
	private bool fromLeft;

	// Token: 0x02000710 RID: 1808
	public enum TransitionState
	{
		// Token: 0x04002FF9 RID: 12281
		None,
		// Token: 0x04002FFA RID: 12282
		PlatformDestroyed,
		// Token: 0x04002FFB RID: 12283
		InStomach
	}
}
