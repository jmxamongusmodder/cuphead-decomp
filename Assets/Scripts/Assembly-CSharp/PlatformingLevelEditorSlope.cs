using System;
using UnityEngine;

// Token: 0x02000901 RID: 2305
[RequireComponent(typeof(PolygonCollider2D))]
public class PlatformingLevelEditorSlope : AbstractMonoBehaviour
{
	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06003610 RID: 13840 RVA: 0x001F6A18 File Offset: 0x001F4E18
	private PolygonCollider2D polygonCollider
	{
		get
		{
			if (this._polygonCollider == null)
			{
				this._polygonCollider = base.GetComponent<PolygonCollider2D>();
			}
			return this._polygonCollider;
		}
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x001F6A3D File Offset: 0x001F4E3D
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.5f);
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x001F6A50 File Offset: 0x001F4E50
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x001F6A58 File Offset: 0x001F4E58
	private void DrawGizmos(float alpha)
	{
		Gizmos.color = Color.cyan;
		for (int i = 0; i < this.polygonCollider.points.Length; i++)
		{
			Vector3 from = this.polygonCollider.points[i];
			Vector3 to = (i != this.polygonCollider.points.Length - 1) ? this.polygonCollider.points[i + 1] : this.polygonCollider.points[0];
			Gizmos.DrawLine(from, to);
		}
	}

	// Token: 0x04003E1C RID: 15900
	private PolygonCollider2D _polygonCollider;
}
