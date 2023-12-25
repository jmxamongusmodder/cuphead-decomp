using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000796 RID: 1942
public class RumRunnersLevelMobIntroAnimation : MonoBehaviour
{
	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06002B16 RID: 11030 RVA: 0x0019228B File Offset: 0x0019068B
	// (set) Token: 0x06002B17 RID: 11031 RVA: 0x00192293 File Offset: 0x00190693
	public float bugGirlDamage { get; set; }

	// Token: 0x06002B18 RID: 11032 RVA: 0x0019229C File Offset: 0x0019069C
	private void Start()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.grub.SetActive(false);
		}
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x001922BC File Offset: 0x001906BC
	private IEnumerator bugWalk()
	{
		float walkSpeed = this.bugGirlWalkDistance / this.bugGirlWalkDuration;
		for (;;)
		{
			yield return null;
			this.bugGirlTransform.position = this.bugGirlTransform.position + new Vector3(walkSpeed * CupheadTime.Delta, 0f);
		}
		yield break;
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x001922D8 File Offset: 0x001906D8
	private IEnumerator timeout_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, RumRunnersLevelMobIntroAnimation.IntroTimeoutDuration);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06002B1B RID: 11035 RVA: 0x001922F3 File Offset: 0x001906F3
	public void StartBugWalk()
	{
		this.bugWalkCoroutine = base.StartCoroutine(this.bugWalk());
	}

	// Token: 0x06002B1C RID: 11036 RVA: 0x00192307 File Offset: 0x00190707
	public void StopBugWalk()
	{
		base.StopCoroutine(this.bugWalkCoroutine);
	}

	// Token: 0x06002B1D RID: 11037 RVA: 0x00192318 File Offset: 0x00190718
	public void BarrelExit()
	{
		this.barrelAnimator.SetTrigger("Exit");
		SpriteRenderer component = this.barrelAnimator.GetComponent<SpriteRenderer>();
		component.sortingLayerName = "Foreground";
		component.sortingOrder = 100;
		base.StartCoroutine(this.timeout_cr());
	}

	// Token: 0x040033D4 RID: 13268
	private static readonly float IntroTimeoutDuration = 2f;

	// Token: 0x040033D5 RID: 13269
	[SerializeField]
	private Transform bugGirlTransform;

	// Token: 0x040033D6 RID: 13270
	[SerializeField]
	private float bugGirlWalkDistance;

	// Token: 0x040033D7 RID: 13271
	[SerializeField]
	private float bugGirlWalkDuration;

	// Token: 0x040033D8 RID: 13272
	[SerializeField]
	private Animator barrelAnimator;

	// Token: 0x040033D9 RID: 13273
	[SerializeField]
	private GameObject grub;

	// Token: 0x040033DB RID: 13275
	private Coroutine bugWalkCoroutine;
}
