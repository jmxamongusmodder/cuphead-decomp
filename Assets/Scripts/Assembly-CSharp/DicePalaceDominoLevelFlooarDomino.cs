using System;
using UnityEngine;

// Token: 0x020005BA RID: 1466
public class DicePalaceDominoLevelFlooarDomino : MonoBehaviour
{
	// Token: 0x06001C7B RID: 7291 RVA: 0x00104B28 File Offset: 0x00102F28
	private void Update()
	{
		Vector3 position = base.transform.position;
		position.x -= this.speed * CupheadTime.Delta;
		base.transform.position = position;
		if (base.transform.position.x <= 0f)
		{
			position.x += this.resetPositionX;
			base.transform.position = position;
		}
	}

	// Token: 0x04002575 RID: 9589
	[SerializeField]
	public float speed = 300f;

	// Token: 0x04002576 RID: 9590
	public float resetPositionX = 2808f;
}
