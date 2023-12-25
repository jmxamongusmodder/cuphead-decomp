using System;
using UnityEngine;

// Token: 0x02000682 RID: 1666
public class FlyingMermaidLevelDebrisSpawner : ScrollingSpriteSpawner
{
	// Token: 0x06002327 RID: 8999 RVA: 0x0014A344 File Offset: 0x00148744
	protected override void OnSpawn(GameObject obj)
	{
		base.OnSpawn(obj);
		FlyingMermaidLevelFloater component = obj.GetComponent<FlyingMermaidLevelFloater>();
		if (component != null)
		{
			component.trackingWater = this.trackingWater;
		}
	}

	// Token: 0x04002BCC RID: 11212
	[SerializeField]
	private GameObject trackingWater;
}
