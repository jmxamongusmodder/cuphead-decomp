using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200094C RID: 2380
public class MapNPCChaliceFanB : AbstractMonoBehaviour
{
	// Token: 0x0600379D RID: 14237 RVA: 0x001FF190 File Offset: 0x001FD590
	private void Start()
	{
		if (PlayerData.Data.hasTalkedToChaliceFan)
		{
			Dialoguer.SetGlobalFloat(25, 1f);
		}
		this.AddDialoguerEvents();
		int num = 0;
		for (int i = 0; i < Level.chaliceLevels.Length; i++)
		{
			if (PlayerData.Data.GetLevelData(Level.chaliceLevels[i]).completedAsChaliceP1)
			{
				this.lineSprites[i].enabled = true;
			}
		}
		for (int j = 0; j < Level.chaliceLevels.Length; j++)
		{
			if (PlayerData.Data.GetLevelData(Level.chaliceLevels[j]).completedAsChaliceP1)
			{
				num++;
			}
			else if (this.undefeatedBoss == Levels.Test)
			{
				this.undefeatedBoss = Level.chaliceLevels[j];
			}
		}
		if (num == Level.chaliceLevels.Length)
		{
			Dialoguer.SetGlobalFloat(25, 2f);
			this.campfire.gameObject.SetActive(true);
			base.StartCoroutine(this.campfire_smoke_cr());
			this.questComplete = true;
		}
		else
		{
			this.SetBossRefText(this.undefeatedBoss);
		}
		base.StartCoroutine(this.blink_cr());
	}

	// Token: 0x0600379E RID: 14238 RVA: 0x001FF2B4 File Offset: 0x001FD6B4
	private void SetBossRefText(Levels level)
	{
		TranslationElement translationElement = Localization.Find(level.ToString() + "Reference");
		SpeechBubble.Instance.setBossRefText = translationElement.translation.text;
	}

	// Token: 0x0600379F RID: 14239 RVA: 0x001FF2F6 File Offset: 0x001FD6F6
	private void UpdateBossRef()
	{
		this.SetBossRefText(this.undefeatedBoss);
	}

	// Token: 0x060037A0 RID: 14240 RVA: 0x001FF304 File Offset: 0x001FD704
	private void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x060037A1 RID: 14241 RVA: 0x001FF30C File Offset: 0x001FD70C
	public void AddDialoguerEvents()
	{
		Localization.OnLanguageChangedEvent += this.UpdateBossRef;
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded += this.OnDialogueEnded;
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x001FF34B File Offset: 0x001FD74B
	public void RemoveDialoguerEvents()
	{
		Localization.OnLanguageChangedEvent -= this.UpdateBossRef;
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded -= this.OnDialogueEnded;
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x001FF38A File Offset: 0x001FD78A
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "MetChaliceFan")
		{
			PlayerData.Data.hasTalkedToChaliceFan = true;
			PlayerData.SaveCurrentFile();
			Dialoguer.SetGlobalFloat(25, 1f);
		}
	}

	// Token: 0x060037A4 RID: 14244 RVA: 0x001FF3B8 File Offset: 0x001FD7B8
	private IEnumerator blink_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.blinkRange.RandomFloat());
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f > 0.1f)
			{
				yield return null;
			}
			this.blinkRend.enabled = true;
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f < 0.9f)
			{
				yield return null;
			}
			this.blinkRend.enabled = false;
		}
		yield break;
	}

	// Token: 0x060037A5 RID: 14245 RVA: 0x001FF3D4 File Offset: 0x001FD7D4
	private IEnumerator campfire_smoke_cr()
	{
		this.campfire.SetBool("SmokeL", true);
		yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.5f, 1f));
		this.campfire.SetBool("SmokeR", true);
		for (;;)
		{
			if (!this.campfire.GetCurrentAnimatorStateInfo(1).IsName("None") || !this.campfire.GetCurrentAnimatorStateInfo(2).IsName("None"))
			{
				yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(1.5f, 3f));
			}
			this.campfire.SetBool("SmokeL", Rand.Bool());
			this.campfire.SetBool("SmokeR", Rand.Bool());
			if (this.campfire.GetCurrentAnimatorStateInfo(1).IsName("None"))
			{
				this.campfire.SetBool("SmokeL", true);
			}
			if (this.campfire.GetCurrentAnimatorStateInfo(2).IsName("None"))
			{
				this.campfire.SetBool("SmokeR", true);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x001FF3F0 File Offset: 0x001FD7F0
	private void OnDialogueEnded()
	{
		if (this.questComplete && !PlayerData.Data.unlockedChaliceRecolor)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.ChaliceFan);
			PlayerData.Data.unlockedChaliceRecolor = true;
			PlayerData.SaveCurrentFile();
			MapUI.Current.Refresh();
		}
	}

	// Token: 0x04003FA6 RID: 16294
	private const int CHALICEFANBSTATE_INDEX = 25;

	// Token: 0x04003FA7 RID: 16295
	[SerializeField]
	private SpriteRenderer[] lineSprites;

	// Token: 0x04003FA8 RID: 16296
	[SerializeField]
	private SpriteRenderer blinkRend;

	// Token: 0x04003FA9 RID: 16297
	[SerializeField]
	private MinMax blinkRange = new MinMax(3f, 5f);

	// Token: 0x04003FAA RID: 16298
	[SerializeField]
	private Animator campfire;

	// Token: 0x04003FAB RID: 16299
	private Levels undefeatedBoss = Levels.Test;

	// Token: 0x04003FAC RID: 16300
	private bool questComplete;
}
