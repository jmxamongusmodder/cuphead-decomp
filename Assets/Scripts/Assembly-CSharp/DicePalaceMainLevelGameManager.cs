using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005D5 RID: 1493
public class DicePalaceMainLevelGameManager : LevelProperties.DicePalaceMain.Entity
{
	// Token: 0x06001D61 RID: 7521 RVA: 0x0010D24C File Offset: 0x0010B64C
	public override void LevelInit(LevelProperties.DicePalaceMain properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.StartDice;
		this.kingDiceAni = this.kingDice.GetComponent<Animator>();
		this.maxSpaces = this.allBoardSpaces.Length;
		this.GameSetup();
		this.marker.position = this.boardSpacesObj[DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED].Pivot.position;
		this.marker.rotation = this.boardSpacesObj[DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED].Pivot.rotation;
		if (!DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX)
		{
			AudioManager.Play("vox_intro");
			this.emitAudioFromObject.Add("vox_intro");
			DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX = true;
		}
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x0010D308 File Offset: 0x0010B708
	public void GameSetup()
	{
		LevelProperties.DicePalaceMain.Dice properties = base.properties.CurrentState.dice;
		this.dice = UnityEngine.Object.Instantiate<DicePalaceMainLevelDice>(this.dicePrefab);
		this.dice.Init(Vector2.zero, properties, this.pivotPoint1);
		this.pivotPoint1.position = this.dice.transform.position;
		this.CheckSafeSpaces();
		this.CheckHearts();
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x0010D378 File Offset: 0x0010B778
	public void CheckSafeSpaces()
	{
		for (int i = 0; i < DicePalaceMainLevelGameInfo.SAFE_INDEXES.Count; i++)
		{
			this.allBoardSpaces[DicePalaceMainLevelGameInfo.SAFE_INDEXES[i]] = DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace;
			this.boardSpacesObj[DicePalaceMainLevelGameInfo.SAFE_INDEXES[i] + 1].Clear = true;
		}
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x0010D3D0 File Offset: 0x0010B7D0
	private void CheckHearts()
	{
		for (int i = 0; i < DicePalaceMainLevelGameInfo.HEART_INDEXES.Length; i++)
		{
			this.boardSpacesObj[DicePalaceMainLevelGameInfo.HEART_INDEXES[i] + 1].HasHeart = true;
		}
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x0010D40B File Offset: 0x0010B80B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.dicePrefab = null;
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x0010D41A File Offset: 0x0010B81A
	private void StartDice()
	{
		if (Level.IsTowerOfPower)
		{
			this.EndBoardGame(this.dice);
		}
		else
		{
			base.StartCoroutine(this.check_for_rolled_cr());
		}
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x0010D444 File Offset: 0x0010B844
	public void RevealDice()
	{
		this.dice.StartRoll();
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x0010D454 File Offset: 0x0010B854
	private IEnumerator check_for_rolled_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.dice.revealDelay);
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = false;
		this.kingDiceAni.SetTrigger("OnReveal");
		yield return this.kingDiceAni.WaitForAnimationToEnd(this.kingDice, "Dice_Reveal", false, true);
		LevelProperties.DicePalaceMain.Dice p = base.properties.CurrentState.dice;
		int spacesToMove = 0;
		bool playingGame = true;
		while (playingGame)
		{
			while (this.dice.waitingToRoll)
			{
				yield return null;
			}
			spacesToMove = 0;
			DicePalaceMainLevelDice.Roll roll = this.dice.roll;
			if (roll != DicePalaceMainLevelDice.Roll.One)
			{
				if (roll != DicePalaceMainLevelDice.Roll.Two)
				{
					if (roll == DicePalaceMainLevelDice.Roll.Three)
					{
						spacesToMove = 3;
					}
				}
				else
				{
					spacesToMove = 2;
				}
			}
			else
			{
				spacesToMove = 1;
			}
			int spaces = (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + spacesToMove <= 1) ? 0 : (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + spacesToMove - 1);
			if (spaces < this.maxSpaces && this.allBoardSpaces[spaces] != DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace && this.allBoardSpaces[spaces] != DicePalaceMainLevelGameManager.BoardSpaces.StartOver && spaces + 1 < this.boardSpacesObj.Length && !this.boardSpacesObj[spaces + 1].HasHeart)
			{
				AudioManager.Stop("vox_curious");
				AudioManager.Play("vox_laugh");
				this.emitAudioFromObject.Add("vox_laugh");
			}
			yield return base.StartCoroutine(this.MoveMarker(spacesToMove, false));
			DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED += spacesToMove;
			if (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED > this.maxSpaces)
			{
				DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = this.maxSpaces;
				playingGame = false;
				this.kingDiceAni.SetBool("IsSafe", true);
				break;
			}
			DicePalaceMainLevelGameManager.BoardSpaces space = this.allBoardSpaces[spaces];
			this.kingDiceAni.SetTrigger("OnEager");
			if (playingGame)
			{
				if (space == DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace || DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED == this.previousSpace)
				{
					AudioManager.Play("vox_curious");
					this.emitAudioFromObject.Add("vox_curious");
					this.kingDiceAni.SetBool("IsSafe", true);
				}
				else if (space == DicePalaceMainLevelGameManager.BoardSpaces.StartOver)
				{
					AudioManager.Play("vox_startover");
					this.emitAudioFromObject.Add("vox_startover");
					AudioManager.Stop("vox_curious");
					DicePalaceMainLevelGameInfo.SAFE_INDEXES.Add(spaces);
					this.boardSpacesObj[spaces + 1].Clear = true;
					this.CheckSafeSpaces();
					yield return base.StartCoroutine(this.MoveMarker(-this.maxSpaces, true));
					this.kingDiceAni.SetBool("IsSafe", true);
					DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0;
					yield return CupheadTime.WaitForSeconds(this, p.pauseWhenRolled);
				}
				else
				{
					this.previousSpace = DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED;
					this.kingDiceAni.SetBool("IsSafe", false);
					DicePalaceMainLevelGameInfo.SAFE_INDEXES.Add(spaces);
					for (int i = 0; i < DicePalaceMainLevelGameInfo.HEART_INDEXES.Length; i++)
					{
						if (DicePalaceMainLevelGameInfo.HEART_INDEXES[i] == spaces)
						{
							if (DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS == null)
							{
								DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = new PlayersStatsBossesHub();
							}
							PlayerStatsManager playerStats = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats;
							if (playerStats.Health > 0)
							{
								DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.BonusHP++;
								playerStats.SetHealth(playerStats.Health + 1);
							}
							if (PlayerManager.Multiplayer)
							{
								if (DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS == null)
								{
									DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = new PlayersStatsBossesHub();
								}
								PlayerStatsManager stats = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats;
								if (stats.Health > 0)
								{
									DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.BonusHP++;
									stats.SetHealth(stats.Health + 1);
								}
							}
							this.boardSpacesObj[DicePalaceMainLevelGameInfo.HEART_INDEXES[i] + 1].HasHeart = false;
							this.heart.transform.position = this.boardSpacesObj[DicePalaceMainLevelGameInfo.HEART_INDEXES[i] + 1].HeartSpacePosition;
							this.heart.SetActive(true);
							AudioManager.Play("pop_up");
							this.emitAudioFromObject.Add("pop_up");
							yield return CupheadTime.WaitForSeconds(this, 1.5f);
							this.heart.SetActive(false);
							DicePalaceMainLevelGameInfo.HEART_INDEXES[i] = -1;
							break;
						}
					}
					yield return base.StartCoroutine(this.start_mini_boss_cr(this.SelectLevel(space)));
				}
				this.dice.waitingToRoll = true;
				yield return null;
			}
			yield return null;
		}
		this.EndBoardGame(this.dice);
		yield break;
	}

	// Token: 0x06001D69 RID: 7529 RVA: 0x0010D470 File Offset: 0x0010B870
	private IEnumerator MoveMarker(int spacesToMove, bool resetBoard)
	{
		int side = 1;
		if (spacesToMove < 0)
		{
			side = -1;
			spacesToMove *= -1;
		}
		for (int i = 0; i < spacesToMove; i++)
		{
			int index = DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + (1 + i) * side;
			if (index < 0 || index >= this.boardSpacesObj.Length)
			{
				break;
			}
			float t = 0f;
			Vector3 startPos = this.marker.position;
			Vector3 endPos = this.boardSpacesObj[index].Pivot.position;
			Quaternion startRot = this.marker.rotation;
			Quaternion endRot = this.boardSpacesObj[index].Pivot.rotation;
			if (!resetBoard)
			{
				this.markerAnimator.SetTrigger("Move");
				yield return this.markerAnimator.WaitForAnimationToStart(this, "Move", false);
			}
			float movement = (!resetBoard) ? 0.3f : 0.083333336f;
			while (t < movement)
			{
				this.marker.position = Vector3.Lerp(startPos, endPos, t / movement);
				this.marker.rotation = Quaternion.Lerp(startRot, endRot, t / movement);
				t += CupheadTime.Delta;
				yield return null;
			}
			this.marker.position = endPos;
			this.marker.rotation = endRot;
			AudioManager.Play("counter_move");
			this.emitAudioFromObject.Add("counter_move");
			if (!resetBoard)
			{
				this.markerAnimator.SetTrigger("Marker");
			}
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x0010D49C File Offset: 0x0010B89C
	private Levels SelectLevel(DicePalaceMainLevelGameManager.BoardSpaces space)
	{
		Levels result = Levels.DicePalaceMain;
		switch (space)
		{
		case DicePalaceMainLevelGameManager.BoardSpaces.Booze:
			result = Levels.DicePalaceBooze;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Chips:
			result = Levels.DicePalaceChips;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Cigar:
			result = Levels.DicePalaceCigar;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Domino:
			result = Levels.DicePalaceDomino;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.EightBall:
			result = Levels.DicePalaceEightBall;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.FlyingHorse:
			result = Levels.DicePalaceFlyingHorse;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.FlyingMemory:
			result = Levels.DicePalaceFlyingMemory;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Pachinko:
			result = Levels.DicePalacePachinko;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Rabbit:
			result = Levels.DicePalaceRabbit;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Roulette:
			result = Levels.DicePalaceRoulette;
			break;
		}
		return result;
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x0010D558 File Offset: 0x0010B958
	private IEnumerator start_mini_boss_cr(Levels level)
	{
		this.kingDiceAni.SetTrigger("OnEat");
		DicePalaceMainLevelGameInfo.SetPlayersStats();
		yield return this.kingDiceAni.WaitForAnimationToStart(this, "Eat_Screen", false);
		AudioManager.Play("king_dice_eat_screen");
		this.kingDice.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Player.ToString();
		this.kingDice.GetComponent<SpriteRenderer>().sortingOrder = 2000;
		Level.ScoringData.time += Level.Current.LevelTime;
		yield return this.kingDiceAni.WaitForAnimationToEnd(this, "Eat_Screen", false, true);
		SceneLoader.LoadLevel(level, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
		yield return null;
		yield break;
	}

	// Token: 0x06001D6C RID: 7532 RVA: 0x0010D57C File Offset: 0x0010B97C
	private void EndBoardGame(DicePalaceMainLevelDice dice1)
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.flashEnd_cr());
		base.StartCoroutine(this.announcerSfx_cr());
		this.kingDice.StartKingDiceBattle();
		if (dice1 != null)
		{
			UnityEngine.Object.Destroy(dice1.gameObject);
		}
		DicePalaceMainLevelGameInfo.CleanUpRetry();
	}

	// Token: 0x06001D6D RID: 7533 RVA: 0x0010D5D0 File Offset: 0x0010B9D0
	private IEnumerator flashEnd_cr()
	{
		DicePalaceMainLevelBoardSpace endSpace = this.boardSpacesObj[this.boardSpacesObj.Length - 1];
		for (;;)
		{
			endSpace.Clear = true;
			yield return CupheadTime.WaitForSeconds(this, this.endSpaceFlashRate);
			endSpace.Clear = false;
			yield return CupheadTime.WaitForSeconds(this, this.endSpaceFlashRate);
		}
		yield break;
	}

	// Token: 0x06001D6E RID: 7534 RVA: 0x0010D5EC File Offset: 0x0010B9EC
	private IEnumerator announcerSfx_cr()
	{
		AudioManager.Play("level_announcer_ready");
		AudioManager.Play("level_bell_intro");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		AudioManager.Play("level_announcer_begin");
		yield break;
	}

	// Token: 0x04002644 RID: 9796
	private const float MarkerMovementTime = 0.3f;

	// Token: 0x04002645 RID: 9797
	private const float MarkerFastMove = 0.083333336f;

	// Token: 0x04002646 RID: 9798
	[SerializeField]
	private DicePalaceMainLevelGameManager.BoardSpaces[] allBoardSpaces;

	// Token: 0x04002647 RID: 9799
	[SerializeField]
	private DicePalaceMainLevelBoardSpace[] boardSpacesObj;

	// Token: 0x04002648 RID: 9800
	[SerializeField]
	private DicePalaceMainLevelBoardSpace startSpaceObj;

	// Token: 0x04002649 RID: 9801
	[SerializeField]
	private DicePalaceMainLevelBoardSpace endSpaceObj;

	// Token: 0x0400264A RID: 9802
	[SerializeField]
	private DicePalaceMainLevelKingDice kingDice;

	// Token: 0x0400264B RID: 9803
	[SerializeField]
	private DicePalaceMainLevelDice dicePrefab;

	// Token: 0x0400264C RID: 9804
	[SerializeField]
	private Transform pivotPoint1;

	// Token: 0x0400264D RID: 9805
	[SerializeField]
	private Transform marker;

	// Token: 0x0400264E RID: 9806
	[SerializeField]
	private Animator markerAnimator;

	// Token: 0x0400264F RID: 9807
	[SerializeField]
	private float endSpaceFlashRate;

	// Token: 0x04002650 RID: 9808
	[SerializeField]
	private GameObject heart;

	// Token: 0x04002651 RID: 9809
	private Animator kingDiceAni;

	// Token: 0x04002652 RID: 9810
	private DicePalaceMainLevelDice dice;

	// Token: 0x04002653 RID: 9811
	private DicePalaceMainLevelGameInfo gameInfo;

	// Token: 0x04002654 RID: 9812
	private int previousSpace;

	// Token: 0x04002655 RID: 9813
	private int maxSpaces;

	// Token: 0x020005D6 RID: 1494
	public enum BoardSpaces
	{
		// Token: 0x04002657 RID: 9815
		Booze,
		// Token: 0x04002658 RID: 9816
		Chips,
		// Token: 0x04002659 RID: 9817
		Cigar,
		// Token: 0x0400265A RID: 9818
		Domino,
		// Token: 0x0400265B RID: 9819
		EightBall,
		// Token: 0x0400265C RID: 9820
		FlyingHorse,
		// Token: 0x0400265D RID: 9821
		FlyingMemory,
		// Token: 0x0400265E RID: 9822
		Pachinko,
		// Token: 0x0400265F RID: 9823
		Rabbit,
		// Token: 0x04002660 RID: 9824
		Roulette,
		// Token: 0x04002661 RID: 9825
		FreeSpace,
		// Token: 0x04002662 RID: 9826
		StartOver
	}
}
