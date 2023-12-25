using System;
using UnityEngine;

// Token: 0x02000990 RID: 2448
public class MapEquipUICardBackSelectIcon : AbstractMapCardIcon
{
	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06003946 RID: 14662 RVA: 0x00208512 File Offset: 0x00206912
	// (set) Token: 0x06003947 RID: 14663 RVA: 0x0020851A File Offset: 0x0020691A
	public int Index { get; set; }

	// Token: 0x06003948 RID: 14664 RVA: 0x00208524 File Offset: 0x00206924
	public int GetIndexOfNeighbor(Trilean2 direction)
	{
		MapEquipUICardBackSelectIcon mapEquipUICardBackSelectIcon = null;
		if (direction.x < 0)
		{
			mapEquipUICardBackSelectIcon = this.left;
		}
		if (direction.x > 0)
		{
			mapEquipUICardBackSelectIcon = this.right;
		}
		if (direction.y > 0)
		{
			mapEquipUICardBackSelectIcon = this.up;
		}
		if (direction.y < 0)
		{
			mapEquipUICardBackSelectIcon = this.down;
		}
		if (mapEquipUICardBackSelectIcon == null)
		{
			return this.Index;
		}
		return mapEquipUICardBackSelectIcon.Index;
	}

	// Token: 0x040040D7 RID: 16599
	[Header("Directions")]
	[SerializeField]
	private MapEquipUICardBackSelectIcon up;

	// Token: 0x040040D8 RID: 16600
	[SerializeField]
	private MapEquipUICardBackSelectIcon down;

	// Token: 0x040040D9 RID: 16601
	[SerializeField]
	private MapEquipUICardBackSelectIcon left;

	// Token: 0x040040DA RID: 16602
	[SerializeField]
	private MapEquipUICardBackSelectIcon right;
}
