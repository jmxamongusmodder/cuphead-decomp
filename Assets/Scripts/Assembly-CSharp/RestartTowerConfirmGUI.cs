using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000468 RID: 1128
public class RestartTowerConfirmGUI : AbstractMonoBehaviour
{
	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06001138 RID: 4408 RVA: 0x000A4247 File Offset: 0x000A2647
	// (set) Token: 0x06001139 RID: 4409 RVA: 0x000A424E File Offset: 0x000A264E
	public static Color COLOR_SELECTED { get; private set; }

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x0600113A RID: 4410 RVA: 0x000A4256 File Offset: 0x000A2656
	// (set) Token: 0x0600113B RID: 4411 RVA: 0x000A425D File Offset: 0x000A265D
	public static Color COLOR_INACTIVE { get; private set; }

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x0600113C RID: 4412 RVA: 0x000A4265 File Offset: 0x000A2665
	// (set) Token: 0x0600113D RID: 4413 RVA: 0x000A426D File Offset: 0x000A266D
	public bool restartTowerConfirmMenuOpen { get; private set; }

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x0600113E RID: 4414 RVA: 0x000A4276 File Offset: 0x000A2676
	// (set) Token: 0x0600113F RID: 4415 RVA: 0x000A427E File Offset: 0x000A267E
	public bool inputEnabled { get; private set; }

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06001140 RID: 4416 RVA: 0x000A4287 File Offset: 0x000A2687
	// (set) Token: 0x06001141 RID: 4417 RVA: 0x000A4290 File Offset: 0x000A2690
	private int verticalSelection
	{
		get
		{
			return this._verticalSelection;
		}
		set
		{
			bool flag = value > this._verticalSelection;
			int num = (int)Mathf.Repeat((float)value, (float)this.currentItems.Count);
			while (!this.currentItems[num].text.gameObject.activeSelf)
			{
				num = ((!flag) ? (num - 1) : (num + 1));
				num = (int)Mathf.Repeat((float)num, (float)this.currentItems.Count);
			}
			this._verticalSelection = num;
			this.UpdateVerticalSelection();
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06001142 RID: 4418 RVA: 0x000A4315 File Offset: 0x000A2715
	// (set) Token: 0x06001143 RID: 4419 RVA: 0x000A431D File Offset: 0x000A271D
	public bool justClosed { get; private set; }

	// Token: 0x06001144 RID: 4420 RVA: 0x000A4328 File Offset: 0x000A2728
	protected override void Awake()
	{
		base.Awake();
		this.restartTowerConfirmMenuOpen = false;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
		this.currentItems = new List<RestartTowerConfirmGUI.Button>(this.mainObjectButtons);
		RestartTowerConfirmGUI.COLOR_SELECTED = this.currentItems[0].text.color;
		RestartTowerConfirmGUI.COLOR_INACTIVE = this.currentItems[this.currentItems.Count - 1].text.color;
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x000A43B1 File Offset: 0x000A27B1
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x000A43C0 File Offset: 0x000A27C0
	private void Update()
	{
		this.justClosed = false;
		if (!this.inputEnabled)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.Pause) || this.GetButtonDown(CupheadButton.Cancel))
		{
			this.MenuSelectSound();
			this.HideMenu();
			return;
		}
		if (this.GetButtonDown(CupheadButton.Accept))
		{
			this.MenuSelectSound();
			int verticalSelection = this.verticalSelection;
			if (verticalSelection != 0)
			{
				if (verticalSelection == 1)
				{
					this.ToPauseMenu();
				}
			}
			else
			{
				this.RestartTower();
			}
			return;
		}
		if (this._selectionTimer >= 0.15f)
		{
			if (this.GetButton(CupheadButton.MenuUp))
			{
				this.MenuSelectSound();
				this.verticalSelection--;
			}
			if (this.GetButton(CupheadButton.MenuDown))
			{
				this.MenuSelectSound();
				this.verticalSelection++;
			}
		}
		else
		{
			this._selectionTimer += Time.deltaTime;
		}
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x000A44B4 File Offset: 0x000A28B4
	private void UpdateVerticalSelection()
	{
		this._selectionTimer = 0f;
		for (int i = 0; i < this.currentItems.Count; i++)
		{
			RestartTowerConfirmGUI.Button button = this.currentItems[i];
			if (i == this.verticalSelection)
			{
				button.text.color = RestartTowerConfirmGUI.COLOR_SELECTED;
			}
			else
			{
				button.text.color = RestartTowerConfirmGUI.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x000A4526 File Offset: 0x000A2926
	public void ShowMenu()
	{
		this.restartTowerConfirmMenuOpen = true;
		this.verticalSelection = 0;
		this.canvasGroup.alpha = 1f;
		base.FrameDelayedCallback(new Action(this.Interactable), 1);
		this.UpdateVerticalSelection();
	}

	// Token: 0x06001149 RID: 4425 RVA: 0x000A4560 File Offset: 0x000A2960
	public void HideMenu()
	{
		this.verticalSelection = 0;
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
		this.restartTowerConfirmMenuOpen = false;
		this.justClosed = true;
	}

	// Token: 0x0600114A RID: 4426 RVA: 0x000A45B1 File Offset: 0x000A29B1
	private void Interactable()
	{
		this.verticalSelection = 0;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this.inputEnabled = true;
	}

	// Token: 0x0600114B RID: 4427 RVA: 0x000A45D9 File Offset: 0x000A29D9
	protected void MenuSelectSound()
	{
		AudioManager.Play("level_menu_select");
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x000A45E5 File Offset: 0x000A29E5
	private void ToPauseMenu()
	{
		this.restartTowerConfirmMenuOpen = false;
		this.HideMenu();
	}

	// Token: 0x0600114D RID: 4429 RVA: 0x000A45F4 File Offset: 0x000A29F4
	private void RestartTower()
	{
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		SceneLoader.ResetTheTowerOfPower();
		Dialoguer.EndDialogue();
	}

	// Token: 0x0600114E RID: 4430 RVA: 0x000A460E File Offset: 0x000A2A0E
	protected bool GetButtonDown(CupheadButton button)
	{
		if (this.input.GetButtonDown(button))
		{
			AudioManager.Play("level_menu_select");
			return true;
		}
		return false;
	}

	// Token: 0x0600114F RID: 4431 RVA: 0x000A462E File Offset: 0x000A2A2E
	protected bool GetButton(CupheadButton button)
	{
		return this.input.GetButton(button);
	}

	// Token: 0x04001AB8 RID: 6840
	[SerializeField]
	private GameObject mainObject;

	// Token: 0x04001AB9 RID: 6841
	[SerializeField]
	private RestartTowerConfirmGUI.Button[] mainObjectButtons;

	// Token: 0x04001ABA RID: 6842
	private List<RestartTowerConfirmGUI.Button> currentItems;

	// Token: 0x04001ABB RID: 6843
	private CanvasGroup canvasGroup;

	// Token: 0x04001ABC RID: 6844
	private AbstractPauseGUI pauseMenu;

	// Token: 0x04001ABD RID: 6845
	private float _selectionTimer;

	// Token: 0x04001ABE RID: 6846
	private const float _SELECTION_TIME = 0.15f;

	// Token: 0x04001ABF RID: 6847
	private int _verticalSelection;

	// Token: 0x04001AC0 RID: 6848
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04001AC1 RID: 6849
	private int lastIndex;

	// Token: 0x02000469 RID: 1129
	[Serializable]
	public class Button
	{
		// Token: 0x04001AC3 RID: 6851
		public Text text;
	}
}
