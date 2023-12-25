using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000ACB RID: 2763
public static class PlayerManager
{
	// Token: 0x140000AA RID: 170
	// (add) Token: 0x0600424E RID: 16974 RVA: 0x0023C568 File Offset: 0x0023A968
	// (remove) Token: 0x0600424F RID: 16975 RVA: 0x0023C59C File Offset: 0x0023A99C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event PlayerManager.PlayerChangedDelegate OnPlayerJoinedEvent;

	// Token: 0x140000AB RID: 171
	// (add) Token: 0x06004250 RID: 16976 RVA: 0x0023C5D0 File Offset: 0x0023A9D0
	// (remove) Token: 0x06004251 RID: 16977 RVA: 0x0023C604 File Offset: 0x0023AA04
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event PlayerManager.PlayerChangedDelegate OnPlayerLeaveEvent;

	// Token: 0x140000AC RID: 172
	// (add) Token: 0x06004252 RID: 16978 RVA: 0x0023C638 File Offset: 0x0023AA38
	// (remove) Token: 0x06004253 RID: 16979 RVA: 0x0023C66C File Offset: 0x0023AA6C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnControlsChanged;

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06004254 RID: 16980 RVA: 0x0023C6A0 File Offset: 0x0023AAA0
	public static bool ShouldShowJoinPrompt
	{
		get
		{
			return PlayerManager.playerSlots[1].joinState == PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed;
		}
	}

	// Token: 0x06004255 RID: 16981 RVA: 0x0023C6B4 File Offset: 0x0023AAB4
	public static void Awake()
	{
		PlayerManager.Multiplayer = false;
		PlayerManager.players = new Dictionary<int, AbstractPlayerController>();
		PlayerManager.players.Add(0, null);
		PlayerManager.players.Add(1, null);
		PlayerManager.playerInputs = new Dictionary<int, Player>();
		PlayerManager.playerInputs.Add(0, ReInput.players.GetPlayer(0));
		PlayerManager.playerInputs.Add(1, ReInput.players.GetPlayer(1));
	}

	// Token: 0x06004256 RID: 16982 RVA: 0x0023C720 File Offset: 0x0023AB20
	public static void Init()
	{
		OnlineInterface @interface = OnlineManager.Instance.Interface;
		if (PlayerManager.<>f__mg$cache0 == null)
		{
			PlayerManager.<>f__mg$cache0 = new SignInEventHandler(PlayerManager.OnUserSignedIn);
		}
		@interface.OnUserSignedIn += PlayerManager.<>f__mg$cache0;
		OnlineInterface interface2 = OnlineManager.Instance.Interface;
		if (PlayerManager.<>f__mg$cache1 == null)
		{
			PlayerManager.<>f__mg$cache1 = new SignOutEventHandler(PlayerManager.OnUserSignedOut);
		}
		interface2.OnUserSignedOut += PlayerManager.<>f__mg$cache1;
		if (PlayerManager.<>f__mg$cache2 == null)
		{
			PlayerManager.<>f__mg$cache2 = new Action<ControllerStatusChangedEventArgs>(PlayerManager.OnControllerConnected);
		}
		ReInput.ControllerConnectedEvent += PlayerManager.<>f__mg$cache2;
		if (PlayerManager.<>f__mg$cache3 == null)
		{
			PlayerManager.<>f__mg$cache3 = new Action<ControllerStatusChangedEventArgs>(PlayerManager.OnControllerDisconnected);
		}
		ReInput.ControllerDisconnectedEvent += PlayerManager.<>f__mg$cache3;
		PlmInterface interface3 = PlmManager.Instance.Interface;
		if (PlayerManager.<>f__mg$cache4 == null)
		{
			PlayerManager.<>f__mg$cache4 = new OnUnconstrainedHandler(PlayerManager.OnUnconstrained);
		}
		interface3.OnUnconstrained += PlayerManager.<>f__mg$cache4;
		PlmInterface interface4 = PlmManager.Instance.Interface;
		if (PlayerManager.<>f__mg$cache5 == null)
		{
			PlayerManager.<>f__mg$cache5 = new OnResumeHandler(PlayerManager.OnResume);
		}
		interface4.OnResume += PlayerManager.<>f__mg$cache5;
		PlmInterface interface5 = PlmManager.Instance.Interface;
		if (PlayerManager.<>f__mg$cache6 == null)
		{
			PlayerManager.<>f__mg$cache6 = new OnSuspendHandler(PlayerManager.OnSuspend);
		}
		interface5.OnSuspend += PlayerManager.<>f__mg$cache6;
	}

	// Token: 0x06004257 RID: 16983 RVA: 0x0023C850 File Offset: 0x0023AC50
	public static void SetPlayerCanJoin(PlayerId player, bool canJoin, bool promptBeforeJoin)
	{
		PlayerManager.PlayerSlot playerSlot = (player != PlayerId.PlayerOne) ? PlayerManager.playerSlots[1] : PlayerManager.playerSlots[0];
		playerSlot.canJoin = canJoin;
		playerSlot.promptBeforeJoin = promptBeforeJoin;
		if (!canJoin && playerSlot.joinState == PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed)
		{
			playerSlot.joinState = PlayerManager.PlayerSlot.JoinState.NotJoining;
		}
	}

