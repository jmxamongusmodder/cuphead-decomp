using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007D2 RID: 2002
public class SaltbakerLevelPhaseThreeToFourTransition : MonoBehaviour
{
	// Token: 0x06002D71 RID: 11633 RVA: 0x001ACA7B File Offset: 0x001AAE7B
	public void StartSaltman()
	{
		this.anim.Play("Start");
	}

	// Token: 0x06002D72 RID: 11634 RVA: 0x001ACA8D File Offset: 0x001AAE8D
	public void StartHeart()
	{
		base.StartCoroutine(this.move_heart_cr());
		this.anim.Play("Heart", 1, 0f);
	}

	// Token: 0x06002D73 RID: 11635 RVA: 0x001ACAB4 File Offset: 0x001AAEB4
	private IEnumerator move_heart_cr()
	{
		yield return this.anim.WaitForAnimationToStart(this, "HeartLoop", 1, false);
		Vector3 start = this.heart.transform.position;
		Vector3 end = start + Vector3.up * 300f;
		for (float t = 0f; t < 1f; t += 0.083333336f)
		{
			this.heart.transform.position = Vector3.Lerp(start, end, EaseUtils.EaseInSine(0f, 1f, t));
			yield return CupheadTime.WaitForSeconds(this, 0.083333336f);
		}
		base.enabled = false;
		yield break;
	}

	// Token: 0x06002D74 RID: 11636 RVA: 0x001ACACF File Offset: 0x001AAECF
	private void AnimationEvent_SFX_SALTB_Phase3to4_HeartRise()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3top4transition_heartrise");
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x001ACADB File Offset: 0x001AAEDB
	private void AnimationEvent_SFX_SALTB_Phase3to4_Transition()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3top4transition");
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x001ACAE7 File Offset: 0x001AAEE7
	private void AnimationEvent_SFX_SALTB_Phase3to4_TransitionStart()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3top4transition_start");
	}

	// Token: 0x040035F7 RID: 13815
	[SerializeField]
	private Animator anim;

	// Token: 0x040035F8 RID: 13816
	[SerializeField]
	private GameObject heart;
}
