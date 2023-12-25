using System;
using System.Globalization;
using UnityEngine;

// Token: 0x0200038C RID: 908
public class ColorUtils
{
	// Token: 0x06000AC4 RID: 2756 RVA: 0x0008093D File Offset: 0x0007ED3D
	public static Color GetAverageColor(Color[] colors)
	{
		return ColorUtils.GetAverageColor(colors, 1);
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x00080948 File Offset: 0x0007ED48
	public static Color GetAverageColor(Color[] colors, int quality)
	{
		int num = 0;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		for (int i = 0; i < colors.Length; i += quality)
		{
			if (i >= colors.Length)
			{
				break;
			}
			num2 += colors[i].r;
			num3 += colors[i].g;
			num4 += colors[i].b;
			num++;
		}
		num2 /= (float)num;
		num3 /= (float)num;
		num4 /= (float)num;
		return new Color(num2, num3, num4);
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x000809DC File Offset: 0x0007EDDC
	public static string ColorToHex(Color32 color, bool alpha = false)
	{
		string text = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		if (alpha)
		{
			text += color.a.ToString("X2");
		}
		return text;
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x00080A44 File Offset: 0x0007EE44
	public static Color HexToColor(string hex)
	{
		byte r = Parser.ByteParse(hex.Substring(0, 2), NumberStyles.HexNumber);
		byte g = Parser.ByteParse(hex.Substring(2, 2), NumberStyles.HexNumber);
		byte b = Parser.ByteParse(hex.Substring(4, 2), NumberStyles.HexNumber);
		return new Color32(r, g, b, byte.MaxValue);
	}
}
