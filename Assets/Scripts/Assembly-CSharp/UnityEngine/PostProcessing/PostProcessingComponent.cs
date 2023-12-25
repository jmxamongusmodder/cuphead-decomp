using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BF9 RID: 3065
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06004937 RID: 18743 RVA: 0x0025DBC0 File Offset: 0x0025BFC0
		// (set) Token: 0x06004938 RID: 18744 RVA: 0x0025DBC8 File Offset: 0x0025BFC8
		public T model { get; internal set; }

		// Token: 0x06004939 RID: 18745 RVA: 0x0025DBD1 File Offset: 0x0025BFD1
		public virtual void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x0025DBE1 File Offset: 0x0025BFE1
		public override PostProcessingModel GetModel()
		{
			return this.model;
		}
	}
}
