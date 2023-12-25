using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020007FD RID: 2045
public class SnowCultLevelWizard : LevelProperties.SnowCult.Entity
{
	// Token: 0x1400004D RID: 77
	// (add) Token: 0x06002EF9 RID: 12025 RVA: 0x001BB41C File Offset: 0x001B981C
	// (remove) Token: 0x06002EFA RID: 12026 RVA: 0x001BB454 File Offset: 0x001B9854
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x06002EFB RID: 12027 RVA: 0x001BB48A File Offset: 0x001B988A
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002EFC RID: 12028 RVA: 0x001BB4C0 File Offset: 0x001B98C0
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002EFD RID: 12029 RVA: 0x001BB4D8 File Offset: 0x001B98D8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002EFE RID: 12030 RVA: 0x001BB4EC File Offset: 0x001B98EC
	public override void LevelInit(LevelProperties.SnowCult properties)
	{
		base.LevelInit(properties);
		this.state = SnowCultLevelWizard.States.Idle;
		this.wizardHesitationString = new PatternString(properties.CurrentState.wizard.wizardHesitationString, true, true);
		this.attackLocationString = new PatternString(properties.CurrentState.quadShot.attackLocationString, true, true);
		this.quadShotBallDelayString = new PatternString(properties.CurrentState.quadShot.ballDelayString, true, true);
		this.hazardDirectionString = new PatternString(properties.CurrentState.quadShot.hazardDirectionString, true, true);
		this.seriesShotCountString = new PatternString(properties.CurrentState.seriesShot.seriesShotCountString, true, true);
		this.seriesShotParryString = new PatternString(properties.CurrentState.seriesShot.parryString, true);
		this.quadShotBallDelayString.SetSubStringIndex(-1);
		this.hazardDirectionString.SetSubStringIndex(-1);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002EFF RID: 12031 RVA: 0x001BB5D9 File Offset: 0x001B99D9
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002F00 RID: 12032 RVA: 0x001BB5F7 File Offset: 0x001B99F7
	public void PlayerHitByWhale(GameObject hit, CollisionPhase phase)
	{
		this.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002F01 RID: 12033 RVA: 0x001BB604 File Offset: 0x001B9A04
	private IEnumerator intro_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		float t = 0f;
		Vector3 startPos = base.transform.position;
		Vector3 endPos = new Vector3(this.pivotPoint.position.x + 540f, this.pivotPoint.transform.position.y);
		base.animator.SetBool("Turn", true);
		while (t < 1f)
		{
			float easedT = EaseUtils.EaseInOutSine(0f, 1f, t);
			base.transform.position = new Vector3(Mathf.Lerp(startPos.x, endPos.x, easedT), EaseUtils.EaseInSine(startPos.y, endPos.y, easedT));
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		base.StartCoroutine(this.move_cr());
		yield break;
	}

	// Token: 0x06002F02 RID: 12034 RVA: 0x001BB61F File Offset: 0x001B9A1F
	private void AniEvent_StartTurn()
	{
		this.turnAnimationPlaying = true;
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x001BB628 File Offset: 0x001B9A28
	private void AniEvent_CompleteTurn()
	{
		if (!this.dead && this.turnAnimationPlaying)
		{
			bool flag = this.goingLeft;
			if (this.currentPosition > 0.9f)
			{
				flag = !flag;
			}
			base.transform.localScale = new Vector3((float)((!flag) ? 1 : -1), base.transform.localScale.y);
			this.turnAnimationPlaying = false;
		}
	}

	// Token: 0x06002F04 RID: 12036 RVA: 0x001BB6A0 File Offset: 0x001B9AA0
	private void AniEvent_AlignForOutro()
	{
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.position.x - Camera.main.transform.position.x), base.transform.localScale.y);
		this.outroWobbling = true;
	}

	// Token: 0x06002F05 RID: 12037 RVA: 0x001BB707 File Offset: 0x001B9B07
	public bool Turning()
	{
		return base.animator.GetBool("Turn");
	}

	// Token: 0x06002F06 RID: 12038 RVA: 0x001BB71C File Offset: 0x001B9B1C
	private IEnumerator move_cr()
	{
		this.goingLeft = true;
		LevelProperties.SnowCult.Movement p = base.properties.CurrentState.movement;
		float startAngle = 1.5707964f;
		float endAngle = -1.5707964f;
		float angle = endAngle;
		float loopSizeX = 540f;
		float loopSizeY = p.dipAmount;
		float loopSpeed = p.speed;
		float startSpeed = p.speed;
		float endSpeed = p.easing;
		bool easeIn = true;
		Vector3 handleRotationX = Vector3.zero;
		Vector3 handleRotationY = Vector3.zero;
		base.transform.SetPosition(new float?(this.pivotPoint.position.x + loopSizeX), null, null);
		float t = 1f;
		float time = 1f;
		this.isMoving = true;
		for (;;)
		{
			while (!this.isMoving)
			{
				yield return null;
			}
			angle += loopSpeed * CupheadTime.FixedDelta * this.postWhalePositionLerpTimer * ((!this.dead) ? 1f : 1.5f);
			if ((angle < endAngle && !this.goingLeft) || (angle > startAngle && this.goingLeft))
			{
				this.reachedApex = true;
				this.notReachedMid = true;
				loopSpeed = -loopSpeed;
				this.goingLeft = !this.goingLeft;
				t = 0f;
				startSpeed = ((!easeIn) ? ((!this.goingLeft) ? (-p.easing) : p.easing) : ((!this.goingLeft) ? (-p.speed) : p.speed));
				endSpeed = ((!easeIn) ? ((!this.goingLeft) ? (-p.speed) : p.speed) : ((!this.goingLeft) ? (-p.easing) : p.easing));
				easeIn = true;
			}
			else
			{
				this.reachedApex = false;
			}
			if ((angle > startAngle - 1.5f && this.goingLeft && easeIn) || (angle < endAngle + 1.5f && !this.goingLeft && easeIn))
			{
				t = 0f;
				startSpeed = ((!easeIn) ? ((!this.goingLeft) ? (-p.easing) : p.easing) : ((!this.goingLeft) ? (-p.speed) : p.speed));
				endSpeed = ((!easeIn) ? ((!this.goingLeft) ? (-p.speed) : p.speed) : ((!this.goingLeft) ? (-p.easing) : p.easing));
				easeIn = false;
			}
			if (((this.goingLeft && base.transform.position.x < 0f) || (!this.goingLeft && base.transform.position.x > 0f)) && this.notReachedMid)
			{
				this.notReachedMid = false;
			}
			if (t < time)
			{
				t += CupheadTime.FixedDelta;
				loopSpeed = Mathf.Lerp(startSpeed, endSpeed, t / time);
			}
			Vector3 handleRotation = new Vector3(-Mathf.Sin(angle) * loopSizeX, -Mathf.Cos(angle) * loopSizeY, 0f);
			Vector3 destinationPos = this.pivotPoint.position + handleRotation;
			this.lastPos = base.transform.position;
			base.transform.position = new Vector3(destinationPos.x, Mathf.Lerp(base.transform.position.y, destinationPos.y, this.postWhalePositionLerpTimer));
			this.postWhalePositionLerpTimer = Mathf.Clamp(this.postWhalePositionLerpTimer + CupheadTime.FixedDelta * 2.5f, 0f, 1f);
			this.currentPosition = Mathf.InverseLerp(startAngle, endAngle, angle);
			if (this.goingLeft)
			{
				this.currentPosition = 1f - this.currentPosition;
			}
			bool goingLeftOrientation = this.goingLeft;
			if (this.currentPosition > 0.9f)
			{
				goingLeftOrientation = !goingLeftOrientation;
			}
			if (!this.dead)
			{
				base.animator.SetBool("Turn", (int)base.transform.localScale.x != ((!goingLeftOrientation) ? 1 : -1) && !this.seriesShotActive);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002F07 RID: 12039 RVA: 0x001BB737 File Offset: 0x001B9B37
	public void StartQuadAttack()
	{
		base.StartCoroutine(this.quad_cr());
	}

	// Token: 0x06002F08 RID: 12040 RVA: 0x001BB748 File Offset: 0x001B9B48
	private IEnumerator quad_cr()
	{
		this.state = SnowCultLevelWizard.States.Quad;
		LevelProperties.SnowCult.QuadShot p = base.properties.CurrentState.quadShot;
		float targetPosX = this.attackLocationString.PopFloat();
		bool inAttackPos = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!inAttackPos)
		{
			if (this.dead)
			{
				yield break;
			}
			if (Mathf.Abs(targetPosX - base.transform.position.x) < p.distToAttack && !this.turnAnimationPlaying)
			{
				inAttackPos = true;
			}
			yield return wait;
		}
		int curIdleFrame = Mathf.RoundToInt(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 23f);
		if (curIdleFrame >= 14 && curIdleFrame <= 22)
		{
			base.animator.Play("QuadshotIntro", 0, 0.2857143f);
		}
		else if (curIdleFrame >= 2 && curIdleFrame <= 10)
		{
			base.animator.Play("QuadshotIntro", 0, 0.14285715f);
		}
		else
		{
			base.animator.Play("QuadshotIntro");
		}
		this.SFX_SNOWCULT_WizardQuadshotAttack();
		this.isMoving = false;
		List<SnowCultLevelQuadShot> quadShots = new List<SnowCultLevelQuadShot>();
		float downAmount = 0f;
		yield return CupheadTime.WaitForSeconds(this, p.preattackDelay);
		base.animator.Play("QuadshotContinue");
		yield return null;
		yield return base.animator.WaitForAnimationToEnd(this, "QuadshotContinue", false, true);
		this.quadshotMask.enabled = true;
		for (int i = 0; i < 4; i++)
		{
			downAmount = ((i <= 0 || i >= 3) ? 0f : p.distanceDown);
			Vector3 startPos = new Vector3(base.transform.position.x - p.distanceBetween * 0.8f * 2f + p.distanceBetween * 0.8f * 0.5f + p.distanceBetween * 0.8f * (float)i, base.transform.position.y - downAmount);
			Vector3 destPos = new Vector3(base.transform.position.x - p.distanceBetween * 2f + p.distanceBetween / 2f + p.distanceBetween * (float)i, base.transform.position.y - downAmount);
			SnowCultLevelQuadShot snowCultLevelQuadShot = this.quadShotProjectile.Spawn<SnowCultLevelQuadShot>();
			float delay = this.quadShotBallDelayString.PopFloat() / 4f * p.ballDelay;
			snowCultLevelQuadShot.Init(startPos, destPos, p.shotVelocity, this.hazardDirectionString.PopString(), p, i, delay, p.distanceBetween, PlayerManager.GetNext());
			quadShots.Add(snowCultLevelQuadShot);
		}
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		this.quadshotMask.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, p.attackDelay - 0.25f);
		base.animator.Play("QuadshotEnd");
		yield return null;
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.27272728f)
		{
			yield return null;
		}
		AbstractPlayerController player = PlayerManager.GetNext();
		float first = 1000f;
		float second = 1000f;
		SnowCultLevelQuadShot shotQuadChosen = null;
		SnowCultLevelQuadShot shotQuadChosen2 = null;
		for (int j = 0; j < 4; j++)
		{
			float num = Mathf.Abs(quadShots[j].transform.position.x - player.transform.position.x);
			if (num < first)
			{
				second = first;
				first = num;
				shotQuadChosen2 = shotQuadChosen;
				shotQuadChosen = quadShots[j];
			}
			else if (num < second && num != first)
			{
				second = num;
				shotQuadChosen2 = quadShots[j];
			}
		}
		float offset = UnityEngine.Random.Range(0f, p.maxOffset);
		offset = ((!Rand.Bool()) ? (-offset) : offset);
		SnowCultLevelQuadShot shotQuadChosen3 = (!Rand.Bool()) ? shotQuadChosen2 : shotQuadChosen;
		Vector3 endPos = new Vector3(player.transform.position.x, (float)Level.Current.Ground);
		Vector3 direction = endPos - shotQuadChosen3.transform.position;
		Vector3 finalDirection = new Vector3(direction.x + offset, direction.y);
		this.lineStartPos = shotQuadChosen3.transform.position;
		this.lineEndPos = new Vector3(player.transform.position.x + offset, endPos.y);
		bool startWithRight = Rand.Bool();
		int rightIndex = 3;
		for (int k = 0; k < 4; k++)
		{
			int index = (!startWithRight) ? k : (rightIndex - k);
			quadShots[index].Shoot(MathUtils.DirectionToAngle(finalDirection));
		}
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		this.isMoving = true;
		yield return CupheadTime.WaitForSeconds(this, this.wizardHesitationString.PopFloat());
		this.state = SnowCultLevelWizard.States.Idle;
		yield break;
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x001BB763 File Offset: 0x001B9B63
	public void Whale()
	{
		base.StartCoroutine(this.whale_cr());
	}

	// Token: 0x06002F0A RID: 12042 RVA: 0x001BB774 File Offset: 0x001B9B74
	private IEnumerator whale_cr()
	{
		this.state = SnowCultLevelWizard.States.Whale;
		LevelProperties.SnowCult.Whale p = base.properties.CurrentState.whale;
		this.dropAttackComplete = false;
		bool drop = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		AbstractPlayerController player = PlayerManager.GetNext();
		float lastPlayerOffset = base.transform.position.x - Mathf.Clamp(player.transform.position.x, -445f, 445f);
		while (!drop)
		{
			float playerClampedX = Mathf.Clamp(player.transform.position.x, -445f, 445f);
			if (Mathf.Abs(playerClampedX - base.transform.position.x) < p.distToDrop || Mathf.Sign(lastPlayerOffset) != Mathf.Sign(base.transform.position.x - playerClampedX))
			{
				drop = true;
			}
			lastPlayerOffset = base.transform.position.x - playerClampedX;
			yield return wait;
		}
		this.isMoving = false;
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		float currentAnimatorTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		base.animator.Play((currentAnimatorTime <= 0.083333336f || currentAnimatorTime >= 0.5833333f) ? "WhaleDrop_IntroAlt" : "WhaleDrop_Intro");
		this.SFX_SNOWCULT_WizardWhalesmashAttack();
		float t = 0f;
		float val = 0f;
		Vector3 startPos = base.transform.position;
		Vector3 endPos = new Vector3(startPos.x, 200f);
		while (t < 0.22f)
		{
			t += CupheadTime.Delta;
			val = Mathf.InverseLerp(0f, 0.22f, t);
			base.transform.position = Vector3.Lerp(startPos, endPos, EaseUtils.EaseInSine(0f, 1f, val));
			yield return null;
		}
		base.transform.position = endPos;
		t = 0f;
		while (t < p.attackDelay)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("DropWhale");
		while (!this.dropAttackComplete)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.083333336f);
		this.postWhalePositionLerpTimer = 0f;
		this.isMoving = true;
		yield return CupheadTime.WaitForSeconds(this, p.recoveryDelay);
		yield return CupheadTime.WaitForSeconds(this, this.wizardHesitationString.PopFloat());
		this.state = SnowCultLevelWizard.States.Idle;
		yield break;
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x001BB78F File Offset: 0x001B9B8F
	private void WhaleAttackImpact()
	{
		CupheadLevelCamera.Current.Shake(55f, 0.5f, false);
	}

	// Token: 0x06002F0C RID: 12044 RVA: 0x001BB7A8 File Offset: 0x001B9BA8
	private void WhaleAttackComplete()
	{
		this.whaleDropFX.transform.position = new Vector3(base.transform.position.x, this.whaleDropFX.transform.position.y);
		this.whaleDropFX.gameObject.SetActive(true);
		this.whaleDropFX.Play("Main");
		this.dropAttackComplete = true;
	}

	// Token: 0x06002F0D RID: 12045 RVA: 0x001BB81D File Offset: 0x001B9C1D
	public void SeriesShot()
	{
		base.StartCoroutine(this.series_shot_cr());
	}

	// Token: 0x06002F0E RID: 12046 RVA: 0x001BB82C File Offset: 0x001B9C2C
	private IEnumerator series_shot_cr()
	{
		this.seriesShotCanExit = false;
		this.seriesShotActive = true;
		this.state = SnowCultLevelWizard.States.SeriesShot;
		LevelProperties.SnowCult.SeriesShot p = base.properties.CurrentState.seriesShot;
		int shotCount = this.seriesShotCountString.PopInt();
		float t = 0f;
		base.animator.SetTrigger("StartPeashot");
		yield return base.animator.WaitForAnimationToStart(this, "Peashot_Intro", false);
		this.table.Intro(base.transform.position - this.lastPos);
		for (int i = 0; i < shotCount; i++)
		{
			while (t < p.seriesShotWarningTime && !this.dead)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			if (!this.dead)
			{
				base.animator.SetTrigger("OnShoot");
				while (!this.seriesShotFired)
				{
					yield return null;
				}
				this.SFX_SNOWCULT_WizardTarotCardAttackLaunch();
				this.seriesShotFired = false;
			}
			t = 0f;
			while (t < p.betweenShotDelay && !this.dead)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			if (this.dead)
			{
				break;
			}
		}
		this.seriesShotCanExit = true;
		while (this.seriesShotActive)
		{
			yield return null;
		}
		if (!this.dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.wizardHesitationString.PopFloat());
		}
		this.state = SnowCultLevelWizard.States.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002F0F RID: 12047 RVA: 0x001BB847 File Offset: 0x001B9C47
	private void CreatePeashot()
	{
		base.StartCoroutine(this.create_peashot());
	}

	// Token: 0x06002F10 RID: 12048 RVA: 0x001BB858 File Offset: 0x001B9C58
	private IEnumerator create_peashot()
	{
		this.shootFX.Play("ShootFX");
		this.shootFX.Update(0f);
		yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		AbstractPlayerController player = PlayerManager.GetNext();
		Vector3 dir = player.transform.position - this.shootFX.transform.position;
		BasicProjectile proj = this.seriesShot.Create(this.shootFX.transform.position, MathUtils.DirectionToAngle(dir) + 90f, base.properties.CurrentState.seriesShot.bulletSpeed);
		proj.transform.position += dir.normalized * 25f;
		proj.SetParryable(this.seriesShotParryString.PopLetter() == 'P');
		this.seriesShotFired = true;
		while (this.shootFX.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
		{
			Effect sparkle = this.cardSparkle.Create(this.shootFX.transform.position + MathUtils.AngleToDirection((float)UnityEngine.Random.Range(0, 360)) * this.shootFX.GetCurrentAnimatorStateInfo(0).normalizedTime * 200f);
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.005f, 0.01f));
		}
		yield break;
	}

	// Token: 0x06002F11 RID: 12049 RVA: 0x001BB873 File Offset: 0x001B9C73
	private void CanExitPeashotLoop()
	{
		if (this.seriesShotCanExit)
		{
			base.animator.Play("Peashot_Outro_A");
			this.table.Outro();
		}
	}

	// Token: 0x06002F12 RID: 12050 RVA: 0x001BB89B File Offset: 0x001B9C9B
	private void EndPeashotLoop()
	{
		this.seriesShotActive = false;
	}

	// Token: 0x06002F13 RID: 12051 RVA: 0x001BB8A4 File Offset: 0x001B9CA4
	public void ToOutro(SnowCultLevelYeti yeti)
	{
		this.dead = true;
		base.StartCoroutine(this.outro_cr(yeti));
	}

	// Token: 0x06002F14 RID: 12052 RVA: 0x001BB8BB File Offset: 0x001B9CBB
	private void AniEvent_CultistsSummon()
	{
		((SnowCultLevel)Level.Current).CultistsSummon();
	}

	// Token: 0x06002F15 RID: 12053 RVA: 0x001BB8CC File Offset: 0x001B9CCC
	private IEnumerator outro_cr(SnowCultLevelYeti yeti)
	{
		while (!this.reachedApex)
		{
			yield return null;
		}
		if (base.transform.localScale.x != Mathf.Sign(base.transform.position.x - Camera.main.transform.position.x))
		{
			base.animator.SetBool("Turn", true);
		}
		this.state = SnowCultLevelWizard.States.Idle;
		this.isMoving = false;
		base.animator.SetTrigger("OnOutro");
		float t = 0f;
		Vector3 startPos = base.transform.position;
		if (base.transform.position.x < this.pivotPoint.position.x)
		{
			this.outroPos.position = new Vector3(this.pivotPoint.position.x + (this.pivotPoint.position.x - this.outroPos.position.x), this.outroPos.position.y);
			yeti.StartOnLeft(this.pivotPoint.position);
		}
		while (t < 0.5f)
		{
			base.transform.position = new Vector3(EaseUtils.EaseOutSine(startPos.x, this.outroPos.position.x, t * 2f), EaseUtils.EaseOutBack(startPos.y, this.outroPos.position.y, t * 2f));
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != Animator.StringToHash(base.animator.GetLayerName(0) + ".OutroLoop"))
		{
			base.transform.position = this.outroPos.position;
			yield return null;
		}
		yeti.gameObject.SetActive(true);
		yeti.StartYeti();
		while (!yeti.introRibcageClosed)
		{
			base.transform.position = this.outroPos.position;
			yield return null;
		}
		this.OnDeath();
		yield break;
	}

	// Token: 0x06002F16 RID: 12054 RVA: 0x001BB8F0 File Offset: 0x001B9CF0
	private void LateUpdate()
	{
		if (!this.outroWobbling)
		{
			return;
		}
		this.outroWobbleTime += CupheadTime.FixedDelta * 1.5f;
		base.transform.position += new Vector3(Mathf.Sin(this.outroWobbleTime * 3f) * 1f, Mathf.Cos(this.outroWobbleTime * 2f) * 5f);
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x001BB96A File Offset: 0x001B9D6A
	public void OnDeath()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002F18 RID: 12056 RVA: 0x001BB993 File Offset: 0x001B9D93
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(this.lineStartPos, this.lineEndPos);
	}

	// Token: 0x06002F19 RID: 12057 RVA: 0x001BB9AC File Offset: 0x001B9DAC
	private void AnimationEvent_SFX_SNOWCULT_WizardIntro()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_intro");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_intro");
	}

	// Token: 0x06002F1A RID: 12058 RVA: 0x001BB9C8 File Offset: 0x001B9DC8
	private void AnimationEvent_SFX_SNOWCULT_WizardQuadshot_Attack()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_quadshot_attack");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_quadshot_attack");
	}

	// Token: 0x06002F1B RID: 12059 RVA: 0x001BB9E4 File Offset: 0x001B9DE4
	private void SFX_SNOWCULT_WizardWhalesmashAttack()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_whalesmash_attack");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_whalesmash_attack");
	}

	// Token: 0x06002F1C RID: 12060 RVA: 0x001BBA00 File Offset: 0x001B9E00
	private void SFX_SNOWCULT_WizardTarotCardAttackLaunch()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_tarotcardattack_launch");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_tarotcardattack_launch");
	}

	// Token: 0x06002F1D RID: 12061 RVA: 0x001BBA1C File Offset: 0x001B9E1C
	private void SFX_SNOWCULT_WizardQuadshotAttack()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_quadshot_attack");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_quadshot_attack");
	}

	// Token: 0x06002F1E RID: 12062 RVA: 0x001BBA38 File Offset: 0x001B9E38
	private void AnimationEvent_SFX_SNOWCULT_WizardYetiIntroBellComesToLife()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_intro_bell_comestolife");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_intro_bell_comestolife");
	}

	// Token: 0x06002F1F RID: 12063 RVA: 0x001BBA54 File Offset: 0x001B9E54
	private void AnimationEvent_SFX_SNOWCULT_WizardVoiceEffortLarge()
	{
		AudioManager.Stop("sfx_dlc_snowcult_wizard_voice_laugh");
		AudioManager.Play("sfx_dlc_snowcult_wizard_voice_effort_large");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_wizard_voice_effort_large");
	}

	// Token: 0x06002F20 RID: 12064 RVA: 0x001BBA7A File Offset: 0x001B9E7A
	private void AnimationEvent_SFX_SNOWCULT_WizardVoiceLaugh()
	{
		AudioManager.Stop("sfx_dlc_snowcult_wizard_voice_effort_large");
		AudioManager.Stop("sfx_dlc_snowcult_wizard_voice_laugh");
		AudioManager.Play("sfx_dlc_snowcult_wizard_voice_laugh");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_wizard_voice_laugh");
	}

	// Token: 0x06002F21 RID: 12065 RVA: 0x001BBAAA File Offset: 0x001B9EAA
	private void AnimationEvent_SFX_SNOWCULT_WizardVoiceWhee()
	{
		AudioManager.Play("sfx_dlc_snowcult_wizard_voice_whee");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_wizard_voice_whee");
	}

	// Token: 0x040037AE RID: 14254
	public SnowCultLevelWizard.States state;

	// Token: 0x040037B0 RID: 14256
	private const int NUM_OF_SLAM_SLOTS = 4;

	// Token: 0x040037B1 RID: 14257
	private const int NUM_OF_QUAD_SHOTS = 4;

	// Token: 0x040037B2 RID: 14258
	private const float QUAD_SHOT_START_SPACING_MULTIPLIER = 0.8f;

	// Token: 0x040037B3 RID: 14259
	private const float WIZARD_SLAM_OFFSET = 230f;

	// Token: 0x040037B4 RID: 14260
	private const float WHALE_ATTACK_HEIGHT = 200f;

	// Token: 0x040037B5 RID: 14261
	private const float WHALE_ATTACK_MOVE_DELAY = 0.22f;

	// Token: 0x040037B6 RID: 14262
	private const float WHALE_POSTATTACK_MOVE_DELAY = 0.4f;

	// Token: 0x040037B7 RID: 14263
	private const float WHALE_RANGE = 195f;

	// Token: 0x040037B8 RID: 14264
	private DamageDealer damageDealer;

	// Token: 0x040037B9 RID: 14265
	private DamageReceiver damageReceiver;

	// Token: 0x040037BA RID: 14266
	[SerializeField]
	private BasicProjectile seriesShot;

	// Token: 0x040037BB RID: 14267
	[SerializeField]
	private Animator whaleDropFX;

	// Token: 0x040037BC RID: 14268
	[SerializeField]
	private SnowCultLevelTable table;

	// Token: 0x040037BD RID: 14269
	[SerializeField]
	private Animator shootFX;

	// Token: 0x040037BE RID: 14270
	[SerializeField]
	private SnowCultLevelQuadShot quadShotProjectile;

	// Token: 0x040037BF RID: 14271
	[SerializeField]
	private Transform pivotPoint;

	// Token: 0x040037C0 RID: 14272
	[SerializeField]
	private Transform outroPos;

	// Token: 0x040037C1 RID: 14273
	[SerializeField]
	private SpriteMask quadshotMask;

	// Token: 0x040037C2 RID: 14274
	[SerializeField]
	private Effect cardSparkle;

	// Token: 0x040037C3 RID: 14275
	[SerializeField]
	private SpriteRenderer introWizRend;

	// Token: 0x040037C4 RID: 14276
	private Vector3 lineStartPos;

	// Token: 0x040037C5 RID: 14277
	private Vector3 lineEndPos;

	// Token: 0x040037C6 RID: 14278
	private bool goingLeft;

	// Token: 0x040037C7 RID: 14279
	private bool isMoving;

	// Token: 0x040037C8 RID: 14280
	private bool reachedApex;

	// Token: 0x040037C9 RID: 14281
	private bool notReachedMid;

	// Token: 0x040037CA RID: 14282
	private Vector3 lastPos = Vector3.zero;

	// Token: 0x040037CB RID: 14283
	private PatternString wizardHesitationString;

	// Token: 0x040037CC RID: 14284
	private PatternString attackLocationString;

	// Token: 0x040037CD RID: 14285
	private PatternString hazardDirectionString;

	// Token: 0x040037CE RID: 14286
	private PatternString iceSummonString;

	// Token: 0x040037CF RID: 14287
	private PatternString seriesShotCountString;

	// Token: 0x040037D0 RID: 14288
	private PatternString quadShotBallDelayString;

	// Token: 0x040037D1 RID: 14289
	private bool seriesShotFired;

	// Token: 0x040037D2 RID: 14290
	private bool seriesShotCanExit = true;

	// Token: 0x040037D3 RID: 14291
	private bool seriesShotActive;

	// Token: 0x040037D4 RID: 14292
	private PatternString seriesShotParryString;

	// Token: 0x040037D5 RID: 14293
	private bool dropAttackComplete;

	// Token: 0x040037D6 RID: 14294
	private float postWhalePositionLerpTimer = 1f;

	// Token: 0x040037D7 RID: 14295
	public bool dead;

	// Token: 0x040037D8 RID: 14296
	private bool turnAnimationPlaying;

	// Token: 0x040037D9 RID: 14297
	private float currentPosition = 1f;

	// Token: 0x040037DA RID: 14298
	private bool outroWobbling;

	// Token: 0x040037DB RID: 14299
	private float outroWobbleTime;

	// Token: 0x020007FE RID: 2046
	public enum States
	{
		// Token: 0x040037DD RID: 14301
		Idle,
		// Token: 0x040037DE RID: 14302
		Quad,
		// Token: 0x040037DF RID: 14303
		Whale,
		// Token: 0x040037E0 RID: 14304
		Slam,
		// Token: 0x040037E1 RID: 14305
		Wind,
		// Token: 0x040037E2 RID: 14306
		SeriesShot
	}
}
