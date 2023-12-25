using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CC6 RID: 3270
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Camera Motion Blur")]
	public class CameraMotionBlur : PostEffectsBase
	{
		// Token: 0x060051CC RID: 20940 RVA: 0x0029D6E4 File Offset: 0x0029BAE4
		private void CalculateViewProjection()
		{
			Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
			Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true);
			this.currentViewProjMat = gpuprojectionMatrix * worldToCameraMatrix;
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x0029D71C File Offset: 0x0029BB1C
		private new void Start()
		{
			this.CheckResources();
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			this.wasActive = base.gameObject.activeInHierarchy;
			this.CalculateViewProjection();
			this.Remember();
			this.wasActive = false;
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x0029D771 File Offset: 0x0029BB71
		private void OnEnable()
		{
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			this._camera.depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x0029D7A4 File Offset: 0x0029BBA4
		private void OnDisable()
		{
			if (null != this.motionBlurMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.motionBlurMaterial);
				this.motionBlurMaterial = null;
			}
			if (null != this.dx11MotionBlurMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.dx11MotionBlurMaterial);
				this.dx11MotionBlurMaterial = null;
			}
			if (null != this.tmpCam)
			{
				UnityEngine.Object.DestroyImmediate(this.tmpCam);
				this.tmpCam = null;
			}
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x0029D81C File Offset: 0x0029BC1C
		public override bool CheckResources()
		{
			base.CheckSupport(true, true);
			this.motionBlurMaterial = base.CheckShaderAndCreateMaterial(this.shader, this.motionBlurMaterial);
			if (this.supportDX11 && this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11)
			{
				this.dx11MotionBlurMaterial = base.CheckShaderAndCreateMaterial(this.dx11MotionBlurShader, this.dx11MotionBlurMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x0029D890 File Offset: 0x0029BC90
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				this.StartFrame();
			}
			RenderTextureFormat format = (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf)) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.RGHalf;
			RenderTexture temporary = RenderTexture.GetTemporary(CameraMotionBlur.divRoundUp(source.width, this.velocityDownsample), CameraMotionBlur.divRoundUp(source.height, this.velocityDownsample), 0, format);
			this.maxVelocity = Mathf.Max(2f, this.maxVelocity);
			float value = this.maxVelocity;
			bool flag = this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 && this.dx11MotionBlurMaterial == null;
			int num;
			int height;
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction || flag || this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
			{
				this.maxVelocity = Mathf.Min(this.maxVelocity, CameraMotionBlur.MAX_RADIUS);
				num = CameraMotionBlur.divRoundUp(temporary.width, (int)this.maxVelocity);
				height = CameraMotionBlur.divRoundUp(temporary.height, (int)this.maxVelocity);
				value = (float)(temporary.width / num);
			}
			else
			{
				num = CameraMotionBlur.divRoundUp(temporary.width, (int)this.maxVelocity);
				height = CameraMotionBlur.divRoundUp(temporary.height, (int)this.maxVelocity);
				value = (float)(temporary.width / num);
			}
			RenderTexture temporary2 = RenderTexture.GetTemporary(num, height, 0, format);
			RenderTexture temporary3 = RenderTexture.GetTemporary(num, height, 0, format);
			temporary.filterMode = FilterMode.Point;
			temporary2.filterMode = FilterMode.Point;
			temporary3.filterMode = FilterMode.Point;
			if (this.noiseTexture)
			{
				this.noiseTexture.filterMode = FilterMode.Point;
			}
			source.wrapMode = TextureWrapMode.Clamp;
			temporary.wrapMode = TextureWrapMode.Clamp;
			temporary3.wrapMode = TextureWrapMode.Clamp;
			temporary2.wrapMode = TextureWrapMode.Clamp;
			this.CalculateViewProjection();
			if (base.gameObject.activeInHierarchy && !this.wasActive)
			{
				this.Remember();
			}
			this.wasActive = base.gameObject.activeInHierarchy;
			Matrix4x4 matrix4x = Matrix4x4.Inverse(this.currentViewProjMat);
			this.motionBlurMaterial.SetMatrix("_InvViewProj", matrix4x);
			this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
			this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix4x);
			this.motionBlurMaterial.SetFloat("_MaxVelocity", value);
			this.motionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", value);
			this.motionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
			this.motionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
			this.motionBlurMaterial.SetFloat("_Jitter", this.jitter);
			this.motionBlurMaterial.SetTexture("_NoiseTex", this.noiseTexture);
			this.motionBlurMaterial.SetTexture("_VelTex", temporary);
			this.motionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3);
			this.motionBlurMaterial.SetTexture("_TileTexDebug", temporary2);
			if (this.preview)
			{
				Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
				Matrix4x4 identity = Matrix4x4.identity;
				identity.SetTRS(this.previewScale * 0.3333f, Quaternion.identity, Vector3.one);
				Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true);
				this.prevViewProjMat = gpuprojectionMatrix * identity * worldToCameraMatrix;
				this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
				this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix4x);
			}
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				Vector4 zero = Vector4.zero;
				float num2 = Vector3.Dot(base.transform.up, Vector3.up);
				Vector3 rhs = this.prevFramePos - base.transform.position;
				float magnitude = rhs.magnitude;
				float num3 = Vector3.Angle(base.transform.up, this.prevFrameUp) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.x = this.rotationScale * num3;
				num3 = Vector3.Angle(base.transform.forward, this.prevFrameForward) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.y = this.rotationScale * num2 * num3;
				num3 = Vector3.Angle(base.transform.forward, this.prevFrameForward) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.z = this.rotationScale * (1f - num2) * num3;
				if (magnitude > Mathf.Epsilon && this.movementScale > Mathf.Epsilon)
				{
					zero.w = this.movementScale * Vector3.Dot(base.transform.forward, rhs) * ((float)source.width * 0.5f);
					zero.x += this.movementScale * Vector3.Dot(base.transform.up, rhs) * ((float)source.width * 0.5f);
					zero.y += this.movementScale * Vector3.Dot(base.transform.right, rhs) * ((float)source.width * 0.5f);
				}
				if (this.preview)
				{
					this.motionBlurMaterial.SetVector("_BlurDirectionPacked", new Vector4(this.previewScale.y, this.previewScale.x, 0f, this.previewScale.z) * 0.5f * this._camera.fieldOfView);
				}
				else
				{
					this.motionBlurMaterial.SetVector("_BlurDirectionPacked", zero);
				}
			}
			else
			{
				Graphics.Blit(source, temporary, this.motionBlurMaterial, 0);
				Camera camera = null;
				if (this.excludeLayers.value != 0)
				{
					camera = this.GetTmpCam();
				}
				if (camera && this.excludeLayers.value != 0 && this.replacementClear && this.replacementClear.isSupported)
				{
					camera.targetTexture = temporary;
					camera.cullingMask = this.excludeLayers;
					camera.RenderWithShader(this.replacementClear, string.Empty);
				}
			}
			if (!this.preview && Time.frameCount != this.prevFrameCount)
			{
				this.prevFrameCount = Time.frameCount;
				this.Remember();
			}
			source.filterMode = FilterMode.Bilinear;
			if (this.showVelocity)
			{
				this.motionBlurMaterial.SetFloat("_DisplayVelocityScale", this.showVelocityScale);
				Graphics.Blit(temporary, destination, this.motionBlurMaterial, 1);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 && !flag)
			{
				this.dx11MotionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
				this.dx11MotionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
				this.dx11MotionBlurMaterial.SetFloat("_Jitter", this.jitter);
				this.dx11MotionBlurMaterial.SetTexture("_NoiseTex", this.noiseTexture);
				this.dx11MotionBlurMaterial.SetTexture("_VelTex", temporary);
				this.dx11MotionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3);
				this.dx11MotionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				this.dx11MotionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", value);
				Graphics.Blit(temporary, temporary2, this.dx11MotionBlurMaterial, 0);
				Graphics.Blit(temporary2, temporary3, this.dx11MotionBlurMaterial, 1);
				Graphics.Blit(source, destination, this.dx11MotionBlurMaterial, 2);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction || flag)
			{
				this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				Graphics.Blit(temporary, temporary2, this.motionBlurMaterial, 2);
				Graphics.Blit(temporary2, temporary3, this.motionBlurMaterial, 3);
				Graphics.Blit(source, destination, this.motionBlurMaterial, 4);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				Graphics.Blit(source, destination, this.motionBlurMaterial, 6);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
			{
				this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				Graphics.Blit(temporary, temporary2, this.motionBlurMaterial, 2);
				Graphics.Blit(temporary2, temporary3, this.motionBlurMaterial, 3);
				Graphics.Blit(source, destination, this.motionBlurMaterial, 7);
			}
			else
			{
				Graphics.Blit(source, destination, this.motionBlurMaterial, 5);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary3);
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x0029E15C File Offset: 0x0029C55C
		private void Remember()
		{
			this.prevViewProjMat = this.currentViewProjMat;
			this.prevFrameForward = base.transform.forward;
			this.prevFrameUp = base.transform.up;
			this.prevFramePos = base.transform.position;
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x0029E1A8 File Offset: 0x0029C5A8
		private Camera GetTmpCam()
		{
			if (this.tmpCam == null)
			{
				string name = "_" + this._camera.name + "_MotionBlurTmpCam";
				GameObject y = GameObject.Find(name);
				if (null == y)
				{
					this.tmpCam = new GameObject(name, new Type[]
					{
						typeof(Camera)
					});
				}
				else
				{
					this.tmpCam = y;
				}
			}
			this.tmpCam.hideFlags = HideFlags.DontSave;
			this.tmpCam.transform.position = this._camera.transform.position;
			this.tmpCam.transform.rotation = this._camera.transform.rotation;
			this.tmpCam.transform.localScale = this._camera.transform.localScale;
			this.tmpCam.GetComponent<Camera>().CopyFrom(this._camera);
			this.tmpCam.GetComponent<Camera>().enabled = false;
			this.tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
			this.tmpCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
			return this.tmpCam.GetComponent<Camera>();
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x0029E2E0 File Offset: 0x0029C6E0
		private void StartFrame()
		{
			this.prevFramePos = Vector3.Slerp(this.prevFramePos, base.transform.position, 0.75f);
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x0029E303 File Offset: 0x0029C703
		private static int divRoundUp(int x, int d)
		{
			return (x + d - 1) / d;
		}

		// Token: 0x040055AB RID: 21931
		private static float MAX_RADIUS = 10f;

		// Token: 0x040055AC RID: 21932
		public CameraMotionBlur.MotionBlurFilter filterType = CameraMotionBlur.MotionBlurFilter.Reconstruction;

		// Token: 0x040055AD RID: 21933
		public bool preview;

		// Token: 0x040055AE RID: 21934
		public Vector3 previewScale = Vector3.one;

		// Token: 0x040055AF RID: 21935
		public float movementScale;

		// Token: 0x040055B0 RID: 21936
		public float rotationScale = 1f;

		// Token: 0x040055B1 RID: 21937
		public float maxVelocity = 8f;

		// Token: 0x040055B2 RID: 21938
		public float minVelocity = 0.1f;

		// Token: 0x040055B3 RID: 21939
		public float velocityScale = 0.375f;

		// Token: 0x040055B4 RID: 21940
		public float softZDistance = 0.005f;

		// Token: 0x040055B5 RID: 21941
		public int velocityDownsample = 1;

		// Token: 0x040055B6 RID: 21942
		public LayerMask excludeLayers = 0;

		// Token: 0x040055B7 RID: 21943
		private GameObject tmpCam;

		// Token: 0x040055B8 RID: 21944
		public Shader shader;

		// Token: 0x040055B9 RID: 21945
		public Shader dx11MotionBlurShader;

		// Token: 0x040055BA RID: 21946
		public Shader replacementClear;

		// Token: 0x040055BB RID: 21947
		private Material motionBlurMaterial;

		// Token: 0x040055BC RID: 21948
		private Material dx11MotionBlurMaterial;

		// Token: 0x040055BD RID: 21949
		public Texture2D noiseTexture;

		// Token: 0x040055BE RID: 21950
		public float jitter = 0.05f;

		// Token: 0x040055BF RID: 21951
		public bool showVelocity;

		// Token: 0x040055C0 RID: 21952
		public float showVelocityScale = 1f;

		// Token: 0x040055C1 RID: 21953
		private Matrix4x4 currentViewProjMat;

		// Token: 0x040055C2 RID: 21954
		private Matrix4x4 prevViewProjMat;

		// Token: 0x040055C3 RID: 21955
		private int prevFrameCount;

		// Token: 0x040055C4 RID: 21956
		private bool wasActive;

		// Token: 0x040055C5 RID: 21957
		private Vector3 prevFrameForward = Vector3.forward;

		// Token: 0x040055C6 RID: 21958
		private Vector3 prevFrameUp = Vector3.up;

		// Token: 0x040055C7 RID: 21959
		private Vector3 prevFramePos = Vector3.zero;

		// Token: 0x040055C8 RID: 21960
		private Camera _camera;

		// Token: 0x02000CC7 RID: 3271
		public enum MotionBlurFilter
		{
			// Token: 0x040055CA RID: 21962
			CameraMotion,
			// Token: 0x040055CB RID: 21963
			LocalBlur,
			// Token: 0x040055CC RID: 21964
			Reconstruction,
			// Token: 0x040055CD RID: 21965
			ReconstructionDX11,
			// Token: 0x040055CE RID: 21966
			ReconstructionDisc
		}
	}
}
