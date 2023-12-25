using System;
using UnityEngine;

// Token: 0x020007C0 RID: 1984
public class SaltbakerLevelBGTornado : AbstractMonoBehaviour
{
	// Token: 0x06002CE5 RID: 11493 RVA: 0x001A6FB8 File Offset: 0x001A53B8
	private void Start()
	{
		this.t = UnityEngine.Random.Range(0f, 3.1415927f);
		this.rend.material.SetFloat("_BlurAmount", this.blurAmount * 5f);
		this.rend.material.SetFloat("_BlurLerp", this.blurAmount * 5f);
	}

	// Token: 0x06002CE6 RID: 11494 RVA: 0x001A701C File Offset: 0x001A541C
	private void Update()
	{
		this.t += CupheadTime.Delta;
		base.transform.GetChild(0).localPosition = new Vector3(Mathf.Sin(this.t * this.moveSpeed) * this.moveRange, 0f);
	}

	// Token: 0x0400355A RID: 13658
	[SerializeField]
	private float moveRange;

	// Token: 0x0400355B RID: 13659
	[SerializeField]
	private float moveSpeed;

	// Token: 0x0400355C RID: 13660
	private float t;

	// Token: 0x0400355D RID: 13661
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x0400355E RID: 13662
	[SerializeField]
	private float blurAmount;
}
