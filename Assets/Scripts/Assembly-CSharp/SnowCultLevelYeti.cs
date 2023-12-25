using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020007FF RID: 2047
public class SnowCultLevelYeti : LevelProperties.SnowCult.Entity
{
	// Token: 0x1400004E RID: 78
	// (add) Token: 0x06002F23 RID: 12067 RVA: 0x001BDC3C File Offset: 0x001BC03C
	// (remove) Token: 0x06002F24 RID: 12068 RVA: 0x001BDC74 File Offset: 0x001BC074
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06002F25 RID: 12069 RVA: 0x001BDCAA File Offset: 0x001BC0AA
	// (set) Token: 0x06002F26 RID: 12070 RVA: 0x001BDCB2 File Offset: 0x001BC0B2
	public bool inBallForm { get; private set; }

	// Token: 0x06002F27 RID: 12071 RVA: 0x001BDCBC File Offset: 0x001BC0BC
	protected override void Awake()
	{
		base.Awake();
		this.xScale = 1f;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.ballDamageReceiver = this.ball.GetComponent<DamageReceiver>();
		this.ballDamageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.idleAnimFullPathHash = Animator.StringToHash(base.animator.GetLayerName(0) + ".Idle");
		if (Level.Current.mode != Level.Mode.Easy)
		{
			this.InitBats();
		}
		else
		{
			base.properties.OnBossDeath += this.OnBossDeath;
		}
		this.ball.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x06002F28 RID: 12072 RVA: 0x001BDD9F File Offset: 0x001BC19F
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002F29 RID: 12073 RVA: 0x001BDDB8 File Offset: 0x001BC1B8
	private bool InIdleAnim()
	{
		return base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == this.idleAnimFullPathHash;
	}

	// Token: 0x06002F2A RID: 12074 RVA: 0x001BDDE4 File Offset: 0x001BC1E4
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (Level.Current.mode == Level.Mode.Easy && base.properties.CurrentState.stateName == LevelProperties.SnowCult.States.EasyYeti && !this.InIdleAnim() && info.damage >= base.properties.CurrentHealth)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002F2B RID: 12075 RVA: 0x001BDE49 File Offset: 0x001BC249
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002F2C RID: 12076 RVA: 0x001BDE68 File Offset: 0x001BC268
	public override void LevelInit(LevelProperties.SnowCult properties)
	{
		base.LevelInit(properties);
		this.offsetCoordIndex = UnityEngine.Random.Range(0, properties.CurrentState.icePillar.offsetCoordString.Split(new char[]
		{
			','
		}).Length);
		this.snowballMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.snowball.snowballTypeString.Length);
	}

	// Token: 0x06002F2D RID: 12077 RVA: 0x001BDEC8 File Offset: 0x001BC2C8
	public void StartOnLeft(Vector3 reflectionPoint)
	{
		this.yetiSpawnPoint.position = new Vector3(reflectionPoint.x + (reflectionPoint.x - this.yetiSpawnPoint.position.x), this.yetiSpawnPoint.position.y);
		base.transform.localScale = new Vector3(-1f, 1f);
		this.xScale = base.transform.localScale.x;
		this.onLeft = true;
	}

	// Token: 0x06002F2E RID: 12078 RVA: 0x001BDF55 File Offset: 0x001BC355
	public void StartYeti()
	{
		base.StartCoroutine(this.intro_cr());
		if (Level.Current.mode != Level.Mode.Easy)
		{
			base.StartCoroutine(this.bats_attack_cr());
		}
	}

	// Token: 0x06002F2F RID: 12079 RVA: 0x001BDF80 File Offset: 0x001BC380
	private void SetState(SnowCultLevelYeti.States s)
	{
		this.previousState = this.state;
		this.state = s;
	}

