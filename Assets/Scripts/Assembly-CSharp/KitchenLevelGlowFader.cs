using System;
using UnityEngine;

// Token: 0x020006D1 RID: 1745
public class KitchenLevelGlowFader : MonoBehaviour
{
	// Token: 0x06002528 RID: 9512 RVA: 0x0015C804 File Offset: 0x0015AC04
	private void Update()
	{
		this.t += CupheadTime.Delta;
		this.rend.color = new Color(1f, 1f, 1f, Mathf.Lerp(this.alphaMin, 1f, (Mathf.Sin(this.t * this.speedModifier) + 1f) / 2f));
	}

	// Token: 0x04002DD0 RID: 11728
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04002DD1 RID: 11729
	[SerializeField]
	[Range(0f, 6.2831855f)]
	private float startOffset;

	// Token: 0x04002DD2 RID: 11730
	private float t;

	// Token: 0x04002DD3 RID: 11731
	[SerializeField]
	private float speedModifier = 1f;

	// Token: 0x04002DD4 RID: 11732
	[SerializeField]
	private float alphaMin = 0.8f;
}
