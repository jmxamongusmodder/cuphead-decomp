using System;
using UnityEngine;

// Token: 0x02000B21 RID: 2849
public class SpriteOrderChanger : AbstractMonoBehaviour
{
	// Token: 0x060044F3 RID: 17651 RVA: 0x0024763B File Offset: 0x00245A3B
	protected override void Awake()
	{
		base.Awake();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060044F4 RID: 17652 RVA: 0x0024764F File Offset: 0x00245A4F
	private void Update()
	{
		if (this.t >= this.frameDelay)
		{
			this.t = 0;
			this.spriteRenderer.sortingOrder += this.change;
		}
		this.t++;
	}

	// Token: 0x04004ACA RID: 19146
	public int change = 1;

	// Token: 0x04004ACB RID: 19147
	public int frameDelay = 2;

	// Token: 0x04004ACC RID: 19148
	private SpriteRenderer spriteRenderer;

	// Token: 0x04004ACD RID: 19149
	private int t;
}
