using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009D5 RID: 2517
public class ArcadePlayerAnimationController : AbstractArcadePlayerComponent
{
	// Token: 0x06003B45 RID: 15173 RVA: 0x002140AD File Offset: 0x002124AD
	protected override void OnAwake()
	{
		base.OnAwake();
		this.SetSprites(base.player.id == PlayerId.PlayerOne);
	}

	// Token: 0x06003B46 RID: 15174 RVA: 0x002140CC File Offset: 0x002124CC
	private void Start()
	{
		base.basePlayer.OnPlayIntroEvent += this.PlayIntro;
		base.player.motor.OnParryEvent += this.OnParryStart;
		base.player.motor.OnGroundedEvent += this.OnGrounded;
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.player.weaponManager.OnExStart += this.OnEx;
		base.player.weaponManager.OnSuperStart += this.OnSuper;
		base.player.weaponManager.OnSuperEnd += this.OnSuperEnd;
		base.player.weaponManager.OnWeaponFire += this.OnShotFired;
		LevelPauseGUI.OnPauseEvent += this.OnGuiPause;
		LevelPauseGUI.OnPauseEvent += this.OnGuiUnpause;
	}

	// Token: 0x06003B47 RID: 15175 RVA: 0x002141D6 File Offset: 0x002125D6
	private void OnEnable()
	{
		base.StartCoroutine(this.flash_cr());
	}

	// Token: 0x06003B48 RID: 15176 RVA: 0x002141E8 File Offset: 0x002125E8
	private void Update()
	{
		if (base.player.IsDead || !base.player.levelStarted)
		{
			return;
		}
		if (!this.hitAnimation && base.player.motor.LookDirection.x != 0 && base.player.motor.LookDirection.x != this.GetInt(ArcadePlayerAnimationController.Integers.LookX))
		{
			this.SetBool(ArcadePlayerAnimationController.Booleans.Turning, true);
		}
		else
		{
			this.SetBool(ArcadePlayerAnimationController.Booleans.Turning, false);
		}
		this.SetBool(ArcadePlayerAnimationController.Booleans.Grounded, base.player.motor.Grounded);
		this.SetBool(ArcadePlayerAnimationController.Booleans.NearLanding, base.player.motor.GetTimeUntilLand() <= 0.15f && !base.player.motor.Parrying);
		this.SetInt(ArcadePlayerAnimationController.Integers.MoveX, base.player.motor.LookDirection.x);
		this.SetInt(ArcadePlayerAnimationController.Integers.MoveY, base.player.motor.MoveDirection.y);
		this.SetInt(ArcadePlayerAnimationController.Integers.LookX, base.player.motor.TrueLookDirection.x);
		this.SetInt(ArcadePlayerAnimationController.Integers.LookY, base.player.motor.TrueLookDirection.y);
		this.SetBool(ArcadePlayerAnimationController.Booleans.Shooting, base.player.weaponManager.IsShooting);
		AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(0);
		bool flag = currentAnimatorStateInfo.IsName("Idle") || currentAnimatorStateInfo.IsName("Run");
		if (this.shooting)
		{
			this.timeSinceStoppedShooting = 0f;
		}
		else
		{
			this.timeSinceStoppedShooting += CupheadTime.Delta;
		}
		bool flag2 = false;
		if (this.fired && flag)
		{
			this.SetTrigger(ArcadePlayerAnimationController.Triggers.OnFire);
			this.SetInt(ArcadePlayerAnimationController.Integers.ArmVariant, (!Rand.Bool()) ? 1 : 0);
			flag2 = true;
		}
		this.fired = false;
		this.shooting = base.player.weaponManager.IsShooting;
		if (!this.shooting && !flag2)
		{
			this.ResetTrigger(ArcadePlayerAnimationController.Triggers.OnFire);
		}
		this.SetBool(ArcadePlayerAnimationController.Booleans.Dashing, base.player.motor.Dashing);
		this.SetBool(ArcadePlayerAnimationController.Booleans.NearDashEnd, base.player.motor.GetTimeUntilDashEnd() < ((!base.player.motor.Grounded) ? 0.108333334f : 0.15f));
		if (!base.player.motor.Dashing)
		{
			if (base.player.motor.LookDirection.x != 0)
			{
				base.transform.SetScale(new float?(base.player.motor.LookDirection.x), null, null);
			}
		}
		else
		{
			base.transform.SetScale(new float?((float)base.player.motor.DashDirection), null, null);
		}
		base.animator.Update(Time.deltaTime);
		for (int i = 0; i < 3; i++)
		{
			base.animator.Update(0f);
		}
	}

