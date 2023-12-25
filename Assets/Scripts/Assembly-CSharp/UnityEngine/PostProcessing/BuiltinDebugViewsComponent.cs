using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000B99 RID: 2969
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06004828 RID: 18472 RVA: 0x0025E6CF File Offset: 0x0025CACF
		public override bool active
		{
			get
			{
				return base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);
			}
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x0025E704 File Offset: 0x0025CB04
		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			if (mode != BuiltinDebugViewsModel.Mode.Normals)
			{
				if (mode != BuiltinDebugViewsModel.Mode.MotionVectors)
				{
					if (mode == BuiltinDebugViewsModel.Mode.Depth)
					{
						depthTextureMode |= DepthTextureMode.Depth;
					}
				}
				else
				{
					depthTextureMode |= (DepthTextureMode.Depth | DepthTextureMode.MotionVectors);
				}
			}
			else
			{
				depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			return depthTextureMode;
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x0025E760 File Offset: 0x0025CB60
		public override CameraEvent GetCameraEvent()
		{
			return (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeImageEffects;
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x0025E78F File Offset: 0x0025CB8F
		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x0025E798 File Offset: 0x0025CB98
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (this.context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			BuiltinDebugViewsModel.Mode mode = settings.mode;
			if (mode != BuiltinDebugViewsModel.Mode.Depth)
			{
				if (mode != BuiltinDebugViewsModel.Mode.Normals)
				{
					if (mode == BuiltinDebugViewsModel.Mode.MotionVectors)
					{
						this.MotionVectorsPass(cb);
					}
				}
				else
				{
					this.DepthNormalsPass(cb);
				}
			}
			else
			{
				this.DepthPass(cb);
			}
			this.context.Interrupt();
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x0025E83C File Offset: 0x0025CC3C
		private void DepthPass(CommandBuffer cb)
		{
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 0);
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x0025E89C File Offset: 0x0025CC9C
		private void DepthNormalsPass(CommandBuffer cb)
		{
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 1);
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x0025E8D0 File Offset: 0x0025CCD0
		private void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
			int nameID = BuiltinDebugViewsComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(nameID, this.context.width, this.context.height, 0, FilterMode.Bilinear);
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, nameID, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = BuiltinDebugViewsComponent.Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Bilinear);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, nameID);
				cb.Blit(nameID, tempRT, material, 3);
				cb.ReleaseTemporaryRT(nameID);
				nameID = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				this.PrepareArrows();
				float num = 1f / (float)motionVectors.motionVectorsResolution;
				float x = num * (float)this.context.height / (float)this.context.width;
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, new Vector2(x, num));
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, nameID);
			cb.Blit(nameID, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(nameID);
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x0025EAD8 File Offset: 0x0025CED8
		private void PrepareArrows()
		{
			int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (this.m_Arrows == null)
			{
				this.m_Arrows = new BuiltinDebugViewsComponent.ArrowArray();
			}
			if (this.m_Arrows.columnCount != num || this.m_Arrows.rowCount != motionVectorsResolution)
			{
				this.m_Arrows.Release();
				this.m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x0025EB5C File Offset: 0x0025CF5C
		public override void OnDisable()
		{
			if (this.m_Arrows != null)
			{
				this.m_Arrows.Release();
			}
			this.m_Arrows = null;
		}

		// Token: 0x04004D9D RID: 19869
		private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		// Token: 0x04004D9E RID: 19870
		private BuiltinDebugViewsComponent.ArrowArray m_Arrows;

		// Token: 0x02000B9A RID: 2970
		private static class Uniforms
		{
			// Token: 0x04004D9F RID: 19871
			internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			// Token: 0x04004DA0 RID: 19872
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x04004DA1 RID: 19873
			internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			// Token: 0x04004DA2 RID: 19874
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04004DA3 RID: 19875
			internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			// Token: 0x04004DA4 RID: 19876
			internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			// Token: 0x04004DA5 RID: 19877
			internal static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		// Token: 0x02000B9B RID: 2971
		private enum Pass
		{
			// Token: 0x04004DA7 RID: 19879
			Depth,
			// Token: 0x04004DA8 RID: 19880
			Normals,
			// Token: 0x04004DA9 RID: 19881
			MovecOpacity,
			// Token: 0x04004DAA RID: 19882
			MovecImaging,
			// Token: 0x04004DAB RID: 19883
			MovecArrows
		}

		// Token: 0x02000B9C RID: 2972
		private class ArrowArray
		{
			// Token: 0x17000648 RID: 1608
			// (get) Token: 0x06004834 RID: 18484 RVA: 0x0025EBFA File Offset: 0x0025CFFA
			// (set) Token: 0x06004835 RID: 18485 RVA: 0x0025EC02 File Offset: 0x0025D002
			public Mesh mesh { get; private set; }

			// Token: 0x17000649 RID: 1609
			// (get) Token: 0x06004836 RID: 18486 RVA: 0x0025EC0B File Offset: 0x0025D00B
			// (set) Token: 0x06004837 RID: 18487 RVA: 0x0025EC13 File Offset: 0x0025D013
			public int columnCount { get; private set; }

			// Token: 0x1700064A RID: 1610
			// (get) Token: 0x06004838 RID: 18488 RVA: 0x0025EC1C File Offset: 0x0025D01C
			// (set) Token: 0x06004839 RID: 18489 RVA: 0x0025EC24 File Offset: 0x0025D024
			public int rowCount { get; private set; }

			// Token: 0x0600483A RID: 18490 RVA: 0x0025EC30 File Offset: 0x0025D030
			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 item = new Vector2((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(item);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				this.mesh = new Mesh
				{
					hideFlags = HideFlags.DontSave
				};
				this.mesh.SetVertices(list);
				this.mesh.SetUVs(0, list2);
				this.mesh.SetIndices(array2, MeshTopology.Lines, 0);
				this.mesh.UploadMeshData(true);
				this.columnCount = columns;
				this.rowCount = rows;
			}

			// Token: 0x0600483B RID: 18491 RVA: 0x0025EE13 File Offset: 0x0025D213
			public void Release()
			{
				GraphicsUtils.Destroy(this.mesh);
				this.mesh = null;
			}
		}
	}
}
