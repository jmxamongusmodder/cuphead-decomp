using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class MouseLevel : Level
{
	// Token: 0x0600060E RID: 1550 RVA: 0x0006C96C File Offset: 0x0006AD6C
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Mouse.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x0600060F RID: 1551 RVA: 0x0006CA02 File Offset: 0x0006AE02
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Mouse;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000610 RID: 1552 RVA: 0x0006CA09 File Offset: 0x0006AE09
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_mouse;
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000611 RID: 1553 RVA: 0x0006CA10 File Offset: 0x0006AE10
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Mouse.States.Main:
			case LevelProperties.Mouse.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Mouse.States.BrokenCan:
				return this._bossPortraitBrokenCan;
			case LevelProperties.Mouse.States.Cat:
				return this._bossPortraitCat;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000612 RID: 1554 RVA: 0x0006CA90 File Offset: 0x0006AE90
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Mouse.States.Main:
			case LevelProperties.Mouse.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Mouse.States.BrokenCan:
				return this._bossQuoteBrokenCan;
			case LevelProperties.Mouse.States.Cat:
				return this._bossQuoteCat;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x0006CB0E File Offset: 0x0006AF0E
	protected override void Start()
	{
		base.Start();
		this.mouseCan.LevelInit(this.properties);
		this.mouseBrokenCan.LevelInit(this.properties);
		this.cat.LevelInit(this.properties);
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x0006CB49 File Offset: 0x0006AF49
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.mousePattern_cr());
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0006CB58 File Offset: 0x0006AF58
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Mouse.States.BrokenCan)
		{
			this.StopAllCoroutines();
			this.mouseCan.Explode(new Action(this.StartMouseCanPlatform), new Action(this.OnMouseCanTransitionComplete));
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Mouse.States.Cat)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.catIntro_cr());
		}
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x0006CBD8 File Offset: 0x0006AFD8
	private void StartMouseCanPlatform()
	{
		this.wallAnimator.SetTrigger("OnContinue");
		AudioManager.Play("level_mouse_phase2_background_shelf_drop");
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0006CBF4 File Offset: 0x0006AFF4
	private void OnMouseCanTransitionComplete()
	{
		base.StartCoroutine(this.mousePattern_cr());
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0006CC03 File Offset: 0x0006B003
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitBrokenCan = null;
		this._bossPortraitCat = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0006CC20 File Offset: 0x0006B020
	private IEnumerator mousePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.canMove.initialHesitate);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0006CC3C File Offset: 0x0006B03C
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Mouse.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Mouse.Pattern.Dash || this.hasMoved)
		{
			this.hasMoved = true;
			switch (p)
			{
			case LevelProperties.Mouse.Pattern.Dash:
				yield return base.StartCoroutine(this.canDash_cr());
				break;
			case LevelProperties.Mouse.Pattern.CherryBomb:
				yield return base.StartCoroutine(this.cherryBomb_cr());
				break;
			case LevelProperties.Mouse.Pattern.Catapult:
				yield return base.StartCoroutine(this.catapult_cr());
				break;
			case LevelProperties.Mouse.Pattern.RomanCandle:
				yield return base.StartCoroutine(this.romanCandle_cr());
				break;
			default:
				yield return CupheadTime.WaitForSeconds(this, 1f);
				break;
			case LevelProperties.Mouse.Pattern.LeftClaw:
				yield return base.StartCoroutine(this.claw_cr(true));
				break;
			case LevelProperties.Mouse.Pattern.RightClaw:
				yield return base.StartCoroutine(this.claw_cr(false));
				break;
			case LevelProperties.Mouse.Pattern.GhostMouse:
				yield return base.StartCoroutine(this.ghostMouse_cr());
				break;
			}
		}
		yield break;
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0006CC58 File Offset: 0x0006B058
	private IEnumerator canDash_cr()
	{
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		this.mouseCan.StartDash();
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0006CC74 File Offset: 0x0006B074
	private IEnumerator cherryBomb_cr()
	{
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		this.mouseCan.StartCherryBomb();
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0006CC90 File Offset: 0x0006B090
	private IEnumerator catapult_cr()
	{
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		this.mouseCan.StartCatapult();
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0006CCAC File Offset: 0x0006B0AC
	private IEnumerator romanCandle_cr()
	{
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		this.mouseCan.StartRomanCandle();
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0006CCC8 File Offset: 0x0006B0C8
	private IEnumerator claw_cr(bool left)
	{
		while (this.cat.state != MouseLevelCat.State.Idle)
		{
			yield return null;
		}
		this.cat.StartClaw(left);
		while (this.cat.state != MouseLevelCat.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0006CCEC File Offset: 0x0006B0EC
	private IEnumerator ghostMouse_cr()
	{
		while (this.cat.state != MouseLevelCat.State.Idle)
		{
			yield return null;
		}
		this.cat.StartGhostMouse();
		while (this.cat.state != MouseLevelCat.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0006CD08 File Offset: 0x0006B108
	private IEnumerator catIntro_cr()
	{
		this.mouseBrokenCan.Transform();
		while (this.cat.state != MouseLevelCat.State.Idle)
		{
			yield return null;
		}
		base.StartCoroutine(this.mousePattern_cr());
		yield break;
	}

	// Token: 0x04000B37 RID: 2871
	private LevelProperties.Mouse properties;

	// Token: 0x04000B38 RID: 2872
	[SerializeField]
	private MouseLevelCanMouse mouseCan;

	// Token: 0x04000B39 RID: 2873
	[SerializeField]
	private MouseLevelBrokenCanMouse mouseBrokenCan;

	// Token: 0x04000B3A RID: 2874
	[SerializeField]
	private MouseLevelCat cat;

	// Token: 0x04000B3B RID: 2875
	[SerializeField]
	private Animator wallAnimator;

	// Token: 0x04000B3C RID: 2876
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000B3D RID: 2877
	[SerializeField]
	private Sprite _bossPortraitBrokenCan;

	// Token: 0x04000B3E RID: 2878
	[SerializeField]
	private Sprite _bossPortraitCat;

	// Token: 0x04000B3F RID: 2879
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000B40 RID: 2880
	[SerializeField]
	private string _bossQuoteBrokenCan;

	// Token: 0x04000B41 RID: 2881
	[SerializeField]
	private string _bossQuoteCat;

	// Token: 0x04000B42 RID: 2882
	private bool hasMoved;

	// Token: 0x020006DB RID: 1755
	[Serializable]
	public class Prefabs
	{
	}
}
