using System;

// Token: 0x0200036F RID: 879
public static class StringExtensions
{
	// Token: 0x06000A0D RID: 2573 RVA: 0x0007DD1C File Offset: 0x0007C11C
	public static string UpperFirst(this string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		char[] array = str.ToCharArray();
		array[0] = char.ToUpper(array[0]);
		return new string(array);
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x0007DD54 File Offset: 0x0007C154
	public static string LowerFirst(this string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		char[] array = str.ToCharArray();
		array[0] = char.ToLower(array[0]);
		return new string(array);
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x0007DD8C File Offset: 0x0007C18C
	public static string UppercaseWords(this string str)
	{
		char[] array = str.ToCharArray();
		if (array.Length >= 1 && char.IsLower(array[0]))
		{
			array[0] = char.ToUpper(array[0]);
		}
		for (int i = 1; i < array.Length; i++)
		{
			if ((array[i - 1] == ' ' || array[i - 1] == '_' || array[i - 1] == '/') && char.IsLower(array[i]))
			{
				array[i] = char.ToUpper(array[i]);
			}
		}
		return new string(array);
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0007DE18 File Offset: 0x0007C218
	public static string ToLowerIfNecessary(this string str)
	{
		if (str == null)
		{
			throw new NullReferenceException();
		}
		bool flag = false;
		int length = str.Length;
		for (int i = 0; i < length; i++)
		{
			if (char.IsUpper(str[i]))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			return str.ToLower();
		}
		return str;
	}
}
