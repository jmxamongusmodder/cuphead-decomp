using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BF8 RID: 3064
	public abstract class PostProcessingComponentBase
	{
		// Token: 0x06004931 RID: 18737 RVA: 0x0025DBB1 File Offset: 0x0025BFB1
		public virtual DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.None;
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06004932 RID: 18738
		public abstract bool active { get; }

		// Token: 0x06004933 RID: 18739 RVA: 0x0025DBB4 File Offset: 0x0025BFB4
		public virtual void OnEnable()
		{
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x0025DBB6 File Offset: 0x0025BFB6
		public virtual void OnDisable()
		{
		}

		// Token: 0x06004935 RID: 18741
		public abstract PostProcessingModel GetModel();

		// Token: 0x04004F5D RID: 20317
		public PostProcessingContext context;
	}
}