	// Token: 0x06004258 RID: 16984 RVA: 0x0023C8A0 File Offset: 0x0023ACA0
	public static void ClearJoinPrompt()
	{
		for (int i = 0; i < 2; i++)
		{
			if (PlayerManager.playerSlots[i].joinState == PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed)
			{
				PlayerManager.playerSlots[i].joinState = PlayerManager.PlayerSlot.JoinState.NotJoining;
			}
		}
	}

	// Token: 0x06004259 RID: 16985 RVA: 0x0023C8E0 File Offset: 0x0023ACE0
	public static void SetPlayerCanSwitch(PlayerId player, bool canSwitch)
	{
		PlayerManager.PlayerSlot playerSlot = (player != PlayerId.PlayerOne) ? PlayerManager.playerSlots[1] : PlayerManager.playerSlots[0];
		playerSlot.canSwitch = canSwitch;
		playerSlot.requestedSwitch = false;
	}

	// Token: 0x0600425A RID: 16986 RVA: 0x0023C918 File Offset: 0x0023AD18
	public static void PlayerLeave(PlayerId player)
	{
		PlayerManager.PlayerSlot playerSlot = (player != PlayerId.PlayerOne) ? PlayerManager.playerSlots[1] : PlayerManager.playerSlots[0];
		playerSlot.joinState = PlayerManager.PlayerSlot.JoinState.Leaving;
	}

	// Token: 0x0600425B RID: 16987 RVA: 0x0023C946 File Offset: 0x0023AD46
	public static void OnChaliceCharmUnequipped(PlayerId player)
	{
		PlayerManager.playerWasChalice[(int)player] = false;
	}

	// Token: 0x0600425C RID: 16988 RVA: 0x0023C950 File Offset: 0x0023AD50
	private static void OnControllerConnected(ControllerStatusChangedEventArgs args)
	{
	}

