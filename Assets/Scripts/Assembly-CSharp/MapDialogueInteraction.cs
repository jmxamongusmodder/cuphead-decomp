using System;
using UnityEngine;
using System.Collections.Generic;

public class MapDialogueInteraction : AbstractMapInteractiveEntity
{
	[Serializable]
	public struct speechBubblePositionLanguage
	{
		public Localization.Languages languageApplied;
		public Vector2 speechBubblePosition;
	}

	[Serializable]
	public struct DEBUG_DialoguerCondition
	{
		public int ConditionId;
		public float Values;
	}

	[SerializeField]
	private SpeechBubble speechBubblePrefab;
	[SerializeField]
	private Vector2 speechBubblePosition;
	[SerializeField]
	private speechBubblePositionLanguage[] speechBubblePositions;
	[SerializeField]
	private Vector2 panCameraToPosition;
	[SerializeField]
	private int maxLines;
	[SerializeField]
	private bool tailOnTheLeft;
	[SerializeField]
	private bool hideTail;
	[SerializeField]
	private bool expandOnTheRight;
	public DialoguerDialogues dialogueInteraction;
	public bool disabledActivations;
	public List<MapDialogueInteraction.DEBUG_DialoguerCondition> DebugDialogerCondition;
	public bool currentlySpeaking;
}
