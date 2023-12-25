using System;

// Token: 0x0200037C RID: 892
public class ClassStringAssembler
{
	// Token: 0x06000A65 RID: 2661 RVA: 0x0007EA00 File Offset: 0x0007CE00
	public ClassStringAssembler(int indent = 0)
	{
		this.indents = indent;
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x0007EA1A File Offset: 0x0007CE1A
	public void Add(string s)
	{
		this.value += s;
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0007EA30 File Offset: 0x0007CE30
	public void AddLine(string s)
	{
		int index = 0;
		if (s.Length > 0)
		{
			index = s.Length - 1;
		}
		if (s.Length > 0 && (s[0] == '}' || s[0] == ')' || s[0] == ']'))
		{
			this.indents--;
		}
		this.Add("\n" + this.PreIndent() + s);
		if (s.Length > 0 && (s[index] == '{' || s[index] == '(' || s[index] == '['))
		{
			this.indents++;
		}
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0007EAF3 File Offset: 0x0007CEF3
	public void Break()
	{
		this.value += "\n";
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0007EB0B File Offset: 0x0007CF0B
	public void Indent()
	{
		this.indents++;
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x0007EB1B File Offset: 0x0007CF1B
	public void Undent()
	{
		this.indents--;
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0007EB2C File Offset: 0x0007CF2C
	private string PreIndent()
	{
		string text = string.Empty;
		for (int i = 0; i < this.indents; i++)
		{
			text += "\t";
		}
		return text;
	}

	// Token: 0x0400146B RID: 5227
	private int indents;

	// Token: 0x0400146C RID: 5228
	public string value = string.Empty;
}
