using System;
using UnityEngine;

// Token: 0x0200038F RID: 911
public class EaseUtils
{
	// Token: 0x06000ADA RID: 2778 RVA: 0x00080D8C File Offset: 0x0007F18C
	public static float EaseInOut(EaseUtils.EaseType inEase, EaseUtils.EaseType outEase, float start, float end, float value)
	{
		if (value < 0.5f)
		{
			float value2 = Mathf.Clamp(value * 2f, 0f, 1f);
			float end2 = Mathf.Lerp(start, end, 0.5f);
			return EaseUtils.Ease(inEase, start, end2, value2);
		}
		if (value > 0.5f)
		{
			float value2 = Mathf.Clamp(value * 2f - 1f, 0f, 1f);
			float start2 = Mathf.Lerp(start, end, 0.5f);
			return EaseUtils.Ease(outEase, start2, end, value2);
		}
		return Mathf.Lerp(start, end, 0.5f);
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x00080E28 File Offset: 0x0007F228
	public static float Ease(EaseUtils.EaseType ease, float start, float end, float value)
	{
		switch (ease)
		{
		case EaseUtils.EaseType.easeInQuad:
			return EaseUtils.EaseInQuad(start, end, value);
		case EaseUtils.EaseType.easeOutQuad:
			return EaseUtils.EaseOutQuad(start, end, value);
		case EaseUtils.EaseType.easeInOutQuad:
			return EaseUtils.EaseInOutQuad(start, end, value);
		case EaseUtils.EaseType.easeInCubic:
			return EaseUtils.EaseInCubic(start, end, value);
		case EaseUtils.EaseType.easeOutCubic:
			return EaseUtils.EaseOutCubic(start, end, value);
		case EaseUtils.EaseType.easeInOutCubic:
			return EaseUtils.EaseInOutCubic(start, end, value);
		case EaseUtils.EaseType.easeInQuart:
			return EaseUtils.EaseInQuart(start, end, value);
		case EaseUtils.EaseType.easeOutQuart:
			return EaseUtils.EaseOutQuart(start, end, value);
		case EaseUtils.EaseType.easeInOutQuart:
			return EaseUtils.EaseInOutQuart(start, end, value);
		case EaseUtils.EaseType.easeInQuint:
			return EaseUtils.EaseInQuint(start, end, value);
		case EaseUtils.EaseType.easeOutQuint:
			return EaseUtils.EaseOutQuint(start, end, value);
		case EaseUtils.EaseType.easeInOutQuint:
			return EaseUtils.EaseInOutQuint(start, end, value);
		case EaseUtils.EaseType.easeInSine:
			return EaseUtils.EaseInSine(start, end, value);
		case EaseUtils.EaseType.easeOutSine:
			return EaseUtils.EaseOutSine(start, end, value);
		case EaseUtils.EaseType.easeInOutSine:
			return EaseUtils.EaseInOutSine(start, end, value);
		case EaseUtils.EaseType.easeInExpo:
			return EaseUtils.EaseInExpo(start, end, value);
		case EaseUtils.EaseType.easeOutExpo:
			return EaseUtils.EaseOutExpo(start, end, value);
		case EaseUtils.EaseType.easeInOutExpo:
			return EaseUtils.EaseInOutExpo(start, end, value);
		case EaseUtils.EaseType.easeInCirc:
			return EaseUtils.EaseInCirc(start, end, value);
		case EaseUtils.EaseType.easeOutCirc:
			return EaseUtils.EaseOutCirc(start, end, value);
		case EaseUtils.EaseType.easeInOutCirc:
			return EaseUtils.EaseInOutCirc(start, end, value);
		case EaseUtils.EaseType.spring:
			return EaseUtils.Spring(start, end, value);
		case EaseUtils.EaseType.easeInBounce:
			return EaseUtils.EaseInBounce(start, end, value);
		case EaseUtils.EaseType.easeOutBounce:
			return EaseUtils.EaseOutBounce(start, end, value);
		case EaseUtils.EaseType.easeInOutBounce:
			return EaseUtils.EaseInOutBounce(start, end, value);
		case EaseUtils.EaseType.easeInBack:
			return EaseUtils.EaseInBack(start, end, value);
		case EaseUtils.EaseType.easeOutBack:
			return EaseUtils.EaseOutBack(start, end, value);
		case EaseUtils.EaseType.easeInOutBack:
			return EaseUtils.EaseInOutBack(start, end, value);
		case EaseUtils.EaseType.easeInElastic:
			return EaseUtils.EaseInElastic(start, end, value);
		case EaseUtils.EaseType.easeOutElastic:
			return EaseUtils.EaseOutElastic(start, end, value);
		case EaseUtils.EaseType.easeInOutElastic:
			return EaseUtils.EaseInOutElastic(start, end, value);
		}
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x00080FE3 File Offset: 0x0007F3E3
	public static float Linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x00080FF0 File Offset: 0x0007F3F0
	public static float Clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * value;
		}
		return result;
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x00081068 File Offset: 0x0007F468
	public static float Spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x000810CC File Offset: 0x0007F4CC
	public static float EaseInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x000810DA File Offset: 0x0007F4DA
	public static float EaseOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x000810F0 File Offset: 0x0007F4F0
	public static float EaseInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value + start;
		}
		value -= 1f;
		return -end / 2f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x00081147 File Offset: 0x0007F547
	public static float EaseInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x00081157 File Offset: 0x0007F557
	public static float EaseOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00081178 File Offset: 0x0007F578
	public static float EaseInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value + 2f) + start;
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x000811CC File Offset: 0x0007F5CC
	public static float EaseInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x000811DE File Offset: 0x0007F5DE
	public static float EaseOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x00081200 File Offset: 0x0007F600
	public static float EaseInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value + start;
		}
		value -= 2f;
		return -end / 2f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x00081259 File Offset: 0x0007F659
	public static float EaseInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0008126D File Offset: 0x0007F66D
	public static float EaseOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x00081290 File Offset: 0x0007F690
	public static float EaseInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x000812EC File Offset: 0x0007F6EC
	public static float EaseInOutArbitraryCoefficient(float start, float end, float value, float c)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(value, c) + start;
		}
		value -= 2f;
		return end * 2f + start - end / 2f * (Mathf.Pow(Mathf.Abs(value), c - 1f) * Mathf.Abs(value) + 2f);
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x0008135E File Offset: 0x0007F75E
	public static float EaseInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value / 1f * 1.5707964f) + end + start;
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0008137E File Offset: 0x0007F77E
	public static float EaseOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value / 1f * 1.5707964f) + start;
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0008139B File Offset: 0x0007F79B
	public static float EaseInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.1415927f * value / 1f) - 1f) + start;
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x000813C5 File Offset: 0x0007F7C5
	public static float EaseInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x000813ED File Offset: 0x0007F7ED
	public static float EaseOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x00081418 File Offset: 0x0007F818
	public static float EaseInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0008148B File Offset: 0x0007F88B
	public static float EaseInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x000814AB File Offset: 0x0007F8AB
	public static float EaseOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x000814D0 File Offset: 0x0007F8D0
	public static float EaseInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x00081540 File Offset: 0x0007F940
	public static float EaseInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - EaseUtils.EaseOutBounce(0f, end, num - value) + start;
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0008156C File Offset: 0x0007F96C
	public static float EaseOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.95454544f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x00081614 File Offset: 0x0007FA14
	public static float EaseInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num / 2f)
		{
			return EaseUtils.EaseInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return EaseUtils.EaseOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00081678 File Offset: 0x0007FA78
	public static float EaseInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x000816AC File Offset: 0x0007FAAC
	public static float EaseOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value = value / 1f - 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x000816EC File Offset: 0x0007FAEC
	public static float EaseInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0008176C File Offset: 0x0007FB6C
	public static float Punch(float amplitude, float value)
	{
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.2831855f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.2831855f / num);
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x000817E4 File Offset: 0x0007FBE4
	public static float EaseInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0008189C File Offset: 0x0007FC9C
	public static float EaseOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0008194C File Offset: 0x0007FD4C
	public static float EaseInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num / 2f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) * 0.5f + end + start;
	}

	// Token: 0x04001494 RID: 5268
	public const float EaseOutBounceTime = 0.36363637f;

	// Token: 0x02000390 RID: 912
	public enum EaseType
	{
		// Token: 0x04001496 RID: 5270
		easeInQuad,
		// Token: 0x04001497 RID: 5271
		easeOutQuad,
		// Token: 0x04001498 RID: 5272
		easeInOutQuad,
		// Token: 0x04001499 RID: 5273
		easeInCubic,
		// Token: 0x0400149A RID: 5274
		easeOutCubic,
		// Token: 0x0400149B RID: 5275
		easeInOutCubic,
		// Token: 0x0400149C RID: 5276
		easeInQuart,
		// Token: 0x0400149D RID: 5277
		easeOutQuart,
		// Token: 0x0400149E RID: 5278
		easeInOutQuart,
		// Token: 0x0400149F RID: 5279
		easeInQuint,
		// Token: 0x040014A0 RID: 5280
		easeOutQuint,
		// Token: 0x040014A1 RID: 5281
		easeInOutQuint,
		// Token: 0x040014A2 RID: 5282
		easeInSine,
		// Token: 0x040014A3 RID: 5283
		easeOutSine,
		// Token: 0x040014A4 RID: 5284
		easeInOutSine,
		// Token: 0x040014A5 RID: 5285
		easeInExpo,
		// Token: 0x040014A6 RID: 5286
		easeOutExpo,
		// Token: 0x040014A7 RID: 5287
		easeInOutExpo,
		// Token: 0x040014A8 RID: 5288
		easeInCirc,
		// Token: 0x040014A9 RID: 5289
		easeOutCirc,
		// Token: 0x040014AA RID: 5290
		easeInOutCirc,
		// Token: 0x040014AB RID: 5291
		linear,
		// Token: 0x040014AC RID: 5292
		spring,
		// Token: 0x040014AD RID: 5293
		easeInBounce,
		// Token: 0x040014AE RID: 5294
		easeOutBounce,
		// Token: 0x040014AF RID: 5295
		easeInOutBounce,
		// Token: 0x040014B0 RID: 5296
		easeInBack,
		// Token: 0x040014B1 RID: 5297
		easeOutBack,
		// Token: 0x040014B2 RID: 5298
		easeInOutBack,
		// Token: 0x040014B3 RID: 5299
		easeInElastic,
		// Token: 0x040014B4 RID: 5300
		easeOutElastic,
		// Token: 0x040014B5 RID: 5301
		easeInOutElastic,
		// Token: 0x040014B6 RID: 5302
		punch
	}
}
