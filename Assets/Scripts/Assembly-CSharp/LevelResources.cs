using System;

// Token: 0x020004A9 RID: 1193
public class LevelResources : AbstractMonoBehaviour
{
	// Token: 0x0600137A RID: 4986 RVA: 0x000AB99C File Offset: 0x000A9D9C
	private void OnDestroy()
	{
		this.levelHUD = null;
		this.levelGUI = null;
		this.levelAudio = null;
		this.levelBossDeathExplosion = null;
		this.levelPlayer = null;
		this.planePlayer = null;
		this.joinEffect = null;
		this.platformingIntro = null;
		this.platformingWin = null;
		this.levelIntro = null;
		this.levelUIInteractionDialogue = null;
	}

	// Token: 0x04001C87 RID: 7303
	public const string EDITOR_PATH = "Assets/_CUPHEAD/Prefabs/LevelResources/Level_Resources.prefab";

	// Token: 0x04001C88 RID: 7304
	public LevelHUD levelHUD;

	// Token: 0x04001C89 RID: 7305
	public LevelGUI levelGUI;

	// Token: 0x04001C8A RID: 7306
	public LevelAudio levelAudio;

	// Token: 0x04001C8B RID: 7307
	public Effect levelBossDeathExplosion;

	// Token: 0x04001C8C RID: 7308
	public LevelPlayerController levelPlayer;

	// Token: 0x04001C8D RID: 7309
	public PlanePlayerController planePlayer;

	// Token: 0x04001C8E RID: 7310
	public PlayerJoinEffect joinEffect;

	// Token: 0x04001C8F RID: 7311
	public PlatformingLevelIntroAnimation platformingIntro;

	// Token: 0x04001C90 RID: 7312
	public PlatformingLevelWinAnimation platformingWin;

	// Token: 0x04001C91 RID: 7313
	public LevelIntroAnimation levelIntro;

	// Token: 0x04001C92 RID: 7314
	public LevelKOAnimation levelKO;

	// Token: 0x04001C93 RID: 7315
	public LevelUIInteractionDialogue levelUIInteractionDialogue;
}
