using System;
using UnityEngine;

// Token: 0x02000B8E RID: 2958
public class UnityDefaultGui : MonoBehaviour
{
	// Token: 0x0600480D RID: 18445 RVA: 0x0025D86D File Offset: 0x0025BC6D
	private void Awake()
	{
		Dialoguer.Initialize();
	}

	// Token: 0x0600480E RID: 18446 RVA: 0x0025D874 File Offset: 0x0025BC74
	private void Start()
	{
		this.addDialoguerEvents();
	}

	// Token: 0x0600480F RID: 18447 RVA: 0x0025D87C File Offset: 0x0025BC7C
	private void OnGUI()
	{
		if (!this._showing)
		{
			return;
		}
		if (!this._windowShowing)
		{
			return;
		}
		GUI.color = this._guiColor;
		GUI.depth = 10;
		Rect rect = new Rect((float)Screen.width * 0.5f - 250f, (float)Screen.height - 200f - 100f, 500f, 200f);
		Rect position = new Rect(rect.x, rect.y, rect.width, rect.height - (float)(45 * this._choices.Length));
		GUI.Box(position, string.Empty);
		GUI.color = GUI.contentColor;
		GUI.Label(new Rect(position.x + 10f, position.y + 10f, position.width - 20f, position.height - 20f), this._windowText);
		if (this._selectionClicked)
		{
			return;
		}
		for (int i = 0; i < this._choices.Length; i++)
		{
			Rect position2 = new Rect(rect.x, rect.yMax - (float)(45 * (this._choices.Length - i)) + 5f, rect.width, 40f);
			if (GUI.Button(position2, this._choices[i]))
			{
				this._selectionClicked = true;
				Dialoguer.ContinueDialogue(i);
			}
		}
		GUI.color = GUI.contentColor;
	}

	// Token: 0x06004810 RID: 18448 RVA: 0x0025D9F8 File Offset: 0x0025BDF8
	public void addDialoguerEvents()
	{
		Dialoguer.events.onStarted += this.onStartedHandler;
		Dialoguer.events.onEnded += this.onEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.onInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += this.onTextPhaseHandler;
		Dialoguer.events.onWindowClose += this.onWindowCloseHandler;
	}

	// Token: 0x06004811 RID: 18449 RVA: 0x0025DA73 File Offset: 0x0025BE73
	private void onStartedHandler()
	{
		this._showing = true;
	}

	// Token: 0x06004812 RID: 18450 RVA: 0x0025DA7C File Offset: 0x0025BE7C
	private void onEndedHandler()
	{
		this._showing = false;
	}

	// Token: 0x06004813 RID: 18451 RVA: 0x0025DA85 File Offset: 0x0025BE85
	private void onInstantlyEndedHandler()
	{
		this._showing = true;
		this._windowShowing = false;
		this._selectionClicked = false;
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x0025DA9C File Offset: 0x0025BE9C
	private void onTextPhaseHandler(DialoguerTextData data)
	{
		this._guiColor = GUI.contentColor;
		this._windowText = data.text;
		if (data.windowType == DialoguerTextPhaseType.Text)
		{
			this._choices = new string[]
			{
				"Continue"
			};
		}
		else
		{
			this._choices = data.choices;
		}
		string theme = data.theme;
		if (theme != null)
		{
			if (theme == "bad")
			{
				this._guiColor = Color.red;
				goto IL_AD;
			}
			if (theme == "good")
			{
				this._guiColor = Color.green;
				goto IL_AD;
			}
		}
		this._guiColor = GUI.contentColor;
		IL_AD:
		this._windowShowing = true;
		this._selectionClicked = false;
	}

	// Token: 0x06004815 RID: 18453 RVA: 0x0025DB64 File Offset: 0x0025BF64
	private void onWindowCloseHandler()
	{
		this._windowShowing = false;
		this._selectionClicked = false;
	}

	// Token: 0x04004D49 RID: 19785
	public const float HEIGHT = 200f;

	// Token: 0x04004D4A RID: 19786
	public const float WIDTH = 500f;

	// Token: 0x04004D4B RID: 19787
	private bool _showing;

	// Token: 0x04004D4C RID: 19788
	private bool _windowShowing;

	// Token: 0x04004D4D RID: 19789
	private bool _selectionClicked;

	// Token: 0x04004D4E RID: 19790
	private string _windowText = string.Empty;

	// Token: 0x04004D4F RID: 19791
	private string[] _choices;

	// Token: 0x04004D50 RID: 19792
	private Color _guiColor;
}
