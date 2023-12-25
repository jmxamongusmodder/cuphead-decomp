using System;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
public class LevelIntroAnimation : AbstractLevelHUDComponent
{
	// Token: 0x06001363 RID: 4963 RVA: 0x000AB468 File Offset: 0x000A9868
	public static LevelIntroAnimation Create(Action callback)
	{
		LevelIntroAnimation levelIntroAnimation = UnityEngine.Object.Instantiate<LevelIntroAnimation>(Level.Current.LevelResources.levelIntro);
		levelIntroAnimation.callback = callback;
		return levelIntroAnimation;
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x000AB494 File Offset: 0x000A9894
	public static LevelIntroAnimation CreateCustom(LevelIntroAnimation prefab, Action callback)
	{
		LevelIntroAnimation levelIntroAnimation = UnityEngine.Object.Instantiate<LevelIntroAnimation>(prefab);
		levelIntroAnimation.callback = callback;
		return levelIntroAnimation;
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x000AB4B0 File Offset: 0x000A98B0
	protected override void Awake()
	{
		base.Awake();
		this._parentToHudCanvas = true;
		base.transform.SetParent(Camera.main.transform, false);
		base.transform.ResetLocalTransforms();
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x000AB4E0 File Offset: 0x000A98E0
	private void StartLevel()
	{
		if (this.callback != null)
		{
			this.callback();
		}
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x000AB4F8 File Offset: 0x000A98F8
	private void OnAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x000AB505 File Offset: 0x000A9905
	public void Play()
	{
		base.GetComponent<Animator>().Play("Intro");
	}

	// Token: 0x04001C7C RID: 7292
	private Action callback;
}
