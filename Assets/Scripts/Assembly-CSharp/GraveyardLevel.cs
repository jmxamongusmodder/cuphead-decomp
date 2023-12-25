using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F5 RID: 501
public class GraveyardLevel : Level
{
	// Token: 0x06000587 RID: 1415 RVA: 0x00068FC4 File Offset: 0x000673C4
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Graveyard.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000588 RID: 1416 RVA: 0x0006905A File Offset: 0x0006745A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Graveyard;
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06000589 RID: 1417 RVA: 0x00069061 File Offset: 0x00067461
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_graveyard;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x0600058A RID: 1418 RVA: 0x00069065 File Offset: 0x00067465
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x0600058B RID: 1419 RVA: 0x0006906D File Offset: 0x0006746D
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00069075 File Offset: 0x00067475
	protected override void Awake()
	{
		this.originalMode = Level.CurrentMode;
		Level.SetCurrentMode(Level.Mode.Normal);
		base.Awake();
		Level.IsGraveyard = true;
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x00069094 File Offset: 0x00067494
	protected override void Start()
	{
		base.Start();
		for (int i = 0; i < this.splitDevil.Length; i++)
		{
			this.splitDevil[i].LevelInit(this.properties);
		}
		this.attackCounterString = new PatternString(this.properties.CurrentState.splitDevilBeam.attacksBeforeBeamString, true);
		this.attackCounter = this.attackCounterString.PopInt();
		AudioManager.PlayLoop("sfx_dlc_graveyard_amb_loop");
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0006910F File Offset: 0x0006750F
	protected override void PlayAnnouncerReady()
	{
		AudioManager.Play("level_announcer_ready_ghostly");
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x0006911B File Offset: 0x0006751B
	protected override void PlayAnnouncerBegin()
	{
		AudioManager.Play("level_announcer_begin_ghostly");
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x00069127 File Offset: 0x00067527
	public bool CheckForBeamAttack()
	{
		this.attackCounter--;
		if (this.attackCounter == -1)
		{
			this.attackCounter = this.attackCounterString.PopInt();
			return true;
		}
		return false;
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00069158 File Offset: 0x00067558
	protected override void OnLevelStart()
	{
		for (int i = 0; i < this.splitDevil.Length; i++)
		{
			this.splitDevil[i].NextPattern();
		}
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0006918C File Offset: 0x0006758C
	protected override void OnWin()
	{
		base.OnWin();
		for (int i = 0; i < this.splitDevil.Length; i++)
		{
			this.splitDevil[i].Die();
		}
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x000691C5 File Offset: 0x000675C5
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		Level.SetCurrentMode(this.originalMode);
		this.splitDevil = null;
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x000691E8 File Offset: 0x000675E8
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

	// Token: 0x06000595 RID: 1429 RVA: 0x00069204 File Offset: 0x00067604
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Graveyard.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000A68 RID: 2664
	private LevelProperties.Graveyard properties;

	// Token: 0x04000A69 RID: 2665
	[SerializeField]
	private GraveyardLevelSplitDevil[] splitDevil;

	// Token: 0x04000A6A RID: 2666
	private PatternString attackCounterString;

	// Token: 0x04000A6B RID: 2667
	private int attackCounter;

	// Token: 0x04000A6C RID: 2668
	private Level.Mode originalMode;

	// Token: 0x04000A6D RID: 2669
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000A6E RID: 2670
	[SerializeField]
	private string _bossQuoteMain;
}
