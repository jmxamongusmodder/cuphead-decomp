using System;
using UnityEngine;

// Token: 0x0200035A RID: 858
public class ColorAttribute : PropertyAttribute
{
	// Token: 0x06000950 RID: 2384 RVA: 0x0007BE2E File Offset: 0x0007A22E
	public ColorAttribute(float w)
	{
		this.color = new Color(w, w, w, 1f);
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0007BE49 File Offset: 0x0007A249
	public ColorAttribute(float r, float g, float b)
	{
		this.color = new Color(r, g, b, 1f);
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0007BE64 File Offset: 0x0007A264
	public ColorAttribute(float r, float g, float b, float a)
	{
		this.color = new Color(r, g, b, a);
	}

	// Token: 0x04001427 RID: 5159
	public Color color;
}
