using System;
using UnityEngine;

// Token: 0x02000905 RID: 2309
public class PlatformingLevelIntroAnimation : AbstractLevelHUDComponent
{
	// Token: 0x0600362A RID: 13866 RVA: 0x001F73F8 File Offset: 0x001F57F8
	public static PlatformingLevelIntroAnimation Create(Action callback)
	{
		PlatformingLevelIntroAnimation platformingLevelIntroAnimation = UnityEngine.Object.Instantiate<PlatformingLevelIntroAnimation>(Level.Current.LevelResources.platformingIntro);
		platformingLevelIntroAnimation.callback = callback;
		return platformingLevelIntroAnimation;
	}

	// Token: 0x0600362B RID: 13867 RVA: 0x001F7422 File Offset: 0x001F5822
	protected override void Awake()
	{
		base.Awake();
		this._parentToHudCanvas = true;
		base.transform.SetParent(Camera.main.transform, false);
		base.transform.ResetLocalTransforms();
	}

	// Token: 0x0600362C RID: 13868 RVA: 0x001F7452 File Offset: 0x001F5852
	private void StartLevel()
	{
		if (this.callback != null)
		{
			this.callback();
		}
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x001F746A File Offset: 0x001F586A
	private void OnAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x001F7477 File Offset: 0x001F5877
	public void Play()
	{
		base.GetComponent<Animator>().Play("Intro");
	}

	// Token: 0x04003E2B RID: 15915
	private Action callback;
}
