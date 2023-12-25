using System;
using UnityEngine;

// Token: 0x02000398 RID: 920
public class RectUtils
{
	// Token: 0x06000B35 RID: 2869 RVA: 0x000824D9 File Offset: 0x000808D9
	public static Rect OffsetRect(Rect rect, int offset)
	{
		return new Rect(rect.x - (float)offset, rect.y - (float)offset, rect.width + (float)(offset * 2), rect.height + (float)(offset * 2));
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0008250C File Offset: 0x0008090C
	public static Rect[] HorizontalDivide(Rect rect, int sections, float space)
	{
		Rect[] array = new Rect[sections];
		float num = rect.width / (float)sections - space * (float)(sections - 1) / (float)sections;
		for (int i = 0; i < sections; i++)
		{
			array[i] = new Rect(rect.x + num * (float)i + space * (float)i, rect.y, num, rect.height);
		}
		return array;
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x00082579 File Offset: 0x00080979
	public static Rect NewFromCenter(float xCenter, float yCenter, float width, float height)
	{
		return new Rect(xCenter - width * 0.5f, yCenter - height * 0.5f, width, height);
	}
}
