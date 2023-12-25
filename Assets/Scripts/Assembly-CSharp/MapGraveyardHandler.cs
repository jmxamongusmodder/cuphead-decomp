using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x02000939 RID: 2361
public class MapGraveyardHandler : MapDialogueInteraction
{
	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06003736 RID: 14134 RVA: 0x001FCAC2 File Offset: 0x001FAEC2
	// (set) Token: 0x06003737 RID: 14135 RVA: 0x001FCACA File Offset: 0x001FAECA
	public bool canReenter { get; private set; }

	// Token: 0x06003738 RID: 14136 RVA: 0x001FCAD4 File Offset: 0x001FAED4
	protected override void Start()
	{
		this.extantGhosts = new Animator[2];
		if (!PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Charm.charm_curse) && !PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Charm.charm_curse))
		{
			foreach (MapGraveyardGrave mapGraveyardGrave in this.grave)
			{
				mapGraveyardGrave.SetInteractable(false);
			}
			base.gameObject.SetActive(false);
			return;
		}
		base.Start();
		this.puzzleOrder = PlayerData.Data.curseCharmPuzzleOrder;
		this.AddDialoguerEvents();
		if (!PlayerData.Data.curseCharmPuzzleComplete)
		{
			this.ResetGraves();
		}
		else
		{
			if (!PlayerData.Data.GetLevelData(Levels.Graveyard).completed)
			{
				this.showBeam();
			}
			this.grave[5].SetInteractable(true);
		}
	}

	// Token: 0x06003739 RID: 14137 RVA: 0x001FCBAD File Offset: 0x001FAFAD
	protected override MapUIInteractionDialogue Show(PlayerInput player)
	{
		if (!PlayerData.Data.GetLevelData(Levels.Graveyard).completed)
		{
			return base.Show(player);
		}
		return null;
	}

	// Token: 0x0600373A RID: 14138 RVA: 0x001FCBD1 File Offset: 0x001FAFD1
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600373B RID: 14139 RVA: 0x001FCBE9 File Offset: 0x001FAFE9
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600373C RID: 14140 RVA: 0x001FCC01 File Offset: 0x001FB001
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (metadata == "LOADGRAVEYARD")
		{
			base.StartCoroutine(this.load_fight_cr());
		}
	}

	// Token: 0x0600373D RID: 14141 RVA: 0x001FCC20 File Offset: 0x001FB020
	private IEnumerator load_fight_cr()
	{
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		base.SetPlayerReturnPos();
		Map.Current.CurrentState = Map.State.Graveyard;
		if (Map.Current.players[0] != null)
		{
			Map.Current.players[0].animator.SetTrigger("Sleep");
		}
		if (Map.Current.players[1] != null)
		{
			Map.Current.players[1].animator.SetTrigger("Sleep");
		}
		yield return new WaitForSeconds(1f);
		SceneLoader.LoadScene(Scenes.scene_level_graveyard, SceneLoader.Transition.Blur, SceneLoader.Transition.Blur, SceneLoader.Icon.HourglassBroken, null);
		yield break;
	}

	// Token: 0x0600373E RID: 14142 RVA: 0x001FCC3B File Offset: 0x001FB03B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.RemoveDialoguerEvents();
	}

	// Token: 0x0600373F RID: 14143 RVA: 0x001FCC49 File Offset: 0x001FB049
	private void showBeam()
	{
		this.beamAnimator.Play("Aura", 1, 0f);
		this.beamAnimator.Play("Start", 0, 0f);
		this.beamAnimator.Update(0f);
	}

	// Token: 0x06003740 RID: 14144 RVA: 0x001FCC88 File Offset: 0x001FB088
	public void ActivatedGrave(int index, int playerNum, Vector3 ghostPos)
	{
		if (!PlayerData.Data.curseCharmPuzzleComplete)
		{
			if (index >= 0 && this.entryCount < 3)
			{
				Animator component = UnityEngine.Object.Instantiate<GameObject>(this.ghostPrefab, ghostPos, Quaternion.identity).GetComponent<Animator>();
				this.SFX_GRAVEYARD_Interact(this.entryCount);
				if (index == this.puzzleOrder[this.entryCount])
				{
					this.correctCount++;
				}
				this.entryCount++;
				if (this.entryCount == this.puzzleOrder.Length)
				{
					if (this.correctCount == this.entryCount)
					{
						component.Play("Yes");
						this.SFX_GRAVEYARD_Positive();
						this.showBeam();
						PlayerData.Data.curseCharmPuzzleComplete = true;
						PlayerData.SaveCurrentFile();
					}
					else
					{
						component.Play("No");
						this.SFX_GRAVEYARD_Negative();
						base.StartCoroutine(this.reset_cr());
					}
					this.extantGhosts[0].SetTrigger("EngageEnd");
					this.extantGhosts[1].SetTrigger("EngageEnd");
				}
				else
				{
					component.Play("EngageStart");
					this.extantGhosts[this.entryCount - 1] = component;
				}
			}
		}
		else if ((index == -1 && !PlayerData.Data.GetLevelData(Levels.Graveyard).completed) || this.canReenter)
		{
			this.StartSpeechBubble();
		}
	}

	// Token: 0x06003741 RID: 14145 RVA: 0x001FCDF4 File Offset: 0x001FB1F4
	private void UpdateReenterCodeActive()
	{
		switch (this.interactor)
		{
		default:
			if (base.PlayerWithinDistance(0))
			{
				Player actions = Map.Current.players[0].input.actions;
				if (actions.GetButton(11) && actions.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
				}
				else
				{
					this.currentDuration = 0f;
				}
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Mugman:
			if (base.PlayerWithinDistance(1))
			{
				Player actions2 = Map.Current.players[1].input.actions;
				if (actions2.GetButton(11) && actions2.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
				}
				else
				{
					this.currentDuration = 0f;
				}
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Either:
		{
			bool flag = false;
			if (base.PlayerWithinDistance(0))
			{
				Player actions3 = Map.Current.players[0].input.actions;
				if (actions3.GetButton(11) && actions3.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
					flag = true;
				}
			}
			if (base.PlayerWithinDistance(1))
			{
				Player actions4 = Map.Current.players[1].input.actions;
				if (actions4.GetButton(11) && actions4.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
					flag = true;
				}
			}
			if (!flag)
			{
				this.currentDuration = 0f;
			}
			break;
		}
		case AbstractMapInteractiveEntity.Interactor.Both:
			if (Map.Current.players[0] == null || Map.Current.players[1] == null)
			{
				this.canReenter = false;
			}
			if (base.PlayerWithinDistance(0) && base.PlayerWithinDistance(1))
			{
				if (Map.Current.players[0].input.actions.GetButton(13))
				{
					if (Map.Current.players[1].input.actions.GetButton(13))
					{
						this.currentDuration += CupheadTime.Delta;
					}
					else
					{
						this.currentDuration = 0f;
					}
				}
				else
				{
					this.currentDuration = 0f;
				}
			}
			break;
		}
		if (this.currentDuration >= this.pressDurationToReEnable && !this.canReenter && PlayerData.Data.GetLevelData(Levels.Graveyard).completed)
		{
			this.SFX_GRAVEYARD_Positive();
			this.showBeam();
			this.canReenter = true;
		}
	}

	// Token: 0x06003742 RID: 14146 RVA: 0x001FD0D4 File Offset: 0x001FB4D4
	private void ResetGraves()
	{
		foreach (MapGraveyardGrave mapGraveyardGrave in this.grave)
		{
			mapGraveyardGrave.SetInteractable(true);
		}
		this.entryCount = 0;
		this.correctCount = 0;
	}

	// Token: 0x06003743 RID: 14147 RVA: 0x001FD118 File Offset: 0x001FB518
	private IEnumerator reset_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.ResetGraves();
		yield break;
	}

	// Token: 0x06003744 RID: 14148 RVA: 0x001FD133 File Offset: 0x001FB533
	protected override void Update()
	{
		this.UpdateReenterCodeActive();
	}

	// Token: 0x06003745 RID: 14149 RVA: 0x001FD13B File Offset: 0x001FB53B
	private void SFX_GRAVEYARD_Activate()
	{
		AudioManager.Play("sfx_dlc_worldmap_graveyard_activate");
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x001FD147 File Offset: 0x001FB547
	private void SFX_GRAVEYARD_Interact(int i)
	{
		AudioManager.Play("sfx_dlc_worldmap_graveyard_interact_" + (i + 1));
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x001FD160 File Offset: 0x001FB560
	private void SFX_GRAVEYARD_Negative()
	{
		AudioManager.Play("sfx_dlc_worldmap_graveyard_negative");
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x001FD16C File Offset: 0x001FB56C
	private void SFX_GRAVEYARD_Positive()
	{
		AudioManager.Play("sfx_dlc_worldmap_graveyard_positive");
	}

	// Token: 0x04003F5C RID: 16220
	[SerializeField]
	private GameObject graveFire;

	// Token: 0x04003F5D RID: 16221
	[SerializeField]
	private MapGraveyardGrave[] grave;

	// Token: 0x04003F5E RID: 16222
	[SerializeField]
	private float pressDurationToReEnable = 1f;

	// Token: 0x04003F5F RID: 16223
	[SerializeField]
	private GameObject ghostPrefab;

	// Token: 0x04003F60 RID: 16224
	[SerializeField]
	private Animator beamAnimator;

	// Token: 0x04003F61 RID: 16225
	private int[] puzzleOrder;

	// Token: 0x04003F62 RID: 16226
	private int entryCount;

	// Token: 0x04003F63 RID: 16227
	private int correctCount;

	// Token: 0x04003F64 RID: 16228
	private float currentDuration;

	// Token: 0x04003F65 RID: 16229
	private Animator[] extantGhosts;
}
