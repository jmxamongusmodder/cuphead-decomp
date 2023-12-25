using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000427 RID: 1063
public class MapDialogueInteraction : AbstractMapInteractiveEntity
{
	// Token: 0x06000F6A RID: 3946 RVA: 0x00099930 File Offset: 0x00097D30
	protected virtual void Start()
	{
		if (this.speechBubble == null)
		{
			Vector3 vector = base.transform.position;
			if (this.speechBubblePositions != null)
			{
				vector = this.ApplyCustonMargin(vector);
			}
			else
			{
				vector.x += this.speechBubblePosition.x;
				vector.y += this.speechBubblePosition.y;
			}
			this.speechBubble = SpeechBubble.Instance;
			if (this.speechBubble == null)
			{
				this.speechBubble = UnityEngine.Object.Instantiate<GameObject>(this.speechBubblePrefab.gameObject, vector, Quaternion.identity, MapUI.Current.sceneCanvas.transform).GetComponent<SpeechBubble>();
			}
		}
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x00099A1C File Offset: 0x00097E1C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueEndedHandler;
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x00099A50 File Offset: 0x00097E50
	private void OnDialogueEndedHandler()
	{
		this.IsDialogueEnded = true;
		this.Check();
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x00099A60 File Offset: 0x00097E60
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Vector3 vector = base.transform.position;
		if (this.speechBubblePositions != null)
		{
			vector = this.ApplyCustonMargin(vector);
		}
		else
		{
			vector.x += this.speechBubblePosition.x;
			vector.y += this.speechBubblePosition.y;
		}
		Gizmos.DrawWireSphere(vector, this.interactionDistance * 0.5f);
		vector = base.transform.position;
		vector.x += this.panCameraToPosition.x;
		vector.y += this.panCameraToPosition.y;
		Gizmos.DrawWireSphere(vector, this.interactionDistance * 0.5f);
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x00099B2C File Offset: 0x00097F2C
	protected override void Activate(MapPlayerController player)
	{
		if (this.dialogues[(int)player.id] != null && this.dialogues[(int)player.id].transform.localScale.x == 1f && MapBasicStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapDifficultySelectStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapConfirmStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && (Map.Current == null || Map.Current.CurrentState != Map.State.Graveyard))
		{
			base.Activate(player);
			this.StartSpeechBubble();
		}
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x00099BD4 File Offset: 0x00097FD4
	protected virtual void StartSpeechBubble()
	{
		if (this.speechBubble.displayState == SpeechBubble.DisplayState.Hidden)
		{
			Vector3 vector = base.transform.position;
			if (this.speechBubblePositions != null)
			{
				vector = this.ApplyCustonMargin(vector);
			}
			else
			{
				vector.x += this.speechBubblePosition.x;
				vector.y += this.speechBubblePosition.y;
			}
			this.speechBubble.basePosition = vector;
			vector = base.transform.position;
			vector.x += this.panCameraToPosition.x;
			vector.y += this.panCameraToPosition.y;
			this.speechBubble.panPosition = vector;
			this.speechBubble.maxLines = this.maxLines;
			this.speechBubble.tailOnTheLeft = this.tailOnTheLeft;
			this.speechBubble.expandOnTheRight = this.expandOnTheRight;
			this.speechBubble.hideTail = this.hideTail;
			if (this.cutsceneCoroutine != null)
			{
				base.StopCoroutine(this.cutsceneCoroutine);
			}
			this.cutsceneCoroutine = base.StartCoroutine(this.CutScene_cr());
		}
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x00099D14 File Offset: 0x00098114
	protected override void Check()
	{
		if (this.disabledActivations)
		{
			return;
		}
		base.Check();
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x00099D28 File Offset: 0x00098128
	private IEnumerator CutScene_cr()
	{
		if (this.speechBubble.displayState != SpeechBubble.DisplayState.Hidden)
		{
			yield break;
		}
		for (int i = 0; i < Map.Current.players.Length; i++)
		{
			if (!(Map.Current.players[i] == null))
			{
				Map.Current.players[i].Disable();
			}
		}
		yield return null;
		this.currentlySpeaking = true;
		Dialoguer.StartDialogue(this.dialogueInteraction);
		DialoguerEvents.EndedHandler afterDialogue = null;
		afterDialogue = delegate()
		{
			Dialoguer.events.onEnded -= afterDialogue;
			this.StartCoroutine(this.reactivate_input_cr());
		};
		Dialoguer.events.onEnded += afterDialogue;
		yield break;
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x00099D44 File Offset: 0x00098144
	private IEnumerator reactivate_input_cr()
	{
		if (CupheadMapCamera.Current != null)
		{
			CupheadMapCamera.Current.SetActiveCollider(false);
		}
		while (CupheadMapCamera.Current != null && CupheadMapCamera.Current.IsCameraFarFromPlayer())
		{
			yield return null;
		}
		if (CupheadMapCamera.Current != null)
		{
			CupheadMapCamera.Current.SetActiveCollider(true);
		}
		for (int i = 0; i < Map.Current.players.Length; i++)
		{
			if (!(Map.Current.players[i] == null))
			{
				Map.Current.players[i].Enable();
			}
		}
		this.currentlySpeaking = false;
		yield break;
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x00099D60 File Offset: 0x00098160
	private Vector3 ApplyCustonMargin(Vector3 pos)
	{
		int num = 0;
		bool flag = false;
		while (!flag && num < this.speechBubblePositions.Length)
		{
			flag = (this.speechBubblePositions[num].languageApplied == Localization.language);
			num = ((!flag) ? (num + 1) : num);
		}
		if (flag)
		{
			MapDialogueInteraction.speechBubblePositionLanguage speechBubblePositionLanguage = this.speechBubblePositions[num];
			pos.x += speechBubblePositionLanguage.speechBubblePosition.x;
			pos.y += speechBubblePositionLanguage.speechBubblePosition.y;
		}
		else
		{
			pos.x += this.speechBubblePosition.x;
			pos.y += this.speechBubblePosition.y;
		}
		return pos;
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x00099E38 File Offset: 0x00098238
	public IEnumerator DebugStartDialogue()
	{
		yield return base.StartCoroutine(this.debugActivate_input_cr());
		yield break;
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x00099E54 File Offset: 0x00098254
	private IEnumerator debugActivate_input_cr()
	{
		bool DoneLanguage = false;
		int index = 0;
		WaitForSeconds Wait = new WaitForSeconds(0.3f);
		Localization.Languages startedLanguage = Localization.language;
		int ConditionIndex = 0;
		bool DoneVariables = false;
		yield return Wait;
		yield return Wait;
		while (!DoneLanguage)
		{
			DoneVariables = false;
			ConditionIndex = 0;
			while (!DoneVariables)
			{
				if (this.DebugDialogerCondition.Count > 0)
				{
					Dialoguer.SetGlobalFloat(this.DebugDialogerCondition[ConditionIndex].ConditionId, this.DebugDialogerCondition[ConditionIndex].Values);
				}
				this.IsDialogueEnded = false;
				index = 0;
				yield return Wait;
				yield return Wait;
				this.Activate(Map.Current.players[0]);
				yield return Wait;
				while (!this.IsDialogueEnded)
				{
					string ConditionKey = string.Empty;
					if (this.DebugDialogerCondition.Count > 0)
					{
						ConditionKey = string.Concat(new object[]
						{
							"_",
							this.DebugDialogerCondition[ConditionIndex].ConditionId,
							"_",
							this.DebugDialogerCondition[ConditionIndex].Values
						});
					}
					ScreenshotHandler.cameraType camera = ScreenshotHandler.cameraType.Map;
					string folderName = "LOC_Screenshot";
					object[] array = new object[6];
					array[0] = this.dialogueInteraction.ToString();
					array[1] = ConditionKey;
					array[2] = "_";
					array[3] = Localization.language;
					array[4] = "_";
					int num = 5;
					int num2;
					index = (num2 = index) + 1;
					array[num] = num2;
					ScreenshotHandler.TakeScreenshot_Static(camera, folderName, string.Concat(array));
					yield return Wait;
					Dialoguer.ContinueDialogue();
					yield return Wait;
				}
				ConditionIndex++;
				if (ConditionIndex >= this.DebugDialogerCondition.Count)
				{
					DoneVariables = true;
				}
			}
			yield return null;
			yield return null;
			yield return null;
			if (Localization.language == startedLanguage)
			{
				DoneLanguage = true;
			}
		}
		yield break;
	}

	// Token: 0x0400187F RID: 6271
	[SerializeField]
	private SpeechBubble speechBubblePrefab;

	// Token: 0x04001880 RID: 6272
	[SerializeField]
	private Vector2 speechBubblePosition;

	// Token: 0x04001881 RID: 6273
	[SerializeField]
	private MapDialogueInteraction.speechBubblePositionLanguage[] speechBubblePositions;

	// Token: 0x04001882 RID: 6274
	[SerializeField]
	private Vector2 panCameraToPosition;

	// Token: 0x04001883 RID: 6275
	[SerializeField]
	private int maxLines = -1;

	// Token: 0x04001884 RID: 6276
	[SerializeField]
	private bool tailOnTheLeft;

	// Token: 0x04001885 RID: 6277
	[SerializeField]
	private bool hideTail;

	// Token: 0x04001886 RID: 6278
	[SerializeField]
	private bool expandOnTheRight;

	// Token: 0x04001887 RID: 6279
	protected SpeechBubble speechBubble;

	// Token: 0x04001888 RID: 6280
	public DialoguerDialogues dialogueInteraction;

	// Token: 0x04001889 RID: 6281
	private Coroutine cutsceneCoroutine;

	// Token: 0x0400188A RID: 6282
	[HideInInspector]
	public bool disabledActivations;

	// Token: 0x0400188B RID: 6283
	[Header("DEBUG")]
	public List<MapDialogueInteraction.DEBUG_DialoguerCondition> DebugDialogerCondition = new List<MapDialogueInteraction.DEBUG_DialoguerCondition>();

	// Token: 0x0400188C RID: 6284
	private bool IsDialogueEnded;

	// Token: 0x0400188D RID: 6285
	public bool currentlySpeaking;

	// Token: 0x02000428 RID: 1064
	[Serializable]
	public struct speechBubblePositionLanguage
	{
		// Token: 0x0400188E RID: 6286
		public Localization.Languages languageApplied;

		// Token: 0x0400188F RID: 6287
		public Vector2 speechBubblePosition;
	}

	// Token: 0x02000429 RID: 1065
	[Serializable]
	public struct DEBUG_DialoguerCondition
	{
		// Token: 0x04001890 RID: 6288
		public int ConditionId;

		// Token: 0x04001891 RID: 6289
		public float Values;
	}
}
