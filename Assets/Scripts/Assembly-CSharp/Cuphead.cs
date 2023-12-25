using System;
using Rewired;
using Rewired.UI.ControlMapper;
using UnityEngine;

// Token: 0x020003ED RID: 1005
public class Cuphead : AbstractMonoBehaviour
{
	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0008E44B File Offset: 0x0008C84B
	// (set) Token: 0x06000D91 RID: 3473 RVA: 0x0008E452 File Offset: 0x0008C852
	public static Cuphead Current { get; private set; }

	// Token: 0x06000D92 RID: 3474 RVA: 0x0008E45C File Offset: 0x0008C85C
	public static void Init(bool lightInit = false)
	{
		if (Cuphead.Current == null)
		{
			UnityEngine.Object.Instantiate<Cuphead>(Resources.Load<Cuphead>("Core/CupheadCore"));
		}
		else
		{
			if (!Cuphead.didLightInit)
			{
				return;
			}
			Cuphead.didLightInit = false;
		}
		if (lightInit)
		{
			Cuphead.didLightInit = true;
		}
		else
		{
			Cuphead.Current.rewired.gameObject.SetActive(true);
			Cuphead.Current.eventSystem.gameObject.SetActive(true);
			Cuphead.Current.controlMapper.gameObject.SetActive(true);
			PlayerManager.Awake();
			if (!PlatformHelper.PreloadSettingsData)
			{
				OnlineManager.Instance.Init();
			}
			PlmManager.Instance.Init();
			PlayerManager.Init();
			Cuphead.didFullInit = true;
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06000D93 RID: 3475 RVA: 0x0008E522 File Offset: 0x0008C922
	public ScoringEditorData ScoringProperties
	{
		get
		{
			return this.scoringProperties;
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0008E52A File Offset: 0x0008C92A
	// (set) Token: 0x06000D95 RID: 3477 RVA: 0x0008E532 File Offset: 0x0008C932
	public AchievementToastManager achievementToastManager { get; private set; }

	// Token: 0x06000D96 RID: 3478 RVA: 0x0008E53C File Offset: 0x0008C93C
	protected override void Awake()
	{
		base.Awake();
		if (Cuphead.Current == null)
		{
			Cuphead.Current = this;
			base.gameObject.name = base.gameObject.name.Replace("(Clone)", string.Empty);
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.noiseHandler = UnityEngine.Object.Instantiate<AudioNoiseHandler>(this.noiseHandler);
			this.noiseHandler.transform.SetParent(base.transform);
			bool hasBootedUpGame = SettingsData.Data.hasBootedUpGame;
			if (PlatformHelper.ShowAchievements)
			{
				this.achievementToastManager = UnityEngine.Object.Instantiate<AchievementToastManager>(this.achievementToastManagerPrefab);
				this.achievementToastManager.transform.SetParent(base.transform);
			}
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x0008E609 File Offset: 0x0008CA09
	private void OnDestroy()
	{
		if (Cuphead.Current == this)
		{
			Cuphead.Current = null;
		}
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x0008E621 File Offset: 0x0008CA21
	private void Update()
	{
		if (Cuphead.didFullInit)
		{
			PlayerManager.Update();
		}
		Cursor.visible = !Screen.fullScreen;
	}

	// Token: 0x04001707 RID: 5895
	private const string PATH = "Core/CupheadCore";

	// Token: 0x04001708 RID: 5896
	private static bool didLightInit;

	// Token: 0x04001709 RID: 5897
	private static bool didFullInit;

	// Token: 0x0400170B RID: 5899
	[SerializeField]
	private AudioNoiseHandler noiseHandler;

	// Token: 0x0400170C RID: 5900
	[SerializeField]
	private InputManager rewired;

	// Token: 0x0400170D RID: 5901
	public ControlMapper controlMapper;

	// Token: 0x0400170E RID: 5902
	[SerializeField]
	private CupheadEventSystem eventSystem;

	// Token: 0x0400170F RID: 5903
	[SerializeField]
	private CupheadRenderer renderer;

	// Token: 0x04001710 RID: 5904
	[SerializeField]
	private ScoringEditorData scoringProperties;

	// Token: 0x04001711 RID: 5905
	[SerializeField]
	private AchievementToastManager achievementToastManagerPrefab;
}
