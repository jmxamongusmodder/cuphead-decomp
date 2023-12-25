using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C90 RID: 3216
	public static class TMP_TextUtilities
	{
		// Token: 0x0600512A RID: 20778 RVA: 0x0029654C File Offset: 0x0029494C
		public static CaretInfo GetCursorInsertionIndex(TMP_Text textComponent, Vector3 position, Camera camera)
		{
			int num = TMP_TextUtilities.FindNearestCharacter(textComponent, position, camera, false);
			RectTransform rectTransform = textComponent.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			TMP_CharacterInfo tmp_CharacterInfo = textComponent.textInfo.characterInfo[num];
			Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
			Vector3 vector2 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
			float num2 = (position.x - vector.x) / (vector2.x - vector.x);
			if (num2 < 0.5f)
			{
				return new CaretInfo(num, CaretPosition.Left);
			}
			return new CaretInfo(num, CaretPosition.Right);
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x002965EC File Offset: 0x002949EC
		public static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera)
		{
			int num = TMP_TextUtilities.FindNearestCharacter(textComponent, position, camera, false);
			RectTransform rectTransform = textComponent.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			TMP_CharacterInfo tmp_CharacterInfo = textComponent.textInfo.characterInfo[num];
			Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
			Vector3 vector2 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
			float num2 = (position.x - vector.x) / (vector2.x - vector.x);
			if (num2 < 0.5f)
			{
				return num;
			}
			return num + 1;
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x00296680 File Offset: 0x00294A80
		public static bool IsIntersectingRectTransform(RectTransform rectTransform, Vector3 position, Camera camera)
		{
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			rectTransform.GetWorldCorners(TMP_TextUtilities.m_rectWorldCorners);
			return TMP_TextUtilities.PointIntersectRectangle(position, TMP_TextUtilities.m_rectWorldCorners[0], TMP_TextUtilities.m_rectWorldCorners[1], TMP_TextUtilities.m_rectWorldCorners[2], TMP_TextUtilities.m_rectWorldCorners[3]);
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x002966F8 File Offset: 0x00294AF8
		public static int FindIntersectingCharacter(TextMeshProUGUI text, Vector3 position, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tmp_CharacterInfo.isVisible)
				{
					Vector3 a = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
					Vector3 b = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
					Vector3 c = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
					Vector3 d = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x002967EC File Offset: 0x00294BEC
		public static int FindIntersectingCharacter(TextMeshPro text, Vector3 position, Camera camera, bool visibleOnly)
		{
			Transform transform = text.transform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tmp_CharacterInfo.isVisible)
				{
					Vector3 a = transform.TransformPoint(tmp_CharacterInfo.bottomLeft);
					Vector3 b = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
					Vector3 c = transform.TransformPoint(tmp_CharacterInfo.topRight);
					Vector3 d = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x002968E0 File Offset: 0x00294CE0
		public static int FindNearestCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = 0;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tmp_CharacterInfo.isVisible)
				{
					Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
					Vector3 vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
					Vector3 vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
					Vector3 vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4))
					{
						return i;
					}
					float num2 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
					float num3 = TMP_TextUtilities.DistanceToLine(vector2, vector3, position);
					float num4 = TMP_TextUtilities.DistanceToLine(vector3, vector4, position);
					float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector, position);
					float num6 = (num2 >= num3) ? num3 : num2;
					num6 = ((num6 >= num4) ? num4 : num6);
					num6 = ((num6 >= num5) ? num5 : num6);
					if (num > num6)
					{
						num = num6;
						result = i;
					}
				}
			}
			return result;
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x00296A58 File Offset: 0x00294E58
		public static int FindNearestCharacter(TextMeshProUGUI text, Vector3 position, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = 0;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tmp_CharacterInfo.isVisible)
				{
					Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
					Vector3 vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
					Vector3 vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
					Vector3 vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4))
					{
						return i;
					}
					float num2 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
					float num3 = TMP_TextUtilities.DistanceToLine(vector2, vector3, position);
					float num4 = TMP_TextUtilities.DistanceToLine(vector3, vector4, position);
					float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector, position);
					float num6 = (num2 >= num3) ? num3 : num2;
					num6 = ((num6 >= num4) ? num4 : num6);
					num6 = ((num6 >= num5) ? num5 : num6);
					if (num > num6)
					{
						num = num6;
						result = i;
					}
				}
			}
			return result;
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x00296BD0 File Offset: 0x00294FD0
		public static int FindNearestCharacter(TextMeshPro text, Vector3 position, Camera camera, bool visibleOnly)
		{
			Transform transform = text.transform;
			float num = float.PositiveInfinity;
			int result = 0;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tmp_CharacterInfo.isVisible)
				{
					Vector3 vector = transform.TransformPoint(tmp_CharacterInfo.bottomLeft);
					Vector3 vector2 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
					Vector3 vector3 = transform.TransformPoint(tmp_CharacterInfo.topRight);
					Vector3 vector4 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4))
					{
						return i;
					}
					float num2 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
					float num3 = TMP_TextUtilities.DistanceToLine(vector2, vector3, position);
					float num4 = TMP_TextUtilities.DistanceToLine(vector3, vector4, position);
					float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector, position);
					float num6 = (num2 >= num3) ? num3 : num2;
					num6 = ((num6 >= num4) ? num4 : num6);
					num6 = ((num6 >= num5) ? num5 : num6);
					if (num > num6)
					{
						num = num6;
						result = i;
					}
				}
			}
			return result;
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x00296D48 File Offset: 0x00295148
		public static int FindIntersectingWord(TextMeshProUGUI text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tmp_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 a = Vector3.zero;
				Vector3 b = Vector3.zero;
				Vector3 d = Vector3.zero;
				Vector3 c = Vector3.zero;
				float num = float.NegativeInfinity;
				float num2 = float.PositiveInfinity;
				for (int j = 0; j < tmp_WordInfo.characterCount; j++)
				{
					int num3 = tmp_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num3];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					bool flag2 = num3 <= text.maxVisibleCharacters && (int)tmp_CharacterInfo.lineNumber <= text.maxVisibleLines && (text.OverflowMode != TextOverflowModes.Page || (int)(tmp_CharacterInfo.pageNumber + 1) == text.pageToDisplay);
					num = Mathf.Max(num, tmp_CharacterInfo.ascender);
					num2 = Mathf.Min(num2, tmp_CharacterInfo.descender);
					if (!flag && flag2)
					{
						flag = true;
						a = new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f);
						b = new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f);
						if (tmp_WordInfo.characterCount == 1)
						{
							flag = false;
							d = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
							c = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
							a = rectTransform.TransformPoint(new Vector3(a.x, num2, 0f));
							b = rectTransform.TransformPoint(new Vector3(b.x, num, 0f));
							c = rectTransform.TransformPoint(new Vector3(c.x, num, 0f));
							d = rectTransform.TransformPoint(new Vector3(d.x, num2, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
							{
								return i;
							}
						}
					}
					if (flag && j == tmp_WordInfo.characterCount - 1)
					{
						flag = false;
						d = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
						c = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
						a = rectTransform.TransformPoint(new Vector3(a.x, num2, 0f));
						b = rectTransform.TransformPoint(new Vector3(b.x, num, 0f));
						c = rectTransform.TransformPoint(new Vector3(c.x, num, 0f));
						d = rectTransform.TransformPoint(new Vector3(d.x, num2, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
						{
							return i;
						}
					}
					else if (flag && lineNumber != (int)text.textInfo.characterInfo[num3 + 1].lineNumber)
					{
						flag = false;
						d = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
						c = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
						a = rectTransform.TransformPoint(new Vector3(a.x, num2, 0f));
						b = rectTransform.TransformPoint(new Vector3(b.x, num, 0f));
						c = rectTransform.TransformPoint(new Vector3(c.x, num, 0f));
						d = rectTransform.TransformPoint(new Vector3(d.x, num2, 0f));
						num = float.NegativeInfinity;
						num2 = float.PositiveInfinity;
						if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x0029716C File Offset: 0x0029556C
		public static int FindIntersectingWord(TextMeshPro text, Vector3 position, Camera camera)
		{
			Transform transform = text.transform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tmp_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 a = Vector3.zero;
				Vector3 b = Vector3.zero;
				Vector3 d = Vector3.zero;
				Vector3 c = Vector3.zero;
				float num = float.NegativeInfinity;
				float num2 = float.PositiveInfinity;
				for (int j = 0; j < tmp_WordInfo.characterCount; j++)
				{
					int num3 = tmp_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num3];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					bool flag2 = num3 <= text.maxVisibleCharacters && (int)tmp_CharacterInfo.lineNumber <= text.maxVisibleLines && (text.OverflowMode != TextOverflowModes.Page || (int)(tmp_CharacterInfo.pageNumber + 1) == text.pageToDisplay);
					num = Mathf.Max(num, tmp_CharacterInfo.ascender);
					num2 = Mathf.Min(num2, tmp_CharacterInfo.descender);
					if (!flag && flag2)
					{
						flag = true;
						a = new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f);
						b = new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f);
						if (tmp_WordInfo.characterCount == 1)
						{
							flag = false;
							d = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
							c = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
							a = transform.TransformPoint(new Vector3(a.x, num2, 0f));
							b = transform.TransformPoint(new Vector3(b.x, num, 0f));
							c = transform.TransformPoint(new Vector3(c.x, num, 0f));
							d = transform.TransformPoint(new Vector3(d.x, num2, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
							{
								return i;
							}
						}
					}
					if (flag && j == tmp_WordInfo.characterCount - 1)
					{
						flag = false;
						d = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
						c = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
						a = transform.TransformPoint(new Vector3(a.x, num2, 0f));
						b = transform.TransformPoint(new Vector3(b.x, num, 0f));
						c = transform.TransformPoint(new Vector3(c.x, num, 0f));
						d = transform.TransformPoint(new Vector3(d.x, num2, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
						{
							return i;
						}
					}
					else if (flag && lineNumber != (int)text.textInfo.characterInfo[num3 + 1].lineNumber)
					{
						flag = false;
						d = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
						c = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
						a = transform.TransformPoint(new Vector3(a.x, num2, 0f));
						b = transform.TransformPoint(new Vector3(b.x, num, 0f));
						c = transform.TransformPoint(new Vector3(c.x, num, 0f));
						d = transform.TransformPoint(new Vector3(d.x, num2, 0f));
						num = float.NegativeInfinity;
						num2 = float.PositiveInfinity;
						if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x00297590 File Offset: 0x00295990
		public static int FindNearestWord(TextMeshProUGUI text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = 0;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tmp_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				Vector3 vector4 = Vector3.zero;
				for (int j = 0; j < tmp_WordInfo.characterCount; j++)
				{
					int num2 = tmp_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num2];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					bool flag2 = num2 <= text.maxVisibleCharacters && (int)tmp_CharacterInfo.lineNumber <= text.maxVisibleLines && (text.OverflowMode != TextOverflowModes.Page || (int)(tmp_CharacterInfo.pageNumber + 1) == text.pageToDisplay);
					if (!flag && flag2)
					{
						flag = true;
						vector = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
						vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
						if (tmp_WordInfo.characterCount == 1)
						{
							flag = false;
							vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
						}
					}
					if (flag && j == tmp_WordInfo.characterCount - 1)
					{
						flag = false;
						vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
					}
					else if (flag && lineNumber != (int)text.textInfo.characterInfo[num2 + 1].lineNumber)
					{
						flag = false;
						vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
					}
				}
				float num3 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
				float num4 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
				float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
				float num6 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
				float num7 = (num3 >= num4) ? num4 : num3;
				num7 = ((num7 >= num5) ? num5 : num7);
				num7 = ((num7 >= num6) ? num6 : num7);
				if (num > num7)
				{
					num = num7;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x002978EC File Offset: 0x00295CEC
		public static int FindNearestWord(TextMeshPro text, Vector3 position, Camera camera)
		{
			Transform transform = text.transform;
			float num = float.PositiveInfinity;
			int result = 0;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tmp_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				Vector3 vector4 = Vector3.zero;
				for (int j = 0; j < tmp_WordInfo.characterCount; j++)
				{
					int num2 = tmp_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num2];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					bool flag2 = num2 <= text.maxVisibleCharacters && (int)tmp_CharacterInfo.lineNumber <= text.maxVisibleLines && (text.OverflowMode != TextOverflowModes.Page || (int)(tmp_CharacterInfo.pageNumber + 1) == text.pageToDisplay);
					if (!flag && flag2)
					{
						flag = true;
						vector = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
						vector2 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
						if (tmp_WordInfo.characterCount == 1)
						{
							flag = false;
							vector3 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
						}
					}
					if (flag && j == tmp_WordInfo.characterCount - 1)
					{
						flag = false;
						vector3 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						vector4 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
					}
					else if (flag && lineNumber != (int)text.textInfo.characterInfo[num2 + 1].lineNumber)
					{
						flag = false;
						vector3 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						vector4 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
					}
				}
				float num3 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
				float num4 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
				float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
				float num6 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
				float num7 = (num3 >= num4) ? num4 : num3;
				num7 = ((num7 >= num5) ? num5 : num7);
				num7 = ((num7 >= num6) ? num6 : num7);
				if (num > num7)
				{
					num = num7;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x00297C48 File Offset: 0x00296048
		public static int FindIntersectingLink(TextMeshProUGUI text, Vector3 position, Camera camera)
		{
			Transform transform = text.transform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, out position);
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tmp_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 a = Vector3.zero;
				Vector3 b = Vector3.zero;
				Vector3 d = Vector3.zero;
				Vector3 c = Vector3.zero;
				for (int j = 0; j < tmp_LinkInfo.linkTextLength; j++)
				{
					int num = tmp_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					if (!flag)
					{
						flag = true;
						a = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
						b = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
						if (tmp_LinkInfo.linkTextLength == 1)
						{
							flag = false;
							d = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							c = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
							{
								return i;
							}
						}
					}
					if (flag && j == tmp_LinkInfo.linkTextLength - 1)
					{
						flag = false;
						d = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						c = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
						{
							return i;
						}
					}
					else if (flag && lineNumber != (int)text.textInfo.characterInfo[num + 1].lineNumber)
					{
						flag = false;
						d = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						c = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x00297ECC File Offset: 0x002962CC
		public static int FindIntersectingLink(TextMeshPro text, Vector3 position, Camera camera)
		{
			Transform transform = text.transform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, out position);
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tmp_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 a = Vector3.zero;
				Vector3 b = Vector3.zero;
				Vector3 d = Vector3.zero;
				Vector3 c = Vector3.zero;
				for (int j = 0; j < tmp_LinkInfo.linkTextLength; j++)
				{
					int num = tmp_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					if (!flag)
					{
						flag = true;
						a = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
						b = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
						if (tmp_LinkInfo.linkTextLength == 1)
						{
							flag = false;
							d = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							c = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
							{
								return i;
							}
						}
					}
					if (flag && j == tmp_LinkInfo.linkTextLength - 1)
					{
						flag = false;
						d = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						c = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
						{
							return i;
						}
					}
					else if (flag && lineNumber != (int)text.textInfo.characterInfo[num + 1].lineNumber)
					{
						flag = false;
						d = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						c = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, a, b, c, d))
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x00298150 File Offset: 0x00296550
		public static int FindNearestLink(TextMeshProUGUI text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			float num = float.PositiveInfinity;
			int result = 0;
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tmp_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				Vector3 vector4 = Vector3.zero;
				for (int j = 0; j < tmp_LinkInfo.linkTextLength; j++)
				{
					int num2 = tmp_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num2];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					if (!flag)
					{
						flag = true;
						vector = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
						vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
						if (tmp_LinkInfo.linkTextLength == 1)
						{
							flag = false;
							vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
						}
					}
					if (flag && j == tmp_LinkInfo.linkTextLength - 1)
					{
						flag = false;
						vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
					}
					else if (flag && lineNumber != (int)text.textInfo.characterInfo[num2 + 1].lineNumber)
					{
						flag = false;
						vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
					}
				}
				float num3 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
				float num4 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
				float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
				float num6 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
				float num7 = (num3 >= num4) ? num4 : num3;
				num7 = ((num7 >= num5) ? num5 : num7);
				num7 = ((num7 >= num6) ? num6 : num7);
				if (num > num7)
				{
					num = num7;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x00298460 File Offset: 0x00296860
		public static int FindNearestLink(TextMeshPro text, Vector3 position, Camera camera)
		{
			Transform transform = text.transform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, out position);
			float num = float.PositiveInfinity;
			int result = 0;
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tmp_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				Vector3 vector4 = Vector3.zero;
				for (int j = 0; j < tmp_LinkInfo.linkTextLength; j++)
				{
					int num2 = tmp_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num2];
					int lineNumber = (int)tmp_CharacterInfo.lineNumber;
					if (!flag)
					{
						flag = true;
						vector = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
						vector2 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
						if (tmp_LinkInfo.linkTextLength == 1)
						{
							flag = false;
							vector3 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
						}
					}
					if (flag && j == tmp_LinkInfo.linkTextLength - 1)
					{
						flag = false;
						vector3 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						vector4 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
					}
					else if (flag && lineNumber != (int)text.textInfo.characterInfo[num2 + 1].lineNumber)
					{
						flag = false;
						vector3 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						vector4 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
					}
				}
				float num3 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
				float num4 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
				float num5 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
				float num6 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
				float num7 = (num3 >= num4) ? num4 : num3;
				num7 = ((num7 >= num5) ? num5 : num7);
				num7 = ((num7 >= num6) ? num6 : num7);
				if (num > num7)
				{
					num = num7;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x00298770 File Offset: 0x00296B70
		private static bool PointIntersectRectangle(Vector3 m, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			Vector3 vector = b - a;
			Vector3 rhs = m - a;
			Vector3 vector2 = c - b;
			Vector3 rhs2 = m - b;
			float num = Vector3.Dot(vector, rhs);
			float num2 = Vector3.Dot(vector2, rhs2);
			return 0f <= num && num <= Vector3.Dot(vector, vector) && 0f <= num2 && num2 <= Vector3.Dot(vector2, vector2);
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x002987E8 File Offset: 0x00296BE8
		public static bool ScreenPointToWorldPointInRectangle(Transform transform, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
		{
			worldPoint = Vector2.zero;
			Ray ray = RectTransformUtility.ScreenPointToRay(cam, screenPoint);
			Plane plane = new Plane(transform.rotation * Vector3.back, transform.position);
			float distance;
			if (!plane.Raycast(ray, out distance))
			{
				return false;
			}
			worldPoint = ray.GetPoint(distance);
			return true;
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x0029884C File Offset: 0x00296C4C
		private static bool IntersectLinePlane(TMP_TextUtilities.LineSegment line, Vector3 point, Vector3 normal, out Vector3 intersectingPoint)
		{
			intersectingPoint = Vector3.zero;
			Vector3 vector = line.Point2 - line.Point1;
			Vector3 rhs = line.Point1 - point;
			float num = Vector3.Dot(normal, vector);
			float num2 = -Vector3.Dot(normal, rhs);
			if (Mathf.Abs(num) < Mathf.Epsilon)
			{
				return num2 == 0f;
			}
			float num3 = num2 / num;
			if (num3 < 0f || num3 > 1f)
			{
				return false;
			}
			intersectingPoint = line.Point1 + num3 * vector;
			return true;
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x002988F0 File Offset: 0x00296CF0
		public static float DistanceToLine(Vector3 a, Vector3 b, Vector3 point)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = a - point;
			float num = Vector3.Dot(vector, vector2);
			if (num > 0f)
			{
				return Vector3.Dot(vector2, vector2);
			}
			Vector3 vector3 = point - b;
			if (Vector3.Dot(vector, vector3) > 0f)
			{
				return Vector3.Dot(vector3, vector3);
			}
			Vector3 vector4 = vector2 - vector * (num / Vector3.Dot(vector, vector));
			return Vector3.Dot(vector4, vector4);
		}

		// Token: 0x0600513E RID: 20798 RVA: 0x00298969 File Offset: 0x00296D69
		public static char ToLowerFast(char c)
		{
			return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"[(int)c];
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x00298976 File Offset: 0x00296D76
		public static char ToUpperFast(char c)
		{
			return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[(int)c];
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x00298984 File Offset: 0x00296D84
		public static int GetSimpleHashCode(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num ^ (int)s[i]);
			}
			return num;
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x002989BC File Offset: 0x00296DBC
		public static uint GetSimpleHashCodeLowercase(string s)
		{
			uint num = 5381U;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num ^ (uint)TMP_TextUtilities.ToLowerFast(s[i]));
			}
			return num;
		}

		// Token: 0x040053E9 RID: 21481
		private static Vector3[] m_rectWorldCorners = new Vector3[4];

		// Token: 0x040053EA RID: 21482
		private const string k_lookupStringL = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-";

		// Token: 0x040053EB RID: 21483
		private const string k_lookupStringU = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-";

		// Token: 0x02000C91 RID: 3217
		private struct LineSegment
		{
			// Token: 0x06005143 RID: 20803 RVA: 0x00298A07 File Offset: 0x00296E07
			public LineSegment(Vector3 p1, Vector3 p2)
			{
				this.Point1 = p1;
				this.Point2 = p2;
			}

			// Token: 0x040053EC RID: 21484
			public Vector3 Point1;

			// Token: 0x040053ED RID: 21485
			public Vector3 Point2;
		}
	}
}
