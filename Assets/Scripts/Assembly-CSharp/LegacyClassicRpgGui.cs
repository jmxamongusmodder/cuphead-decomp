using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B8B RID: 2955
public class LegacyClassicRpgGui : MonoBehaviour
{
	// Token: 0x060047E3 RID: 18403 RVA: 0x0025C18E File Offset: 0x0025A58E
	private void Awake()
	{
		Dialoguer.Initialize();
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x0025C195 File Offset: 0x0025A595
	private void Start()
	{
		this.addDialoguerEvents();
		this._showDialogueBox = false;
	}

	// Token: 0x060047E5 RID: 18405 RVA: 0x0025C1A4 File Offset: 0x0025A5A4
	private void Update()
	{
		if (!this._dialogue)
		{
			return;
		}
		if (this._windowReady)
		{
			this.calculateText();
		}
		if (!this._dialogue || this._ending)
		{
			return;
		}
		if (!this._isBranchedText)
		{
			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
			{
				if (this._windowCurrentText == this._windowTargetText)
				{
					Dialoguer.ContinueDialogue(0);
				}
				else
				{
					this._windowCurrentText = this._windowTargetText;
					this.audioTextEnd.Play();
				}
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this._currentChoice = (int)Mathf.Repeat((float)(this._currentChoice + 1), (float)this._branchedTextChoices.Length);
				this.audioText.Play();
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this._currentChoice = (int)Mathf.Repeat((float)(this._currentChoice - 1), (float)this._branchedTextChoices.Length);
				this.audioText.Play();
			}
			if (Input.GetMouseButtonDown(0) && this._windowCurrentText != this._windowTargetText)
			{
				this._windowCurrentText = this._windowTargetText;
			}
			if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
			{
				if (this._windowCurrentText == this._windowTargetText)
				{
					Dialoguer.ContinueDialogue(this._currentChoice);
				}
				else
				{
					this._windowCurrentText = this._windowTargetText;
					this.audioTextEnd.Play();
				}
			}
		}
	}

	// Token: 0x060047E6 RID: 18406 RVA: 0x0025C35C File Offset: 0x0025A75C
	public void addDialoguerEvents()
	{
		Dialoguer.events.onStarted += this.onDialogueStartedHandler;
		Dialoguer.events.onEnded += this.onDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.onDialogueInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += this.onDialogueTextPhaseHandler;
		Dialoguer.events.onWindowClose += this.onDialogueWindowCloseHandler;
		Dialoguer.events.onMessageEvent += this.onDialoguerMessageEvent;
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x0025C3ED File Offset: 0x0025A7ED
	private void onDialogueStartedHandler()
	{
		this._dialogue = true;
	}

	// Token: 0x060047E8 RID: 18408 RVA: 0x0025C3F6 File Offset: 0x0025A7F6
	private void onDialogueEndedHandler()
	{
		this._ending = true;
		this.audioTextEnd.Play();
	}

	// Token: 0x060047E9 RID: 18409 RVA: 0x0025C40A File Offset: 0x0025A80A
	private void onDialogueInstantlyEndedHandler()
	{
		this._dialogue = false;
		this._showDialogueBox = false;
		this.resetWindowSize();
	}

	// Token: 0x060047EA RID: 18410 RVA: 0x0025C420 File Offset: 0x0025A820
	private void onDialogueTextPhaseHandler(DialoguerTextData data)
	{
		this._usingPositionRect = data.usingPositionRect;
		this._positionRect = data.rect;
		this._windowCurrentText = string.Empty;
		this._windowTargetText = data.text;
		this._nameText = data.name;
		this._showDialogueBox = true;
		this._isBranchedText = (data.windowType == DialoguerTextPhaseType.BranchedText);
		this._branchedTextChoices = data.choices;
		this._currentChoice = 0;
		if (data.theme != this._theme)
		{
			this.resetWindowSize();
		}
		this._theme = data.theme;
		this.startWindowTweenIn();
	}

	// Token: 0x060047EB RID: 18411 RVA: 0x0025C4C7 File Offset: 0x0025A8C7
	private void onDialogueWindowCloseHandler()
	{
		this.startWindowTweenOut();
	}

	// Token: 0x060047EC RID: 18412 RVA: 0x0025C4CF File Offset: 0x0025A8CF
	private void onDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "playOldRpgSound")
		{
			this.playOldRpgSound(metadata);
		}
	}

