using System;
using UnityEngine;

// Token: 0x0200038D RID: 909
public static class DebugUtilities
{
	// Token: 0x06000AC8 RID: 2760 RVA: 0x00080A9C File Offset: 0x0007EE9C
	public static void DrawLine(Vector3 start, Vector3 end)
	{
		DebugUtilities.DrawLine(start, end, DebugUtilities.DefaultColor, 0f);
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x00080AAF File Offset: 0x0007EEAF
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0f)
	{
		DebugUtilities.DebugDrawer.DrawLine(start, end, color, duration);
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x00080ABA File Offset: 0x0007EEBA
	public static void DrawRay(Vector3 origin, Vector3 direction)
	{
		DebugUtilities.DrawRay(origin, direction, DebugUtilities.DefaultColor, 0f);
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x00080ACD File Offset: 0x0007EECD
	public static void DrawRay(Vector3 origin, Vector3 direction, Color color, float duration = 0f)
	{
		DebugUtilities.DebugDrawer.DrawLine(origin, origin + direction, color, duration);
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00080ADE File Offset: 0x0007EEDE
	public static void DrawBox2D(Vector2 origin, Vector2 size, float angle)
	{
		DebugUtilities.DrawBox2D(origin, size, angle, DebugUtilities.DefaultColor, 0f);
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x00080AF4 File Offset: 0x0007EEF4
	public static void DrawBox2D(Vector2 origin, Vector2 size, float angle, Color color, float duration = 0f)
	{
		Vector2 vector = size * 0.5f;
		Vector2 v = origin + new Vector2(-vector.x, vector.y);
		Vector2 v2 = origin + new Vector2(-vector.x, -vector.y);
		Vector2 v3 = origin + new Vector2(vector.x, vector.y);
		Vector2 v4 = origin + new Vector2(vector.x, -vector.y);
		if (!Mathf.Approximately(angle, 0f))
		{
			throw new Exception("Not supported in this library");
		}
		DebugUtilities.DebugDrawer.DrawLine(v, v3, color, duration);
		DebugUtilities.DebugDrawer.DrawLine(v3, v4, color, duration);
		DebugUtilities.DebugDrawer.DrawLine(v4, v2, color, duration);
		DebugUtilities.DebugDrawer.DrawLine(v2, v, color, duration);
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x00080BE7 File Offset: 0x0007EFE7
	public static void DrawVerticalPole(Vector3 center, float height)
	{
		DebugUtilities.DrawVerticalPole(center, height, DebugUtilities.DefaultColor, 0f);
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x00080BFA File Offset: 0x0007EFFA
	public static void DrawVerticalPole(Vector3 center, float height, Color color, float duration = 0f)
	{
		DebugUtilities.DrawLine(center + Vector3.up * height, center - Vector3.up * height, color, duration);
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x00080C25 File Offset: 0x0007F025
	public static void DrawHorizontalPole(Vector3 center, float width)
	{
		DebugUtilities.DrawHorizontalPole(center, width, DebugUtilities.DefaultColor, 0f);
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x00080C38 File Offset: 0x0007F038
	public static void DrawHorizontalPole(Vector3 center, float width, Color color, float duration = 0f)
	{
		DebugUtilities.DrawLine(center + Vector3.right * width, center - Vector3.right * width, color, duration);
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x00080C63 File Offset: 0x0007F063
	public static void DrawCircle2D(Vector3 position, float radius)
	{
		DebugUtilities.DrawCircle2D(position, radius, DebugUtilities.DefaultColor, 0f, true);
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x00080C77 File Offset: 0x0007F077
	public static void DrawCircle2D(Vector3 position, float radius, Color color, float duration = 0f, bool depthTest = true)
	{
		DebugUtilities.DrawCircle(position, Vector3.forward, Vector3.up, radius, 20, color, duration, depthTest);
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x00080C90 File Offset: 0x0007F090
	public static void DrawCircle(Vector3 position, Vector3 forward, Vector3 up, float radius, int segments = 20)
	{
		DebugUtilities.DrawCircle(position, forward, up, radius, segments, DebugUtilities.DefaultColor, 0f, true);
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x00080CA8 File Offset: 0x0007F0A8
	public static void DrawCircle(Vector3 position, Vector3 forward, Vector3 up, float radius, int segments, Color color, float duration = 0f, bool depthTest = true)
	{
		DebugUtilities.DrawEllipse(position, forward, up, radius, radius, segments, color, duration, depthTest);
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x00080CC8 File Offset: 0x0007F0C8
	public static void DrawEllipse(Vector3 pos, Vector3 forward, Vector3 up, float radiusX, float radiusY, int segments, Color color, float duration = 0f, bool depthTest = true)
	{
		float num = 0f;
		Quaternion rotation = Quaternion.LookRotation(forward, up);
		Vector3 point = Vector3.zero;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < segments + 1; i++)
		{
			zero.x = Mathf.Sin(0.017453292f * num) * radiusX;
			zero.y = Mathf.Cos(0.017453292f * num) * radiusY;
			if (i > 0)
			{
				DebugUtilities.DebugDrawer.DrawLine(rotation * point + pos, rotation * zero + pos, color, duration);
			}
			point = zero;
			num += 360f / (float)segments;
		}
	}

	// Token: 0x04001492 RID: 5266
	private const int DefaultEllipseSegments = 20;

	// Token: 0x04001493 RID: 5267
	private static readonly Color DefaultColor = Color.white;

	// Token: 0x0200038E RID: 910
	private static class DebugDrawer
	{
		// Token: 0x06000AD8 RID: 2776 RVA: 0x00080D78 File Offset: 0x0007F178
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
		{
			global::Debug.DrawLine(start, end, color, duration, false);
		}
	}
}
