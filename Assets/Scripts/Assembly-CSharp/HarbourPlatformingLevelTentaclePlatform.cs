using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D4 RID: 2260
public class HarbourPlatformingLevelTentaclePlatform : LevelPlatform
{
	// Token: 0x060034E4 RID: 13540 RVA: 0x001EC37D File Offset: 0x001EA77D
	public override void AddChild(Transform player)
	{
		base.AddChild(player);
		if (!this.startedSinking)
		{
			base.StartCoroutine(this.sink_cr());
		}
	}

	// Token: 0x060034E5 RID: 13541 RVA: 0x001EC3A0 File Offset: 0x001EA7A0
	private IEnumerator sink_cr()
	{
		this.startedSinking = true;
		float t = 0f;
		Vector2 start = this.drag.transform.position;
		Vector2 end = new Vector2(this.drag.transform.position.x, this.drag.transform.position.y - 500f);
		while (t < this.timeToSink)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / this.timeToSink);
			this.drag.transform.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		UnityEngine.Object.Destroy(this.drag.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x04003D15 RID: 15637
	[SerializeField]
	private Transform drag;

	// Token: 0x04003D16 RID: 15638
	public float timeToSink = 5f;

	// Token: 0x04003D17 RID: 15639
	private bool startedSinking;
}
