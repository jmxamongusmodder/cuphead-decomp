using System;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C0A RID: 3082
	[RequireComponent(typeof(CanvasScalerExt))]
	public class CanvasScalerFitter : MonoBehaviour
	{
		// Token: 0x06004994 RID: 18836 RVA: 0x0026749E File Offset: 0x0026589E
		private void OnEnable()
		{
			this.canvasScaler = base.GetComponent<CanvasScalerExt>();
			this.Update();
			this.canvasScaler.ForceRefresh();
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x002674BD File Offset: 0x002658BD
		private void Update()
		{
			if (Screen.width != this.screenWidth || Screen.height != this.screenHeight)
			{
				this.screenWidth = Screen.width;
				this.screenHeight = Screen.height;
				this.UpdateSize();
			}
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x002674FC File Offset: 0x002658FC
		private void UpdateSize()
		{
			if (this.canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
			{
				return;
			}
			if (this.breakPoints == null)
			{
				return;
			}
			float num = (float)Screen.width / (float)Screen.height;
			float num2 = float.PositiveInfinity;
			int num3 = 0;
			for (int i = 0; i < this.breakPoints.Length; i++)
			{
				float num4 = Mathf.Abs(num - this.breakPoints[i].screenAspectRatio);
				if (num4 <= this.breakPoints[i].screenAspectRatio || MathTools.IsNear(this.breakPoints[i].screenAspectRatio, 0.01f))
				{
					if (num4 < num2)
					{
						num2 = num4;
						num3 = i;
					}
				}
			}
			this.canvasScaler.referenceResolution = this.breakPoints[num3].referenceResolution;
		}

		// Token: 0x04004FAC RID: 20396
		[SerializeField]
		private CanvasScalerFitter.BreakPoint[] breakPoints;

		// Token: 0x04004FAD RID: 20397
		private CanvasScalerExt canvasScaler;

		// Token: 0x04004FAE RID: 20398
		private int screenWidth;

		// Token: 0x04004FAF RID: 20399
		private int screenHeight;

		// Token: 0x04004FB0 RID: 20400
		private Action ScreenSizeChanged;

		// Token: 0x02000C0B RID: 3083
		[Serializable]
		private class BreakPoint
		{
			// Token: 0x04004FB1 RID: 20401
			[SerializeField]
			public string name;

			// Token: 0x04004FB2 RID: 20402
			[SerializeField]
			public float screenAspectRatio;

			// Token: 0x04004FB3 RID: 20403
			[SerializeField]
			public Vector2 referenceResolution;
		}
	}
}
