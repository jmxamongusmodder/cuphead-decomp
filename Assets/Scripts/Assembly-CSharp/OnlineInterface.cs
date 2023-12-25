using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009C5 RID: 2501
public interface OnlineInterface
{
	// Token: 0x14000070 RID: 112
	// (add) Token: 0x06003A8E RID: 14990
	// (remove) Token: 0x06003A8F RID: 14991
	event SignInEventHandler OnUserSignedIn;

	// Token: 0x14000071 RID: 113
	// (add) Token: 0x06003A90 RID: 14992
	// (remove) Token: 0x06003A91 RID: 14993
	event SignOutEventHandler OnUserSignedOut;

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06003A92 RID: 14994
	OnlineUser MainUser { get; }

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06003A93 RID: 14995
	OnlineUser SecondaryUser { get; }

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06003A94 RID: 14996
	bool CloudStorageInitialized { get; }

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x06003A95 RID: 14997
	bool SupportsMultipleUsers { get; }

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x06003A96 RID: 14998
	bool SupportsUserSignIn { get; }

	// Token: 0x06003A97 RID: 14999
	void Init();

	// Token: 0x06003A98 RID: 15000
	void Reset();

	// Token: 0x06003A99 RID: 15001
	void SignInUser(bool silent, PlayerId player, ulong controllerId);

	// Token: 0x06003A9A RID: 15002
	void SwitchUser(PlayerId player, ulong controllerId);

	// Token: 0x06003A9B RID: 15003
	OnlineUser GetUserForController(ulong id);

	// Token: 0x06003A9C RID: 15004
	List<ulong> GetControllersForUser(PlayerId player);

	// Token: 0x06003A9D RID: 15005
	bool IsUserSignedIn(PlayerId player);

	// Token: 0x06003A9E RID: 15006
	OnlineUser GetUser(PlayerId player);

	// Token: 0x06003A9F RID: 15007
	void SetUser(PlayerId player, OnlineUser user);

	// Token: 0x06003AA0 RID: 15008
	Texture2D GetProfilePic(PlayerId player);

	// Token: 0x06003AA1 RID: 15009
	void GetAchievement(PlayerId player, string id, AchievementEventHandler achievementRetrievedHandler);

	// Token: 0x06003AA2 RID: 15010
	void UnlockAchievement(PlayerId player, string id);

	// Token: 0x06003AA3 RID: 15011
	void SyncAchievementsAndStats();

	// Token: 0x06003AA4 RID: 15012
	void SetStat(PlayerId player, string id, int value);

	// Token: 0x06003AA5 RID: 15013
	void SetStat(PlayerId player, string id, float value);

	// Token: 0x06003AA6 RID: 15014
	void SetStat(PlayerId player, string id, string value);

	// Token: 0x06003AA7 RID: 15015
	void IncrementStat(PlayerId player, string id, int value);

	// Token: 0x06003AA8 RID: 15016
	void SetRichPresence(PlayerId player, string id, bool active);

	// Token: 0x06003AA9 RID: 15017
	void SetRichPresenceActive(PlayerId player, bool active);

	// Token: 0x06003AAA RID: 15018
	void InitializeCloudStorage(PlayerId player, InitializeCloudStoreHandler handler);

	// Token: 0x06003AAB RID: 15019
	void UninitializeCloudStorage();

	// Token: 0x06003AAC RID: 15020
	void SaveCloudData(IDictionary<string, string> data, SaveCloudDataHandler handler);

	// Token: 0x06003AAD RID: 15021
	void LoadCloudData(string[] keys, LoadCloudDataHandler handler);

	// Token: 0x06003AAE RID: 15022
	void UpdateControllerMapping();

	// Token: 0x06003AAF RID: 15023
	bool ControllerMappingChanged();
}
