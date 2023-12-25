using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000AD3 RID: 2771
public class PlayerStatsManager : AbstractPlayerComponent
{
	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x0600428D RID: 17037 RVA: 0x0023DDCD File Offset: 0x0023C1CD
	// (set) Token: 0x0600428E RID: 17038 RVA: 0x0023DDD4 File Offset: 0x0023C1D4
	public static bool GlobalInvincibility { get; private set; }

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x0600428F RID: 17039 RVA: 0x0023DDDC File Offset: 0x0023C1DC
	// (set) Token: 0x06004290 RID: 17040 RVA: 0x0023DDE4 File Offset: 0x0023C1E4
	public int HealthMax { get; private set; }

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06004291 RID: 17041 RVA: 0x0023DDED File Offset: 0x0023C1ED
	// (set) Token: 0x06004292 RID: 17042 RVA: 0x0023DDF5 File Offset: 0x0023C1F5
	public int Health { get; private set; }

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06004293 RID: 17043 RVA: 0x0023DDFE File Offset: 0x0023C1FE
	// (set) Token: 0x06004294 RID: 17044 RVA: 0x0023DE06 File Offset: 0x0023C206
	public int HealerHP { get; private set; }

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x06004295 RID: 17045 RVA: 0x0023DE0F File Offset: 0x0023C20F
	// (set) Token: 0x06004296 RID: 17046 RVA: 0x0023DE17 File Offset: 0x0023C217
	public int HealerHPReceived { get; private set; }

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x06004297 RID: 17047 RVA: 0x0023DE20 File Offset: 0x0023C220
	// (set) Token: 0x06004298 RID: 17048 RVA: 0x0023DE28 File Offset: 0x0023C228
	public int HealerHPCounter { get; private set; }

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x06004299 RID: 17049 RVA: 0x0023DE31 File Offset: 0x0023C231
	// (set) Token: 0x0600429A RID: 17050 RVA: 0x0023DE39 File Offset: 0x0023C239
	public int CurseCharmLevel
	{
		get
		{
			return this._curseCharmLevel;
		}
		private set
		{
			this._curseCharmLevel = value;
		}
	}

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x0600429B RID: 17051 RVA: 0x0023DE42 File Offset: 0x0023C242
	public bool CurseSmokeDash
	{
		get
		{
			return this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel >= 0 && this.curseCharmDashCounter == 0;
		}
	}

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x0600429C RID: 17052 RVA: 0x0023DE71 File Offset: 0x0023C271
	public bool CurseWhetsone
	{
		get
		{
			return this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel >= 0 && this.curseCharmWhetstoneCounter == 0;
		}
	}

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x0600429D RID: 17053 RVA: 0x0023DEA0 File Offset: 0x0023C2A0
	// (set) Token: 0x0600429E RID: 17054 RVA: 0x0023DEA8 File Offset: 0x0023C2A8
	public float SuperMeterMax { get; private set; }

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x0600429F RID: 17055 RVA: 0x0023DEB1 File Offset: 0x0023C2B1
	// (set) Token: 0x060042A0 RID: 17056 RVA: 0x0023DEB9 File Offset: 0x0023C2B9
	public float SuperMeter { get; private set; }

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x060042A1 RID: 17057 RVA: 0x0023DEC2 File Offset: 0x0023C2C2
	// (set) Token: 0x060042A2 RID: 17058 RVA: 0x0023DECA File Offset: 0x0023C2CA
	public bool SuperInvincible { get; private set; }

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x060042A3 RID: 17059 RVA: 0x0023DED3 File Offset: 0x0023C2D3
	// (set) Token: 0x060042A4 RID: 17060 RVA: 0x0023DEDB File Offset: 0x0023C2DB
	public bool ChaliceShieldOn { get; private set; }

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x060042A5 RID: 17061 RVA: 0x0023DEE4 File Offset: 0x0023C2E4
	public bool CanGainSuperMeter
	{
		get
		{
			return this.Loadout.charm != Charm.charm_EX && (!this.SuperInvincible || this.ChaliceShieldOn);
		}
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x060042A6 RID: 17062 RVA: 0x0023DF12 File Offset: 0x0023C312
	// (set) Token: 0x060042A7 RID: 17063 RVA: 0x0023DF1A File Offset: 0x0023C31A
	public float ExCost { get; private set; }

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x060042A8 RID: 17064 RVA: 0x0023DF23 File Offset: 0x0023C323
	// (set) Token: 0x060042A9 RID: 17065 RVA: 0x0023DF2B File Offset: 0x0023C32B
	public int Deaths { get; private set; }

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x060042AA RID: 17066 RVA: 0x0023DF34 File Offset: 0x0023C334
	// (set) Token: 0x060042AB RID: 17067 RVA: 0x0023DF3C File Offset: 0x0023C33C
	public int ParriesThisJump { get; private set; }

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x060042AC RID: 17068 RVA: 0x0023DF45 File Offset: 0x0023C345
	// (set) Token: 0x060042AD RID: 17069 RVA: 0x0023DF4D File Offset: 0x0023C34D
	public float StoneTime { get; private set; }

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x060042AE RID: 17070 RVA: 0x0023DF56 File Offset: 0x0023C356
	// (set) Token: 0x060042AF RID: 17071 RVA: 0x0023DF5E File Offset: 0x0023C35E
	public float ReverseTime { get; private set; }

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x060042B0 RID: 17072 RVA: 0x0023DF67 File Offset: 0x0023C367
	public bool CanUseEx
	{
		get
		{
			return this.Loadout.charm == Charm.charm_EX || (this.SuperMeter >= this.ExCost && this.CanGainSuperMeter);
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x060042B1 RID: 17073 RVA: 0x0023DF9B File Offset: 0x0023C39B
	// (set) Token: 0x060042B2 RID: 17074 RVA: 0x0023DFA3 File Offset: 0x0023C3A3
	public PlayerData.PlayerLoadouts.PlayerLoadout Loadout { get; private set; }

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x060042B3 RID: 17075 RVA: 0x0023DFAC File Offset: 0x0023C3AC
	// (set) Token: 0x060042B4 RID: 17076 RVA: 0x0023DFB4 File Offset: 0x0023C3B4
	public PlayerStatsManager.PlayerState State { get; private set; }

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x060042B5 RID: 17077 RVA: 0x0023DFBD File Offset: 0x0023C3BD
	// (set) Token: 0x060042B6 RID: 17078 RVA: 0x0023DFC5 File Offset: 0x0023C3C5
	public bool DiceGameBonusHP { get; set; }

	// Token: 0x140000AD RID: 173
	// (add) Token: 0x060042B7 RID: 17079 RVA: 0x0023DFD0 File Offset: 0x0023C3D0
	// (remove) Token: 0x060042B8 RID: 17080 RVA: 0x0023E008 File Offset: 0x0023C408
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PlayerStatsManager.OnPlayerHealthChangeHandler OnHealthChangedEvent;

	// Token: 0x140000AE RID: 174
	// (add) Token: 0x060042B9 RID: 17081 RVA: 0x0023E040 File Offset: 0x0023C440
	// (remove) Token: 0x060042BA RID: 17082 RVA: 0x0023E078 File Offset: 0x0023C478
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PlayerStatsManager.OnPlayerSuperChangedHandler OnSuperChangedEvent;

	// Token: 0x140000AF RID: 175
	// (add) Token: 0x060042BB RID: 17083 RVA: 0x0023E0B0 File Offset: 0x0023C4B0
	// (remove) Token: 0x060042BC RID: 17084 RVA: 0x0023E0E8 File Offset: 0x0023C4E8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PlayerStatsManager.OnPlayerWeaponChangedHandler OnWeaponChangedEvent;

	// Token: 0x140000B0 RID: 176
	// (add) Token: 0x060042BD RID: 17085 RVA: 0x0023E120 File Offset: 0x0023C520
	// (remove) Token: 0x060042BE RID: 17086 RVA: 0x0023E158 File Offset: 0x0023C558
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PlayerStatsManager.OnPlayerDeathHandler OnPlayerDeathEvent;

	// Token: 0x140000B1 RID: 177
	// (add) Token: 0x060042BF RID: 17087 RVA: 0x0023E190 File Offset: 0x0023C590
	// (remove) Token: 0x060042C0 RID: 17088 RVA: 0x0023E1C8 File Offset: 0x0023C5C8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PlayerStatsManager.OnPlayerDeathHandler OnPlayerReviveEvent;

	// Token: 0x140000B2 RID: 178
	// (add) Token: 0x060042C1 RID: 17089 RVA: 0x0023E200 File Offset: 0x0023C600
	// (remove) Token: 0x060042C2 RID: 17090 RVA: 0x0023E238 File Offset: 0x0023C638
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PlayerStatsManager.OnStoneHandler OnStoneShake;

	// Token: 0x140000B3 RID: 179
	// (add) Token: 0x060042C3 RID: 17091 RVA: 0x0023E270 File Offset: 0x0023C670
	// (remove) Token: 0x060042C4 RID: 17092 RVA: 0x0023E2A8 File Offset: 0x0023C6A8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PlayerStatsManager.OnStoneHandler OnStoned;

	// Token: 0x060042C5 RID: 17093 RVA: 0x0023E2E0 File Offset: 0x0023C6E0
	protected override void OnAwake()
	{
		base.OnAwake();
		PlayerStatsManager.GlobalInvincibility = false;
		PlayerStatsManager.DebugInvincible = false;
		this.SuperInvincible = false;
		this.ChaliceShieldOn = false;
		base.basePlayer.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		LevelPlayerController levelPlayerController = base.basePlayer as LevelPlayerController;
		PlanePlayerController planePlayerController = base.basePlayer as PlanePlayerController;
		if (levelPlayerController != null)
		{
			levelPlayerController.motor.OnDashStartEvent += this.onDashStartEventHandler;
			levelPlayerController.motor.OnParryEvent += this.onParryEventHandler;
		}
		else if (planePlayerController != null)
		{
			planePlayerController.animationController.OnShrinkEvent += this.onShrinkEventHandler;
			planePlayerController.parryController.OnParryStartEvent += this.onParryEventHandler;
		}
		LevelPlayerWeaponManager component = base.GetComponent<LevelPlayerWeaponManager>();
		if (component != null)
		{
			component.OnWeaponChangeEvent += this.OnWeaponChange;
			component.OnSuperEnd += this.OnSuperEnd;
		}
		PlanePlayerWeaponManager component2 = base.GetComponent<PlanePlayerWeaponManager>();
		if (component2 != null)
		{
			component2.OnWeaponChangeEvent += this.OnWeaponChange;
		}
		this.Deaths = 0;
		this.hardInvincibility = false;
	}

	// Token: 0x060042C6 RID: 17094 RVA: 0x0023E424 File Offset: 0x0023C824
	private void OnEnable()
	{
		if (this.superBuilderRoutine != null)
		{
			base.StopCoroutine(this.superBuilderRoutine);
		}
		this.superBuilderRoutine = this.charmSuperBuilder_cr();
		base.StartCoroutine(this.superBuilderRoutine);
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x0023E456 File Offset: 0x0023C856
	private void FixedUpdate()
	{
		this.UpdateStone();
		this.UpdateReverse();
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x0023E464 File Offset: 0x0023C864
	public void LevelInit()
	{
		Level.Current.OnWinEvent += this.OnWin;
		Level.Current.OnLoseEvent += this.OnLose;
		this.Loadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.basePlayer.id);
		this.isChalice = (this.Loadout.charm == Charm.charm_chalice && !Level.Current.BlockChaliceCharm[(int)base.basePlayer.id]);
		if (!Level.Current.blockChalice && (PlayerManager.playerWasChalice[0] || PlayerManager.playerWasChalice[1]))
		{
			this.isChalice = PlayerManager.playerWasChalice[(int)base.basePlayer.id];
		}
		if (Level.IsDicePalace && !DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY)
		{
			this.isChalice = (base.basePlayer.id == (PlayerId)DicePalaceMainLevelGameInfo.CHALICE_PLAYER);
		}
		if (this.Loadout.charm == Charm.charm_curse)
		{
			this.CurseCharmLevel = CharmCurse.CalculateLevel(base.basePlayer.id);
		}
		this.ExCost = 10f;
		this.SuperMeterMax = 50f;
		this.CalculateHealthMax();
		PlayersStatsBossesHub playerStats = Level.GetPlayerStats(base.basePlayer.id);
		if (Level.IsInBossesHub && playerStats != null)
		{
			this.Health = playerStats.HP;
			this.SuperMeter = playerStats.SuperCharge;
			this.HealerHP = playerStats.healerHP;
			this.HealerHPReceived = playerStats.healerHPReceived;
			this.HealerHPCounter = playerStats.healerHPCounter;
		}
		else
		{
			this.Health = this.HealthMax;
			this.SuperMeter = 0f;
		}
		if (this.Health >= OnlineAchievementData.DLC.Triggers.HP9Trigger)
		{
			OnlineManager.Instance.Interface.UnlockAchievement(base.basePlayer.id, OnlineAchievementData.DLC.HP9);
		}
		this.UpdateHealerStats();
		if (this.isChalice)
		{
			if (this.Loadout.super == Super.level_super_beam)
			{
				this.Loadout.super = Super.level_super_chalice_vert_beam;
			}
			else if (this.Loadout.super == Super.level_super_invincible)
			{
				this.Loadout.super = Super.level_super_chalice_shield;
			}
			else if (this.Loadout.super == Super.level_super_ghost)
			{
				this.Loadout.super = Super.level_super_chalice_iii;
			}
		}
		else if (this.Loadout.super == Super.level_super_chalice_vert_beam)
		{
			this.Loadout.super = Super.level_super_beam;
		}
		else if (this.Loadout.super == Super.level_super_chalice_shield)
		{
			this.Loadout.super = Super.level_super_invincible;
		}
		else if (this.Loadout.super == Super.level_super_chalice_iii)
		{
			this.Loadout.super = Super.level_super_ghost;
		}
	}

	// Token: 0x060042C9 RID: 17097 RVA: 0x0023E758 File Offset: 0x0023CB58
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Level.Current != null)
		{
			Level.Current.OnWinEvent -= this.OnWin;
			Level.Current.OnLoseEvent -= this.OnLose;
		}
		if (this.Loadout != null)
		{
			if (this.Loadout.super == Super.level_super_chalice_vert_beam)
			{
				this.Loadout.super = Super.level_super_beam;
			}
			else if (this.Loadout.super == Super.level_super_chalice_shield)
			{
				this.Loadout.super = Super.level_super_invincible;
			}
			else if (this.Loadout.super == Super.level_super_chalice_iii)
			{
				this.Loadout.super = Super.level_super_ghost;
			}
		}
	}

	// Token: 0x060042CA RID: 17098 RVA: 0x0023E82C File Offset: 0x0023CC2C
	public void UpdateHealerStats()
	{
		if ((this.Loadout.charm == Charm.charm_healer || (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel >= 0)) && !Level.IsChessBoss)
		{
			PlayersStatsBossesHub playerStats = Level.GetPlayerStats(base.basePlayer.id);
			if (playerStats != null)
			{
				this.HealthMax += playerStats.healerHP;
			}
		}
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x0023E8A3 File Offset: 0x0023CCA3
	private bool DjimmiInUse()
	{
		return PlayerData.Data.DjimmiActivatedCurrentRegion() && Level.Current.AllowDjimmi() && Level.Current.mode != Level.Mode.Hard;
	}

	// Token: 0x060042CC RID: 17100 RVA: 0x0023E8D8 File Offset: 0x0023CCD8
	private void CalculateHealthMax()
	{
		this.HealthMax = 3;
		if (this.Loadout.charm == Charm.charm_health_up_1 && !Level.IsChessBoss)
		{
			this.HealthMax += WeaponProperties.CharmHealthUpOne.healthIncrease;
		}
		else if (this.Loadout.charm == Charm.charm_health_up_2 && !Level.IsChessBoss)
		{
			this.HealthMax += WeaponProperties.CharmHealthUpTwo.healthIncrease;
		}
		else if (this.Loadout.charm == Charm.charm_healer && !Level.IsChessBoss)
		{
			this.HealthMax += this.HealerHP;
		}
		else if (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel >= 0 && !Level.IsChessBoss)
		{
			this.HealthMax += this.HealerHP;
			this.HealthMax += CharmCurse.GetHealthModifier(this.CurseCharmLevel);
		}
		else if (this.isChalice)
		{
			this.HealthMax++;
		}
		if (this.DjimmiInUse())
		{
			this.HealthMax *= 2;
		}
		if (Level.IsInBossesHub)
		{
			PlayersStatsBossesHub playerStats = Level.GetPlayerStats(base.basePlayer.id);
			if (playerStats != null)
			{
				this.HealthMax += playerStats.BonusHP;
			}
		}
		if (this.HealthMax > 9)
		{
			this.HealthMax = 9;
		}
	}

	// Token: 0x060042CD RID: 17101 RVA: 0x0023EA65 File Offset: 0x0023CE65
	private void OnWin()
	{
		PlayerStatsManager.GlobalInvincibility = true;
	}

	// Token: 0x060042CE RID: 17102 RVA: 0x0023EA6D File Offset: 0x0023CE6D
	private void OnLose()
	{
		PlayerStatsManager.GlobalInvincibility = true;
	}

	// Token: 0x060042CF RID: 17103 RVA: 0x0023EA78 File Offset: 0x0023CE78
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.SuperInvincible)
		{
			return;
		}
		if (this.ChaliceShieldOn)
		{
			return;
		}
		if (this.Loadout.charm == Charm.charm_pit_saver && !Level.IsChessBoss)
		{
			if (info.damageSource == DamageDealer.DamageSource.Pit)
			{
				return;
			}
			this.SuperMeter += WeaponProperties.CharmPitSaver.meterAmount;
			this.OnSuperChanged(true);
		}
		if (info.stoneTime > 0f)
		{
			this.GetStoned(info.stoneTime);
		}
		if (info.damage > 0f)
		{
			this.TakeDamage();
		}
	}

	// Token: 0x060042D0 RID: 17104 RVA: 0x0023EB14 File Offset: 0x0023CF14
	public void GetStoned(float time)
	{
		if (time > 0f && this.StoneTime <= 0f && this.timeSinceStoned > 1f)
		{
			this.StoneTime = time;
			this.timeSinceStoned = 0f;
			this.OnStoned();
		}
	}

	// Token: 0x060042D1 RID: 17105 RVA: 0x0023EB6C File Offset: 0x0023CF6C
	public void ReverseControls(float reverseTime)
	{
		if (this.timeSinceReversed > 0f && this.ReverseTime <= 0f && this.timeSinceReversed > 1f)
		{
			this.ReverseTime = reverseTime;
			this.timeSinceReversed = 0f;
		}
	}

	// Token: 0x060042D2 RID: 17106 RVA: 0x0023EBBC File Offset: 0x0023CFBC
	private void TakeDamage()
	{
		if (this.SuperInvincible)
		{
			return;
		}
		if (this.hardInvincibility)
		{
			return;
		}
		if (Level.Current.Ending)
		{
			return;
		}
		if (this.State != PlayerStatsManager.PlayerState.Ready && (!this.isChalice || this.Loadout.super != Super.level_super_ghost))
		{
			return;
		}
		if (this.StoneTime > 0f)
		{
			this.StoneTime = 0f;
		}
		if (PlayerStatsManager.GlobalInvincibility || PlayerStatsManager.DebugInvincible)
		{
			return;
		}
		this.Health--;
		PlayersStatsBossesHub playerStats = Level.GetPlayerStats(base.basePlayer.id);
		if (Level.IsInBossesHub && playerStats != null)
		{
			if (playerStats.BonusHP > 0)
			{
				playerStats.LoseBonusHP();
			}
			else if (playerStats.healerHP > 0)
			{
				playerStats.LoseHealerHP();
			}
			this.CalculateHealthMax();
		}
		this.OnHealthChanged();
		if (this.Health < 3)
		{
			Level.ScoringData.numTimesHit++;
		}
		Vibrator.Vibrate(1f, 0.2f, base.basePlayer.id);
		if (this.Health <= 0)
		{
			this.OnStatsDeath();
		}
		else
		{
			base.StartCoroutine(this.hit_cr());
		}
	}

	// Token: 0x060042D3 RID: 17107 RVA: 0x0023ED0D File Offset: 0x0023D10D
	public void OnPitKnockUp()
	{
		base.basePlayer.damageReceiver.TakeDamage(new DamageDealer.DamageInfo(1f, DamageDealer.Direction.Neutral, base.transform.position, DamageDealer.DamageSource.Pit));
	}

	// Token: 0x060042D4 RID: 17108 RVA: 0x0023ED3B File Offset: 0x0023D13B
	public void OnDealDamage(float damage, DamageDealer dealer)
	{
		if (this.CanGainSuperMeter)
		{
			this.SuperMeter += 0.0625f * damage / dealer.DamageMultiplier;
			this.OnSuperChanged(false);
		}
	}

	// Token: 0x060042D5 RID: 17109 RVA: 0x0023ED6C File Offset: 0x0023D16C
	public void OnParry(float multiplier = 1f, bool countParryTowardsScore = true)
	{
		if ((this.Loadout.charm == Charm.charm_healer || (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel >= 0)) && !Level.IsChessBoss)
		{
			if (this.HealerHPReceived < 3)
			{
				this.HealerCharm();
			}
			else
			{
				this.SuperChangedFromParry(multiplier);
			}
		}
		else
		{
			this.SuperChangedFromParry(multiplier);
		}
		if (countParryTowardsScore && !Level.Current.Ending)
		{
			Level.ScoringData.numParries++;
		}
		OnlineManager.Instance.Interface.IncrementStat(base.basePlayer.id, "Parries", 1);
		if (Level.Current.CurrentLevel != Levels.Tutorial && Level.Current.CurrentLevel != Levels.ShmupTutorial && (Level.Current.playerMode == PlayerMode.Level || Level.Current.playerMode == PlayerMode.Arcade))
		{
			this.ParriesThisJump++;
			if (this.ParriesThisJump > PlayerData.Data.GetNumParriesInRow(base.basePlayer.id))
			{
				PlayerData.Data.SetNumParriesInRow(base.basePlayer.id, this.ParriesThisJump);
			}
			if (this.ParriesThisJump == 5)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(base.basePlayer.id, "ParryChain");
			}
		}
		if (this.SuperMeter == this.SuperMeterMax)
		{
			AudioManager.Play("player_parry_power_up_full");
		}
		else
		{
			AudioManager.Play("player_parry_power_up");
		}
	}

