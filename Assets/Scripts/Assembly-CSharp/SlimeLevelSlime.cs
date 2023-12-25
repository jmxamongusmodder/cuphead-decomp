using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007DD RID: 2013
public class SlimeLevelSlime : LevelProperties.Slime.Entity
{
	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x06002DEC RID: 11756 RVA: 0x001B12A3 File Offset: 0x001AF6A3
	// (set) Token: 0x06002DED RID: 11757 RVA: 0x001B12AA File Offset: 0x001AF6AA
	public static bool TINIES { get; private set; }

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x06002DEE RID: 11758 RVA: 0x001B12B2 File Offset: 0x001AF6B2
	// (set) Token: 0x06002DEF RID: 11759 RVA: 0x001B12BA File Offset: 0x001AF6BA
	public SlimeLevelSlime.State state { get; private set; }

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x06002DF0 RID: 11760 RVA: 0x001B12C3 File Offset: 0x001AF6C3
	// (set) Token: 0x06002DF1 RID: 11761 RVA: 0x001B12CB File Offset: 0x001AF6CB
	public LevelProperties.Slime.State CurrentPropertyState { get; set; }

	// Token: 0x06002DF2 RID: 11762 RVA: 0x001B12D4 File Offset: 0x001AF6D4
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.onGroundY = base.transform.position.y;
		this.shadowY = this.shadow.transform.position.y;
		SlimeLevelSlime.TINIES = false;
		this.shadow.enabled = false;
		if (this.isBig)
		{
			foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
			{
				collider2D.enabled = false;
			}
		}
		foreach (Animator animator in this.questionMarks)
		{
			animator.GetComponent<Collider2D>().enabled = false;
		}
	}

	// Token: 0x06002DF3 RID: 11763 RVA: 0x001B13C9 File Offset: 0x001AF7C9
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002DF4 RID: 11764 RVA: 0x001B13E4 File Offset: 0x001AF7E4
	private void LateUpdate()
	{
		this.updateShadow(base.transform.position.y - this.onGroundY);
	}

	// Token: 0x06002DF5 RID: 11765 RVA: 0x001B1411 File Offset: 0x001AF811
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002DF6 RID: 11766 RVA: 0x001B1430 File Offset: 0x001AF830
	public override void LevelInit(LevelProperties.Slime properties)
	{
		base.LevelInit(properties);
		this.CurrentPropertyState = properties.CurrentState;
		this.jumpsBeforeFirstPunch = this.CurrentPropertyState.jump.bigSlimeInitialJumpPunchCount;
		if (this.isBig)
		{
			properties.OnBossDeath += this.OnBossDeath;
		}
	}

	// Token: 0x06002DF7 RID: 11767 RVA: 0x001B1484 File Offset: 0x001AF884
	private void QuestionMarksOn()
	{
		foreach (Animator animator in this.questionMarks)
		{
			animator.transform.SetScale(new float?(base.transform.localScale.x), null, null);
			animator.SetBool("IsOn", true);
			animator.GetComponent<Collider2D>().enabled = true;
		}
	}

	// Token: 0x06002DF8 RID: 11768 RVA: 0x001B1500 File Offset: 0x001AF900
	private void QuestionMarksOff()
	{
		foreach (Animator animator in this.questionMarks)
		{
			if (animator != null)
			{
				animator.SetBool("IsOn", false);
				animator.GetComponent<Collider2D>().enabled = false;
			}
		}
	}

	// Token: 0x06002DF9 RID: 11769 RVA: 0x001B1550 File Offset: 0x001AF950
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.dustPrefab = null;
		this.explosionPrefab = null;
	}

	// Token: 0x06002DFA RID: 11770 RVA: 0x001B1566 File Offset: 0x001AF966
	public void IntroContinue()
	{
		this.StartJump();
	}

	// Token: 0x06002DFB RID: 11771 RVA: 0x001B156E File Offset: 0x001AF96E
	public void StartJump()
	{
		this.state = SlimeLevelSlime.State.Jump;
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x06002DFC RID: 11772 RVA: 0x001B1584 File Offset: 0x001AF984
	private IEnumerator jump_cr()
	{
		LevelProperties.Slime.Jump p = this.CurrentPropertyState.jump;
		string[] pattern = p.patternString.Split(new char[]
		{
			','
		});
		int i = UnityEngine.Random.Range(0, pattern.Length);
		int numJumpsLeft = 0;
		if (this.firstTime && this.isBig)
		{
			numJumpsLeft = p.bigSlimeInitialJumpPunchCount;
			this.firstTime = false;
		}
		else
		{
			numJumpsLeft = p.numJumps.RandomInt();
		}
		base.animator.SetTrigger("Jump");
		float delay = p.groundDelay;
		this.playerToPunch = PlayerManager.GetNext();
		for (;;)
		{
			if (pattern[i][0] == 'D')
			{
				Parser.FloatTryParse(pattern[i].Substring(1), out delay);
			}
			else
			{
				yield return base.animator.WaitForAnimationToStart(this, "Jump_Squish_Loop", false);
				if (this.isBig)
				{
					this.BigJumpAudio();
				}
				else
				{
					this.SmallJumpAudio();
				}
				yield return CupheadTime.WaitForSeconds(this, delay);
				base.animator.SetTrigger("Continue");
				yield return base.animator.WaitForAnimationToStart(this, "Up", false);
				bool goingUp = true;
				bool highJump = pattern[i][0] == 'H';
				if (pattern[i][0] == 'R')
				{
					highJump = Rand.Bool();
				}
				this.velocityY = ((!highJump) ? p.lowJumpVerticalSpeed : p.highJumpVerticalSpeed);
				float speedX = (!highJump) ? p.lowJumpHorizontalSpeed : p.highJumpHorizontalSpeed;
				this.gravity = ((!highJump) ? p.lowJumpGravity : p.highJumpGravity);
				this.inAir = true;
				SlimeLevelSlime.Direction moveDir = this.facingDirection;
				this.shadow.enabled = true;
				while (goingUp || base.transform.position.y > this.onGroundY)
				{
					this.velocityY -= this.gravity * CupheadTime.FixedDelta * this.hitPauseCoefficient();
					float velocityX = (moveDir != SlimeLevelSlime.Direction.Left) ? speedX : (-speedX);
					base.transform.AddPosition(velocityX * CupheadTime.FixedDelta * this.hitPauseCoefficient(), this.velocityY * CupheadTime.FixedDelta * this.hitPauseCoefficient(), 0f);
					if (this.velocityY < 0f && goingUp)
					{
						goingUp = false;
						base.animator.SetTrigger("Apex");
					}
					if ((moveDir == SlimeLevelSlime.Direction.Left && base.transform.position.x < -this.maxX) || (moveDir == SlimeLevelSlime.Direction.Right && base.transform.position.x > this.maxX))
					{
						if (moveDir == SlimeLevelSlime.Direction.Left)
						{
							base.transform.SetPosition(new float?(-this.maxX), null, null);
							moveDir = SlimeLevelSlime.Direction.Right;
						}
						else
						{
							base.transform.SetPosition(new float?(this.maxX), null, null);
							moveDir = SlimeLevelSlime.Direction.Left;
						}
						if (!goingUp)
						{
							speedX = 0f;
						}
						this.Turn();
					}
					yield return new WaitForFixedUpdate();
				}
				base.transform.SetPosition(null, new float?(this.onGroundY), null);
				this.shadow.enabled = false;
				this.inAir = false;
				delay = p.groundDelay;
				float screenShakeCoefficient = (!highJump) ? 1f : 1.5f;
				screenShakeCoefficient *= ((!this.isBig) ? 1f : 2f);
				CupheadLevelCamera.Current.Shake(5f * screenShakeCoefficient, 0.2f * screenShakeCoefficient, false);
				this.dustPrefab.Create(base.transform.position);
				if (this.wantsToTransform && base.transform.position.x > -350f && base.transform.position.x < 350f)
				{
					break;
				}
				if (this.dieOnLand)
				{
					goto Block_23;
				}
				base.animator.SetTrigger("Land");
				if (this.isBig && !this.firstPunch)
				{
					this.jumpsBeforeFirstPunch--;
					if (this.jumpsBeforeFirstPunch == 0)
					{
						goto Block_26;
					}
				}
				else
				{
					numJumpsLeft--;
					if (numJumpsLeft <= 0 && this.inPunchPosition())
					{
						goto Block_28;
					}
				}
			}
			i = (i + 1) % pattern.Length;
		}
		base.animator.SetTrigger("Transform");
		yield break;
		Block_23:
		base.animator.SetTrigger("LandingDeath");
		this.state = SlimeLevelSlime.State.Dying;
		yield break;
		Block_26:
		this.firstPunch = true;
		yield return this.Punch();
		yield break;
		Block_28:
		yield return this.Punch();
		yield break;
		yield break;
	}

	// Token: 0x06002DFD RID: 11773 RVA: 0x001B15A0 File Offset: 0x001AF9A0
	private IEnumerator Punch()
	{
		if (this.playerToPunch == null || this.playerToPunch.IsDead)
		{
			this.playerToPunch = PlayerManager.GetNext();
		}
		this.punchDirection = ((this.playerToPunch.transform.position.x <= base.transform.position.x) ? SlimeLevelSlime.Direction.Left : SlimeLevelSlime.Direction.Right);
		if (!this.isBig)
		{
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToEnd(this, "Jump_Squish_Loop", false, true);
		}
		this.StartPunch();
		yield break;
	}

	// Token: 0x06002DFE RID: 11774 RVA: 0x001B15BC File Offset: 0x001AF9BC
	private void updateShadow(float jumpY)
	{
		this.shadow.transform.SetPosition(null, new float?(this.shadowY), null);
		float normalizedTime = Mathf.Clamp01(jumpY / this.shadowMaxY);
		Animator component = this.shadow.GetComponent<Animator>();
		component.Play("Idle", 0, normalizedTime);
		component.speed = 0f;
	}

	// Token: 0x06002DFF RID: 11775 RVA: 0x001B1628 File Offset: 0x001AFA28
	private void Turn()
	{
		base.animator.SetTrigger("Turn");
		base.StartCoroutine(this.turn_cr());
	}

	// Token: 0x06002E00 RID: 11776 RVA: 0x001B1648 File Offset: 0x001AFA48
	private IEnumerator turn_cr()
	{
		int upTurn = Animator.StringToHash(base.animator.GetLayerName(0) + ".Up_Turn");
		int downTurn = Animator.StringToHash(base.animator.GetLayerName(0) + ".Down_Turn");
		int startSquish = Animator.StringToHash(base.animator.GetLayerName(0) + ".Jump_Squish_Start");
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != upTurn && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != downTurn && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != startSquish)
		{
			yield return new WaitForEndOfFrame();
		}
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == upTurn || base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == downTurn)
		{
			yield return new WaitForEndOfFrame();
		}
		this.facingDirection = ((this.facingDirection != SlimeLevelSlime.Direction.Left) ? SlimeLevelSlime.Direction.Left : SlimeLevelSlime.Direction.Right);
		base.transform.SetScale(new float?((float)((this.facingDirection != SlimeLevelSlime.Direction.Right) ? 1 : -1)), null, null);
		yield break;
	}

	// Token: 0x06002E01 RID: 11777 RVA: 0x001B1664 File Offset: 0x001AFA64
	private bool inPunchPosition()
	{
		if (this.playerToPunch == null || this.playerToPunch.IsDead)
		{
			this.playerToPunch = PlayerManager.GetNext();
		}
		return (this.playerToPunch.transform.position.x > base.transform.position.x && base.transform.position.x < this.punchMaxX && base.transform.position.x > this.punchMinX) || (this.playerToPunch.transform.position.x < base.transform.position.x && base.transform.position.x > -this.punchMaxX && base.transform.position.x < -this.punchMinX);
	}

	// Token: 0x06002E02 RID: 11778 RVA: 0x001B177E File Offset: 0x001AFB7E
	public void StartPunch()
	{
		this.state = SlimeLevelSlime.State.Punch;
		base.StartCoroutine(this.punch_cr());
	}

	// Token: 0x06002E03 RID: 11779 RVA: 0x001B1794 File Offset: 0x001AFB94
	private IEnumerator punch_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0f);
		bool turn = this.punchDirection != this.facingDirection;
		base.animator.SetTrigger((!turn) ? "StartPunch" : "StartPunchTurn");
		yield return base.animator.WaitForAnimationToStart(this, "Punch_Pre_Hold", false);
		yield return CupheadTime.WaitForSeconds(this, this.CurrentPropertyState.punch.preHold);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Punch_Hold", false);
		yield return CupheadTime.WaitForSeconds(this, this.CurrentPropertyState.punch.mainHold);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Punch_End", false);
		this.BigPunchPlaying = false;
		this.StartJump();
		if (this.isBig)
		{
			base.animator.SetBool("FirstPunch", false);
		}
		yield break;
	}

	// Token: 0x06002E04 RID: 11780 RVA: 0x001B17B0 File Offset: 0x001AFBB0
	private void PunchTurn()
	{
		base.animator.SetTrigger("Continue");
		this.facingDirection = ((this.facingDirection != SlimeLevelSlime.Direction.Left) ? SlimeLevelSlime.Direction.Left : SlimeLevelSlime.Direction.Right);
		base.transform.SetScale(new float?((float)((this.facingDirection != SlimeLevelSlime.Direction.Right) ? 1 : -1)), null, null);
	}

	// Token: 0x06002E05 RID: 11781 RVA: 0x001B181B File Offset: 0x001AFC1B
	public void Transform()
	{
		this.wantsToTransform = true;
	}

	// Token: 0x06002E06 RID: 11782 RVA: 0x001B1824 File Offset: 0x001AFC24
	public void TurnBig()
	{
		this.bigSlime.transform.position = base.transform.position;
		this.bigSlime.StartJump();
		this.bigSlime.facingDirection = this.facingDirection;
		this.bigSlime.transform.localScale = base.transform.localScale;
		foreach (Collider2D collider2D in this.bigSlime.GetComponents<Collider2D>())
		{
			collider2D.enabled = true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002E07 RID: 11783 RVA: 0x001B18B9 File Offset: 0x001AFCB9
	private void OnBossDeath()
	{
		this.Die(false);
	}

	// Token: 0x06002E08 RID: 11784 RVA: 0x001B18C4 File Offset: 0x001AFCC4
	public void DeathTransform()
	{
		if (this.state != SlimeLevelSlime.State.Dying)
		{
			base.properties.OnBossDeath -= this.OnBossDeath;
			if (this.inAir)
			{
				this.dieOnLand = true;
			}
			else
			{
				this.Die(false);
			}
		}
		base.StartCoroutine(this.transformToTombstone_cr());
	}

	// Token: 0x06002E09 RID: 11785 RVA: 0x001B1920 File Offset: 0x001AFD20
	private IEnumerator transformToTombstone_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Death_Loop", false);
		yield return CupheadTime.WaitForSeconds(this, 3.5f);
		this.tombStone.StartIntro(base.transform.position.x);
		yield break;
	}

	// Token: 0x06002E0A RID: 11786 RVA: 0x001B193C File Offset: 0x001AFD3C
	private void Die(bool earlyKnockout)
	{
		this.StopAllCoroutines();
		this.state = SlimeLevelSlime.State.Dying;
		base.animator.ResetTrigger("Continue");
		if (earlyKnockout)
		{
			base.animator.SetTrigger("EarlyKnockout");
			if (Level.Current.mode == Level.Mode.Easy)
			{
				base.properties.WinInstantly();
			}
			else
			{
				base.properties.DealDamageToNextNamedState();
			}
		}
		else if (this.inAir)
		{
			base.animator.SetTrigger("AirDeath");
			base.StartCoroutine(this.airDeath_cr());
		}
		else
		{
			base.animator.SetTrigger("GroundDeath");
		}
	}

	// Token: 0x06002E0B RID: 11787 RVA: 0x001B19E8 File Offset: 0x001AFDE8
	private IEnumerator airDeath_cr()
	{
		this.velocityY = Mathf.Min(0f, this.velocityY);
		while (base.transform.position.y > this.onGroundY)
		{
			this.velocityY -= this.gravity * CupheadTime.FixedDelta * this.hitPauseCoefficient();
			base.transform.AddPosition(0f, this.velocityY * CupheadTime.FixedDelta * this.hitPauseCoefficient(), 0f);
			yield return new WaitForFixedUpdate();
		}
		base.transform.SetPosition(null, new float?(this.onGroundY), null);
		float screenShakeCoefficient = 2.5f;
		CupheadLevelCamera.Current.Shake(5f * screenShakeCoefficient, 0.2f * screenShakeCoefficient, false);
		this.dustPrefab.Create(base.transform.position);
		this.shadow.enabled = false;
		base.animator.SetTrigger("Continue");
		yield break;
	}

	// Token: 0x06002E0C RID: 11788 RVA: 0x001B1A03 File Offset: 0x001AFE03
	private float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06002E0D RID: 11789 RVA: 0x001B1A24 File Offset: 0x001AFE24
	public void Explode()
	{
		this.explosionPrefab.Create(base.transform.position);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002E0E RID: 11790 RVA: 0x001B1A48 File Offset: 0x001AFE48
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002E0F RID: 11791 RVA: 0x001B1A5B File Offset: 0x001AFE5B
	private void IntroAudio()
	{
		AudioManager.Play("slime_small_intro_anim");
		this.emitAudioFromObject.Add("slime_small_intro_anim");
		AudioManager.Play("slime_tiphat");
		this.emitAudioFromObject.Add("slime_tiphat");
	}

	// Token: 0x06002E10 RID: 11792 RVA: 0x001B1A91 File Offset: 0x001AFE91
	private void BlinkAudio()
	{
		AudioManager.Play("slime_blink");
	}

	// Token: 0x06002E11 RID: 11793 RVA: 0x001B1A9D File Offset: 0x001AFE9D
	private void SmallJumpAudio()
	{
		AudioManager.Play("slime_small_jump");
	}

	// Token: 0x06002E12 RID: 11794 RVA: 0x001B1AA9 File Offset: 0x001AFEA9
	private void SmallLandAudio()
	{
		AudioManager.Play("slime_small_land");
	}

	// Token: 0x06002E13 RID: 11795 RVA: 0x001B1AB5 File Offset: 0x001AFEB5
	private void SmallStretchPunchAudio()
	{
		AudioManager.Play("slime_small_stretch_punch");
		this.emitAudioFromObject.Add("slime_small_stretch_punch");
	}

	// Token: 0x06002E14 RID: 11796 RVA: 0x001B1AD1 File Offset: 0x001AFED1
	private void SmallTransformAudio()
	{
		AudioManager.Play("slime_small_transform");
	}

	// Token: 0x06002E15 RID: 11797 RVA: 0x001B1ADD File Offset: 0x001AFEDD
	private void BigJumpAudio()
	{
		AudioManager.Play("slime_big_jump");
	}

	// Token: 0x06002E16 RID: 11798 RVA: 0x001B1AE9 File Offset: 0x001AFEE9
	private void BigLandAudio()
	{
		AudioManager.Play("slime_big_land");
	}

	// Token: 0x06002E17 RID: 11799 RVA: 0x001B1AF8 File Offset: 0x001AFEF8
	private void BigPunchAudio()
	{
		if (!this.BigPunchPlaying)
		{
			AudioManager.Play("slime_big_punch");
			this.emitAudioFromObject.Add("slime_big_punch");
			AudioManager.Play("slime_big_punch_voice");
			this.emitAudioFromObject.Add("slime_big_punch_voice");
			this.BigPunchPlaying = true;
		}
	}

	// Token: 0x06002E18 RID: 11800 RVA: 0x001B1B4B File Offset: 0x001AFF4B
	private void BigDeathAudio()
	{
		if (!this.deathAudioPlayed)
		{
			AudioManager.Play("slime_big_death");
			AudioManager.Play("slime_big_death_voice");
			this.deathAudioPlayed = true;
		}
	}

	// Token: 0x0400366B RID: 13931
	[SerializeField]
	private Animator[] questionMarks;

	// Token: 0x0400366D RID: 13933
	private const float TRANSFORM_MAX_X = 350f;

	// Token: 0x0400366F RID: 13935
	private SlimeLevelSlime.Direction facingDirection;

	// Token: 0x04003670 RID: 13936
	private DamageDealer damageDealer;

	// Token: 0x04003671 RID: 13937
	private DamageReceiver damageReceiver;

	// Token: 0x04003672 RID: 13938
	private float onGroundY;

	// Token: 0x04003673 RID: 13939
	private float shadowY;

	// Token: 0x04003674 RID: 13940
	private bool wantsToTransform;

	// Token: 0x04003675 RID: 13941
	private SlimeLevelSlime.Direction punchDirection;

	// Token: 0x04003676 RID: 13942
	private bool inAir;

	// Token: 0x04003677 RID: 13943
	private float velocityY;

	// Token: 0x04003678 RID: 13944
	private float gravity;

	// Token: 0x04003679 RID: 13945
	private bool dieOnLand;

	// Token: 0x0400367A RID: 13946
	private bool deathAudioPlayed;

	// Token: 0x0400367B RID: 13947
	private bool firstTime = true;

	// Token: 0x0400367C RID: 13948
	private bool firstPunch;

	// Token: 0x0400367D RID: 13949
	private bool BigPunchPlaying;

	// Token: 0x0400367E RID: 13950
	private int jumpsBeforeFirstPunch;

	// Token: 0x04003680 RID: 13952
	private AbstractPlayerController playerToPunch;

	// Token: 0x04003681 RID: 13953
	[SerializeField]
	private SpriteRenderer shadow;

	// Token: 0x04003682 RID: 13954
	[SerializeField]
	private float shadowMaxY;

	// Token: 0x04003683 RID: 13955
	[SerializeField]
	private SlimeLevelSlime bigSlime;

	// Token: 0x04003684 RID: 13956
	[SerializeField]
	private SlimeLevelTombstone tombStone;

	// Token: 0x04003685 RID: 13957
	[SerializeField]
	private Effect explosionPrefab;

	// Token: 0x04003686 RID: 13958
	[SerializeField]
	private Effect dustPrefab;

	// Token: 0x04003687 RID: 13959
	[SerializeField]
	private bool isBig;

	// Token: 0x04003688 RID: 13960
	[SerializeField]
	private float punchMaxX;

	// Token: 0x04003689 RID: 13961
	[SerializeField]
	private float punchMinX;

	// Token: 0x0400368A RID: 13962
	[SerializeField]
	private float maxX;

	// Token: 0x0400368B RID: 13963
	[SerializeField]
	private Transform eyeMaxPosition;

	// Token: 0x020007DE RID: 2014
	public enum State
	{
		// Token: 0x0400368D RID: 13965
		Intro,
		// Token: 0x0400368E RID: 13966
		Jump,
		// Token: 0x0400368F RID: 13967
		Punch,
		// Token: 0x04003690 RID: 13968
		Dying
	}

	// Token: 0x020007DF RID: 2015
	private enum Direction
	{
		// Token: 0x04003692 RID: 13970
		Left,
		// Token: 0x04003693 RID: 13971
		Right
	}
}
