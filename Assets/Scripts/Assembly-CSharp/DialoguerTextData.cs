using System;
using System.Collections.Generic;
using DialoguerCore;
using UnityEngine;

// Token: 0x02000B70 RID: 2928
public struct DialoguerTextData
{
	// Token: 0x0600469B RID: 18075 RVA: 0x0024E998 File Offset: 0x0024CD98
	public DialoguerTextData(string text, string themeName, bool newWindow, string name, string portrait, string metadata, string audio, float audioDelay, Rect rect, List<string> choices, int dialogueID, int nodeID)
	{
		this.dialogueID = dialogueID;
		this.nodeID = nodeID;
		this.rawText = text;
		this.theme = themeName;
		this.newWindow = newWindow;
		this.name = name;
		this.portrait = portrait;
		this.metadata = metadata;
		this.audio = audio;
		this.audioDelay = audioDelay;
		this.rect = new Rect(rect.x, rect.y, rect.width, rect.height);
		if (choices != null)
		{
			string[] array = choices.ToArray();
			this.choices = (array.Clone() as string[]);
		}
		else
		{
			this.choices = null;
		}
		this._cachedText = null;
	}

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x0600469C RID: 18076 RVA: 0x0024EA4C File Offset: 0x0024CE4C
	public string text
	{
		get
		{
			if (this._cachedText == null)
			{
				this._cachedText = DialoguerUtils.insertTextPhaseStringVariables(this.rawText);
			}
			return this._cachedText;
		}
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x0600469D RID: 18077 RVA: 0x0024EA70 File Offset: 0x0024CE70
	public bool usingPositionRect
	{
		get
		{
			return this.rect.x != 0f || this.rect.y != 0f || this.rect.width != 0f || this.rect.height != 0f;
		}
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x0600469E RID: 18078 RVA: 0x0024EAE0 File Offset: 0x0024CEE0
	public DialoguerTextPhaseType windowType
	{
		get
		{
			return (this.choices != null) ? DialoguerTextPhaseType.BranchedText : DialoguerTextPhaseType.Text;
		}
	}

	// Token: 0x0600469F RID: 18079 RVA: 0x0024EAF4 File Offset: 0x0024CEF4
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"\nTheme ID: ",
			this.theme,
			"\nNew Window: ",
			this.newWindow.ToString(),
			"\nName: ",
			this.name,
			"\nPortrait: ",
			this.portrait,
			"\nMetadata: ",
			this.metadata,
			"\nAudio Clip: ",
			this.audio,
			"\nAudio Delay: ",
			this.audioDelay.ToString(),
			"\nRect: ",
			this.rect.ToString(),
			"\nRaw Text: ",
			this.rawText,
			"\nDialogue ID:",
			this.dialogueID,
			"\nNode ID:",
			this.nodeID
		});
	}

	// Token: 0x04004C7B RID: 19579
	public readonly int dialogueID;

	// Token: 0x04004C7C RID: 19580
	public readonly int nodeID;

	// Token: 0x04004C7D RID: 19581
	public readonly string rawText;

	// Token: 0x04004C7E RID: 19582
	public readonly string theme;

	// Token: 0x04004C7F RID: 19583
	public readonly bool newWindow;

	// Token: 0x04004C80 RID: 19584
	public readonly string name;

	// Token: 0x04004C81 RID: 19585
	public readonly string portrait;

	// Token: 0x04004C82 RID: 19586
	public readonly string metadata;

	// Token: 0x04004C83 RID: 19587
	public readonly string audio;

	// Token: 0x04004C84 RID: 19588
	public readonly float audioDelay;

	// Token: 0x04004C85 RID: 19589
	public readonly Rect rect;

	// Token: 0x04004C86 RID: 19590
	public readonly string[] choices;

	// Token: 0x04004C87 RID: 19591
	private string _cachedText;
}
