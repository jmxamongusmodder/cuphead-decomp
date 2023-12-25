using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Data
{
	// Token: 0x02000C52 RID: 3154
	public class UserDataStore_Cuphead : UserDataStore
	{
		// Token: 0x06004DD7 RID: 19927 RVA: 0x0027767A File Offset: 0x00275A7A
		public override void Save()
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveAll();
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x00277699 File Offset: 0x00275A99
		public override void SaveControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x002776BB File Offset: 0x00275ABB
		public override void SaveControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x002776DC File Offset: 0x00275ADC
		public override void SavePlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SavePlayerDataNow(playerId);
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x002776FC File Offset: 0x00275AFC
		public override void SaveInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x00277720 File Offset: 0x00275B20
		public override void Load()
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadAll();
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x0027774C File Offset: 0x00275B4C
		public override void LoadControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadControllerDataNow(playerId, controllerType, controllerId);
		}

		// Token: 0x06004DDE RID: 19934 RVA: 0x0027777C File Offset: 0x00275B7C
		public override void LoadControllerData(ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x002777AC File Offset: 0x00275BAC
		public override void LoadPlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadPlayerDataNow(playerId);
		}

		// Token: 0x06004DE0 RID: 19936 RVA: 0x002777D8 File Offset: 0x00275BD8
		public override void LoadInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			int num = this.LoadInputBehaviorNow(playerId, behaviorId);
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x00277805 File Offset: 0x00275C05
		protected override void OnInitialize()
		{
			if (this.loadDataOnStart)
			{
				this.Load();
			}
		}

		// Token: 0x06004DE2 RID: 19938 RVA: 0x00277818 File Offset: 0x00275C18
		protected override void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == ControllerType.Joystick)
			{
				int num = this.LoadJoystickData(args.controllerId);
			}
		}

		// Token: 0x06004DE3 RID: 19939 RVA: 0x0027784A File Offset: 0x00275C4A
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

		// Token: 0x06004DE4 RID: 19940 RVA: 0x00277870 File Offset: 0x00275C70
		protected override void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x00277880 File Offset: 0x00275C80
		private int LoadAll()
		{
			int num = 0;
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				num += this.LoadPlayerDataNow(allPlayers[i]);
			}
			return num + this.LoadAllJoystickCalibrationData();
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x002778CB File Offset: 0x00275CCB
		private int LoadPlayerDataNow(int playerId)
		{
			return this.LoadPlayerDataNow(ReInput.players.GetPlayer(playerId));
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x002778E0 File Offset: 0x00275CE0
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

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0027798C File Offset: 0x00275D8C
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

		// Token: 0x06004DE9 RID: 19945 RVA: 0x002779CE File Offset: 0x00275DCE
		private int LoadJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return 0;
			}
			return (!joystick.ImportCalibrationMapFromXmlString(this.GetJoystickCalibrationMapXml(joystick))) ? 0 : 1;
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x002779F1 File Offset: 0x00275DF1
		private int LoadJoystickCalibrationData(int joystickId)
		{
			return this.LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x00277A04 File Offset: 0x00275E04
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

		// Token: 0x06004DEC RID: 19948 RVA: 0x00277A70 File Offset: 0x00275E70
		private int LoadControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			int num = 0;
			num += this.LoadControllerMaps(playerId, controllerType, controllerId);
			return num + this.LoadControllerDataNow(controllerType, controllerId);
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x00277A98 File Offset: 0x00275E98
		private int LoadControllerDataNow(ControllerType controllerType, int controllerId)
		{
			int num = 0;
			if (controllerType == ControllerType.Joystick)
			{
				num += this.LoadJoystickCalibrationData(controllerId);
			}
			return num;
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x00277ABC File Offset: 0x00275EBC
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
			List<string> allControllerMapsXml = this.GetAllControllerMapsXml(player, true, controllerType, controller);
			if (allControllerMapsXml.Count == 0)
			{
				return num;
			}
			return num + player.controllers.maps.AddMapsFromXml(controllerType, controllerId, allControllerMapsXml);
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x00277B24 File Offset: 0x00275F24
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

		// Token: 0x06004DF0 RID: 19952 RVA: 0x00277B84 File Offset: 0x00275F84
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

		// Token: 0x06004DF1 RID: 19953 RVA: 0x00277BC4 File Offset: 0x00275FC4
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

		// Token: 0x06004DF2 RID: 19954 RVA: 0x00277C18 File Offset: 0x00276018
		private void SaveAll()
		{
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				this.SavePlayerDataNow(allPlayers[i]);
			}
			this.SaveAllJoystickCalibrationData();
			PlayerPrefs.Save();
		}

		// Token: 0x06004DF3 RID: 19955 RVA: 0x00277C5F File Offset: 0x0027605F
		private void SavePlayerDataNow(int playerId)
		{
			this.SavePlayerDataNow(ReInput.players.GetPlayer(playerId));
			PlayerPrefs.Save();
		}

		// Token: 0x06004DF4 RID: 19956 RVA: 0x00277C78 File Offset: 0x00276078
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

		// Token: 0x06004DF5 RID: 19957 RVA: 0x00277CA4 File Offset: 0x002760A4
		private void SaveAllJoystickCalibrationData()
		{
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				this.SaveJoystickCalibrationData(joysticks[i]);
			}
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x00277CE0 File Offset: 0x002760E0
		private void SaveJoystickCalibrationData(int joystickId)
		{
			this.SaveJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x00277CF4 File Offset: 0x002760F4
		private void SaveJoystickCalibrationData(Joystick joystick)
		{
			if (joystick == null)
			{
				return;
			}
			JoystickCalibrationMapSaveData calibrationMapSaveData = joystick.GetCalibrationMapSaveData();
			string joystickCalibrationMapPlayerPrefsKey = this.GetJoystickCalibrationMapPlayerPrefsKey(calibrationMapSaveData);
			PlayerPrefs.SetString(joystickCalibrationMapPlayerPrefsKey, calibrationMapSaveData.map.ToXmlString());
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x00277D28 File Offset: 0x00276128
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

		// Token: 0x06004DF9 RID: 19961 RVA: 0x00277D8B File Offset: 0x0027618B
		private void SaveControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
		{
			this.SaveControllerMaps(playerId, controllerType, controllerId);
			this.SaveControllerDataNow(controllerType, controllerId);
			PlayerPrefs.Save();
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x00277DA3 File Offset: 0x002761A3
		private void SaveControllerDataNow(ControllerType controllerType, int controllerId)
		{
			if (controllerType == ControllerType.Joystick)
			{
				this.SaveJoystickCalibrationData(controllerId);
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x00277DB8 File Offset: 0x002761B8
		private void SaveControllerMaps(Player player, PlayerSaveData playerSaveData)
		{
			foreach (ControllerMapSaveData controllerMapSaveData in playerSaveData.AllControllerMapSaveData)
			{
				string controllerMapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, controllerMapSaveData);
				PlayerPrefs.SetString(controllerMapPlayerPrefsKey, controllerMapSaveData.map.ToXmlString());
			}
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x00277E28 File Offset: 0x00276228
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
				string controllerMapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, mapSaveData[i]);
				PlayerPrefs.SetString(controllerMapPlayerPrefsKey, mapSaveData[i].map.ToXmlString());
			}
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x00277EA8 File Offset: 0x002762A8
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

		// Token: 0x06004DFE RID: 19966 RVA: 0x00277EE4 File Offset: 0x002762E4
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

		// Token: 0x06004DFF RID: 19967 RVA: 0x00277F28 File Offset: 0x00276328
		private void SaveInputBehaviorNow(Player player, InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return;
			}
			string inputBehaviorPlayerPrefsKey = this.GetInputBehaviorPlayerPrefsKey(player, inputBehavior);
			PlayerPrefs.SetString(inputBehaviorPlayerPrefsKey, inputBehavior.ToXmlString());
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x00277F58 File Offset: 0x00276358
		private string GetBasePlayerPrefsKey(Player player)
		{
			string str = this.playerPrefsKeyPrefix;
			return str + "|playerName=" + player.name;
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x00277F80 File Offset: 0x00276380
		private string GetControllerMapPlayerPrefsKey(Player player, ControllerMapSaveData saveData)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=ControllerMap";
			text = text + "|controllerMapType=" + saveData.mapTypeString;
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"|categoryId=",
				saveData.map.categoryId,
				"|layoutId=",
				saveData.map.layoutId
			});
			text = text + "|hardwareIdentifier=" + saveData.controllerHardwareIdentifier;
			if (saveData.mapType == typeof(JoystickMap))
			{
				text = text + "|hardwareGuid=" + ((JoystickMapSaveData)saveData).joystickHardwareTypeGuid.ToString();
			}
			return text;
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x00278048 File Offset: 0x00276448
		private string GetControllerMapXml(Player player, ControllerType controllerType, int categoryId, int layoutId, Controller controller)
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
			if (controllerType == ControllerType.Joystick)
			{
				Joystick joystick = (Joystick)controller;
				text = text + "|hardwareGuid=" + joystick.hardwareTypeGuid.ToString();
			}
			if (!PlayerPrefs.HasKey(text))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(text);
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x0027810C File Offset: 0x0027650C
		private List<string> GetAllControllerMapsXml(Player player, bool userAssignableMapsOnly, ControllerType controllerType, Controller controller)
		{
			List<string> list = new List<string>();
			IList<InputMapCategory> mapCategories = ReInput.mapping.MapCategories;
			for (int i = 0; i < mapCategories.Count; i++)
			{
				InputMapCategory inputMapCategory = mapCategories[i];
				if (!userAssignableMapsOnly || inputMapCategory.userAssignable)
				{
					IList<InputLayout> list2 = ReInput.mapping.MapLayouts(controllerType);
					for (int j = 0; j < list2.Count; j++)
					{
						InputLayout inputLayout = list2[j];
						string controllerMapXml = this.GetControllerMapXml(player, controllerType, inputMapCategory.id, inputLayout.id, controller);
						if (!(controllerMapXml == string.Empty))
						{
							list.Add(controllerMapXml);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x002781CC File Offset: 0x002765CC
		private string GetJoystickCalibrationMapPlayerPrefsKey(JoystickCalibrationMapSaveData saveData)
		{
			string str = this.playerPrefsKeyPrefix;
			str += "|dataType=CalibrationMap";
			str = str + "|controllerType=" + saveData.controllerType.ToString();
			str = str + "|hardwareIdentifier=" + saveData.hardwareIdentifier;
			return str + "|hardwareGuid=" + saveData.joystickHardwareTypeGuid.ToString();
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x00278240 File Offset: 0x00276640
		private string GetJoystickCalibrationMapXml(Joystick joystick)
		{
			string text = this.playerPrefsKeyPrefix;
			text += "|dataType=CalibrationMap";
			text = text + "|controllerType=" + joystick.type.ToString();
			text = text + "|hardwareIdentifier=" + joystick.hardwareIdentifier;
			text = text + "|hardwareGuid=" + joystick.hardwareTypeGuid.ToString();
			if (!PlayerPrefs.HasKey(text))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(text);
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x002782CC File Offset: 0x002766CC
		private string GetInputBehaviorPlayerPrefsKey(Player player, InputBehavior saveData)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=InputBehavior";
			return text + "|id=" + saveData.id;
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x00278308 File Offset: 0x00276708
		private string GetInputBehaviorXml(Player player, int id)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=InputBehavior";
			text = text + "|id=" + id;
			if (!PlayerPrefs.HasKey(text))
			{
				return string.Empty;
			}
			return PlayerPrefs.GetString(text);
		}

		// Token: 0x040051C4 RID: 20932
		private const string thisScriptName = "UserDataStore_PlayerPrefs";

		// Token: 0x040051C5 RID: 20933
		private const string editorLoadedMessage = "\nIf unexpected input issues occur, the loaded XML data may be outdated or invalid. Clear PlayerPrefs using the inspector option on the UserDataStore_PlayerPrefs component.";

		// Token: 0x040051C6 RID: 20934
		[SerializeField]
		private bool isEnabled = true;

		// Token: 0x040051C7 RID: 20935
		[SerializeField]
		private bool loadDataOnStart = true;

		// Token: 0x040051C8 RID: 20936
		[SerializeField]
		private string playerPrefsKeyPrefix = "RewiredSaveData";
	}
}
