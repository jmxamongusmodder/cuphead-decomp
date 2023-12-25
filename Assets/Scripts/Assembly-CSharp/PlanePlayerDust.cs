using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A9B RID: 2715
public class PlanePlayerDust : AbstractMonoBehaviour
{
	// Token: 0x06004112 RID: 16658 RVA: 0x002359B3 File Offset: 0x00233DB3
	public void Initialize(AbstractPlayerController playerController, float smallY, float bigY)
	{
		this.playerController = (PlanePlayerController)playerController;
		this.smallY = smallY;
		this.bigY = bigY;
		if (playerController != null)
		{
			base.StartCoroutine(this.setupSorting_cr());
		}
	}

	// Token: 0x06004113 RID: 16659 RVA: 0x002359E8 File Offset: 0x00233DE8
	private IEnumerator setupSorting_cr()
	{
		while (this.playerController.animationController.spriteRenderer == null)
		{
			yield return null;
		}
		int playerOrder = this.playerController.animationController.spriteRenderer.sortingOrder;
		this.shadowRenderer.sortingOrder += playerOrder;
		this.backRenderer.sortingOrder += playerOrder;
		this.frontRenderer.sortingOrder += playerOrder;
		yield break;
	}

	// Token: 0x06004114 RID: 16660 RVA: 0x00235A04 File Offset: 0x00233E04
	private void Update()
	{
		if (this.playerController == null)
		{
			return;
		}
		if (this.playerController.IsDead)
		{
			base.animator.SetInteger(PlanePlayerDust.SizeParameter, 0);
			base.animator.SetBool(PlanePlayerDust.ShadowLoopParameter, false);
			this.shadowRenderer.enabled = false;
			return;
		}
		float bottom = this.playerController.bottom;
		if (bottom < this.bigY)
		{
			base.animator.SetInteger(PlanePlayerDust.SizeParameter, 2);
			base.animator.SetBool(PlanePlayerDust.ShadowLoopParameter, true);
		}
		else if (bottom < this.smallY)
		{
			base.animator.SetInteger(PlanePlayerDust.SizeParameter, 1);
			this.setManualShadow(bottom);
		}
		else
		{
			base.animator.SetInteger(PlanePlayerDust.SizeParameter, 0);
			this.setManualShadow(bottom);
		}
		base.transform.position = new Vector3(this.playerController.center.x, this.bigY) + PlanePlayerDust.PositionOffset;
	}

	// Token: 0x06004115 RID: 16661 RVA: 0x00235B18 File Offset: 0x00233F18
	private void setManualShadow(float bottom)
	{
		base.animator.SetBool(PlanePlayerDust.ShadowLoopParameter, false);
		if (bottom >= this.smallY)
		{
			this.shadowRenderer.enabled = false;
		}
		else
		{
			this.shadowRenderer.enabled = true;
			float num = MathUtilities.LerpMapping(bottom, this.smallY, this.bigY, 0f, (float)PlanePlayerDust.ManualShadowSpriteCount, true);
			base.animator.Play(PlanePlayerDust.ManualState, 2, num / (float)PlanePlayerDust.ManualShadowSpriteCount);
		}
	}

	// Token: 0x040047AC RID: 18348
	private static readonly int SizeParameter = Animator.StringToHash("Size");

	// Token: 0x040047AD RID: 18349
	private static readonly int ShadowLoopParameter = Animator.StringToHash("ShadowLoop");

	// Token: 0x040047AE RID: 18350
	private static readonly int ManualState = Animator.StringToHash("Manual");

	// Token: 0x040047AF RID: 18351
	private static readonly Vector3 PositionOffset = new Vector3(-70f, -20f);

	// Token: 0x040047B0 RID: 18352
	private static readonly int ManualShadowSpriteCount = 7;

	// Token: 0x040047B1 RID: 18353
	[SerializeField]
	private SpriteRenderer shadowRenderer;

	// Token: 0x040047B2 RID: 18354
	[SerializeField]
	private SpriteRenderer backRenderer;

	// Token: 0x040047B3 RID: 18355
	[SerializeField]
	private SpriteRenderer frontRenderer;

	// Token: 0x040047B4 RID: 18356
	private float smallY;

	// Token: 0x040047B5 RID: 18357
	private float bigY;

	// Token: 0x040047B6 RID: 18358
	private PlanePlayerController playerController;
}
