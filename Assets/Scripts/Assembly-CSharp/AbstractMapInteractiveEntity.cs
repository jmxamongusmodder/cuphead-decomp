using System;
using UnityEngine;

public class AbstractMapInteractiveEntity : MapSprite
{
	[Serializable]
	public class PositionProperties
	{
		public Vector2 singlePlayer;
		public Vector2 playerOne;
		public Vector2 playerTwo;
	}

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
	public PositionProperties returnPositions;
	public bool playerCanWalkBehind;
	public MapUIInteractionDialogue[] dialogues;
}
