using System;
using System.Diagnostics;

// Token: 0x0200040A RID: 1034
public class CutscenePauseGUI : AbstractPauseGUI
{
	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06000E75 RID: 3701 RVA: 0x00093F50 File Offset: 0x00092350
	// (remove) Token: 0x06000E76 RID: 3702 RVA: 0x00093F84 File Offset: 0x00092384
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnPauseEvent;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06000E77 RID: 3703 RVA: 0x00093FB8 File Offset: 0x000923B8
	// (remove) Token: 0x06000E78 RID: 3704 RVA: 0x00093FEC File Offset: 0x000923EC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnUnpauseEvent;

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06000E79 RID: 3705 RVA: 0x00094020 File Offset: 0x00092420
	protected override bool CanPause
	{
		get
		{
			return PauseManager.state != PauseManager.State.Paused && this.pauseAllowed;
		}
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x00094036 File Offset: 0x00092436
	protected override void OnPause()
	{
		base.OnPause();
		CupheadCutsceneCamera.Current.StartBlur();
		if (CutscenePauseGUI.OnPauseEvent != null)
		{
			CutscenePauseGUI.OnPauseEvent();
		}
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x0009405C File Offset: 0x0009245C
	protected override void OnUnpause()
	{
		base.OnUnpause();
		CupheadCutsceneCamera.Current.EndBlur();
		if (CutscenePauseGUI.OnUnpauseEvent != null)
		{
			CutscenePauseGUI.OnUnpauseEvent();
		}
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00094082 File Offset: 0x00092482
	private void OnDestroy()
	{
		PauseManager.Unpause();
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0009408C File Offset: 0x0009248C
	protected override void Update()
	{
		base.Update();
		if (base.state != AbstractPauseGUI.State.Paused)
		{
			return;
		}
		if (base.GetButtonDown(CupheadButton.Pause))
		{
			this.Unpause();
			return;
		}
		if (base.GetButtonDown(CupheadButton.Cancel))
		{
			this.Unpause();
			return;
		}
		if (base.GetButtonDown(CupheadButton.Accept))
		{
			Cutscene.Current.Skip();
			return;
		}
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x000940EB File Offset: 0x000924EB
	private void Restart()
	{
		base.state = AbstractPauseGUI.State.Animating;
		SceneLoader.ReloadLevel();
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x000940F9 File Offset: 0x000924F9
	private void StartNewGame()
	{
		base.state = AbstractPauseGUI.State.Animating;
		PlayerManager.ResetPlayers();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x00094111 File Offset: 0x00092511
	protected override void InAnimation(float i)
	{
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x00094113 File Offset: 0x00092513
	protected override void OutAnimation(float i)
	{
	}

	// Token: 0x040017B4 RID: 6068
	public bool pauseAllowed = true;
}
