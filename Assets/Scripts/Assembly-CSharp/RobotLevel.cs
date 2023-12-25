using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000264 RID: 612
public class RobotLevel : Level
{
	// Token: 0x060006CF RID: 1743 RVA: 0x00071BD0 File Offset: 0x0006FFD0
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Robot.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00071C66 File Offset: 0x00070066
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Robot;
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00071C6D File Offset: 0x0007006D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_robot;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00071C74 File Offset: 0x00070074
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Robot.States.Main:
			case LevelProperties.Robot.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Robot.States.HeliHead:
				return this._bossPortraitHeliHead;
			case LevelProperties.Robot.States.Inventor:
				return this._bossPortraitInventor;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00071CF4 File Offset: 0x000700F4
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Robot.States.Main:
			case LevelProperties.Robot.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Robot.States.HeliHead:
				return this._bossQuoteHeliHead;
			case LevelProperties.Robot.States.Inventor:
				return this._bossQuoteInventor;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x00071D74 File Offset: 0x00070174
	protected override void Start()
	{
		base.Start();
		this.properties.OnBossDamaged -= base.timeline.DealDamage;
		float[] array = new float[base.timeline.events.Count];
		for (int i = 0; i < base.timeline.events.Count; i++)
		{
			array[i] = base.timeline.events[i].percentage;
		}
		base.timeline = new Level.Timeline();
		base.timeline.health = 0f;
		base.timeline.health += (float)this.properties.CurrentState.hose.health;
		base.timeline.health += (float)this.properties.CurrentState.orb.chestHP;
		base.timeline.health += (float)this.properties.CurrentState.shotBot.hatchGateHealth;
		base.timeline.health += (float)this.properties.CurrentState.heart.heartHP;
		float num = base.timeline.health;
		if (Level.Current.mode != Level.Mode.Easy)
		{
			for (int j = 0; j < array.Length; j++)
			{
				float num2 = this.properties.TotalHealth * ((j >= array.Length - 1) ? array[j] : (array[j] - array[j + 1]));
				Level.Current.timeline.health += num2;
			}
			base.timeline.AddEvent(new Level.Timeline.Event(string.Empty, 1f - num / Level.Current.timeline.health));
			for (int k = 0; k < array.Length; k++)
			{
				num += this.properties.TotalHealth * ((k >= array.Length - 1) ? array[k] : (array[k] - array[k + 1]));
				if (k < array.Length - 1)
				{
					base.timeline.AddEvent(new Level.Timeline.Event(string.Empty, 1f - num / Level.Current.timeline.health));
				}
			}
		}
		this.robot.LevelInit(this.properties);
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x00071FDB File Offset: 0x000703DB
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.robotPattern_cr());
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x00071FEC File Offset: 0x000703EC
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		LevelProperties.Robot.States stateName = this.properties.CurrentState.stateName;
		if (stateName != LevelProperties.Robot.States.HeliHead)
		{
			if (stateName == LevelProperties.Robot.States.Inventor)
			{
				this.heliHead.ChangeState();
			}
		}
		else
		{
			this.StopAllCoroutines();
			this.robot.TriggerPhaseTwo(new Action(this.OnHeliheadSpawn));
		}
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0007205A File Offset: 0x0007045A
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitHeliHead = null;
		this._bossPortraitInventor = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x00072078 File Offset: 0x00070478
	private IEnumerator robotPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x00072094 File Offset: 0x00070494
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Robot.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Robot.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x000720AF File Offset: 0x000704AF
	private void OnHeliheadSpawn()
	{
		base.StartCoroutine(this.spawnHeliHead_cr());
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x000720C0 File Offset: 0x000704C0
	private IEnumerator spawnHeliHead_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2.5f);
		this.robot.animator.SetTrigger("Phase2Transition");
		yield return this.robot.animator.WaitForAnimationToEnd(this, "Death Dance", true, true);
		this.heliHead.GetComponent<RobotLevelHelihead>().InitHeliHead(this.properties);
		yield break;
	}

	// Token: 0x04000D76 RID: 3446
	private LevelProperties.Robot properties;

	// Token: 0x04000D77 RID: 3447
	[SerializeField]
	private RobotLevelRobot robot;

	// Token: 0x04000D78 RID: 3448
	[SerializeField]
	private RobotLevelHelihead heliHead;

	// Token: 0x04000D79 RID: 3449
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000D7A RID: 3450
	[SerializeField]
	private Sprite _bossPortraitHeliHead;

	// Token: 0x04000D7B RID: 3451
	[SerializeField]
	private Sprite _bossPortraitInventor;

	// Token: 0x04000D7C RID: 3452
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000D7D RID: 3453
	[SerializeField]
	private string _bossQuoteHeliHead;

	// Token: 0x04000D7E RID: 3454
	[SerializeField]
	private string _bossQuoteInventor;
}
