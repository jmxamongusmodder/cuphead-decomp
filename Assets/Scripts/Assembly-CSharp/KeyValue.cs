using System;
using System.Collections.Generic;

// Token: 0x0200037D RID: 893
[Serializable]
public class KeyValue
{
	// Token: 0x06000A6C RID: 2668 RVA: 0x0007EB63 File Offset: 0x0007CF63
	public KeyValue()
	{
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0007EB76 File Offset: 0x0007CF76
	public KeyValue(string key, float value)
	{
		this.key = key;
		this.value = value;
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0007EB98 File Offset: 0x0007CF98
	public static KeyValue[] ListFromString(string keyValueString, char[] allowedCharacters)
	{
		List<KeyValue> list = new List<KeyValue>();
		List<char> list2 = new List<char>(allowedCharacters);
		list2.Add(',');
		list2.Add(':');
		keyValueString.Replace(" ", string.Empty);
		for (int i = 0; i < keyValueString.Length; i++)
		{
			bool flag = true;
			foreach (char c in list2)
			{
				if (keyValueString[i] == c)
				{
					flag = false;
				}
			}
			if (flag)
			{
				keyValueString.Remove(i, 1);
			}
		}
		string[] array = keyValueString.Split(new char[]
		{
			','
		});
		for (int j = 0; j < array.Length; j++)
		{
			string[] array2 = array[j].Split(new char[]
			{
				':'
			});
			if (array2.Length == 2)
			{
				string text = array2[0].Replace(" ", string.Empty);
				float num = 0f;
				bool flag2 = Parser.FloatTryParse(array2[1], out num);
				if (flag2 && text != null && !(text == string.Empty))
				{
					list.Add(new KeyValue(text, num));
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0007ED04 File Offset: 0x0007D104
	public KeyValue Clone()
	{
		return new KeyValue(this.key, this.value);
	}

	// Token: 0x0400146D RID: 5229
	public const char PAIR_SEPARATOR = ',';

	// Token: 0x0400146E RID: 5230
	public const char VALUE_SEPARATOR = ':';

	// Token: 0x0400146F RID: 5231
	public string key = string.Empty;

	// Token: 0x04001470 RID: 5232
	public float value;
}
