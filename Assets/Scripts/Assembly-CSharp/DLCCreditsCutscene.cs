using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003FA RID: 1018
public class DLCCreditsCutscene : Cutscene
{
	// Token: 0x06000DEB RID: 3563 RVA: 0x0009075A File Offset: 0x0008EB5A
	protected override void Start()
	{
		base.Start();
		CutsceneGUI.Current.pause.pauseAllowed = false;
		this.input = new CupheadInput.AnyPlayerInput(false);
		base.StartCoroutine(this.credits_cr());
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0009078C File Offset: 0x0008EB8C
	private void Update()
	{
		if (this.canSkip)
		{
			if (this.input.GetButtonDown(CupheadButton.Pause))
			{
				this.canSkip = false;
				this.StopAllCoroutines();
				this.goToNext();
				return;
			}
			if (this.input.GetAnyButtonHeld() && !this.input.GetButtonDown(CupheadButton.Pause))
			{
				if (this.multiplier == 1f)
				{
					this.multiplier = 8f;
					AudioManager.ChangeBGMPitch(8f, 0.125f);
				}
			}
			else if (this.multiplier > 1f)
			{
				this.multiplier = 1f;
				AudioManager.ChangeBGMPitch(1f, 0.125f);
			}
		}
		else if (this.multiplier > 1f)
		{
			this.multiplier = 1f;
			AudioManager.ChangeBGMPitch(1f, 0.125f);
		}
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x00090874 File Offset: 0x0008EC74
	private IEnumerator credits_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		AudioManager.PlayBGM();
		this.canSkip = true;
		float preferredHeight = this.contentTransform.GetComponent<VerticalLayoutGroup>().preferredHeight;
		float speed = preferredHeight / this.scrollDuration;
		float elapsedTime = 0f;
		float accumulator = 0f;
		while (elapsedTime < this.scrollDuration)
		{
			yield return null;
			for (accumulator += CupheadTime.Delta * this.multiplier; accumulator > 0.041666668f; accumulator -= 0.041666668f)
			{
				elapsedTime += 0.041666668f;
			}
			Vector2 position = this.contentTransform.anchoredPosition;
			position.y = Mathf.Lerp(0f, preferredHeight - 720f, elapsedTime / this.scrollDuration);
			this.contentTransform.anchoredPosition = position;
		}
		this.canSkip = false;
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.goToNext();
		yield break;
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x0009088F File Offset: 0x0008EC8F
	private void goToNext()
	{
		PlayerManager.ResetPlayers();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
	}

	// Token: 0x0400174D RID: 5965
	[SerializeField]
	private float scrollDuration;

	// Token: 0x0400174E RID: 5966
	[SerializeField]
	private RectTransform contentTransform;

	// Token: 0x0400174F RID: 5967
	[SerializeField]
	private float memphisFontSize;

	// Token: 0x04001750 RID: 5968
	[SerializeField]
	private float vogueBoldFontSize;

	// Token: 0x04001751 RID: 5969
	[SerializeField]
	private float vogueExtraBoldFontSize;

	// Token: 0x04001752 RID: 5970
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04001753 RID: 5971
	private bool canSkip;

	// Token: 0x04001754 RID: 5972
	private float multiplier = 1f;
}
