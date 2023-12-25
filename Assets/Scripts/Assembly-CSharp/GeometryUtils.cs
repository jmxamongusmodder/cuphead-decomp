using System;
using UnityEngine;

// Token: 0x02000392 RID: 914
public class GeometryUtils
{
	// Token: 0x06000B06 RID: 2822 RVA: 0x00081BC4 File Offset: 0x0007FFC4
	public static Vector3[] GetCircle(Vector3 center, float radius, GeometryUtils.Axis axis = GeometryUtils.Axis.Y, int resolution = 128)
	{
		Vector3[] array = new Vector3[resolution];
		float num = 6.2831855f / (float)resolution;
		for (int i = 0; i < resolution; i++)
		{
			float f = num * (float)i;
			float x = radius * Mathf.Cos(f);
			float y = radius * Mathf.Sin(f);
			array[i] = new Vector3(x, y, 0f);
		}
		Quaternion rotation;
		if (axis == GeometryUtils.Axis.X)
		{
			rotation = Quaternion.AngleAxis(90f, Vector3.up);
		}
		else if (axis == GeometryUtils.Axis.Y)
		{
			rotation = Quaternion.AngleAxis(90f, Vector3.right);
		}
		else
		{
			rotation = Quaternion.AngleAxis(0f, Vector3.up);
		}
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = rotation * array[j] + center;
		}
		return array;
	}

	// Token: 0x02000393 RID: 915
	public enum Axis
	{
		// Token: 0x040014B8 RID: 5304
		X,
		// Token: 0x040014B9 RID: 5305
		Y,
		// Token: 0x040014BA RID: 5306
		Z
	}
}
