using System;
using UnityEngine;

// Token: 0x02000B8C RID: 2956
public class LegacyNextGenRpgGui : MonoBehaviour
{
	// Token: 0x060047FB RID: 18427 RVA: 0x0025CDF6 File Offset: 0x0025B1F6
	private void Awake()
	{
		Dialoguer.Initialize();
	}

	// Token: 0x060047FC RID: 18428 RVA: 0x0025CDFD File Offset: 0x0025B1FD
	private void Start()
	{
		this.addDialoguerEvents();
		this._dialogue = false;
	}

	// Token: 0x060047FD RID: 18429 RVA: 0x0025CE0C File Offset: 0x0025B20C
	private void Update()
	{
		if (this._showWindow && Input.GetMouseButtonDown(0))
		{
			if (this._choices != null)
			{
				this.audioSelect.Play();
			}
			Dialoguer.ContinueDialogue(this._currentChoice);
		}
	}

	// Token: 0x060047FE RID: 18430 RVA: 0x0025CE48 File Offset: 0x0025B248
	public void addDialoguerEvents()
	{
		Dialoguer.events.onStarted += this.onDialogueStartedHandler;
		Dialoguer.events.onEnded += this.onDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.onDialogueInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += this.onDialogueTextPhaseHandler;
		Dialoguer.events.onWindowClose += this.onDialogueWindowCloseHandler;
		Dialoguer.events.onMessageEvent += this.onDialoguerMessageEvent;
	}

	// Token: 0x060047FF RID: 18431 RVA: 0x0025CED9 File Offset: 0x0025B2D9
	private void onDialogueStartedHandler()
	{
		this._dialogue = true;
	}

	// Token: 0x06004800 RID: 18432 RVA: 0x0025CEE2 File Offset: 0x0025B2E2
	private void onDialogueEndedHandler()
	{
		this._dialogue = false;
		this._showWindow = false;
	}

	// Token: 0x06004801 RID: 18433 RVA: 0x0025CEF2 File Offset: 0x0025B2F2
	private void onDialogueInstantlyEndedHandler()
	{
		this._dialogue = false;
		this._showWindow = false;
	}

	// Token: 0x06004802 RID: 18434 RVA: 0x0025CF04 File Offset: 0x0025B304
	private void onDialogueTextPhaseHandler(DialoguerTextData data)
	{
		this._currentChoice = 0;
		if (data.choices != null)
		{
			this._choices = new string[6];
			for (int i = 0; i < 6; i++)
			{
				if (data.choices.Length > i && data.choices[i] != null)
				{
					this._choices[i] = data.choices[i];
					this._currentChoice = i;
				}
			}
		}
		else
		{
			this._choices = null;
		}
		this._text = data.text;
		if (data.name != null && data.name != string.Empty)
		{
			this._text = data.name + ": " + this._text;
		}
		this._showWindow = true;
	}

	// Token: 0x06004803 RID: 18435 RVA: 0x0025CFD6 File Offset: 0x0025B3D6
	private void onDialogueWindowCloseHandler()
	{
		this._showWindow = false;
	}

	// Token: 0x06004804 RID: 18436 RVA: 0x0025CFDF File Offset: 0x0025B3DF
	private void onDialoguerMessageEvent(string message, string metadata)
	{
	}

	// Token: 0x06004805 RID: 18437 RVA: 0x0025CFE4 File Offset: 0x0025B3E4
	private void OnGUI()
	{
		if (!this._dialogue)
		{
			return;
		}
		if (!this._showWindow)
		{
			return;
		}
		GUI.skin = this.guiSkin;
		int num = 260;
		Rect rect = new Rect((float)Screen.width * 0.5f - 300f, (float)(Screen.height - num), 600f, 80f);
		GUIStyle guistyle = new GUIStyle("label");
		guistyle.alignment = TextAnchor.MiddleCenter;
		this.drawText(this._text, rect, guistyle);
		if (this._choices != null)
		{
			this.drawChoiceRing();
		}
	}

	// Token: 0x06004806 RID: 18438 RVA: 0x0025D07C File Offset: 0x0025B47C
	private void drawText(string text, Rect rect)
	{
		GUIStyle style = new GUIStyle("label");
		this.drawText(text, rect, style);
	}

	// Token: 0x06004807 RID: 18439 RVA: 0x0025D0A4 File Offset: 0x0025B4A4
	private void drawText(string text, Rect rect, GUIStyle style)
	{
		GUI.color = Color.black;
		for (int i = 0; i < LegacyNextGenRpgGui.TEXT_OUTLINE_WIDTH; i++)
		{
			for (int j = 0; j < LegacyNextGenRpgGui.TEXT_OUTLINE_WIDTH; j++)
			{
				GUI.Label(new Rect(rect.x + (float)(i + 1), rect.y + (float)(j + 1), rect.width, rect.height), text, style);
				GUI.Label(new Rect(rect.x - (float)(i + 1), rect.y - (float)(j + 1), rect.width, rect.height), text, style);
				GUI.Label(new Rect(rect.x + (float)(i + 1), rect.y - (float)(j + 1), rect.width, rect.height), text, style);
				GUI.Label(new Rect(rect.x - (float)(i + 1), rect.y + (float)(j + 1), rect.width, rect.height), text, style);
			}
		}
		GUI.color = GUI.contentColor;
		GUI.Label(rect, text, style);
	}

