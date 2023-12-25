using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005B9 RID: 1465
public class DicePalaceDominoLevelFloor : AbstractCollidableObject
{
	// Token: 0x06001C73 RID: 7283 RVA: 0x001046FD File Offset: 0x00102AFD
	public void InitFloor(LevelProperties.DicePalaceDomino properties)
	{
		this.properties = properties;
		this.tiles = new List<DicePalaceDominoLevelFloorTile>();
		this.preTiles = new List<DicePalaceDominoLevelFloorTile>();
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x0010471C File Offset: 0x00102B1C
	public void StartSpawningTiles()
	{
		base.StartCoroutine(this.tileSpawn_cr());
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x0010472C File Offset: 0x00102B2C
	private IEnumerator tileSpawn_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		for (int i = 0; i < this._floors.Length; i++)
		{
			this._floors[i].speed = this.properties.CurrentState.domino.floorSpeed;
		}
		this._teethSprite.speed = this.properties.CurrentState.domino.floorSpeed;
		this.AddForces();
		for (int j = 0; j < this.preTiles.Count; j++)
		{
			if (this.preTiles[j].currentColourIndex == (int)this.spikesColour)
			{
				this.preTiles[j].TriggerSpikes(true);
			}
			else
			{
				this.preTiles[j].TriggerSpikes(false);
			}
			this.preTiles[j].InitTile();
		}
		yield break;
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x00104748 File Offset: 0x00102B48
	private void AddForces()
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				this.levelForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.Ground, -this.properties.CurrentState.domino.floorSpeed);
				levelPlayerController.motor.AddForce(this.levelForce);
			}
		}
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x001047E8 File Offset: 0x00102BE8
	private int ParseColour(char c)
	{
		if (c == 'B')
		{
			return 0;
		}
		if (c == 'G')
		{
			return 1;
		}
		if (c == 'R')
		{
			return 2;
		}
		if (c != 'Y')
		{
			return 0;
		}
		return 3;
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x00104818 File Offset: 0x00102C18
	public void CheckTiles(DicePalaceDominoLevelBouncyBall.Colour color)
	{
		this.spikesColour = color;
		base.StartCoroutine(this.check_tiles_cr(this.spikesColour));
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x00104834 File Offset: 0x00102C34
	private IEnumerator check_tiles_cr(DicePalaceDominoLevelBouncyBall.Colour color)
	{
		foreach (DicePalaceDominoLevelFloorTile dicePalaceDominoLevelFloorTile in this.tiles)
		{
			if (dicePalaceDominoLevelFloorTile.isActivated)
			{
				if (dicePalaceDominoLevelFloorTile.currentColourIndex == (int)color)
				{
					dicePalaceDominoLevelFloorTile.TriggerSpikes(true);
				}
				else
				{
					dicePalaceDominoLevelFloorTile.TriggerSpikes(false);
				}
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x0400256C RID: 9580
	[Header("Floor")]
	[SerializeField]
	private DicePalaceDominoLevelScrollingFloor[] _floors;

	// Token: 0x0400256D RID: 9581
	[SerializeField]
	private ScrollingSprite _teethSprite;

	// Token: 0x0400256E RID: 9582
	private DicePalaceDominoLevelBouncyBall.Colour spikesColour = DicePalaceDominoLevelBouncyBall.Colour.none;

	// Token: 0x0400256F RID: 9583
	public Action OnToggleFlashEvent;

	// Token: 0x04002570 RID: 9584
	public Action OnColourChangeEvent;

	// Token: 0x04002571 RID: 9585
	private LevelProperties.DicePalaceDomino properties;

	// Token: 0x04002572 RID: 9586
	private List<DicePalaceDominoLevelFloorTile> tiles;

	// Token: 0x04002573 RID: 9587
	private List<DicePalaceDominoLevelFloorTile> preTiles;

	// Token: 0x04002574 RID: 9588
	private LevelPlayerMotor.VelocityManager.Force levelForce;
}
