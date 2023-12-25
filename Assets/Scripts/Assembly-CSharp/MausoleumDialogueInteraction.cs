using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006D3 RID: 1747
public class MausoleumDialogueInteraction : DialogueInteractionPoint
{
	// Token: 0x06002532 RID: 9522 RVA: 0x0015CB06 File Offset: 0x0015AF06
	public void BeginDialogue()
	{
		this.Activate();
		this.chaliceAnimator.SetBool("Talking", true);
		this.speechBubble.waitForRealease = false;
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x0015CB2B File Offset: 0x0015AF2B
	protected override void Start()
	{
		base.Start();
		Dialoguer.events.onTextPhase += this.OnDialogueTextSound;
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x0015CB49 File Offset: 0x0015AF49
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onTextPhase -= this.OnDialogueTextSound;
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x0015CB67 File Offset: 0x0015AF67
	private void OnDialogueTextSound(DialoguerTextData data)
	{
		if (!string.IsNullOrEmpty("mausoleum_queen_ghost_speech"))
		{
			AudioManager.Stop("mausoleum_queen_ghost_speech");
		}
		AudioManager.Play("mausoleum_queen_ghost_speech");
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x0015CB8C File Offset: 0x0015AF8C
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
		this.chaliceAnimator.SetBool("Talking", false);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		SceneLoader.LoadLastMap();
		yield break;
	}

	// Token: 0x04002DD8 RID: 11736
	public Animator chaliceAnimator;
}
