using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C0C RID: 3084
	[RequireComponent(typeof(CanvasScalerExt))]
	public class CanvasScalerFitterControlMapperOverride : MonoBehaviour
	{
		// Token: 0x06004999 RID: 18841 RVA: 0x002675EB File Offset: 0x002659EB
		private void OnEnable()
		{
			this.canvasScaler = base.GetComponent<CanvasScalerExt>();
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x002675F9 File Offset: 0x002659F9
		private void LateUpdate()
		{
			this.canvasScaler.referenceResolution = this.targetResolution;
		}

		// Token: 0x04004FB4 RID: 20404
		[SerializeField]
		private Vector2 targetResolution = new Vector2(1885f, 600f);

		// Token: 0x04004FB5 RID: 20405
		private CanvasScalerExt canvasScaler;
	}
}
