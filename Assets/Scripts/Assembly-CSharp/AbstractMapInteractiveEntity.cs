using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000925 RID: 2341
public abstract class AbstractMapInteractiveEntity : MapSprite
{
	// Token: 0x14000066 RID: 102
	// (add) Token: 0x060036BE RID: 14014 RVA: 0x00098964 File Offset: 0x00096D64
	// (remove) Token: 0x060036BF RID: 14015 RVA: 0x0009899C File Offset: 0x00096D9C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnActivateEvent;

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x060036C0 RID: 14016 RVA: 0x000989D2 File Offset: 0x00096DD2
	// (set) Token: 0x060036C1 RID: 14017 RVA: 0x000989DA File Offset: 0x00096DDA
	private protected AbstractMapInteractiveEntity.State state { protected get; private set; }

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x060036C2 RID: 14018 RVA: 0x000989E3 File Offset: 0x00096DE3
	// (set) Token: 0x060036C3 RID: 14019 RVA: 0x000989EB File Offset: 0x00096DEB
	private protected MapPlayerController playerActivating { protected get; private set; }

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x060036C4 RID: 14020 RVA: 0x000989F4 File Offset: 0x00096DF4
	// (set) Token: 0x060036C5 RID: 14021 RVA: 0x000989FC File Offset: 0x00096DFC
	private protected MapPlayerController playerChecking { protected get; private set; }

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x060036C6 RID: 14022 RVA: 0x00098A05 File Offset: 0x00096E05
	protected override bool ChangesDepth
	{
		get
		{
			return this.playerCanWalkBehind;
		}
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x00098A0D File Offset: 0x00096E0D
	protected override void Awake()
	{
		base.Awake();
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		this.lockInput = true;
		base.StartCoroutine(this.lock_input_cr());
	}

	// Token: 0x060036C8 RID: 14024 RVA: 0x00098A30 File Offset: 0x00096E30
	private IEnumerator lock_input_cr()
	{
		yield return new WaitForSeconds(1f);
		this.lockInput = false;
		yield return null;
		yield break;
	}

	// Token: 0x060036C9 RID: 14025 RVA: 0x00098A4C File Offset: 0x00096E4C
	protected override void Update()
	{
		base.Update();
		if (this.lockInput)
		{
			return;
		}
		if (InterruptingPrompt.IsInterrupting())
		{
			return;
		}
		this.Check();
		if (this.state == AbstractMapInteractiveEntity.State.Activated)
		{
			return;
		}
		if (MapConfirmStartUI.Current.CurrentState != AbstractMapSceneStartUI.State.Inactive || MapDifficultySelectStartUI.Current.CurrentState != AbstractMapSceneStartUI.State.Inactive || MapEventNotification.Current.showing || (Map.Current != null && Map.Current.CurrentState == Map.State.Graveyard) || SceneLoader.IsInBlurTransition)
		{
			return;
		}
		switch (this.interactor)
		{
		default:
			if (this.PlayerWithinDistance(0) && Map.Current.players[0].input.actions.GetButtonDown(13))
			{
				this.Activate(Map.Current.players[0]);
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Mugman:
			if (this.PlayerWithinDistance(1) && Map.Current.players[1].input.actions.GetButtonDown(13))
			{
				this.Activate(Map.Current.players[1]);
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Either:
			if (this.PlayerWithinDistance(0) && Map.Current.players[0].input.actions.GetButtonDown(13))
			{
				this.Activate(Map.Current.players[0]);
				return;
			}
			if (this.PlayerWithinDistance(1) && Map.Current.players[1].input.actions.GetButtonDown(13))
			{
				this.Activate(Map.Current.players[1]);
				return;
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Both:
			if (Map.Current.players[0] == null || Map.Current.players[1] == null)
			{
				return;
			}
			if (this.PlayerWithinDistance(0) && this.PlayerWithinDistance(1))
			{
				if (Map.Current.players[0].input.actions.GetButtonDown(13) && Map.Current.players[1].input.actions.GetButton(13))
				{
					this.Activate(Map.Current.players[0]);
					return;
				}
				if (Map.Current.players[1].input.actions.GetButtonDown(13) && Map.Current.players[0].input.actions.GetButton(13))
				{
					this.Activate(Map.Current.players[1]);
					return;
				}
			}
			break;
		}
	}

	// Token: 0x060036CA RID: 14026 RVA: 0x00098D10 File Offset: 0x00097110
	protected AbstractMapInteractiveEntity.MapActivateData PlayersAbleToActivate()
	{
		AbstractMapInteractiveEntity.MapActivateData result;
		result.length = 0;
		result.controller1 = null;
		result.controller2 = null;
		if (Map.Current.CurrentState != Map.State.Ready)
		{
			return result;
		}
		switch (this.interactor)
		{
		default:
			this.playerChecking = Map.Current.players[0];
			if (this.PlayerWithinDistance(0))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 1, this.playerChecking, null);
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Mugman:
			this.playerChecking = Map.Current.players[1];
			if (this.PlayerWithinDistance(1))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 1, this.playerChecking, null);
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Either:
			this.playerChecking = Map.Current.players[0];
			if (this.PlayerWithinDistance(0) && this.PlayerWithinDistance(1))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 2, Map.Current.players[0], Map.Current.players[1]);
			}
			if (this.PlayerWithinDistance(0))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 1, Map.Current.players[0], null);
			}
			this.playerChecking = Map.Current.players[1];
			if (this.PlayerWithinDistance(1))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 1, Map.Current.players[1], null);
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Both:
			this.playerChecking = Map.Current.players[0];
			if (this.PlayerWithinDistance(0) && this.PlayerWithinDistance(1))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 2, Map.Current.players[0], Map.Current.players[1]);
			}
			break;
		}
		return result;
	}

	// Token: 0x060036CB RID: 14027 RVA: 0x00098EC8 File Offset: 0x000972C8
	protected bool AbleToActivate()
	{
		return this.PlayersAbleToActivate().Length > 0;
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x00098EE8 File Offset: 0x000972E8
	public bool PlayerWithinDistance(int i)
	{
		if (Map.Current.players[i] == null || Map.Current.players[i].state != MapPlayerController.State.Walking || Map.Current.players[i].hideInteractionPrompts)
		{
			return false;
		}
		Vector2 a = base.transform.position + this.interactionPoint;
		Vector2 b = Map.Current.players[i].transform.position;
		return Vector2.Distance(a, b) <= this.interactionDistance;
	}

	// Token: 0x060036CD RID: 14029 RVA: 0x00098F84 File Offset: 0x00097384
	protected virtual void Check()
	{
		AbstractMapInteractiveEntity.MapActivateData mapActivateData = this.PlayersAbleToActivate();
		this.showed.CopyTo(this.checkPrevious, 0);
		for (int i = 0; i < this.showed.Length; i++)
		{
			this.showed[i] = false;
		}
		for (int j = 0; j < mapActivateData.Length; j++)
		{
			if (mapActivateData[j].id < (PlayerId)this.showed.Length)
			{
				this.showed[(int)mapActivateData[j].id] = true;
			}
		}
		for (int k = 0; k < Map.Current.players.Length; k++)
		{
			if (!(Map.Current.players[k] == null))
			{
				int id = (int)Map.Current.players[k].id;
				if (this.checkPrevious[id] != this.showed[id])
				{
					if (this.showed[id])
					{
						this.dialogues[id] = this.Show(Map.Current.players[k].input);
					}
					else
					{
						this.Hide(this.dialogues[id]);
						this.dialogues[id] = null;
					}
				}
			}
		}
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x000990C4 File Offset: 0x000974C4
	protected virtual void ReCheck()
	{
		AbstractMapInteractiveEntity.MapActivateData mapActivateData = this.PlayersAbleToActivate();
		this.CleanUpHiddenPrompts();
		this.showed.CopyTo(this.recheckPrevious, 0);
		for (int i = 0; i < this.showed.Length; i++)
		{
			this.showed[i] = false;
		}
		for (int j = 0; j < mapActivateData.Length; j++)
		{
			if (mapActivateData[j].id < (PlayerId)this.showed.Length)
			{
				this.showed[(int)mapActivateData[j].id] = true;
			}
		}
		for (int k = 0; k < Map.Current.players.Length; k++)
		{
			if (!(Map.Current.players[k] == null))
			{
				int id = (int)Map.Current.players[k].id;
				if (this.recheckPrevious[id] != this.showed[id])
				{
					if (this.showed[id])
					{
						this.dialogues[id] = this.Show(Map.Current.players[k].input);
					}
					else
					{
						this.Hide(this.dialogues[id]);
						this.dialogues[id] = null;
					}
				}
			}
		}
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x00099208 File Offset: 0x00097608
	public virtual void CleanUpHiddenPrompts()
	{
		for (int i = 0; i < this.showed.Length; i++)
		{
			if (this.showed[i] && this.dialogues[i] == null)
			{
				this.showed[i] = false;
			}
		}
	}

	// Token: 0x060036D0 RID: 14032 RVA: 0x00099258 File Offset: 0x00097658
	protected virtual void Activate(MapPlayerController player)
	{
		MapUIInteractionDialogue mapUIInteractionDialogue = this.dialogues[(int)player.id];
		if (mapUIInteractionDialogue == null)
		{
			return;
		}
		this.playerActivating = player;
		mapUIInteractionDialogue.Close();
		this.state = AbstractMapInteractiveEntity.State.Activated;
		if (this.OnActivateEvent != null)
		{
			this.OnActivateEvent();
		}
		this.Activate();
	}

	// Token: 0x060036D1 RID: 14033 RVA: 0x000992B2 File Offset: 0x000976B2
	protected virtual void Activate()
	{
	}

	// Token: 0x060036D2 RID: 14034 RVA: 0x000992B4 File Offset: 0x000976B4
	protected virtual MapUIInteractionDialogue Show(PlayerInput player)
	{
		AudioManager.Play("world_map_level_bubble_appear");
		this.state = AbstractMapInteractiveEntity.State.Ready;
		return MapUIInteractionDialogue.Create(this.dialogueProperties, player, this.dialogueOffset);
	}

	// Token: 0x060036D3 RID: 14035 RVA: 0x000992D9 File Offset: 0x000976D9
	public virtual void Hide(MapUIInteractionDialogue dialogue)
	{
		AudioManager.Play("world_map_level_bubble_disappear");
		if (dialogue == null)
		{
			return;
		}
		dialogue.Close();
		dialogue = null;
		this.state = AbstractMapInteractiveEntity.State.Inactive;
	}

	// Token: 0x060036D4 RID: 14036 RVA: 0x00099304 File Offset: 0x00097704
	public void SetPlayerReturnPos()
	{
		PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.playerOne;
		PlayerData.Data.CurrentMapData.playerTwoPosition = base.transform.position + this.returnPositions.playerTwo;
		if (!PlayerManager.Multiplayer)
		{
			PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.singlePlayer;
		}
	}

	// Token: 0x060036D5 RID: 14037 RVA: 0x000993B7 File Offset: 0x000977B7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		AbstractMapInteractiveEntity.HasPopupOpened = false;
	}

	// Token: 0x060036D6 RID: 14038 RVA: 0x000993C8 File Offset: 0x000977C8
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.dialogueOffset, 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.dialogueOffset, 0.06f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, 0.06f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, this.interactionDistance);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, this.interactionDistance + 0.01f);
		Vector3 vector = new Vector3(0.3f, 0.3f, 0.3f);
		Vector3 size = vector * 0.9f;
		Vector3 vector2 = new Vector3(0.25f, 0.25f, 0.25f);
		Vector3 size2 = vector2 * 0.9f;
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(this.returnPositions.singlePlayer + base.transform.position, vector);
		Gizmos.color = Color.black;
		Gizmos.DrawWireCube(this.returnPositions.playerOne + base.transform.position, vector2);
		Gizmos.DrawWireCube(this.returnPositions.playerTwo + base.transform.position, vector2);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(this.returnPositions.singlePlayer + base.transform.position, size);
		Gizmos.DrawWireCube(this.returnPositions.playerOne + base.transform.position, size2);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(this.returnPositions.playerTwo + base.transform.position, size2);
		Gizmos.color = Color.white;
	}

	// Token: 0x060036D7 RID: 14039 RVA: 0x00099678 File Offset: 0x00097A78
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (!Application.isPlaying)
		{
			return;
		}
		switch (this.interactor)
		{
		case AbstractMapInteractiveEntity.Interactor.Cuphead:
			this.DrawGizmoLineToPlayer(0, this.PlayerWithinDistance(0));
			break;
		case AbstractMapInteractiveEntity.Interactor.Mugman:
			this.DrawGizmoLineToPlayer(1, this.PlayerWithinDistance(1));
			break;
		case AbstractMapInteractiveEntity.Interactor.Either:
			this.DrawGizmoLineToPlayer(0, this.PlayerWithinDistance(0));
			this.DrawGizmoLineToPlayer(1, this.PlayerWithinDistance(1));
			break;
		case AbstractMapInteractiveEntity.Interactor.Both:
			this.DrawGizmoLineToPlayer(0, this.PlayerWithinDistance(0) && this.PlayerWithinDistance(1));
			this.DrawGizmoLineToPlayer(1, this.PlayerWithinDistance(0) && this.PlayerWithinDistance(1));
			break;
		}
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x00099740 File Offset: 0x00097B40
	private void DrawGizmoLineToPlayer(int i, bool valid)
	{
		if (Map.Current.players[i] == null)
		{
			return;
		}
		Gizmos.color = ((!valid) ? Color.red : Color.green);
		Gizmos.DrawLine(base.transform.position + this.interactionPoint, Map.Current.players[i].transform.position);
	}

	// Token: 0x04003EE8 RID: 16104
	protected const string MapWorld1 = "MapWorld_1";

	// Token: 0x04003EE9 RID: 16105
	protected const string MapWorld2 = "MapWorld_2";

	// Token: 0x04003EEA RID: 16106
	protected const string MapWorld3 = "MapWorld_3";

	// Token: 0x04003EEB RID: 16107
	protected const string MapWorld4Exit = "KingDiceToWorld3WorldMap";

	// Token: 0x04003EEC RID: 16108
	protected const string Inkwell = "Inkwell";

	// Token: 0x04003EED RID: 16109
	protected const string Mausoleum = "Mausoleum";

	// Token: 0x04003EEE RID: 16110
	protected const string Mausoleum1 = "Mausoleum_1";

	// Token: 0x04003EEF RID: 16111
	protected const string Mausoleum2 = "Mausoleum_2";

	// Token: 0x04003EF0 RID: 16112
	protected const string Mausoleum3 = "Mausoleum_3";

	// Token: 0x04003EF1 RID: 16113
	protected const string Devil = "Devil";

	// Token: 0x04003EF2 RID: 16114
	protected const string DicePalaceMain = "DicePalaceMain";

	// Token: 0x04003EF3 RID: 16115
	protected const string KingDice = "KingDice";

	// Token: 0x04003EF4 RID: 16116
	protected const string Shop = "Shop";

	// Token: 0x04003EF5 RID: 16117
	protected const string ElderKettleLevel = "ElderKettleLevel";

	// Token: 0x04003EF6 RID: 16118
	protected const string Kitchen = "BakeryWorldMap";

	// Token: 0x04003EF7 RID: 16119
	protected const string KitchenFight = "Saltbaker";

	// Token: 0x04003EF8 RID: 16120
	protected const string KingOfGamesCastle = "KingOfGamesWorldMap";

	// Token: 0x04003EF9 RID: 16121
	protected static bool HasPopupOpened;

	// Token: 0x04003EFB RID: 16123
	public AbstractMapInteractiveEntity.Interactor interactor = AbstractMapInteractiveEntity.Interactor.Either;

	// Token: 0x04003EFC RID: 16124
	public Vector2 interactionPoint;

	// Token: 0x04003EFD RID: 16125
	public float interactionDistance = 1f;

	// Token: 0x04003EFE RID: 16126
	public AbstractUIInteractionDialogue.Properties dialogueProperties;

	// Token: 0x04003EFF RID: 16127
	public Vector2 dialogueOffset;

	// Token: 0x04003F00 RID: 16128
	public AbstractMapInteractiveEntity.PositionProperties returnPositions;

	// Token: 0x04003F01 RID: 16129
	public bool playerCanWalkBehind = true;

	// Token: 0x04003F05 RID: 16133
	[HideInInspector]
	public MapUIInteractionDialogue[] dialogues = new MapUIInteractionDialogue[2];

	// Token: 0x04003F06 RID: 16134
	private bool lastInteractable;

	// Token: 0x04003F07 RID: 16135
	private bool lockInput;

	// Token: 0x04003F08 RID: 16136
	private bool[] showed = new bool[2];

	// Token: 0x04003F09 RID: 16137
	private bool[] checkPrevious = new bool[2];

	// Token: 0x04003F0A RID: 16138
	private bool[] recheckPrevious = new bool[2];

	// Token: 0x02000926 RID: 2342
	public enum Interactor
	{
		// Token: 0x04003F0C RID: 16140
		Cuphead,
		// Token: 0x04003F0D RID: 16141
		Mugman,
		// Token: 0x04003F0E RID: 16142
		Either,
		// Token: 0x04003F0F RID: 16143
		Both
	}

	// Token: 0x02000927 RID: 2343
	protected enum State
	{
		// Token: 0x04003F11 RID: 16145
		Inactive,
		// Token: 0x04003F12 RID: 16146
		Ready,
		// Token: 0x04003F13 RID: 16147
		Activated
	}

	// Token: 0x02000928 RID: 2344
	protected struct MapActivateData
	{
		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x060036D9 RID: 14041 RVA: 0x000997BA File Offset: 0x00097BBA
		public int Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x1700047D RID: 1149
		public MapPlayerController this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this.controller1;
				}
				if (index == 1)
				{
					return this.controller2;
				}
				return null;
			}
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x000997E0 File Offset: 0x00097BE0
		public static AbstractMapInteractiveEntity.MapActivateData Fill(ref AbstractMapInteractiveEntity.MapActivateData mapActivateData, int length, MapPlayerController controller1 = null, MapPlayerController controller2 = null)
		{
			mapActivateData.length = length;
			if (length >= 1)
			{
				mapActivateData.controller1 = controller1;
			}
			if (length >= 2)
			{
				mapActivateData.controller2 = controller2;
			}
			return mapActivateData;
		}

		// Token: 0x04003F14 RID: 16148
		public int length;

		// Token: 0x04003F15 RID: 16149
		public MapPlayerController controller1;

		// Token: 0x04003F16 RID: 16150
		public MapPlayerController controller2;
	}

	// Token: 0x02000929 RID: 2345
	[Serializable]
	public class PositionProperties
	{
		// Token: 0x04003F17 RID: 16151
		[Header("One Player")]
		public Vector2 singlePlayer = new Vector2(0f, -1f);

		// Token: 0x04003F18 RID: 16152
		[Header("Two Players")]
		public Vector2 playerOne = new Vector2(-1f, -1f);

		// Token: 0x04003F19 RID: 16153
		public Vector2 playerTwo = new Vector2(1f, -1f);
	}
}
