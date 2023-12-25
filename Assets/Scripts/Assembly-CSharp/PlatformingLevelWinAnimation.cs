using System;
using UnityEngine;

// Token: 0x0200090C RID: 2316
public class PlatformingLevelWinAnimation : AbstractLevelHUDComponent
{
	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06003654 RID: 13908 RVA: 0x001F7DB2 File Offset: 0x001F61B2
	// (set) Token: 0x06003655 RID: 13909 RVA: 0x001F7DBA File Offset: 0x001F61BA
	public PlatformingLevelWinAnimation.State CurrentState { get; private set; }

	// Token: 0x06003656 RID: 13910 RVA: 0x001F7DC3 File Offset: 0x001F61C3
	public static PlatformingLevelWinAnimation Create()
	{
		return UnityEngine.Object.Instantiate<PlatformingLevelWinAnimation>(Level.Current.LevelResources.platformingWin);
	}

	// Token: 0x06003657 RID: 13911 RVA: 0x001F7DD9 File Offset: 0x001F61D9
	protected override void Awake()
	{
		base.Awake();
		this._parentToHudCanvas = true;
	}

	// Token: 0x06003658 RID: 13912 RVA: 0x001F7DE8 File Offset: 0x001F61E8
	private void OnAnimComplete()
	{
		this.CurrentState = PlatformingLevelWinAnimation.State.Complete;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04003E44 RID: 15940
	private const float FRAME_DELAY = 5f;

	// Token: 0x0200090D RID: 2317
	public enum State
	{
		// Token: 0x04003E47 RID: 15943
		Paused,
		// Token: 0x04003E48 RID: 15944
		Unpaused,
		// Token: 0x04003E49 RID: 15945
		Complete
	}
}
