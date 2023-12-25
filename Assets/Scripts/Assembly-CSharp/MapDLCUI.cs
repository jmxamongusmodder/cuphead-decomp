using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200099C RID: 2460
public class MapDLCUI : AbstractMonoBehaviour
{
	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x0600399A RID: 14746 RVA: 0x0020BAA7 File Offset: 0x00209EA7
	// (set) Token: 0x0600399B RID: 14747 RVA: 0x0020BAB0 File Offset: 0x00209EB0
	private int selection
	{
		get
		{
			return this._selection;
		}
		set
		{
			bool flag = value > this._selection;
			int num = (int)Mathf.Repeat((float)value, (float)this.menuItems.Length);
			while (!this.menuItems[num].gameObject.activeSelf)
			{
				num = ((!flag) ? (num - 1) : (num + 1));
				num = (int)Mathf.Repeat((float)num, (float)this.menuItems.Length);
			}
			this._selection = num;
			this.UpdateSelection();
		}
	}

	// Token: 0x0600399C RID: 14748 RVA: 0x0020BB26 File Offset: 0x00209F26
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x0600399D RID: 14749 RVA: 0x0020BB34 File Offset: 0x00209F34
	protected override void Awake()
	{
		base.Awake();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
		this.COLOR_SELECTED = this.menuItems[0].color;
		this.COLOR_INACTIVE = this.menuItems[this.menuItems.Length - 1].color;
	}

	// Token: 0x0600399E RID: 14750 RVA: 0x0020BB92 File Offset: 0x00209F92
	private void OnDestroy()
	{
		PauseManager.Unpause();
	}

	// Token: 0x0600399F RID: 14751 RVA: 0x0020BB9C File Offset: 0x00209F9C
	private void Update()
	{
		if (!this.inputEnabled)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.Accept))
		{
			this.MenuSelectSound();
			int selection = this.selection;
			if (selection != 0)
			{
				if (selection == 1)
				{
					this.Close();
				}
			}
			else
			{
				this.ExitToTitle();
			}
			return;
		}
		if (this._selectionTimer >= 0.15f)
		{
			if (this.GetButton(CupheadButton.MenuUp))
			{
				this.MenuMoveSound();
				this.selection--;
			}
			if (this.GetButton(CupheadButton.MenuDown))
			{
				this.MenuMoveSound();
				this.selection++;
			}
		}
		else
		{
			this._selectionTimer += Time.deltaTime;
		}
	}

	// Token: 0x060039A0 RID: 14752 RVA: 0x0020BC64 File Offset: 0x0020A064
	private void UpdateVerticalSelection()
	{
		this._selectionTimer = 0f;
		for (int i = 0; i < this.menuItems.Length; i++)
		{
			Text text = this.menuItems[i];
			if (i == this.selection)
			{
				text.color = this.COLOR_SELECTED;
			}
			else
			{
				text.color = this.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x060039A1 RID: 14753 RVA: 0x0020BCC8 File Offset: 0x0020A0C8
	private void UpdateSelection()
	{
		this._selectionTimer = 0f;
		for (int i = 0; i < this.menuItems.Length; i++)
		{
			Text text = this.menuItems[i];
			if (i == this.selection)
			{
				text.color = this.COLOR_SELECTED;
			}
			else
			{
				text.color = this.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x060039A2 RID: 14754 RVA: 0x0020BD2B File Offset: 0x0020A12B
	private void ExitToTitle()
	{
		PlayerManager.ResetPlayers();
		Dialoguer.EndDialogue();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x060039A3 RID: 14755 RVA: 0x0020BD41 File Offset: 0x0020A141
	private void Close()
	{
		this.HideMenu();
	}

	// Token: 0x060039A4 RID: 14756 RVA: 0x0020BD49 File Offset: 0x0020A149
	public void ShowMenu()
	{
		this.visible = true;
		this.selection = 0;
		this.UpdateVerticalSelection();
		base.StartCoroutine(this.show_cr());
	}

	// Token: 0x060039A5 RID: 14757 RVA: 0x0020BD6C File Offset: 0x0020A16C
	private IEnumerator show_cr()
	{
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(0f, 1f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 1f;
		yield return null;
		this.Interactable();
		yield break;
	}

	// Token: 0x060039A6 RID: 14758 RVA: 0x0020BD87 File Offset: 0x0020A187
	public void HideMenu()
	{
		base.StartCoroutine(this.hide_cr());
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
	}

	// Token: 0x060039A7 RID: 14759 RVA: 0x0020BDB8 File Offset: 0x0020A1B8
	private IEnumerator hide_cr()
	{
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(1f, 0f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 0f;
		yield return null;
		this.selection = 0;
		this.visible = false;
		yield break;
	}

	// Token: 0x060039A8 RID: 14760 RVA: 0x0020BDD3 File Offset: 0x0020A1D3
	private void Interactable()
	{
		this.selection = 0;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this.inputEnabled = true;
	}

	// Token: 0x060039A9 RID: 14761 RVA: 0x0020BDFB File Offset: 0x0020A1FB
	protected bool GetButtonDown(CupheadButton button)
	{
		if (this.input.GetButtonDown(button))
		{
			AudioManager.Play("level_menu_select");
			return true;
		}
		return false;
	}

	// Token: 0x060039AA RID: 14762 RVA: 0x0020BE1B File Offset: 0x0020A21B
	protected bool GetButton(CupheadButton button)
	{
		return this.input.GetButton(button);
	}

	// Token: 0x060039AB RID: 14763 RVA: 0x0020BE31 File Offset: 0x0020A231
	protected void MenuSelectSound()
	{
		AudioManager.Play("level_menu_select");
	}

	// Token: 0x060039AC RID: 14764 RVA: 0x0020BE3D File Offset: 0x0020A23D
	protected void MenuMoveSound()
	{
		AudioManager.Play("level_menu_move");
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x060039AD RID: 14765 RVA: 0x0020BE49 File Offset: 0x0020A249
	// (set) Token: 0x060039AE RID: 14766 RVA: 0x0020BE51 File Offset: 0x0020A251
	public bool visible { get; private set; }

	// Token: 0x04004157 RID: 16727
	[SerializeField]
	private Text[] menuItems;

	// Token: 0x04004158 RID: 16728
	private float _selectionTimer;

	// Token: 0x04004159 RID: 16729
	private const float _SELECTION_TIME = 0.15f;

	// Token: 0x0400415A RID: 16730
	private int _selection;

	// Token: 0x0400415B RID: 16731
	private Color COLOR_SELECTED;

	// Token: 0x0400415C RID: 16732
	private Color COLOR_INACTIVE;

	// Token: 0x0400415D RID: 16733
	private bool inputEnabled;

	// Token: 0x0400415E RID: 16734
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x0400415F RID: 16735
	private CanvasGroup canvasGroup;
}
