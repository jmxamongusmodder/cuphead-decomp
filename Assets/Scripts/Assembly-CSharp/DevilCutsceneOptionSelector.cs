using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003F8 RID: 1016
public class DevilCutsceneOptionSelector : AbstractMonoBehaviour
{
	// Token: 0x06000DDF RID: 3551 RVA: 0x0008FF5F File Offset: 0x0008E35F
	private void Start()
	{
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.cursor.gameObject.SetActive(false);
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x0008FF7E File Offset: 0x0008E37E
	public void Show()
	{
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.fadeIn_cr());
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x0008FF9C File Offset: 0x0008E39C
	private IEnumerator main_cr()
	{
		this.cursor.gameObject.SetActive(true);
		if (this.cutscene.IsTranslationTextActive())
		{
			this.cursor.transform.position = this.options[this.currentOption].position;
		}
		else
		{
			this.cursor.transform.position = this.bakedOptions[this.currentOption].position;
		}
		for (;;)
		{
			int prevOption = this.currentOption;
			if (this.input.GetButtonDown(CupheadButton.MenuLeft))
			{
				this.currentOption = Mathf.Max(0, this.currentOption - 1);
			}
			if (this.input.GetButtonDown(CupheadButton.MenuRight))
			{
				this.currentOption = Mathf.Min(this.options.Length - 1, this.currentOption + 1);
			}
			if (this.cutscene.IsTranslationTextActive())
			{
				this.cursor.transform.position = this.options[this.currentOption].position;
			}
			else
			{
				this.cursor.transform.position = this.bakedOptions[this.currentOption].position;
			}
			if (this.currentOption > prevOption)
			{
				this.ToggleSFX();
				base.animator.SetTrigger("MoveRight");
			}
			if (this.currentOption < prevOption)
			{
				this.ToggleSFX();
				base.animator.SetTrigger("MoveLeft");
			}
			if (this.input.GetButtonDown(CupheadButton.Accept))
			{
				break;
			}
			yield return null;
		}
		if (this.currentOption == 0)
		{
			this.cutscene.RefuseDevil();
		}
		else
		{
			this.cutscene.JoinDevil();
		}
		this.cursor.gameObject.SetActive(false);
		yield break;
		yield break;
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x0008FFB8 File Offset: 0x0008E3B8
	private IEnumerator fadeIn_cr()
	{
		float t = 0f;
		while (t < 0.75f)
		{
			this.cursorImage.color = new Color(1f, 1f, 1f, t / 0.75f);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.cursorImage.color = new Color(1f, 1f, 1f, 1f);
		yield break;
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x0008FFD3 File Offset: 0x0008E3D3
	private void ToggleSFX()
	{
		AudioManager.Play("ui_toggle");
	}

	// Token: 0x0400173E RID: 5950
	public Transform[] bakedOptions;

	// Token: 0x0400173F RID: 5951
	public Transform[] options;

	// Token: 0x04001740 RID: 5952
	public Transform cursor;

	// Token: 0x04001741 RID: 5953
	public DevilCutscene cutscene;

	// Token: 0x04001742 RID: 5954
	private int currentOption;

	// Token: 0x04001743 RID: 5955
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04001744 RID: 5956
	public Image cursorImage;
}
