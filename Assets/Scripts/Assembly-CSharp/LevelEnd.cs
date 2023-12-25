using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004A3 RID: 1187
public class LevelEnd : AbstractMonoBehaviour
{
	// Token: 0x06001355 RID: 4949 RVA: 0x000AAC94 File Offset: 0x000A9094
	protected override void Awake()
	{
		base.Awake();
		base.transform.SetAsFirstSibling();
		base.gameObject.name = "LEVEL_END_CONTROLER";
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x000AACB8 File Offset: 0x000A90B8
	private static LevelEnd Create()
	{
		GameObject gameObject = new GameObject();
		return gameObject.AddComponent<LevelEnd>();
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x000AACD4 File Offset: 0x000A90D4
	public static void Win(IEnumerator knockoutSFXCoroutine, Action onBossDeathCallback, Action explosionsCallback, Action explosionsFalloffCallback, Action explosionsEndCallback, AbstractPlayerController[] players, float bossDeathTime, bool goToWinScreen, bool isMausoleum, bool isDevil, bool isTowerOfPower)
	{
		LevelEnd levelEnd = LevelEnd.Create();
		levelEnd.StartCoroutine(levelEnd.win_cr(knockoutSFXCoroutine, onBossDeathCallback, explosionsCallback, explosionsFalloffCallback, explosionsEndCallback, players, bossDeathTime, goToWinScreen, isMausoleum, isDevil, isTowerOfPower));
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x000AAD08 File Offset: 0x000A9108
	private IEnumerator win_cr(IEnumerator knockoutSFXCoroutine, Action onBossDeathCallback, Action explosionsCallback, Action explosionsFalloffCallback, Action explosionsEndCallback, AbstractPlayerController[] players, float bossDeathTime, bool goToWinScreen, bool isMausoleum, bool isDevil, bool isTowerOfPower)
	{
		PauseManager.Pause();
		LevelKOAnimation koAnim = LevelKOAnimation.Create(isMausoleum);
		if (Level.IsChessBoss)
		{
			AudioManager.StartBGMAlternate(0);
		}
		if (Level.Current.CurrentLevel == Levels.Saltbaker)
		{
			AudioManager.StartBGMAlternate(2);
		}
		base.StartCoroutine(knockoutSFXCoroutine);
		yield return koAnim.StartCoroutine(koAnim.anim_cr());
		PauseManager.Unpause();
		explosionsCallback();
		CupheadTime.SetAll(1f);
		if (!isMausoleum)
		{
			foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
			{
				if (!(abstractPlayerController == null))
				{
					abstractPlayerController.OnLevelWin();
				}
			}
		}
		if (onBossDeathCallback != null)
		{
			onBossDeathCallback();
		}
		yield return new WaitForSeconds(bossDeathTime + 0.3f);
		foreach (AbstractProjectile abstractProjectile in UnityEngine.Object.FindObjectsOfType<AbstractProjectile>())
		{
			abstractProjectile.OnLevelEnd();
		}
		if (Level.IsTowerOfPower)
		{
			TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerOne);
			if (PlayerManager.Multiplayer)
			{
				TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerTwo);
			}
		}
		else if (Level.IsDicePalace && !Level.IsDicePalaceMain)
		{
			DicePalaceMainLevelGameInfo.SetPlayersStats();
		}
		SceneLoader.properties.transitionStart = SceneLoader.Transition.Fade;
		SceneLoader.properties.transitionStartTime = 3f;
		if (Level.IsChessBoss || Level.Current.CurrentLevel == Levels.Saltbaker)
		{
			yield return new WaitForSeconds(2f);
		}
		if (goToWinScreen)
		{
			SceneLoader.LoadScene(Scenes.scene_win, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
		}
		else if (Level.IsTowerOfPower)
		{
			SceneLoader.ContinueTowerOfPower();
		}
		else if (Level.IsGraveyard)
		{
			SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.None, null);
		}
		else if (Level.IsChessBoss)
		{
			if (SceneLoader.CurrentContext is GauntletContext)
			{
				int currentIndex = Array.IndexOf<Levels>(Level.kingOfGamesLevels, Level.Current.CurrentLevel);
				int num = MathUtilities.NextIndex(currentIndex, Level.kingOfGamesLevels.Length);
				if (num == 0)
				{
					SceneLoader.LoadScene(Scenes.scene_level_chess_castle, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, new GauntletContext(true));
				}
				else
				{
					Levels level = Level.kingOfGamesLevels[num];
					SceneLoader.Transition transitionStart = SceneLoader.Transition.Fade;
					GauntletContext context = new GauntletContext(false);
					SceneLoader.LoadLevel(level, transitionStart, SceneLoader.Icon.Hourglass, context);
				}
			}
			else
			{
				SceneLoader.LoadScene(Scenes.scene_level_chess_castle, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
			}
		}
		else if (!isMausoleum)
		{
			SceneLoader.ReloadLevel();
		}
		yield return new WaitForSeconds(2.5f);
		explosionsEndCallback();
		yield break;
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x000AAD58 File Offset: 0x000A9158
	public static void Lose(bool isMausoleum, bool secretTriggered)
	{
		LevelEnd levelEnd = LevelEnd.Create();
		levelEnd.StartCoroutine(levelEnd.lose_cr(isMausoleum, secretTriggered));
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x000AAD7C File Offset: 0x000A917C
	private IEnumerator lose_cr(bool isMausoleum, bool secretTriggered)
	{
		if (isMausoleum)
		{
			AudioManager.Play("level_announcer_fail");
		}
		PauseManager.Unpause();
		foreach (AbstractPausableComponent abstractPausableComponent in UnityEngine.Object.FindObjectsOfType<AbstractPausableComponent>())
		{
			abstractPausableComponent.OnLevelEnd();
		}
		LevelGameOverGUI.Current.In(secretTriggered);
		if (Level.IsChessBoss)
		{
			yield return new WaitForSeconds(1f);
			AudioManager.StartBGMAlternate(1);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x000AADA0 File Offset: 0x000A91A0
	public static void PlayerJoined()
	{
		LevelEnd levelEnd = LevelEnd.Create();
		levelEnd.StartCoroutine(levelEnd.playerJoined_cr());
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x000AADC0 File Offset: 0x000A91C0
	private IEnumerator playerJoined_cr()
	{
		PauseManager.Unpause();
		foreach (AbstractPausableComponent abstractPausableComponent in UnityEngine.Object.FindObjectsOfType<AbstractPausableComponent>())
		{
			abstractPausableComponent.OnLevelEnd();
		}
		yield return new WaitForSeconds(1f);
		yield return new WaitForSeconds(1f);
		SceneLoader.LoadLastMap();
		yield break;
	}

	// Token: 0x04001C76 RID: 7286
	private const string NAME = "LEVEL_END_CONTROLER";

	// Token: 0x04001C77 RID: 7287
	private const float WIN_FADE_TIME = 3f;

	// Token: 0x04001C78 RID: 7288
	private const float JOIN_WAIT = 1f;
}
