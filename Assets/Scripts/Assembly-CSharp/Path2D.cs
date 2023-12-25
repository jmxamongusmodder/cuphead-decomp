using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200035F RID: 863
public class Path2D : MonoBehaviour
{
	// Token: 0x06000998 RID: 2456 RVA: 0x0007C104 File Offset: 0x0007A504
	protected virtual void OnDrawGizmos()
	{
		this.DrawGizmos(0.1f);
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0007C111 File Offset: 0x0007A511
	protected virtual void OnDrawGizmosSelected()
	{
		this.DrawGizmos(1f);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0007C120 File Offset: 0x0007A520
	private void DrawGizmos(float a)
	{
		for (int i = 0; i < this.nodes.Count; i++)
		{
			if (i > 0)
			{
				Gizmos.DrawLine(this.nodes[i], this.nodes[i - 1]);
			}
		}
	}

	// Token: 0x0400143B RID: 5179
	public Path2D.Space space;

	// Token: 0x0400143C RID: 5180
	public List<Vector2> nodes = new List<Vector2>(2);

	// Token: 0x02000360 RID: 864
	public enum Space
	{
		// Token: 0x0400143E RID: 5182
		Global,
		// Token: 0x0400143F RID: 5183
		Local
	}
}
