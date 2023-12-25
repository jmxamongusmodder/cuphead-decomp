using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class BaronessLevel : Level
{
	// Token: 0x060000C0 RID: 192 RVA: 0x00054D38 File Offset: 0x00053138
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Baroness.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x060000C1 RID: 193 RVA: 0x00054DCE File Offset: 0x000531CE
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Baroness;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x060000C2 RID: 194 RVA: 0x00054DD5 File Offset: 0x000531D5
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_baroness;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060000C3 RID: 195 RVA: 0x00054DD9 File Offset: 0x000531D9
	// (set) Token: 0x060000C4 RID: 196 RVA: 0x00054DE0 File Offset: 0x000531E0
	public static List<string> PICKED_BOSSES { get; private set; }

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060000C5 RID: 197 RVA: 0x00054DE8 File Offset: 0x000531E8
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Baroness.States.Main:
			case LevelProperties.Baroness.States.Generic:
				if (this.currentMiniBoss == null)
				{
					return this._bossPortraitChase;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Gumball)
				{
					return this._bossPortraitGumball;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Waffle)
				{
					return this._bossPortraitWaffle;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.CandyCorn)
				{
					return this._bossPortraitCandyCorn;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Cupcake)
				{
					return this._bossPortraitCupcake;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Jawbreaker)
				{
					return this._bossPortraitJawbreaker;
				}
				return this._bossPortraitChase;
			case LevelProperties.Baroness.States.Chase:
				return this._bossPortraitChase;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitChase;
			}
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060000C6 RID: 198 RVA: 0x00054EEC File Offset: 0x000532EC
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Baroness.States.Main:
			case LevelProperties.Baroness.States.Generic:
				if (this.currentMiniBoss == null)
				{
					return this._bossQuoteChase;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Gumball)
				{
					return this._bossQuoteGumball;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Waffle)
				{
					return this._bossQuoteWaffle;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.CandyCorn)
				{
					return this._bossQuoteCandyCorn;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Cupcake)
				{
					return this._bossQuoteCupcake;
				}
				if (this.currentMiniBoss.bossId == BaronessLevelCastle.BossPossibility.Jawbreaker)
				{
					return this._bossQuoteJawbreaker;
				}
				return this._bossQuoteChase;
			case LevelProperties.Baroness.States.Chase:
				return this._bossQuoteChase;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteChase;
			}
		}
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00054FEF File Offset: 0x000533EF
	protected override void Start()
	{
		base.Start();
		this.castle.LevelInit(this.properties);
		this.PickMiniBosses();
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x0005500E File Offset: 0x0005340E
	public void PickMiniBosses()
	{
		base.StartCoroutine(this.pickminibosses_cr());
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00055020 File Offset: 0x00053420
	private IEnumerator update_current_boss_cr()
	{
		while (this.properties.CurrentState.stateName != LevelProperties.Baroness.States.Chase)
		{
			while (BaronessLevelCastle.CURRENT_MINI_BOSS == this.currentMiniBoss && BaronessLevelCastle.CURRENT_MINI_BOSS != null)
			{
				yield return null;
			}
			this.currentMiniBoss = BaronessLevelCastle.CURRENT_MINI_BOSS;
			if (this.currentMiniBoss != null)
			{
				this.currentMiniBoss.OnDamageTakenEvent += base.timeline.DealDamage;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x0005503C File Offset: 0x0005343C
	private IEnumerator pickminibosses_cr()
	{
		LevelProperties.Baroness.Open p = this.properties.CurrentState.open;
		string[] pattern = p.miniBossString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int randIndex = 0;
		List<string> tempList = new List<string>(pattern);
		BaronessLevel.PICKED_BOSSES = new List<string>();
		for (int i = 0; i < p.miniBossAmount; i++)
		{
			randIndex = UnityEngine.Random.Range(0, tempList.ToArray().Length);
			BaronessLevel.PICKED_BOSSES.Add(tempList[randIndex]);
			tempList.Remove(tempList[randIndex]);
		}
		this.SetUpTimeline();
		yield return null;
		yield break;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00055058 File Offset: 0x00053458
	private void SetUpTimeline()
	{
		this.properties.OnBossDamaged -= base.timeline.DealDamage;
		base.timeline = new Level.Timeline();
		base.timeline.health = 0f;
		List<float> list = new List<float>();
		for (int i = 0; i < BaronessLevel.PICKED_BOSSES.Count; i++)
		{
			string text = BaronessLevel.PICKED_BOSSES[i];
			if (text != null)
			{
				if (!(text == "1"))
				{
					if (!(text == "2"))
					{
						if (!(text == "3"))
						{
							if (!(text == "4"))
							{
								if (text == "5")
								{
									base.timeline.health += (float)this.properties.CurrentState.jawbreaker.jawbreakerHomingHP;
									list.Add((float)this.properties.CurrentState.jawbreaker.jawbreakerHomingHP);
								}
							}
							else
							{
								base.timeline.health += (float)this.properties.CurrentState.cupcake.HP;
								list.Add((float)this.properties.CurrentState.cupcake.HP);
							}
						}
						else
						{
							base.timeline.health += (float)this.properties.CurrentState.candyCorn.HP;
							list.Add((float)this.properties.CurrentState.candyCorn.HP);
						}
					}
					else
					{
						base.timeline.health += (float)this.properties.CurrentState.waffle.HP;
						list.Add((float)this.properties.CurrentState.waffle.HP);
					}
				}
				else
				{
					base.timeline.health += (float)this.properties.CurrentState.gumball.HP;
					list.Add((float)this.properties.CurrentState.gumball.HP);
				}
			}
		}
		base.timeline.health += this.properties.CurrentHealth;
		for (int j = 0; j < BaronessLevel.PICKED_BOSSES.Count; j++)
		{
			base.timeline.AddEventAtHealth(BaronessLevel.PICKED_BOSSES[j], base.timeline.GetHealthOfLastEvent() + (int)list[j]);
		}
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		this.castle.StartIntro();
		base.StartCoroutine(this.update_current_boss_cr());
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00055327 File Offset: 0x00053727
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Baroness.States.Chase)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.chase_cr());
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00055358 File Offset: 0x00053758
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.baronessPattern_cr());
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00055367 File Offset: 0x00053767
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitCandyCorn = null;
		this._bossPortraitChase = null;
		this._bossPortraitCupcake = null;
		this._bossPortraitGumball = null;
		this._bossPortraitJawbreaker = null;
		this._bossPortraitWaffle = null;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x0005539C File Offset: 0x0005379C
	private IEnumerator baronessPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x000553B8 File Offset: 0x000537B8
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Baroness.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Baroness.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000553D4 File Offset: 0x000537D4
	private IEnumerator chase_cr()
	{
		this.castle.StartChase();
		yield return null;
		yield break;
	}

	// Token: 0x040001B8 RID: 440
	private LevelProperties.Baroness properties;

	// Token: 0x040001BA RID: 442
	[SerializeField]
	private BaronessLevelCastle castle;

	// Token: 0x040001BB RID: 443
	private BaronessLevelMiniBossBase currentMiniBoss;

	// Token: 0x040001BC RID: 444
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitGumball;

	// Token: 0x040001BD RID: 445
	[SerializeField]
	private Sprite _bossPortraitWaffle;

	// Token: 0x040001BE RID: 446
	[SerializeField]
	private Sprite _bossPortraitCandyCorn;

	// Token: 0x040001BF RID: 447
	[SerializeField]
	private Sprite _bossPortraitCupcake;

	// Token: 0x040001C0 RID: 448
	[SerializeField]
	private Sprite _bossPortraitJawbreaker;

	// Token: 0x040001C1 RID: 449
	[SerializeField]
	private Sprite _bossPortraitChase;

	// Token: 0x040001C2 RID: 450
	[SerializeField]
	private string _bossQuoteGumball;

	// Token: 0x040001C3 RID: 451
	[SerializeField]
	private string _bossQuoteWaffle;

	// Token: 0x040001C4 RID: 452
	[SerializeField]
	private string _bossQuoteCandyCorn;

	// Token: 0x040001C5 RID: 453
	[SerializeField]
	private string _bossQuoteCupcake;

	// Token: 0x040001C6 RID: 454
	[SerializeField]
	private string _bossQuoteJawbreaker;

	// Token: 0x040001C7 RID: 455
	[SerializeField]
	private string _bossQuoteChase;
}
