using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class DevilLevel : Level
{
	// Token: 0x0600028F RID: 655 RVA: 0x0005C73C File Offset: 0x0005AB3C
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Devil.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000290 RID: 656 RVA: 0x0005C7D2 File Offset: 0x0005ABD2
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Devil;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000291 RID: 657 RVA: 0x0005C7D9 File Offset: 0x0005ABD9
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_devil;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000292 RID: 658 RVA: 0x0005C7E0 File Offset: 0x0005ABE0
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Devil.States.Main:
			case LevelProperties.Devil.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Devil.States.GiantHead:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.Devil.States.Hands:
			case LevelProperties.Devil.States.Tears:
				return this._bossPortraitPhaseThree;
			}
			global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000293 RID: 659 RVA: 0x0005C868 File Offset: 0x0005AC68
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Devil.States.Main:
			case LevelProperties.Devil.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Devil.States.GiantHead:
				return this._bossQuotePhaseTwo;
			case LevelProperties.Devil.States.Hands:
			case LevelProperties.Devil.States.Tears:
				return this._bossQuotePhaseThree;
			}
			global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x06000294 RID: 660 RVA: 0x0005C8EE File Offset: 0x0005ACEE
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0005C8F6 File Offset: 0x0005ACF6
	protected override void Start()
	{
		base.Start();
		this.isDevil = true;
		this.sittingDevil.LevelInit(this.properties);
		this.giantHead.LevelInit(this.properties);
		base.StartCoroutine(this.DelayedStart());
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0005C934 File Offset: 0x0005AD34
	private IEnumerator DelayedStart()
	{
		yield return null;
		this.phase2Background.SetActive(false);
		yield break;
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0005C94F File Offset: 0x0005AD4F
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.devilPattern_cr());
		this.sittingDevil.StartDemons();
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0005C96C File Offset: 0x0005AD6C
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Devil.States.Split)
		{
			this.StopAllCoroutines();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Devil.States.GiantHead)
		{
			this.StopAllCoroutines();
			this.sittingDevil.StartTransform();
			base.StartCoroutine(this.phase_1_end_trans());
			base.StartCoroutine(this.devilPattern_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Devil.States.Hands)
		{
			this.StopAllCoroutines();
			this.giantHead.StartHands();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Devil.States.Tears)
		{
			this.StopAllCoroutines();
			this.giantHead.StartTears();
		}
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0005CA39 File Offset: 0x0005AE39
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseThree = null;
		this._bossPortraitPhaseTwo = null;
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0005CA58 File Offset: 0x0005AE58
	private IEnumerator phase_1_end_trans()
	{
		foreach (DevilLevelEffectSpawner devilLevelEffectSpawner in this.smokeSpawners)
		{
			devilLevelEffectSpawner.KillSmoke();
		}
		while (!DevilLevelHole.PHASE_1_COMPLETE)
		{
			yield return null;
		}
		this.groundHandler.SetActive(false);
		bool startZoomout = false;
		float t = 0f;
		float cameraSlideUpTime = 1f;
		float time = 3.3f;
		float endCameraTime = 2f;
		Vector3 phase1Start = this.phase1Scroll.transform.position;
		Vector3 phase1End = Vector3.zero;
		Vector3 cameraStart = CupheadLevelCamera.Current.transform.position;
		Vector3 cameraEffectEnd = new Vector3(CupheadLevelCamera.Current.transform.position.x, 50f);
		Vector3 cameraOffsetEnd = new Vector3(CupheadLevelCamera.Current.transform.position.x, 600f);
		foreach (ParallaxLayer parallaxLayer in this.parallax)
		{
			parallaxLayer.enabled = false;
		}
		this.sittingDevil.RemoveFire();
		yield return base.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraEffectEnd, cameraSlideUpTime));
		base.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraOffsetEnd, time));
		while (t < time)
		{
			if (t >= 2f && !startZoomout)
			{
				this.ZoomOut(cameraStart, endCameraTime);
				startZoomout = true;
			}
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			this.phase1Scroll.transform.position = Vector3.Lerp(phase1Start, phase1End, val);
			Color c = this.phase1Foreground.color;
			c.a = Mathf.Clamp(1f - t * 2f, 0f, 1f);
			this.phase1Foreground.color = c;
			yield return null;
		}
		this.phase1Scroll.transform.position = phase1End;
		this.giantHead.transform.parent = null;
		this.giantHead.StartIntroTransform();
		base.StartCoroutine(this.phase2BackgroundFade_cr(2f));
		AudioManager.FadeBGMVolume(0f, 0.5f, true);
		AudioManager.PlayBGMPlaylistManually(false);
		AudioManager.Play("transition_sting");
		yield return CupheadTime.WaitForSeconds(this, endCameraTime);
		this.phase1Scroll.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(this.sittingDevil.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0005CA74 File Offset: 0x0005AE74
	private IEnumerator phase2BackgroundFade_cr(float time)
	{
		SpriteRenderer[] sprites = this.phase2Background.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < sprites.Length; i++)
		{
			Color color = sprites[i].color;
			color.a = 0f;
			sprites[i].color = color;
		}
		this.phase2Background.SetActive(true);
		float t = 0f;
		while (t < time)
		{
			for (int j = 0; j < sprites.Length; j++)
			{
				Color color2 = sprites[j].color;
				color2.a = t / time;
				sprites[j].color = color2;
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		for (int k = 0; k < sprites.Length; k++)
		{
			Color color3 = sprites[k].color;
			color3.a = 1f;
			sprites[k].color = color3;
		}
		yield break;
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0005CA98 File Offset: 0x0005AE98
	private void ZoomOut(Vector3 cameraStart, float endCameraTime)
	{
		AudioManager.FadeBGMVolume(0f, 1f, true);
		Level.Current.SetBounds(new int?(932), new int?(932), new int?(460), new int?(306));
		base.StartCoroutine(CupheadLevelCamera.Current.change_zoom_cr(0.811f, 10f));
		base.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraStart, endCameraTime));
		this.phase3Platforms.SetActive(true);
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		player.transform.SetScale(new float?(1f), null, null);
		player.transform.position = this.Phase2P1spawn.position;
		player.GetComponent<LevelPlayerMotor>().ForceLooking(new Trilean2(1, 0));
		if (player.stats.isChalice)
		{
			player.GetComponent<LevelPlayerMotor>().DashComplete();
			player.GetComponent<LevelPlayerAnimationController>().ScaredChalice(false);
		}
		else
		{
			player.GetComponent<LevelPlayerAnimationController>().PlayIntro();
		}
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null)
		{
			player2.transform.SetScale(new float?(1f), null, null);
			player2.transform.position = this.Phase2P2spawn.position;
			player2.GetComponent<LevelPlayerMotor>().ForceLooking(new Trilean2(1, 0));
			if (player2.stats.isChalice)
			{
				player2.GetComponent<LevelPlayerMotor>().DashComplete();
				player2.GetComponent<LevelPlayerAnimationController>().ScaredChalice(false);
			}
			else
			{
				player2.GetComponent<LevelPlayerAnimationController>().PlayIntro();
			}
		}
		base.StartCoroutine(this.disable_input_cr());
		this.pit.SetActive(true);
	}

	// Token: 0x0600029D RID: 669 RVA: 0x0005CC60 File Offset: 0x0005B060
	private IEnumerator disable_input_cr()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		LevelPlayerMotor motorP = player.GetComponent<LevelPlayerMotor>();
		LevelPlayerWeaponManager weaponManagerP = player.GetComponent<LevelPlayerWeaponManager>();
		motorP.DisableInput();
		if (player.stats.isChalice)
		{
			motorP.ForceLooking(new Trilean2(0, 0));
		}
		weaponManagerP.DisableInput();
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null)
		{
			player2.GetComponent<LevelPlayerMotor>().DisableInput();
			if (player2.stats.isChalice)
			{
				player2.GetComponent<LevelPlayerMotor>().ForceLooking(new Trilean2(0, 0));
			}
			player2.GetComponent<LevelPlayerWeaponManager>().DisableInput();
		}
		if (!player.GetComponent<LevelPlayerController>().IsDead)
		{
			yield return motorP.animator.WaitForAnimationToEnd(this, (!player.stats.isChalice) ? "Intro_Scared" : "Intro_Chalice_Scared", (!player.stats.isChalice) ? 0 : 3, false, true);
		}
		else if (player2 != null && !player2.GetComponent<LevelPlayerController>().IsDead)
		{
			yield return player2.GetComponent<LevelPlayerMotor>().animator.WaitForAnimationToEnd(this, (!player2.stats.isChalice) ? "Intro_Scared" : "Intro_Chalice_Scared", (!player2.stats.isChalice) ? 0 : 3, false, true);
		}
		motorP.ClearBufferedInput();
		motorP.EnableInput();
		weaponManagerP.EnableInput();
		if (player.stats.isChalice)
		{
			motorP.ForceLooking(new Trilean2(1, 0));
		}
		player.GetComponent<LevelPlayerAnimationController>().ResetMoveX();
		player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null)
		{
			player2.GetComponent<LevelPlayerMotor>().ClearBufferedInput();
			player2.GetComponent<LevelPlayerMotor>().EnableInput();
			player2.GetComponent<LevelPlayerWeaponManager>().EnableInput();
			player2.GetComponent<LevelPlayerAnimationController>().ResetMoveX();
			if (player2.stats.isChalice)
			{
				player2.GetComponent<LevelPlayerMotor>().ForceLooking(new Trilean2(1, 0));
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600029E RID: 670 RVA: 0x0005CC7C File Offset: 0x0005B07C
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(this.Phase2P1spawn.position, 30f);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(this.Phase2P2spawn.position, 30f);
	}

	// Token: 0x0600029F RID: 671 RVA: 0x0005CCD0 File Offset: 0x0005B0D0
	private IEnumerator devilPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0005CCEC File Offset: 0x0005B0EC
	private IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.Devil.Pattern.Clap:
			yield return base.StartCoroutine(this.clap_cr());
			break;
		case LevelProperties.Devil.Pattern.Head:
			yield return base.StartCoroutine(this.head_cr());
			break;
		case LevelProperties.Devil.Pattern.Pitchfork:
			yield return base.StartCoroutine(this.pitchfork_cr());
			break;
		case LevelProperties.Devil.Pattern.BombEye:
			yield return base.StartCoroutine(this.bombEye_cr());
			break;
		case LevelProperties.Devil.Pattern.SkullEye:
			yield return base.StartCoroutine(this.skullEye_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0005CD08 File Offset: 0x0005B108
	private IEnumerator clap_cr()
	{
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		this.sittingDevil.StartClap();
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0005CD24 File Offset: 0x0005B124
	private IEnumerator head_cr()
	{
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		this.sittingDevil.StartHead();
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x0005CD40 File Offset: 0x0005B140
	private IEnumerator pitchfork_cr()
	{
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		this.sittingDevil.StartPitchfork();
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x0005CD5C File Offset: 0x0005B15C
	private IEnumerator bombEye_cr()
	{
		while (this.giantHead.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		this.giantHead.StartBombEye();
		while (this.giantHead.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0005CD78 File Offset: 0x0005B178
	private IEnumerator skullEye_cr()
	{
		while (this.giantHead.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		this.giantHead.StartSkullEye();
		while (this.giantHead.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040004ED RID: 1261
	private LevelProperties.Devil properties;

	// Token: 0x040004EE RID: 1262
	private const float Phase2FadeInTime = 2f;

	// Token: 0x040004EF RID: 1263
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x040004F0 RID: 1264
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;

	// Token: 0x040004F1 RID: 1265
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;

	// Token: 0x040004F2 RID: 1266
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x040004F3 RID: 1267
	[SerializeField]
	private string _bossQuotePhaseTwo;

	// Token: 0x040004F4 RID: 1268
	[SerializeField]
	private string _bossQuotePhaseThree;

	// Token: 0x040004F5 RID: 1269
	[SerializeField]
	private GameObject groundHandler;

	// Token: 0x040004F6 RID: 1270
	[SerializeField]
	private ParallaxLayer[] parallax;

	// Token: 0x040004F7 RID: 1271
	[SerializeField]
	private GameObject pit;

	// Token: 0x040004F8 RID: 1272
	[SerializeField]
	private GameObject middlePiece;

	// Token: 0x040004F9 RID: 1273
	[SerializeField]
	private Transform phase1Scroll;

	// Token: 0x040004FA RID: 1274
	[SerializeField]
	private SpriteRenderer phase1Foreground;

	// Token: 0x040004FB RID: 1275
	[SerializeField]
	private GameObject phase2Background;

	// Token: 0x040004FC RID: 1276
	[SerializeField]
	private GameObject phase3Platforms;

	// Token: 0x040004FD RID: 1277
	[SerializeField]
	private SpriteRenderer phase1Fade;

	// Token: 0x040004FE RID: 1278
	[SerializeField]
	private DevilLevelSittingDevil sittingDevil;

	// Token: 0x040004FF RID: 1279
	[SerializeField]
	private DevilLevelGiantHead giantHead;

	// Token: 0x04000500 RID: 1280
	[SerializeField]
	private DevilLevelEffectSpawner[] smokeSpawners;

	// Token: 0x04000501 RID: 1281
	[SerializeField]
	private Transform Phase2P1spawn;

	// Token: 0x04000502 RID: 1282
	[SerializeField]
	private Transform Phase2P2spawn;
}
