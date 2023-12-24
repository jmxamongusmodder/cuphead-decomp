using UnityEngine;

public class AbstractLevelInteractiveEntity : AbstractPausableComponent
{
	public enum Interactor
	{
		Cuphead = 0,
		Mugman = 1,
		Either = 2,
		Both = 3,
	}

	public Interactor interactor;
	public Vector2 interactionPoint;
	public float interactionDistance;
	public AbstractUIInteractionDialogue.Properties dialogueProperties;
	public Vector2 dialogueOffset;
	public bool once;
	public bool hasTarget;
}
