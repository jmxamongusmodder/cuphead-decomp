using System;
using UnityEngine;

// Token: 0x020004BB RID: 1211
public class AirplaneLevelHydrantAttack : MonoBehaviour
{
	// Token: 0x0600140E RID: 5134 RVA: 0x000B297A File Offset: 0x000B0D7A
	private void AniEvent_SpawnHydrant(AnimationEvent ev)
	{
		((GameObject)ev.objectReferenceParameter).GetComponent<BasicProjectile>().Create(this.spawnPos.position, ev.floatParameter, (float)ev.intParameter * 1.5f);
	}

	// Token: 0x0600140F RID: 5135 RVA: 0x000B29B5 File Offset: 0x000B0DB5
	private void SFX_DOGFIGHT_Leader_CopterBG()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_leader_copterbackground");
	}

	// Token: 0x06001410 RID: 5136 RVA: 0x000B29C1 File Offset: 0x000B0DC1
	private void SFX_DOGFIGHT_Leader_CopterBGCannonFire()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_leader_copterbackground_canon");
	}

	// Token: 0x04001D3B RID: 7483
	private const float SPEED_MODIFIER = 1.5f;

	// Token: 0x04001D3C RID: 7484
	[SerializeField]
	private Transform spawnPos;

	// Token: 0x04001D3D RID: 7485
	private float speed = 800f;
}
