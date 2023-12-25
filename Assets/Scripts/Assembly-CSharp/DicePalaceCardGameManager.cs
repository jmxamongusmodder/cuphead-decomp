using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005A4 RID: 1444
public class DicePalaceCardGameManager : AbstractPausableComponent
{
	// Token: 0x06001BCF RID: 7119 RVA: 0x000FD78C File Offset: 0x000FBB8C
	public void GameSetup(LevelProperties.DicePalaceCard cardProperties)
	{
		this.properties = cardProperties.CurrentState.blocks;
		this.GridDimX = this.properties.gridWidth;
		this.GridDimY = this.properties.gridHeight;
		this.SetSize();
		this.typePattern = this.properties.cardTypeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.amountPattern = this.properties.cardAmountString.GetRandom<string>().Split(new char[]
		{
			','
		});
		Vector3 position = base.transform.position;
		position.y = 360f - this.gridBlockPrefab.GetComponent<Renderer>().bounds.size.y;
		position.x = -640f + this.gridBlockPrefab.GetComponent<Renderer>().bounds.size.x;
		base.transform.position = position;
		this.selectedPrefab = new DicePalaceCardLevelBlock();
		this.totalColumns = new List<DicePalaceCardLevelColumn>();
		this.typeIndex = UnityEngine.Random.Range(0, this.typePattern.Length);
		this.amountIndex = UnityEngine.Random.Range(0, this.amountPattern.Length);
		this.GenerateGrid();
		this.startingPos = base.transform.position.y;
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x000FD8F0 File Offset: 0x000FBCF0
	public IEnumerator start_game_cr()
	{
		for (;;)
		{
			this.SpawnColumn();
			yield return CupheadTime.WaitForSeconds(this, this.properties.attackDelayRange);
		}
		yield break;
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x000FD90C File Offset: 0x000FBD0C
	private void SetSize()
	{
		this.hearts.transform.SetScale(new float?(this.properties.blockSize), new float?(this.properties.blockSize), new float?(this.properties.blockSize));
		this.spades.transform.SetScale(new float?(this.properties.blockSize), new float?(this.properties.blockSize), new float?(this.properties.blockSize));
		this.clubs.transform.SetScale(new float?(this.properties.blockSize), new float?(this.properties.blockSize), new float?(this.properties.blockSize));
		this.diamonds.transform.SetScale(new float?(this.properties.blockSize), new float?(this.properties.blockSize), new float?(this.properties.blockSize));
		this.gridBlockPrefab.transform.SetScale(new float?(this.properties.blockSize), new float?(this.properties.blockSize), new float?(this.properties.blockSize));
		this.GridSpacing = this.properties.blockSize;
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x000FDA6C File Offset: 0x000FBE6C
	private void SpawnColumn()
	{
		int num = 1;
		int num2 = -1;
		float num3 = this.gridBlockPrefab.GetComponent<Renderer>().bounds.size.y / 2f;
		float num4 = 0f;
		Parser.FloatTryParse(this.amountPattern[this.amountIndex], out num4);
		DicePalaceCardLevelColumn item = UnityEngine.Object.Instantiate<DicePalaceCardLevelColumn>(this.columnObject);
		this.totalColumns.Add(item);
		int index = this.totalColumns.Count - 1;
		Vector3 position = this.totalColumns[index].transform.position;
		position.x = 640f;
		position.y = 360f - num3;
		this.totalColumns[index].transform.position = position;
		int num5 = 0;
		while ((float)num5 < num4)
		{
			if (this.typePattern[this.typeIndex][0] == 'H')
			{
				this.selectedPrefab = this.hearts;
			}
			else if (this.typePattern[this.typeIndex][0] == 'S')
			{
				this.selectedPrefab = this.spades;
			}
			else if (this.typePattern[this.typeIndex][0] == 'D')
			{
				this.selectedPrefab = this.diamonds;
			}
			else if (this.typePattern[this.typeIndex][0] == 'C')
			{
				this.selectedPrefab = this.clubs;
			}
			this.typeIndex = (this.typeIndex + 1) % this.typePattern.Length;
			DicePalaceCardLevelBlock dicePalaceCardLevelBlock = UnityEngine.Object.Instantiate<DicePalaceCardLevelBlock>(this.selectedPrefab);
			this.totalColumns[index].blockPieces.Add(dicePalaceCardLevelBlock);
			dicePalaceCardLevelBlock.transform.parent = this.totalColumns[index].transform;
			Vector3 position2 = this.totalColumns[index].blockPieces[num5].transform.position;
			if (num5 % 2 == 0 && num5 != 0)
			{
				this.totalColumns[index].blockPieces[num5].stopOffsetX = num2;
				position2.x = 640f - this.totalColumns[index].blockPieces[num5].GetComponent<Renderer>().bounds.size.x * (float)Mathf.Abs(this.totalColumns[index].blockPieces[num5].stopOffsetX);
				num2--;
			}
			else if (num5 % 2 == 1 && num5 != 0)
			{
				this.totalColumns[index].blockPieces[num5].stopOffsetX = num;
				position2.x = 640f + this.totalColumns[index].blockPieces[num5].GetComponent<Renderer>().bounds.size.x * (float)Mathf.Abs(this.totalColumns[index].blockPieces[num5].stopOffsetX);
				num++;
			}
			else
			{
				position2.x = 640f;
			}
			position2.y = 360f - num3;
			this.totalColumns[index].blockPieces[num5].transform.position = position2;
			num5++;
		}
		this.amountIndex = (this.amountIndex + 1) % this.amountPattern.Length;
		base.StartCoroutine(this.horizontal_moving_column(this.totalColumns[index]));
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x000FDE24 File Offset: 0x000FC224
	private IEnumerator horizontal_moving_column(DicePalaceCardLevelColumn currentColumn)
	{
		AbstractPlayerController player = PlayerManager.GetNext();
		float offset = this.gridBlocks[1, 0].transform.position.x - this.gridBlocks[0, 0].transform.position.x;
		int playerXPos = 0;
		int stopXPos = 0;
		bool selectStop = false;
		float dist = 0f;
		float distOffset = 20f;
		while (currentColumn.transform.position.x != this.gridBlocks[stopXPos, 0].transform.position.x)
		{
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
			for (int i = 0; i < this.GridDimX - 1; i++)
			{
				if (player.transform.position.x > this.gridBlocks[i, 0].transform.position.x - offset / 2f)
				{
					if (player.transform.position.x < this.gridBlocks[i + 1, 0].transform.position.x - offset / 2f)
					{
						playerXPos = i;
					}
					else if (i + 1 == this.GridDimX - 1)
					{
						playerXPos = i + 1;
					}
				}
			}
			dist = this.gridBlocks[playerXPos, 0].transform.position.x - currentColumn.transform.position.x;
			Vector3 pos = currentColumn.transform.position;
			if (dist < distOffset)
			{
				selectStop = true;
			}
			int overFlow = this.GridDimX - playerXPos - currentColumn.blockPieces.Count;
			if (selectStop)
			{
				if (this.gridBlocks[1, 0].transform.position.x > player.transform.position.x && currentColumn.blockPieces.Count >= 1)
				{
					int value;
					if (currentColumn.blockPieces.Count % 2 == 1)
					{
						value = currentColumn.blockPieces[currentColumn.blockPieces.Count - 1].stopOffsetX - 1;
					}
					else
					{
						value = currentColumn.blockPieces[currentColumn.blockPieces.Count - 1].stopOffsetX;
					}
					stopXPos = Mathf.Abs(value) - 1;
					selectStop = false;
				}
				else if (this.gridBlocks[this.GridDimX - 1, 0].transform.position.x < player.transform.position.x || Mathf.Sign((float)overFlow) == -1f)
				{
					stopXPos = this.GridDimX - 1 - Mathf.Abs(currentColumn.blockPieces[currentColumn.blockPieces.Count - 1].stopOffsetX);
					selectStop = false;
				}
				else
				{
					stopXPos = playerXPos;
					selectStop = false;
				}
			}
			pos.x = Mathf.MoveTowards(currentColumn.transform.position.x, this.gridBlocks[stopXPos, this.GridDimY - 1].transform.position.x, this.properties.blockSpeed * CupheadTime.Delta);
			currentColumn.transform.position = pos;
			yield return null;
		}
		base.StartCoroutine(this.vertical_moving_column(currentColumn, stopXPos));
		yield return null;
		yield break;
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x000FDE48 File Offset: 0x000FC248
	private IEnumerator vertical_moving_column(DicePalaceCardLevelColumn currentColumn, int stopXPos)
	{
		currentColumn.blockCounter = 0;
		currentColumn.blockXPos = new int[currentColumn.blockPieces.Count];
		currentColumn.columnStopYPos = new int[currentColumn.blockPieces.Count];
		for (int i = 0; i < currentColumn.blockPieces.Count; i++)
		{
			currentColumn.blockXPos[i] = stopXPos + currentColumn.blockPieces[i].stopOffsetX;
			for (int j = this.GridDimY - 1; j >= 0; j--)
			{
				if (!this.gridBlocks[currentColumn.blockXPos[i], j].hasBlock)
				{
					if (j > 0)
					{
						if (this.gridBlocks[currentColumn.blockXPos[i], j - 1].hasBlock)
						{
							currentColumn.columnStopYPos[i] = j;
							this.gridBlocks[currentColumn.blockXPos[i], j].hasBlock = true;
						}
					}
					else
					{
						currentColumn.columnStopYPos[i] = 0;
						this.gridBlocks[currentColumn.blockXPos[i], 0].hasBlock = true;
					}
				}
			}
			base.StartCoroutine(this.drop_block_cr(currentColumn, currentColumn.columnStopYPos[i], i));
		}
		while (currentColumn.blockCounter < currentColumn.blockPieces.Count)
		{
			yield return null;
		}
		currentColumn.blockPieces.Clear();
		currentColumn.transform.DetachChildren();
		UnityEngine.Object.Destroy(currentColumn.gameObject);
		this.doneDropping = false;
		this.checkAgain = false;
		base.StartCoroutine(this.check_to_drop_blocks());
		while (!this.doneDropping)
		{
			yield return null;
		}
		this.CheckFullGrid();
		this.ScaleCheck();
		this.CheckForTop();
		base.StartCoroutine(this.check_to_drop_blocks());
		this.CheckFullGrid();
		this.ScaleCheck();
		yield break;
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x000FDE74 File Offset: 0x000FC274
	private IEnumerator drop_block_cr(DicePalaceCardLevelColumn currentColumn, int indexToDropTo, int blockToDrop)
	{
		while (currentColumn.blockPieces[blockToDrop].transform.position.y > this.gridBlocks[currentColumn.blockXPos[blockToDrop], indexToDropTo].transform.position.y)
		{
			Vector3 pos = currentColumn.blockPieces[blockToDrop].transform.position;
			pos.y = Mathf.MoveTowards(currentColumn.blockPieces[blockToDrop].transform.position.y, this.gridBlocks[currentColumn.blockXPos[blockToDrop], indexToDropTo].transform.position.y, this.properties.blockDropSpeed * CupheadTime.Delta);
			currentColumn.blockPieces[blockToDrop].transform.position = pos;
			yield return null;
		}
		currentColumn.blockPieces[blockToDrop].transform.parent = base.transform;
		this.gridBlocks[currentColumn.blockXPos[blockToDrop], indexToDropTo].blockHeld = currentColumn.blockPieces[blockToDrop];
		currentColumn.blockCounter++;
		yield break;
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x000FDEA4 File Offset: 0x000FC2A4
	private IEnumerator check_to_drop_blocks()
	{
		for (int i = 0; i < this.GridDimX; i++)
		{
			for (int j = this.GridDimY - 1; j >= 0; j--)
			{
				if (this.gridBlocks[i, j].hasBlock && this.gridBlocks[i, j].Ycoordinate > 0f)
				{
					int num = j - 1;
					int num2 = j + 1;
					DicePalaceCardLevelBlock blockHeld = this.gridBlocks[i, j].blockHeld;
					if (!this.gridBlocks[i, num].hasBlock)
					{
						this.checkAgain = true;
						this.CheckFullGrid();
						base.StartCoroutine(this.drop_current_cr(i, j, num, blockHeld));
						if (this.gridBlocks[i, num2].hasBlock && this.gridBlocks[i, num2].Ycoordinate < (float)this.GridDimY)
						{
							base.StartCoroutine(this.drop_current_cr(i, num2, j, this.gridBlocks[i, num2].blockHeld));
							num2++;
						}
					}
					else
					{
						this.checkAgain = false;
					}
				}
			}
		}
		if (!this.checkAgain)
		{
			this.doneDropping = true;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x000FDEC0 File Offset: 0x000FC2C0
	private IEnumerator drop_current_cr(int x, int y, int spaceBelow, DicePalaceCardLevelBlock block)
	{
		if (this.gridBlocks[x, y].blockHeld != null && !this.gridBlocks[x, spaceBelow].hasBlock)
		{
			if (y >= 0 && this.gridBlocks[x, y].hasBlock)
			{
				this.gridBlocks[x, y].hasBlock = false;
				this.gridBlocks[x, spaceBelow].hasBlock = true;
			}
			while (this.gridBlocks[x, y].blockHeld.transform.position.y != this.gridBlocks[x, spaceBelow].transform.position.y)
			{
				Vector3 pos = this.gridBlocks[x, y].blockHeld.transform.position;
				pos.y = Mathf.MoveTowards(this.gridBlocks[x, y].blockHeld.transform.position.y, this.gridBlocks[x, spaceBelow].transform.position.y, this.properties.blockDropSpeed * CupheadTime.Delta);
				this.gridBlocks[x, y].blockHeld.transform.position = pos;
				yield return null;
			}
			this.gridBlocks[x, y].blockHeld = null;
			this.gridBlocks[x, spaceBelow].blockHeld = block;
			this.CheckFullGrid();
			base.StartCoroutine(this.check_to_drop_blocks());
			this.ScaleCheck();
		}
		yield break;
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x000FDEF8 File Offset: 0x000FC2F8
	public void GenerateGrid()
	{
		this.gridBlocks = new DicePalaceCardLevelGridBlock[this.GridDimX, this.GridDimY];
		for (int i = 0; i < this.GridDimX; i++)
		{
			for (int j = 0; j < this.GridDimY; j++)
			{
				Vector3 a = new Vector3((float)i * this.GridSpacing, (float)j * this.GridSpacing);
				this.gridBlocks[i, j] = UnityEngine.Object.Instantiate<DicePalaceCardLevelGridBlock>(this.gridBlockPrefab);
				this.gridBlocks[i, j].transform.position = a + base.transform.position;
				this.gridBlocks[i, j].transform.parent = base.transform;
				this.gridBlocks[i, j].Xcoordinate = (float)i;
				this.gridBlocks[i, j].Ycoordinate = (float)j;
			}
		}
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x000FDFE8 File Offset: 0x000FC3E8
	private void CheckFullGrid()
	{
		for (int i = 0; i < this.GridDimX; i++)
		{
			for (int j = 0; j < this.GridDimY; j++)
			{
				if (this.gridBlocks[i, j].blockHeld != null)
				{
					if (this.gridBlocks[i, j].Xcoordinate < (float)(this.GridDimX - 2) && this.gridBlocks[i, j].Ycoordinate < (float)(this.GridDimY - 2) && this.gridBlocks[i, j].hasBlock)
					{
						this.DiagonalsUpCheck(i, j, this.gridBlocks[i, j].blockHeld.suit);
					}
					if (this.gridBlocks[i, j].Xcoordinate < (float)(this.GridDimX - 2) && this.gridBlocks[i, j].Ycoordinate >= 2f && this.gridBlocks[i, j].hasBlock)
					{
						this.DiagonalsDownCheck(i, j, this.gridBlocks[i, j].blockHeld.suit);
					}
					if (this.gridBlocks[i, j].Xcoordinate < (float)(this.GridDimX - 2) && this.gridBlocks[i, j].hasBlock)
					{
						this.RowsCheck(i, j, this.gridBlocks[i, j].blockHeld.suit);
					}
					if (this.gridBlocks[i, j].Ycoordinate < (float)(this.GridDimY - 2) && this.gridBlocks[i, j].hasBlock)
					{
						this.ColumnsCheck(i, j, this.gridBlocks[i, j].blockHeld.suit, true);
					}
				}
			}
		}
	}

	// Token: 0x06001BDA RID: 7130 RVA: 0x000FE1D8 File Offset: 0x000FC5D8
	private void RowsCheck(int x, int y, DicePalaceCardLevelBlock.Suit suit)
	{
		int num = x + 1;
		int num2 = num + 1;
		int i = num2 + 1;
		if (this.gridBlocks[num, y].blockHeld != null && this.gridBlocks[num2, y].blockHeld != null && this.gridBlocks[num, y].blockHeld.suit == suit && this.gridBlocks[num2, y].blockHeld.suit == suit)
		{
			this.DeleteBlock(this.gridBlocks[x, y]);
			this.DeleteBlock(this.gridBlocks[num, y]);
			this.DeleteBlock(this.gridBlocks[num2, y]);
			if (y < this.GridDimY - 2)
			{
				this.ColumnsCheck(x, y, suit, true);
				this.ColumnsCheck(num, y, suit, true);
				this.ColumnsCheck(num2, y, suit, true);
			}
			if (y >= 2)
			{
				this.ColumnsCheck(x, y, suit, false);
				this.ColumnsCheck(num, y, suit, false);
				this.ColumnsCheck(num2, y, suit, false);
			}
			if (num2 < this.GridDimX - 2)
			{
				this.DiagonalsUpCheck(x, y, suit);
				this.DiagonalsUpCheck(num, y, suit);
				this.DiagonalsUpCheck(num2, y, suit);
			}
			if (y >= 2 && x < this.GridDimX - 2)
			{
				this.DiagonalsDownCheck(x, y, suit);
				this.DiagonalsDownCheck(num, y, suit);
				this.DiagonalsDownCheck(num2, y, suit);
			}
			while (i <= this.GridDimX - 1)
			{
				if (!this.gridBlocks[i, y].hasBlock || this.gridBlocks[i, y].blockHeld.suit != suit)
				{
					break;
				}
				this.DeleteBlock(this.gridBlocks[i, y]);
				if (i >= this.GridDimX)
				{
					break;
				}
				i++;
			}
		}
	}

	// Token: 0x06001BDB RID: 7131 RVA: 0x000FE3C8 File Offset: 0x000FC7C8
	private void ColumnsCheck(int x, int y, DicePalaceCardLevelBlock.Suit suit, bool checkingUp)
	{
		int num = y + 1;
		int num2 = num + 1;
		int num3 = y - 1;
		int num4 = y - 2;
		int num5;
		int num6;
		if (checkingUp)
		{
			num5 = num;
			num6 = num2;
		}
		else
		{
			num5 = num3;
			num6 = num4;
		}
		if (this.gridBlocks[x, num5].blockHeld != null && this.gridBlocks[x, num6].blockHeld != null && this.gridBlocks[x, num5].blockHeld.suit == suit && this.gridBlocks[x, num6].blockHeld.suit == suit)
		{
			this.DeleteBlock(this.gridBlocks[x, y]);
			this.DeleteBlock(this.gridBlocks[x, num5]);
			this.DeleteBlock(this.gridBlocks[x, num6]);
			if (x < this.GridDimX - 2)
			{
				if (y >= 2)
				{
					this.DiagonalsDownCheck(x, y, suit);
				}
				if (num >= 2)
				{
					this.DiagonalsDownCheck(x, num, suit);
				}
				if (num2 >= 2)
				{
					this.DiagonalsDownCheck(x, num2, suit);
				}
				this.RowsCheck(x, y, suit);
				this.RowsCheck(x, num, suit);
				this.RowsCheck(x, num2, suit);
				if (num2 < this.GridDimY - 2)
				{
					this.DiagonalsUpCheck(x, y, suit);
					this.DiagonalsUpCheck(x, num, suit);
					this.DiagonalsUpCheck(x, num2, suit);
				}
			}
			this.ExtraCheck(x, y, checkingUp, suit);
		}
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x000FE53C File Offset: 0x000FC93C
	private void DiagonalsUpCheck(int x, int y, DicePalaceCardLevelBlock.Suit suit)
	{
		int num = x + 1;
		int num2 = num + 1;
		int num3 = num2 + 1;
		int num4 = y + 1;
		int num5 = num4 + 1;
		int num6 = num5 + 1;
		if (this.gridBlocks[num, num4].blockHeld != null && this.gridBlocks[num2, num5].blockHeld != null && this.gridBlocks[num, num4].blockHeld.suit == suit && this.gridBlocks[num2, num5].blockHeld.suit == suit)
		{
			this.DeleteBlock(this.gridBlocks[x, y]);
			this.DeleteBlock(this.gridBlocks[num, num4]);
			this.DeleteBlock(this.gridBlocks[num2, num5]);
			if (num2 < this.GridDimX - 2)
			{
				this.RowsCheck(x, y, suit);
				this.RowsCheck(num, num4, suit);
				this.RowsCheck(num2, num5, suit);
			}
			if (y >= 2)
			{
				this.ColumnsCheck(x, y, suit, false);
			}
			if (num4 >= 2)
			{
				this.ColumnsCheck(num, num4, suit, false);
			}
			this.ColumnsCheck(num2, num5, suit, false);
			if (num5 >= 2)
			{
			}
			while (num3 <= this.GridDimX - 1 && num6 <= this.GridDimY - 1)
			{
				if (!this.gridBlocks[num3, num6].hasBlock || this.gridBlocks[num3, num6].blockHeld.suit != suit)
				{
					break;
				}
				this.DeleteBlock(this.gridBlocks[num3, num6]);
				if (num3 >= this.GridDimX || num6 >= this.GridDimY)
				{
					break;
				}
				num3++;
				num6++;
			}
		}
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x000FE718 File Offset: 0x000FCB18
	private void DiagonalsDownCheck(int x, int y, DicePalaceCardLevelBlock.Suit suit)
	{
		int num = x + 1;
		int num2 = num + 1;
		int num3 = num2 + 1;
		int num4 = y - 1;
		int num5 = num4 - 1;
		int num6 = num5 - 1;
		if (this.gridBlocks[num, num4].blockHeld != null && this.gridBlocks[num2, num5].blockHeld != null && this.gridBlocks[num, num4].blockHeld.suit == suit && this.gridBlocks[num2, num5].blockHeld.suit == suit)
		{
			this.DeleteBlock(this.gridBlocks[x, y]);
			this.DeleteBlock(this.gridBlocks[num, num4]);
			this.DeleteBlock(this.gridBlocks[num2, num5]);
			if (num2 < this.GridDimX - 2)
			{
				this.RowsCheck(x, y, suit);
				this.RowsCheck(num, num4, suit);
				this.RowsCheck(num2, num5, suit);
			}
			if (num4 >= 2)
			{
				this.ColumnsCheck(num, num4, suit, false);
			}
			if (num5 >= 2)
			{
				this.ColumnsCheck(num2, num5, suit, false);
			}
			this.ColumnsCheck(x, y, suit, false);
			while (num3 <= this.GridDimX - 1 && num6 >= 1)
			{
				if (!this.gridBlocks[num3, num6].hasBlock || this.gridBlocks[num3, num6].blockHeld.suit != suit)
				{
					break;
				}
				this.DeleteBlock(this.gridBlocks[num3, num6]);
				if (num3 >= this.GridDimX || num6 <= 1)
				{
					break;
				}
				num3++;
				num6--;
			}
		}
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x000FE8E0 File Offset: 0x000FCCE0
	private void ExtraCheck(int x, int y, bool checkingUp, DicePalaceCardLevelBlock.Suit suit)
	{
		int i;
		int num;
		if (checkingUp)
		{
			i = y + 1;
			num = 1;
		}
		else
		{
			i = y - 1;
			num = -1;
		}
		while (i <= this.GridDimY - 1)
		{
			if (!this.gridBlocks[x, i].hasBlock || this.gridBlocks[x, i].blockHeld.suit != suit)
			{
				break;
			}
			this.DeleteBlock(this.gridBlocks[x, i]);
			if (i >= this.GridDimY)
			{
				break;
			}
			i += num;
		}
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x000FE988 File Offset: 0x000FCD88
	private void ScaleCheck()
	{
		Vector3 position = base.transform.position;
		int i = 0;
		float y = 0f;
		bool flag = false;
		for (int j = 0; j < this.GridDimY; j++)
		{
			for (int k = 0; k < this.GridDimX; k++)
			{
				if (this.gridBlocks[k, j].blockHeld != null && (float)j != this.currentHeight)
				{
					y = (float)j;
					flag = true;
					this.currentHeight = (float)j;
				}
			}
			while (i < this.GridDimX - 1)
			{
				if (!(this.gridBlocks[i, j].blockHeld != null))
				{
					break;
				}
				i++;
				if (i == this.GridDimX - 1)
				{
					y = (float)j;
					flag = true;
				}
			}
		}
		if (flag)
		{
			base.StartCoroutine(this.move_scale_cr(y));
		}
		base.transform.position = position;
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x000FEA94 File Offset: 0x000FCE94
	private IEnumerator move_scale_cr(float y)
	{
		Vector3 pos = base.transform.position;
		float speed = 200f;
		this.targetPos = this.startingPos - this.gridBlockPrefab.GetComponent<Renderer>().bounds.size.y * (y + 1f);
		while (base.transform.position.y != this.targetPos)
		{
			pos.y = Mathf.MoveTowards(base.transform.position.y, this.targetPos, speed * CupheadTime.Delta);
			base.transform.position = pos;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x000FEAB8 File Offset: 0x000FCEB8
	private void CheckForTop()
	{
		for (int i = 0; i < this.GridDimX; i++)
		{
			if (this.gridBlocks[i, this.GridDimY - 1].hasBlock)
			{
				this.KillAllBlocks();
			}
		}
		this.checkAgain = false;
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x000FEB08 File Offset: 0x000FCF08
	private void KillAllBlocks()
	{
		for (int i = 0; i < this.GridDimX; i++)
		{
			for (int j = 0; j < this.GridDimY; j++)
			{
				if (this.gridBlocks[i, j].hasBlock)
				{
					this.DeleteBlock(this.gridBlocks[i, j]);
				}
			}
		}
		this.targetPos = this.startingPos;
		Vector3 position = base.transform.position;
		position.y = this.startingPos;
		base.transform.position = position;
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x000FEB9E File Offset: 0x000FCF9E
	private void DeleteBlock(DicePalaceCardLevelGridBlock gridBlock)
	{
		if (gridBlock.blockHeld != null)
		{
			gridBlock.blockHeld.DestroyBlock();
			gridBlock.blockHeld = null;
			gridBlock.hasBlock = false;
		}
	}

	// Token: 0x040024D8 RID: 9432
	[SerializeField]
	private DicePalaceCardLevelColumn columnObject;

	// Token: 0x040024D9 RID: 9433
	[SerializeField]
	private DicePalaceCardLevelBlock hearts;

	// Token: 0x040024DA RID: 9434
	[SerializeField]
	private DicePalaceCardLevelBlock spades;

	// Token: 0x040024DB RID: 9435
	[SerializeField]
	private DicePalaceCardLevelBlock clubs;

	// Token: 0x040024DC RID: 9436
	[SerializeField]
	private DicePalaceCardLevelBlock diamonds;

	// Token: 0x040024DD RID: 9437
	[SerializeField]
	private DicePalaceCardLevelGridBlock gridBlockPrefab;

	// Token: 0x040024DE RID: 9438
	private List<DicePalaceCardLevelColumn> totalColumns;

	// Token: 0x040024DF RID: 9439
	public DicePalaceCardLevelGridBlock[,] gridBlocks;

	// Token: 0x040024E0 RID: 9440
	private LevelProperties.DicePalaceCard.Blocks properties;

	// Token: 0x040024E1 RID: 9441
	private float distanceToPlayerY;

	// Token: 0x040024E2 RID: 9442
	private float amountToDropBy;

	// Token: 0x040024E3 RID: 9443
	private float startingPos;

	// Token: 0x040024E4 RID: 9444
	private float targetPos;

	// Token: 0x040024E5 RID: 9445
	private float currentHeight = -1f;

	// Token: 0x040024E6 RID: 9446
	public int GridDimX;

	// Token: 0x040024E7 RID: 9447
	public int GridDimY;

	// Token: 0x040024E8 RID: 9448
	private float GridSpacing;

	// Token: 0x040024E9 RID: 9449
	private bool doneDropping;

	// Token: 0x040024EA RID: 9450
	private bool checkAgain = true;

	// Token: 0x040024EB RID: 9451
	private string[] typePattern;

	// Token: 0x040024EC RID: 9452
	private string[] amountPattern;

	// Token: 0x040024ED RID: 9453
	private List<int> currentStopYPos;

	// Token: 0x040024EE RID: 9454
	private int amountIndex;

	// Token: 0x040024EF RID: 9455
	private int typeIndex;

	// Token: 0x040024F0 RID: 9456
	private DicePalaceCardLevelBlock selectedPrefab;
}
