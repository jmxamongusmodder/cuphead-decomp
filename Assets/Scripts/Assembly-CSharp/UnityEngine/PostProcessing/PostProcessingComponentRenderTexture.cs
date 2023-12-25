using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BFB RID: 3067
	public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06004940 RID: 18752 RVA: 0x0025E1CB File Offset: 0x0025C5CB
		public virtual void Prepare(Material material)
		{
		}
	}
}
