using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000969 RID: 2409
public class Map : AbstractMonoBehaviour
{
	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x0600381F RID: 14367 RVA: 0x00201B53 File Offset: 0x001FFF53
	// (set) Token: 0x06003820 RID: 14368 RVA: 0x00201B5A File Offset: 0x001FFF5A
	public static Map Current { get; private set; }

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06003821 RID: 14369 RVA: 0x00201B62 File Offset: 0x001FFF62
	// (set) Token: 0x06003822 RID: 14370 RVA: 0x00201B6A File Offset: 0x001FFF6A
	public Map.State CurrentState { get; set; }

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x06003823 RID: 14371 RVA: 0x00201B73 File Offset: 0x001FFF73
	// (set) Token: 0x06003824 RID: 14372 RVA: 0x00201B7B File Offset: 0x001FFF7B
	public MapPlayerController[] players { get; private set; }

	// Token: 0x06003825 RID: 14373 RVA: 0x00201B84 File Offset: 0x001FFF84
	protected override void Awake()
	{
		base.Awake();
		Map.Current = this;
		Cuphead.Init(false);
		Level.ResetBossesHub();
		Level.IsGraveyard = false;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
		this.scene = EnumUtils.Parse<Scenes>(SceneManager.GetActiveScene().name);
		PlayerData.Data.CurrentMap = this.scene;
		this.CreateUI();
		this.CreatePlayers();
		this.ui.Init(this.players);
		this.cupheadMapCamera = UnityEngine.Object.FindObjectOfType<CupheadMapCamera>();
		this.cupheadMapCamera.Init(this.cameraProperties);
		CupheadTime.SetAll(1f);
		SceneLoader.OnLoaderCompleteEvent += this.SelectMusic;
	}

