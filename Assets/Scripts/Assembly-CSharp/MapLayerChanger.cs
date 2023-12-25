using System;
using UnityEngine;

// Token: 0x0200093B RID: 2363
public class MapLayerChanger : AbstractMonoBehaviour
{
	// Token: 0x0600374E RID: 14158 RVA: 0x001FD450 File Offset: 0x001FB850
	private void OnTriggerEnter2D(Collider2D collider)
	{
		SpriteRenderer[] componentsInChildren = collider.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			spriteRenderer.sortingOrder = this.sortingOrder;
		}
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x001FD48C File Offset: 0x001FB88C
	private void OnTriggerStay2D(Collider2D collider)
	{
		SpriteRenderer[] componentsInChildren = collider.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			spriteRenderer.sortingOrder = this.sortingOrder;
		}
	}

	// Token: 0x04003F6A RID: 16234
	[SerializeField]
	private int sortingOrder;
}
