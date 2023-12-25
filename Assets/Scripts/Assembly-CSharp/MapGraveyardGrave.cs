using System;
using UnityEngine;

// Token: 0x02000938 RID: 2360
public class MapGraveyardGrave : MonoBehaviour
{
	// Token: 0x0600372E RID: 14126 RVA: 0x001FC84A File Offset: 0x001FAC4A
	private void Start()
	{
		this.hasCharm = this.HasCharm();
	}

	// Token: 0x0600372F RID: 14127 RVA: 0x001FC858 File Offset: 0x001FAC58
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<MapPlayerController>())
		{
			MapPlayerController component = collision.GetComponent<MapPlayerController>();
			if (component.id == PlayerId.PlayerOne)
			{
				if (this.player1 == null)
				{
					this.player1 = component;
				}
				this.p1InTrigger = true;
			}
			else
			{
				if (this.player2 == null)
				{
					this.player2 = component;
				}
				this.p2InTrigger = true;
			}
		}
	}

	// Token: 0x06003730 RID: 14128 RVA: 0x001FC8CC File Offset: 0x001FACCC
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<MapPlayerController>())
		{
			MapPlayerController component = collision.GetComponent<MapPlayerController>();
			if (component.id == PlayerId.PlayerOne)
			{
				this.p1InTrigger = false;
			}
			else
			{
				this.p2InTrigger = false;
			}
		}
	}

	// Token: 0x06003731 RID: 14129 RVA: 0x001FC910 File Offset: 0x001FAD10
	private bool HasCharm()
	{
		return (PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Charm.charm_curse) && CharmCurse.CalculateLevel(PlayerId.PlayerOne) >= 0) || (PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Charm.charm_curse) && CharmCurse.CalculateLevel(PlayerId.PlayerTwo) >= 0);
	}

	// Token: 0x06003732 RID: 14130 RVA: 0x001FC968 File Offset: 0x001FAD68
	private void Update()
	{
		if (SceneLoader.IsInBlurTransition)
		{
			return;
		}
		if (this.hasCharm && !this.main.canReenter)
		{
			return;
		}
		if (this.canInteract || (this.hasCharm && this.main.canReenter))
		{
			if (this.p1InTrigger && this.player1.input.actions.GetButtonDown(13))
			{
				if (this.player1.animationController.facingUpwards)
				{
					this.InteractWith(0);
				}
			}
			else if (this.p2InTrigger && this.player2.input.actions.GetButtonDown(13) && this.player2.animationController.facingUpwards)
			{
				this.InteractWith(1);
			}
		}
	}

	// Token: 0x06003733 RID: 14131 RVA: 0x001FCA50 File Offset: 0x001FAE50
	private void InteractWith(int playerNum)
	{
		if (!this.isResetGrave)
		{
			this.canInteract = false;
		}
		this.main.ActivatedGrave(this.index, playerNum, (!this.isResetGrave) ? this.ghostPos.transform.position : Vector3.zero);
	}

	// Token: 0x06003734 RID: 14132 RVA: 0x001FCAA6 File Offset: 0x001FAEA6
	public void SetInteractable(bool value)
	{
		this.canInteract = value;
	}

	// Token: 0x04003F52 RID: 16210
	[SerializeField]
	private bool isResetGrave;

	// Token: 0x04003F53 RID: 16211
	[SerializeField]
	private int index;

	// Token: 0x04003F54 RID: 16212
	private MapPlayerController player1;

	// Token: 0x04003F55 RID: 16213
	private MapPlayerController player2;

	// Token: 0x04003F56 RID: 16214
	private bool p1InTrigger;

	// Token: 0x04003F57 RID: 16215
	private bool p2InTrigger;

	// Token: 0x04003F58 RID: 16216
	private bool canInteract;

	// Token: 0x04003F59 RID: 16217
	[SerializeField]
	private MapGraveyardHandler main;

	// Token: 0x04003F5A RID: 16218
	[SerializeField]
	private Transform ghostPos;

	// Token: 0x04003F5B RID: 16219
	private bool hasCharm;
}