	// Token: 0x06004808 RID: 18440 RVA: 0x0025D1C4 File Offset: 0x0025B5C4
	private void drawChoiceRing()
	{
		Rect position = new Rect((float)Screen.width * 0.5f - 128f, (float)(Screen.height - 128 - 50), 256f, 128f);
		if (this._ringeRects == null)
		{
			this._ringeRects = new Rect[6];
			this._ringeRects[0] = new Rect(position.center.x, position.y - 40f, (float)Screen.width * 0.5f, position.height * 0.3333f + 40f);
			this._ringeRects[1] = new Rect(position.center.x, position.y + position.height * 0.3333f, (float)Screen.width * 0.5f, position.height * 0.3333f);
			this._ringeRects[2] = new Rect(position.center.x, position.y + position.height * 0.3333f * 2f, (float)Screen.width * 0.5f, position.height * 0.3333f + 40f);
			this._ringeRects[3] = new Rect(0f, position.y - 40f, (float)Screen.width * 0.5f, position.height * 0.3333f + 40f);
			this._ringeRects[4] = new Rect(0f, position.y + position.height * 0.3333f, (float)Screen.width * 0.5f, position.height * 0.3333f);
			this._ringeRects[5] = new Rect(0f, position.y + position.height * 0.3333f * 2f, (float)Screen.width * 0.5f, position.height * 0.3333f + 40f);
		}
		if (this._choicesTextRects == null)
		{
			this._choicesTextRects = new Rect[6];
			this._choicesTextRects[0] = new Rect(position.center.x + position.width * 0.5f - 10f, position.y, (float)Screen.width * 0.5f - position.width * 0.5f + 10f, position.height * 0.3333f);
			this._choicesTextRects[1] = new Rect(position.center.x + position.width * 0.5f + 10f, position.y + position.height * 0.3333f - 5f, (float)Screen.width * 0.5f - position.width * 0.5f - 10f, position.height * 0.3333f);
			this._choicesTextRects[2] = new Rect(position.center.x + position.width * 0.5f, position.y + position.height * 0.3333f * 2f, (float)Screen.width * 0.5f - position.width * 0.5f, position.height * 0.3333f);
			this._choicesTextRects[3] = new Rect(0f, position.y, (float)Screen.width * 0.5f - position.width * 0.5f + 10f, position.height * 0.3333f);
			this._choicesTextRects[4] = new Rect(0f, position.y + position.height * 0.3333f - 5f, (float)Screen.width * 0.5f - position.width * 0.5f - 10f, position.height * 0.3333f);
			this._choicesTextRects[5] = new Rect(0f, position.y + position.height * 0.3333f * 2f, (float)Screen.width * 0.5f - position.width * 0.5f, position.height * 0.3333f);
		}
		GUI.DrawTexture(position, this.ringBase);
		for (int i = 0; i < 6; i++)
		{
			if (this._choices[i] != null && this._choices[i] != string.Empty)
			{
				if (this._currentChoice != i && this._ringeRects[i].Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)))
				{
					this._currentChoice = i;
					this.audioChoice.PlayOneShot(this.audioChoice.clip);
				}
				if (this._currentChoice == i)
				{
					GUI.DrawTexture(position, this.ringHover.getPieces()[i]);
				}
				else
				{
					GUI.DrawTexture(position, this.ringNormal.getPieces()[i]);
				}
				GUIStyle guistyle = new GUIStyle("label");
				if (i > 2)
				{
					guistyle.alignment = TextAnchor.MiddleRight;
				}
				else
				{
					guistyle.alignment = TextAnchor.MiddleLeft;
				}
				this.drawText(this._choices[i], this._choicesTextRects[i], guistyle);
			}
		}
	}

	// Token: 0x04004D32 RID: 19762
	private static int TEXT_OUTLINE_WIDTH = 1;

	// Token: 0x04004D33 RID: 19763
	public GUISkin guiSkin;

	// Token: 0x04004D34 RID: 19764
	public AudioSource audioChoice;

	// Token: 0x04004D35 RID: 19765
	public AudioSource audioSelect;

	// Token: 0x04004D36 RID: 19766
	public Texture ringBase;

	// Token: 0x04004D37 RID: 19767
	public Texture ringTop;

	// Token: 0x04004D38 RID: 19768
	public Texture ringBottom;

	// Token: 0x04004D39 RID: 19769
	public NextGenRingPieces ringNormal;

	// Token: 0x04004D3A RID: 19770
	public NextGenRingPieces ringHover;

	// Token: 0x04004D3B RID: 19771
	private int _currentChoice;

	// Token: 0x04004D3C RID: 19772
	private Rect[] _ringeRects;

	// Token: 0x04004D3D RID: 19773
	private Rect[] _choicesTextRects;

	// Token: 0x04004D3E RID: 19774
	private bool _dialogue;

	// Token: 0x04004D3F RID: 19775
	private bool _showWindow;

	// Token: 0x04004D40 RID: 19776
	private string _text;

	// Token: 0x04004D41 RID: 19777
	private string[] _choices;
}
