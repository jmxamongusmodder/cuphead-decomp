using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000541 RID: 1345
public class ChessKnightLevelKnightDeathArmor : AbstractPausableComponent
{
	// Token: 0x060018BA RID: 6330 RVA: 0x000E036A File Offset: 0x000DE76A
	private void Start()
	{
		base.animator.Play(this.type.ToString());
		base.StartCoroutine(this.grow_cr());
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x000E0398 File Offset: 0x000DE798
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

	// Token: 0x040021C5 RID: 8645
	[SerializeField]
	private ChessKnightLevelKnightDeathArmor.Type type;

	// Token: 0x040021C6 RID: 8646
	[SerializeField]
	private float growthSpeed;

	// Token: 0x02000542 RID: 1346
	public enum Type
	{
		// Token: 0x040021C8 RID: 8648
		Helmet,
		// Token: 0x040021C9 RID: 8649
		Shield,
		// Token: 0x040021CA RID: 8650
		Sword
	}
}
