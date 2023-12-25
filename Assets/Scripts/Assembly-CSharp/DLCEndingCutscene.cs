using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003FC RID: 1020
public class DLCEndingCutscene : DLCGenericCutscene
{
	// Token: 0x06000E09 RID: 3593 RVA: 0x000912F0 File Offset: 0x0008F6F0
	protected override void Start()
	{
		base.Start();
		SceneLoader.OnLoaderCompleteEvent += this.StartMusic;
		if (this.trappedChar == DLCGenericCutscene.TrappedChar.None)
		{
			this.trappedChar = base.DetectCharacter();
		}
		this.shot1Animator.SetBool("NeedToSwap", this.trappedChar != DLCGenericCutscene.TrappedChar.Chalice);
		this.rightCuphead.SetActive(false);
		this.ghostBodyChalice.SetActive(this.trappedChar == DLCGenericCutscene.TrappedChar.Chalice);
		this.ghostBodyCHMM.SetActive(this.trappedChar != DLCGenericCutscene.TrappedChar.Chalice);
		DLCGenericCutscene.TrappedChar trappedChar = this.trappedChar;
		if (trappedChar != DLCGenericCutscene.TrappedChar.Chalice)
		{
			if (trappedChar != DLCGenericCutscene.TrappedChar.Mugman)
			{
				if (trappedChar == DLCGenericCutscene.TrappedChar.Cuphead)
				{
					this.leftCuphead.SetActive(false);
					this.rightMugman.SetActive(false);
					this.rightCuphead.SetActive(false);
					this.trappedChalice.SetActive(false);
					this.trappedMugman.SetActive(false);
					this.text[0] = this.altText[1];
				}
			}
			else
			{
				this.leftMugman.SetActive(false);
				this.rightMugman.SetActive(false);
				this.trappedChalice.SetActive(false);
				this.trappedCuphead.SetActive(false);
				this.text[0] = this.altText[0];
			}
		}
		else
		{
			this.leftMugman.SetActive(false);
			this.rightChalice.SetActive(false);
			this.trappedMugman.SetActive(false);
			this.trappedCuphead.SetActive(false);
		}
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x0009146A File Offset: 0x0008F86A
	private void StartMusic()
	{
		base.StartCoroutine(this.handle_music_cr());
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x0009147C File Offset: 0x0008F87C
	private IEnumerator handle_music_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.StartBGMAlternate(0);
		yield return base.StartCoroutine(this.hold_for_music_advance_and_loop(5.5f, string.Empty));
		AudioManager.StartBGMAlternate(1);
		yield return base.StartCoroutine(this.hold_for_music_advance_and_loop(3f, string.Empty));
		AudioManager.StartBGMAlternate(2);
		yield break;
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x00091498 File Offset: 0x0008F898
	private IEnumerator hold_for_music_advance_and_loop(float time, string loopName)
	{
		float t = 0f;
		this.advanceMusic = false;
		while (t < time && !this.advanceMusic)
		{
			t += Time.deltaTime;
			yield return null;
		}
		if (!this.advanceMusic)
		{
			AudioManager.PlayLoop(loopName);
		}
		while (!this.advanceMusic)
		{
			yield return null;
		}
		AudioManager.Stop(loopName);
		yield break;
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x000914C4 File Offset: 0x0008F8C4
	private IEnumerator crossfade_final_music_cr()
	{
		AudioManager.FadeBGMVolume(0f, 1.5f, true);
		AudioManager.FadeSFXVolume("mus_dlc_ending_4", 0.0001f, 0.0001f);
		yield return null;
		AudioManager.Play("mus_dlc_ending_4");
		AudioManager.FadeSFXVolume("mus_dlc_ending_4", 0.4f, 1.5f);
		yield break;
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x000914D8 File Offset: 0x0008F8D8
	public void AdvanceMusic()
	{
		this.advanceMusic = true;
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x000914E4 File Offset: 0x0008F8E4
	public void SwapChars()
	{
		this.rightChalice.SetActive(false);
		this.trappedChalice.SetActive(true);
		this.ghostBodyChalice.SetActive(true);
		this.ghostBodyCHMM.SetActive(false);
		DLCGenericCutscene.TrappedChar trappedChar = this.trappedChar;
		if (trappedChar != DLCGenericCutscene.TrappedChar.Mugman)
		{
			if (trappedChar == DLCGenericCutscene.TrappedChar.Cuphead)
			{
				this.rightCuphead.SetActive(true);
				this.trappedCuphead.SetActive(false);
			}
		}
		else
		{
			this.rightMugman.SetActive(true);
			this.trappedMugman.SetActive(false);
		}
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x00091574 File Offset: 0x0008F974
	public override void IrisOut()
	{
		SceneLoader.LoadScene(Scenes.scene_cutscene_dlc_credits_comic, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.None, null);
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x00091581 File Offset: 0x0008F981
	public void StartShake()
	{
		this.screenShakeAmt = 4f;
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x0009158E File Offset: 0x0008F98E
	public void StopShake()
	{
		this.screenShakeAmt = 0f;
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x0009159C File Offset: 0x0008F99C
	private void LateUpdate()
	{
		float x = UnityEngine.Random.Range(-this.screenShakeAmt, this.screenShakeAmt);
		float y = UnityEngine.Random.Range(-this.screenShakeAmt, this.screenShakeAmt);
		this.screens[this.curScreen].transform.localPosition = new Vector3(x, y, 0f);
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x000915F4 File Offset: 0x0008F9F4
	protected override void OnScreenAdvance(int which)
	{
		if (which == 3)
		{
			base.StartCoroutine(this.crossfade_final_music_cr());
			GameObject gameObject = GameObject.Find("Fader");
			if (gameObject != null)
			{
				Animator component = gameObject.GetComponent<Animator>();
				if (component != null)
				{
					component.Play("Transparent");
				}
			}
		}
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x0009164A File Offset: 0x0008FA4A
	protected override void OnDestroy()
	{
		SceneLoader.OnLoaderCompleteEvent -= this.StartMusic;
		base.OnDestroy();
	}

	// Token: 0x04001756 RID: 5974
	[SerializeField]
	private DLCGenericCutscene.TrappedChar trappedChar;

	// Token: 0x04001757 RID: 5975
	[SerializeField]
	private GameObject leftCuphead;

	// Token: 0x04001758 RID: 5976
	[SerializeField]
	private GameObject leftMugman;

	// Token: 0x04001759 RID: 5977
	[SerializeField]
	private GameObject rightMugman;

	// Token: 0x0400175A RID: 5978
	[SerializeField]
	private GameObject rightChalice;

	// Token: 0x0400175B RID: 5979
	[SerializeField]
	private GameObject rightCuphead;

	// Token: 0x0400175C RID: 5980
	[SerializeField]
	private GameObject trappedChalice;

	// Token: 0x0400175D RID: 5981
	[SerializeField]
	private GameObject trappedMugman;

	// Token: 0x0400175E RID: 5982
	[SerializeField]
	private GameObject trappedCuphead;

	// Token: 0x0400175F RID: 5983
	[SerializeField]
	private GameObject ghostBodyChalice;

	// Token: 0x04001760 RID: 5984
	[SerializeField]
	private GameObject ghostBodyCHMM;

	// Token: 0x04001761 RID: 5985
	[SerializeField]
	private Animator shot1Animator;

	// Token: 0x04001762 RID: 5986
	[SerializeField]
	private GameObject[] altText;

	// Token: 0x04001763 RID: 5987
	private float screenShakeAmt;

	// Token: 0x04001764 RID: 5988
	private bool advanceMusic;

	// Token: 0x04001765 RID: 5989
	[SerializeField]
	[Range(-1f, 3f)]
	private int fastForward = -1;
}
