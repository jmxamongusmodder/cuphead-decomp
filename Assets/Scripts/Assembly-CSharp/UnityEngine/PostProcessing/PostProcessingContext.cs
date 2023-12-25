using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BFC RID: 3068
	public class PostProcessingContext
	{
		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06004942 RID: 18754 RVA: 0x002651FA File Offset: 0x002635FA
		// (set) Token: 0x06004943 RID: 18755 RVA: 0x00265202 File Offset: 0x00263602
		public bool interrupted { get; private set; }

		// Token: 0x06004944 RID: 18756 RVA: 0x0026520B File Offset: 0x0026360B
		public void Interrupt()
		{
			this.interrupted = true;
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x00265214 File Offset: 0x00263614
		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06004946 RID: 18758 RVA: 0x0026523A File Offset: 0x0026363A
		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == RenderingPath.DeferredShading;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06004947 RID: 18759 RVA: 0x0026524A File Offset: 0x0026364A
		public bool isHdr
		{
			get
			{
				return this.camera.allowHDR;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06004948 RID: 18760 RVA: 0x00265257 File Offset: 0x00263657
		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06004949 RID: 18761 RVA: 0x00265264 File Offset: 0x00263664
		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x0600494A RID: 18762 RVA: 0x00265271 File Offset: 0x00263671
		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		// Token: 0x04004F5F RID: 20319
		public PostProcessingProfile profile;

		// Token: 0x04004F60 RID: 20320
		public Camera camera;

		// Token: 0x04004F61 RID: 20321
		public MaterialFactory materialFactory;

		// Token: 0x04004F62 RID: 20322
		public RenderTextureFactory renderTextureFactory;
	}
}
