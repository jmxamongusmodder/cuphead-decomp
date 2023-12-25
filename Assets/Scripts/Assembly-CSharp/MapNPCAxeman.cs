using System;
using UnityEngine;

// Token: 0x02000947 RID: 2375
public class MapNPCAxeman : MonoBehaviour
{
	// Token: 0x0600377A RID: 14202 RVA: 0x001FE38B File Offset: 0x001FC78B
	private void Start()
	{
		if (PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			base.transform.position = this.positionAfterWorld1;
		}
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x001FE3C2 File Offset: 0x001FC7C2
	protected void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.positionAfterWorld1, 0.5f);
	}

	// Token: 0x04003F8F RID: 16271
	public Vector3 positionAfterWorld1;

	// Token: 0x04003F90 RID: 16272
	[SerializeField]
	private int dialoguerVariableID = 3;
}
