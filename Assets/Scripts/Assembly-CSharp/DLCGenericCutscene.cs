using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003FE RID: 1022
public class DLCGenericCutscene : Cutscene
{
	// Token: 0x06000E2B RID: 3627 RVA: 0x00090C2A File Offset: 0x0008F02A
	protected override void Start()
	{
		base.Start();
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.skip_cr());
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x00090C68 File Offset: 0x0008F068
	protected virtual void Update()
	{
		if (this.arrowVisible)
		{
			this.arrowTransparency = Mathf.Clamp01(this.arrowTransparency + Time.deltaTime / 0.25f);
		}
		else
		{
			this.arrowTransparency = 0f;
		}
		this.arrow.color = new Color(1f, 1f, 1f, this.arrowTransparency);
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x00090CD2 File Offset: 0x0008F0D2
	protected virtual void OnScreenAdvance(int which)
	{
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x00090CD4 File Offset: 0x0008F0D4
	protected virtual void OnContinue()
	{
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x00090CD6 File Offset: 0x0008F0D6
	protected virtual void OnScreenSkip()
	{
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x00090CD8 File Offset: 0x0008F0D8
	private IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.mainDelay);
		this.curScreen = 0;
		while (this.curScreen < this.screens.Length)
		{
			this.screens[this.curScreen].gameObject.SetActive(true);
			int target = Animator.StringToHash(this.screens[this.curScreen].GetLayerName(0) + ".End");
			while (this.screens[this.curScreen].GetCurrentAnimatorStateInfo(0).fullPathHash != target)
			{
				yield return null;
				if (this.arrowVisible)
				{
					while ((this.input.GetButtonDown(CupheadButton.Pause) || !this.input.GetAnyButtonDown()) && !this.fastForwardActive)
					{
						yield return null;
					}
					this.curPathHash = this.screens[this.curScreen].GetCurrentAnimatorStateInfo(0).fullPathHash;
					this.screens[this.curScreen].SetTrigger("Continue");
					this.OnContinue();
					this.text[this.textCounter].SetActive(false);
					this.arrowVisible = false;
				}
				else if (this.allowScreenSkip && this.input.GetAnyButtonDown())
				{
					this.OnScreenSkip();
				}
			}
			this.OnScreenAdvance(this.curScreen);
			if (this.curScreen < this.screens.Length - 1)
			{
				this.screens[this.curScreen].gameObject.SetActive(false);
			}
			this.arrowVisible = false;
			this.curScreen++;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.Skip();
		yield break;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x00090CF4 File Offset: 0x0008F0F4
	public void IrisIn()
	{
		this.allowScreenSkip = false;
		Animator component = this.fader.GetComponent<Animator>();
		Color color = this.fader.color;
		color.a = 1f;
		this.fader.color = color;
		component.SetTrigger("Iris_In");
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x00090D44 File Offset: 0x0008F144
	public virtual void IrisOut()
	{
		Animator component = this.fader.GetComponent<Animator>();
		Color color = this.fader.color;
		color.a = 1f;
		this.fader.color = color;
		component.SetTrigger("Iris_Out");
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x00090D8C File Offset: 0x0008F18C
	public void ShowText()
	{
		this.textCounter++;
		this.text[this.textCounter].SetActive(true);
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x00090DB0 File Offset: 0x0008F1B0
	public void ShowArrow()
	{
		if (this.curPathHash != this.screens[this.curScreen].GetCurrentAnimatorStateInfo(0).fullPathHash)
		{
			this.arrowVisible = true;
		}
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x00090DEC File Offset: 0x0008F1EC
	private IEnumerator skip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.input.GetButtonDown(CupheadButton.Pause))
			{
				base.Skip();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x00090E08 File Offset: 0x0008F208
	protected DLCGenericCutscene.TrappedChar DetectCharacter()
	{
		if (PlayerManager.Multiplayer)
		{
			if (!PlayerManager.playerWasChalice[0] && !PlayerManager.playerWasChalice[1])
			{
				return DLCGenericCutscene.TrappedChar.Chalice;
			}
			if (PlayerManager.playerWasChalice[0])
			{
				return (!PlayerManager.player1IsMugman) ? DLCGenericCutscene.TrappedChar.Cuphead : DLCGenericCutscene.TrappedChar.Mugman;
			}
			return (!PlayerManager.player1IsMugman) ? DLCGenericCutscene.TrappedChar.Mugman : DLCGenericCutscene.TrappedChar.Cuphead;
		}
		else
		{
			if (PlayerManager.playerWasChalice[0])
			{
				return (!PlayerManager.player1IsMugman) ? DLCGenericCutscene.TrappedChar.Cuphead : DLCGenericCutscene.TrappedChar.Mugman;
			}
			return (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm != Charm.charm_chalice) ? DLCGenericCutscene.TrappedChar.Chalice : ((!PlayerManager.player1IsMugman) ? DLCGenericCutscene.TrappedChar.Mugman : DLCGenericCutscene.TrappedChar.Cuphead);
		}
	}

	// Token: 0x04001766 RID: 5990
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04001767 RID: 5991
	[SerializeField]
	private float cursorToVisableTime = 1.25f;

	// Token: 0x04001768 RID: 5992
	[SerializeField]
	private float mainDelay = 0.25f;

	// Token: 0x04001769 RID: 5993
	[SerializeField]
	private Image arrow;

	// Token: 0x0400176A RID: 5994
	[SerializeField]
	protected GameObject[] text;

	// Token: 0x0400176B RID: 5995
	[SerializeField]
	protected Animator[] screens;

	// Token: 0x0400176C RID: 5996
	private int activeScreen;

	// Token: 0x0400176D RID: 5997
	protected bool allowScreenSkip;

	// Token: 0x0400176E RID: 5998
	private float arrowTransparency;

	// Token: 0x0400176F RID: 5999
	private bool arrowVisible;

	// Token: 0x04001770 RID: 6000
	private int textCounter = -1;

	// Token: 0x04001771 RID: 6001
	private int curPathHash;

	// Token: 0x04001772 RID: 6002
	protected int curScreen;

	// Token: 0x04001773 RID: 6003
	protected bool fastForwardActive;

	// Token: 0x04001774 RID: 6004
	public Image fader;

	// Token: 0x020003FF RID: 1023
	protected enum TrappedChar
	{
		// Token: 0x04001776 RID: 6006
		None = -1,
		// Token: 0x04001777 RID: 6007
		Cuphead,
		// Token: 0x04001778 RID: 6008
		Mugman,
		// Token: 0x04001779 RID: 6009
		Chalice
	}
}
