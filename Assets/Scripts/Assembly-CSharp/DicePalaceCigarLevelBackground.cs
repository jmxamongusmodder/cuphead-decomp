using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005AD RID: 1453
public class DicePalaceCigarLevelBackground : AbstractPausableComponent
{
	// Token: 0x06001C09 RID: 7177 RVA: 0x00101548 File Offset: 0x000FF948
	private IEnumerator circulate_fire_cr()
	{
		float loopSize = 6f;
		float angle = 0f;
		for (;;)
		{
			angle += 0.5f * CupheadTime.Delta;
			Vector3 handleRotationX = new Vector3(-Mathf.Sin(angle) * loopSize, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Cos(angle) * loopSize, 0f);
			this.foregroundFireSprite.position = this.firePivot.position;
			this.foregroundFireSprite.position += handleRotationX + handleRotationY;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002518 RID: 9496
	[SerializeField]
	private Transform foregroundFireSprite;

	// Token: 0x04002519 RID: 9497
	[SerializeField]
	private Transform firePivot;
}
