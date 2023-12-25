using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E6 RID: 2278
public class MountainPlatformingLevelMinerRope : AbstractPausableComponent
{
	// Token: 0x06003563 RID: 13667 RVA: 0x001F1F3C File Offset: 0x001F033C
	public void PullRope(float ascendTime, Vector2 startPos)
	{
		base.StartCoroutine(this.pull_up_rope_cr(ascendTime, startPos));
	}

	// Token: 0x06003564 RID: 13668 RVA: 0x001F1F50 File Offset: 0x001F0350
	public IEnumerator pull_up_rope_cr(float ascendTime, Vector2 startPos)
	{
		float t = 0f;
		Vector3 end = new Vector3(base.transform.position.x, startPos.y + 400f);
		Vector3 start = base.transform.position;
		while (t < ascendTime)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / ascendTime);
			base.transform.position = Vector2.Lerp(start, end, val);
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}
}
