using System;
using System.Collections;

// Token: 0x020004AD RID: 1197
public class TestLevelPlatform : LevelPlatform
{
	// Token: 0x06001388 RID: 5000 RVA: 0x000ABFB0 File Offset: 0x000AA3B0
	private void Start()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x000ABFC0 File Offset: 0x000AA3C0
	private IEnumerator loop_cr()
	{
		for (;;)
		{
			yield return base.TweenLocalPositionX(-700f, 700f, 4f, EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenLocalPositionX(700f, -700f, 4f, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x04001C97 RID: 7319
	private const float X = 700f;

	// Token: 0x04001C98 RID: 7320
	private const float TIME = 4f;

	// Token: 0x04001C99 RID: 7321
	private const EaseUtils.EaseType EASE = EaseUtils.EaseType.easeInOutSine;
}
