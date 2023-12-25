using System;
using System.Collections.Generic;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C4C RID: 3148
	public static class UISelectionUtility
	{
		// Token: 0x06004D5D RID: 19805 RVA: 0x00275CE8 File Offset: 0x002740E8
		public static Selectable FindNextSelectable(Selectable selectable, Transform transform, List<Selectable> allSelectables, Vector3 direction)
		{
			RectTransform rectTransform = transform as RectTransform;
			if (rectTransform == null)
			{
				return null;
			}
			direction = direction.normalized;
			Vector2 vector = Quaternion.Inverse(transform.rotation) * direction;
			Vector2 vector2 = transform.TransformPoint(UITools.GetPointOnRectEdge(rectTransform, vector));
			bool flag = direction == Vector3.left || direction == Vector3.right;
			float num = float.PositiveInfinity;
			float num2 = float.PositiveInfinity;
			Selectable selectable2 = null;
			Selectable selectable3 = null;
			Vector2 point = vector2 + vector * 999999f;
			for (int i = 0; i < allSelectables.Count; i++)
			{
				Selectable selectable4 = allSelectables[i];
				if (!(selectable4 == selectable) && !(selectable4 == null))
				{
					if (selectable4.navigation.mode != Navigation.Mode.None)
					{
						if (selectable4.IsInteractable() || ReflectionTools.GetPrivateField<Selectable, bool>(selectable4, "m_GroupsAllowInteraction"))
						{
							RectTransform rectTransform2 = selectable4.transform as RectTransform;
							if (!(rectTransform2 == null))
							{
								Rect worldSpaceRect = UITools.GetWorldSpaceRect(rectTransform2);
								float num3;
								if (MathTools.LineIntersectsRect(vector2, point, worldSpaceRect, out num3))
								{
									if (flag)
									{
										num3 *= 0.25f;
									}
									if (num3 < num2)
									{
										num2 = num3;
										selectable3 = selectable4;
									}
								}
								Vector2 v = rectTransform2.rect.center;
								Vector2 to = selectable4.transform.TransformPoint(v) - vector2;
								float num4 = Mathf.Abs(Vector2.Angle(vector, to));
								if (num4 <= 75f)
								{
									float sqrMagnitude = to.sqrMagnitude;
									if (sqrMagnitude < num)
									{
										num = sqrMagnitude;
										selectable2 = selectable4;
									}
								}
							}
						}
					}
				}
			}
			if (!(selectable3 != null) || !(selectable2 != null))
			{
				return selectable3 ?? selectable2;
			}
			if (num2 > num)
			{
				return selectable2;
			}
			return selectable3;
		}
	}
}
