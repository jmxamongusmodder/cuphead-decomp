using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000408 RID: 1032
public class OutroCutscene : Cutscene
{
	// Token: 0x06000E62 RID: 3682 RVA: 0x00093278 File Offset: 0x00091678
	protected override void Start()
	{
		base.Start();
		this.book.SetActive(Localization.language == Localization.Languages.English);
		this.bookLocalized.SetActive(Localization.language != Localization.Languages.English);
		CreditsScreen.goodEnding = true;
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.skip_cr());
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x000932F0 File Offset: 0x000916F0
	private IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.33f);
		int numScreens = 6;
		for (int i = 0; i < numScreens; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.75f);
			this.arrowVisible = true;
			while (!this.input.GetAnyButtonDown())
			{
				yield return null;
			}
			this.arrowVisible = false;
			base.animator.SetTrigger("Continue");
			if (i != 5)
			{
				this.NextPageSFX();
			}
			if (i == 0)
			{
				this.FireWhooshSFX();
			}
			if (i == 4)
			{
				this.Cheering();
			}
		}
		CreditsScreen.goodEnding = true;
		yield return CupheadTime.WaitForSeconds(this, 6.25f);
		AudioManager.FadeBGMVolume(0f, 3f, true);
		yield return CupheadTime.WaitForSeconds(this, 3f);
		Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_credits, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None);
		yield break;
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x0009330C File Offset: 0x0009170C
	private IEnumerator skip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.input.GetButtonDown(CupheadButton.Pause))
			{
				Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_credits, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x00093328 File Offset: 0x00091728
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

	// Token: 0x06000E66 RID: 3686 RVA: 0x00093398 File Offset: 0x00091798
	private void NextPageSFX()
	{
		AudioManager.Play("ui_confirm");
		AudioManager.Play("ui_pageturn");
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x000933AE File Offset: 0x000917AE
	private void FireWhooshSFX()
	{
		AudioManager.Play("firewhoosh");
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x000933BA File Offset: 0x000917BA
	private void Cheering()
	{
		AudioManager.Play("cheering");
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x000933C6 File Offset: 0x000917C6
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.arrow = null;
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x000933D5 File Offset: 0x000917D5
	protected override void SetRichPresence()
	{
		OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Ending", true);
	}

	// Token: 0x040017A6 RID: 6054
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x040017A7 RID: 6055
	[SerializeField]
	private Image arrow;

	// Token: 0x040017A8 RID: 6056
	[SerializeField]
	private GameObject book;

	// Token: 0x040017A9 RID: 6057
	[SerializeField]
	private GameObject bookLocalized;

	// Token: 0x040017AA RID: 6058
	private float arrowTransparency;

	// Token: 0x040017AB RID: 6059
	private bool arrowVisible;
}
