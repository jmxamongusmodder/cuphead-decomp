using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000402 RID: 1026
public class DLCIntroCutscene : DLCGenericCutscene
{
	// Token: 0x06000E3C RID: 3644 RVA: 0x00091BDE File Offset: 0x0008FFDE
	protected override void Start()
	{
		base.Start();
		this.allowScreenSkip = true;
		this.BGanim.speed = this.screen4BGScrollSpeed;
		this.screen4ForestScrollSpeed = this.screen4ForestScrollStartSpeed;
		AudioManager.PlayLoop("sfx_dlc_intro_oceanamb_loop");
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x00091C14 File Offset: 0x00090014
	protected override void OnScreenSkip()
	{
		base.StartCoroutine(this.skip_title_cr());
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x00091C24 File Offset: 0x00090024
	private IEnumerator skip_title_cr()
	{
		this.allowScreenSkip = false;
		base.IrisIn();
		yield return CupheadTime.WaitForSeconds(this, 0.9f);
		this.screens[this.curScreen].Play("End");
		yield break;
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x00091C40 File Offset: 0x00090040
	protected override void OnScreenAdvance(int which)
	{
		if (which == 0)
		{
			this.canvas.SetActive(true);
			AudioManager.StartBGMAlternate(0);
			AudioManager.Stop("sfx_dlc_intro_oceanamb_loop");
		}
		if (which < this.astralPlanePositions.Length && this.astralPlanePositions[which])
		{
			this.astralPlaneController.position = this.astralPlanePositions[which].position;
			this.astralPlaneController.localScale = this.astralPlanePositions[which].localScale;
		}
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x00091CC0 File Offset: 0x000900C0
	protected override void OnContinue()
	{
		this.allowScreenSkip = false;
		if (this.curScreen == 3 && !this.BGanim.GetBool("End"))
		{
			this.BGanim.SetBool("End", true);
			base.StartCoroutine(this.slow_down_bg_cr());
		}
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x00091D14 File Offset: 0x00090114
	private IEnumerator slow_down_bg_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float end = this.screen4BGScrollSpeed / 2f;
		while (!this.BGanim.GetCurrentAnimatorStateInfo(0).IsName("End") && !this.BGanim.GetCurrentAnimatorStateInfo(0).IsName("AltEnd"))
		{
			yield return null;
		}
		foreach (GameObject gameObject in this.screen4Characters)
		{
			gameObject.transform.parent = this.screen4ScrollEnd.transform;
		}
		while (this.BGanim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			this.BGanim.speed = EaseUtils.EaseOutSine(this.screen4BGScrollSpeed, end, this.BGanim.GetCurrentAnimatorStateInfo(0).normalizedTime);
			this.screen4ForestScrollSpeed = this.screen4ForestScrollStartSpeed * (this.BGanim.speed / this.screen4BGScrollSpeed);
			foreach (GameObject gameObject2 in this.screen4Characters)
			{
				gameObject2.transform.localPosition += Vector3.right * 6.4f;
			}
			yield return wait;
		}
		this.screen4ForestScrollSpeed = 0f;
		yield return this.screens[this.curScreen].WaitForAnimationToStart(this, "holdforBG", false);
		this.screens[this.curScreen].SetTrigger("Continue");
		while (this.screen4Characters[2].transform.localPosition.x < 1420f)
		{
			foreach (GameObject gameObject3 in this.screen4Characters)
			{
				gameObject3.transform.localPosition += Vector3.right * 6.4f;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x00091D30 File Offset: 0x00090130
	protected override void Update()
	{
		base.Update();
		if (this.curScreen == 0)
		{
			CupheadCutsceneCamera.Current.SetPosition(this.cameraPos.transform.position);
		}
		else
		{
			CupheadCutsceneCamera.Current.SetPosition(Vector3.zero);
		}
		if (this.curScreen == 3)
		{
			this.screen4Forest.transform.localPosition += Vector3.left * this.screen4ForestScrollSpeed * CupheadTime.Delta;
			this.screen4Clouds.transform.localPosition += Vector3.left * this.screen4ForestScrollSpeed * CupheadTime.Delta * 0.5f;
			this.screen4FG.transform.localPosition += Vector3.left * this.screen4ForestScrollSpeed * CupheadTime.Delta * 1.5f;
		}
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x00091E4C File Offset: 0x0009024C
	private void LateUpdate()
	{
		if (this.screen4Forest.transform.localPosition.x < -2560f)
		{
			this.screen4Forest.transform.localPosition += Vector3.right * 1280f;
		}
		if (this.screen4Clouds.transform.localPosition.x < -2560f)
		{
			this.screen4Clouds.transform.localPosition += Vector3.right * 1280f;
		}
		if (this.screen4FG.transform.localPosition.x < -5156f)
		{
			this.screen4FG.transform.localPosition += Vector3.right * 4767f;
		}
		if (this.screen4ScrollStart.transform.position.x < -1600f)
		{
			this.screen4ScrollStart.enabled = false;
			this.screen4EndLoopBack.enabled = true;
		}
	}

	// Token: 0x0400177D RID: 6013
	[SerializeField]
	private GameObject canvas;

	// Token: 0x0400177E RID: 6014
	[SerializeField]
	private GameObject cameraPos;

	// Token: 0x0400177F RID: 6015
	[SerializeField]
	private Animator BGanim;

	// Token: 0x04001780 RID: 6016
	[SerializeField]
	private Transform astralPlaneController;

	// Token: 0x04001781 RID: 6017
	[SerializeField]
	private Transform[] astralPlanePositions;

	// Token: 0x04001782 RID: 6018
	[SerializeField]
	private float screen4BGScrollSpeed = 0.1f;

	// Token: 0x04001783 RID: 6019
	[SerializeField]
	private float screen4ForestScrollStartSpeed = 325f;

	// Token: 0x04001784 RID: 6020
	private float screen4ForestScrollSpeed;

	// Token: 0x04001785 RID: 6021
	[SerializeField]
	private GameObject[] screen4Characters;

	// Token: 0x04001786 RID: 6022
	[SerializeField]
	private GameObject screen4Forest;

	// Token: 0x04001787 RID: 6023
	[SerializeField]
	private GameObject screen4Clouds;

	// Token: 0x04001788 RID: 6024
	[SerializeField]
	private GameObject screen4FG;

	// Token: 0x04001789 RID: 6025
	[SerializeField]
	private GameObject screen4ScrollEnd;

	// Token: 0x0400178A RID: 6026
	[SerializeField]
	private SpriteRenderer screen4ScrollStart;

	// Token: 0x0400178B RID: 6027
	[SerializeField]
	private SpriteRenderer screen4EndLoopBack;

	// Token: 0x0400178C RID: 6028
	[SerializeField]
	[Range(-1f, 7f)]
	private int fastForward = -1;
}
