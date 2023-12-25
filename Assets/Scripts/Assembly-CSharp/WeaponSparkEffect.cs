using System;
using UnityEngine;

// Token: 0x02000A49 RID: 2633
public class WeaponSparkEffect : Effect
{
	// Token: 0x06003EBD RID: 16061 RVA: 0x002264FC File Offset: 0x002248FC
	public void SetPlayer(LevelPlayerController player)
	{
		if (player.motor.Grounded)
		{
			this.player = player;
			Vector3 localScale = base.transform.localScale;
			base.transform.parent = player.transform;
			base.transform.localScale = localScale;
			this.playerXScale = player.transform.localScale.x;
		}
	}

	// Token: 0x06003EBE RID: 16062 RVA: 0x00226562 File Offset: 0x00224962
	public void BringToFrontOfPlayer()
	{
		base.GetComponent<SpriteRenderer>().sortingOrder = 100;
	}

	// Token: 0x06003EBF RID: 16063 RVA: 0x00226571 File Offset: 0x00224971
	private void FixedUpdate()
	{
		if (this.player != null && !this.player.motor.Grounded)
		{
			this.player = null;
			base.transform.parent = null;
		}
	}

	// Token: 0x06003EC0 RID: 16064 RVA: 0x002265AC File Offset: 0x002249AC
	private void LateUpdate()
	{
		if (this.player != null && this.player.transform.localScale.x != this.playerXScale)
		{
			base.transform.SetLocalPosition(new float?(-base.transform.localPosition.x), null, null);
			this.player = null;
			base.transform.parent = null;
		}
	}

	// Token: 0x040045CB RID: 17867
	private LevelPlayerController player;

	// Token: 0x040045CC RID: 17868
	private float playerXScale;
}
