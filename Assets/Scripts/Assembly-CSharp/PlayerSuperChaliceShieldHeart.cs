using System;
using UnityEngine;

// Token: 0x02000A57 RID: 2647
public class PlayerSuperChaliceShieldHeart : MonoBehaviour
{
	// Token: 0x06003F16 RID: 16150 RVA: 0x00228CE5 File Offset: 0x002270E5
	public void Destroy()
	{
		this.popped = true;
		this.animator.Play("HeartDie");
		AudioManager.Play("player_super_chalice_shield_end");
	}

	// Token: 0x06003F17 RID: 16151 RVA: 0x00228D08 File Offset: 0x00227108
	private void HeartDie()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003F18 RID: 16152 RVA: 0x00228D18 File Offset: 0x00227118
	private void FixedUpdate()
	{
		if (!this.popped && this.player != null)
		{
			this.offset = new Vector3(this.hoverWidth * Mathf.Cos(this.hoverTime) / (1f + Mathf.Sin(this.hoverTime) * Mathf.Sin(this.hoverTime)), this.hoverWidth * Mathf.Sin(this.hoverTime) * Mathf.Cos(this.hoverTime) / (1f + Mathf.Sin(this.hoverTime) * Mathf.Sin(this.hoverTime)));
			this.hoverTime += CupheadTime.FixedDelta * 2f;
			this.lerpSpeed = Mathf.Min(this.lerpSpeed + CupheadTime.FixedDelta, 3f);
			base.transform.position = Vector3.Lerp(base.transform.position, this.player.transform.position + this.offset, CupheadTime.FixedDelta * this.lerpSpeed);
			base.transform.localScale = new Vector3(this.player.transform.localScale.x, 1f);
		}
	}

	// Token: 0x0400462B RID: 17963
	[SerializeField]
	private Animator animator;

	// Token: 0x0400462C RID: 17964
	public Transform player;

	// Token: 0x0400462D RID: 17965
	private Vector3 offset;

	// Token: 0x0400462E RID: 17966
	private float hoverTime = 1.5707964f;

	// Token: 0x0400462F RID: 17967
	private float hoverWidth = 100f;

	// Token: 0x04004630 RID: 17968
	private bool popped;

	// Token: 0x04004631 RID: 17969
	private float lerpSpeed;
}
