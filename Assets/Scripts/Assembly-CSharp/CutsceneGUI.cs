using System;
using UnityEngine;

// Token: 0x02000409 RID: 1033
public class CutsceneGUI : AbstractMonoBehaviour
{
	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06000E6C RID: 3692 RVA: 0x000936EA File Offset: 0x00091AEA
	// (set) Token: 0x06000E6D RID: 3693 RVA: 0x000936F1 File Offset: 0x00091AF1
	public static CutsceneGUI Current { get; private set; }

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06000E6E RID: 3694 RVA: 0x000936F9 File Offset: 0x00091AF9
	public Canvas Canvas
	{
		get
		{
			return this.canvas;
		}
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x00093701 File Offset: 0x00091B01
	protected override void Awake()
	{
		base.Awake();
		CutsceneGUI.Current = this;
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x00093710 File Offset: 0x00091B10
	private void Start()
	{
		this.uiCamera = UnityEngine.Object.Instantiate<CupheadUICamera>(this.uiCameraPrefab);
		this.uiCamera.transform.SetParent(base.transform);
		this.uiCamera.transform.ResetLocalTransforms();
		this.canvas.worldCamera = this.uiCamera.camera;
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x0009376A File Offset: 0x00091B6A
	private void OnDestroy()
	{
		if (CutsceneGUI.Current == this)
		{
			CutsceneGUI.Current = null;
		}
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x00093782 File Offset: 0x00091B82
	public void CutseneInit()
	{
		this.pause.Init(false);
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x00093790 File Offset: 0x00091B90
	protected virtual void CutsceneSnapshot()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Cutscene.ToString(), 0.15f);
	}

	// Token: 0x040017AC RID: 6060
	public const string PATH = "UI/Cutscene_UI";

	// Token: 0x040017AE RID: 6062
	[SerializeField]
	private Canvas canvas;

	// Token: 0x040017AF RID: 6063
	[SerializeField]
	public CutscenePauseGUI pause;

	// Token: 0x040017B0 RID: 6064
	[Space(10f)]
	[SerializeField]
	private CupheadUICamera uiCameraPrefab;

	// Token: 0x040017B1 RID: 6065
	private CupheadUICamera uiCamera;
}
