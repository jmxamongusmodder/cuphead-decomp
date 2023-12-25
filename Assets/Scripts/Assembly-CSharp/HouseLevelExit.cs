using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006CD RID: 1741
public class HouseLevelExit : AbstractLevelInteractiveEntity
{
	// Token: 0x06002517 RID: 9495 RVA: 0x0015C12C File Offset: 0x0015A52C
	protected override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.playerActivating.transform.position += ((LevelPlayerController)base.playerActivating).motor.DistanceToGround() * Vector3.down;
		this.nonActivating = null;
		this.SwitchToRun(base.playerActivating.id);
		if (PlayerManager.Multiplayer)
		{
			this.nonActivating = (LevelPlayerController)PlayerManager.GetPlayer(PlayerId.PlayerTwo - (int)base.playerActivating.id);
		}
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06002518 RID: 9496 RVA: 0x0015C1CC File Offset: 0x0015A5CC
	private void SwitchToRun(PlayerId id)
	{
		GameObject gameObject = (id != PlayerId.PlayerOne) ? ((!PlayerManager.player1IsMugman) ? this.mugmanRunning : this.cupheadRunning) : ((!PlayerManager.player1IsMugman) ? this.cupheadRunning : this.mugmanRunning);
		AbstractPlayerController player = PlayerManager.GetPlayer(id);
		if (player.stats.isChalice)
		{
			gameObject = this.chaliceRunning;
		}
		player.gameObject.SetActive(false);
		gameObject.transform.position = player.transform.position;
		gameObject.gameObject.SetActive(true);
		((LevelPlayerController)player).DisableInput();
		((LevelPlayerController)player).PauseAll();
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x0015C280 File Offset: 0x0015A680
	private IEnumerator go_cr()
	{
		this.activated = true;
		float timeToGround = 0f;
		if (this.nonActivating != null)
		{
			while (this.nonActivating != null && this.nonActivating.gameObject.activeInHierarchy && !this.nonActivating.motor.Grounded)
			{
				timeToGround += CupheadTime.Delta;
				yield return null;
			}
			if (this.nonActivating.gameObject.activeInHierarchy && this.nonActivating != null)
			{
				this.SwitchToRun(this.nonActivating.id);
			}
		}
		if (timeToGround < 1f)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f - timeToGround);
		}
		SceneLoader.LoadScene(this.sceneLoadOnExit, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x0015C29B File Offset: 0x0015A69B
	protected override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Left, false);
	}

	// Token: 0x04002DC5 RID: 11717
	private bool activated;

	// Token: 0x04002DC6 RID: 11718
	[SerializeField]
	private GameObject cupheadRunning;

	// Token: 0x04002DC7 RID: 11719
	[SerializeField]
	private GameObject mugmanRunning;

	// Token: 0x04002DC8 RID: 11720
	[SerializeField]
	private GameObject chaliceRunning;

	// Token: 0x04002DC9 RID: 11721
	[SerializeField]
	private Scenes sceneLoadOnExit;

	// Token: 0x04002DCA RID: 11722
	private LevelPlayerController nonActivating;
}
