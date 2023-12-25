using System;
using System.Collections;
using Steamworks;
using UnityEngine;
using UnityEngine.U2D;

// Token: 0x020009B6 RID: 2486
public class DLCManager
{
	// Token: 0x06003A4A RID: 14922 RVA: 0x00211DBB File Offset: 0x002101BB
	public static void RefreshDLC()
	{
		DLCManager.refreshDLC();
	}

	// Token: 0x06003A4B RID: 14923 RVA: 0x00211DC2 File Offset: 0x002101C2
	public static void CheckInstallationStatusChanged()
	{
		DLCManager.checkInstallationStatusChanged();
	}

	// Token: 0x06003A4C RID: 14924 RVA: 0x00211DCC File Offset: 0x002101CC
	public static bool DLCEnabled()
	{
		bool result = false;
		DLCManager.dlcEnabled(ref result);
		return result;
	}

	// Token: 0x06003A4D RID: 14925 RVA: 0x00211DE4 File Offset: 0x002101E4
	public static string AssetBundlePath()
	{
		string result = null;
		DLCManager.assetBundlePath(ref result);
		return result;
	}

	// Token: 0x06003A4E RID: 14926 RVA: 0x00211DFC File Offset: 0x002101FC
	public static bool UsesAlternateBundleLoadingMechanism()
	{
		bool result = false;
		DLCManager.usesAlternateBundleLoadingMechanism(ref result);
		return result;
	}

	// Token: 0x06003A4F RID: 14927 RVA: 0x00211E14 File Offset: 0x00210214
	public static DLCManager.AssetBundleLoadWaitInstruction LoadAssetBundle(string path)
	{
		DLCManager.AssetBundleLoadWaitInstruction result = null;
		DLCManager.loadAssetBundle(path, ref result);
		return result;
	}

	// Token: 0x06003A50 RID: 14928 RVA: 0x00211E2C File Offset: 0x0021022C
	public static bool UnloadBundlesImmediately()
	{
		bool result = false;
		DLCManager.unloadBundlesImmediately(ref result);
		return result;
	}

	// Token: 0x06003A51 RID: 14929 RVA: 0x00211E44 File Offset: 0x00210244
	public static bool CanRedirectToStore()
	{
		bool result = false;
		DLCManager.canRedirectToStore(ref result);
		return result;
	}

	// Token: 0x06003A52 RID: 14930 RVA: 0x00211E5B File Offset: 0x0021025B
	public static void LaunchStore()
	{
		DLCManager.launchStore();
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x00211E62 File Offset: 0x00210262
	public static Coroutine[] LoadPersistentAssets()
	{
		if (DLCManager.persistentAssetsLoaded || !DLCManager.DLCEnabled())
		{
			return null;
		}
		DLCManager.persistentAssetsLoaded = true;
		return new Coroutine[]
		{
			AssetLoader<SpriteAtlas>.LoadPersistentAssetsDLC()
		};
	}

	// Token: 0x06003A54 RID: 14932 RVA: 0x00211E8E File Offset: 0x0021028E
	public static void ResetAvailabilityPrompt()
	{
		DLCManager.availabilityPromptTriggered = true;
		DLCManager.showAvailabilityPrompt = false;
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06003A55 RID: 14933 RVA: 0x00211E9C File Offset: 0x0021029C
	// (set) Token: 0x06003A56 RID: 14934 RVA: 0x00211EA3 File Offset: 0x002102A3
	public static bool persistentAssetsLoaded { get; set; }

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06003A57 RID: 14935 RVA: 0x00211EAB File Offset: 0x002102AB
	// (set) Token: 0x06003A58 RID: 14936 RVA: 0x00211EB2 File Offset: 0x002102B2
	public static bool showAvailabilityPrompt { get; private set; }

	// Token: 0x06003A59 RID: 14937 RVA: 0x00211EBC File Offset: 0x002102BC
	private static bool steamDLCStatus()
	{
		ulong num;
		ulong num2;
		return SteamApps.BIsDlcInstalled(DLCManager.DLCAppID) && !SteamApps.GetDlcDownloadProgress(DLCManager.DLCAppID, out num, out num2);
	}

	// Token: 0x06003A5A RID: 14938 RVA: 0x00211EEB File Offset: 0x002102EB
	private static void refreshDLC()
	{
		if (!SteamManager.Initialized)
		{
			DLCManager.dlcAvailable = false;
			return;
		}
		if (!DLCManager.dlcAvailable)
		{
			DLCManager.dlcAvailable = DLCManager.steamDLCStatus();
		}
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x00211F12 File Offset: 0x00210312
	private static void checkInstallationStatusChanged()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (DLCManager.DLCEnabled() || DLCManager.availabilityPromptTriggered)
		{
			return;
		}
		if (DLCManager.steamDLCStatus())
		{
			DLCManager.showAvailabilityPrompt = true;
		}
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x00211F44 File Offset: 0x00210344
	private static void dlcEnabled(ref bool enabled)
	{
		enabled = DLCManager.dlcAvailable;
	}

	// Token: 0x06003A5D RID: 14941 RVA: 0x00211F4D File Offset: 0x0021034D
	private static void assetBundlePath(ref string path)
	{
		path = Application.streamingAssetsPath;
	}

	// Token: 0x06003A5E RID: 14942 RVA: 0x00211F56 File Offset: 0x00210356
	private static void usesAlternateBundleLoadingMechanism(ref bool usesAlternate)
	{
		usesAlternate = false;
	}

	// Token: 0x06003A5F RID: 14943 RVA: 0x00211F5B File Offset: 0x0021035B
	private static void unloadBundlesImmediately(ref bool unloadImmediately)
	{
		unloadImmediately = false;
	}

	// Token: 0x06003A60 RID: 14944 RVA: 0x00211F60 File Offset: 0x00210360
	private static void loadAssetBundle(string path, ref DLCManager.AssetBundleLoadWaitInstruction waitInstruction)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003A61 RID: 14945 RVA: 0x00211F67 File Offset: 0x00210367
	private static void canRedirectToStore(ref bool canRedirect)
	{
		canRedirect = true;
	}

	// Token: 0x06003A62 RID: 14946 RVA: 0x00211F6C File Offset: 0x0021036C
	private static void launchStore()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamFriends.ActivateGameOverlayToStore(DLCManager.DLCAppID, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
	}

	// Token: 0x04004281 RID: 17025
	private static bool availabilityPromptTriggered;

	// Token: 0x04004284 RID: 17028
	private static readonly AppId_t DLCAppID = new AppId_t(1117850U);

	// Token: 0x04004285 RID: 17029
	private static bool dlcAvailable;

	// Token: 0x020009B7 RID: 2487
	public class AssetBundleLoadWaitInstruction : IEnumerator
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06003A65 RID: 14949 RVA: 0x00084A40 File Offset: 0x00082E40
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x00084A43 File Offset: 0x00082E43
		public bool MoveNext()
		{
			return !this.complete;
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x00084A4E File Offset: 0x00082E4E
		public void Reset()
		{
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06003A68 RID: 14952 RVA: 0x00084A50 File Offset: 0x00082E50
		// (set) Token: 0x06003A69 RID: 14953 RVA: 0x00084A58 File Offset: 0x00082E58
		public AssetBundle assetBundle { get; protected set; }

		// Token: 0x04004286 RID: 17030
		protected bool complete;
	}
}
