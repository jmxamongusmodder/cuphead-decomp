using System;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x02000C5F RID: 3167
[DisallowMultipleComponent]
internal class SteamManager : MonoBehaviour
{
	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x06004ECC RID: 20172 RVA: 0x0027AA09 File Offset: 0x00278E09
	private static SteamManager Instance
	{
		get
		{
			return SteamManager.s_instance ?? new GameObject("SteamManager").AddComponent<SteamManager>();
		}
	}

	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x06004ECD RID: 20173 RVA: 0x0027AA26 File Offset: 0x00278E26
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06004ECE RID: 20174 RVA: 0x0027AA32 File Offset: 0x00278E32
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
	}

	// Token: 0x06004ECF RID: 20175 RVA: 0x0027AA34 File Offset: 0x00278E34
	private void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		if (!Packsize.Test())
		{
			global::Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			global::Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary((AppId_t)268910U))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException arg)
		{
			global::Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			global::Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInialized = true;
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x0027AB1C File Offset: 0x00278F1C
	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06004ED1 RID: 20177 RVA: 0x0027AB73 File Offset: 0x00278F73
	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x06004ED2 RID: 20178 RVA: 0x0027AB9D File Offset: 0x00278F9D
	private void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x040051FA RID: 20986
	private static SteamManager s_instance;

	// Token: 0x040051FB RID: 20987
	private static bool s_EverInialized;

	// Token: 0x040051FC RID: 20988
	private bool m_bInitialized;

	// Token: 0x040051FD RID: 20989
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