	// Token: 0x060047ED RID: 18413 RVA: 0x0025C4E8 File Offset: 0x0025A8E8
	private void OnGUI()
	{
		if (!this._showDialogueBox)
		{
			return;
		}
		GUI.skin = this.skin;
		GUI.depth = 10;
		float x = this._usingPositionRect ? this._positionRect.x : ((float)Screen.width * 0.5f);
		float y = this._usingPositionRect ? this._positionRect.y : ((float)(Screen.height - 100));
		float num = this._usingPositionRect ? this._positionRect.width : 512f;
		float num2 = this._usingPositionRect ? this._positionRect.height : 190f;
		Rect rect = this.centerRect(new Rect(x, y, num * this._windowTweenValue, num2 * this._windowTweenValue));
		rect.width = Mathf.Clamp(rect.width, 32f, 2000f);
		rect.height = Mathf.Clamp(rect.height, 32f, 2000f);
		if (this._theme == "good")
		{
			this.drawDialogueBox(rect, new Color(0.2f, 0.8f, 0.4f));
		}
		else if (this._theme == "bad")
		{
			this.drawDialogueBox(rect, new Color(0.8f, 0.2f, 0.2f));
		}
		else
		{
			this.drawDialogueBox(rect);
		}
		if (this._nameText != string.Empty)
		{
			Rect rect2 = new Rect(rect.x, rect.y - 60f, 150f * this._windowTweenValue, 50f * this._windowTweenValue);
			rect2.width = Mathf.Clamp(rect2.width, 32f, 2000f);
			rect2.height = Mathf.Clamp(rect2.height, 32f, 2000f);
			this.drawDialogueBox(rect2);
			this.drawShadowedText(new Rect(rect2.x + 15f * this._windowTweenValue - 5f * (1f - this._windowTweenValue), rect2.y + 5f * this._windowTweenValue - 10f * (1f - this._windowTweenValue), rect2.width - 30f * this._windowTweenValue, rect2.height - 5f * this._windowTweenValue), this._nameText);
		}
		Rect rect3 = new Rect(rect.x + 20f * this._windowTweenValue, rect.y + 10f * this._windowTweenValue, rect.width - 40f * this._windowTweenValue, rect.height - 20f * this._windowTweenValue);
		this.drawShadowedText(rect3, this._windowCurrentText);
		if (this._isBranchedText && this._windowCurrentText == this._windowTargetText && this._branchedTextChoices != null)
		{
			for (int i = 0; i < this._branchedTextChoices.Length; i++)
			{
				float y2 = rect.yMax - (float)(38 * this._branchedTextChoices.Length - 38 * i) - 20f;
				Rect rect4 = new Rect(rect.x + 60f, y2, rect.width - 80f, 38f);
				this.drawShadowedText(rect4, this._branchedTextChoices[i]);
				if (rect4.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)))
				{
					if (this._currentChoice != i)
					{
						this.audioText.Play();
						this._currentChoice = i;
					}
					if (Input.GetMouseButtonDown(0))
					{
						Dialoguer.ContinueDialogue(this._currentChoice);
						break;
					}
				}
				if (this._currentChoice == i)
				{
					GUI.Box(new Rect(rect4.x - 64f, rect4.y, 64f, 64f), string.Empty, GUI.skin.GetStyle("box_cursor"));
				}
			}
		}
	}

	// Token: 0x060047EE RID: 18414 RVA: 0x0025C941 File Offset: 0x0025AD41
	private void drawDialogueBox(Rect rect)
	{
		this.drawDialogueBox(rect, new Color(0.1764706f, 0.43529412f, 1f));
	}

	// Token: 0x060047EF RID: 18415 RVA: 0x0025C960 File Offset: 0x0025AD60
	private void drawDialogueBox(Rect rect, Color color)
	{
		GUI.color = color;
		GUI.Box(rect, string.Empty, GUI.skin.GetStyle("box_background"));
		GUI.color = GUI.contentColor;
		GUI.color = new Color(0f, 0f, 0f, 0.25f);
		Rect position = new Rect(rect.x + 7f, rect.y + 7f, rect.width - 14f, rect.height - 14f);
		GUI.DrawTextureWithTexCoords(position, this.diagonalLines, new Rect(0f, 0f, position.width / (float)this.diagonalLines.width, position.height / (float)this.diagonalLines.height));
		GUI.color = GUI.contentColor;
		GUI.depth = 20;
		GUI.Box(rect, string.Empty, GUI.skin.GetStyle("box_border"));
		GUI.depth = 10;
	}

	// Token: 0x060047F0 RID: 18416 RVA: 0x0025CA68 File Offset: 0x0025AE68
	private void drawShadowedText(Rect rect, string text)
	{
		GUI.color = new Color(0f, 0f, 0f, 0.5f);
		GUI.Label(new Rect(rect.x + 1f, rect.y + 2f, rect.width, rect.height), text);
		GUI.color = GUI.contentColor;
		GUI.Label(rect, text);
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x0025CAD7 File Offset: 0x0025AED7
	private void playOldRpgSound(string metadata)
	{
		if (metadata == "good")
		{
			this.audioGood.Play();
		}
		else if (metadata == "bad")
		{
			this.audioBad.Play();
		}
	}

	// Token: 0x060047F2 RID: 18418 RVA: 0x0025CB14 File Offset: 0x0025AF14
	private void resetWindowSize()
	{
		this._windowTweenValue = 0f;
		this._windowReady = false;
	}

	// Token: 0x060047F3 RID: 18419 RVA: 0x0025CB28 File Offset: 0x0025AF28
	private void startWindowTweenIn()
	{
		this._showDialogueBox = true;
		DialogueriTween.ValueTo(base.gameObject, new Hashtable
		{
			{
				"from",
				this._windowTweenValue
			},
			{
				"to",
				1
			},
			{
				"onupdatetarget",
				base.gameObject
			},
			{
				"onupdate",
				"updateWindowTweenValue"
			},
			{
				"oncompletetarget",
				base.gameObject
			},
			{
				"oncomplete",
				"windowInComplete"
			},
			{
				"time",
				0.5f
			},
			{
				"easetype",
				DialogueriTween.EaseType.easeOutBack
			}
		});
	}

	// Token: 0x060047F4 RID: 18420 RVA: 0x0025CBE0 File Offset: 0x0025AFE0
	private void startWindowTweenOut()
	{
		this._windowReady = false;
		DialogueriTween.ValueTo(base.gameObject, new Hashtable
		{
			{
				"from",
				this._windowTweenValue
			},
			{
				"to",
				0
			},
			{
				"onupdatetarget",
				base.gameObject
			},
			{
				"onupdate",
				"updateWindowTweenValue"
			},
			{
				"oncompletetarget",
				base.gameObject
			},
			{
				"oncomplete",
				"windowOutComplete"
			},
			{
				"time",
				0.5f
			},
			{
				"easetype",
				DialogueriTween.EaseType.easeInBack
			}
		});
	}

	// Token: 0x060047F5 RID: 18421 RVA: 0x0025CC96 File Offset: 0x0025B096
	private void updateWindowTweenValue(float newValue)
	{
		this._windowTweenValue = newValue;
	}

	// Token: 0x060047F6 RID: 18422 RVA: 0x0025CC9F File Offset: 0x0025B09F
	private void windowInComplete()
	{
		this._windowReady = true;
	}

	// Token: 0x060047F7 RID: 18423 RVA: 0x0025CCA8 File Offset: 0x0025B0A8
	private void windowOutComplete()
	{
		this._showDialogueBox = false;
		this.resetWindowSize();
		if (this._ending)
		{
			this._dialogue = false;
			this._ending = false;
		}
	}

	// Token: 0x060047F8 RID: 18424 RVA: 0x0025CCD0 File Offset: 0x0025B0D0
	private Rect centerRect(Rect rect)
	{
		return new Rect(rect.x - rect.width * 0.5f, rect.y - rect.height * 0.5f, rect.width, rect.height);
	}

	// Token: 0x060047F9 RID: 18425 RVA: 0x0025CD10 File Offset: 0x0025B110
	private void calculateText()
	{
		if (this._windowTargetText == string.Empty || this._windowCurrentText == this._windowTargetText)
		{
			return;
		}
		int num = 2;
		if (this._textFrames < num)
		{
			this._textFrames++;
			return;
		}
		this._textFrames = 0;
		int num2 = 1;
		if (this._windowCurrentText != this._windowTargetText)
		{
			for (int i = 0; i < num2; i++)
			{
				if (this._windowTargetText.Length <= this._windowCurrentText.Length)
				{
					break;
				}
				this._windowCurrentText += this._windowTargetText[this._windowCurrentText.Length];
			}
		}
		this.audioText.Play();
	}

	// Token: 0x04004D1C RID: 19740
	public GUISkin skin;

	// Token: 0x04004D1D RID: 19741
	public Texture2D diagonalLines;

	// Token: 0x04004D1E RID: 19742
	public AudioSource audioText;

	// Token: 0x04004D1F RID: 19743
	public AudioSource audioTextEnd;

	// Token: 0x04004D20 RID: 19744
	public AudioSource audioGood;

	// Token: 0x04004D21 RID: 19745
	public AudioSource audioBad;

	// Token: 0x04004D22 RID: 19746
	private bool _dialogue;

	// Token: 0x04004D23 RID: 19747
	private bool _ending;

	// Token: 0x04004D24 RID: 19748
	private bool _showDialogueBox;

	// Token: 0x04004D25 RID: 19749
	private bool _usingPositionRect;

	// Token: 0x04004D26 RID: 19750
	private Rect _positionRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x04004D27 RID: 19751
	private string _windowTargetText = string.Empty;

	// Token: 0x04004D28 RID: 19752
	private string _windowCurrentText = string.Empty;

	// Token: 0x04004D29 RID: 19753
	private string _nameText = string.Empty;

	// Token: 0x04004D2A RID: 19754
	private bool _isBranchedText;

	// Token: 0x04004D2B RID: 19755
	private string[] _branchedTextChoices;

	// Token: 0x04004D2C RID: 19756
	private int _currentChoice;

	// Token: 0x04004D2D RID: 19757
	private string _theme;

	// Token: 0x04004D2E RID: 19758
	private float _windowTweenValue;

	// Token: 0x04004D2F RID: 19759
	private bool _windowReady;

	// Token: 0x04004D30 RID: 19760
	private float _nameTweenValue;

	// Token: 0x04004D31 RID: 19761
	private int _textFrames = int.MaxValue;
}
