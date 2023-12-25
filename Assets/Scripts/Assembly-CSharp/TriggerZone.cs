using System;
using UnityEngine;

// Token: 0x020003EC RID: 1004
public class TriggerZone : MonoBehaviour
{
	// Token: 0x06000D8D RID: 3469 RVA: 0x0008E3E4 File Offset: 0x0008C7E4
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(base.transform.position, this.size);
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x0008E404 File Offset: 0x0008C804
	public bool Contains(Vector3 position)
	{
		Rect zero = Rect.zero;
		zero.size = this.size;
		zero.center = base.transform.position;
		return zero.Contains(position);
	}

	// Token: 0x04001706 RID: 5894
	[SerializeField]
	private Vector2 size;
}
