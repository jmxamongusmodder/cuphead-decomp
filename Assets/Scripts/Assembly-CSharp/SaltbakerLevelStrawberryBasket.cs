using System;
using UnityEngine;

// Token: 0x020007D7 RID: 2007
public class SaltbakerLevelStrawberryBasket : MonoBehaviour
{
	// Token: 0x06002DD1 RID: 11729 RVA: 0x001B0884 File Offset: 0x001AEC84
	public void StartRunIn(bool sbOnLeft)
	{
		base.transform.position = new Vector3((float)((!sbOnLeft) ? (Level.Current.Left - 300) : (Level.Current.Right + 300)), base.transform.position.y);
		base.transform.localScale = new Vector3((float)((!sbOnLeft) ? -1 : 1), 1f);
		this.vel = ((float)(Level.Current.Left + Level.Current.Right) / 2f + 80f * base.transform.localScale.x - base.transform.position.x) / 1.0416666f;
		this.anim.Play("RunIn");
		this.moving = true;
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x001B0970 File Offset: 0x001AED70
	public void GetGrabbed()
	{
		this.moving = false;
	}

	// Token: 0x06002DD3 RID: 11731 RVA: 0x001B097C File Offset: 0x001AED7C
	public void StartRunOut()
	{
		this.anim.Play("RunOut");
		this.anim.Update(0f);
		this.SFX_SALTBAKER_P1_StrawberryBag_CryingRunOff();
		this.moving = true;
		this.vel *= 0.8f;
	}

	// Token: 0x06002DD4 RID: 11732 RVA: 0x001B09C8 File Offset: 0x001AEDC8
	private void Update()
	{
		if (this.moving)
		{
			base.transform.position += this.vel * Vector3.right * CupheadTime.Delta;
			if (Mathf.Abs(base.transform.position.x) > 2000f)
			{
				this.anim.StopPlayback();
				this.moving = false;
			}
		}
	}

	// Token: 0x06002DD5 RID: 11733 RVA: 0x001B0A4C File Offset: 0x001AEE4C
	private void LateUpdate()
	{
		this.rend.enabled = (this.saltbakerTopperRend.sprite == null || this.saltbaker.animator.GetCurrentAnimatorStateInfo(0).IsName("PhaseOneToTwo"));
	}

	// Token: 0x06002DD6 RID: 11734 RVA: 0x001B0A9B File Offset: 0x001AEE9B
	private void SFX_SALTBAKER_P1_StrawberryBag_CryingRunOff()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_strawberrybag_cryingrunoff");
	}

	// Token: 0x04003657 RID: 13911
	private const float GRAB_OFFSET = 80f;

	// Token: 0x04003658 RID: 13912
	[SerializeField]
	private Animator anim;

	// Token: 0x04003659 RID: 13913
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x0400365A RID: 13914
	[SerializeField]
	private SpriteRenderer saltbakerTopperRend;

	// Token: 0x0400365B RID: 13915
	[SerializeField]
	private SaltbakerLevelSaltbaker saltbaker;

	// Token: 0x0400365C RID: 13916
	private bool moving;

	// Token: 0x0400365D RID: 13917
	private float vel;
}
