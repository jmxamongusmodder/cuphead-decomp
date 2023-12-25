using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C98 RID: 3224
	public static class TMPro_ExtensionMethods
	{
		// Token: 0x0600516E RID: 20846 RVA: 0x002990FC File Offset: 0x002974FC
		public static string ArrayToString(this char[] chars)
		{
			string text = string.Empty;
			int num = 0;
			while (num < chars.Length && chars[num] != '\0')
			{
				text += chars[num];
				num++;
			}
			return text;
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x0029913C File Offset: 0x0029753C
		public static int FindInstanceID<T>(this List<T> list, T target) where T : UnityEngine.Object
		{
			int instanceID = target.GetInstanceID();
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i];
				if (t.GetInstanceID() == instanceID)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x0029918C File Offset: 0x0029758C
		public static bool Compare(this Color32 a, Color32 b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x002991E5 File Offset: 0x002975E5
		public static bool CompareRGB(this Color32 a, Color32 b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b;
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x00299220 File Offset: 0x00297620
		public static bool Compare(this Color a, Color b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x00299279 File Offset: 0x00297679
		public static bool CompareRGB(this Color a, Color b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b;
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x002992B4 File Offset: 0x002976B4
		public static Color32 Multiply(this Color32 c1, Color32 c2)
		{
			byte r = (byte)((float)c1.r / 255f * ((float)c2.r / 255f) * 255f);
			byte g = (byte)((float)c1.g / 255f * ((float)c2.g / 255f) * 255f);
			byte b = (byte)((float)c1.b / 255f * ((float)c2.b / 255f) * 255f);
			byte a = (byte)((float)c1.a / 255f * ((float)c2.a / 255f) * 255f);
			return new Color32(r, g, b, a);
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x00299360 File Offset: 0x00297760
		public static Color32 Tint(this Color32 c1, Color32 c2)
		{
			byte r = (byte)((float)c1.r / 255f * ((float)c2.r / 255f) * 255f);
			byte g = (byte)((float)c1.g / 255f * ((float)c2.g / 255f) * 255f);
			byte b = (byte)((float)c1.b / 255f * ((float)c2.b / 255f) * 255f);
			byte a = (byte)((float)c1.a / 255f * ((float)c2.a / 255f) * 255f);
			return new Color32(r, g, b, a);
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x0029940C File Offset: 0x0029780C
		public static Color32 Tint(this Color32 c1, float tint)
		{
			byte r = (byte)Mathf.Clamp((float)c1.r / 255f * tint * 255f, 0f, 255f);
			byte g = (byte)Mathf.Clamp((float)c1.g / 255f * tint * 255f, 0f, 255f);
			byte b = (byte)Mathf.Clamp((float)c1.b / 255f * tint * 255f, 0f, 255f);
			byte a = (byte)Mathf.Clamp((float)c1.a / 255f * tint * 255f, 0f, 255f);
			return new Color32(r, g, b, a);
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x002994C0 File Offset: 0x002978C0
		public static bool Compare(this Vector3 v1, Vector3 v2, int accuracy)
		{
			bool flag = (int)(v1.x * (float)accuracy) == (int)(v2.x * (float)accuracy);
			bool flag2 = (int)(v1.y * (float)accuracy) == (int)(v2.y * (float)accuracy);
			bool flag3 = (int)(v1.z * (float)accuracy) == (int)(v2.z * (float)accuracy);
			return flag && flag2 && flag3;
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x00299528 File Offset: 0x00297928
		public static bool Compare(this Quaternion q1, Quaternion q2, int accuracy)
		{
			bool flag = (int)(q1.x * (float)accuracy) == (int)(q2.x * (float)accuracy);
			bool flag2 = (int)(q1.y * (float)accuracy) == (int)(q2.y * (float)accuracy);
			bool flag3 = (int)(q1.z * (float)accuracy) == (int)(q2.z * (float)accuracy);
			bool flag4 = (int)(q1.w * (float)accuracy) == (int)(q2.w * (float)accuracy);
			return flag && flag2 && flag3 && flag4;
		}
	}
}
