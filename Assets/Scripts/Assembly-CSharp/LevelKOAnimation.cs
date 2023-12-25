using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public class LevelKOAnimation : AbstractLevelHUDComponent
{
	// Token: 0x0600136A RID: 4970 RVA: 0x000AB51F File Offset: 0x000A991F
	public static LevelKOAnimation Create(bool isMaus)
	{
		LevelKOAnimation.isMausoleum = isMaus;
		return UnityEngine.Object.Instantiate<LevelKOAnimation>(Level.Current.LevelResources.levelKO);
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x000AB53B File Offset: 0x000A993B
	protected override void Awake()
	{
		base.Awake();
		this._parentToHudCanvas = true;
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x000AB54A File Offset: 0x000A994A
	private void OnAnimComplete()
	{
		this.state = LevelKOAnimation.State.Complete;
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x000AB554 File Offset: 0x000A9954
	public IEnumerator anim_cr()
	{
		base.GetComponent<Animator>().SetTrigger(LevelKOAnimation.isMausoleum ? "StartMaus" : "Start");
		while (this.state == LevelKOAnimation.State.Animating)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001C7D RID: 7293
	private const float FRAME_DELAY = 5f;

	// Token: 0x04001C7E RID: 7294
	private LevelKOAnimation.State state;

	// Token: 0x04001C7F RID: 7295
	private static bool isMausoleum;

	// Token: 0x020004A7 RID: 1191
	public enum State
	{
		// Token: 0x04001C81 RID: 7297
		Animating,
		// Token: 0x04001C82 RID: 7298
		Complete
	}
}
