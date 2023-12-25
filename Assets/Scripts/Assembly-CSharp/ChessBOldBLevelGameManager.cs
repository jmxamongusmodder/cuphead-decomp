using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052E RID: 1326
public class ChessBOldBLevelGameManager : AbstractPausableComponent
{
	// Token: 0x17000330 RID: 816
	// (get) Token: 0x060017FE RID: 6142 RVA: 0x000D8C87 File Offset: 0x000D7087
	// (set) Token: 0x060017FF RID: 6143 RVA: 0x000D8C8F File Offset: 0x000D708F
	public bool WaitingForParry { get; private set; }

	// Token: 0x06001800 RID: 6144 RVA: 0x000D8C98 File Offset: 0x000D7098
	public void SetupGameManager(LevelProperties.ChessBOldB properties)
	{
		this.properties = properties;
		this.goingClockwise = Rand.Bool();
		this.InitBalls();
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x000D8CB4 File Offset: 0x000D70B4
	private void InitBalls()
	{
		this.birdies = new ChessBOldBReduxLevelBirdie[6];
		Vector3 position = new Vector3(0f, 1000f);
		for (int i = 0; i < 6; i++)
		{
			this.birdies[i] = UnityEngine.Object.Instantiate<ChessBOldBReduxLevelBirdie>(this.birdiePrefab);
			this.birdies[i].transform.position = position;
			this.birdies[i].ParryBirdie = new ChessBOldBReduxLevelBirdie.OnParryBirdie(this.ParriedBall);
		}
		base.StartCoroutine(this.game_cr());
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x000D8D3C File Offset: 0x000D713C
	private IEnumerator game_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 4f);
		LevelProperties.ChessBOldB.Birdie p = this.properties.CurrentState.birdie;
		YieldInstruction wait = new WaitForFixedUpdate();
		this.OnStateChanged();
		float t = 0f;
		float spinTime = 0f;
		float speedTime = 0f;
		int timesToSwitchDir = 0;
		for (;;)
		{
			t = 0f;
			yield return base.StartCoroutine(this.flash_balls_cr());
			this.WaitingForParry = true;
			p = this.properties.CurrentState.birdie;
			this.spinSpeedString = p.spinSpeedString[this.spinSpeedStringMainIndex].Split(new char[]
			{
				','
			});
			this.spinTimeString = p.spinTimeString[this.spinTimeStringMainIndex].Split(new char[]
			{
				','
			});
			this.changeDirString = p.changeDirectionString[this.changeDirStringMainIndex].Split(new char[]
			{
				','
			});
			this.initialDirString = p.initialDirectionString[this.initialDirStringMainIndex].Split(new char[]
			{
				','
			});
			Parser.FloatTryParse(this.spinTimeString[this.spinTimeStringIndex], out spinTime);
			Parser.FloatTryParse(this.spinSpeedString[this.spinSpeedStringIndex], out speedTime);
			Parser.IntTryParse(this.changeDirString[this.changeDirStringIndex], out timesToSwitchDir);
			this.goingClockwise = (this.initialDirString[this.initialDirStringIndex][0] == 'R');
			for (int i = 0; i < this.birdies.Length; i++)
			{
				this.birdies[i].HandleMovement(speedTime, this.goingClockwise);
			}
			float timeUntilSwitch = spinTime / (float)timesToSwitchDir;
			int dirCounter = 0;
			bool turnedPink = false;
			while (t < spinTime)
			{
				if (!this.WaitingForParry)
				{
					break;
				}
				t += CupheadTime.FixedDelta;
				if (!turnedPink && t >= p.prePinkTime)
				{
					for (int j = 0; j < this.birdies.Length; j++)
					{
						this.birdies[j].TurnPink();
					}
					turnedPink = true;
				}
				if (dirCounter < timesToSwitchDir && t > timeUntilSwitch * (float)dirCounter)
				{
					dirCounter++;
					this.goingClockwise = !this.goingClockwise;
					for (int k = 0; k < this.birdies.Length; k++)
					{
						this.birdies[k].HandleMovement(speedTime, this.goingClockwise);
					}
				}
				yield return wait;
			}
			for (int l = 0; l < this.birdies.Length; l++)
			{
				this.birdies[l].StopMoving();
			}
			while (this.WaitingForParry)
			{
				yield return null;
			}
			if (this.spinSpeedStringIndex < this.spinSpeedString.Length - 1)
			{
				this.spinSpeedStringIndex++;
			}
			else
			{
				this.spinSpeedStringMainIndex = (this.spinSpeedStringMainIndex + 1) % p.spinSpeedString.Length;
				this.spinSpeedStringIndex = 0;
			}
			if (this.spinTimeStringIndex < this.spinTimeString.Length - 1)
			{
				this.spinTimeStringIndex++;
			}
			else
			{
				this.spinTimeStringMainIndex = (this.spinTimeStringMainIndex + 1) % p.spinTimeString.Length;
				this.spinTimeStringIndex = 0;
			}
			if (this.changeDirStringIndex < this.changeDirString.Length - 1)
			{
				this.changeDirStringIndex++;
			}
			else
			{
				this.changeDirStringMainIndex = (this.changeDirStringMainIndex + 1) % p.changeDirectionString.Length;
				this.changeDirStringIndex = 0;
			}
			if (this.initialDirStringIndex < this.initialDirString.Length - 1)
			{
				this.initialDirStringIndex++;
			}
			else
			{
				this.initialDirStringMainIndex = (this.initialDirStringMainIndex + 1) % p.initialDirectionString.Length;
				this.initialDirStringIndex = 0;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x000D8D57 File Offset: 0x000D7157
	private void ParriedBall(bool chosenBall)
	{
		if (chosenBall)
		{
			this.WaitingForParry = false;
			this.properties.DealDamage(1f);
			this.boss.HandleHurt(true);
		}
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x000D8D84 File Offset: 0x000D7184
	public void OnStateChanged()
	{
		LevelProperties.ChessBOldB.Birdie birdie = this.properties.CurrentState.birdie;
		this.spinSpeedStringMainIndex = UnityEngine.Random.Range(0, birdie.spinSpeedString.Length);
		this.spinSpeedString = birdie.spinSpeedString[this.spinSpeedStringMainIndex].Split(new char[]
		{
			','
		});
		this.spinSpeedStringIndex = UnityEngine.Random.Range(0, this.spinSpeedString.Length);
		this.spinTimeStringMainIndex = UnityEngine.Random.Range(0, birdie.spinTimeString.Length);
		this.spinTimeString = birdie.spinTimeString[this.spinTimeStringMainIndex].Split(new char[]
		{
			','
		});
		this.spinTimeStringIndex = UnityEngine.Random.Range(0, this.spinTimeString.Length);
		this.changeDirStringMainIndex = UnityEngine.Random.Range(0, birdie.changeDirectionString.Length);
		this.changeDirString = birdie.changeDirectionString[this.changeDirStringMainIndex].Split(new char[]
		{
			','
		});
		this.changeDirStringIndex = UnityEngine.Random.Range(0, this.changeDirString.Length);
		this.initialDirStringMainIndex = UnityEngine.Random.Range(0, birdie.initialDirectionString.Length);
		this.initialDirString = birdie.initialDirectionString[this.initialDirStringMainIndex].Split(new char[]
		{
			','
		});
		this.initialDirStringIndex = UnityEngine.Random.Range(0, this.initialDirString.Length);
		this.chosenStringMainIndex = UnityEngine.Random.Range(0, birdie.chosenString.Length);
		this.chosenString = birdie.chosenString[this.chosenStringMainIndex].Split(new char[]
		{
			','
		});
		this.chosenStringIndex = UnityEngine.Random.Range(0, this.chosenString.Length);
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x000D8F1C File Offset: 0x000D731C
	private IEnumerator flash_balls_cr()
	{
		LevelProperties.ChessBOldB.Birdie p = this.properties.CurrentState.birdie;
		for (int i = 0; i < this.birdies.Length; i++)
		{
			this.birdies[i].transform.position = new Vector3(0f, 1000f);
		}
		yield return CupheadTime.WaitForSeconds(this, p.fadeInTime);
		this.boss.HandleHurt(false);
		float angleOffset = 60f;
		int chosenIndex = 0;
		this.chosenString = p.chosenString[this.chosenStringMainIndex].Split(new char[]
		{
			','
		});
		Parser.IntTryParse(this.chosenString[this.chosenStringIndex], out chosenIndex);
		for (int j = 0; j < this.birdies.Length; j++)
		{
			bool chosenBall = j == chosenIndex;
			this.birdies[j].Setup(this.pivotPoint, angleOffset * (float)j, this.properties.CurrentState.birdie, 370f, chosenBall);
		}
		Color color = this.birdies[chosenIndex].GetComponent<SpriteRenderer>().color;
		this.birdies[chosenIndex].GetComponent<SpriteRenderer>().color = this.redFlash;
		yield return CupheadTime.WaitForSeconds(this, p.flashTime);
		this.birdies[chosenIndex].GetComponent<SpriteRenderer>().color = color;
		if (this.chosenStringIndex < this.chosenString.Length - 1)
		{
			this.chosenStringIndex++;
		}
		else
		{
			this.chosenStringMainIndex = (this.chosenStringMainIndex + 1) % p.chosenString.Length;
			this.chosenStringIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x04002126 RID: 8486
	private const int NUM_OF_BALLS = 6;

	// Token: 0x04002127 RID: 8487
	private const float LOOP_SIZE = 370f;

	// Token: 0x04002128 RID: 8488
	[SerializeField]
	private Color redFlash;

	// Token: 0x04002129 RID: 8489
	[SerializeField]
	private Transform pivotPoint;

	// Token: 0x0400212A RID: 8490
	[SerializeField]
	private ChessBOldBLevelBoss boss;

	// Token: 0x0400212B RID: 8491
	[SerializeField]
	private ChessBOldBReduxLevelBirdie birdiePrefab;

	// Token: 0x0400212C RID: 8492
	private ChessBOldBReduxLevelBirdie[] birdies;

	// Token: 0x0400212D RID: 8493
	private LevelProperties.ChessBOldB properties;

	// Token: 0x0400212F RID: 8495
	private bool goingClockwise;

	// Token: 0x04002130 RID: 8496
	private int spinSpeedStringMainIndex;

	// Token: 0x04002131 RID: 8497
	private string[] spinSpeedString;

	// Token: 0x04002132 RID: 8498
	private int spinSpeedStringIndex;

	// Token: 0x04002133 RID: 8499
	private int spinTimeStringMainIndex;

	// Token: 0x04002134 RID: 8500
	private string[] spinTimeString;

	// Token: 0x04002135 RID: 8501
	private int spinTimeStringIndex;

	// Token: 0x04002136 RID: 8502
	private int changeDirStringMainIndex;

	// Token: 0x04002137 RID: 8503
	private string[] changeDirString;

	// Token: 0x04002138 RID: 8504
	private int changeDirStringIndex;

	// Token: 0x04002139 RID: 8505
	private int initialDirStringMainIndex;

	// Token: 0x0400213A RID: 8506
	private string[] initialDirString;

	// Token: 0x0400213B RID: 8507
	private int initialDirStringIndex;

	// Token: 0x0400213C RID: 8508
	private int chosenStringMainIndex;

	// Token: 0x0400213D RID: 8509
	private string[] chosenString;

	// Token: 0x0400213E RID: 8510
	private int chosenStringIndex;
}
