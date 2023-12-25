using System;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public class TutorialLevel : Level
{
	// Token: 0x06000835 RID: 2101 RVA: 0x000789EC File Offset: 0x00076DEC
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Tutorial.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000836 RID: 2102 RVA: 0x00078A82 File Offset: 0x00076E82
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Tutorial;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000837 RID: 2103 RVA: 0x00078A85 File Offset: 0x00076E85
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_tutorial;
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000838 RID: 2104 RVA: 0x00078A88 File Offset: 0x00076E88
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000839 RID: 2105 RVA: 0x00078A90 File Offset: 0x00076E90
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00078A98 File Offset: 0x00076E98
	protected override void Start()
	{
		base.Start();
		this.background.SetParent(UnityEngine.Camera.main.transform);
		this.background.ResetLocalTransforms();
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00078AC0 File Offset: 0x00076EC0
	protected override void OnLevelStart()
	{
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00078AC4 File Offset: 0x00076EC4
	public void GoBackToHouse()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.playerGoBackToHouseEffects[0].gameObject.SetActive(true);
		this.playerGoBackToHouseEffects[0].transform.position = player.transform.position;
		player.gameObject.SetActive(false);
		this.playerGoBackToHouseEffects[0].animator.SetTrigger("OnStartTutorial");
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			this.playerGoBackToHouseEffects[1].gameObject.SetActive(true);
			this.playerGoBackToHouseEffects[1].transform.position = player.transform.position;
			player.gameObject.SetActive(false);
			this.playerGoBackToHouseEffects[1].animator.SetTrigger("OnStartTutorial");
		}
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00078B94 File Offset: 0x00076F94
	protected override void OnDestroy()
	{
		base.OnDestroy();
		for (int i = 0; i < this.playerGoBackToHouseEffects.Length; i++)
		{
			this.playerGoBackToHouseEffects[i].Clean();
		}
		this.playerGoBackToHouseEffects = null;
	}

	// Token: 0x040010A2 RID: 4258
	private LevelProperties.Tutorial properties;

	// Token: 0x040010A3 RID: 4259
	[SerializeField]
	private Transform background;

	// Token: 0x040010A4 RID: 4260
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x040010A5 RID: 4261
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x040010A6 RID: 4262
	[SerializeField]
	private PlayerDeathEffect[] playerGoBackToHouseEffects;

	// Token: 0x02000831 RID: 2097
	[Serializable]
	public class Prefabs
	{
	}
}