	// Token: 0x060042D6 RID: 17110 RVA: 0x0023EF0A File Offset: 0x0023D30A
	private void SuperChangedFromParry(float multiplier)
	{
		if (this.CanGainSuperMeter)
		{
			this.SuperMeter += 10f * multiplier;
			this.OnSuperChanged(true);
		}
	}

	// Token: 0x060042D7 RID: 17111 RVA: 0x0023EF32 File Offset: 0x0023D332
	public bool NextParryActivatesHealerCharm()
	{
		return !Level.IsChessBoss && this.Loadout.charm == Charm.charm_healer && this.HealerHPReceived == this.HealerHPCounter;
	}

	// Token: 0x060042D8 RID: 17112 RVA: 0x0023EF68 File Offset: 0x0023D368
	private void HealerCharm()
	{
		int num = this.HealerHPReceived + 1;
		if (this.Loadout.charm == Charm.charm_curse)
		{
			num = CharmCurse.GetHealerInterval(this.CurseCharmLevel, this.HealerHPReceived);
		}
		this.HealerHPCounter++;
		if (this.HealerHPCounter >= num)
		{
			this.HealerHP++;
			this.HealerHPReceived++;
			this.SetHealth(this.Health + 1);
			this.OnHealthChanged();
			this.HealerHPCounter = 0;
			LevelPlayerController levelPlayerController = base.basePlayer as LevelPlayerController;
			PlanePlayerController planePlayerController = base.basePlayer as PlanePlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.animationController.OnHealerCharm();
			}
			else if (planePlayerController != null)
			{
				planePlayerController.animationController.OnHealerCharm();
			}
		}
	}

	// Token: 0x060042D9 RID: 17113 RVA: 0x0023F041 File Offset: 0x0023D441
	public void ParryOneQuarter()
	{
		this.OnParry(0.25f, true);
	}

	// Token: 0x060042DA RID: 17114 RVA: 0x0023F04F File Offset: 0x0023D44F
	public void ResetJumpParries()
	{
		this.ParriesThisJump = 0;
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x060042DB RID: 17115 RVA: 0x0023F058 File Offset: 0x0023D458
	public bool PartnerCanSteal
	{
		get
		{
			return this.Health > 1;
		}
	}

	// Token: 0x060042DC RID: 17116 RVA: 0x0023F064 File Offset: 0x0023D464
	public void OnPartnerStealHealth()
	{
		if (!this.PartnerCanSteal)
		{
			return;
		}
		this.Health--;
		PlayersStatsBossesHub playerStats = Level.GetPlayerStats(base.basePlayer.id);
		if (Level.IsInBossesHub && playerStats != null)
		{
			playerStats.LoseBonusHP();
			this.CalculateHealthMax();
		}
		this.OnHealthChanged();
	}

	// Token: 0x060042DD RID: 17117 RVA: 0x0023F0BE File Offset: 0x0023D4BE
	public void OnSuper()
	{
		if (this.Loadout.super != Super.level_super_invincible || Level.Current.playerMode != PlayerMode.Level)
		{
			this.SuperMeter = 0f;
			this.OnSuperChanged(true);
		}
		this.State = PlayerStatsManager.PlayerState.Super;
	}

	// Token: 0x060042DE RID: 17118 RVA: 0x0023F0FD File Offset: 0x0023D4FD
	public void OnSuperEnd()
	{
		if (this.Loadout.super == Super.level_super_invincible && Level.Current.playerMode == PlayerMode.Level)
		{
			base.StartCoroutine(this.emptySuper_cr());
		}
		this.State = PlayerStatsManager.PlayerState.Ready;
	}

	// Token: 0x060042DF RID: 17119 RVA: 0x0023F137 File Offset: 0x0023D537
	public void OnEx()
	{
		if (this.Loadout.charm == Charm.charm_EX)
		{
			return;
		}
		this.SuperMeter -= 10f;
		this.OnSuperChanged(true);
	}

	// Token: 0x060042E0 RID: 17120 RVA: 0x0023F168 File Offset: 0x0023D568
	private void OnWeaponChange(Weapon weapon)
	{
		if (this.OnWeaponChangedEvent != null)
		{
			this.OnWeaponChangedEvent(weapon);
		}
	}

	// Token: 0x060042E1 RID: 17121 RVA: 0x0023F184 File Offset: 0x0023D584
	private void OnHealthChanged()
	{
		this.Health = Mathf.Clamp(this.Health, 0, this.HealthMax);
		if (this.OnHealthChangedEvent != null)
		{
			this.OnHealthChangedEvent(this.Health, base.basePlayer.id);
		}
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x0023F1D0 File Offset: 0x0023D5D0
	private void OnSuperChanged(bool playEffect = true)
	{
		this.SuperMeter = Mathf.Clamp(this.SuperMeter, 0f, this.SuperMeterMax);
		if (this.OnSuperChangedEvent != null)
		{
			this.OnSuperChangedEvent(this.SuperMeter, base.basePlayer.id, playEffect);
		}
	}

	// Token: 0x060042E3 RID: 17123 RVA: 0x0023F224 File Offset: 0x0023D624
	private void OnStatsDeath()
	{
		AudioManager.Play("player_die");
		base.StartCoroutine(this.death_sound_cr());
		if (this.OnPlayerDeathEvent != null)
		{
			this.OnPlayerDeathEvent(base.basePlayer.id);
		}
		EventManager.Instance.Raise(PlayerEvent<PlayerStatsManager.DeathEvent>.Shared(base.basePlayer.id));
		this.Deaths++;
		PlayerData.Data.Die(base.basePlayer.id);
	}

	// Token: 0x060042E4 RID: 17124 RVA: 0x0023F2A6 File Offset: 0x0023D6A6
	public void OnPreRevive()
	{
		if (!Level.IsTowerOfPowerMain || TowerOfPowerLevelGameInfo.CURRENT_TURN > 0)
		{
			this.Health = 1;
		}
	}

	// Token: 0x060042E5 RID: 17125 RVA: 0x0023F2C4 File Offset: 0x0023D6C4
	public void OnRevive()
	{
		this.OnHealthChanged();
		if (this.OnPlayerReviveEvent != null)
		{
			this.OnPlayerReviveEvent(base.basePlayer.id);
		}
		EventManager.Instance.Raise(PlayerEvent<PlayerStatsManager.ReviveEvent>.Shared(base.basePlayer.id));
	}

	// Token: 0x060042E6 RID: 17126 RVA: 0x0023F312 File Offset: 0x0023D712
	public void SetHealth(int health)
	{
		this.Health = health;
		this.CalculateHealthMax();
		this.OnHealthChanged();
		if (health >= OnlineAchievementData.DLC.Triggers.HP9Trigger)
		{
			OnlineManager.Instance.Interface.UnlockAchievement(base.basePlayer.id, OnlineAchievementData.DLC.HP9);
		}
	}

	// Token: 0x060042E7 RID: 17127 RVA: 0x0023F351 File Offset: 0x0023D751
	public void SetInvincible(bool superInvincible)
	{
		this.SuperInvincible = superInvincible;
	}

	// Token: 0x060042E8 RID: 17128 RVA: 0x0023F35A File Offset: 0x0023D75A
	public void SetChaliceShield(bool chaliceShield)
	{
		this.ChaliceShieldOn = chaliceShield;
	}

	// Token: 0x060042E9 RID: 17129 RVA: 0x0023F363 File Offset: 0x0023D763
	public void AddEx()
	{
		if (this.CanGainSuperMeter)
		{
			this.SuperMeter += 10f;
			this.OnSuperChanged(true);
		}
	}

	// Token: 0x060042EA RID: 17130 RVA: 0x0023F38C File Offset: 0x0023D78C
	private void onDashStartEventHandler()
	{
		if (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel > -1)
		{
			this.curseCharmDashCounter++;
			if (this.curseCharmDashCounter > CharmCurse.GetSmokeDashInterval(this.CurseCharmLevel))
			{
				this.curseCharmDashCounter = 0;
			}
		}
	}

	// Token: 0x060042EB RID: 17131 RVA: 0x0023F3E8 File Offset: 0x0023D7E8
	private void onShrinkEventHandler()
	{
		if (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel > -1)
		{
			this.curseCharmDashCounter++;
			if (this.curseCharmDashCounter > CharmCurse.GetSmokeDashInterval(this.CurseCharmLevel))
			{
				this.curseCharmDashCounter = 0;
			}
		}
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x0023F444 File Offset: 0x0023D844
	private void onParryEventHandler()
	{
		if (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel > -1)
		{
			this.curseCharmWhetstoneCounter++;
			if (this.curseCharmWhetstoneCounter > CharmCurse.GetWhetstoneInterval(this.CurseCharmLevel))
			{
				this.curseCharmWhetstoneCounter = 0;
			}
		}
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x0023F4A0 File Offset: 0x0023D8A0
	private void UpdateStone()
	{
		PlanePlayerController planePlayerController = base.basePlayer as PlanePlayerController;
		if (planePlayerController != null)
		{
			this.currentMoveDir = planePlayerController.motor.MoveDirection;
		}
		this.lastMoveDir = this.currentMoveDir;
		this.timeSinceStoned += CupheadTime.FixedDelta;
		if (this.StoneTime <= 0f)
		{
			return;
		}
		if ((this.lastMoveDir != this.currentMoveDir && (this.currentMoveDir.x != 0 || this.currentMoveDir.y != 0)) || base.basePlayer.input.actions.GetAnyButtonDown())
		{
			this.StoneTime -= CupheadTime.Delta;
			this.StoneTime -= 0.1f;
			this.OnStoneShake();
		}
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x0023F594 File Offset: 0x0023D994
	private void UpdateReverse()
	{
		LevelPlayerController x = base.basePlayer as LevelPlayerController;
		if (x == null)
		{
			return;
		}
		this.timeSinceReversed += CupheadTime.FixedDelta;
		if (this.ReverseTime <= 0f)
		{
			return;
		}
		this.ReverseTime -= CupheadTime.Delta;
	}

	// Token: 0x060042EF RID: 17135 RVA: 0x0023F5F4 File Offset: 0x0023D9F4
	public override void StopAllCoroutines()
	{
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x0023F5F8 File Offset: 0x0023D9F8
	private IEnumerator death_sound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("player_die_vinylscratch");
		yield break;
	}

	// Token: 0x060042F1 RID: 17137 RVA: 0x0023F614 File Offset: 0x0023DA14
	private IEnumerator hit_cr()
	{
		this.hardInvincibility = true;
		for (int i = 0; i < 10; i++)
		{
			yield return null;
		}
		this.hardInvincibility = false;
		yield break;
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x0023F630 File Offset: 0x0023DA30
	private IEnumerator charmSuperBuilder_cr()
	{
		while (this.Loadout == null)
		{
			yield return null;
		}
		if ((this.Loadout.charm != Charm.charm_super_builder && (this.Loadout.charm != Charm.charm_curse || this.CurseCharmLevel <= -1)) || Level.IsChessBoss)
		{
			yield break;
		}
		float delay = 0f;
		float amount = 0f;
		if (this.Loadout.charm == Charm.charm_super_builder)
		{
			delay = WeaponProperties.CharmSuperBuilder.delay;
			amount = WeaponProperties.CharmSuperBuilder.amount;
		}
		else if (this.Loadout.charm == Charm.charm_curse && this.CurseCharmLevel > -1)
		{
			delay = WeaponProperties.CharmCurse.superMeterDelay;
			amount = CharmCurse.GetSuperMeterAmount(this.CurseCharmLevel);
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			if (this.CanGainSuperMeter)
			{
				this.SuperMeter += amount;
				this.OnSuperChanged(false);
			}
		}
	}

	// Token: 0x060042F3 RID: 17139 RVA: 0x0023F64C File Offset: 0x0023DA4C
	private IEnumerator emptySuper_cr()
	{
		while (this.SuperMeter > 0f)
		{
			this.SuperMeter -= this.SuperMeterMax * CupheadTime.Delta / WeaponProperties.LevelSuperInvincibility.durationFX;
			this.OnSuperChanged(true);
			yield return null;
		}
		this.SuperMeter = 0f;
		this.OnSuperChanged(true);
		yield break;
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x060042F4 RID: 17140 RVA: 0x0023F667 File Offset: 0x0023DA67
	// (set) Token: 0x060042F5 RID: 17141 RVA: 0x0023F66E File Offset: 0x0023DA6E
	public static bool DebugInvincible { get; private set; }

	// Token: 0x060042F6 RID: 17142 RVA: 0x0023F676 File Offset: 0x0023DA76
	public void DebugAddSuper()
	{
		this.AddEx();
	}

	// Token: 0x060042F7 RID: 17143 RVA: 0x0023F67E File Offset: 0x0023DA7E
	public void DebugFillSuper()
	{
		this.SuperMeter = 50f;
		this.OnSuperChanged(true);
	}

	// Token: 0x060042F8 RID: 17144 RVA: 0x0023F694 File Offset: 0x0023DA94
	public static void DebugToggleInvincible()
	{
		PlayerStatsManager.DebugInvincible = !PlayerStatsManager.DebugInvincible;
		string text = (!PlayerStatsManager.DebugInvincible) ? "red" : "green";
	}

	// Token: 0x040048F4 RID: 18676
	public const int HEALTH_MAX = 3;

	// Token: 0x040048F5 RID: 18677
	public const int HEALTH_TRUE_MAX = 9;

	// Token: 0x040048F6 RID: 18678
	private const float TIME_HIT = 2f;

	// Token: 0x040048F7 RID: 18679
	private const float TIME_REVIVED = 3f;

	// Token: 0x040048F8 RID: 18680
	private const int SUPER_MAX = 50;

	// Token: 0x040048F9 RID: 18681
	private const float SUPER_ON_PARRY = 10f;

	// Token: 0x040048FA RID: 18682
	private const float SUPER_ON_DEAL_DAMAGE = 0.0625f;

	// Token: 0x040048FB RID: 18683
	private const float EX_COST = 10f;

	// Token: 0x040048FC RID: 18684
	private const int HEALER_HP_MAX = 3;

	// Token: 0x04004903 RID: 18691
	public bool isChalice;

	// Token: 0x04004904 RID: 18692
	private int _curseCharmLevel;

	// Token: 0x04004905 RID: 18693
	private int curseCharmDashCounter;

	// Token: 0x04004906 RID: 18694
	private int curseCharmWhetstoneCounter;

	// Token: 0x04004910 RID: 18704
	private float timeSinceStoned = 1000f;

	// Token: 0x04004911 RID: 18705
	private float timeSinceReversed = 1000f;

	// Token: 0x04004912 RID: 18706
	private bool hardInvincibility;

	// Token: 0x0400491D RID: 18717
	private IEnumerator superBuilderRoutine;

	// Token: 0x0400491E RID: 18718
	private const float STONE_REDUCTION = 0.1f;

	// Token: 0x0400491F RID: 18719
	private Trilean2 lastMoveDir;

	// Token: 0x04004920 RID: 18720
	private Trilean2 currentMoveDir;

	// Token: 0x02000AD4 RID: 2772
	public class DeathEvent : PlayerEvent<PlayerStatsManager.DeathEvent>
	{
	}

	// Token: 0x02000AD5 RID: 2773
	public class ReviveEvent : PlayerEvent<PlayerStatsManager.ReviveEvent>
	{
	}

	// Token: 0x02000AD6 RID: 2774
	public enum PlayerState
	{
		// Token: 0x04004923 RID: 18723
		Ready,
		// Token: 0x04004924 RID: 18724
		Super
	}

	// Token: 0x02000AD7 RID: 2775
	// (Invoke) Token: 0x060042FC RID: 17148
	public delegate void OnPlayerHealthChangeHandler(int health, PlayerId playerId);

	// Token: 0x02000AD8 RID: 2776
	// (Invoke) Token: 0x06004300 RID: 17152
	public delegate void OnPlayerSuperChangedHandler(float super, PlayerId playerId, bool playEffect);

	// Token: 0x02000AD9 RID: 2777
	// (Invoke) Token: 0x06004304 RID: 17156
	public delegate void OnPlayerWeaponChangedHandler(Weapon weapon);

	// Token: 0x02000ADA RID: 2778
	// (Invoke) Token: 0x06004308 RID: 17160
	public delegate void OnPlayerDeathHandler(PlayerId playerId);

	// Token: 0x02000ADB RID: 2779
	// (Invoke) Token: 0x0600430C RID: 17164
	public delegate void OnStoneHandler();
}
