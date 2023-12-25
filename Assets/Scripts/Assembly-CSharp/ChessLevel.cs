using System;
using UnityEngine;

// Token: 0x02000544 RID: 1348
public abstract class ChessLevel : Level
{
	// Token: 0x060018C2 RID: 6338 RVA: 0x00057B96 File Offset: 0x00055F96
	protected override void Awake()
	{
		this.originalMode = Level.CurrentMode;
		Level.SetCurrentMode(Level.Mode.Normal);
		base.Awake();
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x00057BAF File Offset: 0x00055FAF
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Level.SetCurrentMode(this.originalMode);
		this.levelIntroAnimation = null;
	}

	// Token: 0x060018C4 RID: 6340 RVA: 0x00057BC9 File Offset: 0x00055FC9
	protected override LevelIntroAnimation CreateLevelIntro(Action callback)
	{
		return LevelIntroAnimation.CreateCustom(this.levelIntroAnimation, callback);
	}

	// Token: 0x040021CF RID: 8655
	[SerializeField]
	private LevelIntroAnimation levelIntroAnimation;

	// Token: 0x040021D0 RID: 8656
	private Level.Mode originalMode;
}
