using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public abstract class Level : AbstractPausableComponent
{
	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06001269 RID: 4713 RVA: 0x0004C9E7 File Offset: 0x0004ADE7
	// (set) Token: 0x0600126A RID: 4714 RVA: 0x0004C9EE File Offset: 0x0004ADEE
	public static Level Current { get; private set; }

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x0600126B RID: 4715 RVA: 0x0004C9F6 File Offset: 0x0004ADF6
	// (set) Token: 0x0600126C RID: 4716 RVA: 0x0004C9FD File Offset: 0x0004ADFD
	public static Level.Mode CurrentMode { get; private set; } = Level.Mode.Normal;

	// Token: 0x0600126D RID: 4717 RVA: 0x0004CA05 File Offset: 0x0004AE05
	public static void SetCurrentMode(Level.Mode mode)
	{
		Level.CurrentMode = mode;
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x0600126E RID: 4718 RVA: 0x0004CA0D File Offset: 0x0004AE0D
	// (set) Token: 0x0600126F RID: 4719 RVA: 0x0004CA14 File Offset: 0x0004AE14
	public static bool PreviouslyWon { get; private set; }

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06001270 RID: 4720 RVA: 0x0004CA1C File Offset: 0x0004AE1C
	// (set) Token: 0x06001271 RID: 4721 RVA: 0x0004CA23 File Offset: 0x0004AE23
	public static bool Won { get; private set; }

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06001272 RID: 4722 RVA: 0x0004CA2B File Offset: 0x0004AE2B
	// (set) Token: 0x06001273 RID: 4723 RVA: 0x0004CA32 File Offset: 0x0004AE32
	public static LevelScoringData.Grade Grade { get; private set; }

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06001274 RID: 4724 RVA: 0x0004CA3A File Offset: 0x0004AE3A
	// (set) Token: 0x06001275 RID: 4725 RVA: 0x0004CA41 File Offset: 0x0004AE41
	public static LevelScoringData.Grade PreviousGrade { get; private set; }

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06001276 RID: 4726 RVA: 0x0004CA49 File Offset: 0x0004AE49
	// (set) Token: 0x06001277 RID: 4727 RVA: 0x0004CA50 File Offset: 0x0004AE50
	public static Level.Mode Difficulty { get; private set; }

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06001278 RID: 4728 RVA: 0x0004CA58 File Offset: 0x0004AE58
	// (set) Token: 0x06001279 RID: 4729 RVA: 0x0004CA5F File Offset: 0x0004AE5F
	public static Level.Mode PreviousDifficulty { get; private set; }

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x0600127A RID: 4730 RVA: 0x0004CA67 File Offset: 0x0004AE67
	// (set) Token: 0x0600127B RID: 4731 RVA: 0x0004CA6E File Offset: 0x0004AE6E
	public static LevelScoringData ScoringData { get; private set; }

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x0600127C RID: 4732 RVA: 0x0004CA76 File Offset: 0x0004AE76
	// (set) Token: 0x0600127D RID: 4733 RVA: 0x0004CA7D File Offset: 0x0004AE7D
	public static Levels PreviousLevel { get; private set; }

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x0600127E RID: 4734 RVA: 0x0004CA85 File Offset: 0x0004AE85
	// (set) Token: 0x0600127F RID: 4735 RVA: 0x0004CA8C File Offset: 0x0004AE8C
	public static Level.Type PreviousLevelType { get; private set; }

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06001280 RID: 4736 RVA: 0x0004CA94 File Offset: 0x0004AE94
	// (set) Token: 0x06001281 RID: 4737 RVA: 0x0004CA9B File Offset: 0x0004AE9B
	public static bool IsDicePalace { get; protected set; }

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06001282 RID: 4738 RVA: 0x0004CAA3 File Offset: 0x0004AEA3
	// (set) Token: 0x06001283 RID: 4739 RVA: 0x0004CAAA File Offset: 0x0004AEAA
	public static bool IsDicePalaceMain { get; protected set; }

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06001284 RID: 4740 RVA: 0x0004CAB2 File Offset: 0x0004AEB2
	// (set) Token: 0x06001285 RID: 4741 RVA: 0x0004CAB9 File Offset: 0x0004AEB9
	public static bool SuperUnlocked { get; protected set; }

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06001286 RID: 4742 RVA: 0x0004CAC1 File Offset: 0x0004AEC1
	// (set) Token: 0x06001287 RID: 4743 RVA: 0x0004CAC8 File Offset: 0x0004AEC8
	public static bool OverrideDifficulty { get; protected set; }

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06001288 RID: 4744 RVA: 0x0004CAD0 File Offset: 0x0004AED0
	// (set) Token: 0x06001289 RID: 4745 RVA: 0x0004CAD7 File Offset: 0x0004AED7
	public static bool IsChessBoss { get; protected set; }

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x0600128A RID: 4746 RVA: 0x0004CADF File Offset: 0x0004AEDF
	// (set) Token: 0x0600128B RID: 4747 RVA: 0x0004CAE6 File Offset: 0x0004AEE6
	public static bool IsTowerOfPower { get; protected set; }

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x0600128C RID: 4748 RVA: 0x0004CAEE File Offset: 0x0004AEEE
	// (set) Token: 0x0600128D RID: 4749 RVA: 0x0004CAF5 File Offset: 0x0004AEF5
	public static bool IsTowerOfPowerMain { get; protected set; }

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x0600128E RID: 4750 RVA: 0x0004CAFD File Offset: 0x0004AEFD
	// (set) Token: 0x0600128F RID: 4751 RVA: 0x0004CB04 File Offset: 0x0004AF04
	public static bool IsGraveyard { get; set; }

	// Token: 0x06001290 RID: 4752 RVA: 0x0004CB0C File Offset: 0x0004AF0C
	public static void ResetPreviousLevelInfo()
	{
		Level.Won = false;
		Level.SuperUnlocked = false;
		PlayerManager.playerWasChalice[0] = false;
		PlayerManager.playerWasChalice[1] = false;
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x0004CB2C File Offset: 0x0004AF2C
	public static Levels GetEnumByName(string levelName)
	{
		return (Levels)Enum.Parse(typeof(Levels), levelName);
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x0004CB50 File Offset: 0x0004AF50
	public static string GetLevelName(Levels level)
	{
		return Localization.Translate(level.ToString()).text;
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06001293 RID: 4755 RVA: 0x0004CB77 File Offset: 0x0004AF77
	// (set) Token: 0x06001294 RID: 4756 RVA: 0x0004CB7F File Offset: 0x0004AF7F
	public Level.Mode mode { get; protected set; }

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06001295 RID: 4757 RVA: 0x0004CB88 File Offset: 0x0004AF88
	// (set) Token: 0x06001296 RID: 4758 RVA: 0x0004CB90 File Offset: 0x0004AF90
	public bool defeatedMinion { get; set; }

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06001297 RID: 4759 RVA: 0x0004CB99 File Offset: 0x0004AF99
	// (set) Token: 0x06001298 RID: 4760 RVA: 0x0004CBA1 File Offset: 0x0004AFA1
	public bool PlayersCreated { get; private set; }

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06001299 RID: 4761 RVA: 0x0004CBAA File Offset: 0x0004AFAA
	// (set) Token: 0x0600129A RID: 4762 RVA: 0x0004CBB2 File Offset: 0x0004AFB2
	public bool Initialized { get; private set; }

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x0600129B RID: 4763 RVA: 0x0004CBBB File Offset: 0x0004AFBB
	// (set) Token: 0x0600129C RID: 4764 RVA: 0x0004CBC3 File Offset: 0x0004AFC3
	public bool Started { get; private set; }

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x0600129D RID: 4765 RVA: 0x0004CBCC File Offset: 0x0004AFCC
	// (set) Token: 0x0600129E RID: 4766 RVA: 0x0004CBD4 File Offset: 0x0004AFD4
	public bool[] BlockChaliceCharm { get; private set; }

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x0600129F RID: 4767 RVA: 0x0004CBDD File Offset: 0x0004AFDD
	// (set) Token: 0x060012A0 RID: 4768 RVA: 0x0004CBE5 File Offset: 0x0004AFE5
	public float LevelTime { get; private set; }

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x060012A1 RID: 4769 RVA: 0x0004CBEE File Offset: 0x0004AFEE
	public int Ground
	{
		get
		{
			return -this.bounds.bottom;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x060012A2 RID: 4770 RVA: 0x0004CBFC File Offset: 0x0004AFFC
	public int Ceiling
	{
		get
		{
			return this.bounds.top;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x060012A3 RID: 4771 RVA: 0x0004CC09 File Offset: 0x0004B009
	public int Left
	{
		get
		{
			return -this.bounds.left;
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x060012A4 RID: 4772 RVA: 0x0004CC17 File Offset: 0x0004B017
	public int Right
	{
		get
		{
			return this.bounds.right;
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x060012A5 RID: 4773 RVA: 0x0004CC24 File Offset: 0x0004B024
	public int Width
	{
		get
		{
			return this.bounds.left + this.bounds.right;
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x060012A6 RID: 4774 RVA: 0x0004CC3D File Offset: 0x0004B03D
	public int Height
	{
		get
		{
			return this.bounds.top + this.bounds.bottom;
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x060012A7 RID: 4775 RVA: 0x0004CC56 File Offset: 0x0004B056
	public Level.Type LevelType
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0004CC5E File Offset: 0x0004B05E
	// (set) Token: 0x060012A9 RID: 4777 RVA: 0x0004CC66 File Offset: 0x0004B066
	public bool CameraRotates { get; protected set; }

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x060012AA RID: 4778 RVA: 0x0004CC6F File Offset: 0x0004B06F
	public bool IntroComplete
	{
		get
		{
			return this.intro.introComplete;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x060012AB RID: 4779 RVA: 0x0004CC7C File Offset: 0x0004B07C
	// (set) Token: 0x060012AC RID: 4780 RVA: 0x0004CC84 File Offset: 0x0004B084
	public Level.Timeline timeline { get; protected set; }

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x060012AD RID: 4781
	public abstract Levels CurrentLevel { get; }

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x060012AE RID: 4782
	public abstract Scenes CurrentScene { get; }

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x060012AF RID: 4783 RVA: 0x0004CC8D File Offset: 0x0004B08D
	public Level.Camera CameraSettings
	{
		get
		{
			return this.camera;
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x060012B0 RID: 4784
	public abstract Sprite BossPortrait { get; }

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x060012B1 RID: 4785
	public abstract string BossQuote { get; }

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0004CC95 File Offset: 0x0004B095
	// (set) Token: 0x060012B3 RID: 4787 RVA: 0x0004CC9D File Offset: 0x0004B09D
	public bool Ending { get; protected set; }

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x060012B4 RID: 4788 RVA: 0x0004CCA6 File Offset: 0x0004B0A6
	public static bool IsInBossesHub
	{
		get
		{
			return Level.IsDicePalace || Level.IsDicePalaceMain || Level.IsTowerOfPower;
		}
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x0004CCC4 File Offset: 0x0004B0C4
	public static PlayersStatsBossesHub GetPlayerStats(PlayerId playerId)
	{
		if (Level.IsTowerOfPower)
		{
			return TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId];
		}
		if (Level.IsDicePalace || Level.IsDicePalaceMain)
		{
			return (playerId != PlayerId.PlayerOne) ? DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS : DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS;
		}
		return null;
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x0004CD03 File Offset: 0x0004B103
	protected virtual void OnEnable()
	{
		EventManager.Instance.AddListener<PlayerStatsManager.DeathEvent>(new EventManager.EventDelegate<PlayerStatsManager.DeathEvent>(this.OnPlayerDeath));
		EventManager.Instance.AddListener<PlayerStatsManager.ReviveEvent>(new EventManager.EventDelegate<PlayerStatsManager.ReviveEvent>(this.OnPlayerRevive));
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x0004CD31 File Offset: 0x0004B131
	protected virtual void OnDisable()
	{
		EventManager.Instance.RemoveListener<PlayerStatsManager.DeathEvent>(new EventManager.EventDelegate<PlayerStatsManager.DeathEvent>(this.OnPlayerDeath));
		EventManager.Instance.RemoveListener<PlayerStatsManager.ReviveEvent>(new EventManager.EventDelegate<PlayerStatsManager.ReviveEvent>(this.OnPlayerRevive));
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x0004CD60 File Offset: 0x0004B160
	protected override void Awake()
	{
		base.Awake();
		this.CheckIfInABossesHub();
		Cuphead.Init(false);
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
		DamageDealer.didDamageWithNonSmallPlaneWeapon = false;
		Levels currentLevel = this.CurrentLevel;
		switch (currentLevel)
		{
		case Levels.Platforming_Level_1_1:
		case Levels.Platforming_Level_1_2:
		case Levels.Platforming_Level_3_1:
		case Levels.Platforming_Level_3_2:
			break;
		default:
			if (currentLevel != Levels.Platforming_Level_2_2 && currentLevel != Levels.Platforming_Level_2_1)
			{
				this.mode = Level.CurrentMode;
				goto IL_95;
			}
			break;
		}
		this.mode = Level.Mode.Normal;
		IL_95:
		Level.Current = this;
		PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(this.CurrentLevel);
		Level.Won = false;
		this.BGMPlaylistCurrent = levelData.bgmPlayListCurrent;
		Level.PreviousLevel = this.CurrentLevel;
		Level.PreviousLevelType = this.type;
		Level.PreviouslyWon = levelData.completed;
		Level.PreviousGrade = levelData.grade;
		Level.PreviousDifficulty = levelData.difficultyBeaten;
		Level.SuperUnlocked = false;
		Level.IsChessBoss = false;
		Level.IsGraveyard = false;
		this.Ending = false;
		this.PartialInit();
		Application.targetFrameRate = 60;
		this.CreateUI();
		this.CreateHUD();
		LevelCoin.OnLevelStart();
		SceneLoader.SetCurrentLevel(this.CurrentLevel);
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x0004CEA4 File Offset: 0x0004B2A4
	public virtual bool AllowDjimmi()
	{
		return !this.isMausoleum && this.type != Level.Type.Tutorial && !(SceneLoader.CurrentContext is GauntletContext);
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x0004CED0 File Offset: 0x0004B2D0
	protected virtual void CheckIfInABossesHub()
	{
		Level.IsDicePalace = false;
		Level.IsDicePalaceMain = false;
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x0004CEDE File Offset: 0x0004B2DE
	public static void ResetBossesHub()
	{
		Level.IsDicePalace = false;
		Level.IsDicePalaceMain = false;
		if (Level.IsTowerOfPower)
		{
			TowerOfPowerLevelGameInfo.GameInfo.CleanUp();
			Level.IsTowerOfPower = false;
			Level.IsTowerOfPowerMain = false;
		}
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x0004CF0C File Offset: 0x0004B30C
	protected virtual void PartialInit()
	{
		if (Level.ScoringData == null || ((this.type == Level.Type.Battle || this.type == Level.Type.Platforming) && (!Level.IsDicePalace || (Level.IsDicePalaceMain && DicePalaceMainLevelGameInfo.TURN_COUNTER == 0)) && (!Level.IsTowerOfPowerMain || TowerOfPowerLevelGameInfo.TURN_COUNTER == 0)))
		{
			Level.ScoringData = new LevelScoringData();
			Level.ScoringData.goalTime = ((this.mode != Level.Mode.Easy) ? ((this.mode != Level.Mode.Normal) ? this.goalTimes.hard : this.goalTimes.normal) : this.goalTimes.easy);
		}
		if ((Level.IsDicePalace && !Level.IsDicePalaceMain) || (Level.IsTowerOfPower && !Level.IsTowerOfPowerMain))
		{
			Level.ScoringData.goalTime += ((this.mode != Level.Mode.Easy) ? ((this.mode != Level.Mode.Normal) ? this.goalTimes.hard : this.goalTimes.normal) : this.goalTimes.easy);
		}
		Level.ScoringData.difficulty = this.mode;
	}

	// Token: 0x060012BD RID: 4797 RVA: 0x0004D050 File Offset: 0x0004B450
	protected virtual void Start()
	{
		CupheadTime.SetAll(1f);
		switch (this.type)
		{
		default:
			base.StartCoroutine(this.startBattle_cr());
			break;
		case Level.Type.Tutorial:
			base.StartCoroutine(this.startNonBattle_cr());
			break;
		case Level.Type.Platforming:
			base.StartCoroutine(this.startPlatforming_cr());
			break;
		}
		this.CreateAudio();
		this.CreateColliders();
		this.CreatePlayers();
		this.CreateCamera();
		this.gui.LevelInit();
		this.hud.LevelInit();
		this.SetRichPresence();
		this.Initialized = true;
		if (this.playerMode != PlayerMode.Plane && this.CurrentLevel != Levels.Devil && this.CurrentLevel != Levels.Saltbaker && this.type != Level.Type.Platforming && this.type != Level.Type.Tutorial)
		{
			base.StartCoroutine(this.check_intros_cr());
		}
		if (this.CurrentLevel == Levels.Devil || this.CurrentLevel == Levels.Saltbaker)
		{
			this.CheckIntros();
		}
	}

	// Token: 0x060012BE RID: 4798 RVA: 0x0004D170 File Offset: 0x0004B570
	protected virtual void Update()
	{
		if (!this.Started)
		{
			this.CheckPlayerHoldingButtons();
		}
		this.LevelTime += CupheadTime.Delta;
		if (this.playerIsDead)
		{
			this.playerDeathDelayFrames++;
			if (this.playerDeathDelayFrames < 5)
			{
				if (PlayerManager.Multiplayer)
				{
					if (this.players[0].IsDead && this.players[1].IsDead)
					{
						this._OnLose();
					}
				}
				else
				{
					this._OnLose();
				}
				this.playerIsDead = false;
				this.playerDeathDelayFrames = 0;
			}
		}
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x0004D218 File Offset: 0x0004B618
	protected override void OnDestroy()
	{
		base.OnDestroy();
		PlayerManager.ClearPlayers();
		Level.Current = null;
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
		this.LevelResources = null;
		this.players = null;
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x0004D268 File Offset: 0x0004B668
	public void SetBounds(int? left, int? right, int? top, int? bottom)
	{
		if (left != null)
		{
			this.bounds.left = left.Value;
		}
		if (right != null)
		{
			this.bounds.right = right.Value;
		}
		if (top != null)
		{
			this.bounds.top = top.Value;
		}
		if (bottom != null)
		{
			this.bounds.bottom = bottom.Value;
		}
		this.bounds.SetColliderPositions();
	}

	// Token: 0x060012C1 RID: 4801 RVA: 0x0004D2F8 File Offset: 0x0004B6F8
	protected void CleanUpScore()
	{
		Level.ScoringData = null;
	}

	// Token: 0x060012C2 RID: 4802 RVA: 0x0004D300 File Offset: 0x0004B700
	public void RegisterMinionKilled()
	{
		if (!this.defeatedMinion)
		{
		}
		this.defeatedMinion = true;
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x0004D314 File Offset: 0x0004B714
	private void CreateAudio()
	{
		LevelAudio.Create();
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x0004D31C File Offset: 0x0004B71C
	protected virtual void CreatePlayers()
	{
		this.PlayersCreated = true;
		foreach (AbstractPlayerController abstractPlayerController in UnityEngine.Object.FindObjectsOfType<AbstractPlayerController>())
		{
			UnityEngine.Object.Destroy(abstractPlayerController.gameObject);
		}
		this.players = new AbstractPlayerController[2];
		this.BlockChaliceCharm = new bool[2];
		this.BlockChaliceCharm[0] = this.blockChalice;
		this.BlockChaliceCharm[1] = this.blockChalice;
		if (this.playerMode == PlayerMode.Custom)
		{
			return;
		}
		if (PlayerManager.Multiplayer && this.allowMultiplayer)
		{
			if (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).charm == Charm.charm_chalice && PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm == Charm.charm_chalice)
			{
				this.BlockChaliceCharm[(!Rand.Bool()) ? 1 : 0] = true;
			}
			if (this.isMausoleum)
			{
				this.BlockChaliceCharm[0] = true;
				this.BlockChaliceCharm[1] = true;
			}
			Vector3 v = (this.playerMode != PlayerMode.Plane) ? this.spawns.playerOne : this.player1PlaneSpawnPos;
			this.players[0] = AbstractPlayerController.Create(PlayerId.PlayerOne, v, this.playerMode);
			Vector3 v2 = (this.playerMode != PlayerMode.Plane) ? this.spawns.playerTwo : this.player2PlaneSpawnPos;
			this.players[1] = AbstractPlayerController.Create(PlayerId.PlayerTwo, v2, this.playerMode);
		}
		else
		{
			Vector3 v3 = (this.playerMode != PlayerMode.Plane) ? this.spawns.playerOneSingle : this.player1PlaneSpawnPos;
			this.players[0] = AbstractPlayerController.Create(PlayerId.PlayerOne, v3, this.playerMode);
		}
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x0004D4F8 File Offset: 0x0004B8F8
	private void CheckPlayerCharacters()
	{
		Level.ScoringData.player1IsChalice = this.players[0].stats.isChalice;
		if (PlayerManager.Multiplayer && this.allowMultiplayer)
		{
			Level.ScoringData.player2IsChalice = this.players[1].stats.isChalice;
		}
	}

	// Token: 0x060012C6 RID: 4806 RVA: 0x0004D554 File Offset: 0x0004B954
	private void CheckIntros()
	{
		LevelPlayerAnimationController component = this.players[0].GetComponent<LevelPlayerAnimationController>();
		if (component != null)
		{
			if (this.players[0].stats.Loadout.charm == Charm.charm_chalice)
			{
				if (this.players[1] != null && this.players[1].stats.isChalice && this.CurrentLevel != Levels.Devil && this.CurrentLevel != Levels.Saltbaker && (!Level.IsDicePalace || DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY))
				{
					component.CookieFail();
				}
				if (this.players[0].stats.isChalice && (this.CurrentLevel == Levels.Devil || this.CurrentLevel == Levels.Saltbaker))
				{
					component.ScaredChalice(this.CurrentLevel == Levels.Devil);
				}
			}
			else if (this.CurrentLevel != Levels.Devil && this.CurrentLevel != Levels.Saltbaker)
			{
				if (this.player1HeldJump && !this.player1HeldSuper)
				{
					component.IsIntroB();
				}
				else if (!this.player1HeldJump && !this.player1HeldSuper && Rand.Bool())
				{
					component.IsIntroB();
				}
			}
		}
		if (this.players.Length >= 2 && this.players[1] != null)
		{
			LevelPlayerAnimationController component2 = this.players[1].GetComponent<LevelPlayerAnimationController>();
			if (component2 != null)
			{
				if (this.players[1].stats.Loadout.charm == Charm.charm_chalice)
				{
					if (this.players[0].stats.isChalice && this.CurrentLevel != Levels.Devil && this.CurrentLevel != Levels.Saltbaker && (!Level.IsDicePalace || DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY))
					{
						component2.CookieFail();
					}
					if (this.players[1].stats.isChalice && (this.CurrentLevel == Levels.Devil || this.CurrentLevel == Levels.Saltbaker))
					{
						component2.ScaredChalice(this.CurrentLevel == Levels.Devil);
					}
				}
				else if (PlayerManager.Multiplayer && this.CurrentLevel != Levels.Devil && this.CurrentLevel != Levels.Saltbaker)
				{
					if (this.player2HeldJump && !this.player2HeldSuper)
					{
						component2.IsIntroB();
					}
					else if (!this.player2HeldJump && !this.player2HeldSuper && Rand.Bool())
					{
						component2.IsIntroB();
					}
				}
			}
		}
	}

	// Token: 0x060012C7 RID: 4807 RVA: 0x0004D824 File Offset: 0x0004BC24
	protected virtual void CreatePlayerTwoOnJoin()
	{
		if (PlayerManager.Multiplayer && this.allowMultiplayer)
		{
			if (this.players[0].stats.isChalice || this.blockChalice)
			{
				this.BlockChaliceCharm[1] = true;
			}
			this.players[1] = AbstractPlayerController.Create(PlayerId.PlayerTwo, this.players[0].center, this.playerMode);
			this.players[1].LevelJoin(this.players[0].center);
		}
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x0004D8B4 File Offset: 0x0004BCB4
	private void CreateCamera()
	{
		if (this.players == null)
		{
			global::Debug.LogError("Level.CreateCamera() must be called AFTER Level.CreatePlayers()", null);
		}
		CupheadLevelCamera cupheadLevelCamera = UnityEngine.Object.FindObjectOfType<CupheadLevelCamera>();
		cupheadLevelCamera.Init(this.camera);
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x0004D8E9 File Offset: 0x0004BCE9
	private void CreateUI()
	{
		this.gui = UnityEngine.Object.FindObjectOfType<LevelGUI>();
		if (this.gui == null)
		{
			this.gui = this.LevelResources.levelGUI.InstantiatePrefab<LevelGUI>();
		}
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x0004D922 File Offset: 0x0004BD22
	private void CreateHUD()
	{
		this.hud = UnityEngine.Object.FindObjectOfType<LevelHUD>();
		if (this.hud == null)
		{
			this.hud = this.LevelResources.levelHUD.InstantiatePrefab<LevelHUD>();
		}
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x0004D95C File Offset: 0x0004BD5C
	private void CreateColliders()
	{
		if (this.playerMode == PlayerMode.Plane)
		{
			return;
		}
		this.collidersRoot = new GameObject("Colliders").transform;
		this.collidersRoot.parent = base.transform;
		this.collidersRoot.ResetLocalTransforms();
		this.SetupCollider(Level.Bounds.Side.Left);
		this.SetupCollider(Level.Bounds.Side.Right);
		this.SetupCollider(Level.Bounds.Side.Top);
		this.SetupCollider(Level.Bounds.Side.Bottom);
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x0004D9C8 File Offset: 0x0004BDC8
	private Transform SetupCollider(Level.Bounds.Side side)
	{
		string name = string.Empty;
		string tag = string.Empty;
		int layer = 0;
		int num = 0;
		Vector2 zero = Vector2.zero;
		switch (side)
		{
		case Level.Bounds.Side.Left:
			name = "Level_Wall_Left";
			tag = "Wall";
			layer = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString());
			num = 90;
			break;
		case Level.Bounds.Side.Right:
			name = "Level_Wall_Right";
			tag = "Wall";
			layer = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString());
			num = -90;
			break;
		case Level.Bounds.Side.Top:
			name = "Level_Ceiling";
			tag = "Ceiling";
			layer = LayerMask.NameToLayer(Layers.Bounds_Ceiling.ToString());
			break;
		case Level.Bounds.Side.Bottom:
			name = "Level_Ground";
			tag = "Ground";
			layer = LayerMask.NameToLayer(Layers.Bounds_Ground.ToString());
			num = 180;
			break;
		}
		GameObject gameObject = new GameObject(name);
		gameObject.tag = tag;
		gameObject.layer = layer;
		gameObject.transform.ResetLocalTransforms();
		gameObject.transform.SetPosition(new float?(zero.x), new float?(zero.y), null);
		gameObject.transform.SetEulerAngles(null, null, new float?((float)num));
		gameObject.transform.parent = this.collidersRoot;
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.isTrigger = true;
		boxCollider2D.size = new Vector2(10000f, 400f);
		Rigidbody2D rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
		rigidbody2D.gravityScale = 0f;
		rigidbody2D.drag = 0f;
		rigidbody2D.angularDrag = 0f;
		rigidbody2D.isKinematic = true;
		this.bounds.colliders.Add(side, boxCollider2D);
		this.bounds.SetColliderPositions();
		gameObject.SetActive(this.bounds.GetEnabled(side));
		return gameObject.transform;
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x0004DBD4 File Offset: 0x0004BFD4
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(this.spawns.playerOne, 20f);
		Gizmos.DrawSphere(this.spawns.playerTwo, 20f);
		Gizmos.DrawSphere(this.spawns.playerOneSingle, 30f);
		Gizmos.color = Color.red;
		Gizmos.DrawCube(this.spawns.playerOneSingle, new Vector3(20f, 20f, 20f));
		Gizmos.DrawWireSphere(this.spawns.playerOne, 20f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.spawns.playerTwo, 20f);
		Gizmos.color = Color.white;
		if (this.camera.bounds.topEnabled)
		{
			Gizmos.DrawLine(new Vector3((float)this.camera.bounds.right, (float)this.camera.bounds.top, 0f), new Vector3((float)(-(float)this.camera.bounds.left), (float)this.camera.bounds.top, 0f));
		}
		if (this.camera.bounds.bottomEnabled)
		{
			Gizmos.DrawLine(new Vector3((float)this.camera.bounds.right, (float)(-(float)this.camera.bounds.bottom), 0f), new Vector3((float)(-(float)this.camera.bounds.left), (float)(-(float)this.camera.bounds.bottom), 0f));
		}
		if (this.camera.bounds.leftEnabled)
		{
			Gizmos.DrawLine(new Vector3((float)(-(float)this.camera.bounds.left), (float)this.camera.bounds.top, 0f), new Vector3((float)(-(float)this.camera.bounds.left), (float)(-(float)this.camera.bounds.bottom), 0f));
		}
		if (this.camera.bounds.rightEnabled)
		{
			Gizmos.DrawLine(new Vector3((float)this.camera.bounds.right, (float)this.camera.bounds.top, 0f), new Vector3((float)this.camera.bounds.right, (float)(-(float)this.camera.bounds.bottom), 0f));
		}
		if (this.bounds.topEnabled)
		{
			Gizmos.color = Color.blue;
		}
		else
		{
			Gizmos.color = Color.black;
		}
		Gizmos.DrawLine(new Vector3((float)this.bounds.right, (float)this.bounds.top, 0f), new Vector3((float)(-(float)this.bounds.left), (float)this.bounds.top, 0f));
		if (this.bounds.bottomEnabled)
		{
			Gizmos.color = Color.green;
		}
		else
		{
			Gizmos.color = Color.black;
		}
		Gizmos.DrawLine(new Vector3((float)this.bounds.right, (float)(-(float)this.bounds.bottom), 0f), new Vector3((float)(-(float)this.bounds.left), (float)(-(float)this.bounds.bottom), 0f));
		if (this.bounds.leftEnabled)
		{
			Gizmos.color = Color.red;
		}
		else
		{
			Gizmos.color = Color.black;
		}
		Gizmos.DrawLine(new Vector3((float)(-(float)this.bounds.left), (float)this.bounds.top, 0f), new Vector3((float)(-(float)this.bounds.left), (float)(-(float)this.bounds.bottom), 0f));
		if (this.bounds.rightEnabled)
		{
			Gizmos.color = Color.red;
		}
		else
		{
			Gizmos.color = Color.black;
		}
		Gizmos.DrawLine(new Vector3((float)this.bounds.right, (float)this.bounds.top, 0f), new Vector3((float)this.bounds.right, (float)(-(float)this.bounds.bottom), 0f));
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x0004E05A File Offset: 0x0004C45A
	protected virtual void OnPlayerJoined(PlayerId playerId)
	{
		LevelNewPlayerGUI.Current.Init();
		this.CreatePlayerTwoOnJoin();
		this.SetRichPresence();
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x0004E074 File Offset: 0x0004C474
	private void OnPlayerLeave(PlayerId playerId)
	{
		if (playerId == PlayerId.PlayerTwo)
		{
			AbstractPlayerController player = PlayerManager.GetPlayer(playerId);
			if (player != null)
			{
				player.OnLeave(playerId);
			}
			if (PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead)
			{
				this._OnLose();
			}
		}
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x0004E0B8 File Offset: 0x0004C4B8
	private void SetRichPresence()
	{
		if (this.CurrentLevel == Levels.Mausoleum)
		{
			OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Mausoleum", true);
		}
		else if (this.CurrentLevel == Levels.Tutorial || this.CurrentLevel == Levels.House)
		{
			OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Tutorial", true);
		}
		else if (this.CurrentLevel == Levels.ShmupTutorial)
		{
			OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Blueprint", true);
		}
		else
		{
			Level.Type type = this.type;
			if (type != Level.Type.Battle)
			{
				if (type == Level.Type.Platforming)
				{
					OnlineManager.Instance.Interface.SetStat(PlayerId.Any, "PlatformingLevel", SceneLoader.SceneName);
					OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Playing", true);
				}
			}
			else
			{
				OnlineManager.Instance.Interface.SetStat(PlayerId.Any, "Boss", SceneLoader.SceneName);
				OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Fighting", true);
			}
		}
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x0004E1F0 File Offset: 0x0004C5F0
	private void OnPlayerDeath(PlayerStatsManager.DeathEvent e)
	{
		if (this.timeline != null && this.LevelType != Level.Type.Platforming)
		{
			this.timeline.OnPlayerDeath(e.playerId);
		}
		this.playerIsDead = true;
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x0004E221 File Offset: 0x0004C621
	private void OnPlayerRevive(PlayerStatsManager.ReviveEvent e)
	{
		this.timeline.OnPlayerRevive(e.playerId);
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x0004E234 File Offset: 0x0004C634
	private void CheckPlayerHoldingButtons()
	{
		if (PlayerManager.GetPlayerInput(PlayerId.PlayerOne).GetButton(2) && !this.player1HeldJump)
		{
			this.player1HeldJump = true;
		}
		if (PlayerManager.GetPlayerInput(PlayerId.PlayerOne).GetButton(4) && !this.player1HeldSuper)
		{
			this.player1HeldSuper = true;
		}
		if (PlayerManager.Multiplayer)
		{
			if (PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).GetButton(2) && !this.player2HeldJump)
			{
				this.player2HeldJump = true;
			}
			if (PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).GetButton(4) && !this.player2HeldSuper)
			{
				this.player2HeldSuper = true;
			}
		}
	}

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x060012D4 RID: 4820 RVA: 0x0004E2D8 File Offset: 0x0004C6D8
	// (remove) Token: 0x060012D5 RID: 4821 RVA: 0x0004E310 File Offset: 0x0004C710
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnLevelStartEvent;

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x060012D6 RID: 4822 RVA: 0x0004E348 File Offset: 0x0004C748
	// (remove) Token: 0x060012D7 RID: 4823 RVA: 0x0004E380 File Offset: 0x0004C780
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnLevelEndEvent;

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x060012D8 RID: 4824 RVA: 0x0004E3B8 File Offset: 0x0004C7B8
	// (remove) Token: 0x060012D9 RID: 4825 RVA: 0x0004E3F0 File Offset: 0x0004C7F0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPlatformingLevelAwakeEvent;

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x060012DA RID: 4826 RVA: 0x0004E428 File Offset: 0x0004C828
	// (remove) Token: 0x060012DB RID: 4827 RVA: 0x0004E460 File Offset: 0x0004C860
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnStateChangedEvent;

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x060012DC RID: 4828 RVA: 0x0004E498 File Offset: 0x0004C898
	// (remove) Token: 0x060012DD RID: 4829 RVA: 0x0004E4D0 File Offset: 0x0004C8D0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnWinEvent;

	// Token: 0x14000036 RID: 54
	// (add) Token: 0x060012DE RID: 4830 RVA: 0x0004E508 File Offset: 0x0004C908
	// (remove) Token: 0x060012DF RID: 4831 RVA: 0x0004E540 File Offset: 0x0004C940
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPreWinEvent;

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x060012E0 RID: 4832 RVA: 0x0004E578 File Offset: 0x0004C978
	// (remove) Token: 0x060012E1 RID: 4833 RVA: 0x0004E5B0 File Offset: 0x0004C9B0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnLoseEvent;

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x060012E2 RID: 4834 RVA: 0x0004E5E8 File Offset: 0x0004C9E8
	// (remove) Token: 0x060012E3 RID: 4835 RVA: 0x0004E620 File Offset: 0x0004CA20
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPreLoseEvent;

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x060012E4 RID: 4836 RVA: 0x0004E658 File Offset: 0x0004CA58
	// (remove) Token: 0x060012E5 RID: 4837 RVA: 0x0004E690 File Offset: 0x0004CA90
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnTransitionInCompleteEvent;

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x060012E6 RID: 4838 RVA: 0x0004E6C8 File Offset: 0x0004CAC8
	// (remove) Token: 0x060012E7 RID: 4839 RVA: 0x0004E700 File Offset: 0x0004CB00
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnIntroEvent;

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x060012E8 RID: 4840 RVA: 0x0004E738 File Offset: 0x0004CB38
	// (remove) Token: 0x060012E9 RID: 4841 RVA: 0x0004E770 File Offset: 0x0004CB70
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnBossDeathExplosionsEvent;

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x060012EA RID: 4842 RVA: 0x0004E7A8 File Offset: 0x0004CBA8
	// (remove) Token: 0x060012EB RID: 4843 RVA: 0x0004E7E0 File Offset: 0x0004CBE0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnBossDeathExplosionsEndEvent;

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x060012EC RID: 4844 RVA: 0x0004E818 File Offset: 0x0004CC18
	// (remove) Token: 0x060012ED RID: 4845 RVA: 0x0004E850 File Offset: 0x0004CC50
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnBossDeathExplosionsFalloffEvent;

	// Token: 0x060012EE RID: 4846 RVA: 0x0004E888 File Offset: 0x0004CC88
	private void _OnLevelStart()
	{
		this.OnLevelStart();
		if (this.OnLevelStartEvent != null)
		{
			this.OnLevelStartEvent();
		}
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		InterruptingPrompt.SetCanInterrupt(true);
		PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(this.CurrentLevel);
		if (levelData != null && !Level.IsTowerOfPower)
		{
			levelData.played = true;
		}
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x0004E8E7 File Offset: 0x0004CCE7
	private void _OnLevelEnd()
	{
		this.Ending = true;
		this.OnLevelEnd();
		if (this.OnLevelEndEvent != null)
		{
			this.OnLevelEndEvent();
		}
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		PlayerManager.ClearJoinPrompt();
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x0004E919 File Offset: 0x0004CD19
	protected void zHack_OnStateChanged()
	{
		this.OnStateChanged();
		if (this.OnStateChangedEvent != null)
		{
			this.OnStateChangedEvent();
		}
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x0004E938 File Offset: 0x0004CD38
	protected void zHack_OnWin()
	{
		PlayerManager.playerWasChalice[0] = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice;
		PlayerManager.playerWasChalice[1] = (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice);
		this.CheckPlayerCharacters();
		Level.Won = true;
		Level.Difficulty = this.mode;
		PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(this.CurrentLevel);
		Level.ScoringData.finalHP = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health;
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			Level.ScoringData.finalHP = Mathf.Max(Level.ScoringData.finalHP, PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health);
		}
		Level.ScoringData.finalHP = Mathf.Min(Level.ScoringData.finalHP, (int)Cuphead.Current.ScoringProperties.hitsForNoScore);
		Level.ScoringData.usedDjimmi = (PlayerData.Data.DjimmiActivatedCurrentRegion() && this.AllowDjimmi() && this.mode != Level.Mode.Hard);
		if (Level.ScoringData.usedDjimmi && (!Level.IsDicePalace || Level.IsDicePalaceMain))
		{
			PlayerData.Data.DeactivateDjimmi();
		}
		if (!Level.IsTowerOfPower)
		{
			levelData.completed = true;
			if (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).charm == Charm.charm_chalice)
			{
				levelData.completedAsChaliceP1 = true;
			}
			if (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm == Charm.charm_chalice)
			{
				levelData.completedAsChaliceP2 = true;
			}
		}
		Level.ScoringData.time += this.LevelTime;
		if ((this.type == Level.Type.Battle || this.type == Level.Type.Platforming) && (!Level.IsDicePalace || Level.IsDicePalaceMain))
		{
			Level.Grade = Level.ScoringData.CalculateGrade();
			float time = Level.ScoringData.time;
			if (!Level.IsTowerOfPower)
			{
				if (Level.Difficulty > Level.PreviousDifficulty || !Level.PreviouslyWon)
				{
					levelData.difficultyBeaten = Level.Difficulty;
				}
				if (Level.Grade > Level.PreviousGrade || !Level.PreviouslyWon)
				{
					levelData.grade = Level.Grade;
					levelData.bestTime = time;
				}
				else if (Level.Grade == Level.PreviousGrade && time < levelData.bestTime)
				{
					levelData.bestTime = time;
				}
				if (this.CurrentLevel == Levels.Devil)
				{
					PlayerData.Data.IsHardModeAvailable = true;
				}
				if (this.CurrentLevel == Levels.Saltbaker)
				{
					PlayerData.Data.IsHardModeAvailableDLC = true;
				}
			}
		}
		if (Level.IsChessBoss)
		{
			if (PlayerData.Data.currentChessBossZone != MapCastleZones.Zone.None)
			{
				MapCastleZones.Zone currentChessBossZone = PlayerData.Data.currentChessBossZone;
				PlayerData.Data.currentChessBossZone = MapCastleZones.Zone.None;
				List<MapCastleZones.Zone> usedChessBossZones = PlayerData.Data.usedChessBossZones;
				if (!usedChessBossZones.Contains(currentChessBossZone))
				{
					usedChessBossZones.Add(currentChessBossZone);
				}
			}
			string[] array;
			if (ChessCastleLevel.Coins.TryGetValue(this.CurrentLevel, out array))
			{
				foreach (string coinID in array)
				{
					if (!PlayerData.Data.coinManager.GetCoinCollected(coinID))
					{
						PlayerData.Data.coinManager.SetCoinValue(coinID, true, PlayerId.Any);
						PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1);
						PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1);
					}
				}
			}
		}
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		if (Level.Difficulty != Level.Mode.Easy && player != null && player.stats.Loadout.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerOne) >= 0)
		{
			levelData.curseCharmP1 = true;
		}
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (Level.Difficulty != Level.Mode.Easy && player2 != null && player2.stats.Loadout.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerTwo) >= 0)
		{
			levelData.curseCharmP2 = true;
		}
		this._OnLevelEnd();
		this._OnPreWin();
		if (this.LevelType == Level.Type.Battle)
		{
			base.StartCoroutine(this.bossDeath_cr());
		}
		this.OnWin();
		if (this.OnWinEvent != null)
		{
			this.OnWinEvent();
		}
		if (!Level.IsTowerOfPower)
		{
			PlayerData.SaveCurrentFile();
		}
		if (!Level.IsTowerOfPower)
		{
			Levels[] levels = null;
			Levels[] levels2 = null;
			string str = null;
			Scenes currentMap = PlayerData.Data.CurrentMap;
			bool flag = Array.Exists<Levels>(Level.kingOfGamesLevels, (Levels level) => this.CurrentLevel == level);
			switch (currentMap)
			{
			case Scenes.scene_map_world_1:
				levels2 = (levels = Level.world1BossLevels);
				str = "World1";
				break;
			case Scenes.scene_map_world_2:
				levels2 = (levels = Level.world2BossLevels);
				str = "World2";
				break;
			case Scenes.scene_map_world_3:
				levels2 = (levels = Level.world3BossLevels);
				str = "World3";
				break;
			case Scenes.scene_map_world_4:
				levels2 = (levels = Level.world4BossLevels);
				str = "World4";
				break;
			default:
				if (currentMap == Scenes.scene_map_world_DLC)
				{
					levels = Level.worldDLCBossLevels;
					levels2 = Level.worldDLCBossLevelsWithSaltbaker;
					str = "WorldDLC";
				}
				break;
			}
			if (currentMap == Scenes.scene_map_world_4)
			{
				if (this.CurrentLevel == Levels.DicePalaceMain)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "CompleteDicePalace");
				}
				else if (this.CurrentLevel == Levels.Devil)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "CompleteDevil");
				}
			}
			else if (this.type == Level.Type.Battle && PlayerData.Data.CheckLevelsCompleted(levels))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "Complete" + str);
			}
			if (this.CurrentLevel == Levels.Saltbaker)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatSaltbaker);
			}
			if (this.type == Level.Type.Battle && Level.Difficulty == Level.Mode.Hard && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Hard) && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Hard) && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Hard) && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world4BossLevels, Level.Mode.Hard))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "NewGamePlus");
			}
			if (this.type == Level.Type.Battle && Level.Grade >= LevelScoringData.Grade.AMinus && PlayerData.Data.CheckLevelsHaveMinGrade(levels2, LevelScoringData.Grade.AMinus))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "ARank" + str);
			}
			if (this.type == Level.Type.Platforming && !this.isMausoleum && Level.ScoringData.pacifistRun && PlayerData.Data.CheckLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.P))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "PacifistRun");
			}
			if ((this.type == Level.Type.Battle || this.type == Level.Type.Platforming) && (!Level.IsDicePalace || Level.IsDicePalaceMain) && !this.isMausoleum && !flag && Level.ScoringData.numTimesHit == 0)
			{
				if (Level.IsDicePalaceMain)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "NoHitsTakenDicePalace");
				}
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "NoHitsTaken");
			}
			if (this.type == Level.Type.Battle)
			{
				if (DamageDealer.lastPlayerDamageSource == DamageDealer.DamageSource.Super)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(DamageDealer.lastPlayer, "SuperWin");
					AbstractPlayerController abstractPlayerController = (DamageDealer.lastPlayer != PlayerId.PlayerOne) ? player2 : player;
					if (abstractPlayerController != null && abstractPlayerController.stats.isChalice)
					{
						OnlineManager.Instance.Interface.UnlockAchievement(DamageDealer.lastPlayer, OnlineAchievementData.DLC.ChaliceSuperWin);
					}
				}
				if (DamageDealer.lastPlayerDamageSource == DamageDealer.DamageSource.Ex)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(DamageDealer.lastPlayer, "ExWin");
				}
				if (this.playerMode == PlayerMode.Plane && !DamageDealer.didDamageWithNonSmallPlaneWeapon)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "SmallPlaneOnlyWin");
				}
				if (DamageDealer.lastDamageWasDLCWeapon)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(DamageDealer.lastPlayer, OnlineAchievementData.DLC.DefeatBossDLCWeapon);
				}
				if (player != null && player.stats.Loadout.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerOne))
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerOne, OnlineAchievementData.DLC.Paladin);
				}
				if (player2 != null && player2.stats.Loadout.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerTwo))
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerTwo, OnlineAchievementData.DLC.Paladin);
				}
			}
			int num = 0;
			int num2 = 0;
			List<Levels> list = new List<Levels>(Level.world1BossLevels);
			list.AddRange(Level.world2BossLevels);
			list.AddRange(Level.world3BossLevels);
			foreach (Levels levelID in list)
			{
				PlayerData.PlayerLevelDataObject levelData2 = PlayerData.Data.GetLevelData(levelID);
				if (levelData2.completed && levelData2.difficultyBeaten >= Level.Mode.Normal)
				{
					num2++;
				}
			}
			List<Levels> list2 = new List<Levels>(Level.world1BossLevels);
			list2.AddRange(Level.world2BossLevels);
			list2.AddRange(Level.world3BossLevels);
			list2.AddRange(Level.world4BossLevels);
			list2.AddRange(Level.platformingLevels);
			foreach (Levels levelID2 in list2)
			{
				PlayerData.PlayerLevelDataObject levelData3 = PlayerData.Data.GetLevelData(levelID2);
				if (levelData3.completed && levelData3.grade >= LevelScoringData.Grade.AMinus)
				{
					num++;
				}
			}
			if (this.type == Level.Type.Battle && this.CurrentLevel != Levels.Mausoleum)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "DefeatBoss");
			}
			if (this.type == Level.Type.Battle && Array.Exists<Levels>(Level.chaliceLevels, (Levels level) => this.CurrentLevel == level))
			{
				bool flag2 = false;
				if (player != null && player.stats.isChalice)
				{
					flag2 = true;
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerOne, OnlineAchievementData.DLC.DefeatBossAsChalice);
				}
				if (player2 != null && player2.stats.isChalice)
				{
					flag2 = true;
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerTwo, OnlineAchievementData.DLC.DefeatBossAsChalice);
				}
				if (flag2)
				{
					if (PlayerData.Data.CountLevelsChaliceCompleted(Level.chaliceLevels, PlayerId.PlayerOne) >= OnlineAchievementData.DLC.Triggers.DefeatXBossesAsChaliceTrigger)
					{
						OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerOne, OnlineAchievementData.DLC.DefeatXBossesAsChalice);
					}
					if (PlayerData.Data.CountLevelsChaliceCompleted(Level.chaliceLevels, PlayerId.PlayerTwo) >= OnlineAchievementData.DLC.Triggers.DefeatXBossesAsChaliceTrigger)
					{
						OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerTwo, OnlineAchievementData.DLC.DefeatXBossesAsChalice);
					}
				}
			}
			if (this.type == Level.Type.Battle && this.CurrentLevel == Levels.Graveyard)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatDevilPhase2);
			}
			if (Level.Grade == LevelScoringData.Grade.S && this.CurrentLevel != Levels.Mausoleum && !flag)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "SRank");
				if (Array.Exists<Levels>(Level.worldDLCBossLevelsWithSaltbaker, (Levels level) => this.CurrentLevel == level))
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.SRankAnyDLC);
				}
			}
			if (Array.Exists<Levels>(Level.worldDLCBossLevels, (Levels level) => this.CurrentLevel == level) && !this.defeatedMinion)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatBossNoMinions);
			}
			if (flag && PlayerData.Data.CheckLevelsCompleted(Level.kingOfGamesLevels))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatAllKOG);
			}
			OnlineManager.Instance.Interface.SetStat(PlayerId.Any, "ARanks", num);
			OnlineManager.Instance.Interface.SetStat(PlayerId.Any, "BossesDefeatedNormal", num2);
			OnlineManager.Instance.Interface.SyncAchievementsAndStats();
		}
		if (!this.isMausoleum)
		{
			InterruptingPrompt.SetCanInterrupt(false);
		}
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x0004F660 File Offset: 0x0004DA60
	private void _OnPreWin()
	{
		this.OnPreWin();
		if (this.OnPreWinEvent != null)
		{
			this.OnPreWinEvent();
		}
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x0004F680 File Offset: 0x0004DA80
	protected void _OnLose()
	{
		this._OnLevelEnd();
		this._OnPreLose();
		this.OnLose();
		if (this.OnLoseEvent != null)
		{
			this.OnLoseEvent();
		}
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		LevelEnd.Lose(this.isMausoleum, this.secretTriggered);
		if (!Level.IsTowerOfPower)
		{
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x0004F6DD File Offset: 0x0004DADD
	private void _OnPreLose()
	{
		this.OnPreLose();
		if (this.OnPreLoseEvent != null)
		{
			this.OnPreLoseEvent();
		}
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x0004F6FB File Offset: 0x0004DAFB
	private void _OnTransitionInComplete()
	{
		this.OnTransitionInComplete();
		if (this.OnTransitionInCompleteEvent != null)
		{
			this.OnTransitionInCompleteEvent();
		}
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x0004F719 File Offset: 0x0004DB19
	private void OnStartExplosions()
	{
		if (this.OnBossDeathExplosionsEvent != null)
		{
			this.OnBossDeathExplosionsEvent();
		}
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x0004F731 File Offset: 0x0004DB31
	private void OnEndExplosions()
	{
		if (this.OnBossDeathExplosionsEndEvent != null)
		{
			this.OnBossDeathExplosionsEndEvent();
		}
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x0004F749 File Offset: 0x0004DB49
	private void OnFalloffExplosions()
	{
		if (this.OnBossDeathExplosionsFalloffEvent != null)
		{
			this.OnBossDeathExplosionsFalloffEvent();
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x060012F9 RID: 4857 RVA: 0x0004F761 File Offset: 0x0004DB61
	protected virtual float LevelIntroTime
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x060012FA RID: 4858 RVA: 0x0004F768 File Offset: 0x0004DB68
	protected virtual float BossDeathTime
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x0004F76F File Offset: 0x0004DB6F
	protected virtual void PlayAnnouncerReady()
	{
		if (!this.isMausoleum)
		{
			AudioManager.Play("level_announcer_ready");
		}
		else
		{
			AudioManager.Play("level_announcer_opening_line");
		}
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x0004F795 File Offset: 0x0004DB95
	protected virtual void PlayAnnouncerBegin()
	{
		AudioManager.Play("level_announcer_begin");
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x0004F7A1 File Offset: 0x0004DBA1
	protected virtual LevelIntroAnimation CreateLevelIntro(Action callback)
	{
		return LevelIntroAnimation.Create(callback);
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x0004F7A9 File Offset: 0x0004DBA9
	protected virtual void OnLevelStart()
	{
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x0004F7AB File Offset: 0x0004DBAB
	protected virtual void OnStateChanged()
	{
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x0004F7AD File Offset: 0x0004DBAD
	protected virtual void OnWin()
	{
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x0004F7AF File Offset: 0x0004DBAF
	protected virtual void OnPreWin()
	{
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x0004F7B1 File Offset: 0x0004DBB1
	protected virtual void OnLose()
	{
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x0004F7B3 File Offset: 0x0004DBB3
	protected virtual void OnPreLose()
	{
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x0004F7B5 File Offset: 0x0004DBB5
	protected virtual void OnTransitionInComplete()
	{
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x0004F7B8 File Offset: 0x0004DBB8
	protected virtual IEnumerator knockoutSFX_cr()
	{
		if (!this.isMausoleum)
		{
			AudioManager.Play("level_announcer_knockout_bell");
			AudioManager.Play("level_announcer_knockout");
			yield return CupheadTime.WaitForSeconds(this, 1.4f);
			if (!Level.IsChessBoss && this.CurrentLevel != Levels.Saltbaker && this.CurrentLevel != Levels.Graveyard)
			{
				AudioManager.Play("level_boss_defeat_sting");
			}
		}
		else
		{
			AudioManager.Play("level_announcer_victory");
		}
		yield break;
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x0004F7D3 File Offset: 0x0004DBD3
	protected virtual void OnBossDeath()
	{
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x0004F7D8 File Offset: 0x0004DBD8
	private IEnumerator check_intros_cr()
	{
		yield return new WaitForSeconds(0.25f);
		this.CheckIntros();
		yield return null;
		yield break;
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x0004F7F4 File Offset: 0x0004DBF4
	protected virtual IEnumerator startBattle_cr()
	{
		LevelIntroAnimation introAnim = this.CreateLevelIntro(new Action(this.intro.OnReadyAnimComplete));
		yield return new WaitForSeconds(0.4f + SceneLoader.EndTransitionDelay);
		if (!Level.IsDicePalaceMain && !Level.IsTowerOfPowerMain)
		{
			this.PlayAnnouncerReady();
			AudioManager.Play("level_bell_intro");
		}
		yield return new WaitForSeconds(0.25f);
		if (this.players[0] != null)
		{
			this.players[0].PlayIntro();
		}
		if (this.players[1] != null)
		{
			if (!this.players[1].stats.isChalice)
			{
				yield return CupheadTime.WaitForSeconds(this, 0.7f);
			}
			this.players[1].PlayIntro();
		}
		yield return new WaitForSeconds(0.25f);
		this._OnTransitionInComplete();
		if (this.OnIntroEvent != null)
		{
			this.OnIntroEvent();
		}
		this.OnIntroEvent = null;
		yield return new WaitForSeconds(this.LevelIntroTime);
		if (!Level.IsDicePalaceMain && !Level.IsTowerOfPowerMain)
		{
			introAnim.Play();
			while (!this.intro.readyComplete)
			{
				yield return null;
			}
			this.PlayAnnouncerBegin();
		}
		else if (!Level.IsTowerOfPowerMain)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.5f);
		}
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (!(abstractPlayerController == null))
			{
				abstractPlayerController.LevelStart();
			}
		}
		this.Started = true;
		this._OnLevelStart();
		yield break;
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x0004F810 File Offset: 0x0004DC10
	protected virtual IEnumerator startPlatforming_cr()
	{
		PlatformingLevelIntroAnimation introAnim = PlatformingLevelIntroAnimation.Create(new Action(this.intro.OnReadyAnimComplete));
		yield return new WaitForEndOfFrame();
		if (this.players[0] != null)
		{
			this.players[0].OnPlatformingLevelAwake();
		}
		if (this.players[1] != null)
		{
			this.players[1].OnPlatformingLevelAwake();
		}
		yield return new WaitForSeconds(0.4f + SceneLoader.EndTransitionDelay);
		this._OnTransitionInComplete();
		if (this.OnIntroEvent != null)
		{
			this.OnIntroEvent();
		}
		this.OnIntroEvent = null;
		introAnim.Play();
		AudioManager.Play("level_announcer_begin");
		while (!this.intro.readyComplete)
		{
			yield return null;
		}
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (!(abstractPlayerController == null))
			{
				abstractPlayerController.LevelStart();
			}
		}
		this.Started = true;
		this._OnLevelStart();
		yield break;
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x0004F82C File Offset: 0x0004DC2C
	protected virtual IEnumerator startNonBattle_cr()
	{
		yield return new WaitForSeconds(0.4f + SceneLoader.EndTransitionDelay - 0.25f);
		if (this.playerMode == PlayerMode.Plane)
		{
			yield return new WaitForSeconds(0.5f);
			if (this.players[0] != null)
			{
				this.players[0].PlayIntro();
			}
			if (this.players[1] != null)
			{
				yield return CupheadTime.WaitForSeconds(this, 0.7f);
				this.players[1].PlayIntro();
			}
			yield return new WaitForSeconds(0.25f);
		}
		this._OnTransitionInComplete();
		if (this.OnIntroEvent != null)
		{
			this.OnIntroEvent();
		}
		this.OnIntroEvent = null;
		if (this.playerMode == PlayerMode.Plane)
		{
			yield return new WaitForSeconds(1.25f);
		}
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (!(abstractPlayerController == null))
			{
				abstractPlayerController.LevelStart();
			}
		}
		this.Started = true;
		this._OnLevelStart();
		yield break;
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x0004F848 File Offset: 0x0004DC48
	protected virtual IEnumerator bossDeath_cr()
	{
		LevelEnd.Win(this.knockoutSFX_cr(), new Action(this.OnBossDeath), new Action(this.OnStartExplosions), new Action(this.OnFalloffExplosions), new Action(this.OnEndExplosions), this.players, this.BossDeathTime, (this.type == Level.Type.Battle || this.type == Level.Type.Platforming) && (!Level.IsDicePalace || Level.IsDicePalaceMain) && !Level.IsTowerOfPower && !this.isMausoleum && !Level.IsGraveyard && !Level.IsChessBoss, this.isMausoleum, this.isDevil, this.isTowerOfPower);
		yield return null;
		yield break;
	}

	// Token: 0x04001BD5 RID: 7125
	private const int BOUND_COLLIDER_SIZE = 400;

	// Token: 0x04001BD6 RID: 7126
	private const float IRIS_NO_INTRO_DELAY = 0.4f;

	// Token: 0x04001BD7 RID: 7127
	private const float IRIS_OPEN_DELAY = 1f;

	// Token: 0x04001BD8 RID: 7128
	private const int PLAYER_DEATH_DELAY = 5;

	// Token: 0x04001BD9 RID: 7129
	public const string GENERIC_STATE_NAME = "Generic";

	// Token: 0x04001BED RID: 7149
	public static readonly Levels[] world1BossLevels = new Levels[]
	{
		Levels.Veggies,
		Levels.Slime,
		Levels.FlyingBlimp,
		Levels.Flower,
		Levels.Frogs
	};

	// Token: 0x04001BEE RID: 7150
	public static readonly Levels[] world2BossLevels = new Levels[]
	{
		Levels.Baroness,
		Levels.Clown,
		Levels.FlyingGenie,
		Levels.Dragon,
		Levels.FlyingBird
	};

	// Token: 0x04001BEF RID: 7151
	public static readonly Levels[] world3BossLevels = new Levels[]
	{
		Levels.Bee,
		Levels.Pirate,
		Levels.SallyStagePlay,
		Levels.Mouse,
		Levels.Robot,
		Levels.FlyingMermaid,
		Levels.Train
	};

	// Token: 0x04001BF0 RID: 7152
	public static readonly Levels[] world4BossLevels = new Levels[]
	{
		Levels.DicePalaceMain,
		Levels.Devil
	};

	// Token: 0x04001BF1 RID: 7153
	public static readonly Levels[] world4MiniBossLevels = new Levels[]
	{
		Levels.DicePalaceBooze,
		Levels.DicePalaceChips,
		Levels.DicePalaceCigar,
		Levels.DicePalaceDomino,
		Levels.DicePalaceEightBall,
		Levels.DicePalaceFlyingHorse,
		Levels.DicePalaceFlyingMemory,
		Levels.DicePalaceRabbit,
		Levels.DicePalaceRoulette
	};

	// Token: 0x04001BF2 RID: 7154
	public static readonly Levels[] worldDLCBossLevels = new Levels[]
	{
		Levels.Airplane,
		Levels.FlyingCowboy,
		Levels.OldMan,
		Levels.RumRunners,
		Levels.SnowCult
	};

	// Token: 0x04001BF3 RID: 7155
	public static readonly Levels[] worldDLCBossLevelsWithSaltbaker = new Levels[]
	{
		Levels.Airplane,
		Levels.FlyingCowboy,
		Levels.OldMan,
		Levels.RumRunners,
		Levels.SnowCult,
		Levels.Saltbaker
	};

	// Token: 0x04001BF4 RID: 7156
	public static readonly Levels[] platformingLevels = new Levels[]
	{
		Levels.Platforming_Level_1_1,
		Levels.Platforming_Level_1_2,
		Levels.Platforming_Level_2_1,
		Levels.Platforming_Level_2_2,
		Levels.Platforming_Level_3_1,
		Levels.Platforming_Level_3_2
	};

	// Token: 0x04001BF5 RID: 7157
	public static readonly Levels[] kingOfGamesLevels = new Levels[]
	{
		Levels.ChessPawn,
		Levels.ChessKnight,
		Levels.ChessBishop,
		Levels.ChessRook,
		Levels.ChessQueen
	};

	// Token: 0x04001BF6 RID: 7158
	public static readonly Levels[] kingOfGamesLevelsWithCastle = new Levels[]
	{
		Levels.ChessPawn,
		Levels.ChessKnight,
		Levels.ChessBishop,
		Levels.ChessRook,
		Levels.ChessQueen,
		Levels.ChessCastle
	};

	// Token: 0x04001BF7 RID: 7159
	public static readonly Levels[] chaliceLevels = new Levels[]
	{
		Levels.Veggies,
		Levels.Slime,
		Levels.FlyingBlimp,
		Levels.Flower,
		Levels.Frogs,
		Levels.Baroness,
		Levels.Clown,
		Levels.FlyingGenie,
		Levels.Dragon,
		Levels.FlyingBird,
		Levels.Bee,
		Levels.Pirate,
		Levels.SallyStagePlay,
		Levels.Mouse,
		Levels.Robot,
		Levels.FlyingMermaid,
		Levels.Train,
		Levels.DicePalaceMain,
		Levels.Devil,
		Levels.Airplane,
		Levels.FlyingCowboy,
		Levels.OldMan,
		Levels.RumRunners,
		Levels.SnowCult,
		Levels.Saltbaker
	};

	// Token: 0x04001BF8 RID: 7160
	public LevelResources LevelResources;

	// Token: 0x04001BF9 RID: 7161
	[SerializeField]
	protected Level.Type type;

	// Token: 0x04001BFA RID: 7162
	[SerializeField]
	public PlayerMode playerMode;

	// Token: 0x04001BFB RID: 7163
	[SerializeField]
	protected bool allowMultiplayer = true;

	// Token: 0x04001BFC RID: 7164
	[SerializeField]
	public bool blockChalice;

	// Token: 0x04001BFE RID: 7166
	[SerializeField]
	protected Level.IntroProperties intro;

	// Token: 0x04001BFF RID: 7167
	[SerializeField]
	protected Level.Spawns spawns;

	// Token: 0x04001C00 RID: 7168
	[SerializeField]
	protected Level.Bounds bounds = new Level.Bounds(640, 640, 360, 200);

	// Token: 0x04001C01 RID: 7169
	public int playerShadowSortingOrder;

	// Token: 0x04001C02 RID: 7170
	[SerializeField]
	protected Level.Camera camera = new Level.Camera(CupheadLevelCamera.Mode.Lerp, 640, 640, 360, 360);

	// Token: 0x04001C03 RID: 7171
	protected LevelGUI gui;

	// Token: 0x04001C04 RID: 7172
	protected LevelHUD hud;

	// Token: 0x04001C05 RID: 7173
	protected AbstractPlayerController[] players;

	// Token: 0x04001C06 RID: 7174
	protected Transform collidersRoot;

	// Token: 0x04001C07 RID: 7175
	protected Level.GoalTimes goalTimes;

	// Token: 0x04001C08 RID: 7176
	protected bool waitingForPlayerJoin;

	// Token: 0x04001C09 RID: 7177
	protected bool isMausoleum;

	// Token: 0x04001C0A RID: 7178
	protected bool isDevil;

	// Token: 0x04001C0B RID: 7179
	protected bool isTowerOfPower;

	// Token: 0x04001C0C RID: 7180
	protected bool secretTriggered;

	// Token: 0x04001C16 RID: 7190
	public int BGMPlaylistCurrent;

	// Token: 0x04001C17 RID: 7191
	private readonly Vector3 player1PlaneSpawnPos = new Vector3(-550f, 74.3f);

	// Token: 0x04001C18 RID: 7192
	private readonly Vector3 player2PlaneSpawnPos = new Vector3(-450f, -79.8f);

	// Token: 0x04001C19 RID: 7193
	private int playerDeathDelayFrames;

	// Token: 0x04001C1A RID: 7194
	private bool playerIsDead;

	// Token: 0x04001C1B RID: 7195
	private bool player1HeldJump;

	// Token: 0x04001C1C RID: 7196
	private bool player2HeldJump;

	// Token: 0x04001C1D RID: 7197
	private bool player1HeldSuper;

	// Token: 0x04001C1E RID: 7198
	private bool player2HeldSuper;

	// Token: 0x02000492 RID: 1170
	public enum Type
	{
		// Token: 0x04001C2D RID: 7213
		Battle,
		// Token: 0x04001C2E RID: 7214
		Tutorial,
		// Token: 0x04001C2F RID: 7215
		Platforming
	}

	// Token: 0x02000493 RID: 1171
	public enum Mode
	{
		// Token: 0x04001C31 RID: 7217
		Easy,
		// Token: 0x04001C32 RID: 7218
		Normal,
		// Token: 0x04001C33 RID: 7219
		Hard
	}

	// Token: 0x02000494 RID: 1172
	[Serializable]
	public class Bounds
	{
		// Token: 0x06001310 RID: 4880 RVA: 0x0004F890 File Offset: 0x0004DC90
		public Bounds()
		{
			this.left = 0;
			this.right = 0;
			this.top = 0;
			this.bottom = 0;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0004F8E8 File Offset: 0x0004DCE8
		public Bounds(int left, int right, int top, int bottom)
		{
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0004F940 File Offset: 0x0004DD40
		public void SetColliderPositions()
		{
			Rect rect = default(Rect);
			rect.xMin = (float)(-(float)this.left);
			rect.xMax = (float)this.right;
			rect.yMin = (float)(-(float)this.bottom);
			rect.yMax = (float)this.top;
			if (this.colliders.ContainsKey(Level.Bounds.Side.Left) && this.colliders[Level.Bounds.Side.Left] != null)
			{
				this.colliders[Level.Bounds.Side.Left].transform.position = new Vector2((float)(-(float)this.left - 200), rect.center.y);
			}
			if (this.colliders.ContainsKey(Level.Bounds.Side.Right) && this.colliders[Level.Bounds.Side.Right] != null)
			{
				this.colliders[Level.Bounds.Side.Right].transform.position = new Vector2((float)(this.right + 200), rect.center.y);
			}
			if (this.colliders.ContainsKey(Level.Bounds.Side.Top) && this.colliders[Level.Bounds.Side.Top] != null)
			{
				this.colliders[Level.Bounds.Side.Top].transform.position = new Vector2(rect.center.x, (float)(this.top + 200));
			}
			if (this.colliders.ContainsKey(Level.Bounds.Side.Bottom) && this.colliders[Level.Bounds.Side.Bottom] != null)
			{
				this.colliders[Level.Bounds.Side.Bottom].transform.position = new Vector2(rect.center.x, (float)(-(float)this.bottom - 200));
			}
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0004FB22 File Offset: 0x0004DF22
		public int GetValue(Level.Bounds.Side side)
		{
			switch (side)
			{
			case Level.Bounds.Side.Left:
				return this.left;
			case Level.Bounds.Side.Right:
				return this.right;
			case Level.Bounds.Side.Top:
				return this.top;
			default:
				return this.bottom;
			}
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x0004FB5C File Offset: 0x0004DF5C
		public void SetValue(Level.Bounds.Side side, int value)
		{
			switch (side)
			{
			case Level.Bounds.Side.Left:
				this.left = value;
				break;
			case Level.Bounds.Side.Right:
				this.right = value;
				break;
			case Level.Bounds.Side.Top:
				this.top = value;
				break;
			default:
				this.bottom = value;
				break;
			}
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0004FBB4 File Offset: 0x0004DFB4
		public bool GetEnabled(Level.Bounds.Side side)
		{
			switch (side)
			{
			case Level.Bounds.Side.Left:
				return this.leftEnabled;
			case Level.Bounds.Side.Right:
				return this.rightEnabled;
			case Level.Bounds.Side.Top:
				return this.topEnabled;
			default:
				return this.bottomEnabled;
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0004FBEC File Offset: 0x0004DFEC
		public void SetEnabled(Level.Bounds.Side side, bool value)
		{
			switch (side)
			{
			case Level.Bounds.Side.Left:
				this.leftEnabled = value;
				break;
			case Level.Bounds.Side.Right:
				this.rightEnabled = value;
				break;
			case Level.Bounds.Side.Top:
				this.topEnabled = value;
				break;
			default:
				this.bottomEnabled = value;
				break;
			}
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0004FC44 File Offset: 0x0004E044
		public Level.Bounds Copy()
		{
			return base.MemberwiseClone() as Level.Bounds;
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x0004FC51 File Offset: 0x0004E051
		public int Width
		{
			get
			{
				return this.left + this.right;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x0004FC60 File Offset: 0x0004E060
		public int Height
		{
			get
			{
				return this.top + this.bottom;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x0004FC70 File Offset: 0x0004E070
		public Vector2 Center
		{
			get
			{
				return new Vector2((float)(this.right - this.left), (float)(this.top - this.bottom)) / 2f;
			}
		}

		// Token: 0x04001C34 RID: 7220
		public int left;

		// Token: 0x04001C35 RID: 7221
		public int right;

		// Token: 0x04001C36 RID: 7222
		public int top;

		// Token: 0x04001C37 RID: 7223
		public int bottom;

		// Token: 0x04001C38 RID: 7224
		public bool topEnabled = true;

		// Token: 0x04001C39 RID: 7225
		public bool bottomEnabled = true;

		// Token: 0x04001C3A RID: 7226
		public bool leftEnabled = true;

		// Token: 0x04001C3B RID: 7227
		public bool rightEnabled = true;

		// Token: 0x04001C3C RID: 7228
		public Dictionary<Level.Bounds.Side, BoxCollider2D> colliders = new Dictionary<Level.Bounds.Side, BoxCollider2D>();

		// Token: 0x02000495 RID: 1173
		public enum Side
		{
			// Token: 0x04001C3E RID: 7230
			Left,
			// Token: 0x04001C3F RID: 7231
			Right,
			// Token: 0x04001C40 RID: 7232
			Top,
			// Token: 0x04001C41 RID: 7233
			Bottom
		}
	}

	// Token: 0x02000496 RID: 1174
	[Serializable]
	public class Spawns
	{
		// Token: 0x17000303 RID: 771
		public Vector2 this[int i]
		{
			get
			{
				if (i == 0)
				{
					return this.playerOne;
				}
				if (i == 1)
				{
					return this.playerTwo;
				}
				if (i == 2)
				{
					return this.playerOneSingle;
				}
				global::Debug.LogError("Spawn index '" + i + "' not in range", null);
				return Vector2.zero;
			}
		}

		// Token: 0x04001C42 RID: 7234
		public Vector2 playerOne = new Vector2(-460f, 0f);

		// Token: 0x04001C43 RID: 7235
		public Vector2 playerTwo = new Vector2(-580f, 0f);

		// Token: 0x04001C44 RID: 7236
		public Vector2 playerOneSingle = new Vector2(-520f, 0f);
	}

	// Token: 0x02000497 RID: 1175
	[Serializable]
	public class Camera
	{
		// Token: 0x0600131D RID: 4893 RVA: 0x0004FD58 File Offset: 0x0004E158
		public Camera(CupheadLevelCamera.Mode mode, int left, int right, int top, int bottom)
		{
			this.mode = mode;
			this.bounds = new Level.Bounds(left, right, top, bottom);
		}

		// Token: 0x04001C45 RID: 7237
		public CupheadLevelCamera.Mode mode = CupheadLevelCamera.Mode.Relative;

		// Token: 0x04001C46 RID: 7238
		[Space(10f)]
		[Range(0.5f, 2f)]
		public float zoom = 1f;

		// Token: 0x04001C47 RID: 7239
		[Space(10f)]
		public bool moveX;

		// Token: 0x04001C48 RID: 7240
		public bool moveY;

		// Token: 0x04001C49 RID: 7241
		public bool stabilizeY;

		// Token: 0x04001C4A RID: 7242
		public float stabilizePaddingTop = 50f;

		// Token: 0x04001C4B RID: 7243
		public float stabilizePaddingBottom = 100f;

		// Token: 0x04001C4C RID: 7244
		[Space(10f)]
		public bool colliders;

		// Token: 0x04001C4D RID: 7245
		[Space(10f)]
		public Level.Bounds bounds;

		// Token: 0x04001C4E RID: 7246
		[HideInInspector]
		public VectorPath path;

		// Token: 0x04001C4F RID: 7247
		public bool pathMovesOnlyForward;
	}

	// Token: 0x02000498 RID: 1176
	public class GoalTimes
	{
		// Token: 0x0600131E RID: 4894 RVA: 0x0004FDAB File Offset: 0x0004E1AB
		public GoalTimes(float easy, float normal, float hard)
		{
			this.easy = easy;
			this.normal = normal;
			this.hard = hard;
		}

		// Token: 0x04001C50 RID: 7248
		public readonly float easy;

		// Token: 0x04001C51 RID: 7249
		public readonly float normal;

		// Token: 0x04001C52 RID: 7250
		public readonly float hard;
	}

	// Token: 0x02000499 RID: 1177
	[Serializable]
	public class IntroProperties
	{
		// Token: 0x06001320 RID: 4896 RVA: 0x0004FDD0 File Offset: 0x0004E1D0
		public void OnIntroAnimComplete()
		{
			this.introComplete = true;
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0004FDD9 File Offset: 0x0004E1D9
		public void OnReadyAnimComplete()
		{
			this.readyComplete = true;
		}

		// Token: 0x04001C53 RID: 7251
		[NonSerialized]
		public bool introComplete;

		// Token: 0x04001C54 RID: 7252
		[NonSerialized]
		public bool readyComplete;
	}

	// Token: 0x0200049A RID: 1178
	public class Timeline
	{
		// Token: 0x06001322 RID: 4898 RVA: 0x0004FDE2 File Offset: 0x0004E1E2
		public Timeline()
		{
			this.health = 0f;
			this.damage = 0f;
			this.cuphead = -1f;
			this.mugman = -1f;
			this.events = new List<Level.Timeline.Event>();
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06001323 RID: 4899 RVA: 0x0004FE21 File Offset: 0x0004E221
		// (set) Token: 0x06001324 RID: 4900 RVA: 0x0004FE29 File Offset: 0x0004E229
		public float damage { get; private set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06001325 RID: 4901 RVA: 0x0004FE32 File Offset: 0x0004E232
		// (set) Token: 0x06001326 RID: 4902 RVA: 0x0004FE3A File Offset: 0x0004E23A
		public List<Level.Timeline.Event> events { get; private set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06001327 RID: 4903 RVA: 0x0004FE43 File Offset: 0x0004E243
		// (set) Token: 0x06001328 RID: 4904 RVA: 0x0004FE4B File Offset: 0x0004E24B
		public float cuphead { get; private set; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06001329 RID: 4905 RVA: 0x0004FE54 File Offset: 0x0004E254
		// (set) Token: 0x0600132A RID: 4906 RVA: 0x0004FE5C File Offset: 0x0004E25C
		public float mugman { get; private set; }

		// Token: 0x0600132B RID: 4907 RVA: 0x0004FE68 File Offset: 0x0004E268
		public int GetHealthOfLastEvent()
		{
			float num = 1f;
			for (int i = 0; i < this.events.Count; i++)
			{
				if (this.events[i].percentage < num)
				{
					num = this.events[i].percentage;
				}
			}
			return (int)(this.health * (1f - num));
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0004FED5 File Offset: 0x0004E2D5
		public void DealDamage(float damage)
		{
			this.damage += damage;
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0004FEE8 File Offset: 0x0004E2E8
		public void OnPlayerDeath(PlayerId playerId)
		{
			if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
			{
				if (PlayerManager.player1IsMugman)
				{
					this.mugman = this.damage;
				}
				else
				{
					this.cuphead = this.damage;
				}
			}
			else if (PlayerManager.player1IsMugman)
			{
				this.cuphead = this.damage;
			}
			else
			{
				this.mugman = this.damage;
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0004FF60 File Offset: 0x0004E360
		public void OnPlayerRevive(PlayerId playerId)
		{
			if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
			{
				if (PlayerManager.player1IsMugman)
				{
					this.mugman = -1f;
				}
				else
				{
					this.cuphead = -1f;
				}
			}
			else if (PlayerManager.player1IsMugman)
			{
				this.cuphead = -1f;
			}
			else
			{
				this.mugman = -1f;
			}
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0004FFD4 File Offset: 0x0004E3D4
		public void SetPlayerDamage(PlayerId playerId, float value)
		{
			if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
			{
				if (PlayerManager.player1IsMugman)
				{
					this.mugman = value;
				}
				else
				{
					this.cuphead = value;
				}
			}
			else if (PlayerManager.player1IsMugman)
			{
				this.cuphead = value;
			}
			else
			{
				this.mugman = value;
			}
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00050037 File Offset: 0x0004E437
		public void AddEvent(Level.Timeline.Event e)
		{
			this.events.Add(e);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00050048 File Offset: 0x0004E448
		public void AddEventAtHealth(string eventName, int targetHealth)
		{
			float percentage = 1f - (float)targetHealth / this.health;
			this.AddEvent(new Level.Timeline.Event(eventName, percentage));
		}

		// Token: 0x04001C55 RID: 7253
		public float health;

		// Token: 0x0200049B RID: 1179
		public class Event
		{
			// Token: 0x06001332 RID: 4914 RVA: 0x00050072 File Offset: 0x0004E472
			public Event(string name, float percentage)
			{
				this.name = name;
				this.percentage = percentage;
			}

			// Token: 0x17000308 RID: 776
			// (get) Token: 0x06001333 RID: 4915 RVA: 0x00050088 File Offset: 0x0004E488
			// (set) Token: 0x06001334 RID: 4916 RVA: 0x00050090 File Offset: 0x0004E490
			public string name { get; private set; }

			// Token: 0x17000309 RID: 777
			// (get) Token: 0x06001335 RID: 4917 RVA: 0x00050099 File Offset: 0x0004E499
			// (set) Token: 0x06001336 RID: 4918 RVA: 0x000500A1 File Offset: 0x0004E4A1
			public float percentage { get; private set; }
		}
	}
}
