using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000902 RID: 2306
public class PlatformingLevelEnd : AbstractMonoBehaviour
{
	// Token: 0x06003615 RID: 13845 RVA: 0x001F6B06 File Offset: 0x001F4F06
	protected override void Awake()
	{
		base.Awake();
		base.transform.SetAsFirstSibling();
		base.gameObject.name = "PLATFORMING_LEVEL_END_CONTROLER";
	}

	// Token: 0x06003616 RID: 13846 RVA: 0x001F6B2C File Offset: 0x001F4F2C
	private static PlatformingLevelEnd Create()
	{
		GameObject gameObject = new GameObject();
		return gameObject.AddComponent<PlatformingLevelEnd>();
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x001F6B48 File Offset: 0x001F4F48
	public static void Win()
	{
		PlatformingLevelEnd platformingLevelEnd = PlatformingLevelEnd.Create();
		platformingLevelEnd.StartCoroutine(platformingLevelEnd.win_cr());
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x001F6B68 File Offset: 0x001F4F68
	private void OnWinComplete()
	{
		this.winReadyToExit = true;
	}

	// Token: 0x06003619 RID: 13849 RVA: 0x001F6B74 File Offset: 0x001F4F74
	private IEnumerator win_cr()
	{
		PlatformingLevelExit.OnWinCompleteEvent += this.OnWinComplete;
		PauseManager.Pause();
		PlatformingLevelWinAnimation bravoAnimation = PlatformingLevelWinAnimation.Create();
		AudioManager.Play("platforming_announcer_bravo");
		while (bravoAnimation.CurrentState == PlatformingLevelWinAnimation.State.Paused)
		{
			yield return null;
		}
		PauseManager.Unpause();
		CupheadTime.SetAll(1f);
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				levelPlayerController.OnLevelWin();
			}
		}
		foreach (AbstractProjectile abstractProjectile in UnityEngine.Object.FindObjectsOfType<AbstractProjectile>())
		{
			abstractProjectile.OnLevelEnd();
		}
		foreach (AbstractPlatformingLevelEnemy abstractPlatformingLevelEnemy in UnityEngine.Object.FindObjectsOfType<AbstractPlatformingLevelEnemy>())
		{
			abstractPlatformingLevelEnemy.OnLevelEnd();
		}
		yield return null;
		while (!this.winReadyToExit)
		{
			yield return null;
		}
		SceneLoader.properties.transitionStart = SceneLoader.Transition.Fade;
		SceneLoader.properties.transitionStartTime = 3f;
		SceneLoader.LoadScene(Scenes.scene_win, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
		yield break;
	}

	// Token: 0x0600361A RID: 13850 RVA: 0x001F6B90 File Offset: 0x001F4F90
	public static void Lose()
	{
		PlatformingLevelEnd platformingLevelEnd = PlatformingLevelEnd.Create();
		platformingLevelEnd.StartCoroutine(platformingLevelEnd.lose_cr());
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x001F6BB0 File Offset: 0x001F4FB0
	private IEnumerator lose_cr()
	{
		PauseManager.Unpause();
		foreach (AbstractPausableComponent abstractPausableComponent in UnityEngine.Object.FindObjectsOfType<AbstractPausableComponent>())
		{
			abstractPausableComponent.OnLevelEnd();
		}
		LevelGameOverGUI.Current.In(false);
		yield return null;
		yield break;
	}

	// Token: 0x04003E1D RID: 15901
	private const string NAME = "PLATFORMING_LEVEL_END_CONTROLER";

	// Token: 0x04003E1E RID: 15902
	private const float WIN_FADE_TIME = 3f;

	// Token: 0x04003E1F RID: 15903
	private bool winReadyToExit;
}
