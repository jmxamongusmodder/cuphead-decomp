using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000836 RID: 2102
public class TutorialPlayerDeathEffect : PlayerDeathEffect
{
	// Token: 0x060030BB RID: 12475 RVA: 0x001CB050 File Offset: 0x001C9450
	protected override void Awake()
	{
		base.Awake();
		this.tr = base.transform;
		this.startPos = this.tr.position;
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x001CB075 File Offset: 0x001C9475
	protected override void Start()
	{
		base.Start();
		this.Init();
	}

	// Token: 0x060030BD RID: 12477 RVA: 0x001CB084 File Offset: 0x001C9484
	private void Update()
	{
		if (this.tr.localPosition.y >= 270f)
		{
			this.tr.position = this.startPos;
		}
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x001CB0BF File Offset: 0x001C94BF
	protected override void OnParrySwitch()
	{
		base.OnParrySwitch();
		if (this.parrySwitch.enabled)
		{
			base.animator.SetTrigger("OnParryTutorial");
		}
		this.parrySwitch.enabled = false;
	}

	// Token: 0x060030BF RID: 12479 RVA: 0x001CB0F4 File Offset: 0x001C94F4
	private void Init()
	{
		this.tr.position = this.startPos;
		this.playerId = PlayerId.PlayerOne;
		base.animator.SetInteger("Mode", 0);
		base.animator.SetBool("CanParry", true);
		this.spriteRenderer = this.cuphead;
		this.cuphead.gameObject.SetActive(true);
		this.mugman.gameObject.SetActive(false);
		this.parrySwitch.enabled = true;
		this.parrySwitch.gameObject.SetActive(true);
	}

	// Token: 0x060030C0 RID: 12480 RVA: 0x001CB186 File Offset: 0x001C9586
	protected override void OnReviveParryAnimComplete()
	{
		this.StopAllCoroutines();
		base.animator.Play("Level_Start");
		this.exiting = false;
		this.Init();
		base.StartCoroutine(base.float_cr());
	}

	// Token: 0x060030C1 RID: 12481 RVA: 0x001CB1B8 File Offset: 0x001C95B8
	protected override IEnumerator checkOutOfFrame_cr()
	{
		yield return null;
		yield break;
	}

	// Token: 0x060030C2 RID: 12482 RVA: 0x001CB1CC File Offset: 0x001C95CC
	private void OnDestroy()
	{
		this.tr = null;
	}

	// Token: 0x0400395E RID: 14686
	protected Vector3 startPos;

	// Token: 0x0400395F RID: 14687
	private Transform tr;
}
