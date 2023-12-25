using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class AirshipClamLevel : Level
{
	// Token: 0x0600005B RID: 91 RVA: 0x0005400C File Offset: 0x0005240C
	protected override void PartialInit()
	{
		this.properties = LevelProperties.AirshipClam.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600005C RID: 92 RVA: 0x000540A2 File Offset: 0x000524A2
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.AirshipClam;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600005D RID: 93 RVA: 0x000540A9 File Offset: 0x000524A9
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_airship_clam;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600005E RID: 94 RVA: 0x000540AD File Offset: 0x000524AD
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600005F RID: 95 RVA: 0x000540B5 File Offset: 0x000524B5
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000540BD File Offset: 0x000524BD
	protected override void Start()
	{
		base.Start();
		this.clam.LevelInit(this.properties);
	}

	// Token: 0x06000061 RID: 97 RVA: 0x000540D6 File Offset: 0x000524D6
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.airshipclamPattern_cr());
	}

	// Token: 0x06000062 RID: 98 RVA: 0x000540E8 File Offset: 0x000524E8
	private IEnumerator airshipclamPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00054104 File Offset: 0x00052504
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.AirshipClam.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.AirshipClam.Pattern.Spit)
		{
			if (p != LevelProperties.AirshipClam.Pattern.Barnacles)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				base.StartCoroutine(this.barnacles_cr());
			}
		}
		else
		{
			base.StartCoroutine(this.spit_cr());
		}
		yield break;
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00054120 File Offset: 0x00052520
	private IEnumerator spit_cr()
	{
		if (!this.attacking)
		{
			this.clam.OnSpitStart(new Action(this.EndAttack));
			this.attacking = true;
		}
		while (this.attacking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000065 RID: 101 RVA: 0x0005413C File Offset: 0x0005253C
	private IEnumerator barnacles_cr()
	{
		if (!this.attacking)
		{
			this.clam.OnBarnaclesStart(new Action(this.EndAttack));
			this.attacking = true;
		}
		while (this.attacking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00054157 File Offset: 0x00052557
	private void EndAttack()
	{
		this.attacking = false;
	}

	// Token: 0x04000100 RID: 256
	private LevelProperties.AirshipClam properties;

	// Token: 0x04000101 RID: 257
	[SerializeField]
	private AirshipClamLevelClam clam;

	// Token: 0x04000102 RID: 258
	private bool attacking;

	// Token: 0x04000103 RID: 259
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000104 RID: 260
	[SerializeField]
	[Multiline]
	private string _bossQuote;
}
