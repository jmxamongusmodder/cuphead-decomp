using System;
using UnityEngine;

// Token: 0x020004BF RID: 1215
public class AirplaneLevelLeaderAnimation : MonoBehaviour
{
	// Token: 0x06001437 RID: 5175 RVA: 0x000B45E0 File Offset: 0x000B29E0
	private void Start()
	{
		this.rootPosition = base.transform.position;
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x000B45F3 File Offset: 0x000B29F3
	private void AniEvent_StartBulldog()
	{
		this.bulldogAnimation.SetTrigger("Continue");
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x000B4605 File Offset: 0x000B2A05
	private void AniEvent_SFX_LeaderBark()
	{
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_introbark");
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x000B4614 File Offset: 0x000B2A14
	private void Update()
	{
		base.transform.position = this.rootPosition + Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX * Vector3.right + Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleY * Vector3.up;
		this.wobbleTimer += CupheadTime.Delta * this.wobbleSpeed;
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x000B469E File Offset: 0x000B2A9E
	private void AnimationEvent_SFX_DOGFIGHT_Intro_LeaderCopterFlyby()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_leader_copterflybyexit");
	}

	// Token: 0x04001D61 RID: 7521
	[SerializeField]
	private Animator bulldogAnimation;

	// Token: 0x04001D62 RID: 7522
	private Vector3 rootPosition;

	// Token: 0x04001D63 RID: 7523
	private float wobbleTimer;

	// Token: 0x04001D64 RID: 7524
	[SerializeField]
	private float wobbleX = 10f;

	// Token: 0x04001D65 RID: 7525
	[SerializeField]
	private float wobbleY = 10f;

	// Token: 0x04001D66 RID: 7526
	[SerializeField]
	private float wobbleSpeed = 1f;
}
