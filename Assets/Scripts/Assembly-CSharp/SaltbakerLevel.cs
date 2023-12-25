using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x0200029D RID: 669
public class SaltbakerLevel : Level
{
	// Token: 0x06000759 RID: 1881 RVA: 0x0007460C File Offset: 0x00072A0C
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Saltbaker.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x0600075A RID: 1882 RVA: 0x000746A2 File Offset: 0x00072AA2
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Saltbaker;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x0600075B RID: 1883 RVA: 0x000746A9 File Offset: 0x00072AA9
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_saltbaker;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x0600075C RID: 1884 RVA: 0x000746B0 File Offset: 0x00072AB0
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Saltbaker.States.Main:
				return this._bossPortraitMain;
			case LevelProperties.Saltbaker.States.PhaseTwo:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.Saltbaker.States.PhaseThree:
				return this._bossPortraitPhaseThree;
			case LevelProperties.Saltbaker.States.PhaseFour:
				return this._bossPortraitPhaseFour;
			}
			global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x0600075D RID: 1885 RVA: 0x0007473C File Offset: 0x00072B3C
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Saltbaker.States.Main:
				return this._bossQuoteMain;
			case LevelProperties.Saltbaker.States.PhaseTwo:
				return this._bossQuotePhaseTwo;
			case LevelProperties.Saltbaker.States.PhaseThree:
				return this._bossQuotePhaseThree;
			case LevelProperties.Saltbaker.States.PhaseFour:
				return this._bossQuotePhaseFour;
			}
			global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x0600075E RID: 1886 RVA: 0x000747C5 File Offset: 0x00072BC5
	// (set) Token: 0x0600075F RID: 1887 RVA: 0x000747CD File Offset: 0x00072BCD
	public bool playerLost { get; private set; }

	// Token: 0x06000760 RID: 1888 RVA: 0x000747D8 File Offset: 0x00072BD8
	protected override void Start()
	{
		base.Start();
		this.trappedCharacter.Setup();
		this.trappedCharacterPhaseThree.Setup();
		this.saltbaker.LevelInit(this.properties);
		this.saltbakerBouncer.LevelInit(this.properties);
		this.saltbakerPillarHandler.LevelInit(this.properties);
		this.fires = new List<SaltbakerLevelJumper>();
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x0007483F File Offset: 0x00072C3F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this._bossPortraitPhaseFour = null;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x00074863 File Offset: 0x00072C63
	protected override void OnLose()
	{
		this.playerLost = true;
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x0007486C File Offset: 0x00072C6C
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Saltbaker.States.PhaseTwo)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.saltbaker.phase_one_to_two_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Saltbaker.States.PhaseThree)
		{
			this.StopAllCoroutines();
			this.KillFires();
			this.saltbaker.OnPhaseThree();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Saltbaker.States.PhaseFour)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.phase_three_to_four_cr());
		}
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x00074910 File Offset: 0x00072D10
	public void StartPhase3()
	{
		this.ClearFires();
		this.phase3BG.SetActive(true);
		this.bounds.bottomEnabled = false;
		GameObject.Find("Level_Ground").SetActive(false);
		Level.Current.SetBounds(null, null, new int?(500), null);
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.transform.position = new Vector3(levelPlayerController.transform.position.x, (float)Level.Current.Ceiling);
				levelPlayerController.motor.DoPostSuperHop();
			}
		}
		this.SpawnCutters();
		base.StartCoroutine(this.phase_two_to_three_cr());
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x00074A20 File Offset: 0x00072E20
	private IEnumerator phase_two_to_three_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		while (t < 1f)
		{
			t += CupheadTime.FixedDelta;
			this.transitionFader.color = new Color(1f, 1f, 1f, Mathf.InverseLerp(1f, 0f, t));
			yield return wait;
		}
		this.saltbakerBouncer.gameObject.SetActive(true);
		this.saltbakerBouncer.StartBouncer(new Vector3(0f, 700f));
		this.transitionFader.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x00074A3C File Offset: 0x00072E3C
	private IEnumerator phase_three_to_four_cr()
	{
		this.DestroyRunners();
		this.saltbakerBouncer.EndBouncer();
		this.groundCrack.Play("Start");
		base.StartCoroutine(this.flash_sky_cr());
		this.tornadoActivator.enabled = true;
		this.phase3to4Transition.StartSaltman();
		while (this.saltbakerBouncer != null)
		{
			yield return null;
		}
		this.saltbakerPillarHandler.gameObject.SetActive(true);
		this.saltbakerPillarHandler.StartPlatforms();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.saltbakerPillarHandler.suppressCenterPlatforms = false;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.groundCrack.SetTrigger("Continue");
		this.tornadoActivator.SetTrigger("Continue");
		this.saltbakerPillarHandler.StartPillarOfDoom();
		while (this.phase3to4Transition.enabled)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.saltbakerPillarHandler.StartHeart();
		base.StartCoroutine(this.bg_salt_spillage_cr());
		UnityEngine.Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer("Renderer");
		this.phaseFourBlurCamera.SetActive(true);
		this.phaseFourBlurTexture.gameObject.SetActive(true);
		float t = 0f;
		while (t < this.phaseFourBlurDimTime)
		{
			t += CupheadTime.Delta;
			float tNormalized = t / this.phaseFourBlurDimTime;
			this.phaseFourBlurController.blurSize = Mathf.Lerp(0f, this.phaseFourBlurAmount, tNormalized);
			this.phaseFourBlurTexture.material.color = Color.Lerp(Color.white, new Color(this.phaseFourDimAmount, this.phaseFourDimAmount, this.phaseFourDimAmount, 1f), tNormalized);
			yield return null;
		}
		this.phaseFourBlurController.blurSize = this.phaseFourBlurAmount;
		this.phaseFourBlurTexture.material.color = new Color(this.phaseFourDimAmount, this.phaseFourDimAmount, this.phaseFourDimAmount, 1f);
		yield break;
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x00074A58 File Offset: 0x00072E58
	private IEnumerator flash_sky_cr()
	{
		for (;;)
		{
			float dimRate = UnityEngine.Random.Range(2f, 4f);
			this.skyFront.color = Color.white;
			while (this.skyFront.color.r > 0f)
			{
				float c = this.skyFront.color.r - 0.041666668f * dimRate;
				this.skyFront.color = new Color(c, c, c, 1f);
				yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
			}
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.1f, 4f));
		}
		yield break;
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x00074A74 File Offset: 0x00072E74
	private IEnumerator bg_salt_spillage_cr()
	{
		this.saltSpillageDelay = new PatternString(this.saltSpillageDelayString, true);
		this.saltSpillageOrder = new PatternString(this.saltSpillageOrderString, true);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.saltSpillageDelay.PopFloat());
			this.groundCrack.Play("Spill", this.saltSpillageOrder.PopInt(), 0f);
		}
		yield break;
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x00074A90 File Offset: 0x00072E90
	public void SpawnJumpers()
	{
		int numberFireJumpers = this.properties.CurrentState.jumper.numberFireJumpers;
		if (numberFireJumpers == 0)
		{
			return;
		}
		float y = (float)Level.Current.Ceiling;
		float num = (float)Level.Current.Left;
		float num2 = (float)(Level.Current.Width / numberFireJumpers);
		float num3 = num + num2 / 2f;
		float jumpDelay = this.properties.CurrentState.jumper.jumpDelay;
		for (int i = 0; i < numberFireJumpers; i++)
		{
			float x = num3 + num2 * (float)i;
			Vector3 position = new Vector3(x, y);
			SaltbakerLevelJumper saltbakerLevelJumper = this.jumperPrefab.Create(position, this, this.properties.CurrentState.swooper, this.properties.CurrentState.jumper, jumpDelay * (float)i, false);
			saltbakerLevelJumper.GetComponent<SpriteRenderer>().sortingOrder = i + -5;
			this.fires.Add(saltbakerLevelJumper);
		}
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x00074B84 File Offset: 0x00072F84
	public void SpawnSwoopers()
	{
		float[] array = new float[]
		{
			(float)Level.Current.Right / 2f,
			(float)Level.Current.Left / 2f
		};
		int num = this.properties.CurrentState.swooper.numberFireSwoopers;
		if (num > 2)
		{
			global::Debug.Break();
			num = 2;
		}
		if (num == 0)
		{
			return;
		}
		float num2 = (float)(Level.Current.Left + Level.Current.Right) / 2f;
		float jumpDelay = this.properties.CurrentState.swooper.jumpDelay;
		for (int i = 0; i < num; i++)
		{
			SaltbakerLevelJumper saltbakerLevelJumper = this.jumperPrefab.Create(new Vector3(array[i], (float)Level.Current.Ceiling), this, this.properties.CurrentState.swooper, this.properties.CurrentState.jumper, jumpDelay * (float)i, true);
			saltbakerLevelJumper.GetComponent<SpriteRenderer>().sortingOrder = 3 + i;
			this.fires.Add(saltbakerLevelJumper);
		}
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x00074C9C File Offset: 0x0007309C
	public void KillFires()
	{
		foreach (SaltbakerLevelJumper saltbakerLevelJumper in this.fires)
		{
			saltbakerLevelJumper.Die();
		}
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x00074CF8 File Offset: 0x000730F8
	public void ClearFires()
	{
		foreach (SaltbakerLevelJumper saltbakerLevelJumper in this.fires)
		{
			if (saltbakerLevelJumper != null)
			{
				UnityEngine.Object.Destroy(saltbakerLevelJumper.gameObject);
			}
		}
		this.fires.Clear();
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00074D70 File Offset: 0x00073170
	public bool IsPositionAvailable(Vector3 pos, SaltbakerLevelJumper fire)
	{
		float num = fire.GetComponent<Collider2D>().bounds.size.x * 1f;
		for (int i = 0; i < this.fires.Count; i++)
		{
			if (this.fires[i] != fire)
			{
				float num2 = pos.x + num;
				float num3 = pos.x - num;
				if (num2 > this.fires[i].GetAimPos().x - num && num3 < this.fires[i].GetAimPos().x + num)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x00074E34 File Offset: 0x00073234
	private void SpawnCutters()
	{
		LevelProperties.Saltbaker.Cutter cutter = this.properties.CurrentState.cutter;
		if (cutter.cutterCount <= 0)
		{
			return;
		}
		this.cutters = new List<SaltbakerLevelCutter>();
		AbstractPlayerController next = PlayerManager.GetNext();
		float num = 50f;
		float minDistance = 50f;
		float[] array = new float[2];
		bool flag = Rand.Bool();
		List<Vector2> list = new List<Vector2>();
		float num2 = Mathf.Min(PlayerManager.GetNext().transform.position.x, PlayerManager.GetNext().transform.position.x);
		float num3 = Mathf.Max(PlayerManager.GetNext().transform.position.x, PlayerManager.GetNext().transform.position.x);
		list.Add(new Vector2((float)Level.Current.Left + num, num2));
		list.Add(new Vector2(num2, num3));
		list.Add(new Vector2(num3, (float)Level.Current.Right - num));
		list.RemoveAll((Vector2 s) => Mathf.Abs(s.x - s.y) < minDistance * 2f);
		list.Sort((Vector2 s1, Vector2 s2) => Mathf.Abs(s1.x - s1.y).CompareTo(Mathf.Abs(s2.x - s2.y)));
		if (list.Count == 3)
		{
			list.RemoveAt(0);
		}
		if (list.Count == 2)
		{
			array[0] = Mathf.Lerp(list[0].x, list[0].y, 0.5f);
			array[1] = Mathf.Lerp(list[1].x, list[1].y, 0.5f);
		}
		if (list.Count == 1)
		{
			array[0] = Mathf.Lerp(list[0].x, list[0].y, 0.333f);
			array[1] = Mathf.Lerp(list[0].x, list[0].y, 0.667f);
		}
		for (int i = 0; i < cutter.cutterCount; i++)
		{
			Vector3 position = new Vector3(array[i], (float)Level.Current.Ground + 26f);
			SaltbakerLevelCutter item = this.cutterPrefab.Create(position, cutter.cutterSpeed, flag, i);
			flag = !flag;
			this.cutters.Add(item);
		}
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x000750E0 File Offset: 0x000734E0
	private void DestroyRunners()
	{
		if (this.cutters == null)
		{
			return;
		}
		foreach (SaltbakerLevelCutter saltbakerLevelCutter in this.cutters)
		{
			saltbakerLevelCutter.Sink();
		}
		this.cutters.Clear();
	}

	// Token: 0x04000EFF RID: 3839
	private LevelProperties.Saltbaker properties;

	// Token: 0x04000F00 RID: 3840
	private const float FIRE_OFFSET_MODIFIER = 1f;

	// Token: 0x04000F01 RID: 3841
	[SerializeField]
	private GameObject phase3BG;

	// Token: 0x04000F02 RID: 3842
	[SerializeField]
	private GameObject[] cracksBG;

	// Token: 0x04000F03 RID: 3843
	[SerializeField]
	private SaltbakerLevelCutter cutterPrefab;

	// Token: 0x04000F04 RID: 3844
	private List<SaltbakerLevelCutter> cutters;

	// Token: 0x04000F05 RID: 3845
	[SerializeField]
	private SaltbakerLevelJumper jumperPrefab;

	// Token: 0x04000F06 RID: 3846
	private List<SaltbakerLevelJumper> fires;

	// Token: 0x04000F07 RID: 3847
	[SerializeField]
	private SaltbakerLevelSaltbaker saltbaker;

	// Token: 0x04000F08 RID: 3848
	[SerializeField]
	private SaltbakerLevelBouncer saltbakerBouncer;

	// Token: 0x04000F09 RID: 3849
	[SerializeField]
	private SaltbakerLevelPillarHandler saltbakerPillarHandler;

	// Token: 0x04000F0A RID: 3850
	[SerializeField]
	private SpriteRenderer skyFront;

	// Token: 0x04000F0B RID: 3851
	[SerializeField]
	private SpriteRenderer transitionFader;

	// Token: 0x04000F0C RID: 3852
	[SerializeField]
	private SaltbakerLevelPhaseThreeToFourTransition phase3to4Transition;

	// Token: 0x04000F0D RID: 3853
	[SerializeField]
	private string saltSpillageOrderString;

	// Token: 0x04000F0E RID: 3854
	private PatternString saltSpillageOrder;

	// Token: 0x04000F0F RID: 3855
	[SerializeField]
	private string saltSpillageDelayString;

	// Token: 0x04000F10 RID: 3856
	private PatternString saltSpillageDelay;

	// Token: 0x04000F11 RID: 3857
	[SerializeField]
	private Animator groundCrack;

	// Token: 0x04000F12 RID: 3858
	[SerializeField]
	private Animator tornadoActivator;

	// Token: 0x04000F13 RID: 3859
	[SerializeField]
	private GameObject phaseFourBlurCamera;

	// Token: 0x04000F14 RID: 3860
	[SerializeField]
	private MeshRenderer phaseFourBlurTexture;

	// Token: 0x04000F15 RID: 3861
	[SerializeField]
	private float phaseFourBlurAmount = 3f;

	// Token: 0x04000F16 RID: 3862
	[SerializeField]
	private float phaseFourDimAmount = 0.8f;

	// Token: 0x04000F17 RID: 3863
	[SerializeField]
	private float phaseFourBlurDimTime = 1f;

	// Token: 0x04000F18 RID: 3864
	[SerializeField]
	private BlurOptimized phaseFourBlurController;

	// Token: 0x04000F19 RID: 3865
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000F1A RID: 3866
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;

	// Token: 0x04000F1B RID: 3867
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;

	// Token: 0x04000F1C RID: 3868
	[SerializeField]
	private Sprite _bossPortraitPhaseFour;

	// Token: 0x04000F1D RID: 3869
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000F1E RID: 3870
	[SerializeField]
	private string _bossQuotePhaseTwo;

	// Token: 0x04000F1F RID: 3871
	[SerializeField]
	private string _bossQuotePhaseThree;

	// Token: 0x04000F20 RID: 3872
	[SerializeField]
	private string _bossQuotePhaseFour;

	// Token: 0x04000F21 RID: 3873
	private int crackOn;

	// Token: 0x04000F22 RID: 3874
	public float yScrollPos;

	// Token: 0x04000F23 RID: 3875
	[SerializeField]
	private SaltbakerLevelBGTrappedCharacter trappedCharacter;

	// Token: 0x04000F24 RID: 3876
	[SerializeField]
	private SaltbakerLevelBGTrappedCharacter trappedCharacterPhaseThree;
}
