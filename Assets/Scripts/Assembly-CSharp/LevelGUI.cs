using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000481 RID: 1153
public class LevelGUI : AbstractMonoBehaviour
{
	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x060011D5 RID: 4565 RVA: 0x000A715D File Offset: 0x000A555D
	// (set) Token: 0x060011D6 RID: 4566 RVA: 0x000A7164 File Offset: 0x000A5564
	public static LevelGUI Current { get; private set; }

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x060011D7 RID: 4567 RVA: 0x000A716C File Offset: 0x000A556C
	// (remove) Token: 0x060011D8 RID: 4568 RVA: 0x000A71A0 File Offset: 0x000A55A0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action DebugOnDisableGuiEvent;

	// Token: 0x060011D9 RID: 4569 RVA: 0x000A71D4 File Offset: 0x000A55D4
	public static void DebugDisableGUI()
	{
		if (LevelGUI.DebugOnDisableGuiEvent != null)
		{
			LevelGUI.DebugOnDisableGuiEvent();
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x060011DA RID: 4570 RVA: 0x000A71EA File Offset: 0x000A55EA
	public Canvas Canvas
	{
		get
		{
			return this.canvas;
		}
	}

	// Token: 0x060011DB RID: 4571 RVA: 0x000A71F2 File Offset: 0x000A55F2
	protected override void Awake()
	{
		base.Awake();
		LevelGUI.Current = this;
	}

	// Token: 0x060011DC RID: 4572 RVA: 0x000A7200 File Offset: 0x000A5600
	private void Start()
	{
		this.uiCamera = UnityEngine.Object.Instantiate<CupheadUICamera>(this.uiCameraPrefab);
		this.uiCamera.transform.SetParent(base.transform);
		this.uiCamera.transform.ResetLocalTransforms();
		this.canvas.worldCamera = this.uiCamera.camera;
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x000A725A File Offset: 0x000A565A
	private void OnDestroy()
	{
		this.pause = null;
		this.options = null;
		this.restartTowerConfirm = null;
		if (LevelGUI.Current == this)
		{
			LevelGUI.Current = null;
		}
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x000A7288 File Offset: 0x000A5688
	public void LevelInit()
	{
		this.options = this.optionsPrefab.InstantiatePrefab<OptionsGUI>();
		this.options.rectTransform.SetParent(this.optionsRoot, false);
		if (PlatformHelper.ShowAchievements)
		{
			this.achievements = this.achievementsPrefab.InstantiatePrefab<AchievementsGUI>();
			this.achievements.rectTransform.SetParent(this.achievementsRoot, false);
		}
		if (Level.IsTowerOfPower)
		{
			this.restartTowerConfirm = this.restartTowerConfirmPrefab.InstantiatePrefab<RestartTowerConfirmGUI>();
			this.restartTowerConfirm.rectTransform.SetParent(this.restartTowerConfirmRoot, false);
		}
		this.pause.Init(true, this.options, this.achievements, this.restartTowerConfirm);
	}

	// Token: 0x04001B58 RID: 7000
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04001B59 RID: 7001
	[SerializeField]
	private LevelPauseGUI pause;

	// Token: 0x04001B5A RID: 7002
	[SerializeField]
	private LevelGameOverGUI gameOver;

	// Token: 0x04001B5B RID: 7003
	[SerializeField]
	private OptionsGUI optionsPrefab;

	// Token: 0x04001B5C RID: 7004
	[SerializeField]
	private RectTransform optionsRoot;

	// Token: 0x04001B5D RID: 7005
	[SerializeField]
	private RestartTowerConfirmGUI restartTowerConfirmPrefab;

	// Token: 0x04001B5E RID: 7006
	[SerializeField]
	private RectTransform restartTowerConfirmRoot;

	// Token: 0x04001B5F RID: 7007
	[SerializeField]
	private AchievementsGUI achievementsPrefab;

	// Token: 0x04001B60 RID: 7008
	[SerializeField]
	private RectTransform achievementsRoot;

	// Token: 0x04001B61 RID: 7009
	private OptionsGUI options;

	// Token: 0x04001B62 RID: 7010
	private AchievementsGUI achievements;

	// Token: 0x04001B63 RID: 7011
	private RestartTowerConfirmGUI restartTowerConfirm;

	// Token: 0x04001B64 RID: 7012
	[Space(10f)]
	[SerializeField]
	private CupheadUICamera uiCameraPrefab;

	// Token: 0x04001B65 RID: 7013
	private CupheadUICamera uiCamera;
}
