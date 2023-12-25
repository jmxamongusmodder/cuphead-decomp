using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BFA RID: 3066
	public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x0600493C RID: 18748
		public abstract CameraEvent GetCameraEvent();

		// Token: 0x0600493D RID: 18749
		public abstract string GetName();

		// Token: 0x0600493E RID: 18750
		public abstract void PopulateCommandBuffer(CommandBuffer cb);
	}
}
