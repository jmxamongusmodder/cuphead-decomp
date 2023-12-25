using System;
using UnityEngine;

// Token: 0x02000991 RID: 2449
public class MapEquipUICardBackSelectSelectionCursor : MapEquipUICursor
{
	// Token: 0x0600394A RID: 14666 RVA: 0x0020864F File Offset: 0x00206A4F
	public override void SetPosition(Vector3 position)
	{
		base.SetPosition(position);
		this.Show();
	}

	// Token: 0x0600394B RID: 14667 RVA: 0x0020865E File Offset: 0x00206A5E
	public override void Show()
	{
		base.Show();
		base.animator.Play("Idle");
	}

	// Token: 0x0600394C RID: 14668 RVA: 0x00208676 File Offset: 0x00206A76
	public void Select()
	{
		base.animator.Play("Select");
	}

	// Token: 0x040040DC RID: 16604
	public int selectedIndex = -1;
}
