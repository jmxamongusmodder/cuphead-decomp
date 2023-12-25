using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CDA RID: 3290
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Global Fog")]
	internal class GlobalFog : PostEffectsBase
	{
		// Token: 0x0600521C RID: 21020 RVA: 0x002A19E8 File Offset: 0x0029FDE8
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.fogMaterial = base.CheckShaderAndCreateMaterial(this.fogShader, this.fogMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x002A1A24 File Offset: 0x0029FE24
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || (!this.distanceFog && !this.heightFog))
			{
				Graphics.Blit(source, destination);
				return;
			}
			Camera component = base.GetComponent<Camera>();
			Transform transform = component.transform;
			float nearClipPlane = component.nearClipPlane;
			float farClipPlane = component.farClipPlane;
			float fieldOfView = component.fieldOfView;
			float aspect = component.aspect;
			Matrix4x4 identity = Matrix4x4.identity;
			float num = fieldOfView * 0.5f;
			Vector3 b = transform.right * nearClipPlane * Mathf.Tan(num * 0.017453292f) * aspect;
			Vector3 b2 = transform.up * nearClipPlane * Mathf.Tan(num * 0.017453292f);
			Vector3 vector = transform.forward * nearClipPlane - b + b2;
			float d = vector.magnitude * farClipPlane / nearClipPlane;
			vector.Normalize();
			vector *= d;
			Vector3 vector2 = transform.forward * nearClipPlane + b + b2;
			vector2.Normalize();
			vector2 *= d;
			Vector3 vector3 = transform.forward * nearClipPlane + b - b2;
			vector3.Normalize();
			vector3 *= d;
			Vector3 vector4 = transform.forward * nearClipPlane - b - b2;
			vector4.Normalize();
			vector4 *= d;
			identity.SetRow(0, vector);
			identity.SetRow(1, vector2);
			identity.SetRow(2, vector3);
			identity.SetRow(3, vector4);
			Vector3 position = transform.position;
			float num2 = position.y - this.height;
			float z = (num2 > 0f) ? 0f : 1f;
			this.fogMaterial.SetMatrix("_FrustumCornersWS", identity);
			this.fogMaterial.SetVector("_CameraWS", position);
			this.fogMaterial.SetVector("_HeightParams", new Vector4(this.height, num2, z, this.heightDensity * 0.5f));
			this.fogMaterial.SetVector("_DistanceParams", new Vector4(-Mathf.Max(this.startDistance, 0f), 0f, 0f, 0f));
			FogMode fogMode = RenderSettings.fogMode;
			float fogDensity = RenderSettings.fogDensity;
			float fogStartDistance = RenderSettings.fogStartDistance;
			float fogEndDistance = RenderSettings.fogEndDistance;
			bool flag = fogMode == FogMode.Linear;
			float num3 = (!flag) ? 0f : (fogEndDistance - fogStartDistance);
			float num4 = (Mathf.Abs(num3) <= 0.0001f) ? 0f : (1f / num3);
			Vector4 value;
			value.x = fogDensity * 1.2011224f;
			value.y = fogDensity * 1.442695f;
			value.z = ((!flag) ? 0f : (-num4));
			value.w = ((!flag) ? 0f : (fogEndDistance * num4));
			this.fogMaterial.SetVector("_SceneFogParams", value);
			this.fogMaterial.SetVector("_SceneFogMode", new Vector4((float)fogMode, (float)((!this.useRadialDistance) ? 0 : 1), 0f, 0f));
			int passNr;
			if (this.distanceFog && this.heightFog)
			{
				passNr = 0;
			}
			else if (this.distanceFog)
			{
				passNr = 1;
			}
			else
			{
				passNr = 2;
			}
			GlobalFog.CustomGraphicsBlit(source, destination, this.fogMaterial, passNr);
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x002A1DE0 File Offset: 0x002A01E0
		private static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
		{
			RenderTexture.active = dest;
			fxMaterial.SetTexture("_MainTex", source);
			GL.PushMatrix();
			GL.LoadOrtho();
			fxMaterial.SetPass(passNr);
			GL.Begin(7);
			GL.MultiTexCoord2(0, 0f, 0f);
			GL.Vertex3(0f, 0f, 3f);
			GL.MultiTexCoord2(0, 1f, 0f);
			GL.Vertex3(1f, 0f, 2f);
			GL.MultiTexCoord2(0, 1f, 1f);
			GL.Vertex3(1f, 1f, 1f);
			GL.MultiTexCoord2(0, 0f, 1f);
			GL.Vertex3(0f, 1f, 0f);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x0400567A RID: 22138
		[Tooltip("Apply distance-based fog?")]
		public bool distanceFog = true;

		// Token: 0x0400567B RID: 22139
		[Tooltip("Distance fog is based on radial distance from camera when checked")]
		public bool useRadialDistance;

		// Token: 0x0400567C RID: 22140
		[Tooltip("Apply height-based fog?")]
		public bool heightFog = true;

		// Token: 0x0400567D RID: 22141
		[Tooltip("Fog top Y coordinate")]
		public float height = 1f;

		// Token: 0x0400567E RID: 22142
		[Range(0.001f, 10f)]
		public float heightDensity = 2f;

		// Token: 0x0400567F RID: 22143
		[Tooltip("Push fog away from the camera by this amount")]
		public float startDistance;

		// Token: 0x04005680 RID: 22144
		public Shader fogShader;

		// Token: 0x04005681 RID: 22145
		private Material fogMaterial;
	}
}
