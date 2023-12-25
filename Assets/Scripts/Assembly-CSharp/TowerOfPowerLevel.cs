using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002D1 RID: 721
public class TowerOfPowerLevel : Level
{
	// Token: 0x060007F9 RID: 2041 RVA: 0x00077CC8 File Offset: 0x000760C8
	protected override void PartialInit()
	{
		this.properties = LevelProperties.TowerOfPower.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x060007FA RID: 2042 RVA: 0x00077D5E File Offset: 0x0007615E
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.TowerOfPower;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x060007FB RID: 2043 RVA: 0x00077D65 File Offset: 0x00076165
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_tower_of_power;
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x060007FC RID: 2044 RVA: 0x00077D69 File Offset: 0x00076169
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x060007FD RID: 2045 RVA: 0x00077D71 File Offset: 0x00076171
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x060007FE RID: 2046 RVA: 0x00077D79 File Offset: 0x00076179
	protected override float LevelIntroTime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x060007FF RID: 2047 RVA: 0x00077D80 File Offset: 0x00076180
	public TowerOfPowerLevelGameManager GameManager
	{
		get
		{
			return this.gameManager;
		}
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00077D88 File Offset: 0x00076188
	protected override void Start()
	{
		base.Start();
		this.gameManager.LevelInit(this.properties);
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (abstractPlayerController != null)
			{
				abstractPlayerController.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00077DE4 File Offset: 0x000761E4
	protected override void Awake()
	{
		base.Awake();
		if (TowerOfPowerLevelGameInfo.GameInfo != null)
		{
			Level.Current.OnLoseEvent += TowerOfPowerLevelGameInfo.GameInfo.CleanUp;
		}
		base.OnLoseEvent += this.ResetScore;
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00077E33 File Offset: 0x00076233
	protected override void OnPlayerJoined(PlayerId playerId)
	{
		TowerOfPowerLevelGameInfo.InitAddedPlayer(playerId, this.properties.CurrentState.slotMachine.DefaultStartingToken);
		base.OnPlayerJoined(playerId);
		TowerOfPowerLevelGameInfo.InitEquipment(playerId);
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x00077E5D File Offset: 0x0007625D
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Level.IsTowerOfPowerMain = false;
		base.OnLoseEvent -= this.ResetScore;
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00077E7D File Offset: 0x0007627D
	private void ResetScore()
	{
		base.OnLoseEvent -= this.ResetScore;
		base.CleanUpScore();
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x00077E97 File Offset: 0x00076297
	protected override void CheckIfInABossesHub()
	{
		base.CheckIfInABossesHub();
		Level.IsTowerOfPower = true;
		Level.IsTowerOfPowerMain = true;
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00077EAB File Offset: 0x000762AB
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.towerofpowerPattern_cr());
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00077EBC File Offset: 0x000762BC
	private IEnumerator towerofpowerPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00077ED8 File Offset: 0x000762D8
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.TowerOfPower.Pattern p = this.properties.CurrentState.NextPattern;
		yield return null;
		yield break;
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00077EF3 File Offset: 0x000762F3
	private void OnGUI()
	{
	}

	// Token: 0x0400104C RID: 4172
	private LevelProperties.TowerOfPower properties;

	// Token: 0x0400104D RID: 4173
	[SerializeField]
	private TowerOfPowerLevelGameManager gameManager;

	// Token: 0x0400104E RID: 4174
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x0400104F RID: 4175
	[SerializeField]
	[Multiline]
	private string _bossQuote;
}
