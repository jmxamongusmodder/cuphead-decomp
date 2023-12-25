using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A11 RID: 2577
public class LevelPlayerAnimationController : AbstractLevelPlayerComponent
{
	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x0021AE8F File Offset: 0x0021928F
	// (set) Token: 0x06003CE6 RID: 15590 RVA: 0x0021AE97 File Offset: 0x00219297
	public SpriteRenderer spriteRenderer { get; private set; }

	// Token: 0x06003CE7 RID: 15591 RVA: 0x0021AEA0 File Offset: 0x002192A0
	private void Start()
	{
		if (!this.chaliceActivated)
		{
			base.animator.SetLayerWeight(3, 0f);
			base.animator.SetLayerWeight(4, 0f);
		}
		base.basePlayer.OnPlayIntroEvent += this.PlayIntro;
		base.basePlayer.OnPlatformingLevelAwakeEvent += this.CheckIfChaliceAndActivate;
		base.player.motor.OnParryEvent += this.OnParryStart;
		base.player.motor.OnGroundedEvent += this.OnGrounded;
		base.player.motor.OnDashStartEvent += this.OnDashStart;
		base.player.motor.OnDashEndEvent += this.OnDashEnd;
		base.player.motor.OnDoubleJumpEvent += this.ChaliceDoubleJumpFX;
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.player.weaponManager.OnExStart += this.OnEx;
		base.player.weaponManager.OnSuperStart += this.OnSuper;
		base.player.weaponManager.OnSuperEnd += this.OnSuperEnd;
		base.player.weaponManager.OnWeaponFire += this.OnShotFired;
		LevelPauseGUI.OnPauseEvent += this.OnGuiPause;
		LevelPauseGUI.OnPauseEvent += this.OnGuiUnpause;
		this.lastTrueLookDir = base.player.motor.TrueLookDirection;
		this.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss);
		PlayerRecolorHandler.SetChaliceRecolorEnabled(this.chalice.GetComponent<SpriteRenderer>().sharedMaterial, SettingsData.Data.filter == BlurGamma.Filter.Chalice);
		if (base.player.stats.Loadout.charm == Charm.charm_curse)
		{
			this.curseCharmLevel = CharmCurse.CalculateLevel(base.player.id);
		}
		if (Level.Current.LevelType != Level.Type.Platforming)
		{
			if (base.player.stats.isChalice)
			{
				if ((Level.IsDicePalace && !DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY) || SceneLoader.CurrentLevel == Levels.Kitchen || SceneLoader.CurrentLevel == Levels.ChaliceTutorial)
				{
					this.CheckIfChaliceAndActivate();
					base.basePlayer.OnPlayIntroEvent -= this.PlayIntro;
				}
				else if (SceneLoader.CurrentLevel != Levels.Devil && SceneLoader.CurrentLevel != Levels.Saltbaker)
				{
					this.StartChaliceIntroHold(false);
				}
			}
			else if (base.player.stats.Loadout.charm == Charm.charm_chalice && SceneLoader.CurrentLevel != Levels.Devil && SceneLoader.CurrentLevel != Levels.Saltbaker && SceneLoader.CurrentLevel != Levels.Kitchen && SceneLoader.CurrentLevel != Levels.ChaliceTutorial && (!Level.IsDicePalace || DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY))
			{
				this.StartChaliceIntroHold(true);
			}
		}
		if (SceneLoader.CurrentLevel == Levels.ChaliceTutorial)
		{
			this.spriteRenderer.gameObject.layer = 31;
			foreach (SpriteRenderer spriteRenderer in this.chaliceSprites)
			{
				spriteRenderer.gameObject.layer = 31;
			}
			foreach (SpriteRenderer spriteRenderer2 in this.chaliceJumpShootRenderers)
			{
				spriteRenderer2.gameObject.layer = 31;
			}
		}
	}

	// Token: 0x06003CE8 RID: 15592 RVA: 0x0021B284 File Offset: 0x00219684
	private void OnEnable()
	{
		base.StartCoroutine(this.flash_cr());
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x0021B294 File Offset: 0x00219694
	private void OnDisable()
	{
		if (this.paladinShadows[0])
		{
			this.paladinShadows[0].enabled = false;
		}
		if (this.paladinShadows[1])
		{
			this.paladinShadows[1].enabled = false;
		}
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x0021B2E4 File Offset: 0x002196E4
	private void Update()
	{
		if (base.player.IsDead || !base.player.levelStarted)
		{
			return;
		}
		if (this.curseCharmLevel > -1 && !this.showCurseFX && !Level.IsChessBoss)
		{
			this.InitializeCurseFX();
			this.showCurseFX = true;
		}
		if (base.player.stats.isChalice && this.chaliceActivated)
		{
			this.ChaliceAimSpriteHandling();
			this.ChaliceJumpHandling();
			this.ChaliceJumpShootHandling();
			if (!base.player.motor.Dashing)
			{
				base.animator.SetLayerWeight(3, 1f);
				if (this.chaliceInvincibleSparklesCoroutine != null)
				{
					base.StopCoroutine(this.chaliceInvincibleSparklesCoroutine);
					this.chaliceInvincibleSparklesCoroutine = null;
				}
			}
		}
		if (this.curseCharmLevel > -1)
		{
			this.HandleCurseFX();
		}
		if (!this.hitAnimation && base.player.motor.LookDirection.x != 0 && this.lastTrueLookDir.x != base.player.motor.TrueLookDirection.x)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.Turning, true);
		}
		else
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.Turning, false);
		}
		this.lastTrueLookDir = base.player.motor.TrueLookDirection;
		this.SetBool(LevelPlayerAnimationController.Booleans.Grounded, base.player.motor.Grounded);
		this.SetBool(LevelPlayerAnimationController.Booleans.Locked, base.player.motor.Locked);
		if (base.player.motor.Locked)
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0);
		}
		else
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveX, base.player.motor.LookDirection.x);
		}
		if (base.player.motor.Ducking || base.player.motor.IsUsingSuperOrEx)
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveY, 0);
			this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, true);
		}
		else
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveY, base.player.motor.MoveDirection.y);
			this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, false);
		}
		this.SetInt(LevelPlayerAnimationController.Integers.LookX, base.player.motor.LookDirection.x);
		this.SetInt(LevelPlayerAnimationController.Integers.LookY, base.player.motor.LookDirection.y);
		this.SetBool(LevelPlayerAnimationController.Booleans.Shooting, base.player.weaponManager.IsShooting);
		float num = (!base.player.weaponManager.IsShooting && this.timeSinceStoppedShooting >= 0.0833f) ? 0f : 1f;
		if (!base.player.stats.isChalice)
		{
			base.animator.SetLayerWeight(1, num);
			base.animator.SetLayerWeight(2, (base.player.motor.LookDirection.y <= 0) ? 0f : num);
		}
		else
		{
			if (!base.player.motor.Grounded && base.animator.GetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX))
			{
				num = 0f;
			}
			if (!this.ExitingChaliceSuper())
			{
				base.animator.SetLayerWeight(4, 1f - num);
			}
			else
			{
				base.animator.SetLayerWeight(4, 0f);
			}
			base.animator.SetLayerWeight(5, num);
			base.animator.SetLayerWeight(6, (base.player.motor.LookDirection.y <= 0) ? 0f : num);
			if (base.player.motor.ChaliceDuckDashed && !base.player.motor.Grounded)
			{
				this.chaliceFellFromDuckDash = true;
			}
			if (base.player.motor.Grounded)
			{
				this.chaliceFellFromDuckDash = false;
			}
		}
		if (this.shooting)
		{
			this.timeSinceStoppedShooting = 0f;
		}
		else
		{
			this.timeSinceStoppedShooting += CupheadTime.Delta;
		}
		bool flag = false;
		if (this.fired && ((base.player.motor.Grounded && (base.player.motor.LookDirection.x == 0 || base.player.motor.Locked || base.player.motor.LookDirection.y < 0)) || (base.player.stats.isChalice && !base.player.motor.ChaliceDoubleJumped)))
		{
			this.SetTrigger(LevelPlayerAnimationController.Triggers.OnFire);
			flag = true;
		}
		this.fired = false;
		this.shooting = base.player.weaponManager.IsShooting;
		if (!this.shooting && !flag)
		{
			this.ResetTrigger(LevelPlayerAnimationController.Triggers.OnFire);
		}
		if (base.player.motor.Dashing && this.GetBool(LevelPlayerAnimationController.Booleans.Dashing) != base.player.motor.Dashing)
		{
			if (base.player.stats.isChalice)
			{
				base.animator.SetLayerWeight(3, 0f);
			}
			if (base.player.stats.isChalice && base.player.motor.Ducking)
			{
				this.ChaliceDuckDashHandling();
			}
			else
			{
				this.Play("Dash.Air");
				if (base.player.stats.Loadout.charm != Charm.charm_smoke_dash || !base.player.stats.CurseSmokeDash || Level.IsChessBoss || (base.player.stats.isChalice && !base.player.motor.Ducking))
				{
					this.dashEffect.Create(base.transform.position, base.transform.localScale);
				}
				if (base.player.stats.isChalice)
				{
					this.chaliceDashEffectActive = this.chaliceDashEffect.Create(base.transform.position, base.transform.localScale);
					this.chaliceDashEffectActive.transform.parent = base.transform;
				}
			}
		}
		this.SetBool(LevelPlayerAnimationController.Booleans.Dashing, base.player.motor.Dashing);
		if (!base.player.motor.Dashing)
		{
			if (base.player.motor.LookDirection.x != 0 && !this.ExitingChaliceSuper())
			{
				base.transform.SetScale(new float?(base.player.motor.LookDirection.x), null, null);
			}
		}
		else
		{
			base.transform.SetScale(new float?((float)base.player.motor.DashDirection), null, null);
		}
	}

	// Token: 0x06003CEB RID: 15595 RVA: 0x0021BAD1 File Offset: 0x00219ED1
	public void ResetMoveX()
	{
		this.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0);
		this.inScaredIntro = false;
	}

	// Token: 0x06003CEC RID: 15596 RVA: 0x0021BAE8 File Offset: 0x00219EE8
	private void ChaliceDoubleJumpFX()
	{
		float value = 0f;
		if (base.player.input.GetAxis(PlayerInput.Axis.X) > 0f || (base.player.input.GetAxis(PlayerInput.Axis.X) > 0f && base.player.input.GetAxis(PlayerInput.Axis.Y) > 0f))
		{
			value = -35f;
		}
		else if (base.player.input.GetAxis(PlayerInput.Axis.X) < 0f || (base.player.input.GetAxis(PlayerInput.Axis.X) < 0f && base.player.input.GetAxis(PlayerInput.Axis.Y) > 0f))
		{
			value = 35f;
		}
		Effect effect = this.chaliceDoubleJumpEffect.Create(base.transform.position);
		effect.transform.SetEulerAngles(null, null, new float?(value));
	}

	// Token: 0x06003CED RID: 15597 RVA: 0x0021BBE8 File Offset: 0x00219FE8
	private void ChaliceIncrementJumpDescendLoopCounter()
	{
		if (base.player.motor.MoveDirection.y < 0)
		{
			this.SetInt(LevelPlayerAnimationController.Integers.ChaliceJumpDescendLoopCounter, this.GetInt(LevelPlayerAnimationController.Integers.ChaliceJumpDescendLoopCounter) + 1);
		}
	}

	// Token: 0x06003CEE RID: 15598 RVA: 0x0021BC30 File Offset: 0x0021A030
	private void ChaliceResetJumpDescendLoopCounter()
	{
		this.SetInt(LevelPlayerAnimationController.Integers.ChaliceJumpDescendLoopCounter, 0);
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x0021BC40 File Offset: 0x0021A040
	private void ChaliceDuckDashHandling()
	{
		this.Play("Duck.Duck_Dash");
		AudioManager.Play("chalice_roll");
		if (this.chaliceInvincibleSparklesCoroutine != null)
		{
			base.StopCoroutine(this.chaliceInvincibleSparklesCoroutine);
			this.chaliceInvincibleSparklesCoroutine = null;
		}
		this.chaliceInvincibleSparklesCoroutine = base.StartCoroutine(this.chaliceInvincibleSparkle_cr());
	}

	// Token: 0x06003CF0 RID: 15600 RVA: 0x0021BC94 File Offset: 0x0021A094
	private IEnumerator chaliceInvincibleSparkle_cr()
	{
		for (;;)
		{
			float x = UnityEngine.Random.Range(-base.player.colliderManager.Width, base.player.colliderManager.Width);
			float y = UnityEngine.Random.Range(base.player.colliderManager.Height * -0.5f, base.player.colliderManager.Height * 1.5f);
			this.chaliceDuckDashSparkles.Create(base.player.transform.position + new Vector3(x, y, 0f));
			yield return CupheadTime.WaitForSeconds(this, 0.05f);
		}
		yield break;
	}

	// Token: 0x06003CF1 RID: 15601 RVA: 0x0021BCAF File Offset: 0x0021A0AF
	private void ChaliceJumpHandling()
	{
		this.SetBool(LevelPlayerAnimationController.Booleans.DoubleJump, base.player.motor.ChaliceDoubleJumped);
	}

	// Token: 0x06003CF2 RID: 15602 RVA: 0x0021BCCC File Offset: 0x0021A0CC
	private void ChaliceJumpShootHandling()
	{
		bool enabled = (base.player.weaponManager.IsShooting || this.timeSinceStoppedShooting < 0.0833f) && !base.player.motor.Grounded && !base.player.motor.Dashing && !base.player.motor.ChaliceDoubleJumped && !this.chaliceFellFromDuckDash && !this.GetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX) && !this.hitAnimation && !this.super;
		this.chaliceJumpShootRenderers[0].enabled = enabled;
		this.chaliceJumpShootRenderers[1].enabled = enabled;
		if (!base.player.motor.Grounded)
		{
			this.spriteRenderer.enabled = (!base.player.weaponManager.IsShooting && this.timeSinceStoppedShooting >= 0.0833f);
			if (base.player.motor.ChaliceDoubleJumped || this.chaliceFellFromDuckDash || base.player.motor.Dashing || this.GetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX) || this.hitAnimation)
			{
				this.spriteRenderer.enabled = true;
			}
		}
	}

	// Token: 0x06003CF3 RID: 15603 RVA: 0x0021BE30 File Offset: 0x0021A230
	private void ChaliceAimSpriteHandling()
	{
		if (base.player.motor.Locked)
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0);
		}
		else
		{
			this.SetInt(LevelPlayerAnimationController.Integers.MoveX, base.player.motor.LookDirection.x);
		}
		if (base.player.weaponManager.IsShooting || this.GetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle) || (this.GetInt(LevelPlayerAnimationController.Integers.MoveX) != 0 && !base.player.motor.Dashing) || base.player.motor.Dashing || base.player.motor.DashState == LevelPlayerMotor.DashManager.State.End || !base.player.motor.Grounded || this.inScaredIntro)
		{
			this.SwitchChaliceAim(-1);
			this.spriteRenderer.enabled = true;
		}
		else if (base.player.motor.LookDirection.x != 0)
		{
			this.SwitchChaliceAim(2);
			this.spriteRenderer.enabled = false;
			if (base.player.motor.LookDirection.y > 0)
			{
				this.SwitchChaliceAim(1);
				this.spriteRenderer.enabled = false;
			}
			else if (base.player.motor.LookDirection.y < 0)
			{
				this.SwitchChaliceAim(3);
				this.spriteRenderer.enabled = false;
			}
		}
		else if (base.player.motor.LookDirection.y > 0)
		{
			this.SwitchChaliceAim(0);
			this.spriteRenderer.enabled = false;
		}
		else if (base.player.motor.LookDirection.y < 0)
		{
			this.SwitchChaliceAim(4);
			this.spriteRenderer.enabled = false;
		}
		else
		{
			this.SwitchChaliceAim(-1);
			this.spriteRenderer.enabled = true;
		}
	}

	// Token: 0x06003CF4 RID: 15604 RVA: 0x0021C070 File Offset: 0x0021A470
	private void SwitchChaliceAim(int spriteToEnable)
	{
		for (int i = 0; i < this.chaliceSprites.Length; i++)
		{
			this.chaliceSprites[i].enabled = (i == spriteToEnable);
		}
	}

	// Token: 0x06003CF5 RID: 15605 RVA: 0x0021C0A8 File Offset: 0x0021A4A8
	private void ChaliceEndAirEX()
	{
		this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX, false);
		if (base.player.stats.isChalice && !base.player.motor.Grounded)
		{
			string text = this.exDirection;
			if (text != null)
			{
				if (!(text == "Forward"))
				{
					if (!(text == "Up") && !(text == "Down") && !(text == "Diagonal_Down"))
					{
						if (text == "Diagonal_Up")
						{
							base.animator.Play(this.ChaliceAirEXRecovery, 3, 0f);
						}
					}
					else
					{
						base.animator.Play(this.ChaliceAirEXRecovery, 3, 0.041666668f);
					}
				}
				else
				{
					base.animator.Play(this.ChaliceAirEXRecovery, 3, 0.083333336f);
				}
			}
		}
	}

	// Token: 0x06003CF6 RID: 15606 RVA: 0x0021C1A4 File Offset: 0x0021A5A4
	public void IsIntroB()
	{
		if (!base.player.stats.isChalice)
		{
			this.isIntroB = true;
			if ((base.player.id == PlayerId.PlayerOne && PlayerManager.player1IsMugman) || (base.player.id == PlayerId.PlayerTwo && !PlayerManager.player1IsMugman))
			{
				this.Play("Boil_Mugman");
			}
		}
	}

	// Token: 0x06003CF7 RID: 15607 RVA: 0x0021C210 File Offset: 0x0021A610
	public void CookieFail()
	{
		if (Level.Current.CurrentLevel == Levels.Bee && base.player.id == PlayerId.PlayerTwo)
		{
			base.transform.position += Vector3.left * 32f;
		}
		string str = ((base.player.id != PlayerId.PlayerOne || !PlayerManager.player1IsMugman) && (base.player.id != PlayerId.PlayerTwo || PlayerManager.player1IsMugman)) ? "Cuphead" : "Mugman";
		this.Play("Intro_Chalice_" + str + "_Fail");
	}

	// Token: 0x06003CF8 RID: 15608 RVA: 0x0021C2C4 File Offset: 0x0021A6C4
	public void ScaredChalice(bool showPortal)
	{
		this.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0);
		this.inScaredIntro = true;
		this.ActivateChaliceAnimationLayers();
		base.animator.Play("Intro_Chalice_Scared", 3);
		if (!showPortal)
		{
			return;
		}
		bool flag = (base.player.id == PlayerId.PlayerOne && PlayerManager.player1IsMugman) || (base.player.id == PlayerId.PlayerTwo && !PlayerManager.player1IsMugman);
		string text = (!flag) ? "Cuphead" : "Mugman";
		this.chaliceIntroAnimation.Create(base.transform.position, flag, true);
	}

	// Token: 0x06003CF9 RID: 15609 RVA: 0x0021C36B File Offset: 0x0021A76B
	public void ForceDirection()
	{
		this.lastTrueLookDir = base.player.motor.TrueLookDirection;
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x0021C384 File Offset: 0x0021A784
	private void InitializeCurseFX()
	{
		this.curseEffectAngle = (float)UnityEngine.Random.Range(0, 360);
		if (this.curseCharmLevel == 4 && this.paladinShadows != null)
		{
			this.paladinShadowPosition = new Vector3[10];
			this.paladinShadowScale = new Vector3[10];
			this.paladinShadowSprite = new Sprite[10];
			for (int i = 0; i < 10; i++)
			{
				this.paladinShadowPosition[i] = base.transform.position;
				this.paladinShadowSprite[i] = this.spriteRenderer.sprite;
				this.paladinShadowScale[i] = base.transform.localScale;
			}
			this.paladinShadows[0].transform.position = base.transform.position;
			this.paladinShadows[1].transform.position = base.transform.position;
			this.paladinShadows[0].sprite = this.spriteRenderer.sprite;
			this.paladinShadows[1].sprite = this.spriteRenderer.sprite;
			this.paladinShadows[0].enabled = true;
			this.paladinShadows[1].enabled = true;
			this.paladinShadows[0].transform.parent = null;
			this.paladinShadows[1].transform.parent = null;
		}
	}

	// Token: 0x06003CFB RID: 15611 RVA: 0x0021C4EC File Offset: 0x0021A8EC
	private void HandleCurseFX()
	{
		if (PauseManager.state == PauseManager.State.Paused || !this.showCurseFX)
		{
			return;
		}
		this.curseEffectTimer += CupheadTime.Delta;
		while (this.curseEffectTimer >= this.curseEffectDelay)
		{
			Effect effect = this.curseEffect.Create(base.player.center + MathUtils.AngleToDirection(this.curseEffectAngle) * this.curseDistanceRange.RandomFloat());
			string stateName = null;
			if (this.curseCharmLevel < 2)
			{
				stateName = ((!Rand.Bool()) ? "Flames" : "Cloud") + UnityEngine.Random.Range(0, 3).ToString();
			}
			if (this.curseCharmLevel == 2)
			{
				stateName = ((!Rand.Bool()) ? ("Dizzy" + UnityEngine.Random.Range(0, 4).ToString()) : ("Cloud" + UnityEngine.Random.Range(0, 3).ToString()));
			}
			if (this.curseCharmLevel == 3)
			{
				stateName = "Dizzy" + UnityEngine.Random.Range(0, 4).ToString();
			}
			if (this.curseCharmLevel == 4)
			{
				stateName = "Sparkle" + UnityEngine.Random.Range(0, 3).ToString();
			}
			effect.animator.Play(stateName);
			this.curseEffectAngle = (this.curseEffectAngle + this.curseAngleShiftRange.RandomFloat()) % 360f;
			this.curseEffectTimer -= this.curseEffectDelay;
		}
		if (this.curseCharmLevel == 4 && this.paladinShadows != null)
		{
			this.paladinShadows[0].enabled = !base.player.motor.Dashing;
			this.paladinShadows[1].enabled = !base.player.motor.Dashing;
			for (int i = 9; i > 0; i--)
			{
				this.paladinShadowPosition[i] = this.paladinShadowPosition[i - 1];
				this.paladinShadowScale[i] = this.paladinShadowScale[i - 1];
				this.paladinShadowSprite[i] = this.paladinShadowSprite[i - 1];
			}
			this.paladinShadowPosition[0] = base.transform.position;
			this.paladinShadowScale[0] = base.transform.localScale;
			this.paladinShadowSprite[0] = this.spriteRenderer.sprite;
			this.paladinShadows[0].transform.position = this.paladinShadowPosition[5];
			this.paladinShadows[1].transform.position = this.paladinShadowPosition[9];
			this.paladinShadows[0].transform.localScale = this.paladinShadowScale[5];
			this.paladinShadows[1].transform.localScale = this.paladinShadowScale[9];
			this.paladinShadows[0].sprite = this.paladinShadowSprite[5];
			this.paladinShadows[1].sprite = this.paladinShadowSprite[9];
		}
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x0021C87F File Offset: 0x0021AC7F
	public void UpdateAnimator()
	{
		this.Update();
	}

	// Token: 0x06003CFD RID: 15613 RVA: 0x0021C887 File Offset: 0x0021AC87
	public override void OnPause()
	{
		base.OnPause();
		this.SetAlpha(1f);
	}

	// Token: 0x06003CFE RID: 15614 RVA: 0x0021C89A File Offset: 0x0021AC9A
	private void OnGuiPause()
	{
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x0021C89C File Offset: 0x0021AC9C
	private void OnGuiUnpause()
	{
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x0021C89E File Offset: 0x0021AC9E
	public void OnShotFired()
	{
		this.fired = true;
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x0021C8A7 File Offset: 0x0021ACA7
	public void OnRevive(Vector3 pos)
	{
		base.animator.Play("Jump");
	}

	// Token: 0x06003D02 RID: 15618 RVA: 0x0021C8BC File Offset: 0x0021ACBC
	public void OnGravityReversed()
	{
		base.transform.SetScale(null, new float?(base.player.motor.GravityReversalMultiplier), null);
	}

	// Token: 0x06003D03 RID: 15619 RVA: 0x0021C8FB File Offset: 0x0021ACFB
	public override void OnLevelStart()
	{
		this.CheckIfChaliceAndActivate();
	}

	// Token: 0x06003D04 RID: 15620 RVA: 0x0021C903 File Offset: 0x0021AD03
	public void OnLevelWin()
	{
		base.player.damageReceiver.OnWin();
		this.SetTrigger(LevelPlayerAnimationController.Triggers.OnWin);
	}

	// Token: 0x06003D05 RID: 15621 RVA: 0x0021C920 File Offset: 0x0021AD20
	private void ActivateChaliceAnimationLayers()
	{
		base.animator.SetLayerWeight(3, 1f);
		base.animator.SetLayerWeight(4, 1f);
		this.SetChaliceSprites();
		this.chaliceActivated = true;
	}

	// Token: 0x06003D06 RID: 15622 RVA: 0x0021C951 File Offset: 0x0021AD51
	public void CheckIfChaliceAndActivate()
	{
		if (base.player.stats.isChalice)
		{
			this.ActivateChaliceAnimationLayers();
		}
	}

	// Token: 0x06003D07 RID: 15623 RVA: 0x0021C970 File Offset: 0x0021AD70
	private void StartChaliceIntroHold(bool fail)
	{
		if (Level.Current.Started || Level.Current.blockChalice)
		{
			return;
		}
		bool flag = (!PlayerManager.player1IsMugman && base.player.id == PlayerId.PlayerOne) || (PlayerManager.player1IsMugman && base.player.id != PlayerId.PlayerOne);
		if (fail)
		{
			base.animator.Play((!flag) ? "Intro_Chalice_Mugman_Fail_Start" : "Intro_Chalice_Cuphead_Fail_Start");
		}
		else
		{
			base.animator.Play("Intro_Chalice_Hold");
			this.chaliceIntroCurrent = this.chaliceIntroAnimation.Create(base.transform.position + Vector3.down * base.player.motor.DistanceToGround(), !flag, false);
			this.SetChaliceSprites();
		}
	}

	// Token: 0x06003D08 RID: 15624 RVA: 0x0021CA5C File Offset: 0x0021AE5C
	public void PlayIntro()
	{
		this.SetBool(LevelPlayerAnimationController.Booleans.Intro, true);
		bool flag = (base.player.id == PlayerId.PlayerOne && PlayerManager.player1IsMugman) || (base.player.id == PlayerId.PlayerTwo && !PlayerManager.player1IsMugman);
		string str = (!flag) ? "Cuphead" : "Mugman";
		if (SceneLoader.CurrentLevel != Levels.Devil && SceneLoader.CurrentLevel != Levels.Saltbaker)
		{
			if (base.player.stats.isChalice)
			{
				base.animator.Play("Idle", 0);
				base.animator.Play("Intro_Chalice_" + str, 3);
				if (this.chaliceIntroCurrent)
				{
					this.chaliceIntroCurrent.EndHold();
				}
				this.ActivateChaliceAnimationLayers();
			}
			else if (base.player.stats.Loadout.charm != Charm.charm_chalice || Level.Current.blockChalice)
			{
				string str2 = string.Empty;
				str2 = ((!this.isIntroB) ? "Intro_" : "Intro_B_");
				this.Play(str2 + str);
			}
		}
		else if (!base.player.stats.isChalice)
		{
			if (base.player.id == PlayerId.PlayerOne)
			{
				AudioManager.Play("player_scared_intro");
			}
			this.inScaredIntro = true;
			this.Play("Intro_Scared");
		}
	}

	// Token: 0x06003D09 RID: 15625 RVA: 0x0021CBE8 File Offset: 0x0021AFE8
	public void ScaredSprite(bool facingLeft)
	{
		base.animator.enabled = false;
		base.enabled = false;
		base.player.motor.enabled = false;
		if (base.player.id == PlayerId.PlayerOne)
		{
			this.cuphead.GetComponent<SpriteRenderer>().sprite = this.cupheadScaredSprite;
			this.cuphead.GetComponent<SpriteRenderer>().flipX = facingLeft;
		}
		else
		{
			this.mugman.GetComponent<SpriteRenderer>().sprite = this.mugmanScaredSprite;
			this.mugman.GetComponent<SpriteRenderer>().flipX = facingLeft;
		}
	}

	// Token: 0x06003D0A RID: 15626 RVA: 0x0021CC7C File Offset: 0x0021B07C
	public void LevelInit()
	{
		bool sprites = (!PlayerManager.player1IsMugman && base.player.id == PlayerId.PlayerOne) || (PlayerManager.player1IsMugman && base.player.id != PlayerId.PlayerOne);
		this.SetSprites(sprites);
	}

	// Token: 0x06003D0B RID: 15627 RVA: 0x0021CCCC File Offset: 0x0021B0CC
	public void SetSprites(bool isCuphead)
	{
		this.cuphead.SetActive(isCuphead);
		this.mugman.SetActive(!isCuphead);
		this.chalice.SetActive(false);
		if (isCuphead)
		{
			this.spriteRenderer = this.cuphead.GetComponent<SpriteRenderer>();
		}
		else
		{
			this.spriteRenderer = this.mugman.GetComponent<SpriteRenderer>();
		}
		this.tempMaterial = this.spriteRenderer.material;
	}

	// Token: 0x06003D0C RID: 15628 RVA: 0x0021CD3E File Offset: 0x0021B13E
	private void SetChaliceSprites()
	{
		this.cuphead.SetActive(false);
		this.mugman.SetActive(false);
		this.chalice.SetActive(true);
		this.spriteRenderer = this.chalice.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06003D0D RID: 15629 RVA: 0x0021CD75 File Offset: 0x0021B175
	public void EnableSpriteRenderer()
	{
		this.spriteRenderer.enabled = true;
	}

	// Token: 0x06003D0E RID: 15630 RVA: 0x0021CD84 File Offset: 0x0021B184
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (base.player.stats.SuperInvincible)
		{
			return;
		}
		CupheadLevelCamera.Current.Shake(20f, 0.6f, false);
		if (base.player.stats.Health == 4)
		{
			AudioManager.Play("player_damage_crack_level1");
		}
		else if (base.player.stats.Health == 3)
		{
			AudioManager.Play("player_damage_crack_level2");
		}
		else if (base.player.stats.Health == 2)
		{
			AudioManager.Play("player_damage_crack_level3");
		}
		else if (base.player.stats.Health == 1)
		{
			AudioManager.Play("player_damage_crack_level4");
		}
		AudioManager.Play("player_hit");
		bool grounded = base.player.motor.Grounded;
		if (grounded)
		{
			this.Play("Hit.Hit_Ground");
		}
		else
		{
			this.Play("Hit.Hit_Air");
		}
		this.hitAnimation = true;
		this.hitEffect.Create(base.player.center, base.transform.localScale);
	}

	// Token: 0x06003D0F RID: 15631 RVA: 0x0021CEBA File Offset: 0x0021B2BA
	public void OnHealerCharm()
	{
		this.healerCharmEffect.Create(base.player.center, base.transform.localScale, base.player);
		AudioManager.Play("sfx_player_charmhealer_extraheart");
	}

	// Token: 0x06003D10 RID: 15632 RVA: 0x0021CEF0 File Offset: 0x0021B2F0
	private void OnDashStart()
	{
		this.hitAnimation = false;
		if ((base.player.stats.Loadout.charm == Charm.charm_smoke_dash || base.player.stats.CurseSmokeDash) && !Level.IsChessBoss)
		{
			this.spriteRenderer.enabled = false;
			this.smokeDashEffect.Create(base.player.center);
		}
	}

	// Token: 0x06003D11 RID: 15633 RVA: 0x0021CF68 File Offset: 0x0021B368
	private void OnDashEnd()
	{
		if ((base.player.stats.Loadout.charm == Charm.charm_smoke_dash || base.player.stats.CurseSmokeDash) && !Level.IsChessBoss)
		{
			this.spriteRenderer.enabled = true;
			this.smokeDashEffect.Create(base.player.center);
		}
		if (!base.player.motor.Grounded && base.player.stats.isChalice)
		{
			base.animator.Play((!base.player.motor.ChaliceDoubleJumped) ? this.ChaliceJumpDescend : this.ChaliceJumpBall, 3, 0f);
		}
	}

	// Token: 0x06003D12 RID: 15634 RVA: 0x0021D037 File Offset: 0x0021B437
	private void OnRunDust()
	{
		if (base.enabled)
		{
			this.runEffect.Create(this.runDustRoot.position);
		}
	}

	// Token: 0x06003D13 RID: 15635 RVA: 0x0021D05B File Offset: 0x0021B45B
	private void OnChaliceDashSparkle()
	{
		if (base.enabled && base.player.stats.isChalice)
		{
			this.chaliceDashSparkle.Create(this.sparkleRoot.position);
		}
	}

	// Token: 0x06003D14 RID: 15636 RVA: 0x0021D094 File Offset: 0x0021B494
	private void OnBurst()
	{
		this.powerUpBurstEffect.Create(base.player.center);
	}

	// Token: 0x06003D15 RID: 15637 RVA: 0x0021D0AD File Offset: 0x0021B4AD
	private void onHitAnimationComplete()
	{
		this.hitAnimation = false;
	}

	// Token: 0x06003D16 RID: 15638 RVA: 0x0021D0B6 File Offset: 0x0021B4B6
	public void SetSpriteProperties(SpriteLayer layer, int order)
	{
		this.spriteRenderer.sortingLayerName = layer.ToString();
		this.spriteRenderer.sortingOrder = order;
	}

	// Token: 0x06003D17 RID: 15639 RVA: 0x0021D0DC File Offset: 0x0021B4DC
	public void ResetSpriteProperties()
	{
		this.spriteRenderer.sortingLayerName = SpriteLayer.Player.ToString();
		this.spriteRenderer.sortingOrder = ((base.player.id != PlayerId.PlayerOne) ? -1 : 1);
	}

	// Token: 0x06003D18 RID: 15640 RVA: 0x0021D128 File Offset: 0x0021B528
	private void OnParryStart()
	{
		if (this.super)
		{
			return;
		}
		if (base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, true);
		}
		if ((base.player.stats.Loadout.charm == Charm.charm_parry_attack || base.player.stats.CurseWhetsone) && !base.GetComponent<IParryAttack>().AttackParryUsed && !Level.IsChessBoss)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, true);
		}
		else if (base.player.stats.Loadout.charm == Charm.charm_curse)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, false);
		}
		this.SetTrigger(LevelPlayerAnimationController.Triggers.OnParry);
	}

	// Token: 0x06003D19 RID: 15641 RVA: 0x0021D20C File Offset: 0x0021B60C
	public void OnParrySuccess()
	{
		if (base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, false);
		}
		if ((base.player.stats.Loadout.charm == Charm.charm_parry_attack || base.player.stats.CurseWhetsone) && !Level.IsChessBoss)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, false);
		}
		this.SetAlpha(1f);
		if (base.player.stats.isChalice)
		{
			if (this.chaliceDashEffectActive != null)
			{
				UnityEngine.Object.Destroy(this.chaliceDashEffectActive.gameObject);
			}
			base.animator.Play("Jump_Launch", 3, 0f);
		}
	}

	// Token: 0x06003D1A RID: 15642 RVA: 0x0021D2EF File Offset: 0x0021B6EF
	public void OnParryPause()
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.animator.enabled = false;
			this.spriteRenderer.GetComponent<LevelPlayerParryAnimator>().StartSet();
		}
	}

	// Token: 0x06003D1B RID: 15643 RVA: 0x0021D31D File Offset: 0x0021B71D
	public void OnParryAnimEnd()
	{
		this.ResumeNormanAnim();
	}

	// Token: 0x06003D1C RID: 15644 RVA: 0x0021D325 File Offset: 0x0021B725
	public void _ChaliceStartOnIdle4()
	{
		if (base.player.stats.isChalice)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, false);
			base.animator.Play("IdleFromFour", 3);
		}
	}

	// Token: 0x06003D1D RID: 15645 RVA: 0x0021D359 File Offset: 0x0021B759
	public void ResumeNormanAnim()
	{
		this.spriteRenderer.GetComponent<LevelPlayerParryAnimator>().StopSet();
		base.animator.enabled = true;
	}

	// Token: 0x06003D1E RID: 15646 RVA: 0x0021D377 File Offset: 0x0021B777
	private void OnGrounded()
	{
		if (!Level.Current.Started)
		{
			return;
		}
		AudioManager.Play("player_grounded");
		this.groundedEffect.Create(base.transform.position, base.transform.localScale);
	}

	// Token: 0x06003D1F RID: 15647 RVA: 0x0021D3B8 File Offset: 0x0021B7B8
	private void OnEx()
	{
		if (base.player.stats.isChalice)
		{
			this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, true);
		}
		this.exDirection = "Forward";
		if (base.player.motor.LookDirection.x == 0 && base.player.motor.LookDirection.y > 0)
		{
			this.exDirection = "Up";
			AudioManager.Play("player_ex_forward_ground");
		}
		else if (base.player.motor.LookDirection.x != 0 && base.player.motor.LookDirection.y > 0)
		{
			this.exDirection = "Diagonal_Up";
			AudioManager.Play("player_ex_forward_ground");
		}
		else if (base.player.motor.LookDirection.x == 0 && base.player.motor.LookDirection.y < 0)
		{
			this.exDirection = "Down";
			AudioManager.Play("player_ex_forward_ground");
		}
		else if (base.player.motor.LookDirection.x != 0 && base.player.motor.LookDirection.y < 0)
		{
			this.exDirection = "Diagonal_Down";
			AudioManager.Play("player_ex_forward_ground");
		}
		if (this.exDirection == "Forward")
		{
			AudioManager.Play("player_ex_forward_ground");
		}
		string text = "Ex." + this.exDirection + "_";
		if (base.player.motor.Grounded)
		{
			text += "Ground";
		}
		else
		{
			text += "Air";
		}
		this.Play(text);
		this.SetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX, !base.player.motor.Grounded);
	}

	// Token: 0x06003D20 RID: 15648 RVA: 0x0021D600 File Offset: 0x0021BA00
	private void OnSuper()
	{
		Super super = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id).super;
		this.super = true;
		if (base.player.stats.isChalice)
		{
			this.shooting = false;
			this.ChaliceJumpShootHandling();
		}
		this.spriteRenderer.enabled = false;
		this.SwitchChaliceAim(-1);
	}

	// Token: 0x06003D21 RID: 15649 RVA: 0x0021D66C File Offset: 0x0021BA6C
	private void OnSuperEnd()
	{
		this.super = false;
		this.spriteRenderer.enabled = true;
		this.ResetSpriteProperties();
		if (base.player.stats.isChalice)
		{
			this.timeSinceStoppedShooting = 1f;
			if (base.player.stats.Loadout.super == Super.level_super_chalice_shield)
			{
				base.StartCoroutine(this.end_chalice_super_cr((!base.player.motor.Grounded) ? this.ChaliceSuper2ReturnAir : this.ChaliceSuper2Return));
			}
			if (base.player.stats.Loadout.super == Super.level_super_chalice_vert_beam)
			{
				base.StartCoroutine(this.end_chalice_super_cr(this.ChaliceSuper1Return));
			}
		}
	}

	// Token: 0x06003D22 RID: 15650 RVA: 0x0021D738 File Offset: 0x0021BB38
	private bool ExitingChaliceSuper()
	{
		int shortNameHash = base.animator.GetCurrentAnimatorStateInfo(3).shortNameHash;
		return shortNameHash == this.ChaliceSuper1Return || shortNameHash == this.ChaliceSuper2Return || shortNameHash == this.ChaliceSuper2ReturnAir;
	}

	// Token: 0x06003D23 RID: 15651 RVA: 0x0021D780 File Offset: 0x0021BB80
	private IEnumerator end_chalice_super_cr(int animState)
	{
		base.animator.Play(animState, 3, 0f);
		base.animator.Update(0f);
		if (base.player.weaponManager.allowInput)
		{
			base.player.weaponManager.DisableInput();
			while (base.animator.GetCurrentAnimatorStateInfo(3).shortNameHash == animState)
			{
				yield return null;
			}
			base.player.weaponManager.EnableInput();
		}
		yield break;
	}

	// Token: 0x06003D24 RID: 15652 RVA: 0x0021D7A2 File Offset: 0x0021BBA2
	private void _OnSuperAnimEnd()
	{
		base.player.UnpauseAll(false);
		base.player.motor.OnSuperEnd();
	}

	// Token: 0x06003D25 RID: 15653 RVA: 0x0021D7C0 File Offset: 0x0021BBC0
	public void SetOldMaterial()
	{
		this.spriteRenderer.material = this.tempMaterial;
	}

	// Token: 0x06003D26 RID: 15654 RVA: 0x0021D7D3 File Offset: 0x0021BBD3
	public void SetMaterial(Material m)
	{
		this.tempMaterial = this.spriteRenderer.material;
		this.spriteRenderer.material = m;
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x0021D7F2 File Offset: 0x0021BBF2
	public Material GetMaterial()
	{
		return this.spriteRenderer.material;
	}

	// Token: 0x06003D28 RID: 15656 RVA: 0x0021D7FF File Offset: 0x0021BBFF
	public SpriteRenderer GetSpriteRenderer()
	{
		return this.spriteRenderer;
	}

	// Token: 0x06003D29 RID: 15657 RVA: 0x0021D807 File Offset: 0x0021BC07
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.dashEffect = null;
		this.groundedEffect = null;
		this.hitEffect = null;
		this.runEffect = null;
		this.smokeDashEffect = null;
		this.powerUpBurstEffect = null;
		this.cupheadScaredSprite = null;
		this.mugmanScaredSprite = null;
	}

	// Token: 0x06003D2A RID: 15658 RVA: 0x0021D847 File Offset: 0x0021BC47
	protected void Play(string animation)
	{
		base.animator.Play(animation, 0, 0f);
	}

	// Token: 0x06003D2B RID: 15659 RVA: 0x0021D85B File Offset: 0x0021BC5B
	protected bool GetBool(int b)
	{
		return base.animator.GetBool(b);
	}

	// Token: 0x06003D2C RID: 15660 RVA: 0x0021D869 File Offset: 0x0021BC69
	protected void SetBool(int b, bool value)
	{
		base.animator.SetBool(b, value);
	}

	// Token: 0x06003D2D RID: 15661 RVA: 0x0021D878 File Offset: 0x0021BC78
	protected int GetInt(int i)
	{
		return base.animator.GetInteger(i);
	}

	// Token: 0x06003D2E RID: 15662 RVA: 0x0021D886 File Offset: 0x0021BC86
	protected void SetInt(int i, int value)
	{
		base.animator.SetInteger(i, value);
	}

	// Token: 0x06003D2F RID: 15663 RVA: 0x0021D895 File Offset: 0x0021BC95
	protected void SetTrigger(int t)
	{
		base.animator.SetTrigger(t);
	}

	// Token: 0x06003D30 RID: 15664 RVA: 0x0021D8A3 File Offset: 0x0021BCA3
	protected void ResetTrigger(int t)
	{
		base.animator.ResetTrigger(t);
	}

	// Token: 0x06003D31 RID: 15665 RVA: 0x0021D8B4 File Offset: 0x0021BCB4
	private void SetAlpha(float a)
	{
		Color color = this.spriteRenderer.color;
		color.a = a;
		this.spriteRenderer.color = color;
	}

	// Token: 0x06003D32 RID: 15666 RVA: 0x0021D8E4 File Offset: 0x0021BCE4
	public void SetColor(Color color)
	{
		float a = this.spriteRenderer.color.a;
		color.a = a;
		this.spriteRenderer.color = color;
	}

	// Token: 0x06003D33 RID: 15667 RVA: 0x0021D91C File Offset: 0x0021BD1C
	public void ResetColor()
	{
		float a = this.spriteRenderer.color.a;
		this.spriteRenderer.color = new Color(1f, 1f, 1f, a);
	}

	// Token: 0x06003D34 RID: 15668 RVA: 0x0021D95D File Offset: 0x0021BD5D
	public void SetColorOverTime(Color color, float time)
	{
		this.StopColorCoroutine();
		this.colorCoroutine = this.setColor_cr(color, time);
		base.StartCoroutine(this.colorCoroutine);
	}

	// Token: 0x06003D35 RID: 15669 RVA: 0x0021D980 File Offset: 0x0021BD80
	public void StopColorCoroutine()
	{
		if (this.colorCoroutine != null)
		{
			base.StopCoroutine(this.colorCoroutine);
		}
		this.colorCoroutine = null;
	}

	// Token: 0x06003D36 RID: 15670 RVA: 0x0021D9A0 File Offset: 0x0021BDA0
	private IEnumerator setColor_cr(Color color, float time)
	{
		float t = 0f;
		Color startColor = this.spriteRenderer.color;
		while (t < time)
		{
			float val = t / time;
			this.SetColor(Color.Lerp(startColor, color, val));
			t += CupheadTime.Delta;
			yield return null;
		}
		this.SetColor(color);
		yield return null;
		yield break;
	}

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x06003D37 RID: 15671 RVA: 0x0021D9C9 File Offset: 0x0021BDC9
	private bool Flashing
	{
		get
		{
			return base.player.damageReceiver.state == PlayerDamageReceiver.State.Invulnerable;
		}
	}

	// Token: 0x06003D38 RID: 15672 RVA: 0x0021D9E0 File Offset: 0x0021BDE0
	private IEnumerator flash_cr()
	{
		float t = 0f;
		for (;;)
		{
			while (!this.Flashing)
			{
				yield return true;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.417f);
			while (this.Flashing)
			{
				this.SetAlpha(0.3f);
				t = 0f;
				while (t < 0.05f)
				{
					if (!this.Flashing)
					{
						this.SetAlpha(1f);
						break;
					}
					t += base.LocalDeltaTime;
					yield return null;
				}
				if (!this.Flashing)
				{
					this.SetAlpha(1f);
					break;
				}
				this.SetAlpha(1f);
				t = 0f;
				while (t < 0.2f)
				{
					if (!this.Flashing)
					{
						this.SetAlpha(1f);
						break;
					}
					t += base.LocalDeltaTime;
					yield return null;
				}
				if (!this.Flashing)
				{
					this.SetAlpha(1f);
					break;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003D39 RID: 15673 RVA: 0x0021D9FB File Offset: 0x0021BDFB
	private void SoundIntroPowerup()
	{
		if (!this.intropowerupactive)
		{
			AudioManager.Play("player_powerup");
			this.emitAudioFromObject.Add("player_powerup");
			this.intropowerupactive = true;
		}
	}

	// Token: 0x06003D3A RID: 15674 RVA: 0x0021DA29 File Offset: 0x0021BE29
	private void SoundParryAxe()
	{
		AudioManager.Play("player_parry_axe");
		this.emitAudioFromObject.Add("player_parry_axe");
	}

	// Token: 0x04004430 RID: 17456
	private const int PALADIN_SHADOW_BUFFER_SIZE = 10;

	// Token: 0x04004431 RID: 17457
	private int ChaliceSuper1Return = Animator.StringToHash("Chalice_Super_1_Return");

	// Token: 0x04004432 RID: 17458
	private int ChaliceSuper2Return = Animator.StringToHash("Chalice_Super_2_Return");

	// Token: 0x04004433 RID: 17459
	private int ChaliceSuper2ReturnAir = Animator.StringToHash("Chalice_Super_2_Return_Air");

	// Token: 0x04004434 RID: 17460
	private int ChaliceAirEXRecovery = Animator.StringToHash("Air_EX_Recovery");

	// Token: 0x04004435 RID: 17461
	private int ChaliceJumpBall = Animator.StringToHash("Jump_Ball");

	// Token: 0x04004436 RID: 17462
	private int ChaliceJumpDescend = Animator.StringToHash("Jump_Descend");

	// Token: 0x04004437 RID: 17463
	[SerializeField]
	private GameObject cuphead;

	// Token: 0x04004438 RID: 17464
	[SerializeField]
	private GameObject mugman;

	// Token: 0x04004439 RID: 17465
	[SerializeField]
	private GameObject chalice;

	// Token: 0x0400443A RID: 17466
	[SerializeField]
	private SpriteRenderer[] chaliceSprites;

	// Token: 0x0400443B RID: 17467
	[Space(10f)]
	[SerializeField]
	private Transform runDustRoot;

	// Token: 0x0400443C RID: 17468
	[SerializeField]
	private Transform sparkleRoot;

	// Token: 0x0400443D RID: 17469
	[Space(10f)]
	[SerializeField]
	private Effect dashEffect;

	// Token: 0x0400443E RID: 17470
	[SerializeField]
	private Effect groundedEffect;

	// Token: 0x0400443F RID: 17471
	[SerializeField]
	private Effect hitEffect;

	// Token: 0x04004440 RID: 17472
	[SerializeField]
	private Effect runEffect;

	// Token: 0x04004441 RID: 17473
	[SerializeField]
	private Effect curseEffect;

	// Token: 0x04004442 RID: 17474
	[SerializeField]
	private Effect smokeDashEffect;

	// Token: 0x04004443 RID: 17475
	[SerializeField]
	private HealerCharmSparkEffect healerCharmEffect;

	// Token: 0x04004444 RID: 17476
	[SerializeField]
	private Effect powerUpBurstEffect;

	// Token: 0x04004445 RID: 17477
	[SerializeField]
	private Effect chaliceDoubleJumpEffect;

	// Token: 0x04004446 RID: 17478
	[SerializeField]
	private Effect chaliceDashEffect;

	// Token: 0x04004447 RID: 17479
	private Effect chaliceDashEffectActive;

	// Token: 0x04004448 RID: 17480
	[SerializeField]
	private Effect chaliceDashSparkle;

	// Token: 0x04004449 RID: 17481
	[SerializeField]
	private SpriteRenderer[] chaliceJumpShootRenderers;

	// Token: 0x0400444A RID: 17482
	[SerializeField]
	private Material chaliceDuckDashMaterial;

	// Token: 0x0400444B RID: 17483
	[SerializeField]
	private Effect chaliceDuckDashSparkles;

	// Token: 0x0400444C RID: 17484
	private Coroutine chaliceInvincibleSparklesCoroutine;

	// Token: 0x0400444D RID: 17485
	private bool chaliceFellFromDuckDash;

	// Token: 0x0400444E RID: 17486
	[SerializeField]
	private LevelPlayerChaliceIntroAnimation chaliceIntroAnimation;

	// Token: 0x0400444F RID: 17487
	private LevelPlayerChaliceIntroAnimation chaliceIntroCurrent;

	// Token: 0x04004450 RID: 17488
	[SerializeField]
	private Sprite cupheadScaredSprite;

	// Token: 0x04004451 RID: 17489
	[SerializeField]
	private Sprite mugmanScaredSprite;

	// Token: 0x04004453 RID: 17491
	private bool hitAnimation;

	// Token: 0x04004454 RID: 17492
	private bool super;

	// Token: 0x04004455 RID: 17493
	private bool shooting;

	// Token: 0x04004456 RID: 17494
	private bool fired;

	// Token: 0x04004457 RID: 17495
	private bool intropowerupactive;

	// Token: 0x04004458 RID: 17496
	private string exDirection;

	// Token: 0x04004459 RID: 17497
	private Trilean2 lastTrueLookDir = new Trilean2(1, 0);

	// Token: 0x0400445A RID: 17498
	private float timeSinceStoppedShooting = 100f;

	// Token: 0x0400445B RID: 17499
	private Material tempMaterial;

	// Token: 0x0400445C RID: 17500
	private const float STOP_SHOOTING_DELAY = 0.0833f;

	// Token: 0x0400445D RID: 17501
	private bool isIntroB;

	// Token: 0x0400445E RID: 17502
	private bool chaliceActivated;

	// Token: 0x0400445F RID: 17503
	private bool inScaredIntro;

	// Token: 0x04004460 RID: 17504
	[SerializeField]
	private float curseEffectDelay = 0.15f;

	// Token: 0x04004461 RID: 17505
	[SerializeField]
	private MinMax curseAngleShiftRange = new MinMax(60f, 300f);

	// Token: 0x04004462 RID: 17506
	[SerializeField]
	private MinMax curseDistanceRange = new MinMax(0f, 20f);

	// Token: 0x04004463 RID: 17507
	private float curseEffectAngle;

	// Token: 0x04004464 RID: 17508
	private float curseEffectTimer;

	// Token: 0x04004465 RID: 17509
	private int curseCharmLevel = -1;

	// Token: 0x04004466 RID: 17510
	private Vector3[] paladinShadowPosition;

	// Token: 0x04004467 RID: 17511
	private Vector3[] paladinShadowScale;

	// Token: 0x04004468 RID: 17512
	private Sprite[] paladinShadowSprite;

	// Token: 0x04004469 RID: 17513
	[SerializeField]
	private SpriteRenderer[] paladinShadows;

	// Token: 0x0400446A RID: 17514
	private bool showCurseFX;

	// Token: 0x0400446B RID: 17515
	private IEnumerator colorCoroutine;

	// Token: 0x02000A12 RID: 2578
	private static class Booleans
	{
		// Token: 0x0400446C RID: 17516
		public static readonly int Dashing = Animator.StringToHash("Dashing");

		// Token: 0x0400446D RID: 17517
		public static readonly int Locked = Animator.StringToHash("Locked");

		// Token: 0x0400446E RID: 17518
		public static readonly int Shooting = Animator.StringToHash("Shooting");

		// Token: 0x0400446F RID: 17519
		public static readonly int Grounded = Animator.StringToHash("Grounded");

		// Token: 0x04004470 RID: 17520
		public static readonly int Turning = Animator.StringToHash("Turning");

		// Token: 0x04004471 RID: 17521
		public static readonly int Intro = Animator.StringToHash("Intro");

		// Token: 0x04004472 RID: 17522
		public static readonly int Dead = Animator.StringToHash("Dead");

		// Token: 0x04004473 RID: 17523
		public static readonly int HasParryCharm = Animator.StringToHash("HasParryCharm");

		// Token: 0x04004474 RID: 17524
		public static readonly int HasParryAttack = Animator.StringToHash("HasParryAttack");

		// Token: 0x04004475 RID: 17525
		public static readonly int ChaliceOffIdle = Animator.StringToHash("ChaliceOffIdle");

		// Token: 0x04004476 RID: 17526
		public static readonly int DoubleJump = Animator.StringToHash("DoubleJump");

		// Token: 0x04004477 RID: 17527
		public static readonly int ChaliceAirEX = Animator.StringToHash("ChaliceAirEX");
	}

	// Token: 0x02000A13 RID: 2579
	private static class Integers
	{
		// Token: 0x04004478 RID: 17528
		public static readonly int MoveX = Animator.StringToHash("MoveX");

		// Token: 0x04004479 RID: 17529
		public static readonly int MoveY = Animator.StringToHash("MoveY");

		// Token: 0x0400447A RID: 17530
		public static readonly int LookX = Animator.StringToHash("LookX");

		// Token: 0x0400447B RID: 17531
		public static readonly int LookY = Animator.StringToHash("LookY");

		// Token: 0x0400447C RID: 17532
		public static readonly int ChaliceJumpDescendLoopCounter = Animator.StringToHash("ChaliceJumpDescendLoopCounter");
	}

	// Token: 0x02000A14 RID: 2580
	private static class Triggers
	{
		// Token: 0x0400447D RID: 17533
		public static readonly int OnJump = Animator.StringToHash("OnJump");

		// Token: 0x0400447E RID: 17534
		public static readonly int OnGround = Animator.StringToHash("OnGround");

		// Token: 0x0400447F RID: 17535
		public static readonly int OnParry = Animator.StringToHash("OnParry");

		// Token: 0x04004480 RID: 17536
		public static readonly int OnWin = Animator.StringToHash("OnWin");

		// Token: 0x04004481 RID: 17537
		public static readonly int OnTurn = Animator.StringToHash("OnTurn");

		// Token: 0x04004482 RID: 17538
		public static readonly int OnFire = Animator.StringToHash("OnFire");
	}

	// Token: 0x02000A15 RID: 2581
	private enum AnimLayers
	{
		// Token: 0x04004484 RID: 17540
		Base,
		// Token: 0x04004485 RID: 17541
		ShootRun,
		// Token: 0x04004486 RID: 17542
		ShootRunDiag,
		// Token: 0x04004487 RID: 17543
		ChaliceSpecial,
		// Token: 0x04004488 RID: 17544
		ChaliceSync,
		// Token: 0x04004489 RID: 17545
		ChaliceShootRun,
		// Token: 0x0400448A RID: 17546
		ChaliceShootRunDiag
	}

	// Token: 0x02000A16 RID: 2582
	private enum ChaliceAim
	{
		// Token: 0x0400448C RID: 17548
		UpAim,
		// Token: 0x0400448D RID: 17549
		DiagUpAim,
		// Token: 0x0400448E RID: 17550
		ForwardAim,
		// Token: 0x0400448F RID: 17551
		DiagDownAim,
		// Token: 0x04004490 RID: 17552
		DownAim
	}
}
