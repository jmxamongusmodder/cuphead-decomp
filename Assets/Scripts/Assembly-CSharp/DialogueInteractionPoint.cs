using UnityEngine;

public class DialogueInteractionPoint : SpeechInteractionPoint
{
	[SerializeField]
	protected SpeechBubble speechBubble;
	[SerializeField]
	public DialoguerDialogues dialogueInteraction;
	[SerializeField]
	private Vector2 speechBubblePosition;
	public Vector2 playerOneDialoguePosition;
	public Vector2 playerTwoDialoguePosition;
	public Animator animatorOnStart;
	public string animationTriggerOnStart;
	public string animationOnStartTextName;
	public Animator animatorOnEnd;
	public string animationTriggerOnEnd;
	public string animationOnGiveBackInput;
	public string animationOnGiveBackInputAtEnd;
	public bool conversationIsActive;
}
