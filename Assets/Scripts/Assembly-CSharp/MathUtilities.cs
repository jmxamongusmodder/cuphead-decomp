using System;
using UnityEngine;

// Token: 0x02000395 RID: 917
public static class MathUtilities
{
	// Token: 0x06000B10 RID: 2832 RVA: 0x00081D5C File Offset: 0x0008015C
	public static bool SameSign(float a, float b)
	{
		return (Mathf.Approximately(a, 0f) && Mathf.Approximately(b, 0f)) || (a > 0f && b > 0f) || (a < 0f && b < 0f);
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x00081DBC File Offset: 0x000801BC
	public static float LerpMapping(float value, float fromStart, float fromEnd, float toStart, float toEnd, bool clamp = false)
	{
		float num = (value - fromStart) / (fromEnd - fromStart);
		if (clamp)
		{
			num = Mathf.Max(0f, Mathf.Min(1f, num));
		}
		return toStart + (toEnd - toStart) * num;
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x00081DF8 File Offset: 0x000801F8
	public static float SqrDistanceToLine(Ray ray, Vector3 point)
	{
		return Vector3.Cross(ray.direction, point - ray.origin).sqrMagnitude;
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x00081E28 File Offset: 0x00080228
	public static float DistanceToLine(Ray ray, Vector3 point)
	{
		return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x00081E56 File Offset: 0x00080256
	public static float DecimalPart(float value)
	{
		if (value < 0f)
		{
			return value - Mathf.Ceil(value);
		}
		return value - Mathf.Floor(value);
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x00081E74 File Offset: 0x00080274
	public static int NextIndex(int currentIndex, int indexLength)
	{
		currentIndex++;
		if (currentIndex >= indexLength)
		{
			currentIndex = 0;
		}
		return currentIndex;
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x00081E86 File Offset: 0x00080286
	public static int PreviousIndex(int currentIndex, int indexLength)
	{
		currentIndex--;
		if (currentIndex < 0)
		{
			currentIndex = indexLength - 1;
		}
		return currentIndex;
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x00081E9C File Offset: 0x0008029C
	public static bool LinesIntersect(Vector2 s1, Vector2 e1, Vector2 s2, Vector2 e2, out Vector2 intersectionPoint)
	{
		float num = e1.y - s1.y;
		float num2 = s1.x - e1.x;
		float num3 = num * s1.x + num2 * s1.y;
		float num4 = e2.y - s2.y;
		float num5 = s2.x - e2.x;
		float num6 = num4 * s2.x + num5 * s2.y;
		float num7 = num * num5 - num4 * num2;
		if (Mathf.Approximately(num7, 0f))
		{
			intersectionPoint = Vector2.zero;
			return false;
		}
		float num8 = 1f / num7;
		intersectionPoint = new Vector2((num5 * num3 - num2 * num6) * num8, (num * num6 - num4 * num3) * num8);
		return true;
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x00081F66 File Offset: 0x00080366
	public static Vector2 HadamardProduct(Vector2 v1, Vector2 v2)
	{
		return new Vector2(v1.x * v2.x, v1.y * v2.y);
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x00081F8B File Offset: 0x0008038B
	public static Vector3 HadamardProduct(Vector3 v1, Vector3 v2)
	{
		return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x00081FBF File Offset: 0x000803BF
	public static bool BetweenInclusive(int value, int min, int max)
	{
		return value >= min && value <= max;
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x00081FD2 File Offset: 0x000803D2
	public static bool BetweenInclusive(float value, float min, float max)
	{
		return value >= min && value <= max;
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00081FE5 File Offset: 0x000803E5
	public static bool BetweenExclusive(float value, float min, float max)
	{
		return value > min && value < max;
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00081FF5 File Offset: 0x000803F5
	public static bool BetweenInclusiveExclusive(float value, float min, float max)
	{
		return value >= min && value < max;
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x00082005 File Offset: 0x00080405
	public static bool BetweenExclusiveInclusive(float value, float min, float max)
	{
		return value > min && value <= max;
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x00082018 File Offset: 0x00080418
	public static float ClampAngleSoft(float angle)
	{
		if (angle >= 6.2831855f)
		{
			angle -= 6.2831855f;
		}
		else if (angle < 0f)
		{
			angle += 6.2831855f;
		}
		return angle;
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x00082048 File Offset: 0x00080448
	public static float DirectionToAngle(Vector2 direction)
	{
		return Mathf.Atan2(direction.y, direction.x) * 57.29578f;
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x00082064 File Offset: 0x00080464
	public static Vector2 AngleToDirection(float angle)
	{
		float f = angle * 0.017453292f;
		return new Vector2(Mathf.Cos(f), Mathf.Sin(f));
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x0008208C File Offset: 0x0008048C
	public static Vector2 TrigonmetricVector(float t, float amplitude, float frequency, float phaseShift = 0f, float globalPhaseShift = 0f)
	{
		Vector2 result;
		result.x = amplitude * Mathf.Cos(frequency * (t + phaseShift) + globalPhaseShift);
		result.y = amplitude * Mathf.Sin(frequency * (t + phaseShift) + globalPhaseShift);
		return result;
	}

	// Token: 0x040014BB RID: 5307
	public const float Sqrt2 = 1.4142135f;

	// Token: 0x040014BC RID: 5308
	public const float InverseSqrt2 = 0.70710677f;

	// Token: 0x040014BD RID: 5309
	public const float TwoPi = 6.2831855f;

	// Token: 0x040014BE RID: 5310
	public const float HalfPi = 1.5707964f;
}
