using System;
using UnityEngine;

// Token: 0x020007E9 RID: 2025
public class SnowCultLevelBucket : MonoBehaviour
{
	// Token: 0x06002E59 RID: 11865 RVA: 0x001B509C File Offset: 0x001B349C
	private void FixedUpdate()
	{
		base.transform.position += this.fallSpeed * Vector3.down * CupheadTime.FixedDelta;
		this.fallSpeed += CupheadTime.FixedDelta * this.accel;
	}

	// Token: 0x040036ED RID: 14061
	[SerializeField]
	private float fallSpeed = 10f;

	// Token: 0x040036EE RID: 14062
	[SerializeField]
	private float accel = 1f;
}
