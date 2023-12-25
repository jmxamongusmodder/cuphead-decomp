using System;
using UnityEngine;

// Token: 0x020007BF RID: 1983
public class SaltbakerLevelBGSaltHands : MonoBehaviour
{
	// Token: 0x06002CE2 RID: 11490 RVA: 0x001A6ECC File Offset: 0x001A52CC
	public void Play()
	{
		base.transform.position = this.positions[this.positionCounter];
		base.transform.localScale = new Vector3((float)((this.positionCounter != 0) ? -1 : 1), (this.positionCounter != 0) ? 1.02f : 1f);
		for (int i = 0; i < this.rends.Length; i++)
		{
			this.rends[i].sortingOrder = ((this.positionCounter != 0) ? 650 : 850) + i * 5;
		}
		this.anim.Play("SaltHands");
		this.SFX_SALTB_Bouncer_MakeBouncer();
		this.positionCounter = 1 - this.positionCounter;
	}

	// Token: 0x06002CE3 RID: 11491 RVA: 0x001A6FA4 File Offset: 0x001A53A4
	private void SFX_SALTB_Bouncer_MakeBouncer()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3_hands_makebouncer");
	}

	// Token: 0x04003556 RID: 13654
	[SerializeField]
	private Vector2[] positions;

	// Token: 0x04003557 RID: 13655
	[SerializeField]
	private Animator anim;

	// Token: 0x04003558 RID: 13656
	[SerializeField]
	private SpriteRenderer[] rends;

	// Token: 0x04003559 RID: 13657
	private int positionCounter;
}
