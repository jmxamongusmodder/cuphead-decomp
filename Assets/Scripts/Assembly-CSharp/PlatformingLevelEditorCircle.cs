using System;
using UnityEngine;

// Token: 0x020008FE RID: 2302
[RequireComponent(typeof(CircleCollider2D))]
public class PlatformingLevelEditorCircle : AbstractMonoBehaviour
{
	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x06003606 RID: 13830 RVA: 0x001F6454 File Offset: 0x001F4854
	private CircleCollider2D circleCollider
	{
		get
		{
			if (this._circleCollider == null)
			{
				this._circleCollider = base.GetComponent<CircleCollider2D>();
			}
			return this._circleCollider;
		}
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x001F6479 File Offset: 0x001F4879
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.5f);
	}

	// Token: 0x06003608 RID: 13832 RVA: 0x001F648C File Offset: 0x001F488C
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
	}

	// Token: 0x06003609 RID: 13833 RVA: 0x001F6494 File Offset: 0x001F4894
	private void DrawGizmos(float alpha)
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(base.transform.position + this.circleCollider.offset, this.circleCollider.radius);
	}

	// Token: 0x04003E0E RID: 15886
	private CircleCollider2D _circleCollider;
}
