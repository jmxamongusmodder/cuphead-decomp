using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A91 RID: 2705
public class PlanePlayerAnimationController : AbstractPlanePlayerComponent
{
	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x060040A8 RID: 16552 RVA: 0x00232C04 File Offset: 0x00231004
	private Transform activeTransform
	{
		get
		{
			if (base.player.stats.isChalice)
			{
				return this.chalice;
			}
			if (PlayerManager.player1IsMugman && base.player.id == PlayerId.PlayerOne)
			{
				return this.mugman;
			}
			return this.cuphead;
		}
	}

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x060040A9 RID: 16553 RVA: 0x00232C54 File Offset: 0x00231054
	// (set) Token: 0x060040AA RID: 16554 RVA: 0x00232C5C File Offset: 0x0023105C
	public SpriteRenderer spriteRenderer { get; private set; }

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x060040AB RID: 16555 RVA: 0x00232C65 File Offset: 0x00231065
	// (set) Token: 0x060040AC RID: 16556 RVA: 0x00232C6D File Offset: 0x0023106D
	public PlanePlayerAnimationController.ShrinkStates ShrinkState { get; set; }

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x060040AD RID: 16557 RVA: 0x00232C76 File Offset: 0x00231076
	// (set) Token: 0x060040AE RID: 16558 RVA: 0x00232C7E File Offset: 0x0023107E
	public bool Shrinking { get; private set; }

	// Token: 0x140000A0 RID: 160
	// (add) Token: 0x060040AF RID: 16559 RVA: 0x00232C88 File Offset: 0x00231088
	// (remove) Token: 0x060040B0 RID: 16560 RVA: 0x00232CC0 File Offset: 0x002310C0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnExFireAnimEvent;

	// Token: 0x140000A1 RID: 161
	// (add) Token: 0x060040B1 RID: 16561 RVA: 0x00232CF8 File Offset: 0x002310F8
	// (remove) Token: 0x060040B2 RID: 16562 RVA: 0x00232D30 File Offset: 0x00231130
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnShrinkEvent;

	// Token: 0x060040B3 RID: 16563 RVA: 0x00232D68 File Offset: 0x00231168
	private void Start()
	{
		base.player.weaponManager.OnExStartEvent += this.OnExStart;
		base.player.weaponManager.OnSuperStartEvent += this.OnSuperStart;
		base.player.parryController.OnParryStartEvent += this.OnParryStart;
		base.player.parryController.OnParrySuccessEvent += this.OnParrySuccess;
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.player.stats.OnPlayerDeathEvent += this.OnDeath;
		base.player.OnReviveEvent += this.OnRevive;
		base.player.stats.OnStoneShake += this.onStoneShake;
		base.player.stats.OnStoned += this.onStoned;
		if (this.spriteRenderer == null)
		{
			this.spriteRenderer = this.playerSprite.GetComponent<SpriteRenderer>();
		}
		PlayerRecolorHandler.SetChaliceRecolorEnabled(this.chalice.GetComponent<SpriteRenderer>().sharedMaterial, SettingsData.Data.filter == BlurGamma.Filter.Chalice);
		if (base.player.stats.Loadout.charm == Charm.charm_curse)
		{
			this.curseCharmLevel = CharmCurse.CalculateLevel(base.player.id);
		}
		if (this.curseCharmLevel > -1)
		{
			this.InitializeCurseFX();
		}
	}

	// Token: 0x060040B4 RID: 16564 RVA: 0x00232EF7 File Offset: 0x002312F7
	private void OnEnable()
	{
		base.StartCoroutine(this.flash_cr());
		this.CheckActivateCurseFX();
	}

	// Token: 0x060040B5 RID: 16565 RVA: 0x00232F0C File Offset: 0x0023130C
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

	// Token: 0x060040B6 RID: 16566 RVA: 0x00232F59 File Offset: 0x00231359
	private void Update()
	{
		if (this.curseCharmLevel > -1)
		{
			this.HandleCurseFX();
		}
	}

	// Token: 0x060040B7 RID: 16567 RVA: 0x00232F70 File Offset: 0x00231370
	private void FixedUpdate()
	{
		this.HandleRotation();
		this.HandleShrunk();
		this.SetInteger("Y", base.player.motor.MoveDirection.y);
	}

