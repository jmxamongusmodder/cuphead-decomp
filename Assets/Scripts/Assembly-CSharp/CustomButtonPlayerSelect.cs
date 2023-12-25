using System;
using System.Collections;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C2A RID: 3114
public class CustomButtonPlayerSelect : CustomButton
{
	// Token: 0x06004C13 RID: 19475 RVA: 0x00272250 File Offset: 0x00270650
	protected override void Start()
	{
		base.Start();
		if (!PlayerManager.Multiplayer)
		{
			base.interactable = false;
		}
		base.StartCoroutine(this.update_cr());
	}

	// Token: 0x06004C14 RID: 19476 RVA: 0x00272278 File Offset: 0x00270678
	private IEnumerator update_cr()
	{
		while (!this.mapper)
		{
			yield return null;
		}
		for (int i = 0; i < this.selectionTabs.Length; i++)
		{
			this.selectionTabs[i].rectTransform.anchoredPosition = new Vector3((this.associatedText.preferredWidth / 2f + 15f) * (float)(i * 2 - 1), this.selectionTabs[i].rectTransform.anchoredPosition.y, 0f);
		}
		for (;;)
		{
			if (this.myInfo.intData == this.mapper.currentPlayerId)
			{
				this.associatedText.color = base.colors.highlightedColor;
			}
			else
			{
				this.associatedText.color = ((base.currentSelectionState != Selectable.SelectionState.Highlighted) ? base.colors.normalColor : base.colors.highlightedColor);
			}
			for (int j = 0; j < this.selectionTabs.Length; j++)
			{
				this.selectionTabs[j].enabled = (this.myInfo.intData == this.mapper.currentPlayerId);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004C15 RID: 19477 RVA: 0x00272293 File Offset: 0x00270693
	public override void OnSelect(BaseEventData eventData)
	{
		if (this.IsInteractable())
		{
			base.OnSelect(eventData);
		}
		else
		{
			base.StartCoroutine(this.move_selection_cr());
		}
	}

	// Token: 0x06004C16 RID: 19478 RVA: 0x002722BC File Offset: 0x002706BC
	private IEnumerator move_selection_cr()
	{
		yield return new WaitForEndOfFrame();
		EventSystem.current.SetSelectedGameObject(this.FindSelectableOnUp().gameObject);
		yield break;
	}

	// Token: 0x06004C17 RID: 19479 RVA: 0x002722D8 File Offset: 0x002706D8
	protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
	{
		if (this.mapper && this.myInfo.intData == this.mapper.currentPlayerId)
		{
			return;
		}
		switch (state)
		{
		case Selectable.SelectionState.Normal:
			this.associatedText.color = base.colors.normalColor;
			break;
		case Selectable.SelectionState.Highlighted:
			this.associatedText.color = base.colors.highlightedColor;
			break;
		case Selectable.SelectionState.Pressed:
			this.associatedText.color = base.colors.pressedColor;
			break;
		case Selectable.SelectionState.Disabled:
			this.associatedText.color = base.colors.disabledColor;
			break;
		}
	}

	// Token: 0x040050BA RID: 20666
	public ControlMapper mapper;

	// Token: 0x040050BB RID: 20667
	[SerializeField]
	private ButtonInfo myInfo;

	// Token: 0x040050BC RID: 20668
	[SerializeField]
	private Image[] selectionTabs;
}
