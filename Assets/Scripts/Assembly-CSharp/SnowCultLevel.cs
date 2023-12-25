using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020002C2 RID: 706
public class SnowCultLevel : Level
{
	// Token: 0x060007BA RID: 1978 RVA: 0x00076208 File Offset: 0x00074608
	protected override void PartialInit()
	{
		this.properties = LevelProperties.SnowCult.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x060007BB RID: 1979 RVA: 0x0007629E File Offset: 0x0007469E
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.SnowCult;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x060007BC RID: 1980 RVA: 0x000762A5 File Offset: 0x000746A5
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_snow_cult;
		}
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060007BD RID: 1981 RVA: 0x000762AC File Offset: 0x000746AC
	// (remove) Token: 0x060007BE RID: 1982 RVA: 0x000762E4 File Offset: 0x000746E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnYetiHitGround;

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060007BF RID: 1983 RVA: 0x0007631C File Offset: 0x0007471C
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.SnowCult.States.Main:
				return this._bossPortraitMain;
			case LevelProperties.SnowCult.States.JackFrost:
				return this._bossPortraitPhaseThree;
			case LevelProperties.SnowCult.States.Yeti:
			case LevelProperties.SnowCult.States.EasyYeti:
				return this._bossPortraitPhaseTwo;
			}
			global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x000763A0 File Offset: 0x000747A0
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.SnowCult.States.Main:
				return this._bossQuoteMain;
			case LevelProperties.SnowCult.States.JackFrost:
				return this._bossQuotePhaseThree;
			case LevelProperties.SnowCult.States.Yeti:
			case LevelProperties.SnowCult.States.EasyYeti:
				return this._bossQuotePhaseTwo;
			}
			global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00076422 File Offset: 0x00074822
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0007643F File Offset: 0x0007483F
	protected override void Start()
	{
		base.Start();
		this.yeti.LevelInit(this.properties);
		this.jackFrost.LevelInit(this.properties);
		this.wizard.LevelInit(this.properties);
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x0007647A File Offset: 0x0007487A
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.snowcultPattern_cr());
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0007648C File Offset: 0x0007488C
	private IEnumerator snowcultPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x000764A8 File Offset: 0x000748A8
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.SnowCult.States.Yeti)
		{
			base.StartCoroutine(this.to_phase_2_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.SnowCult.States.JackFrost)
		{
			base.StartCoroutine(this.to_phase_3_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.SnowCult.States.EasyYeti)
		{
			base.StartCoroutine(this.to_phase_3_easy_cr());
		}
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x00076530 File Offset: 0x00074930
	private IEnumerator nextPattern_cr()
	{
		while (this.wizard != null && (this.wizard.Turning() || this.wizard.dead))
		{
			yield return null;
		}
		LevelProperties.SnowCult.Pattern p = this.properties.CurrentState.NextPattern;
		if (this.firstAttack)
		{
			while (p != LevelProperties.SnowCult.Pattern.Quad)
			{
				p = this.properties.CurrentState.NextPattern;
			}
			this.firstAttack = false;
		}
		switch (p)
		{
		case LevelProperties.SnowCult.Pattern.Switch:
			yield return base.StartCoroutine(this.switch_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Eye:
			yield return base.StartCoroutine(this.eye_attack_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Shard:
			yield return base.StartCoroutine(this.shard_attack_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Mouth:
			yield return base.StartCoroutine(this.mouth_shot_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Quad:
			yield return base.StartCoroutine(this.quad_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Block:
			yield return base.StartCoroutine(this.ice_block_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.SeriesShot:
			yield return base.StartCoroutine(this.series_shot_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Yeti:
			goto IL_2E6;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		IL_2E6:
		yield break;
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x0007654C File Offset: 0x0007494C
	private IEnumerator to_phase_2_cr()
	{
		this.firstAttack = false;
		this.wizard.ToOutro(this.yeti);
		yield return null;
		yield break;
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00076567 File Offset: 0x00074967
	public void CultistsSummon()
	{
		this.cultists.SetTrigger("Summon");
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00076579 File Offset: 0x00074979
	public void YetiHitGround()
	{
		if (this.OnYetiHitGround != null)
		{
			this.OnYetiHitGround();
		}
		this.cultists.SetTrigger("Summon");
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x000765A4 File Offset: 0x000749A4
	private IEnumerator to_phase_3_easy_cr()
	{
		this.yeti.ToEasyPhaseThree();
		yield return null;
		yield break;
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x000765C0 File Offset: 0x000749C0
	private IEnumerator to_phase_3_cr()
	{
		this.yeti.ForceOutroToStart();
		while (this.yeti.state != SnowCultLevelYeti.States.Idle || this.yeti.inBallForm)
		{
			yield return null;
		}
		this.cultists.SetTrigger("Summon");
		this.yeti.OnDeath();
		this.jackFrost.Intro();
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.yeti.timeToPlatforms);
		this.jackFrost.CreatePlatforms();
		base.StartCoroutine(this.SFX_SNOWCULT_IcePlatformAppear_cr());
		base.StartCoroutine(this.SFX_SNOWCULT_P2_to_P3_Transition_cr());
		for (int i = 0; i < 5; i++)
		{
			this.jackFrost.CreateAscendingPlatform(i);
			if (i < 4)
			{
				yield return CupheadTime.WaitForSeconds(this, 0.2f);
			}
		}
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		LevelPlayerMotor p1Motor = player.GetComponent<LevelPlayerMotor>();
		LevelPlayerMotor p2Motor = null;
		bool hasStarted = false;
		while (!hasStarted)
		{
			if (player2 != null && !player2.IsDead)
			{
				if (p2Motor == null)
				{
					p2Motor = player2.GetComponent<LevelPlayerMotor>();
				}
				if ((player.transform.position.y > -80f && p1Motor.Grounded) || (player2.transform.position.y > -80f && p2Motor.Grounded))
				{
					hasStarted = true;
				}
			}
			else if (player.transform.position.y > -80f && p1Motor.Grounded)
			{
				hasStarted = true;
			}
			yield return null;
		}
		Vector3 cameraEndPos = new Vector3(0f, 950f, 0f);
		float time = this.properties.CurrentState.yeti.timeForCameraMove;
		CupheadLevelCamera.Current.ChangeVerticalBounds(1290, 675);
		this.pit.SetActive(true);
		float cameraStartPos = CupheadLevelCamera.Current.transform.position.y;
		base.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraEndPos, time));
		time = 0f;
		while (time < 0.5f)
		{
			time = Mathf.InverseLerp(cameraStartPos, cameraEndPos.y, CupheadLevelCamera.Current.transform.position.y);
			yield return null;
		}
		Level.Current.SetBounds(new int?(640), new int?(640), new int?(1290), new int?(675));
		while (time < 0.75f)
		{
			time = Mathf.InverseLerp(cameraStartPos, cameraEndPos.y, CupheadLevelCamera.Current.transform.position.y);
			yield return null;
		}
		this.jackFrost.StartPhase3();
		this.pit.transform.parent = null;
		while (time < 0.95f)
		{
			time = Mathf.InverseLerp(cameraStartPos, cameraEndPos.y, CupheadLevelCamera.Current.transform.position.y);
			this.pit.transform.localPosition = CupheadLevelCamera.Current.transform.position + Vector3.down * 500f;
			yield return null;
		}
		this.pit.transform.localPosition = cameraEndPos + Vector3.down * 500f;
		yield break;
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x000765DC File Offset: 0x000749DC
	private IEnumerator quad_cr()
	{
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		this.wizard.StartQuadAttack();
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x000765F8 File Offset: 0x000749F8
	private IEnumerator ice_block_cr()
	{
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		this.wizard.Whale();
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00076614 File Offset: 0x00074A14
	private IEnumerator series_shot_cr()
	{
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		this.wizard.SeriesShot();
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00076630 File Offset: 0x00074A30
	private IEnumerator switch_cr()
	{
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		this.jackFrost.StartSwitch();
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0007664C File Offset: 0x00074A4C
	private IEnumerator eye_attack_cr()
	{
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		this.jackFrost.StartEyeAttack();
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00076668 File Offset: 0x00074A68
	private IEnumerator shard_attack_cr()
	{
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		this.jackFrost.StartShardAttack();
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00076684 File Offset: 0x00074A84
	private IEnumerator mouth_shot_cr()
	{
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		this.jackFrost.StartMouthShot();
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x000766A0 File Offset: 0x00074AA0
	private IEnumerator SFX_SNOWCULT_IcePlatformAppear_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		AudioManager.Play("sfx_dlc_snowcult_p2_iceplatform_appear");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_iceplatform_appear");
		yield break;
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x000766BC File Offset: 0x00074ABC
	private IEnumerator SFX_SNOWCULT_P2_to_P3_Transition_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		AudioManager.Play("sfx_dlc_snowcult_p2_snow_cultists_wave_hands_transition");
		yield break;
	}

	// Token: 0x0400100A RID: 4106
	private LevelProperties.SnowCult properties;

	// Token: 0x0400100B RID: 4107
	private const float CLIMBING_PLATFORMS_INTERAPPEAR_DELAY = 0.2f;

	// Token: 0x0400100C RID: 4108
	private const float HEIGHT_TO_START_PHASE_THREE = -80f;

	// Token: 0x0400100D RID: 4109
	private const float PHASE_THREE_CAMERA_POS = 950f;

	// Token: 0x0400100F RID: 4111
	[SerializeField]
	private SnowCultLevelWizard wizard;

	// Token: 0x04001010 RID: 4112
	[SerializeField]
	private SnowCultLevelYeti yeti;

	// Token: 0x04001011 RID: 4113
	[SerializeField]
	private SnowCultLevelJackFrost jackFrost;

	// Token: 0x04001012 RID: 4114
	[SerializeField]
	private Animator cultists;

	// Token: 0x04001013 RID: 4115
	[SerializeField]
	private GameObject pit;

	// Token: 0x04001014 RID: 4116
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04001015 RID: 4117
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;

	// Token: 0x04001016 RID: 4118
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;

	// Token: 0x04001017 RID: 4119
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04001018 RID: 4120
	[SerializeField]
	private string _bossQuotePhaseTwo;

	// Token: 0x04001019 RID: 4121
	[SerializeField]
	private string _bossQuotePhaseThree;

	// Token: 0x0400101A RID: 4122
	private bool firstAttack = true;
}
