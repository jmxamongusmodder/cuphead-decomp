using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RektTransform
{
	// Token: 0x0200036D RID: 877
	public static class Cast
	{
		// Token: 0x060009C8 RID: 2504 RVA: 0x0007D0A4 File Offset: 0x0007B4A4
		public static RectTransform RT(this GameObject go)
		{
			if (go == null || go.transform == null)
			{
				return null;
			}
			return go.GetComponent<RectTransform>();
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0007D0CB File Offset: 0x0007B4CB
		public static RectTransform RT(this Transform t)
		{
			if (!(t is RectTransform))
			{
				return null;
			}
			return t as RectTransform;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0007D0E0 File Offset: 0x0007B4E0
		public static RectTransform RT(this Component c)
		{
			return c.transform.RT();
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0007D0ED File Offset: 0x0007B4ED
		public static RectTransform RT(this UIBehaviour ui)
		{
			if (ui == null)
			{
				return null;
			}
			return ui.transform as RectTransform;
		}
	}
}
