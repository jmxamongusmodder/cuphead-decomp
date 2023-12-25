using System;
using UnityEngine;

// Token: 0x02000782 RID: 1922
public class RumRunnersGroundTest : MonoBehaviour
{
	// Token: 0x06002A40 RID: 10816 RVA: 0x0018B56C File Offset: 0x0018996C
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(new Vector3(base.transform.position.x, RumRunnersLevel.GroundWalkingPosY(base.transform.position + Vector3.up * 50f, this.collider, this.yOffset, 200f)), 20f);
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(new Vector3(base.transform.position.x, RumRunnersLevel.GroundWalkingPosY(new Vector3(base.transform.position.x, RumRunnersLevel.GroundWalkingPosY(base.transform.position, this.collider, this.yOffset, 200f)), this.collider, this.yOffset, 200f)), 20f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.position, 20f);
	}

	// Token: 0x04003317 RID: 13079
	[SerializeField]
	private Collider2D collider;

	// Token: 0x04003318 RID: 13080
	[SerializeField]
	private float yOffset;
}
