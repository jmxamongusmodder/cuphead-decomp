using System;
using UnityEngine;

// Token: 0x02000A4D RID: 2637
public class PlayerProjectileRoot : AbstractMonoBehaviour
{
	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06003EC8 RID: 16072 RVA: 0x002266F6 File Offset: 0x00224AF6
	public Vector2 Position
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x00226708 File Offset: 0x00224B08
	public float Rotation
	{
		get
		{
			return base.transform.eulerAngles.z;
		}
	}

	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06003ECA RID: 16074 RVA: 0x00226728 File Offset: 0x00224B28
	public Vector3 Scale
	{
		get
		{
			float y = 1f;
			return new Vector3(1f, y, 1f);
		}
	}
}
