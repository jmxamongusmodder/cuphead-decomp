using System;
using System.Collections.Generic;

// Token: 0x0200039D RID: 925
public static class AnimationChartParser
{
	// Token: 0x06000B47 RID: 2887 RVA: 0x00082918 File Offset: 0x00080D18
	public static TextSegment CreateTextSegment(string segment)
	{
		TextSegment textSegment = new TextSegment();
		string text = segment;
		string[] array = segment.Split(new char[]
		{
			'x'
		});
		if (array.Length <= 2)
		{
			if (array.Length == 2)
			{
				Parser.IntTryParse(array[array.Length - 1], out textSegment.multiplier);
				text = array[0].Substring(1, array[0].Length - 2);
			}
			else if (text.Substring(0, 1) == "(")
			{
				text = array[0].Substring(1, array[0].Length - 2);
			}
			string[] array2 = text.Split(new char[]
			{
				'-'
			});
			if (array2.Length <= 2)
			{
				for (int i = 0; i < array2.Length; i++)
				{
					char[] array3 = array2[i].ToCharArray();
					int startIndex = 0;
					for (int j = 0; j < array3.Length; j++)
					{
						if (array3[j] != '\0')
						{
							startIndex = j;
							break;
						}
					}
					if (i == 0)
					{
						Parser.IntTryParse(array2[i].Substring(startIndex), out textSegment.frameStart);
					}
					else
					{
						Parser.IntTryParse(array2[i].Substring(startIndex), out textSegment.frameEnd);
					}
				}
			}
			else
			{
				Debug.LogError("Syntax Error: There can't be more than one hyphen in a segment", null);
			}
			if (text == "#")
			{
				textSegment.isBlank = true;
			}
			return textSegment;
		}
		Debug.LogError("There can't be more than one x in a segment", null);
		return null;
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x00082A8C File Offset: 0x00080E8C
	public static List<int> GetFrames(List<ImageData> spritesChosen, TextSegment[] textSegments)
	{
		bool flag = false;
		List<int> list = new List<int>();
		for (int i = 0; i < textSegments.Length; i++)
		{
			if (textSegments[i].frameStart > textSegments[i].frameEnd)
			{
				flag = true;
			}
			int num;
			if (textSegments[i].frameEnd == 0 && !textSegments[i].isBlank)
			{
				num = 1;
			}
			else
			{
				num = ((!flag) ? (textSegments[i].frameEnd - textSegments[i].frameStart) : (textSegments[i].frameStart - textSegments[i].frameEnd)) + 1;
			}
			int num2 = (textSegments[i].multiplier != 0) ? textSegments[i].multiplier : 1;
			int num3 = 0;
			for (int j = 0; j < num2; j++)
			{
				for (int k = 0; k < num; k++)
				{
					for (int l = 0; l < spritesChosen.Count; l++)
					{
						if (textSegments[i].isBlank)
						{
							num3 = -1;
						}
						if (spritesChosen[l].frameNum == textSegments[i].frameStart)
						{
							if (k == 0)
							{
								num3 = spritesChosen[l].frameNum - 1;
							}
							else if (num3 < textSegments[i].frameEnd - 1)
							{
								num3++;
							}
							else if (num3 > textSegments[i].frameEnd - 1)
							{
								num3--;
							}
						}
					}
					list.Add(num3);
				}
			}
		}
		return list;
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x00082C0C File Offset: 0x0008100C
	public static int GetFrameNumber(string path)
	{
		int result = 0;
		char[] array = path.ToCharArray();
		int startIndex = 0;
		int length = 0;
		for (int i = array.Length - 1; i > 0; i--)
		{
			if (array[i] == '.')
			{
				length = array.Length - i;
			}
			if (array[i] == '0' && array[i - 1] == '_')
			{
				startIndex = i;
				break;
			}
		}
		string text = path.Substring(startIndex, length);
		array = text.ToCharArray();
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] != '0')
			{
				startIndex = j;
				break;
			}
		}
		Parser.IntTryParse(text.Substring(startIndex), out result);
		return result;
	}
}
