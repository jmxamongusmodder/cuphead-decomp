using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
public class DLCCreditsComicCutscene : Cutscene
{
	// Token: 0x06000DE5 RID: 3557 RVA: 0x000903F9 File Offset: 0x0008E7F9
	protected override void Start()
	{
		base.Start();
		CutsceneGUI.Current.pause.pauseAllowed = false;
		this.input = new CupheadInput.AnyPlayerInput(false);
		base.StartCoroutine(this.credits_cr());
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x0009042C File Offset: 0x0008E82C
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

	// Token: 0x06000DE7 RID: 3559 RVA: 0x00090514 File Offset: 0x0008E914
	private IEnumerator credits_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.canSkip = true;
		AudioManager.PlayBGM();
		float distance = this.panels.GetLast<SpriteRenderer>().transform.position.x - this.panels[0].transform.position.x - DLCCreditsComicCutscene.EndingAdjustment;
		float elapsedTime = 0f;
		while (elapsedTime < DLCCreditsComicCutscene.ScrollDuration)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta * this.multiplier;
			Vector3 position = this.parentTransform.position;
			position.x = Mathf.Lerp(0f, -distance, elapsedTime / DLCCreditsComicCutscene.ScrollDuration);
			this.parentTransform.position = position;
		}
		yield return CupheadTime.WaitForSeconds(this, 5f);
		this.canSkip = false;
		this.goToNext();
		yield break;
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x0009052F File Offset: 0x0008E92F
	private void goToNext()
	{
		SceneLoader.LoadScene(Scenes.scene_cutscene_dlc_credits, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
	}

	// Token: 0x04001745 RID: 5957
	private static readonly float AdjustmentAmount = -1f;

	// Token: 0x04001746 RID: 5958
	private static readonly float EndingAdjustment = 15f;

	// Token: 0x04001747 RID: 5959
	private static readonly float ScrollDuration = 90.8f;

	// Token: 0x04001748 RID: 5960
	[SerializeField]
	private Transform parentTransform;

	// Token: 0x04001749 RID: 5961
	[SerializeField]
	private SpriteRenderer[] panels;

	// Token: 0x0400174A RID: 5962
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x0400174B RID: 5963
	private bool canSkip;

	// Token: 0x0400174C RID: 5964
	private float multiplier = 1f;
}
