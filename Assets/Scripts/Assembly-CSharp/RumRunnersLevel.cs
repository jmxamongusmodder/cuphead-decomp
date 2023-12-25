using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000274 RID: 628
public class RumRunnersLevel : Level
{
	// Token: 0x060006F1 RID: 1777 RVA: 0x000723C4 File Offset: 0x000707C4
	protected override void PartialInit()
	{
		this.properties = LevelProperties.RumRunners.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0007245A File Offset: 0x0007085A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.RumRunners;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x060006F3 RID: 1779 RVA: 0x00072461 File Offset: 0x00070861
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_rum_runners;
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060006F4 RID: 1780 RVA: 0x00072468 File Offset: 0x00070868
	// (remove) Token: 0x060006F5 RID: 1781 RVA: 0x000724A0 File Offset: 0x000708A0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Rangef> OnUpperBridgeDestroy;

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x060006F6 RID: 1782 RVA: 0x000724D8 File Offset: 0x000708D8
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.RumRunners.States.Main:
				return this._bossPortraitMain;
			case LevelProperties.RumRunners.States.Worm:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.RumRunners.States.Anteater:
				return this._bossPortraitPhaseThree;
			case LevelProperties.RumRunners.States.MobBoss:
				return this._bossPortraitPhaseFour;
			}
			global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00072564 File Offset: 0x00070964
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.RumRunners.States.Main:
				return this._bossQuoteMain;
			case LevelProperties.RumRunners.States.Worm:
				return this._bossQuotePhaseTwo;
			case LevelProperties.RumRunners.States.Anteater:
				return this._bossQuotePhaseThree;
			case LevelProperties.RumRunners.States.MobBoss:
				return this._bossQuotePhaseFour;
			}
			global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x000725F0 File Offset: 0x000709F0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this._bossPortraitPhaseFour = null;
		if (CupheadLevelCamera.Current != null)
		{
			CupheadLevelCamera.Current.OnShakeEvent -= this.onShakeEventHandler;
		}
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00072648 File Offset: 0x00070A48
	protected override void Start()
	{
		base.Start();
		this.spider.LevelInit(this.properties);
		this.worm.LevelInit(this.properties);
		this.anteater.LevelInit(this.properties);
		CupheadLevelCamera.Current.OnShakeEvent += this.onShakeEventHandler;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x000726A4 File Offset: 0x00070AA4
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(new Vector3(this.topPlatformEffectRange.minimum, -1000f), new Vector3(this.topPlatformEffectRange.minimum, 1000f));
		Gizmos.DrawLine(new Vector3(this.topPlatformEffectRange.maximum, -1000f), new Vector3(this.topPlatformEffectRange.maximum, 1000f));
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(new Vector3(this.middlePlatformEffectRange.minimum, -1000f), new Vector3(this.middlePlatformEffectRange.minimum, 1000f));
		Gizmos.DrawLine(new Vector3(this.middlePlatformEffectRange.maximum, -1000f), new Vector3(this.middlePlatformEffectRange.maximum, 1000f));
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00072788 File Offset: 0x00070B88
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		this.StopAllCoroutines();
		if (this.properties.CurrentState.stateName == LevelProperties.RumRunners.States.Worm)
		{
			base.StartCoroutine(this.worm_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.RumRunners.States.Anteater)
		{
			base.StartCoroutine(this.anteater_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.RumRunners.States.MobBoss)
		{
			base.StartCoroutine(this.winFakeout_cr());
		}
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00072814 File Offset: 0x00070C14
	private IEnumerator worm_cr()
	{
		this.spider.Die();
		while (this.spider != null)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.worm.Setup();
		this.worm.StartBarrels();
		this.ph2SpiderAnimation.gameObject.SetActive(true);
		yield return this.ph2SpiderAnimation.WaitForAnimationToEnd(this, "Ph2", false, true);
		yield return CupheadTime.WaitForSeconds(this, RumRunnersLevel.SpiderTransitionLowerRopeDuration);
		this.worm.StartWorm(this.mobIntro.bugGirlDamage);
		yield return CupheadTime.WaitForSeconds(this, RumRunnersLevel.SpiderTransitionLowerBugDuration);
		this.ph2SpiderAnimation.SetTrigger("End");
		RumRunnersLevelPh2StartAnimation ph2 = this.ph2SpiderAnimation.GetComponent<RumRunnersLevelPh2StartAnimation>();
		while (!ph2.dropped)
		{
			yield return null;
		}
		this.worm.introDrop = true;
		yield break;
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00072830 File Offset: 0x00070C30
	private IEnumerator anteater_cr()
	{
		this.worm.StartDeath();
		yield return this.worm.animator.WaitForAnimationToEnd(this, "Fall", false, true);
		while (Mathf.Abs(this.worm.transform.position.x) > RumRunnersLevel.AnteaterIntroWormTriggerDistance)
		{
			yield return null;
		}
		this.anteater.gameObject.SetActive(true);
		yield return this.anteater.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		this.anteater.StartAnteater();
		yield break;
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0007284B File Offset: 0x00070C4B
	public void DestroyMiddleBridge()
	{
		this.destroyPlatforms(this.destroyedPlatformsMiddle, this.destroyedSpritesMiddleA, this.middlePlatformEffectRange);
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00072865 File Offset: 0x00070C65
	public void DestroyUpperBridge()
	{
		if (this.OnUpperBridgeDestroy != null)
		{
			this.OnUpperBridgeDestroy(this.topPlatformEffectRange);
		}
		this.destroyPlatforms(this.destroyedPlatformsUpper, this.destroyedSpritesUpperA, this.topPlatformEffectRange);
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0007289C File Offset: 0x00070C9C
	public void ShatterBridges()
	{
		this.destroyPlatforms(null, this.destroyedSpritesMiddleB, default(Rangef));
		this.destroyPlatforms(null, this.destroyedSpritesUpperB, default(Rangef));
		float[] array = new float[]
		{
			0f,
			0.25f,
			0.5f,
			0.75f
		};
		array.Shuffle<float>();
		float num = 1280f / this.camera.zoom;
		for (int i = 0; i < array.Length; i++)
		{
			float value = UnityEngine.Random.Range(array[i], array[i] + array[1]);
			value = MathUtilities.LerpMapping(value, 0f, 1f, -num * 0.4f, num * 0.4f, true);
			this.FullscreenDirt(1, new float?(value), 0.15f, 0.3f);
		}
		CupheadLevelCamera.Current.Shake(55f, 0.5f, true);
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00072974 File Offset: 0x00070D74
	private void destroyPlatforms(LevelPlatform[] colliders, GameObject[] sprites, Rangef checkRange = default(Rangef))
	{
		if (colliders != null)
		{
			foreach (LevelPlatform levelPlatform in colliders)
			{
				LevelPlatform levelPlatform2 = null;
				int num = Array.IndexOf<LevelPlatform>(this.swapPlatformsMappingBefore, levelPlatform);
				if (num >= 0)
				{
					levelPlatform2 = this.swapPlatformsMappingAfter[num];
				}
				if (levelPlatform.GetComponentInChildren<LevelPlayerController>())
				{
					LevelPlayerController[] componentsInChildren = levelPlatform.GetComponentsInChildren<LevelPlayerController>();
					foreach (LevelPlayerController levelPlayerController in componentsInChildren)
					{
						if (!checkRange.ContainsExclusive(levelPlayerController.transform.position.x) && levelPlatform2 != null)
						{
							levelPlatform2.AddChild(levelPlayerController.transform);
						}
						else
						{
							levelPlayerController.motor.OnTrampolineKnockUp(RumRunnersLevel.BridgeDestroyBounceHeight);
						}
					}
				}
			}
			foreach (LevelPlatform levelPlatform3 in colliders)
			{
				UnityEngine.Object.Destroy(levelPlatform3.gameObject);
			}
		}
		if (sprites != null)
		{
			foreach (GameObject gameObject in sprites)
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00072AB0 File Offset: 0x00070EB0
	private IEnumerator winFakeout_cr()
	{
		this.anteater.FakeDeathStart();
		AudioManager.Play("level_announcer_knockout_bell");
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_knockout");
		base.StartCoroutine(this.stingSound_cr());
		Vector3 bannerPosition = this.fakeBannerAnimator.transform.position;
		bannerPosition += CupheadLevelCamera.Current.transform.position;
		this.fakeBannerAnimator.transform.position = bannerPosition;
		this.fakeBannerAnimator.SetTrigger("Banner");
		Coroutine dirtCoroutine = base.StartCoroutine(this.fakeDeathDirt_cr());
		yield return this.fakeBannerAnimator.WaitForAnimationToEnd(this, "Banner", false, true);
		base.StopCoroutine(dirtCoroutine);
		this.anteater.FakeDeathContinue();
		yield break;
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00072ACC File Offset: 0x00070ECC
	private IEnumerator fakeDeathDirt_cr()
	{
		float elapsedTime = 0f;
		float delay = 0.4f;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			elapsedTime += delay;
			delay = Mathf.Lerp(0.4f, 0.8f, elapsedTime / 1.5f);
			this.FullscreenDirt(2, null, -1f, -1f);
		}
		yield break;
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00072AE8 File Offset: 0x00070EE8
	private IEnumerator stingSound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		AudioManager.Play("sfx_dlc_rumrun_fake_levelbossdefeatsting");
		yield break;
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00072B03 File Offset: 0x00070F03
	public void FullscreenDirt(int count, float? positionX = null, float customInitialDelay = -1f, float customIntraDelay = -1f)
	{
		base.StartCoroutine(this.dirtFX_cr(count, positionX, customInitialDelay, customIntraDelay));
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00072B18 File Offset: 0x00070F18
	private IEnumerator dirtFX_cr(int count, float? positionX, float customInitialDelay, float customIntraDelay)
	{
		MinMax[] DirtRandomizationX = new MinMax[]
		{
			new MinMax(-50f, 0f),
			new MinMax(0f, 50f)
		};
		MinMax WaitRange = new MinMax(0.08f, 0.12f);
		MinMax previousSpawnRange = (!Rand.Bool()) ? new MinMax(0.5f, 1f) : new MinMax(0f, 0.5f);
		for (int i = 0; i < count; i++)
		{
			float initialDelay = WaitRange.RandomFloat();
			if (customInitialDelay >= 0f)
			{
				initialDelay = customInitialDelay;
			}
			yield return CupheadTime.WaitForSeconds(this, initialDelay);
			Vector3 position = new Vector3(0f, 360f / this.camera.zoom);
			if (positionX == null)
			{
				MinMax minMax;
				if (previousSpawnRange.min == 0f)
				{
					minMax = new MinMax(0.5f, 1f);
				}
				else
				{
					minMax = new MinMax(0f, 0.5f);
				}
				position.x = 1280f * minMax.RandomFloat() - 640f;
				previousSpawnRange = minMax;
			}
			else
			{
				position.x = positionX.Value;
			}
			DirtRandomizationX.Shuffle<MinMax>();
			for (int spawn = 0; spawn < DirtRandomizationX.Length; spawn++)
			{
				this.fullscreenDirtFX.Create(position + new Vector3(DirtRandomizationX[spawn].RandomFloat(), 0f));
				float intraDelay = UnityEngine.Random.Range(0.1f, 0.2f);
				if (customIntraDelay >= 0f)
				{
					intraDelay = UnityEngine.Random.Range(customIntraDelay * 0.8f, customIntraDelay * 1.2f);
				}
				yield return CupheadTime.WaitForSeconds(this, intraDelay);
			}
		}
		yield break;
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00072B50 File Offset: 0x00070F50
	private void onShakeEventHandler(float amount, float time)
	{
		this.FullscreenDirt(2, null, -1f, -1f);
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00072B77 File Offset: 0x00070F77
	protected override void PlayAnnouncerReady()
	{
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_ready");
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x00072B83 File Offset: 0x00070F83
	protected override void PlayAnnouncerBegin()
	{
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_begin");
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00072B90 File Offset: 0x00070F90
	protected override IEnumerator knockoutSFX_cr()
	{
		AudioManager.Play("level_announcer_knockout_bell");
		AudioManager.Play("sfx_DLC_RUMRUN_VX_AnnouncerClearThroat");
		yield return CupheadTime.WaitForSeconds(this, 1.4f);
		AudioManager.Play("level_boss_defeat_sting");
		yield break;
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00072BAC File Offset: 0x00070FAC
	public static float GroundWalkingPosY(Vector2 position, Collider2D collider, float offset = 0f, float rayLength = 200f)
	{
		int layerMask = 1048576;
		position.x = Mathf.Clamp(position.x, (float)Level.Current.Left, (float)Level.Current.Right);
		Vector3 v = new Vector3(position.x, position.y);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(v, Vector3.down, rayLength, layerMask);
		if (raycastHit2D.collider != null)
		{
			float y = raycastHit2D.point.y;
			float num = (!collider) ? 0f : (-collider.offset.y + collider.bounds.size.y / 2f);
			return y + num + offset;
		}
		return position.y;
	}

	// Token: 0x04000DE0 RID: 3552
	private LevelProperties.RumRunners properties;

	// Token: 0x04000DE1 RID: 3553
	private static readonly float SpiderTransitionLowerRopeDuration = 0.5f;

	// Token: 0x04000DE2 RID: 3554
	private static readonly float SpiderTransitionLowerBugDuration = 0.3f;

	// Token: 0x04000DE3 RID: 3555
	private static readonly float BridgeDestroyBounceHeight = -1f;

	// Token: 0x04000DE4 RID: 3556
	private static readonly float AnteaterIntroWormTriggerDistance = 500f;

	// Token: 0x04000DE6 RID: 3558
	[SerializeField]
	private Animator ph2SpiderAnimation;

	// Token: 0x04000DE7 RID: 3559
	[SerializeField]
	private RumRunnersLevelSpider spider;

	// Token: 0x04000DE8 RID: 3560
	[SerializeField]
	private RumRunnersLevelWorm worm;

	// Token: 0x04000DE9 RID: 3561
	[SerializeField]
	private RumRunnersLevelAnteater anteater;

	// Token: 0x04000DEA RID: 3562
	[SerializeField]
	private RumRunnersLevelMobIntroAnimation mobIntro;

	// Token: 0x04000DEB RID: 3563
	[SerializeField]
	private Effect fullscreenDirtFX;

	// Token: 0x04000DEC RID: 3564
	[SerializeField]
	private Animator fakeBannerAnimator;

	// Token: 0x04000DED RID: 3565
	[SerializeField]
	private GameObject[] destroyedSpritesMiddleA;

	// Token: 0x04000DEE RID: 3566
	[SerializeField]
	private GameObject[] destroyedSpritesMiddleB;

	// Token: 0x04000DEF RID: 3567
	[SerializeField]
	private GameObject[] destroyedSpritesUpperA;

	// Token: 0x04000DF0 RID: 3568
	[SerializeField]
	private GameObject[] destroyedSpritesUpperB;

	// Token: 0x04000DF1 RID: 3569
	[SerializeField]
	private LevelPlatform[] destroyedPlatformsMiddle;

	// Token: 0x04000DF2 RID: 3570
	[SerializeField]
	private LevelPlatform[] destroyedPlatformsUpper;

	// Token: 0x04000DF3 RID: 3571
	[SerializeField]
	private LevelPlatform[] swapPlatformsMappingBefore;

	// Token: 0x04000DF4 RID: 3572
	[SerializeField]
	private LevelPlatform[] swapPlatformsMappingAfter;

	// Token: 0x04000DF5 RID: 3573
	[SerializeField]
	private Rangef middlePlatformEffectRange;

	// Token: 0x04000DF6 RID: 3574
	[SerializeField]
	private Rangef topPlatformEffectRange;

	// Token: 0x04000DF7 RID: 3575
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000DF8 RID: 3576
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;

	// Token: 0x04000DF9 RID: 3577
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;

	// Token: 0x04000DFA RID: 3578
	[SerializeField]
	private Sprite _bossPortraitPhaseFour;

	// Token: 0x04000DFB RID: 3579
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000DFC RID: 3580
	[SerializeField]
	private string _bossQuotePhaseTwo;

	// Token: 0x04000DFD RID: 3581
	[SerializeField]
	private string _bossQuotePhaseThree;

	// Token: 0x04000DFE RID: 3582
	[SerializeField]
	private string _bossQuotePhaseFour;
}
