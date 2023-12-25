using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000406 RID: 1030
public class IntroCutscene : Cutscene
{
	// Token: 0x06000E51 RID: 3665 RVA: 0x0009299C File Offset: 0x00090D9C
	protected override void Start()
	{
		base.Start();
		this.book.SetActive(Localization.language == Localization.Languages.English);
		this.bookLocalized.SetActive(Localization.language != Localization.Languages.English);
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.skip_cr());
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x00092A10 File Offset: 0x00090E10
	private IEnumerator main_cr()
	{
		int numScreens = 11;
		yield return CupheadTime.WaitForSeconds(this, 6f);
		for (int i = 0; i < numScreens; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.75f);
			this.arrowVisible = true;
			while (this.input.GetButtonDown(CupheadButton.Pause) || !this.input.GetAnyButtonDown())
			{
				yield return null;
			}
			this.arrowVisible = false;
			base.animator.SetTrigger("Continue");
			if (i != numScreens - 1)
			{
				this.NextPageSFX();
			}
			if (i == 2)
			{
				this.DevilLaugh();
			}
			if (i == 4)
			{
				this.DiceRoll();
			}
			if (i == 5)
			{
				this.DevilSlam();
			}
			if (i == 8)
			{
				this.DevilKick();
			}
		}
		AudioManager.FadeBGMVolume(0f, 0.75f, true);
		yield return CupheadTime.WaitForSeconds(this, 0.75f);
		SceneLoader.LoadLevel(Levels.House, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x00092A2C File Offset: 0x00090E2C
	private IEnumerator skip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.input.GetButtonDown(CupheadButton.Pause))
			{
				SceneLoader.LoadLevel(Levels.House, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000E54 RID: 3668 RVA: 0x00092A48 File Offset: 0x00090E48
	private void Update()
	{
		if (this.arrowVisible)
		{
			this.arrowTransparency = Mathf.Clamp01(this.arrowTransparency + Time.deltaTime / 0.25f);
		}
		else
		{
			this.arrowTransparency = 0f;
		}
		this.arrow.color = new Color(1f, 1f, 1f, this.arrowTransparency * 0.35f);
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x00092AB8 File Offset: 0x00090EB8
	private void NextPageSFX()
	{
		AudioManager.Play("ui_confirm");
		AudioManager.Play("ui_pageturn");
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x00092ACE File Offset: 0x00090ECE
	private void DevilLaugh()
	{
		AudioManager.Play("devil_laugh");
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x00092ADA File Offset: 0x00090EDA
	private void DiceRoll()
	{
		AudioManager.Play("dice_roll");
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x00092AE6 File Offset: 0x00090EE6
	private void DevilSlam()
	{
		AudioManager.Play("devil_slam");
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x00092AF2 File Offset: 0x00090EF2
	private void DevilKick()
	{
		AudioManager.Play("devil_kick");
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x00092AFE File Offset: 0x00090EFE
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.arrow = null;
	}

	// Token: 0x0400179C RID: 6044
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x0400179D RID: 6045
	[SerializeField]
	private Image arrow;

	// Token: 0x0400179E RID: 6046
	[SerializeField]
	private GameObject book;

	// Token: 0x0400179F RID: 6047
	[SerializeField]
	private GameObject bookLocalized;

	// Token: 0x040017A0 RID: 6048
	private float arrowTransparency;

	// Token: 0x040017A1 RID: 6049
	private bool arrowVisible;
}
