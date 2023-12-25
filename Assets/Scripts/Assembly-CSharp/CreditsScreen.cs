using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003F3 RID: 1011
public class CreditsScreen : AbstractMonoBehaviour
{
	// Token: 0x06000DB4 RID: 3508 RVA: 0x0008EF51 File Offset: 0x0008D351
	private void Start()
	{
		this.Init(false);
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x0008EF5C File Offset: 0x0008D35C
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.verticalLayoutGroup = this.content.GetComponent<VerticalLayoutGroup>();
		base.StartCoroutine(this.credits_cr());
		base.StartCoroutine(this.skip_cr());
		base.StartCoroutine(this.fastForward_cr());
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x0008EFB0 File Offset: 0x0008D3B0
	private IEnumerator credits_cr()
	{
		float wait = this.introDuration;
		while (wait > 0f)
		{
			wait -= CupheadTime.Delta * this.timeMultiplier;
			yield return null;
		}
		float accumulator = 0f;
		while (this.content.anchoredPosition.y < this.verticalLayoutGroup.preferredHeight - base.rectTransform.sizeDelta.y)
		{
			accumulator += CupheadTime.Delta * this.timeMultiplier;
			while (accumulator > 0.041666668f)
			{
				accumulator -= 0.041666668f;
				this.content.anchoredPosition = new Vector2(0f, this.content.anchoredPosition.y + this.scrollSpeed * 0.041666668f);
			}
			yield return null;
		}
		this.doneScrolling = true;
		wait = this.outroDuration;
		while (wait > 0f)
		{
			wait -= CupheadTime.Delta * this.timeMultiplier;
			yield return null;
		}
		PlayerManager.ResetPlayers();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
		yield break;
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x0008EFCC File Offset: 0x0008D3CC
	private IEnumerator fastForward_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.input.GetAnyButtonHeld() && !this.input.GetButtonDown(CupheadButton.Pause) && !this.doneScrolling)
			{
				if (this.timeMultiplier == 1f)
				{
					this.timeMultiplier = 8f;
					AudioManager.ChangeBGMPitch(8f, 0.125f);
				}
			}
			else if (this.timeMultiplier > 1f)
			{
				this.timeMultiplier = 1f;
				AudioManager.ChangeBGMPitch(1f, 0.125f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x0008EFE8 File Offset: 0x0008D3E8
	private IEnumerator skip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.input.GetButtonDown(CupheadButton.Pause))
			{
				PlayerManager.ResetPlayers();
				SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0008F003 File Offset: 0x0008D403
	private void LateUpdate()
	{
		if (CupheadMapCamera.Current == null)
		{
			return;
		}
		base.transform.position = CupheadMapCamera.Current.transform.position;
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0008F030 File Offset: 0x0008D430
	protected bool GetButtonDown(CupheadButton button)
	{
		return this.input.GetButtonDown(button);
	}

	// Token: 0x04001724 RID: 5924
	public static bool goodEnding = true;

	// Token: 0x04001725 RID: 5925
	[SerializeField]
	private RectTransform content;

	// Token: 0x04001726 RID: 5926
	private VerticalLayoutGroup verticalLayoutGroup;

	// Token: 0x04001727 RID: 5927
	[SerializeField]
	private float introDuration;

	// Token: 0x04001728 RID: 5928
	[SerializeField]
	private float scrollSpeed;

	// Token: 0x04001729 RID: 5929
	[SerializeField]
	private float outroDuration;

	// Token: 0x0400172A RID: 5930
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x0400172B RID: 5931
	private bool doneScrolling;

	// Token: 0x0400172C RID: 5932
	private float timeMultiplier = 1f;
}