	// Token: 0x060040B8 RID: 16568 RVA: 0x00232FB4 File Offset: 0x002313B4
	public void LevelInit()
	{
		PlayerId id = base.player.id;
		if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
		{
			this.playerSprite = ((!base.player.stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.cuphead : this.mugman) : this.chalice);
		}
		else
		{
			this.playerSprite = ((!base.player.stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.mugman : this.cuphead) : this.chalice);
		}
		this.cuphead.gameObject.SetActive(false);
		this.mugman.gameObject.SetActive(false);
		this.chalice.gameObject.SetActive(false);
		if (Level.Current.Started)
		{
			this.playerSprite.gameObject.SetActive(true);
		}
	}

	// Token: 0x060040B9 RID: 16569 RVA: 0x002330C0 File Offset: 0x002314C0
	public void PlayIntro()
	{
		string str = ((base.player.id != PlayerId.PlayerOne || !PlayerManager.player1IsMugman) && (base.player.id != PlayerId.PlayerTwo || PlayerManager.player1IsMugman)) ? "Cuphead" : "Mugman";
		if (base.player.stats.isChalice)
		{
			base.animator.Play("Intro_Chalice_" + str + ((base.player.id != PlayerId.PlayerOne) ? "_P2" : string.Empty));
		}
		else if (base.player.stats.Loadout.charm == Charm.charm_chalice && !base.player.stats.isChalice)
		{
			base.animator.Play("Intro_Chalice_" + str + "_Fail");
		}
		else
		{
			PlayerId id = base.player.id;
			if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
			{
				base.animator.Play("Intro");
			}
			else
			{
				base.animator.Play("Intro_Alt");
			}
		}
		this.spriteRenderer = this.playerSprite.GetComponent<SpriteRenderer>();
		this.playerSprite.gameObject.SetActive(true);
		if (base.gameObject.activeSelf)
		{
			if (!Level.Current.Started && base.player.id == PlayerId.PlayerTwo && base.player.stats.Loadout.charm != Charm.charm_chalice)
			{
				this.playerSprite.SetLocalPosition(new float?(this.introRoot.transform.localPosition.x), new float?(this.introRoot.transform.localPosition.y), new float?(0f));
			}
			base.StartCoroutine(this.done_intro_cr());
		}
	}

