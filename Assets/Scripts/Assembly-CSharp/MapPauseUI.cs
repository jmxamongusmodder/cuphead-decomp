using System;

// Token: 0x020009A0 RID: 2464
public class MapPauseUI : LevelPauseGUI
{
	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x060039D1 RID: 14801 RVA: 0x0020E9F0 File Offset: 0x0020CDF0
	protected override bool CanPause
	{
		get
		{
			return base.state != AbstractPauseGUI.State.Animating && MapDifficultySelectStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapConfirmStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapBasicStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && (SpeechBubble.Instance == null || (SpeechBubble.Instance != null && SpeechBubble.Instance.displayState != SpeechBubble.DisplayState.WaitForSelection)) && (!(Map.Current != null) || Map.Current.CurrentState != Map.State.Graveyard);
		}
	}
}
