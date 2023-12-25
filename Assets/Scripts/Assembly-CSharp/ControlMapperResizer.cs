using System;
using UnityEngine;

// Token: 0x02000C27 RID: 3111
public class ControlMapperResizer : MonoBehaviour
{
	// Token: 0x06004BE5 RID: 19429 RVA: 0x00271888 File Offset: 0x0026FC88
	private void Update()
	{
		float num = Mathf.Clamp((float)Screen.width / (float)Screen.height / 1.7777778f, 0f, 1f);
		if (this.cachedSize != num)
		{
			this.cachedSize = num;
			base.transform.localScale = new Vector3(num, num);
		}
	}

	// Token: 0x040050A9 RID: 20649
	private float cachedSize;
}
