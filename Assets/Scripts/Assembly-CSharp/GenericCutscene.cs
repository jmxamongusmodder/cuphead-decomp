using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000404 RID: 1028
public class GenericCutscene : Cutscene
{
	// Token: 0x06000E48 RID: 3656 RVA: 0x00092565 File Offset: 0x00090965
	protected override void Start()
	{
		base.Start();
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.skip_cr());
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x000925A4 File Offset: 0x000909A4
	private IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		for (int i = 0; i < this.numScreens; i++)
		{
			if (this.specialCase && i == 3)
			{
				yield return CupheadTime.WaitForSeconds(this, 2.25f);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, 1.25f);
			}
			this.arrowVisible = true;
			while (this.input.GetButtonDown(CupheadButton.Pause) || !this.input.GetAnyButtonDown())
			{
				yield return null;
			}
			this.arrowVisible = false;
			if (i < this.numScreens - 1)
			{
				base.animator.SetTrigger("Continue");
			}
			AudioManager.Play("ui_confirm_generic");
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.Skip();
		yield break;
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x000925C0 File Offset: 0x000909C0
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

	// Token: 0x06000E4B RID: 3659 RVA: 0x000925DC File Offset: 0x000909DC
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
		this.arrow.color = new Color(1f, 1f, 1f, this.arrowTransparency);
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x00092646 File Offset: 0x00090A46
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.arrow = null;
	}

	// Token: 0x04001794 RID: 6036
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04001795 RID: 6037
	[SerializeField]
	private bool specialCase;

	// Token: 0x04001796 RID: 6038
	[SerializeField]
	private Image arrow;

	// Token: 0x04001797 RID: 6039
	[SerializeField]
	private int numScreens;

	// Token: 0x04001798 RID: 6040
	private float arrowTransparency;

	// Token: 0x04001799 RID: 6041
	private bool arrowVisible;
}
