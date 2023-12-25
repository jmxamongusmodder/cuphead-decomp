using System;
using UnityEngine;

// Token: 0x02000B2A RID: 2858
public class PatternString
{
	// Token: 0x06004542 RID: 17730 RVA: 0x00247B00 File Offset: 0x00245F00
	public PatternString(string[] patternString, bool randomizeMain = true, bool randomizeSub = true)
	{
		this.mainPatternString = patternString;
		this.mainIndex = ((!randomizeMain) ? 0 : UnityEngine.Random.Range(0, patternString.Length));
		this.subPatternString = this.mainPatternString[this.mainIndex].Split(new char[]
		{
			','
		});
		this.subIndex = ((!randomizeSub) ? 0 : UnityEngine.Random.Range(0, this.subPatternString.Length));
	}

	// Token: 0x06004543 RID: 17731 RVA: 0x00247B78 File Offset: 0x00245F78
	public PatternString(string patternString, bool randomizeSub = true)
	{
		this.mainIndex = 0;
		this.mainPatternString = new string[1];
		this.mainPatternString[0] = patternString;
		this.subPatternString = this.mainPatternString[0].Split(new char[]
		{
			','
		});
		this.subIndex = ((!randomizeSub) ? 0 : UnityEngine.Random.Range(0, this.subPatternString.Length));
	}

	// Token: 0x06004544 RID: 17732 RVA: 0x00247BE8 File Offset: 0x00245FE8
	public PatternString(string[] patternString, char subSubStringSplitter, bool randomizeMain = true, bool randomizeSub = true)
	{
		this.mainPatternString = patternString;
		this.mainIndex = ((!randomizeMain) ? 0 : UnityEngine.Random.Range(0, patternString.Length));
		this.subPatternString = this.mainPatternString[this.mainIndex].Split(new char[]
		{
			','
		});
		this.subIndex = ((!randomizeSub) ? 0 : UnityEngine.Random.Range(0, this.subPatternString.Length));
		this.subsubPatternString = this.subPatternString[this.subIndex].Split(new char[]
		{
			subSubStringSplitter
		});
		this.subSubStringSplitter = subSubStringSplitter;
	}

	// Token: 0x06004545 RID: 17733 RVA: 0x00247C8A File Offset: 0x0024608A
	public int SubStringLength()
	{
		return this.subPatternString.Length;
	}

	// Token: 0x06004546 RID: 17734 RVA: 0x00247C94 File Offset: 0x00246094
	public void SetMainStringIndex(int value)
	{
		this.mainIndex = value % this.mainPatternString.Length;
	}

	// Token: 0x06004547 RID: 17735 RVA: 0x00247CA6 File Offset: 0x002460A6
	public void SetSubStringIndex(int value)
	{
		this.subIndex = value % this.subPatternString.Length;
	}

	// Token: 0x06004548 RID: 17736 RVA: 0x00247CB8 File Offset: 0x002460B8
	public int GetMainStringIndex()
	{
		return this.mainIndex;
	}

	// Token: 0x06004549 RID: 17737 RVA: 0x00247CC0 File Offset: 0x002460C0
	public int GetSubStringIndex()
	{
		return this.subIndex;
	}

	// Token: 0x0600454A RID: 17738 RVA: 0x00247CC8 File Offset: 0x002460C8
	public char GetSubsubstringLetter(int index)
	{
		return this.subsubPatternString[index][0];
	}

	// Token: 0x0600454B RID: 17739 RVA: 0x00247CD8 File Offset: 0x002460D8
	public float GetSubsubstringFloat(int index)
	{
		float result = 0f;
		if (Parser.FloatTryParse(this.subsubPatternString[index], out result))
		{
			return result;
		}
		global::Debug.LogError("Syntax Error in" + this.subsubPatternString, null);
		return result;
	}

	// Token: 0x0600454C RID: 17740 RVA: 0x00247D18 File Offset: 0x00246118
	private char GetLetter()
	{
		return this.subPatternString[this.subIndex][0];
	}

	// Token: 0x0600454D RID: 17741 RVA: 0x00247D2D File Offset: 0x0024612D
	public char PopLetter()
	{
		this.IncrementString();
		return this.GetLetter();
	}

	// Token: 0x0600454E RID: 17742 RVA: 0x00247D3B File Offset: 0x0024613B
	public string GetString()
	{
		return this.subPatternString[this.subIndex];
	}

	// Token: 0x0600454F RID: 17743 RVA: 0x00247D4A File Offset: 0x0024614A
	public string PopString()
	{
		this.IncrementString();
		return this.GetString();
	}

	// Token: 0x06004550 RID: 17744 RVA: 0x00247D58 File Offset: 0x00246158
	public float GetFloat()
	{
		float result = 0f;
		if (Parser.FloatTryParse(this.subPatternString[this.subIndex], out result))
		{
			return result;
		}
		global::Debug.LogError("Syntax Error in" + this.mainPatternString, null);
		return result;
	}

	// Token: 0x06004551 RID: 17745 RVA: 0x00247D9D File Offset: 0x0024619D
	public float PopFloat()
	{
		this.IncrementString();
		return this.GetFloat();
	}

	// Token: 0x06004552 RID: 17746 RVA: 0x00247DAC File Offset: 0x002461AC
	private int GetInt()
	{
		int result = 0;
		if (Parser.IntTryParse(this.subPatternString[this.subIndex], out result))
		{
			return result;
		}
		global::Debug.LogError("Syntax Error in" + this.mainPatternString, null);
		return result;
	}

	// Token: 0x06004553 RID: 17747 RVA: 0x00247DED File Offset: 0x002461ED
	public int PopInt()
	{
		this.IncrementString();
		return this.GetInt();
	}

	// Token: 0x06004554 RID: 17748 RVA: 0x00247DFC File Offset: 0x002461FC
	public void IncrementString()
	{
		if (this.subIndex < this.subPatternString.Length - 1)
		{
			this.subIndex++;
		}
		else
		{
			this.mainIndex = (this.mainIndex + 1) % this.mainPatternString.Length;
			this.subIndex = 0;
		}
		this.subPatternString = this.mainPatternString[this.mainIndex].Split(new char[]
		{
			','
		});
		if (this.subsubPatternString != null)
		{
			this.subsubPatternString = this.subPatternString[this.subIndex].Split(new char[]
			{
				this.subSubStringSplitter
			});
		}
	}

	// Token: 0x04004AEB RID: 19179
	private int mainIndex;

	// Token: 0x04004AEC RID: 19180
	private int subIndex;

	// Token: 0x04004AED RID: 19181
	private char subSubStringSplitter;

	// Token: 0x04004AEE RID: 19182
	private string[] mainPatternString;

	// Token: 0x04004AEF RID: 19183
	private string[] subPatternString;

	// Token: 0x04004AF0 RID: 19184
	private string[] subsubPatternString;
}
