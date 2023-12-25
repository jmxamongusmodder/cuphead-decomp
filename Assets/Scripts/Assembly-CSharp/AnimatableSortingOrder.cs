using System;
using UnityEngine;

// Token: 0x02000B0C RID: 2828
public class AnimatableSortingOrder : MonoBehaviour
{
	// Token: 0x060044A4 RID: 17572 RVA: 0x00246125 File Offset: 0x00244525
	private void Start()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060044A5 RID: 17573 RVA: 0x00246134 File Offset: 0x00244534
	private void LateUpdate()
	{
		int sortingOrder = (int)this.sortingLayer;
		this.sr.sortingOrder = sortingOrder;
	}

	// Token: 0x04004A5E RID: 19038
	private SpriteRenderer sr;

	// Token: 0x04004A5F RID: 19039
	public float sortingLayer;
}
