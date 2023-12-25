using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000996 RID: 2454
public class MapEquipUIChecklistItem : AbstractMonoBehaviour
{
	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06003963 RID: 14691 RVA: 0x00209A18 File Offset: 0x00207E18
	private float lineWidth
	{
		get
		{
			return this.descriptionText.rectTransform.sizeDelta.x;
		}
	}

	// Token: 0x06003964 RID: 14692 RVA: 0x00209A3D File Offset: 0x00207E3D
	protected override void Awake()
	{
		base.Awake();
		this.originalFontSize = this.descriptionText.fontSize;
	}

	// Token: 0x06003965 RID: 14693 RVA: 0x00209A58 File Offset: 0x00207E58
	public bool EnableCheckbox(bool enabled)
	{
		if (this.checkBox != null)
		{
			this.checkBox.enabled = enabled;
			return enabled;
		}
		return false;
	}

	// Token: 0x06003966 RID: 14694 RVA: 0x00209A88 File Offset: 0x00207E88
	public void SetDescription(Levels selectedLevel, string levelName, bool isFinale)
	{
		PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(selectedLevel);
		Localization.Translation translation = Localization.Translate(selectedLevel.ToString());
		string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
		levelName = translation.text.Replace("\\n", newValue);
		if (levelData.played)
		{
			this.descriptionText.text = levelName;
			this.descriptionText.font = Localization.Instance.fonts[(int)Localization.language][15].fontAsset;
		}
		else
		{
			this.descriptionText.text = this.unknown;
			this.descriptionText.font = Localization.Instance.fonts[0][15].fontAsset;
		}
		if (this.isDicePalaceMiniBoss)
		{
			this.descriptionText.fontSize = ((translation.fonts.fontSize <= 0) ? this.originalFontSize : ((float)translation.fonts.fontSize));
		}
		if (!this.isDicePalaceMiniBoss)
		{
			float num = this.originalFontSize;
			while (this.lineWidth - this.descriptionText.preferredWidth < 0f && this.originalFontSize > 0f)
			{
				num -= 1f;
				this.descriptionText.fontSize = num;
			}
			this.SetLeaderDots(levelName, isFinale);
		}
		if (levelData.played)
		{
			if (!this.isDicePalaceMiniBoss && levelData.completed)
			{
				this.gradeText.text = this.grades[(int)levelData.grade];
				this.timeText.text = this.SecondsToMinutes(levelData.bestTime);
				if (levelData.difficultyBeaten == Level.Mode.Normal && this.checkBox != null && this.checkBox.enabled)
				{
					this.checkMark.enabled = true;
					this.checkMarkHard.enabled = false;
				}
				if (levelData.difficultyBeaten == Level.Mode.Hard && this.checkBox != null && this.checkBox.enabled)
				{
					this.checkMark.enabled = false;
					this.checkMarkHard.enabled = true;
				}
			}
		}
		else
		{
			this.ClearDescription(isFinale);
		}
	}

	// Token: 0x06003967 RID: 14695 RVA: 0x00209CE8 File Offset: 0x002080E8
	private string SecondsToMinutes(float seconds)
	{
		if (seconds == 3.4028235E+38f)
		{
			return "6:66";
		}
		int num = (int)seconds / 60;
		int num2 = (int)seconds % 60;
		return string.Format("{0}:{1:00}", num, num2);
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x00209D2C File Offset: 0x0020812C
	public void ClearDescription(bool isFinale)
	{
		if (!this.isDicePalaceMiniBoss)
		{
			this.gradeText.text = "?";
			this.timeText.text = "?";
			this.descriptionText.text = this.unknown;
			if (isFinale)
			{
				this.SetLeaderDots(this.unknown, isFinale);
			}
			else
			{
				this.SetLeaderDots(this.unknown, isFinale);
			}
		}
		else
		{
			this.descriptionText.text = this.unknown;
		}
		if (this.checkMark != null)
		{
			this.checkMark.enabled = false;
		}
		if (this.checkMarkHard != null)
		{
			this.checkMarkHard.enabled = false;
		}
	}

	// Token: 0x06003969 RID: 14697 RVA: 0x00209DEC File Offset: 0x002081EC
	private void SetLeaderDots(string name, bool isFinale)
	{
		this.leaderDotText.text = this.dots;
		float num = this.lineWidth - this.descriptionText.preferredWidth - this.dotsPadding;
		if (num < 0f)
		{
			this.leaderDotText.text = string.Empty;
			return;
		}
		int num2 = 100000;
		while (this.leaderDotText.text.Length > 2 && this.leaderDotText.preferredWidth > num && num2 > 0)
		{
			num2--;
			this.leaderDotText.text = this.leaderDotText.text.Substring(0, this.leaderDotText.text.Length - 2);
		}
	}

	// Token: 0x04004109 RID: 16649
	[Header("Text")]
	public TextMeshProUGUI descriptionText;

	// Token: 0x0400410A RID: 16650
	public Text leaderDotText;

	// Token: 0x0400410B RID: 16651
	public Text gradeText;

	// Token: 0x0400410C RID: 16652
	public Text timeText;

	// Token: 0x0400410D RID: 16653
	[Header("Images")]
	public Image checkBox;

	// Token: 0x0400410E RID: 16654
	public Image checkMark;

	// Token: 0x0400410F RID: 16655
	public Image checkMarkHard;

	// Token: 0x04004110 RID: 16656
	private readonly string[] grades = new string[]
	{
		"D-",
		"D",
		"D+",
		"C-",
		"C",
		"C+",
		"B-",
		"B",
		"B+",
		"A-",
		"A",
		"A+",
		"S",
		"P"
	};

	// Token: 0x04004111 RID: 16657
	private readonly string unknown = "?????";

	// Token: 0x04004112 RID: 16658
	private readonly string dots = ". . . . . . . . . . . . . . . . . . . . . . .";

	// Token: 0x04004113 RID: 16659
	public bool isDicePalaceMiniBoss;

	// Token: 0x04004114 RID: 16660
	private float dotsPadding = 5f;

	// Token: 0x04004115 RID: 16661
	private float originalFontSize;
}