	// Token: 0x06003826 RID: 14374 RVA: 0x00201C57 File Offset: 0x00200057
	private void Start()
	{
		if (PlatformHelper.ManuallyRefreshDLCAvailability)
		{
			DLCManager.CheckInstallationStatusChanged();
		}
		AudioManager.PlayLoop(string.Empty);
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06003827 RID: 14375 RVA: 0x00201C80 File Offset: 0x00200080
	private void OnDestroy()
	{
		SceneLoader.OnLoaderCompleteEvent -= this.SelectMusic;
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
		this.MapResources = null;
		this.cameraProperties = null;
		this.firstNode = null;
		this.entryPoints = null;
		this.ui = null;
		this.cupheadMapCamera = null;
		this.players = null;
		if (Map.Current == this)
		{
			Map.Current = null;
		}
	}

	// Token: 0x06003828 RID: 14376 RVA: 0x00201D0C File Offset: 0x0020010C
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector3(this.cameraProperties.bounds.left, this.cameraProperties.bounds.top), new Vector3(this.cameraProperties.bounds.left, this.cameraProperties.bounds.bottom));
		Gizmos.DrawLine(new Vector3(this.cameraProperties.bounds.right, this.cameraProperties.bounds.top), new Vector3(this.cameraProperties.bounds.right, this.cameraProperties.bounds.bottom));
		Gizmos.DrawLine(new Vector3(this.cameraProperties.bounds.right, this.cameraProperties.bounds.top), new Vector3(this.cameraProperties.bounds.left, this.cameraProperties.bounds.top));
		Gizmos.DrawLine(new Vector3(this.cameraProperties.bounds.right, this.cameraProperties.bounds.bottom), new Vector3(this.cameraProperties.bounds.left, this.cameraProperties.bounds.bottom));
	}

	// Token: 0x06003829 RID: 14377 RVA: 0x00201E5B File Offset: 0x0020025B
	private void CreateUI()
	{
		this.ui = UnityEngine.Object.FindObjectOfType<MapUI>();
		if (this.ui == null)
		{
			this.ui = MapUI.Create();
		}
	}

	// Token: 0x0600382A RID: 14378 RVA: 0x00201E84 File Offset: 0x00200284
	private void CreatePlayers()
	{
		if (!PlayerData.Data.CurrentMapData.sessionStarted)
		{
			PlayerData.Data.CurrentMapData.sessionStarted = true;
			PlayerData.Data.CurrentMapData.playerOnePosition = this.firstNode.transform.position + this.firstNode.returnPositions.playerOne;
			PlayerData.Data.CurrentMapData.playerTwoPosition = this.firstNode.transform.position + this.firstNode.returnPositions.playerTwo;
			if (!PlayerManager.Multiplayer)
			{
				PlayerData.Data.CurrentMapData.playerOnePosition = this.firstNode.transform.position + this.firstNode.returnPositions.singlePlayer;
			}
		}
		else if (PlayerData.Data.CurrentMapData.enteringFrom != PlayerData.MapData.EntryMethod.None)
		{
			this.entryPoints[(int)PlayerData.Data.CurrentMapData.enteringFrom].SetPlayerReturnPos();
			PlayerData.Data.CurrentMapData.enteringFrom = PlayerData.MapData.EntryMethod.None;
		}
		PlayerData.SaveCurrentFile();
		MapPlayerPose pose = MapPlayerPose.Default;
		if (Level.Won && Level.PreviousLevel != Levels.Saltbaker)
		{
			pose = MapPlayerPose.Won;
		}
		this.players = new MapPlayerController[2];
		this.players[0] = MapPlayerController.Create(PlayerId.PlayerOne, new MapPlayerController.InitObject(PlayerData.Data.CurrentMapData.playerOnePosition, pose));
		if (PlayerManager.Multiplayer)
		{
			this.players[1] = MapPlayerController.Create(PlayerId.PlayerTwo, new MapPlayerController.InitObject(PlayerData.Data.CurrentMapData.playerTwoPosition, pose));
		}
	}

	// Token: 0x0600382B RID: 14379 RVA: 0x00202048 File Offset: 0x00200448
	protected virtual void OnPlayerJoined(PlayerId playerId)
	{
		if (playerId == PlayerId.PlayerTwo)
		{
			Vector3 position = this.players[0].transform.position;
			Vector3 v = position + new Vector3(0.05f, 0.05f, 0f);
			LayerMask layerMask = -257;
			for (int i = 0; i < 10; i++)
			{
				float num = (float)(36 * -(float)i + 150);
				Vector2 vector = new Vector2(Mathf.Cos(0.017453292f * num), Mathf.Sin(0.017453292f * num));
				if (!(Physics2D.CircleCast(position, 0.2f, vector, 0.7f, layerMask.value).collider != null))
				{
					v = position + vector * 0.7f;
					break;
				}
			}
			this.players[1] = MapPlayerController.Create(PlayerId.PlayerTwo, new MapPlayerController.InitObject(v, MapPlayerPose.Joined));
			this.players[1].animationController.spriteRenderer.sortingOrder = this.players[0].animationController.spriteRenderer.sortingOrder;
			LevelNewPlayerGUI.Current.Init();
			this.SetRichPresence();
		}
		this.CheckMusic(true);
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x0020218C File Offset: 0x0020058C
	protected virtual void OnPlayerLeave(PlayerId playerId)
	{
		if (playerId == PlayerId.PlayerTwo)
		{
			this.players[1].OnLeave();
		}
		this.CheckMusic(true);
	}

	// Token: 0x0600382D RID: 14381 RVA: 0x002021A9 File Offset: 0x002005A9
	protected virtual void SelectMusic()
	{
		this.currentMusic = -1;
		this.CheckMusic(false);
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x002021B9 File Offset: 0x002005B9
	public void OnCloseEquipMenu()
	{
		this.CheckMusic(true);
	}

	// Token: 0x0600382F RID: 14383 RVA: 0x002021C2 File Offset: 0x002005C2
	public void OnNPCChangeMusic()
	{
		this.CheckMusic(true);
	}

	// Token: 0x06003830 RID: 14384 RVA: 0x002021CC File Offset: 0x002005CC
	protected virtual void CheckMusic(bool isRecheck)
	{
		int num = this.currentMusic;
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne);
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout2 = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo);
		if ((playerLoadout.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1) || (PlayerManager.Multiplayer && playerLoadout2.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1))
		{
			if ((playerLoadout.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerOne)) || (PlayerManager.Multiplayer && playerLoadout2.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerTwo)))
			{
				num = ((!PlayerData.Data.pianoAudioEnabled) ? 2 : 4);
			}
			else
			{
				num = ((!PlayerData.Data.pianoAudioEnabled) ? 1 : 3);
			}
		}
		else
		{
			num = ((!PlayerData.Data.pianoAudioEnabled) ? -1 : 0);
		}
		if (!isRecheck || num != this.currentMusic)
		{
			this.currentMusic = num;
			if (this.currentMusic == -1)
			{
				AudioManager.PlayBGM();
			}
			else
			{
				AudioManager.StartBGMAlternate(this.currentMusic);
			}
		}
	}

	// Token: 0x06003831 RID: 14385 RVA: 0x0020230D File Offset: 0x0020070D
	public void OnLoadLevel()
	{
	}

	// Token: 0x06003832 RID: 14386 RVA: 0x0020230F File Offset: 0x0020070F
	public void OnLoadShop()
	{
	}

	// Token: 0x06003833 RID: 14387 RVA: 0x00202314 File Offset: 0x00200714
	private IEnumerator start_cr()
	{
		this.SetRichPresence();
		Level.ResetBossesHub();
		if (Level.Won && Level.PreviousLevel != Levels.Saltbaker)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.5f);
			bool longPlayerAnimation = true;
			bool cameraMoved = false;
			Vector3 cameraStartPos = this.cupheadMapCamera.transform.position;
			if (AbstractMapLevelDependentEntity.RegisteredEntities != null)
			{
				while (AbstractMapLevelDependentEntity.RegisteredEntities.Count > 0)
				{
					yield return null;
					this.CurrentState = Map.State.Event;
					AbstractMapLevelDependentEntity entity = AbstractMapLevelDependentEntity.RegisteredEntities[0];
					foreach (AbstractMapLevelDependentEntity abstractMapLevelDependentEntity in AbstractMapLevelDependentEntity.RegisteredEntities)
					{
						if (!abstractMapLevelDependentEntity.panCamera)
						{
							entity = abstractMapLevelDependentEntity;
							break;
						}
						if (!(abstractMapLevelDependentEntity == entity))
						{
							float num = Vector2.Distance(this.cupheadMapCamera.transform.position, abstractMapLevelDependentEntity.CameraPosition);
							if (num < Vector2.Distance(this.cupheadMapCamera.transform.position, entity.CameraPosition))
							{
								entity = abstractMapLevelDependentEntity;
							}
						}
					}
					AbstractMapLevelDependentEntity.RegisteredEntities.Remove(entity);
					if (entity.panCamera)
					{
						yield return this.cupheadMapCamera.MoveToPosition(entity.CameraPosition, 0.5f, 0.9f);
						cameraMoved = true;
					}
					entity.MapMeetCondition();
					while (entity.CurrentState != AbstractMapLevelDependentEntity.State.Complete)
					{
						yield return null;
					}
					yield return CupheadTime.WaitForSeconds(this, 0.25f);
					longPlayerAnimation = false;
				}
				if (cameraMoved)
				{
					this.cupheadMapCamera.MoveToPosition(cameraStartPos, 0.75f, 1f);
				}
			}
			if (!PlayerManager.playerWasChalice[0] && (!PlayerManager.Multiplayer || !PlayerManager.playerWasChalice[1]))
			{
				yield return CupheadTime.WaitForSeconds(this, (!longPlayerAnimation) ? 1f : 2.5f);
				this.players[0].OnWinComplete();
				if (PlayerManager.Multiplayer)
				{
					this.players[1].OnWinComplete();
				}
			}
			else
			{
				if (PlayerManager.playerWasChalice[0])
				{
					this.players[0].OnWinComplete();
				}
				if (PlayerManager.Multiplayer && PlayerManager.playerWasChalice[1])
				{
					this.players[1].OnWinComplete();
				}
				yield return CupheadTime.WaitForSeconds(this, 1f);
				if (!PlayerManager.playerWasChalice[0])
				{
					this.players[0].OnWinComplete();
				}
				if (PlayerManager.Multiplayer && !PlayerManager.playerWasChalice[1])
				{
					this.players[1].OnWinComplete();
				}
			}
			if (!Level.PreviouslyWon || Level.PreviousDifficulty < Level.Mode.Normal || Level.PreviousLevel == Levels.Mausoleum)
			{
				if (!Level.IsDicePalace && !Level.IsDicePalaceMain && Level.PreviousLevel != Levels.Devil && Level.PreviousLevel != Levels.DicePalaceMain && Level.PreviousLevel != Levels.Mausoleum && Level.PreviousLevelType == Level.Type.Battle)
				{
					if (Array.IndexOf<Levels>(Level.worldDLCBossLevels, Level.PreviousLevel) >= 0)
					{
						if (Level.Difficulty == Level.Mode.Easy && !PlayerData.Data.hasBeatenAnyDLCBossOnEasy && !PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal))
						{
							MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.SimpleIngredient);
							PlayerData.Data.hasBeatenAnyDLCBossOnEasy = true;
							PlayerData.SaveCurrentFile();
						}
					}
					else if (Level.Difficulty == Level.Mode.Easy && !PlayerData.Data.hasBeatenAnyBossOnEasy && (!PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Normal) || !PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Normal) || !PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Normal)))
					{
						MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.KingDice);
						PlayerData.Data.hasBeatenAnyBossOnEasy = true;
						PlayerData.SaveCurrentFile();
					}
					if (Level.Difficulty >= Level.Mode.Normal)
					{
						if (Level.PreviousLevel == Levels.Airplane)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.AirplaneIngredient);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
						else if (Level.PreviousLevel == Levels.RumRunners)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.RumIngredient);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
						else if (Level.PreviousLevel == Levels.OldMan)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.OldManIngredient);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
						else if (Level.PreviousLevel == Levels.SnowCult)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.SnowIngredient);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
						else if (Level.PreviousLevel == Levels.FlyingCowboy)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.CowboyIngredient);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
						else if (Level.PreviousLevel == Levels.Graveyard)
						{
							GameObject.Find("GhostDetective").GetComponent<MapNPCGraveyardGhost>().TalkAfterPlayerGotCharm();
						}
						else if (Level.PreviousLevel != Levels.Saltbaker)
						{
							MapEventNotification.Current.ShowEvent(MapEventNotification.Type.SoulContract);
							while (MapEventNotification.Current.showing)
							{
								yield return null;
							}
							longPlayerAnimation = false;
							yield return CupheadTime.WaitForSeconds(this, 0.25f);
						}
					}
					if (PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal) && Array.IndexOf<Levels>(Level.worldDLCBossLevels, Level.PreviousLevel) >= 0)
					{
						MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.BackToKitchen);
					}
					int bossCounter = 0;
					for (int i = 0; i < Level.chaliceLevels.Length; i++)
					{
						if (PlayerData.Data.GetLevelData(Level.chaliceLevels[i]).completedAsChaliceP1)
						{
							bossCounter++;
						}
					}
				}
				else if (Level.SuperUnlocked)
				{
					MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Super);
					if (!PlayerData.Data.hasUnlockedFirstSuper)
					{
						MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Mausoleum);
						PlayerData.Data.hasUnlockedFirstSuper = true;
						PlayerData.SaveCurrentFile();
					}
					longPlayerAnimation = false;
				}
			}
			while (MapEventNotification.Current && MapEventNotification.Current.showing)
			{
				yield return null;
			}
		}
		if (DLCManager.showAvailabilityPrompt)
		{
			yield return CupheadTime.WaitForSeconds(this, (!Level.Won) ? 1.5f : 0.5f);
			DLCManager.ResetAvailabilityPrompt();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.DLCAvailable);
		}
		this.CurrentState = Map.State.Ready;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		InterruptingPrompt.SetCanInterrupt(true);
		Level.ResetPreviousLevelInfo();
		yield break;
	}

	// Token: 0x06003834 RID: 14388 RVA: 0x0020232F File Offset: 0x0020072F
	private void SetRichPresence()
	{
		OnlineManager.Instance.Interface.SetStat(PlayerId.Any, "WorldMap", SceneLoader.SceneName);
		OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Exploring", true);
	}

	// Token: 0x04003FFF RID: 16383
	public MapResources MapResources;

	// Token: 0x04004000 RID: 16384
	[SerializeField]
	private Map.Camera cameraProperties;

	// Token: 0x04004001 RID: 16385
	[Space(10f)]
	[SerializeField]
	private AbstractMapInteractiveEntity firstNode;

	// Token: 0x04004002 RID: 16386
	[SerializeField]
	private AbstractMapInteractiveEntity[] entryPoints;

	// Token: 0x04004003 RID: 16387
	private MapUI ui;

	// Token: 0x04004004 RID: 16388
	private Scenes scene;

	// Token: 0x04004005 RID: 16389
	private CupheadMapCamera cupheadMapCamera;

	// Token: 0x04004008 RID: 16392
	public Levels level;

	// Token: 0x04004009 RID: 16393
	public List<CoinPositionAndID> LevelCoinsIDs = new List<CoinPositionAndID>();

	// Token: 0x0400400A RID: 16394
	protected int currentMusic;

	// Token: 0x0200096A RID: 2410
	public enum State
	{
		// Token: 0x0400400C RID: 16396
		Starting,
		// Token: 0x0400400D RID: 16397
		Ready,
		// Token: 0x0400400E RID: 16398
		Event,
		// Token: 0x0400400F RID: 16399
		Exiting,
		// Token: 0x04004010 RID: 16400
		Graveyard
	}

	// Token: 0x0200096B RID: 2411
	[Serializable]
	public class Camera
	{
		// Token: 0x04004011 RID: 16401
		public bool moveX = true;

		// Token: 0x04004012 RID: 16402
		public bool moveY = true;

		// Token: 0x04004013 RID: 16403
		public CupheadBounds bounds = new CupheadBounds(-6.4f, 6.4f, 3.6f, -3.6f);
	}
}
