using System;
using UnityEngine;

// Token: 0x0200093A RID: 2362
public class MapLadderEntity : AbstractMapInteractiveEntity
{
	// Token: 0x0600374A RID: 14154 RVA: 0x001FD334 File Offset: 0x001FB734
	public void Init(MapPlayerLadderObject playerLadder, Vector2 exit, MapLadder.Location location)
	{
		this.playerLadder = playerLadder;
		this.exit = base.transform.position + exit;
		this.location = location;
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x001FD360 File Offset: 0x001FB760
	protected override MapUIInteractionDialogue Show(PlayerInput player)
	{
		switch (base.playerChecking.state)
		{
		case MapPlayerController.State.Walking:
		case MapPlayerController.State.LadderExit:
			this.dialogueProperties = MapLadder.DIALOGUE_ENTER;
			break;
		case MapPlayerController.State.LadderEnter:
		case MapPlayerController.State.Ladder:
			this.dialogueProperties = MapLadder.DIALOGUE_EXIT;
			break;
		}
		return base.Show(player);
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x001FD3BC File Offset: 0x001FB7BC
	protected override void Activate()
	{
		base.Activate();
		MapPlayerController.State state = base.playerActivating.state;
		if (state != MapPlayerController.State.Walking)
		{
			if (state == MapPlayerController.State.Ladder)
			{
				base.playerActivating.LadderExit(base.transform.position, this.exit, this.location);
			}
		}
		else
		{
			base.playerActivating.LadderEnter(base.transform.position, this.playerLadder, this.location);
		}
	}

	// Token: 0x04003F67 RID: 16231
	private MapPlayerLadderObject playerLadder;

	// Token: 0x04003F68 RID: 16232
	private Vector2 exit;

	// Token: 0x04003F69 RID: 16233
	private MapLadder.Location location;
}
