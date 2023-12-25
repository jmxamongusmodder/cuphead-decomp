using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200041D RID: 1053
[Serializable]
public class SettingsData
{
	// Token: 0x06000F18 RID: 3864 RVA: 0x00096C0C File Offset: 0x0009500C
	public SettingsData()
	{
		this.overscan = 0f;
		this.chromaticAberration = 1f;
		this.screenWidth = Screen.currentResolution.width;
		this.screenHeight = Screen.currentResolution.height;
		this.fullScreen = Screen.fullScreen;
		this.vSyncCount = QualitySettings.vSyncCount;
		this.masterVolume = SettingsData.originalMasterVolume;
		this.sFXVolume = SettingsData.originalsFXVolume;
		this.musicVolume = SettingsData.originalMusicVolume;
		this.hasBootedUpGame = false;
		this.SetCameraEffectDefaults();
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00096CB0 File Offset: 0x000950B0
	public static SettingsData Data
	{
		get
		{
			if (SettingsData._data == null)
			{
				if (!SettingsData.originalAudioValuesInitialized)
				{
					SettingsData.originalAudioValuesInitialized = true;
					SettingsData.originalMasterVolume = AudioManager.masterVolume;
					SettingsData.originalsFXVolume = AudioManager.sfxOptionsVolume;
					SettingsData.originalMusicVolume = AudioManager.bgmOptionsVolume;
				}
				if (SettingsData.hasKey())
				{
					try
					{
						SettingsData._data = JsonUtility.FromJson<SettingsData>(PlayerPrefs.GetString("cuphead_settings_data_v1"));
					}
					catch (ArgumentException)
					{
						SettingsData._data = new SettingsData();
						SettingsData.Save();
					}
				}
				else
				{
					SettingsData._data = new SettingsData();
					SettingsData.Save();
				}
				if (SettingsData._data == null)
				{
					return null;
				}
				SettingsData.ApplySettings();
			}
			return SettingsData._data;
		}
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x00096D6C File Offset: 0x0009516C
	public static void Save()
	{
		string value = JsonUtility.ToJson(SettingsData._data);
		PlayerPrefs.SetString("cuphead_settings_data_v1", value);
		PlayerPrefs.Save();
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x00096D94 File Offset: 0x00095194
	public static void LoadFromCloud(SettingsData.SettingsDataLoadFromCloudHandler handler)
	{
		SettingsData._loadFromCloudHandler = handler;
		if (OnlineManager.Instance.Interface.CloudStorageInitialized)
		{
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			string[] keys = new string[]
			{
				"cuphead_settings_data_v1"
			};
			if (SettingsData.<>f__mg$cache0 == null)
			{
				SettingsData.<>f__mg$cache0 = new LoadCloudDataHandler(SettingsData.OnLoadedCloudData);
			}
			@interface.LoadCloudData(keys, SettingsData.<>f__mg$cache0);
		}
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x00096DF8 File Offset: 0x000951F8
	public static void SaveToCloud()
	{
		if (OnlineManager.Instance.Interface.CloudStorageInitialized)
		{
			string value = JsonUtility.ToJson(SettingsData._data);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["cuphead_settings_data_v1"] = value;
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			IDictionary<string, string> data = dictionary;
			if (SettingsData.<>f__mg$cache1 == null)
			{
				SettingsData.<>f__mg$cache1 = new SaveCloudDataHandler(SettingsData.OnSavedCloudData);
			}
			@interface.SaveCloudData(data, SettingsData.<>f__mg$cache1);
		}
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x00096E63 File Offset: 0x00095263
	private static void OnSavedCloudData(bool success)
	{
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x00096E68 File Offset: 0x00095268
	private static void OnLoadedCloudData(string[] data, CloudLoadResult result)
	{
		if (result == CloudLoadResult.Failed)
		{
			SettingsData.LoadFromCloud(SettingsData._loadFromCloudHandler);
			return;
		}
		try
		{
			if (result == CloudLoadResult.NoData)
			{
				if (SettingsData.hasKey())
				{
					try
					{
						SettingsData._data = JsonUtility.FromJson<SettingsData>(PlayerPrefs.GetString("cuphead_settings_data_v1"));
					}
					catch (ArgumentException)
					{
						SettingsData._data = new SettingsData();
					}
				}
				else
				{
					SettingsData._data = new SettingsData();
				}
				SettingsData.SaveToCloud();
			}
			else
			{
				SettingsData._data = JsonUtility.FromJson<SettingsData>(data[0]);
			}
		}
		catch (ArgumentException)
		{
		}
		if (SettingsData._loadFromCloudHandler != null)
		{
			SettingsData._loadFromCloudHandler(true);
			SettingsData._loadFromCloudHandler = null;
		}
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x00096F2C File Offset: 0x0009532C
	public static void Reset()
	{
		SettingsData._data = new SettingsData();
		SettingsData.Save();
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x00096F3D File Offset: 0x0009533D
	public static void ApplySettings()
	{
		if (SettingsData.OnSettingsAppliedEvent != null)
		{
			SettingsData.OnSettingsAppliedEvent();
		}
		SettingsData.Save();
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x00096F58 File Offset: 0x00095358
	public static void ApplySettingsOnStartup()
	{
		if (Screen.width < 320 || Screen.height < 240)
		{
			SettingsData.Data.screenWidth = 640;
			SettingsData.Data.screenHeight = 480;
			SettingsData.Data.fullScreen = false;
			Screen.SetResolution(SettingsData.Data.screenWidth, SettingsData.Data.screenHeight, SettingsData.Data.fullScreen);
		}
		QualitySettings.vSyncCount = SettingsData.Data.vSyncCount;
		AudioManager.masterVolume = SettingsData.Data.masterVolume;
		AudioManager.sfxOptionsVolume = SettingsData.Data.sFXVolume;
		AudioManager.bgmOptionsVolume = SettingsData.Data.musicVolume;
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0009700B File Offset: 0x0009540B
	private static bool hasKey()
	{
		return PlayerPrefs.HasKey("cuphead_settings_data_v1");
	}

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06000F23 RID: 3875 RVA: 0x00097018 File Offset: 0x00095418
	// (remove) Token: 0x06000F24 RID: 3876 RVA: 0x0009704C File Offset: 0x0009544C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnSettingsAppliedEvent;

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06000F25 RID: 3877 RVA: 0x00097080 File Offset: 0x00095480
	public bool vintageAudioEnabled
	{
		get
		{
			return PlayerData.inGame && PlayerData.Data.vintageAudioEnabled;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000F26 RID: 3878 RVA: 0x00097098 File Offset: 0x00095498
	public BlurGamma.Filter filter
	{
		get
		{
			if (!PlayerData.inGame)
			{
				return BlurGamma.Filter.None;
			}
			return PlayerData.Data.filter;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000F27 RID: 3879 RVA: 0x000970B0 File Offset: 0x000954B0
	// (set) Token: 0x06000F28 RID: 3880 RVA: 0x000970BE File Offset: 0x000954BE
	public float Brightness
	{
		get
		{
			this.ClampBrightness();
			return this.brightness;
		}
		set
		{
			this.brightness = value;
			this.ClampBrightness();
		}
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x000970CD File Offset: 0x000954CD
	private void SetCameraEffectDefaults()
	{
		this.chromaticAberrationEffect = true;
		this.noiseEffect = true;
		this.subtleBlurEffect = true;
		this.brightness = 0f;
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x000970EF File Offset: 0x000954EF
	private void ClampBrightness()
	{
		if (this.brightness < -1f)
		{
			this.brightness = -1f;
		}
		if (this.brightness > 1f)
		{
			this.brightness = 1f;
		}
	}

	// Token: 0x04001839 RID: 6201
	public const string KEY = "cuphead_settings_data_v1";

	// Token: 0x0400183A RID: 6202
	private static SettingsData.SettingsDataLoadFromCloudHandler _loadFromCloudHandler;

	// Token: 0x0400183B RID: 6203
	private static SettingsData _data;

	// Token: 0x0400183D RID: 6205
	public bool hasBootedUpGame;

	// Token: 0x0400183E RID: 6206
	public float overscan;

	// Token: 0x0400183F RID: 6207
	public float chromaticAberration;

	// Token: 0x04001840 RID: 6208
	public int screenWidth;

	// Token: 0x04001841 RID: 6209
	public int screenHeight;

	// Token: 0x04001842 RID: 6210
	public int vSyncCount;

	// Token: 0x04001843 RID: 6211
	public bool fullScreen;

	// Token: 0x04001844 RID: 6212
	public bool forceOriginalTitleScreen;

	// Token: 0x04001845 RID: 6213
	public float masterVolume;

	// Token: 0x04001846 RID: 6214
	public float sFXVolume;

	// Token: 0x04001847 RID: 6215
	public float musicVolume;

	// Token: 0x04001848 RID: 6216
	private static bool originalAudioValuesInitialized;

	// Token: 0x04001849 RID: 6217
	private static float originalMasterVolume;

	// Token: 0x0400184A RID: 6218
	private static float originalsFXVolume;

	// Token: 0x0400184B RID: 6219
	private static float originalMusicVolume;

	// Token: 0x0400184C RID: 6220
	public bool canVibrate = true;

	// Token: 0x0400184D RID: 6221
	public bool rotateControlsWithCamera;

	// Token: 0x0400184E RID: 6222
	public int language = -1;

	// Token: 0x0400184F RID: 6223
	public bool chromaticAberrationEffect;

	// Token: 0x04001850 RID: 6224
	public bool noiseEffect;

	// Token: 0x04001851 RID: 6225
	public bool subtleBlurEffect;

	// Token: 0x04001852 RID: 6226
	[SerializeField]
	private float brightness;

	// Token: 0x04001853 RID: 6227
	[CompilerGenerated]
	private static LoadCloudDataHandler <>f__mg$cache0;

	// Token: 0x04001854 RID: 6228
	[CompilerGenerated]
	private static SaveCloudDataHandler <>f__mg$cache1;

	// Token: 0x0200041E RID: 1054
	// (Invoke) Token: 0x06000F2D RID: 3885
	public delegate void SettingsDataLoadFromCloudHandler(bool success);
}
