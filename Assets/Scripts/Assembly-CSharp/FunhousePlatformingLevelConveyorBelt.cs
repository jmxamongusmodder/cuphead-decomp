using System;
using UnityEngine;

// Token: 0x020008B2 RID: 2226
public class FunhousePlatformingLevelConveyorBelt : ScrollingSprite
{
	// Token: 0x060033E3 RID: 13283 RVA: 0x001E1998 File Offset: 0x001DFD98
	protected override void Start()
	{
		base.Start();
		this.point += base.transform.position;
	}

	// Token: 0x060033E4 RID: 13284 RVA: 0x001E19BC File Offset: 0x001DFDBC
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.DrawSphere(this.point + base.transform.position, 10f);
	}

	// Token: 0x060033E5 RID: 13285 RVA: 0x001E19E4 File Offset: 0x001DFDE4
	protected override void Update()
	{
		this.wait -= CupheadTime.Delta;
		if (this.wait > 0f)
		{
			return;
		}
		base.Update();
		for (int i = 0; i < base.copyRenderers.Count; i++)
		{
			Vector3 position = base.copyRenderers[i].transform.position;
			position.z = position.x - this.point.x;
			if (this.rightToCenter)
			{
				position.z *= -1f;
			}
			base.copyRenderers[i].transform.position = position;
		}
	}

	// Token: 0x04003C29 RID: 15401
	public Vector3 point;

	// Token: 0x04003C2A RID: 15402
	public bool rightToCenter;

	// Token: 0x04003C2B RID: 15403
	public float wait;
}
