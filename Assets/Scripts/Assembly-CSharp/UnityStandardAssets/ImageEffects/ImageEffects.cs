using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CDD RID: 3293
	[AddComponentMenu("")]
	public class ImageEffects
	{
		// Token: 0x06005226 RID: 21030 RVA: 0x002A1EFC File Offset: 0x002A02FC
		public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
		{
			bool flag = source.texelSize.y < 0f;
			if (flag)
			{
				center.y = 1f - center.y;
				angle = -angle;
			}
			Matrix4x4 value = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), Vector3.one);
			material.SetMatrix("_RotationMatrix", value);
			material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
			material.SetFloat("_Angle", angle * 0.017453292f);
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x002A1FAF File Offset: 0x002A03AF
		[Obsolete("Use Graphics.Blit(source,dest) instead")]
		public static void Blit(RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest);
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x002A1FB8 File Offset: 0x002A03B8
		[Obsolete("Use Graphics.Blit(source, destination, material) instead")]
		public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest, material);
		}
	}
}
