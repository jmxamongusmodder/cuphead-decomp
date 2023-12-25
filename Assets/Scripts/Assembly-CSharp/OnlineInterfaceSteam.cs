using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Steamworks;
using UnityEngine;

// Token: 0x020009C6 RID: 2502
public class OnlineInterfaceSteam : OnlineInterface
{
	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x0021204C File Offset: 0x0021044C
	private string SavePath
	{
		get
		{
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cuphead\\");
			}
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Library/Application Support/unity.Studio MDHR.Cuphead/Cuphead/");
			}
			return string.Empty;
		}
	}

	// Token: 0x14000072 RID: 114
	// (add) Token: 0x06003AB2 RID: 15026 RVA: 0x002120AC File Offset: 0x002104AC
	// (remove) Token: 0x06003AB3 RID: 15027 RVA: 0x002120E4 File Offset: 0x002104E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event SignInEventHandler OnUserSignedIn;

	// Token: 0x14000073 RID: 115
	// (add) Token: 0x06003AB4 RID: 15028 RVA: 0x0021211C File Offset: 0x0021051C
	// (remove) Token: 0x06003AB5 RID: 15029 RVA: 0x00212154 File Offset: 0x00210554
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event SignOutEventHandler OnUserSignedOut;

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x06003AB6 RID: 15030 RVA: 0x0021218A File Offset: 0x0021058A
	public OnlineUser MainUser
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x06003AB7 RID: 15031 RVA: 0x0021218D File Offset: 0x0021058D
	public OnlineUser SecondaryUser
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06003AB8 RID: 15032 RVA: 0x00212190 File Offset: 0x00210590
	public bool CloudStorageInitialized
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x00212193 File Offset: 0x00210593
	public bool SupportsMultipleUsers
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06003ABA RID: 15034 RVA: 0x00212196 File Offset: 0x00210596
	public bool SupportsUserSignIn
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x0021219C File Offset: 0x0021059C
	public void Init()
	{
		this.steamManager = new GameObject("SteamManager").AddComponent<SteamManager>();
		this.steamManager.transform.SetParent(Cuphead.Current.transform);
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.RequestCurrentStats();
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x002121E9 File Offset: 0x002105E9
	public void Reset()
	{
	}

	// Token: 0x06003ABD RID: 15037 RVA: 0x002121EB File Offset: 0x002105EB
	public void SignInUser(bool silent, PlayerId player, ulong controllerId)
	{
		this.OnUserSignedIn(null);
	}

	// Token: 0x06003ABE RID: 15038 RVA: 0x002121F9 File Offset: 0x002105F9
	public void SwitchUser(PlayerId player, ulong controllerId)
	{
	}

	// Token: 0x06003ABF RID: 15039 RVA: 0x002121FB File Offset: 0x002105FB
	public OnlineUser GetUserForController(ulong id)
	{
		return null;
	}

	// Token: 0x06003AC0 RID: 15040 RVA: 0x002121FE File Offset: 0x002105FE
	public List<ulong> GetControllersForUser(PlayerId player)
	{
		return null;
	}

	// Token: 0x06003AC1 RID: 15041 RVA: 0x00212201 File Offset: 0x00210601
	public bool IsUserSignedIn(PlayerId player)
	{
		return false;
	}

	// Token: 0x06003AC2 RID: 15042 RVA: 0x00212204 File Offset: 0x00210604
	public OnlineUser GetUser(PlayerId player)
	{
		return null;
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x00212207 File Offset: 0x00210607
	public void SetUser(PlayerId player, OnlineUser user)
	{
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x00212209 File Offset: 0x00210609
	public Texture2D GetProfilePic(PlayerId player)
	{
		return null;
	}

	// Token: 0x06003AC5 RID: 15045 RVA: 0x0021220C File Offset: 0x0021060C
	public void GetAchievement(PlayerId player, string id, AchievementEventHandler achievementRetrievedHandler)
	{
	}

	// Token: 0x06003AC6 RID: 15046 RVA: 0x00212210 File Offset: 0x00210610
	public void UnlockAchievement(PlayerId player, string id)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		bool flag;
		SteamUserStats.GetAchievement(id, out flag);
		if (!flag)
		{
			SteamUserStats.SetAchievement(id);
			SteamUserStats.StoreStats();
		}
	}

	// Token: 0x06003AC7 RID: 15047 RVA: 0x00212244 File Offset: 0x00210644
	public void SyncAchievementsAndStats()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.StoreStats();
	}

	// Token: 0x06003AC8 RID: 15048 RVA: 0x00212257 File Offset: 0x00210657
	public void SetStat(PlayerId player, string id, int value)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.SetStat(id, value);
	}

	// Token: 0x06003AC9 RID: 15049 RVA: 0x0021226C File Offset: 0x0021066C
	public void SetStat(PlayerId player, string id, float value)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.SetStat(id, value);
	}

	// Token: 0x06003ACA RID: 15050 RVA: 0x00212281 File Offset: 0x00210681
	public void SetStat(PlayerId player, string id, string value)
	{
	}

	// Token: 0x06003ACB RID: 15051 RVA: 0x00212284 File Offset: 0x00210684
	public void IncrementStat(PlayerId player, string id, int value)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		int num;
		SteamUserStats.GetStat(id, out num);
		int num2 = num + value;
		SteamUserStats.SetStat(id, num2);
		if (id == "Parries" && (num2 == 20 || num2 == 100))
		{
			SteamUserStats.StoreStats();
		}
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x002122D7 File Offset: 0x002106D7
	public void SetRichPresence(PlayerId player, string id, bool active)
	{
	}

	// Token: 0x06003ACD RID: 15053 RVA: 0x002122D9 File Offset: 0x002106D9
	public void SetRichPresenceActive(PlayerId player, bool active)
	{
	}

	// Token: 0x06003ACE RID: 15054 RVA: 0x002122DB File Offset: 0x002106DB
	public void InitializeCloudStorage(PlayerId player, InitializeCloudStoreHandler handler)
	{
		handler(true);
	}

	// Token: 0x06003ACF RID: 15055 RVA: 0x002122E4 File Offset: 0x002106E4
	public void UninitializeCloudStorage()
	{
	}

	// Token: 0x06003AD0 RID: 15056 RVA: 0x002122E8 File Offset: 0x002106E8
	public void SaveCloudData(IDictionary<string, string> data, SaveCloudDataHandler handler)
	{
		string savePath = this.SavePath;
		if (!Directory.Exists(savePath))
		{
			Directory.CreateDirectory(savePath);
		}
		foreach (string text in data.Keys)
		{
			try
			{
				TextWriter textWriter = new StreamWriter(Path.Combine(savePath, text + ".sav"));
				textWriter.Write(data[text]);
				textWriter.Close();
			}
			catch
			{
				Cuphead.Current.StartCoroutine(this.saveFailed_cr(handler));
				return;
			}
		}
		handler(true);
	}

	// Token: 0x06003AD1 RID: 15057 RVA: 0x002123B0 File Offset: 0x002107B0
	private IEnumerator saveFailed_cr(SaveCloudDataHandler handler)
	{
		yield return new WaitForSeconds(0.25f);
		handler(false);
		yield break;
	}

	// Token: 0x06003AD2 RID: 15058 RVA: 0x002123CC File Offset: 0x002107CC
	public void LoadCloudData(string[] keys, LoadCloudDataHandler handler)
	{
		string[] array = new string[keys.Length];
		string savePath = this.SavePath;
		for (int i = 0; i < array.Length; i++)
		{
			string path = Path.Combine(savePath, keys[i] + ".sav");
			if (File.Exists(path))
			{
				try
				{
					TextReader textReader = new StreamReader(Path.Combine(savePath, keys[i] + ".sav"));
					array[i] = textReader.ReadToEnd();
					textReader.Close();
				}
				catch
				{
					handler(array, CloudLoadResult.Failed);
				}
			}
			else
			{
				handler(array, CloudLoadResult.NoData);
			}
		}
		handler(array, CloudLoadResult.Success);
	}

	// Token: 0x06003AD3 RID: 15059 RVA: 0x00212480 File Offset: 0x00210880
	public void UpdateControllerMapping()
	{
	}

	// Token: 0x06003AD4 RID: 15060 RVA: 0x00212482 File Offset: 0x00210882
	public bool ControllerMappingChanged()
	{
		return false;
	}

	// Token: 0x0400429C RID: 17052
	private SteamManager steamManager;
}
