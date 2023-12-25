using System;
using UnityEngine;

// Token: 0x020005FF RID: 1535
public class DragonLevelScrollingSprite : ScrollingSprite
{
	// Token: 0x06001E87 RID: 7815 RVA: 0x00114771 File Offset: 0x00112B71
	protected override void Awake()
	{
		base.Awake();
		this.playbackSpeed = 0f;
	}

	// Token: 0x06001E88 RID: 7816 RVA: 0x00114784 File Offset: 0x00112B84
	protected override void Update()
	{
		this.playbackSpeed = Mathf.Lerp(0.1f, 1f, DragonLevel.SPEED);
		base.Update();
	}

	// Token: 0x04002762 RID: 10082
	private const float MIN_SPEED = 0.1f;
}