	// Token: 0x060040BA RID: 16570 RVA: 0x002332CC File Offset: 0x002316CC
	private IEnumerator done_intro_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, (base.player.id != PlayerId.PlayerOne) ? "Intro_Alt" : "Intro", false, true);
		base.StartCoroutine(this.puff_cr());
		this.CheckActivateCurseFX();
		yield return null;
		yield break;
	}

	// Token: 0x060040BB RID: 16571 RVA: 0x002332E8 File Offset: 0x002316E8
	private void CheckActivateCurseFX()
	{
		if (this.curseCharmLevel == 4 && this.paladinShadows != null && this.paladinShadowSprite.Length == 10)
		{
			if (this.paladinShadows[0] != null)
			{
				this.paladinShadows[0].enabled = true;
			}
			if (this.paladinShadows[1] != null)
			{
				this.paladinShadows[1].enabled = true;
			}
		}
		this.showCurseFX = true;
	}

	// Token: 0x060040BC RID: 16572 RVA: 0x00233364 File Offset: 0x00231764
	private void ResetPosition()
	{
		this.playerSprite.SetLocalPosition(new float?(0f), new float?(0f), null);
	}

	// Token: 0x060040BD RID: 16573 RVA: 0x0023339C File Offset: 0x0023179C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (base.player.stats.Health <= 0 || info.damage <= 0f)
		{
			return;
		}
		this.hitSparkEffect.Create(base.player.center);
		this.hitDustEffect.Create(base.player.center);
		CupheadLevelCamera.Current.Shake(20f, 0.6f, false);
	}

	// Token: 0x060040BE RID: 16574 RVA: 0x00233413 File Offset: 0x00231813
	public void OnHealerCharm()
	{
		this.healerCharmEffect.Create(base.player.center, base.transform.localScale, base.player);
		AudioManager.Play("player_charmhealer_extraheart");
	}

	// Token: 0x060040BF RID: 16575 RVA: 0x00233446 File Offset: 0x00231846
	public void SetOldMaterial()
	{
		this.spriteRenderer.material = this.tempMaterial;
	}

	// Token: 0x060040C0 RID: 16576 RVA: 0x00233459 File Offset: 0x00231859
	public void SetMaterial(Material m)
	{
		this.tempMaterial = this.spriteRenderer.material;
		this.spriteRenderer.material = m;
	}

	// Token: 0x060040C1 RID: 16577 RVA: 0x00233478 File Offset: 0x00231878
	public Material GetMaterial()
	{
		return this.spriteRenderer.material;
	}

	// Token: 0x060040C2 RID: 16578 RVA: 0x00233485 File Offset: 0x00231885
	public SpriteRenderer GetSpriteRenderer()
	{
		return this.spriteRenderer;
	}

	// Token: 0x060040C3 RID: 16579 RVA: 0x00233490 File Offset: 0x00231890
	private void onStoned()
	{
		this.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Ready;
		base.animator.SetLayerWeight(1, 0f);
		base.StopCoroutine(this.ex_cr());
		this.greenPrefab.Create(base.player.center);
		base.animator.Play("Stone_Idle");
		base.animator.ResetTrigger("Breakout");
		base.StartCoroutine(this.stone_animation_cr());
		base.StartCoroutine(this.create_poofs_cr());
		this.isStoned = true;
	}

	// Token: 0x060040C4 RID: 16580 RVA: 0x0023351C File Offset: 0x0023191C
	private IEnumerator stone_animation_cr()
	{
		while (base.player.stats.StoneTime > 0f)
		{
			yield return null;
		}
		base.animator.SetTrigger("Breakout");
		AnimatorStateInfo animState = base.animator.GetCurrentAnimatorStateInfo(0);
		while (animState.IsName("Stone_Idle") || animState.IsName("Stone_Shake_A") || animState.IsName("Stone_Shake_B") || animState.IsName("Stone_Shake_C") || animState.IsName("Stone_Shake_C") || animState.IsName("Stone_Shake_D") || animState.IsName("Stone_Shake_E") || animState.IsName("Breakout"))
		{
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060040C5 RID: 16581 RVA: 0x00233537 File Offset: 0x00231937
	private void Breakout()
	{
		this.isStoned = false;
		this.breakoutPrefab.Create(base.player.center).transform.parent = base.transform;
		base.StopCoroutine(this.create_poofs_cr());
	}

	// Token: 0x060040C6 RID: 16582 RVA: 0x00233572 File Offset: 0x00231972
	private void onStoneShake()
	{
		base.animator.SetTrigger("Shake");
	}

	// Token: 0x060040C7 RID: 16583 RVA: 0x00233584 File Offset: 0x00231984
	private IEnumerator create_poofs_cr()
	{
		float t = 0f;
		float time = 0.1f;
		while (base.player.stats.StoneTime > 0f)
		{
			if (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Stone_Idle") && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Breakout"))
			{
				string layerName = (!Rand.Bool()) ? SpriteLayer.Effects.ToString() : SpriteLayer.Enemies.ToString();
				Effect poof = UnityEngine.Object.Instantiate<Effect>(this.poofPrefab);
				poof.transform.position = base.player.center;
				poof.animator.SetInteger("Poof", UnityEngine.Random.Range(0, 3));
				poof.GetComponent<SpriteRenderer>().sortingLayerName = layerName;
				while (t < time)
				{
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060040C8 RID: 16584 RVA: 0x002335A0 File Offset: 0x002319A0
	private void HandleRotation()
	{
		float num = 0f;
		if (base.player.motor.MoveDirection.x < 0)
		{
			num = 9f;
		}
		else if (base.player.motor.MoveDirection.x > 0)
		{
			num = -9f;
		}
		if (base.player.Shrunk && !base.player.stats.isChalice)
		{
			num += 5f * (float)(-(float)base.player.motor.MoveDirection.x);
		}
		this.rotation = Mathf.Lerp(this.rotation, num, 7f * CupheadTime.FixedDelta);
		this.activeTransform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.rotation));
	}

	// Token: 0x060040C9 RID: 16585 RVA: 0x002336A0 File Offset: 0x00231AA0
	private void HandleShrunk()
	{
		if (this.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Cooldown)
		{
			if (this.shrinkCooldownTimeLeft <= 0f)
			{
				this.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Ready;
			}
			this.shrinkCooldownTimeLeft -= CupheadTime.FixedDelta;
		}
		if (base.player.Parrying || base.player.WeaponBusy || base.player.stats.StoneTime > 0f || this.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Cooldown)
		{
			return;
		}
		if (this.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Ready && (base.player.input.actions.GetButtonDown(7) || base.player.input.actions.GetButtonDown(6)))
		{
			base.animator.SetLayerWeight(1, 1f);
			base.animator.Play("Shrink_In", 0);
			this.Shrinking = true;
			this.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Shrunk;
			if (this.OnShrinkEvent != null)
			{
				this.OnShrinkEvent();
			}
			if (base.player.stats.Loadout.charm == Charm.charm_smoke_dash || base.player.stats.CurseSmokeDash)
			{
				this.smokeDashEffect.Create(base.player.center);
			}
			AudioManager.Play("player_plane_shrink");
		}
		if (this.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Shrunk && !base.player.input.actions.GetButton(7) && !base.player.input.actions.GetButton(6))
		{
			this.Shrinking = false;
			base.animator.SetLayerWeight(1, 0f);
			base.animator.Play("Shrink_Out", 0);
			this.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Cooldown;
			this.shrinkCooldownTimeLeft = 0.23300001f;
			AudioManager.Play("player_plane_expand");
		}
	}

	// Token: 0x060040CA RID: 16586 RVA: 0x0023388C File Offset: 0x00231C8C
	private IEnumerator bomb_cr()
	{
		yield return null;
		base.animator.SetLayerWeight(2, 1f);
		float t = 0f;
		float[] slowShakeScales = new float[]
		{
			1f,
			1.184f,
			1.09f
		};
		float[] fastShakeScales = new float[]
		{
			1f,
			1.184f,
			1.09f,
			1.34f,
			1.09f,
			1.184f
		};
		while (base.player.weaponManager.states.super == PlanePlayerWeaponManager.States.Super.Intro)
		{
			yield return null;
		}
		while (base.player.weaponManager.states.super == PlanePlayerWeaponManager.States.Super.Countdown)
		{
			if (t < 0.4f * WeaponProperties.PlaneSuperBomb.countdownTime)
			{
				yield return null;
				t += CupheadTime.Delta;
			}
			else if (t < 0.7f * WeaponProperties.PlaneSuperBomb.countdownTime)
			{
				foreach (float scale in slowShakeScales)
				{
					base.transform.SetScale(new float?(scale), new float?(scale), null);
					yield return CupheadTime.WaitForSeconds(this, 0.083333336f);
					t += 0.083333336f;
				}
			}
			else
			{
				foreach (float scale2 in fastShakeScales)
				{
					base.transform.SetScale(new float?(scale2), new float?(scale2), null);
					yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
					t += 0.041666668f;
				}
			}
		}
		base.animator.SetLayerWeight(2, 0f);
		base.transform.SetScale(new float?(1f), new float?(1f), null);
		yield break;
	}

	// Token: 0x060040CB RID: 16587 RVA: 0x002338A7 File Offset: 0x00231CA7
	public void SetSpriteVisible(bool visible)
	{
		this.playerSprite.gameObject.SetActive(visible);
	}

	// Token: 0x060040CC RID: 16588 RVA: 0x002338BC File Offset: 0x00231CBC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.breakoutPrefab = null;
		this.poofPrefab = null;
		this.greenPrefab = null;
		this.puffPrefab = null;
		this.hitSparkEffect = null;
		this.hitDustEffect = null;
		this.smokeDashEffect = null;
		this.shrinkEffect = null;
		this.growEffect = null;
	}

	// Token: 0x060040CD RID: 16589 RVA: 0x00233910 File Offset: 0x00231D10
	private void SetAlpha(float a)
	{
		Color color = this.spriteRenderer.color;
		color.a = a;
		this.spriteRenderer.color = color;
	}

	// Token: 0x060040CE RID: 16590 RVA: 0x0023393D File Offset: 0x00231D3D
	private void OnShrinkInComplete()
	{
		this.shrinkEffect.Create(base.player.center);
		this.Shrinking = false;
	}

	// Token: 0x060040CF RID: 16591 RVA: 0x0023395D File Offset: 0x00231D5D
	private void OnShrinkOutComplete()
	{
		this.growEffect.Create(base.player.center);
	}

	// Token: 0x060040D0 RID: 16592 RVA: 0x00233978 File Offset: 0x00231D78
	private void CreatePuff()
	{
		if (this.playerSprite == null)
		{
			return;
		}
		PlaneLevelEffect planeLevelEffect = this.puffPrefab.Create(this.playerSprite.position + PlanePlayerAnimationController.PUFF_OFFSET) as PlaneLevelEffect;
		if (base.player.motor.MoveDirection.x < 0)
		{
			planeLevelEffect.speed = 2f;
		}
	}

	// Token: 0x060040D1 RID: 16593 RVA: 0x002339F0 File Offset: 0x00231DF0
	private IEnumerator puff_cr()
	{
		float delay = 0.17f;
		for (;;)
		{
			delay = 0.17f;
			if ((base.player.motor.MoveDirection.x != 0 || base.player.motor.MoveDirection.y != 0) && base.player.motor.MoveDirection.x >= 0)
			{
				delay = 0.07f;
			}
			if (base.player.motor.MoveDirection.x >= 0)
			{
				this.CreatePuff();
			}
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		yield break;
	}

	// Token: 0x060040D2 RID: 16594 RVA: 0x00233A0C File Offset: 0x00231E0C
	private void InitializeCurseFX()
	{
		this.curseEffectAngle = (float)UnityEngine.Random.Range(0, 360);
		if (this.curseCharmLevel == 4)
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
			if (this.paladinShadows != null)
			{
				this.paladinShadows[0].transform.position = base.transform.position;
				this.paladinShadows[1].transform.position = base.transform.position;
				this.paladinShadows[0].sprite = this.spriteRenderer.sprite;
				this.paladinShadows[1].sprite = this.spriteRenderer.sprite;
				this.paladinShadows[0].transform.parent = null;
				this.paladinShadows[1].transform.parent = null;
			}
		}
	}

	// Token: 0x060040D3 RID: 16595 RVA: 0x00233B58 File Offset: 0x00231F58
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
			effect.transform.localScale = new Vector3(0.8f, 0.8f);
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

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x060040D4 RID: 16596 RVA: 0x00233EC5 File Offset: 0x002322C5
	private bool Flashing
	{
		get
		{
			return base.player.damageReceiver.state == PlayerDamageReceiver.State.Invulnerable;
		}
	}

	// Token: 0x060040D5 RID: 16597 RVA: 0x00233EDC File Offset: 0x002322DC
	private IEnumerator flash_cr()
	{
		float t = 0f;
		for (;;)
		{
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

	// Token: 0x060040D6 RID: 16598 RVA: 0x00233EF7 File Offset: 0x002322F7
	private void OnExStart()
	{
		base.StartCoroutine(this.ex_cr());
	}

	// Token: 0x060040D7 RID: 16599 RVA: 0x00233F08 File Offset: 0x00232308
	private IEnumerator ex_cr()
	{
		string dir = (base.player.motor.MoveDirection.y > 0) ? "Up" : "Down";
		base.animator.Play("Ex_" + dir);
		if (dir == "Up")
		{
			AudioManager.Play("player_plane_up_ex");
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Ex_" + dir, false, true);
		if (this.OnExFireAnimEvent != null)
		{
			this.OnExFireAnimEvent();
		}
		yield break;
	}

	// Token: 0x060040D8 RID: 16600 RVA: 0x00233F23 File Offset: 0x00232323
	private void OnSuperStart()
	{
		base.StartCoroutine(this.bomb_cr());
	}

	// Token: 0x060040D9 RID: 16601 RVA: 0x00233F34 File Offset: 0x00232334
	public void SetColor(Color color)
	{
		float a = this.spriteRenderer.color.a;
		color.a = a;
		this.spriteRenderer.color = color;
	}

	// Token: 0x060040DA RID: 16602 RVA: 0x00233F6C File Offset: 0x0023236C
	public void ResetColor()
	{
		float a = this.spriteRenderer.color.a;
		this.spriteRenderer.color = new Color(1f, 1f, 1f, a);
	}

	// Token: 0x060040DB RID: 16603 RVA: 0x00233FAD File Offset: 0x002323AD
	public void SetColorOverTime(Color color, float time)
	{
		this.StopColorCoroutine();
		this.colorCoroutine = this.setColor_cr(color, time);
		base.StartCoroutine(this.colorCoroutine);
	}

	// Token: 0x060040DC RID: 16604 RVA: 0x00233FD0 File Offset: 0x002323D0
	public void StopColorCoroutine()
	{
		if (this.colorCoroutine != null)
		{
			base.StopCoroutine(this.colorCoroutine);
		}
		this.colorCoroutine = null;
	}

	// Token: 0x060040DD RID: 16605 RVA: 0x00233FF0 File Offset: 0x002323F0
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

	// Token: 0x060040DE RID: 16606 RVA: 0x0023401C File Offset: 0x0023241C
	private void OnParryStart()
	{
		if (this.isStoned)
		{
			this.Breakout();
		}
		base.animator.SetBool("ParrySuccess", false);
		base.animator.SetBool("ParryPlusCharm", base.player.stats.Loadout.charm == Charm.charm_parry_plus);
		if (base.player.stats.Loadout.charm == Charm.charm_parry_attack || base.player.stats.CurseWhetsone)
		{
			base.animator.Play("ParryAttack");
		}
		else
		{
			base.animator.Play("Parry");
		}
	}

	// Token: 0x060040DF RID: 16607 RVA: 0x002340D0 File Offset: 0x002324D0
	private void OnParrySuccess()
	{
		base.animator.SetBool("ParrySuccess", true);
	}

	// Token: 0x060040E0 RID: 16608 RVA: 0x002340E4 File Offset: 0x002324E4
	private void OnDeath(PlayerId playerId)
	{
		foreach (PlanePlayerDeathPart planePlayerDeathPart in this.deathPieces)
		{
			planePlayerDeathPart.CreatePart(base.player.id, base.transform.position);
		}
		this.deathEffect.Create(base.transform.position);
	}

	// Token: 0x060040E1 RID: 16609 RVA: 0x00234144 File Offset: 0x00232544
	private void OnRevive(Vector3 pos)
	{
		this.SetAlpha(1f);
	}

	// Token: 0x060040E2 RID: 16610 RVA: 0x00234151 File Offset: 0x00232551
	private void SetInteger(string integer, int value)
	{
		base.animator.SetInteger(integer, value);
	}

	// Token: 0x060040E3 RID: 16611 RVA: 0x00234160 File Offset: 0x00232560
	private void SetTrigger(string trigger)
	{
		base.animator.SetTrigger(trigger);
	}

	// Token: 0x0400475B RID: 18267
	private const int PALADIN_SHADOW_BUFFER_SIZE = 10;

	// Token: 0x0400475C RID: 18268
	private const float ROTATION_MAX = 9f;

	// Token: 0x0400475D RID: 18269
	private const float SHUNK_ROTATION_ADD = 5f;

	// Token: 0x0400475E RID: 18270
	private const float ROTATION_SPEED = 7f;

	// Token: 0x0400475F RID: 18271
	private const float INTRO_X = -150f;

	// Token: 0x04004760 RID: 18272
	private const float PUFF_DELAY = 0.17f;

	// Token: 0x04004761 RID: 18273
	private const float PUFF_DELAY_MOVING = 0.07f;

	// Token: 0x04004762 RID: 18274
	private static readonly Vector2 PUFF_OFFSET = new Vector3(-50f, 0f);

	// Token: 0x04004763 RID: 18275
	private const float SHRINK_COOLDOWN = 0.23300001f;

	// Token: 0x04004764 RID: 18276
	[SerializeField]
	private Transform cuphead;

	// Token: 0x04004765 RID: 18277
	[SerializeField]
	private Transform mugman;

	// Token: 0x04004766 RID: 18278
	[SerializeField]
	private Transform chalice;

	// Token: 0x04004767 RID: 18279
	[Space(10f)]
	[SerializeField]
	private Transform introRoot;

	// Token: 0x04004768 RID: 18280
	[Space(10f)]
	[SerializeField]
	private Effect breakoutPrefab;

	// Token: 0x04004769 RID: 18281
	[SerializeField]
	private Effect poofPrefab;

	// Token: 0x0400476A RID: 18282
	[SerializeField]
	private Effect greenPrefab;

	// Token: 0x0400476B RID: 18283
	[SerializeField]
	private PlaneLevelEffect puffPrefab;

	// Token: 0x0400476C RID: 18284
	[Space(10f)]
	[SerializeField]
	private Effect hitSparkEffect;

	// Token: 0x0400476D RID: 18285
	[SerializeField]
	private Effect hitDustEffect;

	// Token: 0x0400476E RID: 18286
	[SerializeField]
	private Effect smokeDashEffect;

	// Token: 0x0400476F RID: 18287
	[SerializeField]
	private HealerCharmSparkEffect healerCharmEffect;

	// Token: 0x04004770 RID: 18288
	[SerializeField]
	private Effect curseEffect;

	// Token: 0x04004771 RID: 18289
	[Space(10f)]
	[SerializeField]
	private Effect shrinkEffect;

	// Token: 0x04004772 RID: 18290
	[SerializeField]
	private Effect growEffect;

	// Token: 0x04004773 RID: 18291
	[Space(10f)]
	[SerializeField]
	private PlanePlayerDeathPart[] deathPieces;

	// Token: 0x04004774 RID: 18292
	[SerializeField]
	private PlaneLevelEffect deathEffect;

	// Token: 0x04004775 RID: 18293
	private Transform playerSprite;

	// Token: 0x04004777 RID: 18295
	private float rotation;

	// Token: 0x04004778 RID: 18296
	private float shrinkCooldownTimeLeft;

	// Token: 0x04004779 RID: 18297
	private Material tempMaterial;

	// Token: 0x0400477C RID: 18300
	private bool isStoned;

	// Token: 0x0400477D RID: 18301
	[SerializeField]
	private float curseEffectDelay = 0.15f;

	// Token: 0x0400477E RID: 18302
	[SerializeField]
	private MinMax curseAngleShiftRange = new MinMax(60f, 300f);

	// Token: 0x0400477F RID: 18303
	[SerializeField]
	private MinMax curseDistanceRange = new MinMax(0f, 20f);

	// Token: 0x04004780 RID: 18304
	private float curseEffectAngle;

	// Token: 0x04004781 RID: 18305
	private float curseEffectTimer;

	// Token: 0x04004782 RID: 18306
	private int curseCharmLevel = -1;

	// Token: 0x04004783 RID: 18307
	private Vector3[] paladinShadowPosition;

	// Token: 0x04004784 RID: 18308
	private Vector3[] paladinShadowScale;

	// Token: 0x04004785 RID: 18309
	private Sprite[] paladinShadowSprite;

	// Token: 0x04004786 RID: 18310
	[SerializeField]
	private SpriteRenderer[] paladinShadows;

	// Token: 0x04004787 RID: 18311
	private bool showCurseFX;

	// Token: 0x0400478A RID: 18314
	private IEnumerator colorCoroutine;

	// Token: 0x02000A92 RID: 2706
	public enum ShrinkStates
	{
		// Token: 0x0400478C RID: 18316
		Ready,
		// Token: 0x0400478D RID: 18317
		Shrunk,
		// Token: 0x0400478E RID: 18318
		Cooldown
	}

	// Token: 0x02000A93 RID: 2707
	private enum AnimLayers
	{
		// Token: 0x04004790 RID: 18320
		Base,
		// Token: 0x04004791 RID: 18321
		Shrunk,
		// Token: 0x04004792 RID: 18322
		Bomb
	}
}
