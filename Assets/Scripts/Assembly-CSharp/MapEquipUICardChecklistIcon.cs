using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000992 RID: 2450
public class MapEquipUICardChecklistIcon : AbstractMapCardIcon
{
	// Token: 0x0600394E RID: 14670 RVA: 0x00208690 File Offset: 0x00206A90
	public void SetTextColor(Color color)
	{
		this.iconText.color = color;
	}

	// Token: 0x040040DD RID: 16605
	public Text iconText;
}
