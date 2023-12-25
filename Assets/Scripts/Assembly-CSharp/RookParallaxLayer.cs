using System;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
public class RookParallaxLayer : ParallaxLayer
{
	// Token: 0x06000D70 RID: 3440 RVA: 0x0008E128 File Offset: 0x0008C528
	protected override void UpdateComparative()
	{
		Vector3 position = base.transform.position;
		position.x = base._offset.x + this._camera.transform.position.x * this.percentage;
		position.y = base._offset.y + this._camera.transform.position.y * this.percentage * this.yModifier;
		base.transform.position = position;
	}

	// Token: 0x04001701 RID: 5889
	[SerializeField]
	private float yModifier = 0.5f;
}
