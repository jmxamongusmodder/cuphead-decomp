using System;
using System.Diagnostics;
using UnityEngine;

namespace RektTransform
{
	// Token: 0x0200036E RID: 878
	public static class RectTransformExtension
	{
		// Token: 0x060009CC RID: 2508 RVA: 0x0007D108 File Offset: 0x0007B508
		[Conditional("REKT_LOG_ACTIVE")]
		private static void Log(object message)
		{
			UnityEngine.Debug.Log(message);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0007D110 File Offset: 0x0007B510
		public static void DebugOutput(this RectTransform RT)
		{
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0007D114 File Offset: 0x0007B514
		public static Rect GetWorldRect(this RectTransform RT)
		{
			Vector3[] array = new Vector3[4];
			RT.GetWorldCorners(array);
			Vector2 size = new Vector2(array[2].x - array[1].x, array[1].y - array[0].y);
			return new Rect(new Vector2(array[1].x, -array[1].y), size);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0007D18C File Offset: 0x0007B58C
		public static MinMax GetAnchors(this RectTransform RT)
		{
			return new MinMax(RT.anchorMin, RT.anchorMax);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0007D19F File Offset: 0x0007B59F
		public static void SetAnchors(this RectTransform RT, MinMax anchors)
		{
			RT.anchorMin = anchors.min;
			RT.anchorMax = anchors.max;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0007D1BB File Offset: 0x0007B5BB
		public static RectTransform GetParent(this RectTransform RT)
		{
			return RT.parent as RectTransform;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0007D1C8 File Offset: 0x0007B5C8
		public static float GetWidth(this RectTransform RT)
		{
			return RT.rect.width;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0007D1E4 File Offset: 0x0007B5E4
		public static float GetHeight(this RectTransform RT)
		{
			return RT.rect.height;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0007D1FF File Offset: 0x0007B5FF
		public static Vector2 GetSize(this RectTransform RT)
		{
			return new Vector2(RT.GetWidth(), RT.GetHeight());
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0007D212 File Offset: 0x0007B612
		public static void SetWidth(this RectTransform RT, float width)
		{
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0007D21C File Offset: 0x0007B61C
		public static void SetHeight(this RectTransform RT, float height)
		{
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0007D226 File Offset: 0x0007B626
		public static void SetSize(this RectTransform RT, float width, float height)
		{
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0007D238 File Offset: 0x0007B638
		public static void SetSize(this RectTransform RT, Vector2 size)
		{
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
			RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0007D258 File Offset: 0x0007B658
		public static Vector2 GetLeft(this RectTransform RT)
		{
			return new Vector2(RT.offsetMin.x, RT.anchoredPosition.y);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0007D288 File Offset: 0x0007B688
		public static Vector2 GetRight(this RectTransform RT)
		{
			return new Vector2(RT.offsetMax.x, RT.anchoredPosition.y);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0007D2B8 File Offset: 0x0007B6B8
		public static Vector2 GetTop(this RectTransform RT)
		{
			return new Vector2(RT.anchoredPosition.x, RT.offsetMax.y);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0007D2E8 File Offset: 0x0007B6E8
		public static Vector2 GetBottom(this RectTransform RT)
		{
			return new Vector2(RT.anchoredPosition.x, RT.offsetMin.y);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0007D318 File Offset: 0x0007B718
		public static void SetLeft(this RectTransform RT, float left)
		{
			float xMin = RT.GetParent().rect.xMin;
			float num = RT.anchorMin.x * 2f - 1f;
			RT.offsetMin = new Vector2(xMin + xMin * num + left, RT.offsetMin.y);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0007D378 File Offset: 0x0007B778
		public static void SetRight(this RectTransform RT, float right)
		{
			float xMax = RT.GetParent().rect.xMax;
			float num = RT.anchorMax.x * 2f - 1f;
			RT.offsetMax = new Vector2(xMax - xMax * num + right, RT.offsetMax.y);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0007D3D8 File Offset: 0x0007B7D8
		public static void SetTop(this RectTransform RT, float top)
		{
			float yMax = RT.GetParent().rect.yMax;
			float num = RT.anchorMax.y * 2f - 1f;
			RT.offsetMax = new Vector2(RT.offsetMax.x, yMax - yMax * num + top);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0007D438 File Offset: 0x0007B838
		public static void SetBottom(this RectTransform RT, float bottom)
		{
			float yMin = RT.GetParent().rect.yMin;
			float num = RT.anchorMin.y * 2f - 1f;
			RT.offsetMin = new Vector2(RT.offsetMin.x, yMin + yMin * num + bottom);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0007D495 File Offset: 0x0007B895
		public static void Left(this RectTransform RT, float left)
		{
			RT.SetLeft(left);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0007D49E File Offset: 0x0007B89E
		public static void Right(this RectTransform RT, float right)
		{
			RT.SetRight(-right);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0007D4A8 File Offset: 0x0007B8A8
		public static void Top(this RectTransform RT, float top)
		{
			RT.SetTop(-top);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0007D4B2 File Offset: 0x0007B8B2
		public static void Bottom(this RectTransform RT, float bottom)
		{
			RT.SetRight(bottom);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0007D4BC File Offset: 0x0007B8BC
		public static void SetLeftFrom(this RectTransform RT, MinMax anchor, float left)
		{
			RT.offsetMin = new Vector2(RT.AnchorToParentSpace(anchor.min - RT.anchorMin).x + left, RT.offsetMin.y);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0007D504 File Offset: 0x0007B904
		public static void SetRightFrom(this RectTransform RT, MinMax anchor, float right)
		{
			RT.offsetMax = new Vector2(RT.AnchorToParentSpace(anchor.max - RT.anchorMax).x + right, RT.offsetMax.y);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0007D54C File Offset: 0x0007B94C
		public static void SetTopFrom(this RectTransform RT, MinMax anchor, float top)
		{
			Vector2 vector = RT.AnchorToParentSpace(anchor.max - RT.anchorMax);
			RT.offsetMax = new Vector2(RT.offsetMax.x, vector.y + top);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0007D594 File Offset: 0x0007B994
		public static void SetBottomFrom(this RectTransform RT, MinMax anchor, float bottom)
		{
			Vector2 vector = RT.AnchorToParentSpace(anchor.min - RT.anchorMin);
			RT.offsetMin = new Vector2(RT.offsetMin.x, vector.y + bottom);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0007D5DC File Offset: 0x0007B9DC
		public static void SetRelativeLeft(this RectTransform RT, float left)
		{
			RT.offsetMin = new Vector2(RT.anchoredPosition.x + left, RT.offsetMin.y);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0007D614 File Offset: 0x0007BA14
		public static void SetRelativeRight(this RectTransform RT, float right)
		{
			RT.offsetMax = new Vector2(RT.anchoredPosition.x + right, RT.offsetMax.y);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0007D64C File Offset: 0x0007BA4C
		public static void SetRelativeTop(this RectTransform RT, float top)
		{
			RT.offsetMax = new Vector2(RT.offsetMax.x, RT.anchoredPosition.y + top);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0007D684 File Offset: 0x0007BA84
		public static void SetRelativeBottom(this RectTransform RT, float bottom)
		{
			RT.offsetMin = new Vector2(RT.offsetMin.x, RT.anchoredPosition.y + bottom);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0007D6BC File Offset: 0x0007BABC
		public static void MoveLeft(this RectTransform RT, float left = 0f)
		{
			float xMin = RT.GetParent().rect.xMin;
			float num = RT.anchorMax.x - RT.anchorMin.x;
			float num2 = RT.anchorMax.x * 2f - 1f;
			RT.anchoredPosition = new Vector2(xMin + xMin * num2 + left - num * xMin, RT.anchoredPosition.y);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0007D740 File Offset: 0x0007BB40
		public static void MoveRight(this RectTransform RT, float right = 0f)
		{
			float xMax = RT.GetParent().rect.xMax;
			float num = RT.anchorMax.x - RT.anchorMin.x;
			float num2 = RT.anchorMax.x * 2f - 1f;
			RT.anchoredPosition = new Vector2(xMax - xMax * num2 - right + num * xMax, RT.anchoredPosition.y);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0007D7C4 File Offset: 0x0007BBC4
		public static void MoveTop(this RectTransform RT, float top = 0f)
		{
			float yMax = RT.GetParent().rect.yMax;
			float num = RT.anchorMax.y - RT.anchorMin.y;
			float num2 = RT.anchorMax.y * 2f - 1f;
			RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, yMax - yMax * num2 - top + num * yMax);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0007D848 File Offset: 0x0007BC48
		public static void MoveBottom(this RectTransform RT, float bottom = 0f)
		{
			float yMin = RT.GetParent().rect.yMin;
			float num = RT.anchorMax.y - RT.anchorMin.y;
			float num2 = RT.anchorMax.y * 2f - 1f;
			RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, yMin + yMin * num2 + bottom - num * yMin);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0007D8CB File Offset: 0x0007BCCB
		public static void MoveLeftInside(this RectTransform RT, float left = 0f)
		{
			RT.MoveLeft(left + RT.GetWidth() / 2f);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0007D8E1 File Offset: 0x0007BCE1
		public static void MoveRightInside(this RectTransform RT, float right = 0f)
		{
			RT.MoveRight(right + RT.GetWidth() / 2f);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0007D8F7 File Offset: 0x0007BCF7
		public static void MoveTopInside(this RectTransform RT, float top = 0f)
		{
			RT.MoveTop(top + RT.GetHeight() / 2f);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0007D90D File Offset: 0x0007BD0D
		public static void MoveBottomInside(this RectTransform RT, float bottom = 0f)
		{
			RT.MoveBottom(bottom + RT.GetHeight() / 2f);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0007D923 File Offset: 0x0007BD23
		public static void MoveLeftOutside(this RectTransform RT, float left = 0f)
		{
			RT.MoveLeft(left - RT.GetWidth() / 2f);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0007D939 File Offset: 0x0007BD39
		public static void MoveRightOutside(this RectTransform RT, float right = 0f)
		{
			RT.MoveRight(right - RT.GetWidth() / 2f);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0007D94F File Offset: 0x0007BD4F
		public static void MoveTopOutside(this RectTransform RT, float top = 0f)
		{
			RT.MoveTop(top - RT.GetHeight() / 2f);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0007D965 File Offset: 0x0007BD65
		public static void MoveBottomOutside(this RectTransform RT, float bottom = 0f)
		{
			RT.MoveBottom(bottom - RT.GetHeight() / 2f);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0007D97B File Offset: 0x0007BD7B
		public static void Move(this RectTransform RT, float x, float y)
		{
			RT.MoveLeft(x);
			RT.MoveBottom(y);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0007D98B File Offset: 0x0007BD8B
		public static void Move(this RectTransform RT, Vector2 point)
		{
			RT.MoveLeft(point.x);
			RT.MoveBottom(point.y);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0007D9A7 File Offset: 0x0007BDA7
		public static void MoveInside(this RectTransform RT, float x, float y)
		{
			RT.MoveLeftInside(x);
			RT.MoveBottomInside(y);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0007D9B7 File Offset: 0x0007BDB7
		public static void MoveInside(this RectTransform RT, Vector2 point)
		{
			RT.MoveLeftInside(point.x);
			RT.MoveBottomInside(point.y);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0007D9D3 File Offset: 0x0007BDD3
		public static void MoveOutside(this RectTransform RT, float x, float y)
		{
			RT.MoveLeftOutside(x);
			RT.MoveBottomOutside(y);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0007D9E3 File Offset: 0x0007BDE3
		public static void MoveOutside(this RectTransform RT, Vector2 point)
		{
			RT.MoveLeftOutside(point.x);
			RT.MoveBottomOutside(point.y);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0007D9FF File Offset: 0x0007BDFF
		public static void MoveFrom(this RectTransform RT, MinMax anchor, Vector2 point)
		{
			RT.MoveFrom(anchor, point.x, point.y);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0007DA18 File Offset: 0x0007BE18
		public static void MoveFrom(this RectTransform RT, MinMax anchor, float x, float y)
		{
			Vector2 vector = RT.AnchorToParentSpace(RectTransformExtension.AnchorOrigin(anchor) - RT.AnchorOrigin());
			RT.anchoredPosition = new Vector2(vector.x + x, vector.y + y);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0007DA5A File Offset: 0x0007BE5A
		public static Vector2 ParentToChildSpace(this RectTransform RT, Vector2 point)
		{
			return RT.ParentToChildSpace(point.x, point.y);
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0007DA70 File Offset: 0x0007BE70
		public static Vector2 ParentToChildSpace(this RectTransform RT, float x, float y)
		{
			float xMin = RT.GetParent().rect.xMin;
			float yMin = RT.GetParent().rect.yMin;
			float num = RT.anchorMin.x * 2f - 1f;
			float num2 = RT.anchorMin.y * 2f - 1f;
			return new Vector2(xMin + xMin * num + x, yMin + yMin * num2 + y);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0007DAF4 File Offset: 0x0007BEF4
		public static Vector2 ChildToParentSpace(this RectTransform RT, float x, float y)
		{
			return RT.AnchorOriginParent() + new Vector2(x, y);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0007DB08 File Offset: 0x0007BF08
		public static Vector2 ChildToParentSpace(this RectTransform RT, Vector2 point)
		{
			return RT.AnchorOriginParent() + point;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0007DB16 File Offset: 0x0007BF16
		public static Vector2 ParentToAnchorSpace(this RectTransform RT, Vector2 point)
		{
			return RT.ParentToAnchorSpace(point.x, point.y);
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0007DB2C File Offset: 0x0007BF2C
		public static Vector2 ParentToAnchorSpace(this RectTransform RT, float x, float y)
		{
			Rect rect = RT.GetParent().rect;
			if (rect.width != 0f)
			{
				x /= rect.width;
			}
			else
			{
				x = 0f;
			}
			if (rect.height != 0f)
			{
				y /= rect.height;
			}
			else
			{
				y = 0f;
			}
			return new Vector2(x, y);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0007DB9C File Offset: 0x0007BF9C
		public static Vector2 AnchorToParentSpace(this RectTransform RT, float x, float y)
		{
			return new Vector2(x * RT.GetParent().rect.width, y * RT.GetParent().rect.height);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0007DBD8 File Offset: 0x0007BFD8
		public static Vector2 AnchorToParentSpace(this RectTransform RT, Vector2 point)
		{
			return new Vector2(point.x * RT.GetParent().rect.width, point.y * RT.GetParent().rect.height);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0007DC20 File Offset: 0x0007C020
		public static Vector2 AnchorOrigin(this RectTransform RT)
		{
			return RectTransformExtension.AnchorOrigin(RT.GetAnchors());
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0007DC30 File Offset: 0x0007C030
		public static Vector2 AnchorOrigin(MinMax anchor)
		{
			float x = anchor.min.x + (anchor.max.x - anchor.min.x) / 2f;
			float y = anchor.min.y + (anchor.max.y - anchor.min.y) / 2f;
			return new Vector2(x, y);
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0007DCA0 File Offset: 0x0007C0A0
		public static Vector2 AnchorOriginParent(this RectTransform RT)
		{
			return Vector2.Scale(RT.AnchorOrigin(), new Vector2(RT.GetParent().rect.width, RT.GetParent().rect.height));
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0007DCE4 File Offset: 0x0007C0E4
		public static Canvas GetRootCanvas(this RectTransform RT)
		{
			Canvas componentInParent = RT.GetComponentInParent<Canvas>();
			while (!componentInParent.isRootCanvas)
			{
				componentInParent = componentInParent.transform.parent.GetComponentInParent<Canvas>();
			}
			return componentInParent;
		}
	}
}
