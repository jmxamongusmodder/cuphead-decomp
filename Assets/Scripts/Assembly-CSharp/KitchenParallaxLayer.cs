using System;
using UnityEngine;

// Token: 0x020003E5 RID: 997
public class KitchenParallaxLayer : ParallaxLayer
{
	// Token: 0x06000D61 RID: 3425 RVA: 0x0008DF11 File Offset: 0x0008C311
	protected override void Start()
	{
		base.Start();
		this.level = (Level.Current as SaltbakerLevel);
	}

	// Token: 0x06000D62 RID: 3426 RVA: 0x0008DF2C File Offset: 0x0008C32C
	protected override void UpdateMinMax()
	{
		Vector3 position = base.transform.position;
		if (!this.ignoreX)
		{
			Vector2 vector = this._camera.transform.position;
			Vector2 zero = Vector2.zero;
			float num = vector.x + Mathf.Abs(this._camera.Left);
			float num2 = this._camera.Right + Mathf.Abs(this._camera.Left);
			zero.x = num / num2;
			if (float.IsNaN(zero.x))
			{
				zero.x = 0.5f;
			}
			position.x = Mathf.Lerp(this.bottomLeft.x, this.topRight.x, zero.x) + this._camera.transform.position.x;
		}
		position.y = Mathf.Lerp(this.startY, this.endY, this.level.yScrollPos);
		base.transform.position = position;
	}

	// Token: 0x040016EA RID: 5866
	[SerializeField]
	private float startY;

	// Token: 0x040016EB RID: 5867
	[SerializeField]
	private float endY;

	// Token: 0x040016EC RID: 5868
	[SerializeField]
	private bool ignoreX;

	// Token: 0x040016ED RID: 5869
	private SaltbakerLevel level;
}
