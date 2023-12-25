using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200082D RID: 2093
public class TrainLevelTrackBumper : AbstractPausableComponent
{
	// Token: 0x06003099 RID: 12441 RVA: 0x001C98FA File Offset: 0x001C7CFA
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x0600309A RID: 12442 RVA: 0x001C9910 File Offset: 0x001C7D10
	private IEnumerator main_cr()
	{
		float startingY = base.transform.position.y;
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.Delta;
			float d = (base.transform.position.x + 6000f * t) % 4500f;
			float y = startingY;
			if (d < 600f)
			{
				float num = d / 600f;
				y += Mathf.Sin(num * 3.1415927f) * 3f;
			}
			base.transform.SetPosition(null, new float?(y), null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003937 RID: 14647
	private const float bumpSpeed = 6000f;

	// Token: 0x04003938 RID: 14648
	private const float bumpHeight = 3f;

	// Token: 0x04003939 RID: 14649
	private const float bumpDuration = 0.1f;

	// Token: 0x0400393A RID: 14650
	private const float bumpPeriod = 0.75f;
}
