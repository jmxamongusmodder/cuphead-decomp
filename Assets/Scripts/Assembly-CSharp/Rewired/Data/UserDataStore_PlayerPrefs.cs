using System;
using System.Collections;
using System.Collections.Generic;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data
{
	// Token: 0x02000C53 RID: 3155
	public class UserDataStore_PlayerPrefs : UserDataStore
	{
		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06004E09 RID: 19977 RVA: 0x0027838F File Offset: 0x0027678F
		// (set) Token: 0x06004E0A RID: 19978 RVA: 0x00278397 File Offset: 0x00276797
		public bool IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				this.isEnabled = value;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06004E0B RID: 19979 RVA: 0x002783A0 File Offset: 0x002767A0
		// (set) Token: 0x06004E0C RID: 19980 RVA: 0x002783A8 File Offset: 0x002767A8
		public bool LoadDataOnStart
		{
			get
			{
				return this.loadDataOnStart;
			}
			set
			{
				this.loadDataOnStart = value;
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06004E0D RID: 19981 RVA: 0x002783B1 File Offset: 0x002767B1
		// (set) Token: 0x06004E0E RID: 19982 RVA: 0x002783B9 File Offset: 0x002767B9
		public bool LoadJoystickAssignments
		{
			get
			{
				return this.loadJoystickAssignments;
			}
			set
			{
				this.loadJoystickAssignments = value;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06004E0F RID: 19983 RVA: 0x002783C2 File Offset: 0x002767C2
		// (set) Token: 0x06004E10 RID: 19984 RVA: 0x002783CA File Offset: 0x002767CA
		public bool LoadKeyboardAssignments
		{
			get
			{
				return this.loadKeyboardAssignments;
			}
			set
			{
				this.loadKeyboardAssignments = value;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06004E11 RID: 19985 RVA: 0x002783D3 File Offset: 0x002767D3
		// (set) Token: 0x06004E12 RID: 19986 RVA: 0x002783DB File Offset: 0x002767DB
		public bool LoadMouseAssignments
		{
			get
			{
				return this.loadMouseAssignments;
			}
			set
			{
				this.loadMouseAssignments = value;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06004E13 RID: 19987 RVA: 0x002783E4 File Offset: 0x002767E4
		// (set) Token: 0x06004E14 RID: 19988 RVA: 0x002783EC File Offset: 0x002767EC
		public string PlayerPrefsKeyPrefix
		{
			get
			{
				return this.playerPrefsKeyPrefix;
			}
			set
			{
				this.playerPrefsKeyPrefix = value;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06004E15 RID: 19989 RVA: 0x002783F5 File Offset: 0x002767F5
		private string playerPrefsKey_controllerAssignments
		{
			get
			{
				return string.Format("{0}_{1}", this.playerPrefsKeyPrefix, "ControllerAssignments");
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06004E16 RID: 19990 RVA: 0x0027840C File Offset: 0x0027680C
		private bool loadControllerAssignments
		{
			get
			{
				return this.loadKeyboardAssignments || this.loadMouseAssignments || this.loadJoystickAssignments;
			}
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x0027842D File Offset: 0x0027682D
		public override void Save()
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveAll();
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x0027844C File Offset: 0x0027684C
		public override void SaveControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x0027846E File Offset: 0x0027686E
		public override void SaveControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x0027848F File Offset: 0x0027688F
		public override void SavePlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SavePlayerDataNow(playerId);
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x002784AF File Offset: 0x002768AF
		public override void SaveInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x002784D0 File Offset: 0x002768D0
		public override void Load()
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadAll();
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x002784FC File Offset: 0x002768FC
		public override void LoadControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x0027852C File Offset: 0x0027692C
		public override void LoadControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x0027855C File Offset: 0x0027695C
		public override void LoadPlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadPlayerDataNow(playerId);
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x00278588 File Offset: 0x00276988
		public override void LoadInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x002785B5 File Offset: 0x002769B5
		protected override void OnInitialize()
		{
			if (this.loadDataOnStart)
			{
				this.Load();
				if (this.loadControllerAssignments && ReInput.controllers.joystickCount > 0)
				{
					this.SaveControllerAssignments();
				}
			}
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x002785EC File Offset: 0x002769EC
		protected override void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == ControllerType.Joystick)
			{
				int num = this.LoadJoystickData(args.controllerId);
				if (this.loadDataOnStart && this.loadJoystickAssignments && !this.wasJoystickEverDetected)
				{
					base.StartCoroutine(this.LoadJoystickAssignmentsDeferred());
				}
				if (this.loadJoystickAssignments && !this.deferredJoystickAssignmentLoadPending)
				{
					this.SaveControllerAssignments();
				}
				this.wasJoystickEverDetected = true;
			}
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x00278670 File Offset: 0x00276A70
		protected override void OnControllerPreDiscconnect(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == ControllerType.Joystick)
			{
				this.SaveJoystickData(args.controllerId);
			}
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x00278696 File Offset: 0x00276A96
		protected override void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (this.loadControllerAssignments)
			{
				this.SaveControllerAssignments();
			}
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x002786B8 File Offset: 0x00276AB8
		private int LoadAll()
		{
			int num = 0;
			if (this.loadControllerAssignments && this.LoadControllerAssignmentsNow())
			{
				num++;
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				num += this.LoadPlayerDataNow(allPlayers[i]);
			}
			return num + this.LoadAllJoystickCalibrationData();
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x0027871D File Offset: 0x00276B1D
		private int LoadPlayerDataNow(int playerId)
		{
			return this.LoadPlayerDataNow(ReInput.players.GetPlayer(playerId));
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x00278730 File Offset: 0x00276B30
		private int LoadPlayerDataNow(Player player)
		{
			if (player == null)
			{
				return 0;
			}
			int num = 0;
			num += this.LoadInputBehaviors(player.id);
			num += this.LoadControllerMaps(player.id, ControllerType.Keyboard, 0);
			num += this.LoadControllerMaps(player.id, ControllerType.Mouse, 0);
			foreach (Joystick joystick in player.controllers.Joysticks)
			{
				num += this.LoadControllerMaps(player.id, ControllerType.Joystick, joystick.id);
			}
			return num;
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x002787DC File Offset: 0x00276BDC
		private int LoadAllJoystickCalibrationData()
		{
			int num = 0;
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				num += this.LoadJoystickCalibrationData(joysticks[i]);
			}
			return num;
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x0027881E File Offset: 0x00276C1E
		private int LoadJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return 0;
			}
			return (!joystick.ImportCalibrationMapFromXmlString(this.GetJoystickCalibrationMapXml(joystick))) ? 0 : 1;
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x00278841 File Offset: 0x00276C41
		private int LoadJoystickCalibrationData(int joystickId)
		{
			return this.LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x00278854 File Offset: 0x00276C54
		private int LoadJoystickData(int joystickId)
		{
			int num = 0;
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (player.controllers.ContainsController(ControllerType.Joystick, joystickId))
				{
					num += this.LoadControllerMaps(player.id, ControllerType.Joystick, joystickId);
				}
			}
			return num + this.LoadJoystickCalibrationData(joystickId);
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x002788C0 File Offset: 0x00276CC0
		private int LoadControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			int num = 0;
			num += this.LoadControllerMaps(playerId, controllerType, controllerId);
			return num + this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x002788E8 File Offset: 0x00276CE8
		private int LoadControllerDataNow(ControllerType controllerType, int controllerId)
		{
			int num = 0;
			if (controllerType == ControllerType.Joystick)
			{
				num += this.LoadJoystickCalibrationData(controllerId);
			}
			return num;
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x0027890C File Offset: 0x00276D0C
		private int LoadControllerMaps(int playerId, ControllerType controllerType, int controllerId)
		{
			int num = 0;
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return num;
			}
			Controller controller = ReInput.controllers.GetController(controllerType, controllerId);
			if (controller == null)
			{
				return num;
			}
			List<UserDataStore_PlayerPrefs.SavedControllerMapData> allControllerMapsXml = this.GetAllControllerMapsXml(player, true, controller);
			if (allControllerMapsXml.Count == 0)
			{
				return num;
			}
			num += player.controllers.maps.AddMapsFromXml(controllerType, controllerId, UserDataStore_PlayerPrefs.SavedControllerMapData.GetXmlStringList(allControllerMapsXml));
			this.AddDefaultMappingsForNewActions(player, allControllerMapsXml, controllerType, controllerId);
			return num;
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x00278984 File Offset: 0x00276D84
		private int LoadInputBehaviors(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return 0;
			}
			int num = 0;
			IList<InputBehavior> inputBehaviors = ReInput.mapping.GetInputBehaviors(player.id);
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				num += this.LoadInputBehaviorNow(player, inputBehaviors[i]);
			}
			return num;
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x002789E4 File Offset: 0x00276DE4
		private int LoadInputBehaviorNow(int playerId, int behaviorId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return 0;
			}
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return 0;
			}
			return this.LoadInputBehaviorNow(player, inputBehavior);
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x00278A24 File Offset: 0x00276E24
		private int LoadInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return 0;
			}
			string inputBehaviorXml = this.GetInputBehaviorXml(player, inputBehavior.id);
			if (inputBehaviorXml == null || inputBehaviorXml == string.Empty)
			{
				return 0;
			}
			return (!inputBehavior.ImportXmlString(inputBehaviorXml)) ? 0 : 1;
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x00278A78 File Offset: 0x00276E78
		private bool LoadControllerAssignmentsNow()
		{
			try
			{
				UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = this.LoadControllerAssignmentData();
				if (controllerAssignmentSaveInfo == null)
				{
					return false;
				}
				if (this.loadKeyboardAssignments || this.loadMouseAssignments)
				{
					this.LoadKeyboardAndMouseAssignmentsNow(controllerAssignmentSaveInfo);
				}
				if (this.loadJoystickAssignments)
				{
					this.LoadJoystickAssignmentsNow(controllerAssignmentSaveInfo);
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06004E33 RID: 20019 RVA: 0x00278AE8 File Offset: 0x00276EE8
		private bool LoadKeyboardAndMouseAssignmentsNow(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
		{
			try
			{
				if (data == null && (data = this.LoadControllerAssignmentData()) == null)
				{
					return false;
				}
				foreach (Player player in ReInput.players.AllPlayers)
				{
					if (data.ContainsPlayer(player.id))
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = data.players[data.IndexOfPlayer(player.id)];
						if (this.loadKeyboardAssignments)
						{
							player.controllers.hasKeyboard = playerInfo.hasKeyboard;
						}
						if (this.loadMouseAssignments)
						{
							player.controllers.hasMouse = playerInfo.hasMouse;
						}
					}
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06004E34 RID: 20020 RVA: 0x00278BD8 File Offset: 0x00276FD8
		private bool LoadJoystickAssignmentsNow(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
		{
			try
			{
				if (ReInput.controllers.joystickCount == 0)
				{
					return false;
				}
				if (data == null && (data = this.LoadControllerAssignmentData()) == null)
				{
					return false;
				}
				foreach (Player player in ReInput.players.AllPlayers)
				{
					player.controllers.ClearControllersOfType(ControllerType.Joystick);
				}
				List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo> list = (!this.loadJoystickAssignments) ? null : new List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>();
				foreach (Player player2 in ReInput.players.AllPlayers)
				{
					if (data.ContainsPlayer(player2.id))
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = data.players[data.IndexOfPlayer(player2.id)];
						for (int i = 0; i < playerInfo.joystickCount; i++)
						{
							UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo2 = playerInfo.joysticks[i];
							if (joystickInfo2 != null)
							{
								Joystick joystick = this.FindJoystickPrecise(joystickInfo2);
								if (joystick != null)
								{
									if (list.Find((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.joystick == joystick) == null)
									{
										list.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick, joystickInfo2.id));
									}
									player2.controllers.AddController(joystick, false);
								}
							}
						}
					}
				}
				if (this.allowImpreciseJoystickAssignmentMatching)
				{
					foreach (Player player3 in ReInput.players.AllPlayers)
					{
						if (data.ContainsPlayer(player3.id))
						{
							UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo2 = data.players[data.IndexOfPlayer(player3.id)];
							for (int j = 0; j < playerInfo2.joystickCount; j++)
							{
								UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = playerInfo2.joysticks[j];
								if (joystickInfo != null)
								{
									Joystick joystick2 = null;
									int num = list.FindIndex((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.oldJoystickId == joystickInfo.id);
									if (num >= 0)
									{
										joystick2 = list[num].joystick;
									}
									else
									{
										List<Joystick> list2;
										if (!this.TryFindJoysticksImprecise(joystickInfo, out list2))
										{
											goto IL_30F;
										}
										using (List<Joystick>.Enumerator enumerator4 = list2.GetEnumerator())
										{
											while (enumerator4.MoveNext())
											{
												Joystick match = enumerator4.Current;
												if (list.Find((UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo x) => x.joystick == match) == null)
												{
													joystick2 = match;
													break;
												}
											}
										}
										if (joystick2 == null)
										{
											goto IL_30F;
										}
										list.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick2, joystickInfo.id));
									}
									player3.controllers.AddController(joystick2, false);
								}
								IL_30F:;
							}
						}
					}
				}
			}
			catch
			{
			}
			if (ReInput.configuration.autoAssignJoysticks)
			{
				ReInput.controllers.AutoAssignJoysticks();
			}
			return true;
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x00278FCC File Offset: 0x002773CC
		private UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo LoadControllerAssignmentData()
		{
			UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo result;
			try
			{
				if (!PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments))
				{
					result = null;
				}
				else
				{
					string @string = PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments);
					if (string.IsNullOrEmpty(@string))
					{
						result = null;
					}
					else
					{
						UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = JsonParser.FromJson<UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo>(@string);
						if (controllerAssignmentSaveInfo == null || controllerAssignmentSaveInfo.playerCount == 0)
						{
							result = null;
						}
						else
						{
							result = controllerAssignmentSaveInfo;
						}
					}
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x00279050 File Offset: 0x00277450
		private IEnumerator LoadJoystickAssignmentsDeferred()
		{
			this.deferredJoystickAssignmentLoadPending = true;
			yield return new WaitForEndOfFrame();
			if (!ReInput.isReady)
			{
				yield break;
			}
			if (this.LoadJoystickAssignmentsNow(null))
			{
			}
			this.SaveControllerAssignments();
			this.deferredJoystickAssignmentLoadPending = false;
			yield break;
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x0027906C File Offset: 0x0027746C
		private void SaveAll()
		{
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				this.SavePlayerDataNow(allPlayers[i]);
			}
			this.SaveAllJoystickCalibrationData();
			if (this.loadControllerAssignments)
			{
				this.SaveControllerAssignments();
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x002790C5 File Offset: 0x002774C5
		private void SavePlayerDataNow(int playerId)
		{
			this.SavePlayerDataNow(ReInput.players.GetPlayer(playerId));
			PlayerPrefs.Save();
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x002790E0 File Offset: 0x002774E0
		private void SavePlayerDataNow(Player player)
		{
			if (player == null)
			{
				return;
			}
			PlayerSaveData saveData = player.GetSaveData(true);
			this.SaveInputBehaviors(player, saveData);
			this.SaveControllerMaps(player, saveData);
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x0027910C File Offset: 0x0027750C
		private void SaveAllJoystickCalibrationData()
		{
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				this.SaveJoystickCalibrationData(joysticks[i]);
			}
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x00279148 File Offset: 0x00277548
		private void SaveJoystickCalibrationData(int joystickId)
		{
			this.SaveJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x0027915C File Offset: 0x0027755C
		private void SaveJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return;
			}
			JoystickCalibrationMapSaveData calibrationMapSaveData = joystick.GetCalibrationMapSaveData();
			string joystickCalibrationMapPlayerPrefsKey = this.GetJoystickCalibrationMapPlayerPrefsKey(joystick);
			PlayerPrefs.SetString(joystickCalibrationMapPlayerPrefsKey, calibrationMapSaveData.map.ToXmlString());
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x00279190 File Offset: 0x00277590
		private void SaveJoystickData(int joystickId)
		{
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (player.controllers.ContainsController(ControllerType.Joystick, joystickId))
				{
					this.SaveControllerMaps(player.id, ControllerType.Joystick, joystickId);
				}
			}
			this.SaveJoystickCalibrationData(joystickId);
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x002791F3 File Offset: 0x002775F3
		private void SaveControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			this.SaveControllerMaps(playerId, controllerType, controllerId);
			this.SaveControllerDataNow(controllerType, controllerId);
			PlayerPrefs.Save();
		}

		// Token: 0x06004E3F RID: 20031 RVA: 0x0027920B File Offset: 0x0027760B
		private void SaveControllerDataNow(ControllerType controllerType, int controllerId)
		{
			if (controllerType == ControllerType.Joystick)
			{
				this.SaveJoystickCalibrationData(controllerId);
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x00279220 File Offset: 0x00277620
		private void SaveControllerMaps(Player player, PlayerSaveData playerSaveData)
		{
			foreach (ControllerMapSaveData saveData in playerSaveData.AllControllerMapSaveData)
			{
				this.SaveControllerMap(player, saveData);
			}
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x0027927C File Offset: 0x0027767C
		private void SaveControllerMaps(int playerId, ControllerType controllerType, int controllerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			if (!player.controllers.ContainsController(controllerType, controllerId))
			{
				return;
			}
			ControllerMapSaveData[] mapSaveData = player.controllers.maps.GetMapSaveData(controllerType, controllerId, true);
			if (mapSaveData == null)
			{
				return;
			}
			for (int i = 0; i < mapSaveData.Length; i++)
			{
				this.SaveControllerMap(player, mapSaveData[i]);
			}
		}

		// Token: 0x06004E42 RID: 20034 RVA: 0x002792E8 File Offset: 0x002776E8
		private void SaveControllerMap(Player player, ControllerMapSaveData saveData)
		{
			string key = this.GetControllerMapPlayerPrefsKey(player, saveData.controller, saveData.categoryId, saveData.layoutId);
			PlayerPrefs.SetString(key, saveData.map.ToXmlString());
			key = this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, saveData.controller, saveData.categoryId, saveData.layoutId);
			PlayerPrefs.SetString(key, this.GetAllActionIdsString());
		}

		// Token: 0x06004E43 RID: 20035 RVA: 0x00279348 File Offset: 0x00277748
		private void SaveInputBehaviors(Player player, PlayerSaveData playerSaveData)
		{
			if (player == null)
			{
				return;
			}
			InputBehavior[] inputBehaviors = playerSaveData.inputBehaviors;
			for (int i = 0; i < inputBehaviors.Length; i++)
			{
				this.SaveInputBehaviorNow(player, inputBehaviors[i]);
			}
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x00279384 File Offset: 0x00277784
		private void SaveInputBehaviorNow(int playerId, int behaviorId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return;
			}
			this.SaveInputBehaviorNow(player, inputBehavior);
			PlayerPrefs.Save();
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x002793C8 File Offset: 0x002777C8
		private void SaveInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return;
			}
			string inputBehaviorPlayerPrefsKey = this.GetInputBehaviorPlayerPrefsKey(player, inputBehavior.id);
			PlayerPrefs.SetString(inputBehaviorPlayerPrefsKey, inputBehavior.ToXmlString());
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x002793FC File Offset: 0x002777FC
		private bool SaveControllerAssignments()
		{
			try
			{
				UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo controllerAssignmentSaveInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo(ReInput.players.allPlayerCount);
				for (int i = 0; i < ReInput.players.allPlayerCount; i++)
				{
					Player player = ReInput.players.AllPlayers[i];
					UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
					controllerAssignmentSaveInfo.players[i] = playerInfo;
					playerInfo.id = player.id;
					playerInfo.hasKeyboard = player.controllers.hasKeyboard;
					playerInfo.hasMouse = player.controllers.hasMouse;
					UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] array = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[player.controllers.joystickCount];
					playerInfo.joysticks = array;
					for (int j = 0; j < player.controllers.joystickCount; j++)
					{
						Joystick joystick = player.controllers.Joysticks[j];
						array[j] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo
						{
							instanceGuid = joystick.deviceInstanceGuid,
							id = joystick.id,
							hardwareIdentifier = joystick.hardwareIdentifier
						};
					}
				}
				PlayerPrefs.SetString(this.playerPrefsKey_controllerAssignments, JsonWriter.ToJson(controllerAssignmentSaveInfo));
				PlayerPrefs.Save();
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06004E47 RID: 20039 RVA: 0x00279548 File Offset: 0x00277948
		private bool ControllerAssignmentSaveDataExists()
		{
			if (!PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments))
			{
				return false;
			}
			string @string = PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments);
			return !string.IsNullOrEmpty(@string);
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x00279584 File Offset: 0x00277984
		private string GetBasePlayerPrefsKey(Player player)
		{
			string str = this.playerPrefsKeyPrefix;
			return str + "|playerName=" + player.name;
		}

		// Token: 0x06004E49 RID: 20041 RVA: 0x002795AC File Offset: 0x002779AC
		private string GetControllerMapPlayerPrefsKey(Player player, Controller controller, int categoryId, int layoutId)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=ControllerMap";
			text = text + "|controllerMapType=" + controller.mapTypeString;
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"|categoryId=",
				categoryId,
				"|layoutId=",
				layoutId
			});
			text = text + "|hardwareIdentifier=" + controller.hardwareIdentifier;
			if (controller.type == ControllerType.Joystick)
			{
				text = text + "|hardwareGuid=" + ((Joystick)controller).hardwareTypeGuid.ToString();
			}
			return text;
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x00279658 File Offset: 0x00277A58
		private string GetControllerMapKnownActionIdsPlayerPrefsKey(Player player, Controller controller, int categoryId, int layoutId)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=ControllerMap_KnownActionIds";
			text = text + "|controllerMapType=" + controller.mapTypeString;
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"|categoryId=",
				categoryId,
				"|layoutId=",
				layoutId
			});
			text = text + "|hardwareIdentifier=" + controller.hardwareIdentifier;
			if (controller.type == ControllerType.Joystick)
			{
				text = text + "|hardwareGuid=" + ((Joystick)controller).hardwareTypeGuid.ToString();
			}
			return text;
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x00279704 File Offset: 0x00277B04
		private string GetJoystickCalibrationMapPlayerPrefsKey(Joystick joystick)
		{
			string str = this.playerPrefsKeyPrefix;
			str += "|dataType=CalibrationMap";
			str = str + "|controllerType=" + joystick.type.ToString();
			str = str + "|hardwareIdentifier=" + joystick.hardwareIdentifier;
			return str + "|hardwareGuid=" + joystick.hardwareTypeGuid.ToString();
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x00279778 File Offset: 0x00277B78
		private string GetInputBehaviorPlayerPrefsKey(Player player, int inputBehaviorId)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=InputBehavior";
			return text + "|id=" + inputBehaviorId;
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x002797AC File Offset: 0x00277BAC
		private string GetControllerMapXml(Player player, Controller controller, int categoryId, int layoutId)
		{
			string controllerMapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, controller, categoryId, layoutId);
			if (!PlayerPrefs.HasKey(controllerMapPlayerPrefsKey))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(controllerMapPlayerPrefsKey);
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x002797DC File Offset: 0x00277BDC
		private List<int> GetControllerMapKnownActionIds(Player player, Controller controller, int categoryId, int layoutId)
		{
			List<int> list = new List<int>();
			string controllerMapKnownActionIdsPlayerPrefsKey = this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controller, categoryId, layoutId);
			if (!PlayerPrefs.HasKey(controllerMapKnownActionIdsPlayerPrefsKey))
			{
				return list;
			}
			string @string = PlayerPrefs.GetString(controllerMapKnownActionIdsPlayerPrefsKey);
			if (string.IsNullOrEmpty(@string))
			{
				return list;
			}
			string[] array = @string.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					int item;
					if (int.TryParse(array[i], out item))
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x00279874 File Offset: 0x00277C74
		private List<UserDataStore_PlayerPrefs.SavedControllerMapData> GetAllControllerMapsXml(Player player, bool userAssignableMapsOnly, Controller controller)
		{
			List<UserDataStore_PlayerPrefs.SavedControllerMapData> list = new List<UserDataStore_PlayerPrefs.SavedControllerMapData>();
			IList<InputMapCategory> mapCategories = ReInput.mapping.MapCategories;
			for (int i = 0; i < mapCategories.Count; i++)
			{
				InputMapCategory inputMapCategory = mapCategories[i];
				if (!userAssignableMapsOnly || inputMapCategory.userAssignable)
				{
					IList<InputLayout> list2 = ReInput.mapping.MapLayouts(controller.type);
					for (int j = 0; j < list2.Count; j++)
					{
						InputLayout inputLayout = list2[j];
						string controllerMapXml = this.GetControllerMapXml(player, controller, inputMapCategory.id, inputLayout.id);
						if (!(controllerMapXml == string.Empty))
						{
							List<int> controllerMapKnownActionIds = this.GetControllerMapKnownActionIds(player, controller, inputMapCategory.id, inputLayout.id);
							list.Add(new UserDataStore_PlayerPrefs.SavedControllerMapData(controllerMapXml, controllerMapKnownActionIds));
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x00279954 File Offset: 0x00277D54
		private string GetJoystickCalibrationMapXml(Joystick joystick)
		{
			string joystickCalibrationMapPlayerPrefsKey = this.GetJoystickCalibrationMapPlayerPrefsKey(joystick);
			if (!PlayerPrefs.HasKey(joystickCalibrationMapPlayerPrefsKey))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(joystickCalibrationMapPlayerPrefsKey);
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x00279980 File Offset: 0x00277D80
		private string GetInputBehaviorXml(Player player, int id)
		{
			string inputBehaviorPlayerPrefsKey = this.GetInputBehaviorPlayerPrefsKey(player, id);
			if (!PlayerPrefs.HasKey(inputBehaviorPlayerPrefsKey))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(inputBehaviorPlayerPrefsKey);
		}

		// Token: 0x06004E52 RID: 20050 RVA: 0x002799B0 File Offset: 0x00277DB0
		private void AddDefaultMappingsForNewActions(Player player, List<UserDataStore_PlayerPrefs.SavedControllerMapData> savedData, ControllerType controllerType, int controllerId)
		{
			if (player == null || savedData == null)
			{
				return;
			}
			List<int> allActionIds = this.GetAllActionIds();
			for (int i = 0; i < savedData.Count; i++)
			{
				UserDataStore_PlayerPrefs.SavedControllerMapData savedControllerMapData = savedData[i];
				if (savedControllerMapData != null)
				{
					if (savedControllerMapData.knownActionIds != null && savedControllerMapData.knownActionIds.Count != 0)
					{
						ControllerMap controllerMap = ControllerMap.CreateFromXml(controllerType, savedData[i].xml);
						if (controllerMap != null)
						{
							ControllerMap map = player.controllers.maps.GetMap(controllerType, controllerId, controllerMap.categoryId, controllerMap.layoutId);
							if (map != null)
							{
								ControllerMap controllerMapInstance = ReInput.mapping.GetControllerMapInstance(ReInput.controllers.GetController(controllerType, controllerId), controllerMap.categoryId, controllerMap.layoutId);
								if (controllerMapInstance != null)
								{
									List<int> list = new List<int>();
									foreach (int item in allActionIds)
									{
										if (!savedControllerMapData.knownActionIds.Contains(item))
										{
											list.Add(item);
										}
									}
									if (list.Count != 0)
									{
										foreach (ActionElementMap actionElementMap in controllerMapInstance.AllMaps)
										{
											if (list.Contains(actionElementMap.actionId))
											{
												if (!map.DoesElementAssignmentConflict(actionElementMap))
												{
													ElementAssignment elementAssignment = new ElementAssignment(controllerType, actionElementMap.elementType, actionElementMap.elementIdentifierId, actionElementMap.axisRange, actionElementMap.keyCode, actionElementMap.modifierKeyFlags, actionElementMap.actionId, actionElementMap.axisContribution, actionElementMap.invert);
													map.CreateElementMap(elementAssignment);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x00279BD4 File Offset: 0x00277FD4
		private List<int> GetAllActionIds()
		{
			List<int> list = new List<int>();
			IList<InputAction> actions = ReInput.mapping.Actions;
			for (int i = 0; i < actions.Count; i++)
			{
				list.Add(actions[i].id);
			}
			return list;
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x00279C1C File Offset: 0x0027801C
		private string GetAllActionIdsString()
		{
			string text = string.Empty;
			List<int> allActionIds = this.GetAllActionIds();
			for (int i = 0; i < allActionIds.Count; i++)
			{
				if (i > 0)
				{
					text += ",";
				}
				text += allActionIds[i];
			}
			return text;
		}

		// Token: 0x06004E55 RID: 20053 RVA: 0x00279C74 File Offset: 0x00278074
		private Joystick FindJoystickPrecise(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo)
		{
			if (joystickInfo == null)
			{
				return null;
			}
			if (joystickInfo.instanceGuid == Guid.Empty)
			{
				return null;
			}
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (joysticks[i].deviceInstanceGuid == joystickInfo.instanceGuid)
				{
					return joysticks[i];
				}
			}
			return null;
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x00279CE8 File Offset: 0x002780E8
		private bool TryFindJoysticksImprecise(UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo, out List<Joystick> matches)
		{
			matches = null;
			if (joystickInfo == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(joystickInfo.hardwareIdentifier))
			{
				return false;
			}
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (string.Equals(joysticks[i].hardwareIdentifier, joystickInfo.hardwareIdentifier, StringComparison.OrdinalIgnoreCase))
				{
					if (matches == null)
					{
						matches = new List<Joystick>();
					}
					matches.Add(joysticks[i]);
				}
			}
			return matches != null;
		}

		// Token: 0x040051C9 RID: 20937
		private const string thisScriptName = "UserDataStore_PlayerPrefs";

		// Token: 0x040051CA RID: 20938
		private const string editorLoadedMessage = "\nIf unexpected input issues occur, the loaded XML data may be outdated or invalid. Clear PlayerPrefs using the inspector option on the UserDataStore_PlayerPrefs component.";

		// Token: 0x040051CB RID: 20939
		private const string playerPrefsKeySuffix_controllerAssignments = "ControllerAssignments";

		// Token: 0x040051CC RID: 20940
		[Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")]
		[SerializeField]
		private bool isEnabled = true;

		// Token: 0x040051CD RID: 20941
		[Tooltip("Should saved data be loaded on start?")]
		[SerializeField]
		private bool loadDataOnStart = true;

		// Token: 0x040051CE RID: 20942
		[Tooltip("Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms. Some platforms/input sources do not provide enough information to reliably save assignments from session to session and reboot to reboot.")]
		[SerializeField]
		private bool loadJoystickAssignments = true;

		// Token: 0x040051CF RID: 20943
		[Tooltip("Should Player Keyboard assignments be saved and loaded?")]
		[SerializeField]
		private bool loadKeyboardAssignments = true;

		// Token: 0x040051D0 RID: 20944
		[Tooltip("Should Player Mouse assignments be saved and loaded?")]
		[SerializeField]
		private bool loadMouseAssignments = true;

		// Token: 0x040051D1 RID: 20945
		[Tooltip("The PlayerPrefs key prefix. Change this to change how keys are stored in PlayerPrefs. Changing this will make saved data already stored with the old key no longer accessible.")]
		[SerializeField]
		private string playerPrefsKeyPrefix = "RewiredSaveData";

		// Token: 0x040051D2 RID: 20946
		private bool allowImpreciseJoystickAssignmentMatching = true;

		// Token: 0x040051D3 RID: 20947
		private bool deferredJoystickAssignmentLoadPending;

		// Token: 0x040051D4 RID: 20948
		private bool wasJoystickEverDetected;

		// Token: 0x02000C54 RID: 3156
		private class SavedControllerMapData
		{
			// Token: 0x06004E57 RID: 20055 RVA: 0x00279D75 File Offset: 0x00278175
			public SavedControllerMapData(string xml, List<int> knownActionIds)
			{
				this.xml = xml;
				this.knownActionIds = knownActionIds;
			}

			// Token: 0x06004E58 RID: 20056 RVA: 0x00279D8C File Offset: 0x0027818C
			public static List<string> GetXmlStringList(List<UserDataStore_PlayerPrefs.SavedControllerMapData> data)
			{
				List<string> list = new List<string>();
				if (data == null)
				{
					return list;
				}
				for (int i = 0; i < data.Count; i++)
				{
					if (data[i] != null)
					{
						if (!string.IsNullOrEmpty(data[i].xml))
						{
							list.Add(data[i].xml);
						}
					}
				}
				return list;
			}

			// Token: 0x040051D5 RID: 20949
			public string xml;

			// Token: 0x040051D6 RID: 20950
			public List<int> knownActionIds;
		}

		// Token: 0x02000C55 RID: 3157
		private class ControllerAssignmentSaveInfo
		{
			// Token: 0x06004E59 RID: 20057 RVA: 0x00279DFD File Offset: 0x002781FD
			public ControllerAssignmentSaveInfo()
			{
			}

			// Token: 0x06004E5A RID: 20058 RVA: 0x00279E08 File Offset: 0x00278208
			public ControllerAssignmentSaveInfo(int playerCount)
			{
				this.players = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[playerCount];
				for (int i = 0; i < playerCount; i++)
				{
					this.players[i] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
				}
			}

			// Token: 0x170007EE RID: 2030
			// (get) Token: 0x06004E5B RID: 20059 RVA: 0x00279E46 File Offset: 0x00278246
			public int playerCount
			{
				get
				{
					return (this.players == null) ? 0 : this.players.Length;
				}
			}

			// Token: 0x06004E5C RID: 20060 RVA: 0x00279E64 File Offset: 0x00278264
			public int IndexOfPlayer(int playerId)
			{
				for (int i = 0; i < this.playerCount; i++)
				{
					if (this.players[i] != null)
					{
						if (this.players[i].id == playerId)
						{
							return i;
						}
					}
				}
				return -1;
			}

			// Token: 0x06004E5D RID: 20061 RVA: 0x00279EB0 File Offset: 0x002782B0
			public bool ContainsPlayer(int playerId)
			{
				return this.IndexOfPlayer(playerId) >= 0;
			}

			// Token: 0x040051D7 RID: 20951
			public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[] players;

			// Token: 0x02000C56 RID: 3158
			public class PlayerInfo
			{
				// Token: 0x170007EF RID: 2031
				// (get) Token: 0x06004E5F RID: 20063 RVA: 0x00279EC7 File Offset: 0x002782C7
				public int joystickCount
				{
					get
					{
						return (this.joysticks == null) ? 0 : this.joysticks.Length;
					}
				}

				// Token: 0x06004E60 RID: 20064 RVA: 0x00279EE4 File Offset: 0x002782E4
				public int IndexOfJoystick(int joystickId)
				{
					for (int i = 0; i < this.joystickCount; i++)
					{
						if (this.joysticks[i] != null)
						{
							if (this.joysticks[i].id == joystickId)
							{
								return i;
							}
						}
					}
					return -1;
				}

				// Token: 0x06004E61 RID: 20065 RVA: 0x00279F30 File Offset: 0x00278330
				public bool ContainsJoystick(int joystickId)
				{
					return this.IndexOfJoystick(joystickId) >= 0;
				}

				// Token: 0x040051D8 RID: 20952
				public int id;

				// Token: 0x040051D9 RID: 20953
				public bool hasKeyboard;

				// Token: 0x040051DA RID: 20954
				public bool hasMouse;

				// Token: 0x040051DB RID: 20955
				public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] joysticks;
			}

			// Token: 0x02000C57 RID: 3159
			public class JoystickInfo
			{
				// Token: 0x040051DC RID: 20956
				public Guid instanceGuid;

				// Token: 0x040051DD RID: 20957
				public string hardwareIdentifier;

				// Token: 0x040051DE RID: 20958
				public int id;
			}
		}

		// Token: 0x02000C58 RID: 3160
		private class JoystickAssignmentHistoryInfo
		{
			// Token: 0x06004E63 RID: 20067 RVA: 0x00279F47 File Offset: 0x00278347
			public JoystickAssignmentHistoryInfo(Joystick joystick, int oldJoystickId)
			{
				if (joystick == null)
				{
					throw new ArgumentNullException("joystick");
				}
				this.joystick = joystick;
				this.oldJoystickId = oldJoystickId;
			}

			// Token: 0x040051DF RID: 20959
			public readonly Joystick joystick;

			// Token: 0x040051E0 RID: 20960
			public readonly int oldJoystickId;
		}
	}
}
