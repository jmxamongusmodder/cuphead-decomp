using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006CC RID: 1740
public class HouseElderKettle : DialogueInteractionPoint
{
	// Token: 0x0600250B RID: 9483 RVA: 0x0015BA6B File Offset: 0x00159E6B
	public void BeginDialogue()
	{
		this.Activate();
		this.speechBubble.waitForRealease = false;
	}

	// Token: 0x0600250C RID: 9484 RVA: 0x0015BA80 File Offset: 0x00159E80
	protected override void Start()
	{
		base.Start();
		this.hasTarget = false;
		Dialoguer.events.onTextPhase += this.OnDialogueTextSound;
		Dialoguer.events.onStarted += this.StartTalkingCoroutine;
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600250D RID: 9485 RVA: 0x0015BADC File Offset: 0x00159EDC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onTextPhase -= this.OnDialogueTextSound;
		Dialoguer.events.onStarted -= this.StartTalkingCoroutine;
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600250E RID: 9486 RVA: 0x0015BB34 File Offset: 0x00159F34
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "ElderKettleBottle")
		{
			base.animator.SetTrigger("Bottle");
			base.StartCoroutine(this.bottle_sound_cr());
		}
		if (message == "ElderKettleFirstWeapon")
		{
			base.animator.SetTrigger("Continue");
		}
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x0015BB8E File Offset: 0x00159F8E
	private void StartTalkingCoroutine()
	{
		base.StartCoroutine(this.talking_crs());
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x0015BBA0 File Offset: 0x00159FA0
	private void OnDialogueTextSound(DialoguerTextData data)
	{
		if (!string.IsNullOrEmpty(this.lastDialogueSFXName))
		{
			AudioManager.Stop(this.lastDialogueSFXName);
		}
		if (data.metadata == "excitedburst")
		{
			if (this.playFirstGroupExcited)
			{
				AudioManager.Play("ek_excitedburst");
				this.lastDialogueSFXName = "ek_excitedburst";
				this.playFirstGroupExcited = false;
			}
			else
			{
				AudioManager.Play("ek_excitedburst2");
				this.lastDialogueSFXName = "ek_excitedburst2";
				this.playFirstGroupExcited = true;
			}
		}
		else if (data.metadata == "laugh")
		{
			if (this.playFirstGroupLaugh)
			{
				AudioManager.Play("ek_laugh");
				this.lastDialogueSFXName = "ek_laugh";
				this.playFirstGroupLaugh = false;
			}
			else
			{
				AudioManager.Play("ek_laugh2");
				this.lastDialogueSFXName = "ek_laugh2";
				this.playFirstGroupLaugh = true;
			}
		}
		else if (data.metadata == "mckellen")
		{
			if (this.playFirstGroupMckellen)
			{
				AudioManager.Play("ek_mckellen");
				this.lastDialogueSFXName = "ek_mckellen";
				this.playFirstGroupMckellen = false;
			}
			else
			{
				AudioManager.Play("ek_mckellen2");
				this.lastDialogueSFXName = "ek_mckellen2";
				this.playFirstGroupMckellen = true;
			}
		}
		else if (data.metadata == "warstory")
		{
			if (this.playFirstGroupWarstory)
			{
				AudioManager.Play("ek_warstory");
				this.lastDialogueSFXName = "ek_warstory";
				this.playFirstGroupWarstory = false;
			}
			else
			{
				AudioManager.Play("ek_warstory2");
				this.lastDialogueSFXName = "ek_warstory2";
				this.playFirstGroupWarstory = true;
			}
		}
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x0015BD4F File Offset: 0x0015A14F
	public void LoopAnimation()
	{
		this.nbLoopsAnimator--;
	}

	// Token: 0x06002512 RID: 9490 RVA: 0x0015BD60 File Offset: 0x0015A160
	private IEnumerator bottle_sound_cr()
	{
		yield return new WaitForSeconds(0.1f);
		AudioManager.Play("sfx_potion_reveal");
		yield break;
	}

	// Token: 0x06002513 RID: 9491 RVA: 0x0015BD74 File Offset: 0x0015A174
	private IEnumerator talking_crs()
	{
		base.animator.SetBool("IsTalking", true);
		this.nbLoopsAnimator = UnityEngine.Random.Range(2, 7);
		while (this.conversationIsActive)
		{
			if (this.nbLoopsAnimator == 0)
			{
				base.animator.SetTrigger("Continue");
				this.nbLoopsAnimator = UnityEngine.Random.Range(2, 7);
			}
			yield return null;
		}
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Talking_Loop_B"))
		{
			base.animator.SetTrigger("Continue");
		}
		base.animator.SetBool("IsTalking", false);
		yield break;
	}

	// Token: 0x06002514 RID: 9492 RVA: 0x0015BD90 File Offset: 0x0015A190
	protected override IEnumerator ReactivateInputsCoroutine(LevelPlayerMotor playerOneMotor, LevelPlayerMotor playerTwoMotor, LevelPlayerWeaponManager playerOneWeaponManager, LevelPlayerWeaponManager playerTwoWeaponManager, Animator animator)
	{
		this.speechBubble.preventQuit = true;
		AbstractPlayerController playercontroller = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		if (playercontroller != null && playercontroller.animator != null && playercontroller.animator.GetCurrentAnimatorStateInfo(0).IsName("Power_Up"))
		{
			yield return playercontroller.animator.WaitForAnimationToEnd(this, "Power_Up", false, true);
		}
		this.speechBubble.preventQuit = false;
		yield return base.StartCoroutine(base.ReactivateInputsCoroutine(playerOneMotor, playerTwoMotor, playerOneWeaponManager, playerTwoWeaponManager, animator));
		yield break;
	}

	// Token: 0x04002DBF RID: 11711
	private string lastDialogueSFXName;

	// Token: 0x04002DC0 RID: 11712
	private int nbLoopsAnimator;

	// Token: 0x04002DC1 RID: 11713
	private bool playFirstGroupMckellen = true;

	// Token: 0x04002DC2 RID: 11714
	private bool playFirstGroupWarstory = true;

	// Token: 0x04002DC3 RID: 11715
	private bool playFirstGroupExcited = true;

	// Token: 0x04002DC4 RID: 11716
	private bool playFirstGroupLaugh = true;
}
