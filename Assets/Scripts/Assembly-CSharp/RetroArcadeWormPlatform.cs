using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200076A RID: 1898
public class RetroArcadeWormPlatform : LevelPlatform
{
	// Token: 0x06002951 RID: 10577 RVA: 0x0018185E File Offset: 0x0017FC5E
	public void Rise()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002952 RID: 10578 RVA: 0x00181870 File Offset: 0x0017FC70
	private IEnumerator move_cr()
	{
		float moveTime = 1f;
		float t = 0f;
		while (t < moveTime)
		{
			t += CupheadTime.FixedDelta;
			base.transform.AddPosition(0f, 50f * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x04003253 RID: 12883
	private const float MOVE_Y = 50f;

	// Token: 0x04003254 RID: 12884
	private const float MOVE_Y_SPEED = 50f;
}
