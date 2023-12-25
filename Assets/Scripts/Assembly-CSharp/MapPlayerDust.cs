using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200097C RID: 2428
public class MapPlayerDust : Effect
{
	// Token: 0x060038AD RID: 14509 RVA: 0x00204778 File Offset: 0x00202B78
	public Effect Create(Vector3 position, float offsetRotation, bool isLeft, int sortingOrder)
	{
		Vector3 vector = Vector3.right * this.offset.x;
		if (isLeft)
		{
			vector *= -1f;
		}
		Vector3 b = Quaternion.Euler(offsetRotation, 0f, 0f) * vector;
		b.y += this.offset.y;
		this.spriteRenderer.sortingOrder = sortingOrder;
		position.z = position.y - 0.01f;
		return this.Create(position + b);
	}

	// Token: 0x060038AE RID: 14510 RVA: 0x0020480C File Offset: 0x00202C0C
	public override void Initialize(Vector3 position, Vector3 scale, bool randomR)
	{
		base.Initialize(position, scale * UnityEngine.Random.Range(this.scaleRange.min, this.scaleRange.max), randomR);
		Color color = this.spriteRenderer.color;
		color.a *= UnityEngine.Random.Range(this.opacityRange.min, this.opacityRange.max);
		this.spriteRenderer.color = color;
		base.animator.SetTrigger("startAnim");
		base.StartCoroutine(this.dust_cr());
	}

	// Token: 0x060038AF RID: 14511 RVA: 0x002048A0 File Offset: 0x00202CA0
	private IEnumerator dust_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.OnEffectComplete();
		yield break;
	}

	// Token: 0x04004065 RID: 16485
	private const string StartAnimTrigger = "startAnim";

	// Token: 0x04004066 RID: 16486
	[SerializeField]
	private MinMax scaleRange;

	// Token: 0x04004067 RID: 16487
	[SerializeField]
	private MinMax opacityRange;

	// Token: 0x04004068 RID: 16488
	[SerializeField]
	private Vector3 offset;

	// Token: 0x04004069 RID: 16489
	[SerializeField]
	private SpriteRenderer spriteRenderer;
}
