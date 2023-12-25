using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000476 RID: 1142
public abstract class AbstractLevelInteractiveEntity : AbstractPausableComponent
{
	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06001184 RID: 4484 RVA: 0x00097354 File Offset: 0x00095754
	// (remove) Token: 0x06001185 RID: 4485 RVA: 0x0009738C File Offset: 0x0009578C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnActivateEvent;

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06001186 RID: 4486 RVA: 0x000973C2 File Offset: 0x000957C2
	// (set) Token: 0x06001187 RID: 4487 RVA: 0x000973CA File Offset: 0x000957CA
	protected AbstractLevelInteractiveEntity.State state { get; set; }

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06001188 RID: 4488 RVA: 0x000973D3 File Offset: 0x000957D3
	// (set) Token: 0x06001189 RID: 4489 RVA: 0x000973DB File Offset: 0x000957DB
	private protected AbstractPlayerController playerActivating { protected get; private set; }

	// Token: 0x0600118A RID: 4490 RVA: 0x000973E4 File Offset: 0x000957E4
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x000973EC File Offset: 0x000957EC
	private void Start()
	{
		Localization.OnLanguageChangedEvent += this.OnLanguageChanged;
	}

	// Token: 0x0600118C RID: 4492 RVA: 0x000973FF File Offset: 0x000957FF
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Localization.OnLanguageChangedEvent -= this.OnLanguageChanged;
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x00097418 File Offset: 0x00095818
	private void OnLanguageChanged()
	{
		this.Hide(PlayerId.PlayerOne);
		this.Hide(PlayerId.PlayerTwo);
		this.lastInteractable = !this.lastInteractable;
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x00097438 File Offset: 0x00095838
	private void FixedUpdate()
	{
		this.Check();
		if (this.state == AbstractLevelInteractiveEntity.State.Activated)
		{
			return;
		}
		switch (this.interactor)
		{
		default:
			if (this.PlayerWithinDistance(PlayerId.PlayerOne) && PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButtonDown(13) && !this.PlayerIsDashing(PlayerId.PlayerOne))
			{
				this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerOne));
			}
			break;
		case AbstractLevelInteractiveEntity.Interactor.Mugman:
			if (this.PlayerWithinDistance(PlayerId.PlayerTwo) && PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButtonDown(13) && !this.PlayerIsDashing(PlayerId.PlayerTwo))
			{
				this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerTwo));
			}
			break;
		case AbstractLevelInteractiveEntity.Interactor.Either:
			if (this.PlayerWithinDistance(PlayerId.PlayerOne) || this.PlayerWithinDistance(PlayerId.PlayerTwo))
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButtonDown(13) && this.PlayerWithinDistance(PlayerId.PlayerOne) && !this.PlayerIsDashing(PlayerId.PlayerOne))
				{
					this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerOne));
					return;
				}
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) == null)
				{
					return;
				}
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButtonDown(13) && this.PlayerWithinDistance(PlayerId.PlayerTwo) && !this.PlayerIsDashing(PlayerId.PlayerTwo))
				{
					this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerTwo));
					return;
				}
			}
			break;
		case AbstractLevelInteractiveEntity.Interactor.Both:
			if (PlayerManager.GetPlayer(PlayerId.PlayerOne) == null || PlayerManager.GetPlayer(PlayerId.PlayerTwo) == null)
			{
				return;
			}
			if (this.PlayerWithinDistance(PlayerId.PlayerOne) && this.PlayerWithinDistance(PlayerId.PlayerTwo))
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButtonDown(13) && this.PlayerWithinDistance(PlayerId.PlayerOne) && PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButton(13) && this.PlayerWithinDistance(PlayerId.PlayerTwo) && !this.PlayerIsDashing(PlayerId.PlayerOne) && !this.PlayerIsDashing(PlayerId.PlayerTwo))
				{
					this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerOne));
					return;
				}
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButtonDown(13) && this.PlayerWithinDistance(PlayerId.PlayerTwo) && PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButton(13) && this.PlayerWithinDistance(PlayerId.PlayerOne) && !this.PlayerIsDashing(PlayerId.PlayerOne) && !this.PlayerIsDashing(PlayerId.PlayerTwo))
				{
					this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerTwo));
					return;
				}
			}
			break;
		}
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x000976E0 File Offset: 0x00095AE0
	protected bool AbleToActivate()
	{
		switch (this.interactor)
		{
		default:
			return this.PlayerWithinDistance(PlayerId.PlayerOne);
		case AbstractLevelInteractiveEntity.Interactor.Mugman:
			return this.PlayerWithinDistance(PlayerId.PlayerTwo);
		case AbstractLevelInteractiveEntity.Interactor.Either:
			return this.PlayerWithinDistance(PlayerId.PlayerOne) || this.PlayerWithinDistance(PlayerId.PlayerTwo);
		case AbstractLevelInteractiveEntity.Interactor.Both:
			return this.PlayerWithinDistance(PlayerId.PlayerOne) && this.PlayerWithinDistance(PlayerId.PlayerTwo);
		}
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x00097768 File Offset: 0x00095B68
	protected bool PlayerWithinDistance(PlayerId id)
	{
		if (PlayerManager.GetPlayer(id) == null)
		{
			return false;
		}
		Vector2 a = base.transform.position + this.interactionPoint;
		Vector2 b = PlayerManager.GetPlayer(id).transform.position;
		return Vector2.Distance(a, b) <= this.interactionDistance;
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x000977CC File Offset: 0x00095BCC
	protected bool PlayerIsDashing(PlayerId id)
	{
		if (PlayerManager.GetPlayer(id) == null)
		{
			return false;
		}
		if (PlayerManager.GetPlayer(id).GetComponent<LevelPlayerMotor>() != null)
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)PlayerManager.GetPlayer(id);
			return levelPlayerController.motor.Dashing;
		}
		return false;
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x0009781C File Offset: 0x00095C1C
	protected virtual void Check()
	{
		bool flag = this.AbleToActivate();
		if (flag != this.lastInteractable)
		{
			if (flag)
			{
				if (this.PlayerWithinDistance(PlayerId.PlayerOne))
				{
					this.Show(PlayerId.PlayerOne);
				}
				else if (this.PlayerWithinDistance(PlayerId.PlayerTwo) && PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					this.Show(PlayerId.PlayerTwo);
				}
			}
			else if (!this.PlayerWithinDistance(PlayerId.PlayerOne))
			{
				this.Hide(PlayerId.PlayerOne);
			}
			else if (!this.PlayerWithinDistance(PlayerId.PlayerTwo) && PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
			{
				this.Hide(PlayerId.PlayerTwo);
			}
		}
		this.lastInteractable = flag;
	}

	// Token: 0x06001193 RID: 4499 RVA: 0x000978C8 File Offset: 0x00095CC8
	private void Activate(AbstractPlayerController player)
	{
		if (this.dialogue == null)
		{
			return;
		}
		this.playerActivating = player;
		this.dialogue.Close();
		this.dialogue = null;
		this.state = AbstractLevelInteractiveEntity.State.Activated;
		if (this.OnActivateEvent != null)
		{
			this.OnActivateEvent();
		}
		this.Activate();
	}

	// Token: 0x06001194 RID: 4500 RVA: 0x00097923 File Offset: 0x00095D23
	protected virtual void Activate()
	{
	}

	// Token: 0x06001195 RID: 4501 RVA: 0x00097928 File Offset: 0x00095D28
	protected virtual void Show(PlayerId playerId)
	{
		this.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogueProperties.text = string.Empty;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Bottom, this.hasTarget);
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x0009797A File Offset: 0x00095D7A
	protected virtual void Hide(PlayerId playerId)
	{
		if (this.dialogue == null)
		{
			return;
		}
		this.dialogue.Close();
		this.dialogue = null;
		this.state = AbstractLevelInteractiveEntity.State.Inactive;
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x000979A8 File Offset: 0x00095DA8
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.dialogueOffset, Mathf.Min(5f, this.interactionDistance));
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.dialogueOffset, Mathf.Min(6f, this.interactionDistance + 1f));
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, this.interactionDistance);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, this.interactionDistance + 1f);
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x00097AB0 File Offset: 0x00095EB0
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (!Application.isPlaying)
		{
			return;
		}
		switch (this.interactor)
		{
		case AbstractLevelInteractiveEntity.Interactor.Cuphead:
			this.DrawGizmoLineToPlayer(PlayerId.PlayerOne, this.PlayerWithinDistance(PlayerId.PlayerOne));
			break;
		case AbstractLevelInteractiveEntity.Interactor.Mugman:
			this.DrawGizmoLineToPlayer(PlayerId.PlayerTwo, this.PlayerWithinDistance(PlayerId.PlayerTwo));
			break;
		case AbstractLevelInteractiveEntity.Interactor.Either:
			this.DrawGizmoLineToPlayer(PlayerId.PlayerOne, this.PlayerWithinDistance(PlayerId.PlayerOne));
			this.DrawGizmoLineToPlayer(PlayerId.PlayerTwo, this.PlayerWithinDistance(PlayerId.PlayerTwo));
			break;
		case AbstractLevelInteractiveEntity.Interactor.Both:
			this.DrawGizmoLineToPlayer(PlayerId.PlayerOne, this.PlayerWithinDistance(PlayerId.PlayerOne) && this.PlayerWithinDistance(PlayerId.PlayerTwo));
			this.DrawGizmoLineToPlayer(PlayerId.PlayerTwo, this.PlayerWithinDistance(PlayerId.PlayerOne) && this.PlayerWithinDistance(PlayerId.PlayerTwo));
			break;
		}
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x00097B78 File Offset: 0x00095F78
	private void DrawGizmoLineToPlayer(PlayerId id, bool valid)
	{
		if (PlayerManager.GetPlayer(id) == null)
		{
			return;
		}
		Gizmos.color = ((!valid) ? Color.red : Color.green);
		Gizmos.DrawLine(base.transform.position + this.interactionPoint, PlayerManager.GetPlayer(id).transform.position);
	}

	// Token: 0x04001B14 RID: 6932
	public AbstractLevelInteractiveEntity.Interactor interactor = AbstractLevelInteractiveEntity.Interactor.Either;

	// Token: 0x04001B15 RID: 6933
	public Vector2 interactionPoint;

	// Token: 0x04001B16 RID: 6934
	public float interactionDistance = 100f;

	// Token: 0x04001B17 RID: 6935
	public AbstractUIInteractionDialogue.Properties dialogueProperties;

	// Token: 0x04001B18 RID: 6936
	public Vector2 dialogueOffset;

	// Token: 0x04001B19 RID: 6937
	public bool once = true;

	// Token: 0x04001B1A RID: 6938
	public bool hasTarget = true;

	// Token: 0x04001B1D RID: 6941
	protected LevelUIInteractionDialogue dialogue;

	// Token: 0x04001B1E RID: 6942
	private bool lastInteractable;

	// Token: 0x02000477 RID: 1143
	public enum Interactor
	{
		// Token: 0x04001B20 RID: 6944
		Cuphead,
		// Token: 0x04001B21 RID: 6945
		Mugman,
		// Token: 0x04001B22 RID: 6946
		Either,
		// Token: 0x04001B23 RID: 6947
		Both
	}

	// Token: 0x02000478 RID: 1144
	protected enum State
	{
		// Token: 0x04001B25 RID: 6949
		Inactive,
		// Token: 0x04001B26 RID: 6950
		Ready,
		// Token: 0x04001B27 RID: 6951
		Activated
	}
}
