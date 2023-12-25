using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B13 RID: 2835
public class OneTimeScrollingSprite : AbstractPausableComponent
{
	// Token: 0x060044C2 RID: 17602 RVA: 0x0024672F File Offset: 0x00244B2F
	private void Start()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x060044C3 RID: 17603 RVA: 0x00246740 File Offset: 0x00244B40
	private IEnumerator loop_cr()
	{
		SpriteRenderer spriteRenderer = base.GetComponent<SpriteRenderer>();
		while (base.transform.position.x + spriteRenderer.bounds.size.x / 2f > -1280f)
		{
			if (this.OutCondition != null && this.OutCondition())
			{
				yield break;
			}
			Vector2 position = base.transform.localPosition;
			position.x -= this.speed * CupheadTime.Delta;
			base.transform.localPosition = position;
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04004A7D RID: 19069
	private const float X_OUT = -1280f;

	// Token: 0x04004A7E RID: 19070
	public float speed;

	// Token: 0x04004A7F RID: 19071
	public Func<bool> OutCondition;
}
