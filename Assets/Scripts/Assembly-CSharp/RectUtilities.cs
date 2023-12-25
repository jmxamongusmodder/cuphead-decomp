using System;
using UnityEngine;

// Token: 0x02000397 RID: 919
public static class RectUtilities
{
	// Token: 0x06000B2B RID: 2859 RVA: 0x000821E1 File Offset: 0x000805E1
	public static Rect AdjustSize(this Rect rect, float left, float right, float top, float bottom)
	{
		rect.xMin += left;
		rect.xMax += right;
		rect.yMin += top;
		rect.yMax += bottom;
		return rect;
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00082224 File Offset: 0x00080624
	public static Rect SliceLeft(ref Rect rect, float amount)
	{
		Rect result = new Rect(rect);
		result.xMax = result.xMin + amount;
		rect.xMin += amount;
		return result;
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x00082260 File Offset: 0x00080660
	public static Rect SliceRight(ref Rect rect, float amount)
	{
		Rect result = new Rect(rect);
		result.xMin = result.xMax - amount;
		rect.xMax -= amount;
		return result;
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0008229C File Offset: 0x0008069C
	public static Rect SliceTop(ref Rect rect, float amount)
	{
		Rect result = new Rect(rect);
		result.yMax = result.yMin + amount;
		rect.yMin += amount;
		return result;
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x000822D8 File Offset: 0x000806D8
	public static Rect SliceBottom(ref Rect rect, float amount)
	{
		Rect result = new Rect(rect);
		result.yMin = result.yMax - amount;
		rect.yMax -= amount;
		return result;
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x00082314 File Offset: 0x00080714
	public static Rect[] SplitVertical(Rect rect, int numberOfGeneratedRects)
	{
		float[] array = new float[numberOfGeneratedRects];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 1f;
		}
		return RectUtilities.SplitVertical(rect, array);
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0008234C File Offset: 0x0008074C
	public static Rect[] SplitVertical(Rect rect, params float[] weights)
	{
		float totalWeight = 0f;
		Array.ForEach<float>(weights, delegate(float weight)
		{
			totalWeight += weight;
		});
		Rect[] array = new Rect[weights.Length];
		float height = rect.height;
		for (int i = 0; i < weights.Length - 1; i++)
		{
			array[i] = RectUtilities.SliceTop(ref rect, Mathf.Floor(height * weights[i] / totalWeight));
		}
		array[array.Length - 1] = rect;
		return array;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x000823DC File Offset: 0x000807DC
	public static Rect[] SplitHorizontal(Rect rect, int numberOfGeneratedRects)
	{
		float[] array = new float[numberOfGeneratedRects];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 1f;
		}
		return RectUtilities.SplitHorizontal(rect, array);
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x00082414 File Offset: 0x00080814
	public static Rect[] SplitHorizontal(Rect rect, params float[] weights)
	{
		float totalWeight = 0f;
		Array.ForEach<float>(weights, delegate(float weight)
		{
			totalWeight += weight;
		});
		Rect[] array = new Rect[weights.Length];
		float width = rect.width;
		for (int i = 0; i < weights.Length - 1; i++)
		{
			array[i] = RectUtilities.SliceLeft(ref rect, Mathf.Floor(width * weights[i] / totalWeight));
		}
		array[array.Length - 1] = rect;
		return array;
	}
}
