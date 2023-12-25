using System;
using UnityEngine;

// Token: 0x02000391 RID: 913
public static class EnumUtils
{
	// Token: 0x06000AFF RID: 2815 RVA: 0x00081A52 File Offset: 0x0007FE52
	public static T[] GetValues<T>()
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("T must be an enum type");
		}
		return (T[])Enum.GetValues(typeof(T));
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x00081A88 File Offset: 0x0007FE88
	public static string[] GetValuesAsStrings<T>()
	{
		T[] values = EnumUtils.GetValues<T>();
		string[] array = new string[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = values[i].ToString();
		}
		return array;
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x00081AD0 File Offset: 0x0007FED0
	public static int GetCount<T>()
	{
		return EnumUtils.GetValues<T>().Length;
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x00081ADC File Offset: 0x0007FEDC
	public static T Random<T>()
	{
		T[] values = EnumUtils.GetValues<T>();
		return values[UnityEngine.Random.Range(0, values.Length)];
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x00081B00 File Offset: 0x0007FF00
	public static T Parse<T>(string name)
	{
		T[] values = EnumUtils.GetValues<T>();
		for (int i = 0; i < values.Length; i++)
		{
			if (name == values[i].ToString())
			{
				return values[i];
			}
		}
		return values[0];
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x00081B58 File Offset: 0x0007FF58
	public static bool TryParse<T>(string name, out T result)
	{
		T[] values = EnumUtils.GetValues<T>();
		for (int i = 0; i < values.Length; i++)
		{
			if (name == values[i].ToString())
			{
				result = values[i];
				return true;
			}
		}
		result = values[0];
		return false;
	}
}