	// Token: 0x06003B49 RID: 15177 RVA: 0x00214588 File Offset: 0x00212988
	public void ChangeToRocket()
	{
		this.prong.SetActive(false);
		string animation = "Rocket";
		this.Play(animation);
	}

	// Token: 0x06003B4A RID: 15178 RVA: 0x002145B0 File Offset: 0x002129B0
	public void ChangeToJetpack()
	{
		this.prong.SetActive(false);
		string animation = "Jetpack";
		this.Play(animation);
	}

	// Token: 0x06003B4B RID: 15179 RVA: 0x002145D6 File Offset: 0x002129D6
	public override void OnPause()
	{
		base.OnPause();
		this.SetAlpha(1f);
	}

	// Token: 0x06003B4C RID: 15180 RVA: 0x002145E9 File Offset: 0x002129E9
	private void OnGuiPause()
	{
	}

	// Token: 0x06003B4D RID: 15181 RVA: 0x002145EB File Offset: 0x002129EB
	private void OnGuiUnpause()
	{
	}

	// Token: 0x06003B4E RID: 15182 RVA: 0x002145ED File Offset: 0x002129ED
	public void OnShotFired()
	{
		this.fired = true;
	}

	// Token: 0x06003B4F RID: 15183 RVA: 0x002145F6 File Offset: 0x002129F6
	public void OnLevelWin()
	{
		base.player.damageReceiver.OnWin();
		this.SetTrigger(ArcadePlayerAnimationController.Triggers.OnWin);
	}

	// Token: 0x06003B50 RID: 15184 RVA: 0x00214610 File Offset: 0x00212A10
	public void PlayIntro()
	{
		this.SetBool(ArcadePlayerAnimationController.Booleans.Intro, true);
		string text = (base.player.id != PlayerId.PlayerOne) ? "Mugman" : "Cuphead";
		this.Play("Intro_" + text);
		if (text == "Cuphead")
		{
			AudioManager.Play("player_intro_cuphead");
		}
		else
		{
			AudioManager.Play("player_intro_mugman");
		}
	}

	// Token: 0x06003B51 RID: 15185 RVA: 0x0021467F File Offset: 0x00212A7F
	public void LevelInit()
	{
		this.SetSprites(base.player.id == PlayerId.PlayerOne);
	}

	// Token: 0x06003B52 RID: 15186 RVA: 0x00214698 File Offset: 0x00212A98
	public void SetSprites(bool isCuphead)
	{
		this.cuphead.SetActive(isCuphead);
		this.mugman.SetActive(!isCuphead);
		if (isCuphead)
		{
			this.spriteRenderer = this.cuphead.GetComponent<SpriteRenderer>();
			this.armRenderer = this.cupheadArm.GetComponent<SpriteRenderer>();
		}
		else
		{
			this.spriteRenderer = this.mugman.GetComponent<SpriteRenderer>();
			this.armRenderer = this.mugmanArm.GetComponent<SpriteRenderer>();
		}
	}

