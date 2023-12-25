using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200028A RID: 650
public class SallyStagePlayLevel : Level
{
	// Token: 0x06000728 RID: 1832 RVA: 0x00073734 File Offset: 0x00071B34
	protected override void PartialInit()
	{
		this.properties = LevelProperties.SallyStagePlay.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000729 RID: 1833 RVA: 0x000737CA File Offset: 0x00071BCA
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.SallyStagePlay;
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x0600072A RID: 1834 RVA: 0x000737D1 File Offset: 0x00071BD1
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_sally_stage_play;
		}
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600072B RID: 1835 RVA: 0x000737D8 File Offset: 0x00071BD8
	// (remove) Token: 0x0600072C RID: 1836 RVA: 0x00073810 File Offset: 0x00071C10
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPhase3;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600072D RID: 1837 RVA: 0x00073848 File Offset: 0x00071C48
	// (remove) Token: 0x0600072E RID: 1838 RVA: 0x00073880 File Offset: 0x00071C80
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPhase2;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600072F RID: 1839 RVA: 0x000738B8 File Offset: 0x00071CB8
	// (remove) Token: 0x06000730 RID: 1840 RVA: 0x000738F0 File Offset: 0x00071CF0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPhase4;

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000731 RID: 1841 RVA: 0x00073928 File Offset: 0x00071D28
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.SallyStagePlay.States.Main:
			case LevelProperties.SallyStagePlay.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.SallyStagePlay.States.House:
				return this._bossPortraitHouse;
			case LevelProperties.SallyStagePlay.States.Angel:
				return this._bossPortraitAngel;
			case LevelProperties.SallyStagePlay.States.Final:
				return this._bossPortraitFinal;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000732 RID: 1842 RVA: 0x000739B4 File Offset: 0x00071DB4
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.SallyStagePlay.States.Main:
			case LevelProperties.SallyStagePlay.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.SallyStagePlay.States.House:
				return this._bossQuoteHouse;
			case LevelProperties.SallyStagePlay.States.Angel:
				return this._bossQuoteAngel;
			case LevelProperties.SallyStagePlay.States.Final:
				return this._bossQuoteFinal;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x00073A40 File Offset: 0x00071E40
	protected override void Start()
	{
		base.Start();
		this.sally.LevelInit(this.properties);
		this.sally.GetParent(this);
		this.angel.LevelInit(this.properties);
		this.backgroundHandler.GetProperties(this.properties, this);
		this.husband.LevelInit(this.properties);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x00073AB4 File Offset: 0x00071EB4
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.SallyStagePlay.States.House)
		{
			base.StartCoroutine(this.residence_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.SallyStagePlay.States.Angel)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.angel_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.SallyStagePlay.States.Final)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.final_cr());
		}
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00073B46 File Offset: 0x00071F46
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.sallystageplayPattern_cr());
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00073B55 File Offset: 0x00071F55
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitAngel = null;
		this._bossPortraitFinal = null;
		this._bossPortraitHouse = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00073B7C File Offset: 0x00071F7C
	private IEnumerator sallystageplayPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.sally.state != SallyStagePlayLevelSally.State.Transition)
			{
				yield return base.StartCoroutine(this.nextPattern_cr());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x00073B98 File Offset: 0x00071F98
	private IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.SallyStagePlay.Pattern.Jump:
			base.StartCoroutine(this.jump_cr());
			break;
		case LevelProperties.SallyStagePlay.Pattern.Umbrella:
			base.StartCoroutine(this.umbrella_cr());
			break;
		case LevelProperties.SallyStagePlay.Pattern.Kiss:
			base.StartCoroutine(this.kiss_cr());
			break;
		case LevelProperties.SallyStagePlay.Pattern.Teleport:
			base.StartCoroutine(this.teleport_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00073BB4 File Offset: 0x00071FB4
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2.2f);
		this.backgroundHandler.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Church);
		yield return null;
		yield break;
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00073BD0 File Offset: 0x00071FD0
	private IEnumerator jump_cr()
	{
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		this.sally.OnJumpAttack();
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00073BEC File Offset: 0x00071FEC
	private IEnumerator umbrella_cr()
	{
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		this.sally.OnUmbrellaAttack();
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00073C08 File Offset: 0x00072008
	private IEnumerator kiss_cr()
	{
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		this.sally.OnKissAttack();
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00073C24 File Offset: 0x00072024
	private IEnumerator teleport_cr()
	{
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		this.sally.OnTeleportAttack();
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x00073C40 File Offset: 0x00072040
	private IEnumerator residence_cr()
	{
		this.backgroundHandler.RollUpCupids();
		this.sally.PrePhase2();
		this.secretTriggered = SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE;
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		if (this.OnPhase2 != null)
		{
			this.OnPhase2();
			this.StopAllCoroutines();
			base.StartCoroutine(this.sallystageplayPattern_cr());
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00073C5C File Offset: 0x0007205C
	private IEnumerator angel_cr()
	{
		if (this.OnPhase3 != null)
		{
			this.OnPhase3();
		}
		this.sally.OnPhase3(SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE);
		yield return null;
		yield break;
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x00073C78 File Offset: 0x00072078
	private IEnumerator final_cr()
	{
		this.angel.OnPhase4();
		if (this.OnPhase4 != null)
		{
			this.OnPhase4();
		}
		yield return null;
		AudioManager.PlayLoop("sally_audience_applause_ph4_loop");
		yield break;
	}

	// Token: 0x04000E76 RID: 3702
	private LevelProperties.SallyStagePlay properties;

	// Token: 0x04000E7A RID: 3706
	[SerializeField]
	private SallyStagePlayLevelBackgroundHandler backgroundHandler;

	// Token: 0x04000E7B RID: 3707
	[SerializeField]
	private SallyStagePlayLevelAngel angel;

	// Token: 0x04000E7C RID: 3708
	[SerializeField]
	private SallyStagePlayLevelSally sally;

	// Token: 0x04000E7D RID: 3709
	[SerializeField]
	private SallyStagePlayLevelFianceDeity husband;

	// Token: 0x04000E7E RID: 3710
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000E7F RID: 3711
	[SerializeField]
	private Sprite _bossPortraitHouse;

	// Token: 0x04000E80 RID: 3712
	[SerializeField]
	private Sprite _bossPortraitAngel;

	// Token: 0x04000E81 RID: 3713
	[SerializeField]
	private Sprite _bossPortraitFinal;

	// Token: 0x04000E82 RID: 3714
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000E83 RID: 3715
	[SerializeField]
	private string _bossQuoteHouse;

	// Token: 0x04000E84 RID: 3716
	[SerializeField]
	private string _bossQuoteAngel;

	// Token: 0x04000E85 RID: 3717
	[SerializeField]
	private string _bossQuoteFinal;
}