	// Token: 0x0600425D RID: 16989 RVA: 0x0023C954 File Offset: 0x0023AD54
	public static void Update()
	{
		if (InterruptingPrompt.IsInterrupting())
		{
			for (int i = 0; i < PlayerManager.playerSlots.Length; i++)
			{
				if (PlayerManager.playerSlots[i].joinState == PlayerManager.PlayerSlot.JoinState.Joined && PlayerManager.playerSlots[i].controllerState == PlayerManager.PlayerSlot.ControllerState.ReconnectPromptDisplayed)
				{
					PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
					Joystick joystick = CupheadInput.CheckForUnconnectedControllerPress();
					Player playerInput = PlayerManager.GetPlayerInput(playerId);
					if (joystick != null)
					{
						PlayerManager.playerSlots[i].controllerState = PlayerManager.PlayerSlot.ControllerState.UsingController;
						PlayerManager.playerSlots[i].controllerId = joystick.id;
						PlayerManager.playerSlots[i].controllerDisconnectFromPlm = false;
						PlayerManager.playerSlots[i].lastController = ControllerType.Joystick;
						playerInput.controllers.AddController(joystick, true);
						ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)playerId].id, ControllerType.Joystick, PlayerManager.playerSlots[i].controllerId);
						PlayerManager.ControlsChanged();
					}
					if (!PlatformHelper.IsConsole && playerInput.GetAnyButtonDown())
					{
						PlayerManager.playerSlots[i].controllerState = PlayerManager.PlayerSlot.ControllerState.NoController;
						PlayerManager.playerSlots[i].controllerDisconnectFromPlm = false;
						PlayerManager.ControlsChanged();
						PlayerManager.playerSlots[i].lastController = ControllerType.Keyboard;
					}
				}
			}
			return;
		}
		for (int j = 0; j < PlayerManager.playerSlots.Length; j++)
		{
			if (PlayerManager.playerSlots[j].canJoin && PlayerManager.playerSlots[j].joinState != PlayerManager.PlayerSlot.JoinState.Joined)
			{
				PlayerId playerId2 = (j != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				bool flag = false;
				Joystick joystick2 = CupheadInput.CheckForUnconnectedControllerPress();
				Player playerInput2 = PlayerManager.GetPlayerInput(playerId2);
				if (joystick2 != null)
				{
					flag = true;
					PlayerManager.playerSlots[j].controllerState = PlayerManager.PlayerSlot.ControllerState.UsingController;
					PlayerManager.playerSlots[j].controllerId = joystick2.id;
				}
				else if (!PlatformHelper.IsConsole && ((!(SceneManager.GetActiveScene().name == "scene_title")) ? playerInput2.GetAnyButtonDown() : (playerInput2.controllers.Keyboard.GetAnyButtonDown() && PlayerManager.playerSlots[j].joinState == PlayerManager.PlayerSlot.JoinState.NotJoining)))
				{
					flag = true;
					PlayerManager.playerSlots[j].controllerState = PlayerManager.PlayerSlot.ControllerState.NoController;
				}
				if (flag)
				{
					if (PlayerManager.playerSlots[j].joinState == PlayerManager.PlayerSlot.JoinState.NotJoining && PlayerManager.playerSlots[j].promptBeforeJoin)
					{
						PlayerManager.playerSlots[j].joinState = PlayerManager.PlayerSlot.JoinState.JoinPromptDisplayed;
					}
					else
					{
						bool flag2 = false;
						PlayerManager.playerSlots[j].joinState = PlayerManager.PlayerSlot.JoinState.JoinRequested;
						if (OnlineManager.Instance.Interface.SupportsMultipleUsers)
						{
							ulong value = (ulong)joystick2.systemId.Value;
							OnlineUser userForController = OnlineManager.Instance.Interface.GetUserForController(value);
							if (userForController != null && ((j == 0 && !userForController.Equals(OnlineManager.Instance.Interface.SecondaryUser)) || (j == 1 && !userForController.Equals(OnlineManager.Instance.Interface.MainUser))))
							{
								OnlineManager.Instance.Interface.SetUser(playerId2, userForController);
								flag2 = true;
							}
							else
							{
								OnlineManager.Instance.Interface.SignInUser(false, playerId2, value);
							}
						}
						else if (OnlineManager.Instance.Interface.SupportsUserSignIn && playerId2 == PlayerId.PlayerOne)
						{
							OnlineManager.Instance.Interface.SignInUser(false, playerId2, 0UL);
						}
						else
						{
							flag2 = true;
						}
						if (flag2)
						{
							if (joystick2 != null)
							{
								playerInput2.controllers.AddController(joystick2, true);
								ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)playerId2].id, ControllerType.Joystick, PlayerManager.playerSlots[j].controllerId);
							}
							PlayerManager.playerSlots[j].joinState = PlayerManager.PlayerSlot.JoinState.Joined;
							if (playerId2 == PlayerId.PlayerTwo)
							{
								PlayerManager.Multiplayer = true;
							}
							PlayerManager.OnPlayerJoinedEvent(playerId2);
							AudioManager.Play("player_spawn");
						}
					}
				}
			}
		}
		for (int k = 0; k < PlayerManager.playerSlots.Length; k++)
		{
			if (OnlineManager.Instance.Interface.SupportsUserSignIn && PlayerManager.playerSlots[k].canSwitch && PlayerManager.playerSlots[k].joinState == PlayerManager.PlayerSlot.JoinState.Joined)
			{
				if (OnlineManager.Instance.Interface.SupportsMultipleUsers || k != 1)
				{
					PlayerId playerId3 = (k != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
					Player playerInput3 = PlayerManager.GetPlayerInput(playerId3);
					if (playerInput3.GetButtonDown(11))
					{
						PlayerManager.playerSlots[k].requestedSwitch = true;
						PlayerManager.playerSlots[(k + 1) % 2].requestedSwitch = false;
						ulong controllerId = 0UL;
						if (playerInput3.controllers.joystickCount > 0)
						{
							controllerId = (ulong)playerInput3.controllers.Joysticks[0].systemId.Value;
						}
						OnlineManager.Instance.Interface.SwitchUser(playerId3, controllerId);
					}
				}
			}
		}
		for (int l = 0; l < PlayerManager.playerSlots.Length; l++)
		{
			if (SceneLoader.CurrentlyLoading)
			{
				break;
			}
			if (PlayerManager.playerSlots[l].joinState == PlayerManager.PlayerSlot.JoinState.Leaving)
			{
				PlayerId playerId4 = (l != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				Player playerInput4 = PlayerManager.GetPlayerInput(playerId4);
				playerInput4.controllers.ClearControllersOfType<Joystick>();
				PlayerManager.playerSlots[l].joinState = PlayerManager.PlayerSlot.JoinState.NotJoining;
				if (playerId4 == PlayerId.PlayerTwo)
				{
					PlayerManager.Multiplayer = false;
				}
				OnlineManager.Instance.Interface.SetRichPresenceActive(playerId4, false);
				OnlineManager.Instance.Interface.SetUser(playerId4, null);
				if (playerId4 == PlayerId.PlayerOne)
				{
					PlayerManager.shouldGoToStartScreen = true;
				}
				else if (PlayerManager.OnPlayerLeaveEvent != null)
				{
					PlayerManager.OnPlayerLeaveEvent(playerId4);
					AudioManager.Play("player_despawn");
				}
			}
		}
		for (int m = 0; m < PlayerManager.playerSlots.Length; m++)
		{
			if (PlayerManager.playerSlots[m].shouldAssignController)
			{
				PlayerId playerId5 = (m != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				Player playerInput5 = PlayerManager.GetPlayerInput(playerId5);
				playerInput5.controllers.AddController<Joystick>(PlayerManager.playerSlots[m].controllerId, true);
				ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)playerId5].id, ControllerType.Joystick, PlayerManager.playerSlots[m].controllerId);
				PlayerManager.playerSlots[m].shouldAssignController = false;
			}
		}
		if (ControllerDisconnectedPrompt.Instance != null && !ControllerDisconnectedPrompt.Instance.Visible && ControllerDisconnectedPrompt.Instance.allowedToShow)
		{
			for (int n = 0; n < 2; n++)
			{
				PlayerId playerId6 = (n != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				if (PlayerManager.IsControllerDisconnected(playerId6, false))
				{
					ControllerDisconnectedPrompt.Instance.Show(playerId6);
					break;
				}
			}
		}
		if (PlmManager.Instance.Interface.IsConstrained())
		{
			if (InterruptingPrompt.CanInterrupt() && PauseManager.state != PauseManager.State.Paused)
			{
				PauseManager.Pause();
				PlayerManager.pausedDueToPlm = true;
			}
		}
		else if (PlayerManager.pausedDueToPlm)
		{
			PauseManager.Unpause();
			PlayerManager.pausedDueToPlm = false;
		}
		if (PlayerManager.shouldGoToSlotSelect)
		{
			PlayerManager.goToSlotSelect();
			PlayerManager.shouldGoToSlotSelect = false;
		}
		if (PlayerManager.shouldGoToStartScreen)
		{
			PlayerManager.goToStartScreen();
			PlayerManager.shouldGoToStartScreen = false;
		}
		for (int num = 0; num < 2; num++)
		{
			PlayerId id = (num != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
			Controller lastActiveController = PlayerManager.GetPlayerInput(id).controllers.GetLastActiveController();
			if (lastActiveController != null && lastActiveController.type != PlayerManager.playerSlots[num].lastController)
			{
				PlayerManager.playerSlots[num].lastController = lastActiveController.type;
				PlayerManager.ControlsChanged();
			}
		}
	}

	// Token: 0x0600425E RID: 16990 RVA: 0x0023D120 File Offset: 0x0023B520
	public static void ControllerRemapped(PlayerId playerId, bool usingController, int controllerId)
	{
		int num = (playerId != PlayerId.PlayerOne) ? 1 : 0;
		PlayerManager.playerSlots[num].controllerState = ((!usingController) ? PlayerManager.PlayerSlot.ControllerState.NoController : PlayerManager.PlayerSlot.ControllerState.UsingController);
		PlayerManager.playerSlots[num].controllerId = controllerId;
	}

	// Token: 0x0600425F RID: 16991 RVA: 0x0023D161 File Offset: 0x0023B561
	public static void ControlsChanged()
	{
		if (PlayerManager.OnControlsChanged != null)
		{
			PlayerManager.OnControlsChanged();
		}
	}

	// Token: 0x06004260 RID: 16992 RVA: 0x0023D178 File Offset: 0x0023B578
	private static void OnUserSignedIn(OnlineUser user)
	{
		for (int i = 0; i < PlayerManager.playerSlots.Length; i++)
		{
			if (PlayerManager.playerSlots[i].canJoin && PlayerManager.playerSlots[i].joinState == PlayerManager.PlayerSlot.JoinState.JoinRequested)
			{
				OnlineManager.Instance.Interface.UpdateControllerMapping();
				if (user == null || (i == 0 && user.Equals(OnlineManager.Instance.Interface.SecondaryUser)) || (i == 1 && user.Equals(OnlineManager.Instance.Interface.MainUser)))
				{
					PlayerManager.playerSlots[i].joinState = PlayerManager.PlayerSlot.JoinState.NotJoining;
				}
				else
				{
					PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
					OnlineManager.Instance.Interface.SetUser(playerId, user);
					if (PlayerManager.playerSlots[i].controllerState == PlayerManager.PlayerSlot.ControllerState.UsingController)
					{
						PlayerManager.playerSlots[i].shouldAssignController = true;
					}
					PlayerManager.playerSlots[i].joinState = PlayerManager.PlayerSlot.JoinState.Joined;
					if (playerId == PlayerId.PlayerTwo)
					{
						PlayerManager.Multiplayer = true;
					}
					PlayerManager.OnPlayerJoinedEvent(playerId);
				}
			}
		}
		for (int j = 0; j < PlayerManager.playerSlots.Length; j++)
		{
			if (PlayerManager.playerSlots[j].canSwitch && PlayerManager.playerSlots[j].requestedSwitch && PlayerManager.playerSlots[j].joinState == PlayerManager.PlayerSlot.JoinState.Joined)
			{
				OnlineManager.Instance.Interface.UpdateControllerMapping();
				PlayerManager.playerSlots[j].requestedSwitch = false;
				PlayerId player = (j != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				if (user != null && !user.Equals(OnlineManager.Instance.Interface.MainUser) && !user.Equals(OnlineManager.Instance.Interface.SecondaryUser))
				{
					OnlineManager.Instance.Interface.SetUser(player, user);
					if (j == 0)
					{
						PlayerManager.shouldGoToSlotSelect = true;
					}
				}
			}
		}
	}

	// Token: 0x06004261 RID: 16993 RVA: 0x0023D368 File Offset: 0x0023B768
	private static void OnUserSignedOut(PlayerId player, string name)
	{
		if (PlmManager.Instance.Interface.IsConstrained())
		{
			return;
		}
		PlayerManager.PlayerSlot playerSlot = (player != PlayerId.PlayerOne) ? PlayerManager.playerSlots[1] : PlayerManager.playerSlots[0];
		if (playerSlot.requestedSwitch)
		{
			return;
		}
		PlayerManager.PlayerLeave(player);
	}

	// Token: 0x06004262 RID: 16994 RVA: 0x0023D3B8 File Offset: 0x0023B7B8
	private static void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
	{
		if (PlmManager.Instance.Interface.IsConstrained())
		{
			return;
		}
		for (int i = 0; i < PlayerManager.playerSlots.Length; i++)
		{
			PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
			if (PlayerManager.playerSlots[i].controllerState == PlayerManager.PlayerSlot.ControllerState.UsingController && PlayerManager.playerSlots[i].controllerId == args.controllerId && PlayerManager.playerSlots[i].joinState == PlayerManager.PlayerSlot.JoinState.Joined)
			{
				PlayerManager.playerInputs[(int)playerId].controllers.RemoveController<Joystick>(args.controllerId);
				PlayerManager.playerSlots[i].controllerState = PlayerManager.PlayerSlot.ControllerState.Disconnected;
				if (playerId == PlayerId.PlayerOne)
				{
					PlayerManager.player1DisconnectedControllerId = args.controllerId;
				}
			}
		}
	}

	// Token: 0x06004263 RID: 16995 RVA: 0x0023D479 File Offset: 0x0023B879
	private static void OnSuspend()
	{
	}

	// Token: 0x06004264 RID: 16996 RVA: 0x0023D47B File Offset: 0x0023B87B
	private static void OnResume()
	{
	}

	// Token: 0x06004265 RID: 16997 RVA: 0x0023D47D File Offset: 0x0023B87D
	private static void OnCloudStorageInitialized(bool success)
	{
		if (!success)
		{
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			PlayerId player = PlayerId.PlayerOne;
			if (PlayerManager.<>f__mg$cache7 == null)
			{
				PlayerManager.<>f__mg$cache7 = new InitializeCloudStoreHandler(PlayerManager.OnCloudStorageInitialized);
			}
			@interface.InitializeCloudStorage(player, PlayerManager.<>f__mg$cache7);
			return;
		}
	}

	// Token: 0x06004266 RID: 16998 RVA: 0x0023D4B3 File Offset: 0x0023B8B3
	private static void OnUnconstrained()
	{
		PlayerManager.CheckForPairingsChanges();
	}

	// Token: 0x06004267 RID: 16999 RVA: 0x0023D4BC File Offset: 0x0023B8BC
	private static void CheckForPairingsChanges()
	{
		bool flag = OnlineManager.Instance.Interface.ControllerMappingChanged();
		for (int i = 0; i < PlayerManager.playerSlots.Length; i++)
		{
			PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
			if (PlayerManager.playerSlots[i].joinState == PlayerManager.PlayerSlot.JoinState.Joined)
			{
				if (!OnlineManager.Instance.Interface.IsUserSignedIn(playerId))
				{
					PlayerManager.PlayerLeave(playerId);
					if (playerId == PlayerId.PlayerOne)
					{
						PlayerManager.PlayerLeave(PlayerId.PlayerTwo);
					}
				}
				else if (!flag)
				{
					if (PlayerManager.playerSlots[i].controllerState == PlayerManager.PlayerSlot.ControllerState.UsingController && PlayerManager.playerInputs[(int)playerId].controllers.joystickCount == 0)
					{
						PlayerManager.playerInputs[(int)playerId].controllers.AddController<Joystick>(PlayerManager.playerSlots[i].controllerId, true);
						ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)playerId].id, ControllerType.Joystick, PlayerManager.playerSlots[i].controllerId);
					}
				}
				else
				{
					List<ulong> controllersForUser = OnlineManager.Instance.Interface.GetControllersForUser(playerId);
					if (controllersForUser == null || controllersForUser.Count != 1)
					{
						PlayerManager.playerInputs[(int)playerId].controllers.ClearControllersOfType<Joystick>();
						PlayerManager.playerSlots[i].controllerState = PlayerManager.PlayerSlot.ControllerState.Disconnected;
						PlayerManager.playerSlots[i].controllerDisconnectFromPlm = true;
					}
					else
					{
						ulong num = controllersForUser[0];
						foreach (Joystick joystick in ReInput.controllers.Joysticks)
						{
							if (joystick.systemId.Value == (long)num)
							{
								if (PlayerManager.playerInputs[(int)playerId].controllers.joystickCount > 0)
								{
								}
								PlayerManager.playerInputs[(int)playerId].controllers.ClearControllersOfType<Joystick>();
								PlayerManager.playerInputs[(int)playerId].controllers.AddController(joystick, true);
								ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)playerId].id, ControllerType.Joystick, PlayerManager.playerSlots[i].controllerId);
								PlayerManager.playerSlots[i].controllerId = joystick.id;
								break;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06004268 RID: 17000 RVA: 0x0023D710 File Offset: 0x0023BB10
	public static void LoadControllerMappings(PlayerId player)
	{
		int num = (player != PlayerId.PlayerOne) ? 1 : 0;
		ReInput.userDataStore.LoadControllerData(PlayerManager.playerInputs[(int)player].id, ControllerType.Joystick, PlayerManager.playerSlots[num].controllerId);
	}

	// Token: 0x06004269 RID: 17001 RVA: 0x0023D754 File Offset: 0x0023BB54
	public static bool IsControllerDisconnected(PlayerId playerId, bool countWaitingForReconnectAsDisconnected = true)
	{
		int num = (playerId != PlayerId.PlayerOne) ? 1 : 0;
		return PlayerManager.playerSlots[num].joinState == PlayerManager.PlayerSlot.JoinState.Joined && (PlayerManager.playerSlots[num].controllerState == PlayerManager.PlayerSlot.ControllerState.Disconnected || PlayerManager.playerSlots[num].controllerState == PlayerManager.PlayerSlot.ControllerState.ReconnectPromptDisplayed || (countWaitingForReconnectAsDisconnected && PlayerManager.playerSlots[num].controllerState == PlayerManager.PlayerSlot.ControllerState.WaitingForReconnect));
	}

	// Token: 0x0600426A RID: 17002 RVA: 0x0023D7C4 File Offset: 0x0023BBC4
	public static void OnDisconnectPromptDisplayed(PlayerId playerId)
	{
		int num = (playerId != PlayerId.PlayerOne) ? 1 : 0;
		PlayerManager.playerSlots[num].controllerState = PlayerManager.PlayerSlot.ControllerState.ReconnectPromptDisplayed;
	}

	// Token: 0x0600426B RID: 17003 RVA: 0x0023D7EC File Offset: 0x0023BBEC
	private static void goToSlotSelect()
	{
		Cuphead.Current.controlMapper.Close(true);
		PlayerManager.playerSlots[0].canSwitch = false;
		PlayerManager.playerSlots[0].requestedSwitch = false;
		PlayerManager.playerSlots[0].canJoin = false;
		PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).controllers.ClearControllersOfType<Joystick>();
		PlayerManager.playerSlots[1] = new PlayerManager.PlayerSlot();
		PlayerManager.Multiplayer = false;
		OnlineManager.Instance.Interface.SetUser(PlayerId.PlayerTwo, null);
		SceneLoader.LoadScene(Scenes.scene_slot_select, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x0600426C RID: 17004 RVA: 0x0023D86D File Offset: 0x0023BC6D
	private static void goToStartScreen()
	{
		Cuphead.Current.controlMapper.Close(true);
		PlayerManager.ResetPlayers();
		if (StartScreenAudio.Instance != null)
		{
			UnityEngine.Object.Destroy(StartScreenAudio.Instance.gameObject);
		}
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x0600426D RID: 17005 RVA: 0x0023D8B0 File Offset: 0x0023BCB0
	public static void ResetPlayers()
	{
		PlayerManager.playerSlots[0] = new PlayerManager.PlayerSlot();
		PlayerManager.playerSlots[1] = new PlayerManager.PlayerSlot();
		PlayerManager.GetPlayerInput(PlayerId.PlayerOne).controllers.ClearControllersOfType<Joystick>();
		PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).controllers.ClearControllersOfType<Joystick>();
		PlayerManager.Multiplayer = false;
		if (OnlineManager.Instance.Interface.SupportsMultipleUsers)
		{
			OnlineManager.Instance.Interface.SetUser(PlayerId.PlayerOne, null);
			OnlineManager.Instance.Interface.SetUser(PlayerId.PlayerTwo, null);
		}
	}

	// Token: 0x0600426E RID: 17006 RVA: 0x0023D931 File Offset: 0x0023BD31
	public static Player GetPlayerInput(PlayerId id)
	{
		return PlayerManager.playerInputs[(int)id];
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x0600426F RID: 17007 RVA: 0x0023D93E File Offset: 0x0023BD3E
	public static AbstractPlayerController Current
	{
		get
		{
			return PlayerManager.GetPlayer(PlayerManager.currentId);
		}
	}

	// Token: 0x06004270 RID: 17008 RVA: 0x0023D94A File Offset: 0x0023BD4A
	public static void SetPlayer(PlayerId id, AbstractPlayerController player)
	{
		PlayerManager.players[(int)id] = player;
	}

	// Token: 0x06004271 RID: 17009 RVA: 0x0023D958 File Offset: 0x0023BD58
	public static void ClearPlayer(PlayerId id)
	{
		PlayerManager.players[(int)id] = null;
	}

	// Token: 0x06004272 RID: 17010 RVA: 0x0023D966 File Offset: 0x0023BD66
	public static void ClearPlayers()
	{
		PlayerManager.currentId = PlayerId.PlayerOne;
		PlayerManager.players[0] = null;
		PlayerManager.players[1] = null;
	}

	// Token: 0x06004273 RID: 17011 RVA: 0x0023D986 File Offset: 0x0023BD86
	public static AbstractPlayerController GetPlayer(PlayerId id)
	{
		return PlayerManager.players[(int)id];
	}

	// Token: 0x06004274 RID: 17012 RVA: 0x0023D993 File Offset: 0x0023BD93
	public static T GetPlayer<T>(PlayerId id) where T : AbstractPlayerController
	{
		return PlayerManager.GetPlayer(id) as T;
	}

	// Token: 0x06004275 RID: 17013 RVA: 0x0023D9A5 File Offset: 0x0023BDA5
	public static AbstractPlayerController GetRandom()
	{
		if (!PlayerManager.Multiplayer || !PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo))
		{
			return PlayerManager.players[0];
		}
		return PlayerManager.GetPlayer(EnumUtils.Random<PlayerId>());
	}

	// Token: 0x06004276 RID: 17014 RVA: 0x0023D9D4 File Offset: 0x0023BDD4
	public static AbstractPlayerController GetNext()
	{
		if (!PlayerManager.Multiplayer || !PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo))
		{
			return PlayerManager.players[0];
		}
		if (!PlayerManager.DoesPlayerExist(PlayerId.PlayerOne))
		{
			return PlayerManager.players[1];
		}
		AbstractPlayerController result = PlayerManager.Current;
		PlayerId playerId = PlayerManager.currentId;
		if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
		{
			PlayerManager.currentId = PlayerId.PlayerTwo;
		}
		else
		{
			PlayerManager.currentId = PlayerId.PlayerOne;
		}
		return result;
	}

	// Token: 0x06004277 RID: 17015 RVA: 0x0023DA4E File Offset: 0x0023BE4E
	public static bool DoesPlayerExist(PlayerId player)
	{
		return !(PlayerManager.players[(int)player] == null) && !PlayerManager.players[(int)player].IsDead;
	}

	// Token: 0x06004278 RID: 17016 RVA: 0x0023DA80 File Offset: 0x0023BE80
	public static bool BothPlayersActive()
	{
		return PlayerManager.DoesPlayerExist(PlayerId.PlayerOne) && PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo);
	}

	// Token: 0x06004279 RID: 17017 RVA: 0x0023DA96 File Offset: 0x0023BE96
	public static AbstractPlayerController GetFirst()
	{
		if (!PlayerManager.DoesPlayerExist(PlayerId.PlayerOne))
		{
			return PlayerManager.players[1];
		}
		return PlayerManager.players[0];
	}

	// Token: 0x0600427A RID: 17018 RVA: 0x0023DABA File Offset: 0x0023BEBA
	public static Dictionary<int, AbstractPlayerController>.ValueCollection GetAllPlayers()
	{
		return PlayerManager.players.Values;
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x0600427B RID: 17019 RVA: 0x0023DAC8 File Offset: 0x0023BEC8
	public static int Count
	{
		get
		{
			int num = 0;
			foreach (int num2 in PlayerManager.players.Keys)
			{
				if (PlayerManager.DoesPlayerExist((PlayerId)num2) && !PlayerManager.GetPlayer((PlayerId)num2).IsDead)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x0600427C RID: 17020 RVA: 0x0023DB44 File Offset: 0x0023BF44
	public static Vector2 Center
	{
		get
		{
			if (!PlayerManager.Multiplayer || PlayerManager.Count < 2)
			{
				return PlayerManager.GetFirst().center;
			}
			return (PlayerManager.players[0].center + PlayerManager.players[1].center) / 2f;
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x0600427D RID: 17021 RVA: 0x0023DBAC File Offset: 0x0023BFAC
	public static Vector2 CameraCenter
	{
		get
		{
			if (!PlayerManager.Multiplayer || PlayerManager.Count < 2)
			{
				return PlayerManager.GetFirst().CameraCenter;
			}
			return (PlayerManager.players[0].center + PlayerManager.players[1].CameraCenter) / 2f;
		}
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x0600427E RID: 17022 RVA: 0x0023DC14 File Offset: 0x0023C014
	public static Vector2 TopPlayerPosition
	{
		get
		{
			if (!PlayerManager.Multiplayer || PlayerManager.Count < 2)
			{
				return PlayerManager.GetFirst().transform.position;
			}
			float y = Mathf.Max(PlayerManager.players[0].transform.position.y, PlayerManager.players[1].transform.position.y);
			return new Vector2((PlayerManager.players[0].transform.position.x + PlayerManager.players[0].transform.position.x) / 2f, y);
		}
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x0600427F RID: 17023 RVA: 0x0023DCD3 File Offset: 0x0023C0D3
	public static float DamageMultiplier
	{
		get
		{
			if (PlayerManager.Count > 1)
			{
				return 0.5f;
			}
			return 1f;
		}
	}

	// Token: 0x040048BF RID: 18623
	private const float SINGLE_PLAYER_DAMAGE_MULTIPLIER = 1f;

	// Token: 0x040048C0 RID: 18624
	private const float MULTIPLAYER_DAMAGE_MULTIPLIER = 0.5f;

	// Token: 0x040048C1 RID: 18625
	private static PlayerManager.PlayerSlot[] playerSlots = new PlayerManager.PlayerSlot[]
	{
		new PlayerManager.PlayerSlot(),
		new PlayerManager.PlayerSlot()
	};

	// Token: 0x040048C2 RID: 18626
	public static bool Multiplayer;

	// Token: 0x040048C3 RID: 18627
	private static bool shouldGoToSlotSelect = false;

	// Token: 0x040048C4 RID: 18628
	private static bool shouldGoToStartScreen = false;

	// Token: 0x040048C5 RID: 18629
	private static bool pausedDueToPlm = false;

	// Token: 0x040048C9 RID: 18633
	public static int player1DisconnectedControllerId;

	// Token: 0x040048CA RID: 18634
	public static bool player1IsMugman;

	// Token: 0x040048CB RID: 18635
	public static bool[] playerWasChalice = new bool[2];

	// Token: 0x040048CC RID: 18636
	private static Dictionary<int, Player> playerInputs;

	// Token: 0x040048CD RID: 18637
	private static Dictionary<int, AbstractPlayerController> players;

	// Token: 0x040048CE RID: 18638
	private static PlayerId currentId;

	// Token: 0x040048CF RID: 18639
	[CompilerGenerated]
	private static SignInEventHandler <>f__mg$cache0;

	// Token: 0x040048D0 RID: 18640
	[CompilerGenerated]
	private static SignOutEventHandler <>f__mg$cache1;

	// Token: 0x040048D1 RID: 18641
	[CompilerGenerated]
	private static Action<ControllerStatusChangedEventArgs> <>f__mg$cache2;

	// Token: 0x040048D2 RID: 18642
	[CompilerGenerated]
	private static Action<ControllerStatusChangedEventArgs> <>f__mg$cache3;

	// Token: 0x040048D3 RID: 18643
	[CompilerGenerated]
	private static OnUnconstrainedHandler <>f__mg$cache4;

	// Token: 0x040048D4 RID: 18644
	[CompilerGenerated]
	private static OnResumeHandler <>f__mg$cache5;

	// Token: 0x040048D5 RID: 18645
	[CompilerGenerated]
	private static OnSuspendHandler <>f__mg$cache6;

	// Token: 0x040048D6 RID: 18646
	[CompilerGenerated]
	private static InitializeCloudStoreHandler <>f__mg$cache7;

	// Token: 0x02000ACC RID: 2764
	private class PlayerSlot
	{
		// Token: 0x040048D7 RID: 18647
		public bool canJoin;

		// Token: 0x040048D8 RID: 18648
		public PlayerManager.PlayerSlot.JoinState joinState;

		// Token: 0x040048D9 RID: 18649
		public PlayerManager.PlayerSlot.ControllerState controllerState;

		// Token: 0x040048DA RID: 18650
		public bool canSwitch;

		// Token: 0x040048DB RID: 18651
		public bool requestedSwitch;

		// Token: 0x040048DC RID: 18652
		public bool promptBeforeJoin;

		// Token: 0x040048DD RID: 18653
		public int controllerId;

		// Token: 0x040048DE RID: 18654
		public bool shouldAssignController;

		// Token: 0x040048DF RID: 18655
		public bool controllerDisconnectFromPlm;

		// Token: 0x040048E0 RID: 18656
		public ControllerType lastController = ControllerType.Custom;

		// Token: 0x02000ACD RID: 2765
		public enum JoinState
		{
			// Token: 0x040048E2 RID: 18658
			NotJoining,
			// Token: 0x040048E3 RID: 18659
			JoinPromptDisplayed,
			// Token: 0x040048E4 RID: 18660
			JoinRequested,
			// Token: 0x040048E5 RID: 18661
			Joined,
			// Token: 0x040048E6 RID: 18662
			Leaving
		}

		// Token: 0x02000ACE RID: 2766
		public enum ControllerState
		{
			// Token: 0x040048E8 RID: 18664
			NoController,
			// Token: 0x040048E9 RID: 18665
			UsingController,
			// Token: 0x040048EA RID: 18666
			Disconnected,
			// Token: 0x040048EB RID: 18667
			ReconnectPromptDisplayed,
			// Token: 0x040048EC RID: 18668
			WaitingForReconnect
		}
	}

	// Token: 0x02000ACF RID: 2767
	// (Invoke) Token: 0x06004283 RID: 17027
	public delegate void PlayerChangedDelegate(PlayerId playerId);
}
