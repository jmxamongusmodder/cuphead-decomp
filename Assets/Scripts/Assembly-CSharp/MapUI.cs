using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009A1 RID: 2465
public class MapUI : AbstractMonoBehaviour
{
	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x060039D3 RID: 14803 RVA: 0x0020EA91 File Offset: 0x0020CE91
	// (set) Token: 0x060039D4 RID: 14804 RVA: 0x0020EA98 File Offset: 0x0020CE98
	public static MapUI Current { get; private set; }

	// Token: 0x060039D5 RID: 14805 RVA: 0x0020EAA0 File Offset: 0x0020CEA0
	public static MapUI Create()
	{
		return UnityEngine.Object.Instantiate<MapUI>(Map.Current.MapResources.mapUI);
	}

	// Token: 0x060039D6 RID: 14806 RVA: 0x0020EAC3 File Offset: 0x0020CEC3
	protected override void Awake()
	{
		base.Awake();
		MapUI.Current = this;
		CupheadEventSystem.Init();
		LevelGUI.DebugOnDisableGuiEvent += this.OnDisableGUI;
	}

	// Token: 0x060039D7 RID: 14807 RVA: 0x0020EAE8 File Offset: 0x0020CEE8
	private void Start()
	{
		this.uiCamera = UnityEngine.Object.Instantiate<CupheadUICamera>(this.uiCameraPrefab);
		this.uiCamera.transform.SetParent(base.transform);
		this.uiCamera.transform.ResetLocalTransforms();
		this.screenCanvas.worldCamera = this.uiCamera.camera;
		this.sceneCanvas.worldCamera = CupheadMapCamera.Current.camera;
		this.hudCanvas.worldCamera = CupheadMapCamera.Current.camera;
		base.StartCoroutine(this.HandleReturnToMapTooltipEvents());
	}

	// Token: 0x060039D8 RID: 14808 RVA: 0x0020EB79 File Offset: 0x0020CF79
	private void OnDestroy()
	{
		LevelGUI.DebugOnDisableGuiEvent -= this.OnDisableGUI;
		if (MapUI.Current == this)
		{
			MapUI.Current = null;
		}
		this.pauseUI = null;
		this.equipUI = null;
		this.optionsPrefab = null;
	}

	// Token: 0x060039D9 RID: 14809 RVA: 0x0020EBB8 File Offset: 0x0020CFB8
	public void Init(MapPlayerController[] players)
	{
		this.optionsUI = this.optionsPrefab.InstantiatePrefab<OptionsGUI>();
		this.optionsUI.rectTransform.SetParent(this.optionsRoot, false);
		if (PlatformHelper.ShowAchievements)
		{
			this.achievementsUI = this.achievementsPrefab.InstantiatePrefab<AchievementsGUI>();
			this.achievementsUI.rectTransform.SetParent(this.achievementsRoot, false);
		}
		this.pauseUI.Init(false, this.optionsUI, this.achievementsUI);
		this.equipUI.Init(false);
	}

	// Token: 0x060039DA RID: 14810 RVA: 0x0020EC43 File Offset: 0x0020D043
	private void OnDisableGUI()
	{
		this.hudCanvas.enabled = false;
	}

	// Token: 0x060039DB RID: 14811 RVA: 0x0020EC51 File Offset: 0x0020D051
	public void Refresh()
	{
		this.optionsUI.SetupButtons();
	}

	// Token: 0x060039DC RID: 14812 RVA: 0x0020EC5E File Offset: 0x0020D05E
	private void Update()
	{
		if (!MapEventNotification.Current.showing && MapEventNotification.Current.EventQueue.Count > 0)
		{
			MapEventNotification.Current.EventQueue.Dequeue()();
		}
	}

	// Token: 0x060039DD RID: 14813 RVA: 0x0020EC98 File Offset: 0x0020D098
	private IEnumerator HandleReturnToMapTooltipEvents()
	{
		yield return new WaitForSeconds(1f);
		if (PlayerData.Data.shouldShowBoatmanTooltip)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Boatman);
			PlayerData.Data.shouldShowBoatmanTooltip = false;
			PlayerData.SaveCurrentFile();
		}
		if (PlayerData.Data.shouldShowShopkeepTooltip)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.ShopKeep);
			PlayerData.Data.shouldShowShopkeepTooltip = false;
			PlayerData.SaveCurrentFile();
		}
		if (PlayerData.Data.shouldShowTurtleTooltip)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Turtle);
			PlayerData.Data.shouldShowTurtleTooltip = false;
			PlayerData.SaveCurrentFile();
		}
		if (PlayerData.Data.shouldShowForkTooltip)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Professional);
			PlayerData.Data.shouldShowForkTooltip = false;
			PlayerData.SaveCurrentFile();
		}
		yield break;
	}

	// Token: 0x040041C2 RID: 16834
	[SerializeField]
	private MapPauseUI pauseUI;

	// Token: 0x040041C3 RID: 16835
	[SerializeField]
	private MapEquipUI equipUI;

	// Token: 0x040041C4 RID: 16836
	[SerializeField]
	private OptionsGUI optionsPrefab;

	// Token: 0x040041C5 RID: 16837
	[SerializeField]
	private RectTransform optionsRoot;

	// Token: 0x040041C6 RID: 16838
	[SerializeField]
	private AchievementsGUI achievementsPrefab;

	// Token: 0x040041C7 RID: 16839
	[SerializeField]
	private RectTransform achievementsRoot;

	// Token: 0x040041C8 RID: 16840
	[Space(10f)]
	[SerializeField]
	public Canvas sceneCanvas;

	// Token: 0x040041C9 RID: 16841
	[SerializeField]
	public Canvas screenCanvas;

	// Token: 0x040041CA RID: 16842
	[SerializeField]
	public Canvas hudCanvas;

	// Token: 0x040041CB RID: 16843
	[Space(10f)]
	[SerializeField]
	private CupheadUICamera uiCameraPrefab;

	// Token: 0x040041CC RID: 16844
	private OptionsGUI optionsUI;

	// Token: 0x040041CD RID: 16845
	private AchievementsGUI achievementsUI;

	// Token: 0x040041CE RID: 16846
	private CupheadUICamera uiCamera;
}
