using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class FlyingMermaidLevel : Level
{
	// Token: 0x06000522 RID: 1314 RVA: 0x00066C68 File Offset: 0x00065068
	protected override void PartialInit()
	{
		this.properties = LevelProperties.FlyingMermaid.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000523 RID: 1315 RVA: 0x00066CFE File Offset: 0x000650FE
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingMermaid;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000524 RID: 1316 RVA: 0x00066D05 File Offset: 0x00065105
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_mermaid;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000525 RID: 1317 RVA: 0x00066D09 File Offset: 0x00065109
	// (set) Token: 0x06000526 RID: 1318 RVA: 0x00066D11 File Offset: 0x00065111
	public bool MerdusaTransformStarted { get; set; }

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000527 RID: 1319 RVA: 0x00066D1C File Offset: 0x0006511C
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingMermaid.States.Main:
			case LevelProperties.FlyingMermaid.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.FlyingMermaid.States.Merdusa:
				return this._bossPortraitMerdusa;
			case LevelProperties.FlyingMermaid.States.Head:
				return this._bossPortraitHead;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000528 RID: 1320 RVA: 0x00066D9C File Offset: 0x0006519C
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingMermaid.States.Main:
			case LevelProperties.FlyingMermaid.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.FlyingMermaid.States.Merdusa:
				return this._bossQuoteMerdusa;
			case LevelProperties.FlyingMermaid.States.Head:
				return this._bossQuoteHead;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00066E1C File Offset: 0x0006521C
	protected override void Start()
	{
		base.Start();
		this.mermaid.LevelInit(this.properties);
		this.merdusa.LevelInit(this.properties);
		this.merdusaHead.LevelInit(this.properties);
		this.MerdusaTransformStarted = false;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00066E69 File Offset: 0x00065269
	protected override void OnLevelStart()
	{
		this.mermaid.IntroContinue();
		base.StartCoroutine(this.mermaidPattern_cr());
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00066E84 File Offset: 0x00065284
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingMermaid.States.Merdusa)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.transform_to_merdusa_cr());
		}
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingMermaid.States.Head)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.mermaidPattern_cr());
			base.StartCoroutine(this.transform_to_head_cr());
		}
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00066EF6 File Offset: 0x000652F6
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitHead = null;
		this._bossPortraitMain = null;
		this._bossPortraitMerdusa = null;
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00066F14 File Offset: 0x00065314
	private IEnumerator mermaidPattern_cr()
	{
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x00066F30 File Offset: 0x00065330
	private IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.FlyingMermaid.Pattern.Yell:
			yield return base.StartCoroutine(this.yell_cr());
			break;
		case LevelProperties.FlyingMermaid.Pattern.Summon:
			yield return base.StartCoroutine(this.summon_cr());
			break;
		case LevelProperties.FlyingMermaid.Pattern.Fish:
			yield return base.StartCoroutine(this.fish_cr());
			break;
		case LevelProperties.FlyingMermaid.Pattern.Zap:
			yield return base.StartCoroutine(this.zap_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		case LevelProperties.FlyingMermaid.Pattern.Bubble:
			yield return base.StartCoroutine(this.bubble_cr());
			break;
		case LevelProperties.FlyingMermaid.Pattern.HeadBlast:
			yield return base.StartCoroutine(this.head_blast_cr());
			break;
		case LevelProperties.FlyingMermaid.Pattern.BubbleHeadBlast:
			yield return base.StartCoroutine(this.bubble_head_blast_cr());
			break;
		}
		yield break;
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x00066F4C File Offset: 0x0006534C
	private IEnumerator yell_cr()
	{
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		this.mermaid.StartYell();
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00066F68 File Offset: 0x00065368
	private IEnumerator transform_to_merdusa_cr()
	{
		this.mermaid.StartTransform();
		while (this.merdusa.state != FlyingMermaidLevelMerdusa.State.Idle)
		{
			yield return null;
		}
		base.StartCoroutine(this.mermaidPattern_cr());
		yield break;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00066F84 File Offset: 0x00065384
	private IEnumerator transform_to_head_cr()
	{
		this.merdusa.StartTransform();
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00066FA0 File Offset: 0x000653A0
	private IEnumerator summon_cr()
	{
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		this.mermaid.StartSummon();
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00066FBC File Offset: 0x000653BC
	private IEnumerator fish_cr()
	{
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		this.mermaid.StartFish();
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00066FD8 File Offset: 0x000653D8
	private IEnumerator zap_cr()
	{
		while (this.merdusa.state != FlyingMermaidLevelMerdusa.State.Idle)
		{
			yield return null;
		}
		this.merdusa.StartZap();
		while (this.merdusa.state != FlyingMermaidLevelMerdusa.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00066FF4 File Offset: 0x000653F4
	private IEnumerator bubble_cr()
	{
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		this.merdusaHead.StartBubble();
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x00067010 File Offset: 0x00065410
	private IEnumerator head_blast_cr()
	{
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		this.merdusaHead.StartHeadBlast();
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0006702C File Offset: 0x0006542C
	private IEnumerator bubble_head_blast_cr()
	{
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		this.merdusaHead.StartHeadBubble();
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040009E4 RID: 2532
	private LevelProperties.FlyingMermaid properties;

	// Token: 0x040009E5 RID: 2533
	[SerializeField]
	private FlyingMermaidLevelMermaid mermaid;

	// Token: 0x040009E6 RID: 2534
	[Header("FlyingMermaidLevel")]
	[SerializeField]
	private FlyingMermaidLevel.Prefabs prefabs;

	// Token: 0x040009E7 RID: 2535
	[SerializeField]
	private FlyingMermaidLevelMerdusa merdusa;

	// Token: 0x040009E8 RID: 2536
	[SerializeField]
	private FlyingMermaidLevelMerdusaHead merdusaHead;

	// Token: 0x040009EA RID: 2538
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x040009EB RID: 2539
	[SerializeField]
	private Sprite _bossPortraitMerdusa;

	// Token: 0x040009EC RID: 2540
	[SerializeField]
	private Sprite _bossPortraitHead;

	// Token: 0x040009ED RID: 2541
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x040009EE RID: 2542
	[SerializeField]
	private string _bossQuoteMerdusa;

	// Token: 0x040009EF RID: 2543
	[SerializeField]
	private string _bossQuoteHead;

	// Token: 0x0200067F RID: 1663
	[Serializable]
	public class Prefabs
	{
	}
}
