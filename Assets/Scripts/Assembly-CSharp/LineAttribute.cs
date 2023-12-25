using System;
using UnityEngine;

// Token: 0x0200035B RID: 859
public class LineAttribute : PropertyAttribute
{
	// Token: 0x06000953 RID: 2387 RVA: 0x0007BE7C File Offset: 0x0007A27C
	public LineAttribute(int height)
	{
		this.height = height;
	}

	// Token: 0x04001428 RID: 5160
	public int height;
}
