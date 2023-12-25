using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000426 RID: 1062
public class DialogueInteractionPoint : SpeechInteractionPoint
{
	// Token: 0x06000F5C RID: 3932 RVA: 0x00097D2C File Offset: 0x0009612C
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = new Color(0.8627451f, 0.8627451f, 0.8627451f);
		Vector3 position = base.transform.position;
		position.x += this.speechBubblePosition.x;
		position.y += this.speechBubblePosition.y;
		Gizmos.DrawWireSphere(position, this.interactionDistance * 0.5f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.playerOneDialoguePosition, 10f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.playerTwoDialoguePosition, 10f);
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x00097DE8 File Offset: 0x000961E8
	protected virtual void Start()
	{
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x00097E44 File Offset: 0x00096244
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueEndedHandler;
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
		this.onEndedActionQueue.Clear();
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x00097EB0 File Offset: 0x000962B0
	private void OnDialogueEndedHandler()
	{
		if (base.AbleToActivate())
		{
			this.Show(PlayerId.PlayerOne);
		}
		foreach (Action action in this.onEndedActionQueue)
		{
			action();
		}
		this.onEndedActionQueue.Clear();
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x00097F28 File Offset: 0x00096328
	protected override void Activate()
	{
		if (this.speechBubble.displayState == SpeechBubble.DisplayState.Hidden)
		{
			Vector3 position = base.transform.position;
			position.x += this.speechBubblePosition.x;
			position.y += this.speechBubblePosition.y;
			this.speechBubble.basePosition = position;
			if (this.cutsceneCoroutine != null)
			{
				base.StopCoroutine(this.cutsceneCoroutine);
			}
			this.cutsceneCoroutine = base.StartCoroutine(this.CutScene_cr());
		}
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x00097FC0 File Offset: 0x000963C0
	private void OnPlayerJoined(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			player2.OnReviveEvent += this.OnRevive;
		}
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x00097FF0 File Offset: 0x000963F0
	private IEnumerator move_cr(AbstractPlayerController player, float xPosition)
	{
		if (player == null)
		{
			yield break;
		}
		yield return null;
		while (!player.gameObject.activeSelf)
		{
			yield return null;
		}
		LevelPlayerMotor playerMotor = null;
		LevelPlayerWeaponManager playerWeaponManager = null;
		if (player)
		{
			playerMotor = player.GetComponent<LevelPlayerMotor>();
			playerWeaponManager = player.GetComponent<LevelPlayerWeaponManager>();
			if (playerWeaponManager)
			{
				playerWeaponManager.DisableInput();
			}
			if (playerMotor)
			{
				while (playerMotor.Dashing)
				{
					yield return null;
				}
				playerMotor.DisableInput();
				yield return playerMotor.StartCoroutine(playerMotor.MoveToX_cr(xPosition, 1));
			}
			this.onEndedActionQueue.Add(delegate
			{
				this.StartCoroutine(this.ReactivateInputsCoroutine(playerMotor, null, playerWeaponManager, null, this.animatorOnEnd));
			});
		}
		yield break;
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00098019 File Offset: 0x00096419
	private void OnPlayerLeave(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
		}
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x00098024 File Offset: 0x00096424
	public void OnRevive(Vector3 pos)
	{
		if (this.conversationIsActive)
		{
			AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			base.StartCoroutine(this.move_cr(player, this.playerTwoDialoguePosition.x));
		}
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x0009805C File Offset: 0x0009645C
	private IEnumerator CutScene_cr()
	{
		if (this.speechBubble.displayState != SpeechBubble.DisplayState.Hidden)
		{
			yield break;
		}
		Coroutine playerOneMove = null;
		Coroutine playerTwoMove = null;
		this.conversationIsActive = true;
		AbstractPlayerController playerOne = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController playerTwo = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		playerOneMove = base.StartCoroutine(this.move_cr(playerOne, this.playerOneDialoguePosition.x));
		playerTwoMove = base.StartCoroutine(this.move_cr(playerTwo, this.playerTwoDialoguePosition.x));
		yield return playerOneMove;
		yield return playerTwoMove;
		if (this.animatorOnStart != null)
		{
			this.StartAnimation();
			while (!this.animatorOnStart.GetCurrentAnimatorStateInfo(0).IsName(this.animationOnStartTextName))
			{
				yield return null;
			}
		}
		Dialoguer.StartDialogue(this.dialogueInteraction);
		this.onEndedActionQueue.Add(delegate
		{
			if (this.animatorOnEnd != null)
			{
				this.EndAnimation();
			}
			playerOne = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			playerTwo = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			LevelPlayerMotor playerTwoMotor = null;
			LevelPlayerWeaponManager playerTwoWeaponManager = null;
			if (playerTwo != null)
			{
				playerTwoMotor = playerTwo.GetComponent<LevelPlayerMotor>();
				playerTwoWeaponManager = playerTwo.GetComponent<LevelPlayerWeaponManager>();
			}
			this.conversationIsActive = false;
			this.StartCoroutine(this.ReactivateInputsCoroutine(playerOne.GetComponent<LevelPlayerMotor>(), playerTwoMotor, playerOne.GetComponent<LevelPlayerWeaponManager>(), playerTwoWeaponManager, this.animatorOnEnd));
		});
		yield break;
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00098078 File Offset: 0x00096478
	protected virtual IEnumerator ReactivateInputsCoroutine(LevelPlayerMotor playerOneMotor, LevelPlayerMotor playerTwoMotor, LevelPlayerWeaponManager playerOneWeaponManager, LevelPlayerWeaponManager playerTwoWeaponManager, Animator animator)
	{
		if (animator != null)
		{
			if (this.animationOnGiveBackInputAtEnd != null && this.animationOnGiveBackInputAtEnd != string.Empty)
			{
				while (!animator.GetCurrentAnimatorStateInfo(0).IsName(this.animationOnGiveBackInput) && (!animator.GetCurrentAnimatorStateInfo(0).IsName(this.animationOnGiveBackInputAtEnd) || (double)animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.99))
				{
					yield return null;
				}
			}
			else
			{
				while (!animator.GetCurrentAnimatorStateInfo(0).IsName(this.animationOnGiveBackInput))
				{
					yield return null;
				}
			}
		}
		playerOneMotor.ClearBufferedInput();
		playerOneMotor.EnableInput();
		playerOneWeaponManager.EnableInput();
		if (playerTwoMotor)
		{
			playerTwoMotor.ClearBufferedInput();
			playerTwoMotor.EnableInput();
		}
		if (playerTwoWeaponManager)
		{
			playerTwoWeaponManager.EnableInput();
		}
		yield break;
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x000980B8 File Offset: 0x000964B8
	protected virtual void StartAnimation()
	{
		this.animatorOnStart.SetTrigger(this.animationTriggerOnStart);
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x000980CB File Offset: 0x000964CB
	protected virtual void EndAnimation()
	{
		this.animatorOnEnd.SetTrigger(this.animationTriggerOnEnd);
	}

	// Token: 0x04001870 RID: 6256
	[SerializeField]
	protected SpeechBubble speechBubble;

	// Token: 0x04001871 RID: 6257
	[SerializeField]
	public DialoguerDialogues dialogueInteraction;

	// Token: 0x04001872 RID: 6258
	[SerializeField]
	private Vector2 speechBubblePosition;

	// Token: 0x04001873 RID: 6259
	public Vector2 playerOneDialoguePosition;

	// Token: 0x04001874 RID: 6260
	public Vector2 playerTwoDialoguePosition;

	// Token: 0x04001875 RID: 6261
	public Animator animatorOnStart;

	// Token: 0x04001876 RID: 6262
	public string animationTriggerOnStart;

	// Token: 0x04001877 RID: 6263
	public string animationOnStartTextName;

	// Token: 0x04001878 RID: 6264
	public Animator animatorOnEnd;

	// Token: 0x04001879 RID: 6265
	public string animationTriggerOnEnd;

	// Token: 0x0400187A RID: 6266
	public string animationOnGiveBackInput;

	// Token: 0x0400187B RID: 6267
	public string animationOnGiveBackInputAtEnd;

	// Token: 0x0400187C RID: 6268
	private Coroutine cutsceneCoroutine;

	// Token: 0x0400187D RID: 6269
	private List<Action> onEndedActionQueue = new List<Action>();

	// Token: 0x0400187E RID: 6270
	[HideInInspector]
	public bool conversationIsActive;
}
