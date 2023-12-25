using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class ChessQueenLevelLionDeathPart : AbstractPausableComponent
{
	// Token: 0x06001913 RID: 6419 RVA: 0x000E3512 File Offset: 0x000E1912
	private void Start()
	{
		base.animator.Play("Loop", 0, UnityEngine.Random.Range(0f, 1f));
		base.StartCoroutine(this.grow_cr());
	}

	// Token: 0x06001914 RID: 6420 RVA: 0x000E3544 File Offset: 0x000E1944
	private IEnumerator grow_cr()
	{
		float elapsed = 0f;
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.041666668f, false);
		for (;;)
		{
			yield return wait;
			elapsed += wait.frameTime + wait.accumulator;
			Vector3 scale = base.transform.localScale;
			scale.x = (1f + elapsed * this.growthSpeed) * Mathf.Sign(scale.x);
			scale.y = 1f + elapsed * this.growthSpeed;
			base.transform.localScale = scale;
		}
		yield break;
	}

	// Token: 0x0400222F RID: 8751
	[SerializeField]
	private float growthSpeed;
}