	// Token: 0x06002F30 RID: 12080 RVA: 0x001BDF98 File Offset: 0x001BC398
	private IEnumerator intro_cr()
	{
		this.state = SnowCultLevelYeti.States.Intro;
		this.introRibcageClosed = false;
		base.transform.position = Vector3.zero;
		base.transform.position = this.yetiSpawnPoint.position;
		base.transform.position += Vector3.up * 300f;
		float t = 0f;
		this.sprite = base.GetComponent<SpriteRenderer>();
		base.animator.Play("Intro", 0, 0f);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(base.animator.GetLayerName(0) + ".Intro"))
		{
			if (t < 0.34f)
			{
				base.transform.position = new Vector3(base.transform.position.x, EaseUtils.EaseOutBack(this.yetiSpawnPoint.position.y + 300f, this.yetiSpawnPoint.position.y, t * 3f));
				t += CupheadTime.FixedDelta;
			}
			this.introShadow.transform.position = new Vector3(base.transform.position.x, -25f);
			yield return wait;
		}
		this.SetState(SnowCultLevelYeti.States.Idle);
		base.StartCoroutine(this.do_patterns_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002F31 RID: 12081 RVA: 0x001BDFB3 File Offset: 0x001BC3B3
	private void ShakeScreenInIntro()
	{
		CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
		((SnowCultLevel)Level.Current).YetiHitGround();
	}

	// Token: 0x06002F32 RID: 12082 RVA: 0x001BDFD9 File Offset: 0x001BC3D9
	public void RibcageClosedAroundWizard()
	{
		this.introRibcageClosed = true;
	}

	// Token: 0x06002F33 RID: 12083 RVA: 0x001BDFE4 File Offset: 0x001BC3E4
	private void FlipSprite()
	{
		base.transform.SetScale(new float?((!this.onLeft) ? this.xScale : (-this.xScale)), null, null);
	}

	// Token: 0x06002F34 RID: 12084 RVA: 0x001BE030 File Offset: 0x001BC430
	private IEnumerator do_patterns_cr()
	{
		LevelProperties.SnowCult.Yeti p = base.properties.CurrentState.yeti;
		this.patternString = p.yetiPatternString.Split(new char[]
		{
			','
		});
		this.patternStringIndex = UnityEngine.Random.Range(0, this.patternString.Length);
		while (!this.forceOutroToStart)
		{
			string text = this.patternString[this.patternStringIndex];
			if (text == null)
			{
				goto IL_144;
			}
			if (!(text == "S"))
			{
				if (!(text == "J"))
				{
					if (!(text == "L"))
					{
						if (!(text == "P"))
						{
							goto IL_144;
						}
						this.StartIcePillar();
					}
					else
					{
						this.Snowball();
					}
				}
				else
				{
					base.StartCoroutine(this.start_jump_cr());
				}
			}
			else
			{
				base.StartCoroutine(this.start_dash_cr());
			}
			IL_154:
			while (this.state != SnowCultLevelYeti.States.Idle)
			{
				yield return null;
			}
			this.patternStringIndex = (this.patternStringIndex + 1) % this.patternString.Length;
			yield return null;
			continue;
			IL_144:
			this.Snowball();
			goto IL_154;
		}
		yield break;
	}

	// Token: 0x06002F35 RID: 12085 RVA: 0x001BE04C File Offset: 0x001BC44C
	private string PeekNextPattern()
	{
		if (this.forceOutroToStart || Level.Current.mode == Level.Mode.Easy)
		{
			return "I";
		}
		string text = this.patternString[(this.patternStringIndex + 1) % this.patternString.Length];
		if (text != null)
		{
			if (text == "S" || text == "J")
			{
				return this.patternString[(this.patternStringIndex + 1) % this.patternString.Length];
			}
			if (text == "L" || text == "P")
			{
				return "I";
			}
		}
		return null;
	}

	// Token: 0x06002F36 RID: 12086 RVA: 0x001BE100 File Offset: 0x001BC500
	private IEnumerator cue_reform_effect_cr(float delayTime, float position, string clipName)
	{
		yield return CupheadTime.WaitForSeconds(this, delayTime - 0.9583333f);
		this.meltFXAnimator[1].gameObject.transform.SetPosition(new float?(position), null, null);
		this.meltFXAnimator[1].gameObject.transform.localScale = new Vector3(base.transform.localScale.x * -1f, 1f);
		this.meltFXAnimator[1].gameObject.SetActive(true);
		this.meltFXAnimator[1].Play(clipName);
		yield break;
	}

	// Token: 0x06002F37 RID: 12087 RVA: 0x001BE130 File Offset: 0x001BC530
	private IEnumerator start_dash_cr()
	{
		this.inBallForm = true;
		float PRE_DASH_TIME = 0.25f;
		float DASH_TIME = 0.375f;
		LevelProperties.SnowCult.Yeti p = base.properties.CurrentState.yeti;
		float start = (!this.onLeft) ? 493f : -493f;
		float end = (!this.onLeft) ? -493f : 493f;
		start += this.ball.transform.localPosition.x * -base.transform.localScale.x * 2f;
		float t = 0f;
		float time = p.slideTime;
		if (this.previousState != SnowCultLevelYeti.States.Move || Level.Current.mode == Level.Mode.Easy)
		{
			base.animator.Play("IdleToDash");
		}
		this.SetState(SnowCultLevelYeti.States.Move);
		YieldInstruction wait = new WaitForFixedUpdate();
		yield return base.animator.WaitForAnimationToStart(this, "PreDash", false);
		base.StartCoroutine(this.cue_reform_effect_cr(PRE_DASH_TIME + p.slideWarning + DASH_TIME + time, end, "DashReformEffect"));
		yield return base.animator.WaitForAnimationToEnd(this, "PreDash", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.slideWarning);
		base.animator.Play("Dash");
		yield return base.animator.WaitForAnimationToEnd(this, "Dash", false, true);
		this.meltFXAnimator[0].transform.position = base.transform.position;
		this.meltFXAnimator[0].transform.localScale = base.transform.localScale;
		this.meltFXAnimator[0].gameObject.SetActive(true);
		this.meltFXAnimator[0].Play("DashMeltEffect");
		this.meltFXAnimator[0].transform.parent = null;
		yield return null;
		base.transform.SetPosition(new float?(start), null, null);
		this.ball.transform.localPosition = this.BALL_DASH_OFFSET;
		this.ball.SetActive(true);
		this.dashGroundFX.SetActive(true);
		this.groundMask.SetActive(true);
		this.sprite.enabled = false;
		this.coll.enabled = false;
		base.animator.Play("DashBall", 1, 0f);
		while (t < time)
		{
			if (t < time - 0.9583333f && t + CupheadTime.FixedDelta >= time - 0.9583333f)
			{
				this.meltFXAnimator[1].gameObject.transform.SetPosition(new float?(end), null, null);
				this.meltFXAnimator[1].gameObject.transform.localScale = new Vector3(base.transform.localScale.x * -1f, 1f);
				this.meltFXAnimator[1].gameObject.SetActive(true);
				this.meltFXAnimator[1].Play("DashReformEffect");
			}
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Lerp(start, end, t / time)), null, null);
			yield return wait;
		}
		this.onLeft = !this.onLeft;
		this.FlipSprite();
		this.sprite.enabled = true;
		this.coll.enabled = true;
		this.ball.SetActive(false);
		this.dashGroundFX.SetActive(false);
		this.groundMask.SetActive(false);
		this.meltFXAnimator[1].gameObject.SetActive(false);
		string text = this.PeekNextPattern();
		if (text != null)
		{
			if (!(text == "I"))
			{
				if (!(text == "S"))
				{
					if (text == "J")
					{
						base.animator.Play("DashToJump");
						yield return base.animator.WaitForAnimationToEnd(this, "DashToJump", false, true);
					}
				}
				else
				{
					base.animator.Play("DashToDash");
					yield return base.animator.WaitForAnimationToEnd(this, "DashToDash", false, true);
				}
			}
			else
			{
				base.animator.Play("DashToIdle");
				yield return base.animator.WaitForAnimationToEnd(this, "DashToIdle", false, true);
			}
		}
		if (this.PeekNextPattern() == "I")
		{
			yield return CupheadTime.WaitForSeconds(this, (!this.forceOutroToStart) ? p.hesitate : 0f);
		}
		this.SetState(SnowCultLevelYeti.States.Idle);
		this.inBallForm = false;
		yield break;
	}

	// Token: 0x06002F38 RID: 12088 RVA: 0x001BE14C File Offset: 0x001BC54C
	private IEnumerator start_jump_cr()
	{
		this.inBallForm = true;
		LevelProperties.SnowCult.Yeti p = base.properties.CurrentState.yeti;
		float PRE_JUMP_TIME = 0.20833333f;
		float JUMP_TIME = 0.25f;
		float endArcPosX = (!this.onLeft) ? -393f : 393f;
		float reformPosX = (!this.onLeft) ? -493f : 493f;
		float xDistance = endArcPosX - base.transform.position.x;
		float ground = base.transform.position.y;
		float timeToApex = p.jumpApexTime;
		float height = p.jumpApexHeight;
		float apexTime2 = timeToApex * timeToApex;
		float g = -2f * height / apexTime2;
		float viY = 2f * height / timeToApex;
		float viX2 = viY * viY;
		float sqrtRooted = viX2 + 2f * g * ground;
		float tEnd = (-viY + Mathf.Sqrt(sqrtRooted)) / g;
		float tEnd2 = (-viY - Mathf.Sqrt(sqrtRooted)) / g;
		float tEnd3 = Mathf.Max(tEnd, tEnd2);
		float velocityX = xDistance / tEnd3;
		Vector3 speed = new Vector3(velocityX, viY);
		float t = 0f;
		if (this.previousState != SnowCultLevelYeti.States.Move || Level.Current.mode == Level.Mode.Easy)
		{
			base.animator.Play("IdleToJump");
		}
		this.SetState(SnowCultLevelYeti.States.Move);
		yield return base.animator.WaitForAnimationToStart(this, "PreJump", false);
		base.StartCoroutine(this.cue_reform_effect_cr(PRE_JUMP_TIME + p.jumpWarning + JUMP_TIME + tEnd3, reformPosX, "JumpReformEffect"));
		yield return base.animator.WaitForAnimationToEnd(this, "PreJump", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.jumpWarning);
		base.animator.Play("Jump");
		this.ball.transform.localPosition = this.BALL_JUMP_OFFSET;
		yield return base.animator.WaitForAnimationToEnd(this, "Jump", false, true);
		this.meltFXAnimator[0].transform.position = base.transform.position;
		this.meltFXAnimator[0].transform.localScale = base.transform.localScale;
		this.meltFXAnimator[0].gameObject.SetActive(true);
		this.meltFXAnimator[0].Play("JumpMeltEffect");
		this.meltFXAnimator[0].transform.parent = null;
		base.transform.position += Vector3.right * (this.ball.transform.localPosition.x * -base.transform.localScale.x * 2f);
		this.ball.SetActive(true);
		this.ballShadow.sprite = this.shadowSprites[0];
		this.ballShadow.enabled = true;
		this.sprite.enabled = false;
		this.coll.enabled = false;
		base.animator.Play("JumpBall", 1, 0f);
		bool stillMoving = true;
		while (stillMoving)
		{
			speed += new Vector3(0f, g * CupheadTime.FixedDelta);
			base.transform.Translate(speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
			this.ballShadow.transform.SetPosition(new float?(this.ball.transform.position.x), new float?(ground - 145f), null);
			this.ballShadow.sprite = this.shadowSprites[Mathf.Clamp((int)((base.transform.position.y - ground) / height * (float)this.shadowSprites.Length), 0, this.shadowSprites.Length - 1)];
			t += CupheadTime.FixedDelta;
			if (t > timeToApex && base.transform.position.y <= ground)
			{
				stillMoving = false;
			}
		}
		base.transform.SetPosition(new float?(reformPosX), new float?(ground), null);
		base.transform.SetEulerAngles(null, null, new float?(0f));
		this.onLeft = !this.onLeft;
		this.FlipSprite();
		this.sprite.enabled = true;
		this.coll.enabled = true;
		this.ballShadow.enabled = false;
		this.ball.SetActive(false);
		this.meltFXAnimator[1].gameObject.SetActive(false);
		string text = this.PeekNextPattern();
		if (text != null)
		{
			if (!(text == "I"))
			{
				if (!(text == "S"))
				{
					if (text == "J")
					{
						base.animator.Play("JumpToJump");
						yield return base.animator.WaitForAnimationToEnd(this, "JumpToJump", false, true);
					}
				}
				else
				{
					base.animator.Play("JumpToDash");
					yield return base.animator.WaitForAnimationToEnd(this, "JumpToDash", false, true);
				}
			}
			else
			{
				base.animator.Play("JumpToIdle");
				yield return base.animator.WaitForAnimationToEnd(this, "JumpToIdle", false, true);
			}
		}
		if (this.PeekNextPattern() == "I")
		{
			yield return CupheadTime.WaitForSeconds(this, (!this.forceOutroToStart) ? p.hesitate : 0f);
		}
		this.SetState(SnowCultLevelYeti.States.Idle);
		this.inBallForm = false;
		yield break;
	}

	// Token: 0x06002F39 RID: 12089 RVA: 0x001BE167 File Offset: 0x001BC567
	public void StartIcePillar()
	{
		this.SetState(SnowCultLevelYeti.States.IcePillar);
		base.animator.SetTrigger("OnSmash");
	}

	// Token: 0x06002F3A RID: 12090 RVA: 0x001BE180 File Offset: 0x001BC580
	private void SpawnIcePillars()
	{
		CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
		this.snowCultBGHandler.CandleGust();
		Vector3 pos = new Vector3(base.transform.position.x + 290f * (float)((!this.onLeft) ? -1 : 1), 95f);
		this.snowBurstA.Create(pos, (float)((!this.onLeft) ? -1 : 1));
		base.StartCoroutine(this.spawn_snowfall_cr());
		base.StartCoroutine(this.ice_pillar_cr());
	}

	// Token: 0x06002F3B RID: 12091 RVA: 0x001BE220 File Offset: 0x001BC620
	private IEnumerator ice_pillar_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		LevelProperties.SnowCult.IcePillar p = base.properties.CurrentState.icePillar;
		float offset = 0f;
		int dir = (!this.onLeft) ? -1 : 1;
		bool type = Rand.Bool();
		Parser.FloatTryParse(p.offsetCoordString.Split(new char[]
		{
			','
		})[this.offsetCoordIndex], out offset);
		for (int i = 0; i < p.icePillarCount; i++)
		{
			Vector3 pos = new Vector3(this.yetiMidPoint.position.x + offset * (float)dir + p.icePillarSpacing * (float)i * (float)dir, -142f);
			SnowCultLevelIcePillar icePillar = this.icePillarPrefab.Spawn<SnowCultLevelIcePillar>();
			icePillar.Init(pos, p, type, p.appearDelay * (float)(i + 1));
			type = !type;
			yield return CupheadTime.WaitForSeconds(this, p.appearDelay);
		}
		this.offsetCoordIndex = (this.offsetCoordIndex + 1) % p.offsetCoordString.Split(new char[]
		{
			','
		}).Length;
		yield return CupheadTime.WaitForSeconds(this, (!this.forceOutroToStart) ? p.hesitate : 0f);
		this.SetState(SnowCultLevelYeti.States.Idle);
		yield return null;
		yield break;
	}

	// Token: 0x06002F3C RID: 12092 RVA: 0x001BE23C File Offset: 0x001BC63C
	private IEnumerator spawn_snowfall_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		Vector3 pos = new Vector3(base.transform.position.x + 290f * (float)((!this.onLeft) ? -1 : 1), 510f);
		this.snowFallA.Create(pos, (float)((!this.onLeft) ? -1 : 1));
		yield return null;
		yield break;
	}

	// Token: 0x06002F3D RID: 12093 RVA: 0x001BE258 File Offset: 0x001BC658
	private void InitBats()
	{
		this.batAttackPositionString = new PatternString(base.properties.CurrentState.snowball.batAttackPosition, true);
		this.batAttackHeightString = new PatternString(base.properties.CurrentState.snowball.batAttackHeight, true);
		this.batAttackWidthString = new PatternString(base.properties.CurrentState.snowball.batAttackWidth, true);
		this.batAttackSideString = new PatternString(base.properties.CurrentState.snowball.batAttackSide, true);
		this.batAttackInterDelayString = new PatternString(base.properties.CurrentState.snowball.batAttackInterDelay, true);
		this.batArcModifierString = new PatternString(base.properties.CurrentState.snowball.batArcModifier, true);
		this.batParryableString = new PatternString(base.properties.CurrentState.snowball.batParryableString, true);
	}

	// Token: 0x06002F3E RID: 12094 RVA: 0x001BE34C File Offset: 0x001BC74C
	private IEnumerator bats_attack_cr()
	{
		LevelProperties.SnowCult.Snowball p = base.properties.CurrentState.snowball;
		this.batLaunchTimer = this.batAttackInterDelayString.PopFloat();
		AbstractPlayerController player = PlayerManager.GetNext();
		for (;;)
		{
			while (this.batCirclingList.Count > 0)
			{
				int which = UnityEngine.Random.Range(0, this.batCirclingList.Count);
				if (this.batCirclingList[which] != null && this.batCirclingList[which].reachedCircle)
				{
					while (this.batLaunchTimer > 0f)
					{
						this.batLaunchTimer -= CupheadTime.Delta;
						yield return null;
					}
					if (this.batCirclingList.Count > which && this.batCirclingList[which] != null)
					{
						this.batLaunchTimer = this.batAttackInterDelayString.PopFloat();
						float height = this.batAttackHeightString.PopFloat();
						float num = this.batAttackWidthString.PopFloat();
						Vector3 position = this.batAttackPositions[this.batAttackPositionString.PopInt()].position;
						position.x *= (float)((!this.onLeft) ? -1 : 1);
						num *= (float)((!this.onLeft) ? 1 : -1);
						bool flag = this.batAttackSideString.PopLetter() == 'S';
						position.x *= (float)((!flag) ? 1 : -1);
						num *= (float)((!flag) ? 1 : -1);
						this.batCirclingList[which].AttackPlayer(position, height, num, this.batArcModifierString.PopFloat());
						this.batCirclingList.RemoveAt(which);
					}
				}
				else if (this.batCirclingList[which] == null)
				{
					this.batCirclingList.RemoveAt(which);
				}
				yield return null;
				player = PlayerManager.GetNext();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002F3F RID: 12095 RVA: 0x001BE367 File Offset: 0x001BC767
	public void ReturnBatToList(SnowCultLevelBat bat)
	{
		this.batCirclingList.Add(bat);
	}

	// Token: 0x06002F40 RID: 12096 RVA: 0x001BE378 File Offset: 0x001BC778
	private IEnumerator spawn_bats_cr()
	{
		this.batSpawnEffectPrefab.Create(base.transform.position + Vector3.up * 180f + Vector3.right * (float)((!this.onLeft) ? -20 : 20));
		if (this.bats == null)
		{
			this.bats = new SnowCultLevelBat[base.properties.CurrentState.snowball.batCount];
		}
		for (int j = 0; j < this.batCirclingList.Count; j++)
		{
			UnityEngine.Object.Destroy(this.batCirclingList[j].gameObject);
		}
		yield return null;
		this.batCirclingList.RemoveAll((SnowCultLevelBat b) => b == null);
		this.SFX_SNOWCULT_YetiFreezerScream();
		for (int i = 0; i < this.bats.Length; i++)
		{
			if (this.bats[i] == null || this.bats[i].gameObject == null || !this.bats[i].gameObject.activeInHierarchy)
			{
				Vector3 launchVelocity = new Vector3((float)((!this.onLeft) ? -1 : 1), 0.25f);
				launchVelocity *= (float)UnityEngine.Random.Range(500, 800);
				launchVelocity = Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(-30, 30)) * launchVelocity;
				this.bats[i] = this.batPrefab.Spawn<SnowCultLevelBat>();
				bool parryable = this.batParryableString.PopLetter() == 'P';
				this.bats[i].Init(base.transform.position + Vector3.up * 180f, launchVelocity, base.properties.CurrentState.snowball, this, parryable, (!parryable) ? ((!this.batColor) ? "Yellow" : string.Empty) : "Pink");
				if (!parryable)
				{
					this.batColor = !this.batColor;
				}
				this.batCirclingList.Add(this.bats[i]);
				yield return CupheadTime.WaitForSeconds(this, 0.125f);
			}
		}
		this.batLaunchTimer = base.properties.CurrentState.snowball.batInitialDelay;
		yield break;
	}

	// Token: 0x06002F41 RID: 12097 RVA: 0x001BE394 File Offset: 0x001BC794
	private void RemoveBats()
	{
		if (this.bats != null)
		{
			for (int i = 0; i < this.bats.Length; i++)
			{
				if (this.bats[i] != null)
				{
					this.bats[i].Dead();
				}
			}
		}
	}

	// Token: 0x06002F42 RID: 12098 RVA: 0x001BE3E5 File Offset: 0x001BC7E5
	public void Snowball()
	{
		base.StartCoroutine(this.snowball_cr());
	}

	// Token: 0x06002F43 RID: 12099 RVA: 0x001BE3F4 File Offset: 0x001BC7F4
	private bool BreakOutOfFridge()
	{
		return this.forceOutroToStart || base.properties.CurrentState.stateName == LevelProperties.SnowCult.States.EasyYeti;
	}

	// Token: 0x06002F44 RID: 12100 RVA: 0x001BE417 File Offset: 0x001BC817
	public void FridgeCanShoot()
	{
		this.fridgeCanShoot = true;
	}

	// Token: 0x06002F45 RID: 12101 RVA: 0x001BE420 File Offset: 0x001BC820
	public float GetIceCubeStartFrame()
	{
		this.iceCubeStartFrame = (this.iceCubeStartFrame + 1) % 3;
		switch (this.iceCubeStartFrame)
		{
		default:
			return 0f;
		case 1:
			return 2f;
		case 2:
			return 5f;
		}
	}

	// Token: 0x06002F46 RID: 12102 RVA: 0x001BE46C File Offset: 0x001BC86C
	public int GetMediumExplosion()
	{
		this.iceCubeExplosionCounterMedium = (this.iceCubeExplosionCounterMedium + 1) % 2;
		return this.iceCubeExplosionCounterMedium;
	}

	// Token: 0x06002F47 RID: 12103 RVA: 0x001BE484 File Offset: 0x001BC884
	public int GetSmallExplosion()
	{
		this.iceCubeExplosionCounterSmall = (this.iceCubeExplosionCounterSmall + 1) % 3;
		return this.iceCubeExplosionCounterSmall;
	}

	// Token: 0x06002F48 RID: 12104 RVA: 0x001BE49C File Offset: 0x001BC89C
	private IEnumerator snowball_cr()
	{
		this.SetState(SnowCultLevelYeti.States.Snowball);
		this.fridgeCanShoot = false;
		base.animator.SetTrigger("OnFridgeMorph");
		LevelProperties.SnowCult.Snowball p = base.properties.CurrentState.snowball;
		string[] snowballType = p.snowballTypeString[this.snowballMainIndex].Split(new char[]
		{
			','
		});
		int target = Animator.StringToHash(base.animator.GetLayerName(0) + ".Idle");
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == target)
		{
			if (this.BreakOutOfFridge())
			{
				base.animator.ResetTrigger("OnFridgeMorph");
				this.SetState(SnowCultLevelYeti.States.Idle);
				yield break;
			}
			yield return null;
		}
		int count = snowballType.Length;
		float t;
		for (int i = 0; i < count; i++)
		{
			if (this.BreakOutOfFridge())
			{
				break;
			}
			while (!this.fridgeCanShoot && !this.forceOutroToStart)
			{
				yield return null;
			}
			this.fridgeCanShoot = false;
			if (!this.BreakOutOfFridge())
			{
				base.animator.Play("FridgeShoot");
				this.SFX_SNOWCULT_YetiFreezerIceCubeLaunch();
				AbstractPlayerController next = PlayerManager.GetNext();
				float num = p.shotMaxAngle;
				float speed = p.shotMaxSpeed;
				float num2 = float.MaxValue;
				Vector2 vector = new Vector2(next.transform.position.x, (float)Level.Current.Ground) - this.cubeLaunchPosition.transform.position;
				vector.x = Mathf.Abs(vector.x);
				MinMax minMax = new MinMax(p.shotMinAngle, p.shotMaxAngle);
				MinMax minMax2 = new MinMax(p.shotMinSpeed, p.shotMaxSpeed);
				if (vector.y > 0f)
				{
					float num3 = minMax2.max / p.shotGravity;
					float num4 = minMax2.max * num3 - 0.5f * p.shotGravity * num3 * num3;
					float num5 = num4 + vector.y * 0f;
					float num6 = Mathf.Sqrt(2f * num5 / p.shotGravity);
					minMax2.max = num6 * p.shotGravity;
					minMax2.min *= minMax2.max / p.shotMaxSpeed;
				}
				float num7 = 0f;
				while (num7 < 1f)
				{
					float floatAt = minMax.GetFloatAt(num7);
					float floatAt2 = minMax2.GetFloatAt(num7);
					Vector2 vector2 = MathUtils.AngleToDirection(floatAt) * floatAt2;
					t = vector.x / vector2.x;
					float num8 = vector2.y * t - 0.5f * p.shotGravity * t * t;
					float num9 = Mathf.Abs(vector.y - num8);
					if (p.shotGravity <= 0.01f)
					{
						goto IL_43C;
					}
					float num10 = vector2.y - p.shotGravity * t;
					if (num10 <= 0f)
					{
						goto IL_43C;
					}
					IL_450:
					num7 += 0.01f;
					continue;
					IL_43C:
					if (num9 < num2)
					{
						num2 = num9;
						num = floatAt;
						speed = floatAt2;
						goto IL_450;
					}
					goto IL_450;
				}
				if (next.transform.position.x < base.transform.position.x)
				{
					num = 180f - num;
				}
				SnowCultLevelSnowball snowCultLevelSnowball = null;
				if (snowballType[i][0] == 'S')
				{
					snowCultLevelSnowball = this.smallSnowballPrefab.Spawn<SnowCultLevelSnowball>();
				}
				else if (snowballType[i][0] == 'M')
				{
					snowCultLevelSnowball = this.mediumSnowballPrefab.Spawn<SnowCultLevelSnowball>();
				}
				else if (snowballType[i][0] == 'L')
				{
					snowCultLevelSnowball = this.largeSnowballPrefab.Spawn<SnowCultLevelSnowball>();
				}
				snowCultLevelSnowball.InitOriginal(this.cubeLaunchPosition.transform.position, p.shotGravity, speed, num, p, this);
				if (i == snowballType.Length - 1 && Level.Current.mode == Level.Mode.Easy && !this.BreakOutOfFridge())
				{
					i = -1;
					this.snowballMainIndex = (this.snowballMainIndex + 1) % p.snowballTypeString.Length;
					snowballType = p.snowballTypeString[this.snowballMainIndex].Split(new char[]
					{
						','
					});
					count = snowballType.Length;
				}
			}
			if (!this.BreakOutOfFridge() && i < count - 1)
			{
				yield return CupheadTime.WaitForSeconds(this, p.snowballThrowDelay);
			}
		}
		t = 0f;
		while (t < p.batLaunchDelay && !this.BreakOutOfFridge())
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		if (!this.BreakOutOfFridge())
		{
			base.animator.SetTrigger("OnFridgeOutro");
			yield return base.animator.WaitForAnimationToStart(this, "FridgeOutroLoop", false);
		}
		else
		{
			yield return base.animator.WaitForAnimationToEnd(this, "FridgeShoot", false, false);
			if (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(base.animator.GetLayerName(0) + ".FridgeIdle"))
			{
				while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2777778f && base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8333333f)
				{
					yield return null;
				}
				base.animator.Play("FridgeOutroOpen");
			}
		}
		if (!this.forceOutroToStart)
		{
			yield return base.StartCoroutine(this.spawn_bats_cr());
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			this.snowballMainIndex = (this.snowballMainIndex + 1) % p.snowballTypeString.Length;
			base.animator.Play("FridgeOutroMorph");
		}
		yield return CupheadTime.WaitForSeconds(this, (!this.BreakOutOfFridge()) ? p.hesitate : 0f);
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		this.SetState(SnowCultLevelYeti.States.Idle);
		yield return null;
		yield break;
	}

	// Token: 0x06002F49 RID: 12105 RVA: 0x001BE4B8 File Offset: 0x001BC8B8
	public void ToEasyPhaseThree()
	{
		LevelProperties.SnowCult.Yeti yeti = base.properties.CurrentState.yeti;
		this.patternString = yeti.yetiPatternString.Split(new char[]
		{
			','
		});
		this.patternStringIndex = UnityEngine.Random.Range(0, this.patternString.Length);
		this.InitBats();
		base.StartCoroutine(this.bats_attack_cr());
	}

	// Token: 0x06002F4A RID: 12106 RVA: 0x001BE519 File Offset: 0x001BC919
	public void ForceOutroToStart()
	{
		base.animator.SetBool("ForceOutro", true);
		this.forceOutroToStart = true;
	}

	// Token: 0x06002F4B RID: 12107 RVA: 0x001BE533 File Offset: 0x001BC933
	private void OnBossDeath()
	{
		this.StopAllCoroutines();
		base.animator.Play("DeathEasy");
	}

	// Token: 0x06002F4C RID: 12108 RVA: 0x001BE54B File Offset: 0x001BC94B
	public void OnDeath()
	{
		base.animator.SetBool("Dead", true);
		this.StopAllCoroutines();
		this.RemoveBats();
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
	}

	// Token: 0x06002F4D RID: 12109 RVA: 0x001BE580 File Offset: 0x001BC980
	public void ActivateLegs()
	{
		this.bucket.transform.parent = null;
		this.legs.transform.parent = null;
		this.legs.SetActive(true);
	}

	// Token: 0x06002F4E RID: 12110 RVA: 0x001BE5B0 File Offset: 0x001BC9B0
	public void DeathAnimationEnded()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002F4F RID: 12111 RVA: 0x001BE5BD File Offset: 0x001BC9BD
	private void AnimationEvent_SFX_SNOWCULT_YetiIntro02DroptoGround()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_intro_02_droptoground");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_intro_02_droptoground");
	}

	// Token: 0x06002F50 RID: 12112 RVA: 0x001BE5D9 File Offset: 0x001BC9D9
	private void AnimationEvent_SFX_SNOWCULT_YetiFridgeToSnowmonster()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_transform_from_fridge_to_snowmonster");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_transform_from_fridge_to_snowmonster");
	}

	// Token: 0x06002F51 RID: 12113 RVA: 0x001BE5F5 File Offset: 0x001BC9F5
	private void AnimationEvent_SFX_SNOWCULT_YetiSnowmonsterToFridge()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_transform_from_snowmonster_to_fridge");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_transform_from_snowmonster_to_fridge");
	}

	// Token: 0x06002F52 RID: 12114 RVA: 0x001BE611 File Offset: 0x001BCA11
	private void AnimationEvent_SFX_SNOWCULT_GroundSmash()
	{
		AudioManager.Play("sfx_DLC_SnowCult_P2_SnowMonster_GroundSmash_withHands");
		this.emitAudioFromObject.Add("sfx_DLC_SnowCult_P2_SnowMonster_GroundSmash_withHands");
	}

	// Token: 0x06002F53 RID: 12115 RVA: 0x001BE62D File Offset: 0x001BCA2D
	private void AnimationEvent_SFX_SNOWCULT_BodyRollPre()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodyrollpre");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodyrollpre");
	}

	// Token: 0x06002F54 RID: 12116 RVA: 0x001BE649 File Offset: 0x001BCA49
	private void AnimationEvent_SFX_SNOWCULT_BodyRoll()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodyroll");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodyroll");
	}

	// Token: 0x06002F55 RID: 12117 RVA: 0x001BE665 File Offset: 0x001BCA65
	private void AnimationEvent_SFX_SNOWCULT_BodyTossPre()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodytosspre");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodytosspre");
	}

	// Token: 0x06002F56 RID: 12118 RVA: 0x001BE681 File Offset: 0x001BCA81
	private void AnimationEvent_SFX_SNOWCULT_BodyToss()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_bodytoss");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_bodytoss");
	}

	// Token: 0x06002F57 RID: 12119 RVA: 0x001BE69D File Offset: 0x001BCA9D
	private void AnimationEvent_SFX_SNOWCULT_YetiDie()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_death_explode");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_death_explode");
	}

	// Token: 0x06002F58 RID: 12120 RVA: 0x001BE6BC File Offset: 0x001BCABC
	private void SFX_SNOWCULT_YetiFreezerScream()
	{
		this.batSoundLong = !this.batSoundLong;
		AudioManager.Play((!this.batSoundLong) ? "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_short" : "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_long");
		this.emitAudioFromObject.Add((!this.batSoundLong) ? "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_short" : "sfx_dlc_snowcult_p2_snowmonster_fridge_freezerscream_long");
	}

	// Token: 0x06002F59 RID: 12121 RVA: 0x001BE71C File Offset: 0x001BCB1C
	private void SFX_SNOWCULT_YetiFreezerIceCubeLaunch()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_launch");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_launch");
	}

	// Token: 0x040037E3 RID: 14307
	public SnowCultLevelYeti.States state;

	// Token: 0x040037E4 RID: 14308
	private SnowCultLevelYeti.States previousState;

	// Token: 0x040037E6 RID: 14310
	private const float BURST_SPAWN_X = 290f;

	// Token: 0x040037E7 RID: 14311
	private const float Y_TO_SPAWN = 95f;

	// Token: 0x040037E8 RID: 14312
	private const float Y_ICE_PILLAR_SPAWN = -142f;

	// Token: 0x040037E9 RID: 14313
	private const float POS_OFFSET_X = 147f;

	// Token: 0x040037EA RID: 14314
	private const float JUMP_LANDING_OFFSET_X = 247f;

	// Token: 0x040037EB RID: 14315
	private const float REFORM_TIME = 0.9583333f;

	// Token: 0x040037EC RID: 14316
	private const float BALL_RADIUS = 180f;

	// Token: 0x040037ED RID: 14317
	private Vector3 BALL_JUMP_OFFSET = new Vector3(50f, -100f);

	// Token: 0x040037EE RID: 14318
	private Vector3 BALL_DASH_OFFSET = new Vector3(50f, -180f);

	// Token: 0x040037EF RID: 14319
	[SerializeField]
	private SnowCultHandleBackground snowCultBGHandler;

	// Token: 0x040037F0 RID: 14320
	[SerializeField]
	private Transform yetiMidPoint;

	// Token: 0x040037F1 RID: 14321
	[SerializeField]
	private Transform yetiSpawnPoint;

	// Token: 0x040037F2 RID: 14322
	[SerializeField]
	private SnowCultLevelIcePillar icePillarPrefab;

	// Token: 0x040037F3 RID: 14323
	[SerializeField]
	private SnowCultLevelBat batPrefab;

	// Token: 0x040037F4 RID: 14324
	[SerializeField]
	private Effect batSpawnEffectPrefab;

	// Token: 0x040037F5 RID: 14325
	private bool batSoundLong;

	// Token: 0x040037F6 RID: 14326
	[SerializeField]
	private SnowCultLevelBurstEffect snowBurstA;

	// Token: 0x040037F7 RID: 14327
	[SerializeField]
	private SnowCultLevelBurstEffect snowFallA;

	// Token: 0x040037F8 RID: 14328
	[Header("Snowballs")]
	[SerializeField]
	private SnowCultLevelSnowball smallSnowballPrefab;

	// Token: 0x040037F9 RID: 14329
	[SerializeField]
	private SnowCultLevelSnowball mediumSnowballPrefab;

	// Token: 0x040037FA RID: 14330
	[SerializeField]
	private SnowCultLevelSnowball largeSnowballPrefab;

	// Token: 0x040037FB RID: 14331
	[SerializeField]
	private GameObject cubeLaunchPosition;

	// Token: 0x040037FC RID: 14332
	[SerializeField]
	private GameObject ball;

	// Token: 0x040037FD RID: 14333
	[SerializeField]
	private Animator[] meltFXAnimator;

	// Token: 0x040037FE RID: 14334
	[SerializeField]
	private GameObject dashGroundFX;

	// Token: 0x040037FF RID: 14335
	[SerializeField]
	private GameObject groundMask;

	// Token: 0x04003800 RID: 14336
	[SerializeField]
	private SpriteRenderer ballShadow;

	// Token: 0x04003801 RID: 14337
	[SerializeField]
	private GameObject introShadow;

	// Token: 0x04003802 RID: 14338
	[SerializeField]
	private Sprite[] shadowSprites;

	// Token: 0x04003803 RID: 14339
	private int offsetCoordIndex;

	// Token: 0x04003804 RID: 14340
	private int snowballMainIndex;

	// Token: 0x04003805 RID: 14341
	private string[] patternString;

	// Token: 0x04003806 RID: 14342
	private int patternStringIndex;

	// Token: 0x04003807 RID: 14343
	private PatternString batAttackPositionString;

	// Token: 0x04003808 RID: 14344
	private PatternString batAttackHeightString;

	// Token: 0x04003809 RID: 14345
	private PatternString batAttackWidthString;

	// Token: 0x0400380A RID: 14346
	private PatternString batAttackSideString;

	// Token: 0x0400380B RID: 14347
	private PatternString batAttackInterDelayString;

	// Token: 0x0400380C RID: 14348
	private PatternString batArcModifierString;

	// Token: 0x0400380D RID: 14349
	private PatternString batParryableString;

	// Token: 0x0400380E RID: 14350
	[SerializeField]
	private Transform[] batAttackPositions;

	// Token: 0x0400380F RID: 14351
	private float xScale;

	// Token: 0x04003810 RID: 14352
	private bool onLeft;

	// Token: 0x04003811 RID: 14353
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x04003812 RID: 14354
	[SerializeField]
	private Collider2D coll;

	// Token: 0x04003813 RID: 14355
	[SerializeField]
	private GameObject legs;

	// Token: 0x04003814 RID: 14356
	[SerializeField]
	private GameObject bucket;

	// Token: 0x04003815 RID: 14357
	private SnowCultLevelBat[] bats;

	// Token: 0x04003816 RID: 14358
	private List<SnowCultLevelBat> batCirclingList = new List<SnowCultLevelBat>();

	// Token: 0x04003817 RID: 14359
	private float batLaunchTimer;

	// Token: 0x04003818 RID: 14360
	private bool batColor;

	// Token: 0x04003819 RID: 14361
	private DamageDealer damageDealer;

	// Token: 0x0400381A RID: 14362
	private DamageReceiver damageReceiver;

	// Token: 0x0400381B RID: 14363
	private DamageReceiver ballDamageReceiver;

	// Token: 0x0400381C RID: 14364
	private bool fridgeCanShoot;

	// Token: 0x0400381D RID: 14365
	public bool introRibcageClosed;

	// Token: 0x0400381E RID: 14366
	private bool forceOutroToStart;

	// Token: 0x0400381F RID: 14367
	private int iceCubeStartFrame;

	// Token: 0x04003820 RID: 14368
	private int iceCubeExplosionCounterMedium;

	// Token: 0x04003821 RID: 14369
	private int iceCubeExplosionCounterSmall;

	// Token: 0x04003822 RID: 14370
	private int idleAnimFullPathHash;

	// Token: 0x02000800 RID: 2048
	public enum States
	{
		// Token: 0x04003825 RID: 14373
		Intro,
		// Token: 0x04003826 RID: 14374
		Idle,
		// Token: 0x04003827 RID: 14375
		Move,
		// Token: 0x04003828 RID: 14376
		IcePillar,
		// Token: 0x04003829 RID: 14377
		Sled,
		// Token: 0x0400382A RID: 14378
		Snowball
	}
}
