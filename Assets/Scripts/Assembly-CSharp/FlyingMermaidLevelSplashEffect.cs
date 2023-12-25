using System;
using UnityEngine;

// Token: 0x0200069E RID: 1694
public class FlyingMermaidLevelSplashEffect : Effect
{
	// Token: 0x060023E7 RID: 9191 RVA: 0x001515F4 File Offset: 0x0014F9F4
	private void LayerUp()
	{
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		int num = component.sortingOrder;
		string text = component.sortingLayerName;
		if (text == "Foreground" || (text == "Background" && num < 80))
		{
			num = num - num % 20 + 21;
		}
		else
		{
			text = "Foreground";
			num = 1;
		}
		component.sortingLayerName = text;
		component.sortingOrder = num;
	}
}
