using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200064A RID: 1610
public class FlyingCowboyLevelBeans : AbstractProjectile
{
	// Token: 0x06002115 RID: 8469 RVA: 0x00131E8C File Offset: 0x0013028C
	public virtual void Init(Vector3 position, bool pointingUp, float speed, float extendTimer)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		GameObject[] array = (!Rand.Bool()) ? this.versionB : this.versionA;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		if (!pointingUp)
		{
			base.animator.Play("BottomIdle");
			base.animator.Update(0f);
		}
		base.animator.Play(0, 0, UnityEngine.Random.Range(0f, 1f));
		base.StartCoroutine(this.move_cr(speed));
		base.StartCoroutine(this.extend_cr(extendTimer));
	}

	// Token: 0x06002116 RID: 8470 RVA: 0x00131F4D File Offset: 0x0013034D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002117 RID: 8471 RVA: 0x00131F6C File Offset: 0x0013036C
	private IEnumerator move_cr(float speed)
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.position += new Vector3(-speed * CupheadTime.FixedDelta, 0f);
			if (base.transform.position.x < -745f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002118 RID: 8472 RVA: 0x00131F90 File Offset: 0x00130390
	private IEnumerator extend_cr(float extendTimer)
	{
		yield return CupheadTime.WaitForSeconds(this, extendTimer);
		base.animator.SetTrigger("Extend");
		yield break;
	}

	// Token: 0x06002119 RID: 8473 RVA: 0x00131FB2 File Offset: 0x001303B2
	private void SFX_COWGIRL_P3_CanPropellerLoop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p3_canpropeller_loop", 0.4f, 0.5f);
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_canpropeller_loop");
	}

	// Token: 0x0600211A RID: 8474 RVA: 0x00131FD8 File Offset: 0x001303D8
	private void AnimationEvent_SFX_COWGIRL_P3_CanUnfurl()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_canpropeller_unfurl");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_canpropeller_unfurl");
	}

	// Token: 0x040029B3 RID: 10675
	[SerializeField]
	private GameObject[] versionA;

	// Token: 0x040029B4 RID: 10676
	[SerializeField]
	private GameObject[] versionB;
}
