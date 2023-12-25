using System;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
public abstract class Cutscene : AbstractPausableComponent
{
	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06000DBE RID: 3518 RVA: 0x0008F4F2 File Offset: 0x0008D8F2
	// (set) Token: 0x06000DBF RID: 3519 RVA: 0x0008F4F9 File Offset: 0x0008D8F9
	public static Cutscene Current { get; private set; }

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0008F501 File Offset: 0x0008D901
	// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x0008F508 File Offset: 0x0008D908
	public static SceneLoader.Properties transitionProperties { get; private set; } = new SceneLoader.Properties();

	// Token: 0x06000DC2 RID: 3522 RVA: 0x0008F510 File Offset: 0x0008D910
	public static void Load(Levels level, Scenes cutscene, SceneLoader.Transition transitionStart, SceneLoader.Transition transitionEnd, SceneLoader.Icon icon = SceneLoader.Icon.Hourglass)
	{
		Cutscene.transitionProperties.transitionStart = transitionStart;
		Cutscene.transitionProperties.transitionEnd = transitionEnd;
		Cutscene.transitionProperties.icon = icon;
		Cutscene.mode = Cutscene.Mode.Level;
		Cutscene.levelAfterCutscene = level;
		SceneLoader.LoadScene(cutscene, transitionStart, transitionEnd, SceneLoader.Icon.None, null);
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x0008F54A File Offset: 0x0008D94A
	public static void Load(Scenes scene, Scenes cutscene, SceneLoader.Transition transitionStart, SceneLoader.Transition transitionEnd, SceneLoader.Icon icon = SceneLoader.Icon.Hourglass)
	{
		Cutscene.transitionProperties.transitionStart = transitionStart;
		Cutscene.transitionProperties.transitionEnd = transitionEnd;
		Cutscene.transitionProperties.icon = icon;
		Cutscene.mode = Cutscene.Mode.Scene;
		Cutscene.sceneAfterCutscene = scene;
		SceneLoader.LoadScene(cutscene, transitionStart, transitionEnd, SceneLoader.Icon.None, null);
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x0008F584 File Offset: 0x0008D984
	protected override void Awake()
	{
		base.Awake();
		Cutscene.Current = this;
		Cuphead.Init(false);
		this.CreateUI();
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x0008F5A0 File Offset: 0x0008D9A0
	protected virtual void Start()
	{
		CupheadTime.SetAll(1f);
		InterruptingPrompt.SetCanInterrupt(true);
		this.CreateCamera();
		this.gui.CutseneInit();
		this.SetRichPresence();
		if (this.translationText != null)
		{
			this.translationText.SetActive(Localization.language != Localization.Languages.English);
		}
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x0008F5FB File Offset: 0x0008D9FB
	private void CreateUI()
	{
		this.gui = UnityEngine.Object.FindObjectOfType<CutsceneGUI>();
		if (this.gui == null)
		{
			this.gui = Resources.Load<CutsceneGUI>("UI/Cutscene_UI").InstantiatePrefab<CutsceneGUI>();
		}
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x0008F634 File Offset: 0x0008DA34
	private void CreateCamera()
	{
		CupheadCutsceneCamera cupheadCutsceneCamera = UnityEngine.Object.FindObjectOfType<CupheadCutsceneCamera>();
		cupheadCutsceneCamera.Init();
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x0008F650 File Offset: 0x0008DA50
	protected virtual void OnCutsceneOver()
	{
		Cutscene.Mode mode = Cutscene.mode;
		if (mode != Cutscene.Mode.Scene)
		{
			if (mode == Cutscene.Mode.Level)
			{
				SceneLoader.LoadLevel(Cutscene.levelAfterCutscene, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
			}
		}
		else
		{
			SceneLoader.LoadScene(Cutscene.sceneAfterCutscene, SceneLoader.Transition.Fade, Cutscene.transitionProperties.transitionEnd, Cutscene.transitionProperties.icon, null);
		}
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0008F6B1 File Offset: 0x0008DAB1
	public void Skip()
	{
		this.OnCutsceneOver();
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x0008F6B9 File Offset: 0x0008DAB9
	protected virtual void SetRichPresence()
	{
		OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Cutscene", true);
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x0008F6D5 File Offset: 0x0008DAD5
	public bool IsTranslationTextActive()
	{
		return this.translationText.activeSelf;
	}

	// Token: 0x0400172E RID: 5934
	[SerializeField]
	private GameObject translationText;

	// Token: 0x04001730 RID: 5936
	private static Scenes sceneAfterCutscene;

	// Token: 0x04001731 RID: 5937
	private static Levels levelAfterCutscene;

	// Token: 0x04001732 RID: 5938
	private static Cutscene.Mode mode;

	// Token: 0x04001733 RID: 5939
	protected CutsceneGUI gui;

	// Token: 0x020003F5 RID: 1013
	public enum Mode
	{
		// Token: 0x04001735 RID: 5941
		Scene,
		// Token: 0x04001736 RID: 5942
		Level
	}
}
