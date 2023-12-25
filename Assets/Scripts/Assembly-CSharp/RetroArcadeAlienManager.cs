using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000737 RID: 1847
public class RetroArcadeAlienManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06002836 RID: 10294 RVA: 0x001770C9 File Offset: 0x001754C9
	// (set) Token: 0x06002837 RID: 10295 RVA: 0x001770D1 File Offset: 0x001754D1
	public RetroArcadeAlien.Direction direction { get; private set; }

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06002838 RID: 10296 RVA: 0x001770DA File Offset: 0x001754DA
	// (set) Token: 0x06002839 RID: 10297 RVA: 0x001770E2 File Offset: 0x001754E2
	public float moveSpeed { get; private set; }

	// Token: 0x0600283A RID: 10298 RVA: 0x001770EC File Offset: 0x001754EC
	public void StartAliens()
	{
		this.p = base.properties.CurrentState.aliens;
		this.aliens = new RetroArcadeAlien[this.p.numColumns, this.alienPrefabs.Length];
		this.direction = ((!Rand.Bool()) ? RetroArcadeAlien.Direction.Right : RetroArcadeAlien.Direction.Left);
		for (int i = 0; i < this.aliens.GetLength(0); i++)
		{
			for (int j = 0; j < this.aliens.GetLength(1); j++)
			{
				Vector2 position = new Vector2(50f * ((float)i - (float)(this.aliens.GetLength(0) - 1) / 2f), 230f - (float)j * 40f + 170f);
				this.aliens[i, j] = this.alienPrefabs[j].Create(position, i, this, this.p);
				this.aliens[i, j].MoveY(-170f);
			}
		}
		this.numDied = 0;
		this.moveSpeed = 640f / this.p.moveTime;
		this.shotRate = this.p.shotRate.Clone();
		this.currentTopRowY = 230f;
		base.StartCoroutine(this.turn_cr());
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.randomShot_cr());
		base.StartCoroutine(this.bonus_cr());
	}

	// Token: 0x0600283B RID: 10299 RVA: 0x00177264 File Offset: 0x00175664
	private IEnumerator turn_cr()
	{
		for (;;)
		{
			if ((this.direction == RetroArcadeAlien.Direction.Right && this.getRightmost().transform.position.x > 320f) || (this.direction == RetroArcadeAlien.Direction.Left && this.getLeftmost().transform.position.x < -320f))
			{
				this.direction = ((this.direction != RetroArcadeAlien.Direction.Left) ? RetroArcadeAlien.Direction.Left : RetroArcadeAlien.Direction.Right);
				float num = -40f;
				if (this.currentTopRowY - (float)(this.aliens.GetLength(1) - 1) * 40f + num < -40f)
				{
					num = 230f - this.currentTopRowY;
				}
				for (int i = 0; i < this.aliens.GetLength(0); i++)
				{
					if (this.isColumnAlive(i))
					{
						for (int j = 0; j < this.aliens.GetLength(1); j++)
						{
							this.aliens[i, j].MoveY(num);
						}
					}
				}
				this.currentTopRowY += num;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x0600283C RID: 10300 RVA: 0x00177280 File Offset: 0x00175680
	private IEnumerator shoot_cr()
	{
		string[] columnPattern = this.p.shotColumnPattern.RandomChoice<string>().Split(new char[]
		{
			','
		});
		int columnPatternIndex = UnityEngine.Random.Range(0, columnPattern.Length);
		yield return CupheadTime.WaitForSeconds(this, this.shotRate.RandomFloat());
		for (;;)
		{
			columnPatternIndex = (columnPatternIndex + 1) % columnPattern.Length;
			int column = 0;
			Parser.IntTryParse(columnPattern[columnPatternIndex], out column);
			column--;
			if (this.isColumnAlive(column))
			{
				this.getBottommostInColumn(column).Shoot();
				yield return CupheadTime.WaitForSeconds(this, this.shotRate.RandomFloat());
			}
		}
		yield break;
	}

	// Token: 0x0600283D RID: 10301 RVA: 0x0017729C File Offset: 0x0017569C
	private IEnumerator randomShot_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, MathUtils.ExpRandom(this.p.randomShotAverageTime));
		for (;;)
		{
			int column = UnityEngine.Random.Range(0, this.aliens.GetLength(0));
			while (!this.isColumnAlive(column))
			{
				column = UnityEngine.Random.Range(0, this.aliens.GetLength(0));
			}
			this.getBottommostInColumn(column).Shoot();
			yield return CupheadTime.WaitForSeconds(this, MathUtils.ExpRandom(this.p.randomShotAverageTime));
		}
		yield break;
	}

	// Token: 0x0600283E RID: 10302 RVA: 0x001772B8 File Offset: 0x001756B8
	private IEnumerator bonus_cr()
	{
		for (int i = 0; i < this.p.bonusAppearCount; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, this.p.bonusAppearTime.RandomFloat());
			this.bonusAlien.Create((!Rand.Bool()) ? RetroArcadeBonusAlien.Direction.Right : RetroArcadeBonusAlien.Direction.Left, this.p);
		}
		yield break;
	}

	// Token: 0x0600283F RID: 10303 RVA: 0x001772D4 File Offset: 0x001756D4
	private RetroArcadeAlien getLeftmost()
	{
		for (int i = 0; i < this.aliens.GetLength(0); i++)
		{
			for (int j = 0; j < this.aliens.GetLength(1); j++)
			{
				if (!this.aliens[i, j].IsDead)
				{
					return this.aliens[i, j];
				}
			}
		}
		return null;
	}

	// Token: 0x06002840 RID: 10304 RVA: 0x00177344 File Offset: 0x00175744
	private RetroArcadeAlien getRightmost()
	{
		for (int i = this.aliens.GetLength(0) - 1; i >= 0; i--)
		{
			for (int j = 0; j < this.aliens.GetLength(1); j++)
			{
				if (!this.aliens[i, j].IsDead)
				{
					return this.aliens[i, j];
				}
			}
		}
		return null;
	}

	// Token: 0x06002841 RID: 10305 RVA: 0x001773B4 File Offset: 0x001757B4
	private RetroArcadeAlien getTopmost()
	{
		for (int i = 0; i < this.aliens.GetLength(1); i++)
		{
			for (int j = 0; j < this.aliens.GetLength(0); j++)
			{
				if (!this.aliens[j, i].IsDead)
				{
					return this.aliens[j, i];
				}
			}
		}
		return null;
	}

	// Token: 0x06002842 RID: 10306 RVA: 0x00177424 File Offset: 0x00175824
	private RetroArcadeAlien getBottommost()
	{
		for (int i = this.aliens.GetLength(1) - 1; i >= 0; i--)
		{
			for (int j = 0; j < this.aliens.GetLength(0); j++)
			{
				if (!this.aliens[j, i].IsDead)
				{
					return this.aliens[j, i];
				}
			}
		}
		return null;
	}

	// Token: 0x06002843 RID: 10307 RVA: 0x00177494 File Offset: 0x00175894
	private bool isColumnAlive(int x)
	{
		for (int i = 0; i < this.aliens.GetLength(1); i++)
		{
			if (!this.aliens[x, i].IsDead)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002844 RID: 10308 RVA: 0x001774D8 File Offset: 0x001758D8
	private RetroArcadeAlien getBottommostInColumn(int x)
	{
		for (int i = this.aliens.GetLength(1) - 1; i >= 0; i--)
		{
			if (!this.aliens[x, i].IsDead)
			{
				return this.aliens[x, i];
			}
		}
		return null;
	}

	// Token: 0x06002845 RID: 10309 RVA: 0x0017752C File Offset: 0x0017592C
	public void OnAlienDie(RetroArcadeAlien alien)
	{
		this.numDied++;
		this.moveSpeed = 640f / (this.p.moveTime - (float)this.numDied * this.p.moveTimeDecrease);
		this.shotRate.max -= this.p.shotRateDecrease;
		this.shotRate.min -= this.p.shotRateDecrease;
		if (!this.isColumnAlive(alien.ColumnIndex))
		{
			for (int i = 0; i < this.aliens.GetLength(1); i++)
			{
				this.aliens[alien.ColumnIndex, i].MoveY(170f + (230f - this.aliens[alien.ColumnIndex, 0].transform.position.y));
			}
		}
		if (this.numDied >= this.aliens.Length)
		{
			this.StopAllCoroutines();
			base.properties.DealDamageToNextNamedState();
			base.StartCoroutine(this.waveOver_cr());
		}
	}

	// Token: 0x06002846 RID: 10310 RVA: 0x00177658 File Offset: 0x00175A58
	private IEnumerator waveOver_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		RetroArcadeAlien[,] array = this.aliens;
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				RetroArcadeAlien retroArcadeAlien = array[i, j];
				UnityEngine.Object.Destroy(retroArcadeAlien.gameObject);
			}
		}
		yield break;
	}

	// Token: 0x040030FE RID: 12542
	private const float TOP_ROW_Y = 230f;

	// Token: 0x040030FF RID: 12543
	private const float COLUMN_SPACING = 50f;

	// Token: 0x04003100 RID: 12544
	private const float ROW_SPACING = 40f;

	// Token: 0x04003101 RID: 12545
	private const float TURNAROUND_X = 320f;

	// Token: 0x04003102 RID: 12546
	private const float MIN_Y = -40f;

	// Token: 0x04003103 RID: 12547
	private const float OFFSCREEN_MOVE_Y = 170f;

	// Token: 0x04003104 RID: 12548
	[SerializeField]
	private RetroArcadeAlien[] alienPrefabs;

	// Token: 0x04003105 RID: 12549
	private RetroArcadeAlien[,] aliens;

	// Token: 0x04003106 RID: 12550
	[SerializeField]
	private RetroArcadeBonusAlien bonusAlien;

	// Token: 0x04003109 RID: 12553
	private MinMax shotRate;

	// Token: 0x0400310A RID: 12554
	private int numDied;

	// Token: 0x0400310B RID: 12555
	private float currentTopRowY;

	// Token: 0x0400310C RID: 12556
	private LevelProperties.RetroArcade.Aliens p;
}
