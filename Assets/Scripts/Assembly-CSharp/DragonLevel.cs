using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class DragonLevel : Level
{
	// Token: 0x0600042D RID: 1069 RVA: 0x00061DA8 File Offset: 0x000601A8
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Dragon.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x0600042E RID: 1070 RVA: 0x00061E3E File Offset: 0x0006023E
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Dragon;
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x0600042F RID: 1071 RVA: 0x00061E45 File Offset: 0x00060245
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dragon;
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000430 RID: 1072 RVA: 0x00061E49 File Offset: 0x00060249
	// (set) Token: 0x06000431 RID: 1073 RVA: 0x00061E50 File Offset: 0x00060250
	public static float SPEED { get; private set; }

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000432 RID: 1074 RVA: 0x00061E58 File Offset: 0x00060258
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Dragon.States.Main:
			case LevelProperties.Dragon.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Dragon.States.ThreeHeads:
				return this._bossPortraitThreeHeads;
			case LevelProperties.Dragon.States.FireMarchers:
				return this._bossPortraitFireMarchers;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000433 RID: 1075 RVA: 0x00061ED8 File Offset: 0x000602D8
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Dragon.States.Main:
			case LevelProperties.Dragon.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Dragon.States.ThreeHeads:
				return this._bossQuoteThreeHeads;
			case LevelProperties.Dragon.States.FireMarchers:
				return this._bossQuoteFireMarchers;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x00061F56 File Offset: 0x00060356
	protected override void Awake()
	{
		base.Awake();
		DragonLevel.SPEED = 0f;
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x00061F68 File Offset: 0x00060368
	protected override void Start()
	{
		base.Start();
		this.dragon.LevelInit(this.properties);
		this.tail.LevelInit(this.properties);
		this.leftSideDragon.LevelInit(this.properties);
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x00061FA4 File Offset: 0x000603A4
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.speed_cr());
		base.StartCoroutine(this.tail_cr());
		base.StartCoroutine(this.dragonPattern_cr());
		this.manager.Init(this.properties.CurrentState.clouds);
		this.SetPlatformVariables(true);
		this.cloudsSameDir = this.properties.CurrentState.clouds.movingRight;
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00062018 File Offset: 0x00060418
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		this.manager.UpdateProperties(this.properties.CurrentState.clouds);
		this.SetPlatformVariables(false);
		if (this.properties.CurrentState.clouds.movingRight != this.cloudsSameDir)
		{
			base.StartCoroutine(this.speed_cr());
			this.cloudsSameDir = this.properties.CurrentState.clouds.movingRight;
		}
		if (this.properties.CurrentState.stateName == LevelProperties.Dragon.States.FireMarchers)
		{
			this.StopAllCoroutines();
			this.dragon.Leave();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Dragon.States.ThreeHeads)
		{
			this.StopAllCoroutines();
			this.leftSideDragon.StartThreeHeads();
			base.StartCoroutine(this.phase3ColorTransition());
		}
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x000620F8 File Offset: 0x000604F8
	private void SetPlatformVariables(bool firstTime)
	{
		foreach (DragonLevelCloudPlatform dragonLevelCloudPlatform in this.platforms)
		{
			dragonLevelCloudPlatform.GetProperties(this.properties.CurrentState.clouds, firstTime);
		}
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0006213B File Offset: 0x0006053B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitThreeHeads = null;
		this._bossPortraitFireMarchers = null;
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x00062158 File Offset: 0x00060558
	private IEnumerator dragonPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00062174 File Offset: 0x00060574
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Dragon.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Dragon.Pattern.Meteor)
		{
			if (p != LevelProperties.Dragon.Pattern.Peashot)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.peashot_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.meteor_cr());
		}
		yield break;
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00062190 File Offset: 0x00060590
	private IEnumerator meteor_cr()
	{
		while (this.dragon.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		this.dragon.StartMeteor();
		while (this.dragon.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x000621AC File Offset: 0x000605AC
	private IEnumerator peashot_cr()
	{
		while (this.dragon.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		this.dragon.StartPeashot();
		while (this.dragon.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x000621C8 File Offset: 0x000605C8
	private IEnumerator speed_cr()
	{
		float t = 0f;
		while (t < 3f)
		{
			DragonLevel.SPEED = t / 3f;
			t += CupheadTime.Delta;
			yield return null;
		}
		DragonLevel.SPEED = 1f;
		yield break;
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x000621DC File Offset: 0x000605DC
	private IEnumerator tail_cr()
	{
		while (this.dragon.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		for (;;)
		{
			while (!this.properties.CurrentState.tail.active)
			{
				yield return null;
			}
			LevelProperties.Dragon.Tail tailProperties = this.properties.CurrentState.tail;
			this.tail.TailStart(tailProperties.warningTime, tailProperties.inTime, tailProperties.holdTime, tailProperties.outTime);
			while (this.tail.state != DragonLevelTail.State.Idle)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, tailProperties.attackDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x000621F8 File Offset: 0x000605F8
	private IEnumerator phase3ColorTransition()
	{
		while (this.leftSideDragon.state != DragonLevelLeftSideDragon.State.ThreeHeads)
		{
			yield return null;
		}
		base.StartCoroutine(this.lightning_cr());
		float t = 0f;
		float fadeTime = 6f;
		DragonLevel.LightningState lastLightningState = this.lightningState;
		HitFlash dragonHitFlash = this.leftSideDragon.GetComponentInChildren<HitFlash>();
		this.dragonMaterial = this.leftSideDragon.GetComponent<SpriteRenderer>().material;
		for (;;)
		{
			LevelPlayerController playerOne = PlayerManager.GetPlayer<LevelPlayerController>(PlayerId.PlayerOne);
			LevelPlayerController playerTwo = PlayerManager.GetPlayer<LevelPlayerController>(PlayerId.PlayerTwo);
			float ratio = Mathf.Min(1f, t / fadeTime);
			Color playerColor;
			Color projectileColor;
			Color dragonColor;
			Color darkSpireColor;
			Color platformColor;
			if (this.lightningState == DragonLevel.LightningState.FirstFlash)
			{
				playerColor = ColorUtils.HexToColor("333333");
				projectileColor = ColorUtils.HexToColor("333333");
				dragonColor = ColorUtils.HexToColor("333333");
				darkSpireColor = new Color(0.2f, 0.2f, 0.2f, this.darkSpire.color.a);
				platformColor = ColorUtils.HexToColor("191919");
			}
			else if (this.lightningState == DragonLevel.LightningState.SecondFlash)
			{
				playerColor = ColorUtils.HexToColor("191919");
				projectileColor = ColorUtils.HexToColor("191919");
				dragonColor = ColorUtils.HexToColor("191919");
				darkSpireColor = new Color(0.1f, 0.1f, 0.1f, this.darkSpire.color.a);
				platformColor = ColorUtils.HexToColor("0c0c0c");
			}
			else
			{
				playerColor = Color.Lerp(Color.white, ColorUtils.HexToColor("d8d8d8"), ratio);
				projectileColor = Color.Lerp(Color.white, ColorUtils.HexToColor("d8d8d8"), ratio);
				dragonColor = Color.black;
				darkSpireColor = new Color(1f, 1f, 1f, this.darkSpire.color.a);
				platformColor = Color.Lerp(Color.white, ColorUtils.HexToColor("9c9da63"), ratio);
			}
			if (playerOne != null)
			{
				playerOne.animationController.SetColor(playerColor);
			}
			if (playerTwo != null)
			{
				playerTwo.animationController.SetColor(playerColor);
			}
			this.darkSpire.color = darkSpireColor;
			if (this.lightningState != lastLightningState)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("PlayerProjectile");
				foreach (GameObject gameObject in array)
				{
					SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
					if (component != null)
					{
						component.color = projectileColor;
					}
				}
			}
			if (!dragonHitFlash.flashing)
			{
				foreach (SpriteRenderer spriteRenderer in this.leftSideDragon.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer.material = ((this.lightningState != DragonLevel.LightningState.Off) ? this.dragonFlashMaterial : this.dragonMaterial);
					spriteRenderer.color = dragonColor;
				}
			}
			foreach (DragonLevelCloudPlatform dragonLevelCloudPlatform in this.manager.platforms)
			{
				dragonLevelCloudPlatform.GetComponent<SpriteRenderer>().color = platformColor;
				dragonLevelCloudPlatform.top.color = platformColor;
			}
			for (int k = 0; k < this.lightningFlashes.Length; k++)
			{
				if (this.lightningState == DragonLevel.LightningState.FirstFlash)
				{
					this.lightningFlashes[k].SetFlash1();
				}
				else if (this.lightningState == DragonLevel.LightningState.SecondFlash)
				{
					this.lightningFlashes[k].SetFlash2();
				}
				else if (this.lightningState == DragonLevel.LightningState.Off)
				{
					this.lightningFlashes[k].SetNormal();
				}
			}
			t += CupheadTime.Delta;
			lastLightningState = this.lightningState;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x00062214 File Offset: 0x00060614
	private IEnumerator lightning_cr()
	{
		for (;;)
		{
			this.lightningState = DragonLevel.LightningState.Off;
			yield return CupheadTime.WaitForSeconds(this, MathUtils.ExpRandom(2f) + 1f);
			this.lightningStrikes.PlayLightning();
			float rand = UnityEngine.Random.value;
			if (rand < 0.25f)
			{
				this.lightningState = DragonLevel.LightningState.FirstFlash;
				yield return CupheadTime.WaitForSeconds(this, 0.041f);
			}
			else if (rand < 0.5f)
			{
				this.lightningState = DragonLevel.LightningState.SecondFlash;
				yield return CupheadTime.WaitForSeconds(this, 0.041f);
			}
			else
			{
				this.lightningState = DragonLevel.LightningState.FirstFlash;
				yield return CupheadTime.WaitForSeconds(this, 0.041f);
				this.lightningState = DragonLevel.LightningState.Off;
				yield return CupheadTime.WaitForSeconds(this, 0.041f);
				this.lightningState = DragonLevel.LightningState.SecondFlash;
				yield return CupheadTime.WaitForSeconds(this, 0.041f);
			}
		}
		yield break;
	}

	// Token: 0x040006FA RID: 1786
	private LevelProperties.Dragon properties;

	// Token: 0x040006FB RID: 1787
	private const float Flash1Probability = 0.25f;

	// Token: 0x040006FC RID: 1788
	private const float Flash2Probability = 0.25f;

	// Token: 0x040006FE RID: 1790
	[SerializeField]
	private DragonLevelBackgroundFlash[] lightningFlashes;

	// Token: 0x040006FF RID: 1791
	[SerializeField]
	private DragonLevelCloudPlatform[] platforms;

	// Token: 0x04000700 RID: 1792
	[SerializeField]
	private SpriteRenderer spire;

	// Token: 0x04000701 RID: 1793
	[SerializeField]
	private SpriteRenderer darkSpire;

	// Token: 0x04000702 RID: 1794
	[SerializeField]
	private DragonLevelDragon dragon;

	// Token: 0x04000703 RID: 1795
	[SerializeField]
	private DragonLevelLeftSideDragon leftSideDragon;

	// Token: 0x04000704 RID: 1796
	[SerializeField]
	private DragonLevelTail tail;

	// Token: 0x04000705 RID: 1797
	[SerializeField]
	private DragonLevelPlatformManager manager;

	// Token: 0x04000706 RID: 1798
	[SerializeField]
	private DragonLevelLightning lightningStrikes;

	// Token: 0x04000707 RID: 1799
	[SerializeField]
	private Material dragonFlashMaterial;

	// Token: 0x04000708 RID: 1800
	private DragonLevel.LightningState lightningState;

	// Token: 0x04000709 RID: 1801
	[SerializeField]
	private SpriteRenderer[] backgroundClouds;

	// Token: 0x0400070A RID: 1802
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x0400070B RID: 1803
	[SerializeField]
	private Sprite _bossPortraitFireMarchers;

	// Token: 0x0400070C RID: 1804
	[SerializeField]
	private Sprite _bossPortraitThreeHeads;

	// Token: 0x0400070D RID: 1805
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x0400070E RID: 1806
	[SerializeField]
	private string _bossQuoteFireMarchers;

	// Token: 0x0400070F RID: 1807
	[SerializeField]
	private string _bossQuoteThreeHeads;

	// Token: 0x04000710 RID: 1808
	private bool cloudsSameDir;

	// Token: 0x04000711 RID: 1809
	private Material dragonMaterial;

	// Token: 0x020005E8 RID: 1512
	private enum LightningState
	{
		// Token: 0x040026CF RID: 9935
		Off,
		// Token: 0x040026D0 RID: 9936
		FirstFlash,
		// Token: 0x040026D1 RID: 9937
		SecondFlash
	}

	// Token: 0x020005E9 RID: 1513
	[Serializable]
	public class Prefabs
	{
	}
}
