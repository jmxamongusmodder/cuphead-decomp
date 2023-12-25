using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002AC RID: 684
public class SlimeLevel : Level
{
	// Token: 0x06000794 RID: 1940 RVA: 0x00075D34 File Offset: 0x00074134
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Slime.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000795 RID: 1941 RVA: 0x00075DCA File Offset: 0x000741CA
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Slime;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x00075DD1 File Offset: 0x000741D1
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_slime;
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000797 RID: 1943 RVA: 0x00075DD8 File Offset: 0x000741D8
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Slime.States.Main:
			case LevelProperties.Slime.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Slime.States.BigSlime:
				return this._bossPortraitBigSlime;
			case LevelProperties.Slime.States.Tombstone:
				return this._bossPortraitTombstone;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000798 RID: 1944 RVA: 0x00075E58 File Offset: 0x00074258
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Slime.States.Main:
			case LevelProperties.Slime.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Slime.States.BigSlime:
				return this._bossQuoteBigSlime;
			case LevelProperties.Slime.States.Tombstone:
				return this._bossQuoteTombstone;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00075ED6 File Offset: 0x000742D6
	protected override void Start()
	{
		base.Start();
		this.smallSlime.LevelInit(this.properties);
		this.bigSlime.LevelInit(this.properties);
		this.tombStone.LevelInit(this.properties);
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00075F11 File Offset: 0x00074311
	protected override void OnLevelStart()
	{
		this.smallSlime.IntroContinue();
		base.StartCoroutine(this.slimePattern_cr());
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00075F2C File Offset: 0x0007432C
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Slime.States.BigSlime)
		{
			this.reachedBigSlimeState = true;
			this.StopAllCoroutines();
			this.smallSlime.Transform();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Slime.States.Tombstone)
		{
			this.StopAllCoroutines();
			this.bigSlime.DeathTransform();
		}
		if (!this.reachedBigSlimeState)
		{
			this.smallSlime.CurrentPropertyState = this.properties.CurrentState;
		}
		this.bigSlime.CurrentPropertyState = this.properties.CurrentState;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x00075FD0 File Offset: 0x000743D0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitBigSlime = null;
		this._bossPortraitMain = null;
		this._bossPortraitTombstone = null;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x00075FF0 File Offset: 0x000743F0
	private IEnumerator slimePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x0007600C File Offset: 0x0007440C
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Slime.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Slime.Pattern.Jump)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000F53 RID: 3923
	private LevelProperties.Slime properties;

	// Token: 0x04000F54 RID: 3924
	[SerializeField]
	private SlimeLevelSlime smallSlime;

	// Token: 0x04000F55 RID: 3925
	[SerializeField]
	private SlimeLevelSlime bigSlime;

	// Token: 0x04000F56 RID: 3926
	[SerializeField]
	private SlimeLevelTombstone tombStone;

	// Token: 0x04000F57 RID: 3927
	private bool reachedBigSlimeState;

	// Token: 0x04000F58 RID: 3928
	private DamageDealer damageDealer;

	// Token: 0x04000F59 RID: 3929
	private DamageReceiver damageReceiver;

	// Token: 0x04000F5A RID: 3930
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000F5B RID: 3931
	[SerializeField]
	private Sprite _bossPortraitBigSlime;

	// Token: 0x04000F5C RID: 3932
	[SerializeField]
	private Sprite _bossPortraitTombstone;

	// Token: 0x04000F5D RID: 3933
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000F5E RID: 3934
	[SerializeField]
	private string _bossQuoteBigSlime;

	// Token: 0x04000F5F RID: 3935
	[SerializeField]
	private string _bossQuoteTombstone;
}