	// Token: 0x06003B53 RID: 15187 RVA: 0x00214710 File Offset: 0x00212B10
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		CupheadLevelCamera.Current.Shake(20f, 0.6f, false);
		AudioManager.Play("player_hit");
		if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Normal)
		{
			this.Play("Hit");
			this.hitAnimation = true;
		}
	}

	// Token: 0x06003B54 RID: 15188 RVA: 0x0021475E File Offset: 0x00212B5E
	private void OnRunDust()
	{
		this.runEffect.Create(this.runDustRoot.position);
	}

	// Token: 0x06003B55 RID: 15189 RVA: 0x00214777 File Offset: 0x00212B77
	private void onHitAnimationComplete()
	{
		this.hitAnimation = false;
	}

	// Token: 0x06003B56 RID: 15190 RVA: 0x00214780 File Offset: 0x00212B80
	private void SetSpriteProperties(SpriteLayer layer, int order)
	{
		this.spriteRenderer.sortingLayerName = layer.ToString();
		this.spriteRenderer.sortingOrder = order;
	}

	// Token: 0x06003B57 RID: 15191 RVA: 0x002147A8 File Offset: 0x00212BA8
	private void ResetSpriteProperties()
	{
		this.spriteRenderer.sortingLayerName = SpriteLayer.Player.ToString();
		this.spriteRenderer.sortingOrder = ((base.player.id != PlayerId.PlayerOne) ? -1 : 1);
	}

	// Token: 0x06003B58 RID: 15192 RVA: 0x002147F1 File Offset: 0x00212BF1
	private void OnParryStart()
	{
		if (this.super)
		{
			return;
		}
		this.SetTrigger(ArcadePlayerAnimationController.Triggers.OnParry);
	}

	// Token: 0x06003B59 RID: 15193 RVA: 0x00214806 File Offset: 0x00212C06
	public void OnParrySuccess()
	{
		this.SetAlpha(1f);
	}

	// Token: 0x06003B5A RID: 15194 RVA: 0x00214813 File Offset: 0x00212C13
	public void OnParryPause()
	{
	}

	// Token: 0x06003B5B RID: 15195 RVA: 0x00214815 File Offset: 0x00212C15
	public void OnParryAnimEnd()
	{
	}

	// Token: 0x06003B5C RID: 15196 RVA: 0x00214817 File Offset: 0x00212C17
	public void ResumeNormanAnim()
	{
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x00214819 File Offset: 0x00212C19
	private void OnGrounded()
	{
		if (!Level.Current.Started)
		{
			return;
		}
		AudioManager.Play("player_grounded");
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x00214838 File Offset: 0x00212C38
	private void OnEx()
	{
		string text = "Forward";
		if (base.player.motor.LookDirection.x == 0 && base.player.motor.LookDirection.y > 0)
		{
			text = "Up";
		}
		else if (base.player.motor.LookDirection.x != 0 && base.player.motor.LookDirection.y > 0)
		{
			text = "Diagonal_Up";
		}
		else if (base.player.motor.LookDirection.x == 0 && base.player.motor.LookDirection.y < 0)
		{
			text = "Down";
		}
		else if (base.player.motor.LookDirection.x != 0 && base.player.motor.LookDirection.y < 0)
		{
			text = "Diagonal_Down";
		}
		if (text == "Forward")
		{
			AudioManager.Play("player_ex_forward_ground");
		}
		string text2 = "Ex." + text + "_";
		if (base.player.motor.Grounded)
		{
			text2 += "Ground";
		}
		else
		{
			text2 += "Air";
		}
		this.Play(text2);
	}

	// Token: 0x06003B5F RID: 15199 RVA: 0x002149F8 File Offset: 0x00212DF8
	private void OnSuper()
	{
		Super super = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id).super;
		this.super = true;
		this.spriteRenderer.enabled = false;
	}

	// Token: 0x06003B60 RID: 15200 RVA: 0x00214A38 File Offset: 0x00212E38
	private void OnSuperEnd()
	{
		this.super = false;
		this.spriteRenderer.enabled = true;
		this.ResetSpriteProperties();
	}

	// Token: 0x06003B61 RID: 15201 RVA: 0x00214A53 File Offset: 0x00212E53
	private void _OnSuperAnimEnd()
	{
		base.player.UnpauseAll(false);
		base.player.motor.OnSuperEnd();
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x00214A71 File Offset: 0x00212E71
	protected void Play(string animation)
	{
		base.animator.Play(animation, 0, 0f);
	}

	// Token: 0x06003B63 RID: 15203 RVA: 0x00214A85 File Offset: 0x00212E85
	protected bool GetBool(ArcadePlayerAnimationController.Booleans b)
	{
		return base.animator.GetBool(b.ToString());
	}

	// Token: 0x06003B64 RID: 15204 RVA: 0x00214A9F File Offset: 0x00212E9F
	protected void SetBool(ArcadePlayerAnimationController.Booleans b, bool value)
	{
		base.animator.SetBool(b.ToString(), value);
	}

	// Token: 0x06003B65 RID: 15205 RVA: 0x00214ABA File Offset: 0x00212EBA
	protected int GetInt(ArcadePlayerAnimationController.Integers i)
	{
		return base.animator.GetInteger(i.ToString());
	}

	// Token: 0x06003B66 RID: 15206 RVA: 0x00214AD4 File Offset: 0x00212ED4
	protected void SetInt(ArcadePlayerAnimationController.Integers i, int value)
	{
		base.animator.SetInteger(i.ToString(), value);
	}

	// Token: 0x06003B67 RID: 15207 RVA: 0x00214AEF File Offset: 0x00212EEF
	protected void SetTrigger(ArcadePlayerAnimationController.Triggers t)
	{
		base.animator.SetTrigger(t.ToString());
	}

	// Token: 0x06003B68 RID: 15208 RVA: 0x00214B09 File Offset: 0x00212F09
	protected void ResetTrigger(ArcadePlayerAnimationController.Triggers t)
	{
		base.animator.ResetTrigger(t.ToString());
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x00214B24 File Offset: 0x00212F24
	private void SetAlpha(float a)
	{
		Color color = this.spriteRenderer.color;
		color.a = a;
		this.spriteRenderer.color = color;
		this.armRenderer.color = color;
	}

	// Token: 0x06003B6A RID: 15210 RVA: 0x00214B60 File Offset: 0x00212F60
	public void SetColor(Color color)
	{
		float a = this.spriteRenderer.color.a;
		color.a = a;
		this.spriteRenderer.color = color;
	}

	// Token: 0x06003B6B RID: 15211 RVA: 0x00214B98 File Offset: 0x00212F98
	public void ResetColor()
	{
		float a = this.spriteRenderer.color.a;
		this.spriteRenderer.color = new Color(1f, 1f, 1f, a);
	}

	// Token: 0x06003B6C RID: 15212 RVA: 0x00214BD9 File Offset: 0x00212FD9
	public void SetColorOverTime(Color color, float time)
	{
		this.StopColorCoroutine();
		this.colorCoroutine = this.setColor_cr(color, time);
		base.StartCoroutine(this.colorCoroutine);
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x00214BFC File Offset: 0x00212FFC
	public void StopColorCoroutine()
	{
		if (this.colorCoroutine != null)
		{
			base.StopCoroutine(this.colorCoroutine);
		}
		this.colorCoroutine = null;
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x00214C1C File Offset: 0x0021301C
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

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06003B6F RID: 15215 RVA: 0x00214C45 File Offset: 0x00213045
	private bool Flashing
	{
		get
		{
			return base.player.damageReceiver.state == PlayerDamageReceiver.State.Invulnerable;
		}
	}

	// Token: 0x06003B70 RID: 15216 RVA: 0x00214C5C File Offset: 0x0021305C
	private IEnumerator flash_cr()
	{
		float t = 0f;
		for (;;)
		{
			while (!this.Flashing)
			{
				yield return true;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.5f);
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
		}
		yield break;
	}

	// Token: 0x040042D6 RID: 17110
	[SerializeField]
	private GameObject prong;

	// Token: 0x040042D7 RID: 17111
	[SerializeField]
	private GameObject cuphead;

	// Token: 0x040042D8 RID: 17112
	[SerializeField]
	private GameObject mugman;

	// Token: 0x040042D9 RID: 17113
	[SerializeField]
	private GameObject cupheadArm;

	// Token: 0x040042DA RID: 17114
	[SerializeField]
	private GameObject mugmanArm;

	// Token: 0x040042DB RID: 17115
	[Space(10f)]
	[SerializeField]
	private Transform runDustRoot;

	// Token: 0x040042DC RID: 17116
	[Space(10f)]
	[SerializeField]
	private Effect dashEffect;

	// Token: 0x040042DD RID: 17117
	[SerializeField]
	private Effect groundedEffect;

	// Token: 0x040042DE RID: 17118
	[SerializeField]
	private Effect hitEffect;

	// Token: 0x040042DF RID: 17119
	[SerializeField]
	private Effect runEffect;

	// Token: 0x040042E0 RID: 17120
	private SpriteRenderer spriteRenderer;

	// Token: 0x040042E1 RID: 17121
	private SpriteRenderer armRenderer;

	// Token: 0x040042E2 RID: 17122
	private bool hitAnimation;

	// Token: 0x040042E3 RID: 17123
	private bool super;

	// Token: 0x040042E4 RID: 17124
	private bool shooting;

	// Token: 0x040042E5 RID: 17125
	private bool fired;

	// Token: 0x040042E6 RID: 17126
	private float timeSinceStoppedShooting = 100f;

	// Token: 0x040042E7 RID: 17127
	private const float STOP_SHOOTING_DELAY = 0.0833f;

	// Token: 0x040042E8 RID: 17128
	private const float JUMP_END_ANIMATION_TIME = 0.15f;

	// Token: 0x040042E9 RID: 17129
	private const float DASH_END_ANIMATION_TIME = 0.15f;

	// Token: 0x040042EA RID: 17130
	private const float DASH_END_AIR_ANIMATION_TIME = 0.108333334f;

	// Token: 0x040042EB RID: 17131
	private IEnumerator colorCoroutine;

	// Token: 0x020009D6 RID: 2518
	public enum Booleans
	{
		// Token: 0x040042ED RID: 17133
		Dashing,
		// Token: 0x040042EE RID: 17134
		Shooting,
		// Token: 0x040042EF RID: 17135
		Grounded,
		// Token: 0x040042F0 RID: 17136
		Turning,
		// Token: 0x040042F1 RID: 17137
		Intro,
		// Token: 0x040042F2 RID: 17138
		Dead,
		// Token: 0x040042F3 RID: 17139
		NearLanding,
		// Token: 0x040042F4 RID: 17140
		NearDashEnd
	}

	// Token: 0x020009D7 RID: 2519
	public enum Integers
	{
		// Token: 0x040042F6 RID: 17142
		MoveX,
		// Token: 0x040042F7 RID: 17143
		MoveY,
		// Token: 0x040042F8 RID: 17144
		LookX,
		// Token: 0x040042F9 RID: 17145
		LookY,
		// Token: 0x040042FA RID: 17146
		ArmVariant
	}

	// Token: 0x020009D8 RID: 2520
	public enum Triggers
	{
		// Token: 0x040042FC RID: 17148
		OnJump,
		// Token: 0x040042FD RID: 17149
		OnGround,
		// Token: 0x040042FE RID: 17150
		OnParry,
		// Token: 0x040042FF RID: 17151
		OnWin,
		// Token: 0x04004300 RID: 17152
		OnTurn,
		// Token: 0x04004301 RID: 17153
		OnFire
	}
}
