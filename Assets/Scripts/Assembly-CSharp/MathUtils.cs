using System;
using UnityEngine;

// Token: 0x02000396 RID: 918
public static class MathUtils
{
	// Token: 0x06000B23 RID: 2851 RVA: 0x000820C6 File Offset: 0x000804C6
	public static float GetPercentage(float min, float max, float t)
	{
		return (t - min) / (max - min);
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x000820CF File Offset: 0x000804CF
	public static int PlusOrMinus()
	{
		return (UnityEngine.Random.value <= 0.5f) ? -1 : 1;
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x000820E7 File Offset: 0x000804E7
	public static float ExpRandom(float mean)
	{
		return -Mathf.Log(UnityEngine.Random.Range(0f, 1f)) * mean;
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x00082100 File Offset: 0x00080500
	public static bool RandomBool()
	{
		return UnityEngine.Random.value > 0.5f;
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0008210E File Offset: 0x0008050E
	public static Vector2 RandomPointInUnitCircle()
	{
		return MathUtils.AngleToDirection(UnityEngine.Random.Range(0f, 360f)) * Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0008213D File Offset: 0x0008053D
	public static float DirectionToAngle(Vector2 direction)
	{
		return Mathf.Atan2(direction.y, direction.x) * 360f / 6.2831855f;
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x00082160 File Offset: 0x00080560
	public static Vector2 AngleToDirection(float angle)
	{
		float f = angle * 3.1415927f * 2f / 360f;
		return new Vector2(Mathf.Cos(f), Mathf.Sin(f));
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x00082194 File Offset: 0x00080594
	public static bool CircleContains(Vector2 center, float radius, Vector2 point)
	{
		return Mathf.Pow(point.x - center.x, 2f) + Mathf.Pow(point.y - center.y, 2f) < Mathf.Pow(radius, 2f);
	}
}
