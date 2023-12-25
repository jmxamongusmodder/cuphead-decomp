using System;
using UnityEngine;

// Token: 0x020008D0 RID: 2256
public class HarbourPlatformingLevelOctopusHead : LevelPlatform
{
	// Token: 0x060034D1 RID: 13521 RVA: 0x001EB727 File Offset: 0x001E9B27
	public override void AddChild(Transform player)
	{
		base.AddChild(player);
		this.octopus.animator.SetBool("playerOn", true);
	}

	// Token: 0x060034D2 RID: 13522 RVA: 0x001EB746 File Offset: 0x001E9B46
	public override void OnPlayerExit(Transform player)
	{
		base.OnPlayerExit(player);
		if (base.transform.childCount <= 1)
		{
			this.octopus.animator.SetBool("playerOn", false);
		}
	}

	// Token: 0x04003CFB RID: 15611
	[SerializeField]
	private HarbourPlatformingLevelOctopus octopus;
}
