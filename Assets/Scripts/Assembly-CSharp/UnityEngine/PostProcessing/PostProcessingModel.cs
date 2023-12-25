using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BFD RID: 3069
	[Serializable]
	public abstract class PostProcessingModel
	{
		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600494C RID: 18764 RVA: 0x00263408 File Offset: 0x00261808
		// (set) Token: 0x0600494D RID: 18765 RVA: 0x00263410 File Offset: 0x00261810
		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		// Token: 0x0600494E RID: 18766
		public abstract void Reset();

		// Token: 0x0600494F RID: 18767 RVA: 0x00263425 File Offset: 0x00261825
		public virtual void OnValidate()
		{
		}

		// Token: 0x04004F64 RID: 20324
		[SerializeField]
		[GetSet("enabled")]
		private bool m_Enabled;
	}
}
