using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000528 RID: 1320
public class ChessBOldALevelBishop : LevelProperties.ChessBOldA.Entity
{
	// Token: 0x060017C8 RID: 6088 RVA: 0x000D5EF6 File Offset: 0x000D42F6
	private void Start()
	{
		this.walls = new List<ChessBOldALevelWall>();
		this.pink.OnActivate += this.GotParried;
		this.damageDealer = DamageDealer.NewEnemy();
		base.GetComponent<SpriteRenderer>().color = Color.red;
	}

	// Token: 0x060017C9 RID: 6089 RVA: 0x000D5F35 File Offset: 0x000D4335
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x000D5F53 File Offset: 0x000D4353
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x000D5F6C File Offset: 0x000D436C
	public override void LevelInit(LevelProperties.ChessBOldA properties)
	{
		base.LevelInit(properties);
		Level.Current.OnWinEvent += this.Win;
		LevelProperties.ChessBOldA.Bishop bishop = properties.CurrentState.bishop;
		if (!bishop.canHurtPlayer)
		{
			base.GetComponent<Collider2D>().enabled = false;
		}
		float f = properties.CurrentHealth / (float)properties.CurrentState.bishop.bishopHealth;
		this.HPToDecrease = Mathf.Ceil(f);
		base.transform.SetScale(new float?(bishop.bishopScale), new float?(bishop.bishopScale), new float?(bishop.bishopScale));
		this.pathIndex = 0;
		base.StartCoroutine(this.intro_cr());
		base.StartCoroutine(this.pink_cr());
		this.SetValues();
		base.StartCoroutine(this.turret_cr());
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x000D603E File Offset: 0x000D443E
	private void GotParried()
	{
		base.properties.DealDamage(this.HPToDecrease);
		base.StartCoroutine(this.stunned_cr());
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x000D6060 File Offset: 0x000D4460
	private IEnumerator stunned_cr()
	{
		this.RemoveCurrentWalls();
		this.walls.Clear();
		this.isStunned = true;
		this.pink.enabled = false;
		base.GetComponent<SpriteRenderer>().color = Color.yellow;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bishop.stunnedTime);
		base.GetComponent<SpriteRenderer>().color = Color.red;
		this.pink.enabled = true;
		this.isStunned = false;
		if (base.properties.CurrentHealth > 0f)
		{
			this.phase++;
			this.SetValues();
			this.SetPathValues();
		}
		yield break;
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x000D607B File Offset: 0x000D447B
	private void SetValues()
	{
		this.SetPinkValues();
		this.SetWallValues();
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x000D6089 File Offset: 0x000D4489
	private void Win()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x000D6094 File Offset: 0x000D4494
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 4f);
		this.SetPathValues();
		yield return null;
		yield break;
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x000D60B0 File Offset: 0x000D44B0
	public void SetPathValues()
	{
		LevelProperties.ChessBOldA.BishopPath bishopPath = base.properties.CurrentState.bishopPath;
		int num = Mathf.Clamp(this.phase, 0, bishopPath.pathTypeString.Split(new char[]
		{
			','
		}).Length - 1);
		int num2 = Mathf.Clamp(this.phase, 0, bishopPath.pathSpeedString.Split(new char[]
		{
			','
		}).Length - 1);
		int num3 = Mathf.Clamp(this.phase, 0, bishopPath.pathDirString.Split(new char[]
		{
			','
		}).Length - 1);
		Parser.FloatTryParse(bishopPath.pathSpeedString.Split(new char[]
		{
			','
		})[num2], out this.pathSpeed);
		this.pathIsClockwise = (bishopPath.pathDirString.Split(new char[]
		{
			','
		})[num3][0] == 'R');
		this.previousPathType = this.pathType;
		char c = bishopPath.pathTypeString.Split(new char[]
		{
			','
		})[num][0];
		if (c != 'S')
		{
			if (c != 'I')
			{
				if (c == 'Q')
				{
					this.pathType = ChessBOldALevelBishop.PathType.Square;
					base.StartCoroutine(this.box_cr());
				}
			}
			else
			{
				this.pathType = ChessBOldALevelBishop.PathType.Infinite;
				base.StartCoroutine(this.infinite_cr());
			}
		}
		else
		{
			this.pathType = ChessBOldALevelBishop.PathType.Straight;
			base.StartCoroutine(this.straight_cr());
		}
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x000D6228 File Offset: 0x000D4628
	private IEnumerator move_cr(Vector3 start, Vector3 end)
	{
		float t = 0f;
		float time = this.pathSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(start, end, t / time);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x000D6254 File Offset: 0x000D4654
	private IEnumerator straight_cr()
	{
		LevelProperties.ChessBOldA.BishopPath p = base.properties.CurrentState.bishopPath;
		float t = this.lerpPos;
		float maxTime = this.pathSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		float startX = -p.straightPathLength;
		float endX = p.straightPathLength;
		float one = 1f;
		if (this.previousPathType != ChessBOldALevelBishop.PathType.Straight)
		{
			this.straightValue = ((!this.pathIsClockwise) ? (one - t / maxTime) : (t / maxTime));
			yield return base.StartCoroutine(this.move_cr(base.transform.position, new Vector3(Mathf.Lerp(startX, endX, this.straightValue), p.straightPathHeight)));
		}
		while (!this.isStunned)
		{
			if (t < maxTime)
			{
				t += CupheadTime.FixedDelta;
				this.straightValue = ((!this.pathIsClockwise) ? (one - t / maxTime) : (t / maxTime));
				base.transform.SetPosition(new float?(Mathf.Lerp(startX, endX, this.straightValue)), null, null);
			}
			else
			{
				this.pathIsClockwise = !this.pathIsClockwise;
				t = 0f;
			}
			yield return wait;
		}
		this.lerpPos = t;
		yield return null;
		yield break;
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x000D6270 File Offset: 0x000D4670
	private IEnumerator infinite_cr()
	{
		LevelProperties.ChessBOldA.BishopPath p = base.properties.CurrentState.bishopPath;
		YieldInstruction wait = new WaitForFixedUpdate();
		float loopSizeX = p.infinitePathLength;
		float loopSizeY = p.infinitePathWidth;
		float speed = this.pathSpeed;
		bool invert = this.pathIsClockwise;
		this.pivotPoint.transform.SetPosition(new float?(loopSizeX), new float?(p.infinitePathHeight), null);
		Vector3 endPos = Vector3.zero;
		Vector3 pivotOffset = Vector3.left * 2f * loopSizeX;
		if (this.previousPathType != ChessBOldALevelBishop.PathType.Infinite)
		{
			endPos = ((!invert) ? this.pivotPoint.position : (this.pivotPoint.position + pivotOffset));
			float value = (float)((!invert) ? -1 : 1);
			Vector3 handleRotationX = new Vector3(Mathf.Cos(this.infinityAngle) * value * loopSizeX, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Sin(this.infinityAngle) * loopSizeY, 0f);
			endPos += handleRotationX + handleRotationY;
			yield return base.StartCoroutine(this.move_cr(base.transform.position, endPos));
		}
		while (!this.isStunned)
		{
			this.infinityAngle += speed * CupheadTime.Delta;
			if (this.infinityAngle > 6.2831855f)
			{
				invert = !invert;
				this.infinityAngle -= 6.2831855f;
			}
			if (this.infinityAngle < 0f)
			{
				this.infinityAngle += 6.2831855f;
			}
			float value;
			if (invert)
			{
				base.transform.position = this.pivotPoint.position + pivotOffset;
				value = 1f;
			}
			else
			{
				base.transform.position = this.pivotPoint.position;
				value = -1f;
			}
			Vector3 handleRotationX = new Vector3(Mathf.Cos(this.infinityAngle) * value * loopSizeX, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Sin(this.infinityAngle) * loopSizeY, 0f);
			base.transform.position += handleRotationX + handleRotationY;
			yield return wait;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x000D628C File Offset: 0x000D468C
	private IEnumerator box_cr()
	{
		LevelProperties.ChessBOldA.BishopPath p = base.properties.CurrentState.bishopPath;
		float boxCenter = p.squarePathHeight;
		float length = p.squarePathLength / 2f;
		float height = p.squarePathWidth / 2f;
		Vector3 topLeft = new Vector3(boxCenter - length, boxCenter + height);
		Vector3 topRight = new Vector3(boxCenter + length, boxCenter + height);
		Vector3 bottomLeft = new Vector3(boxCenter - length, boxCenter - height);
		Vector3 bottomRight = new Vector3(boxCenter + length, boxCenter - height);
		Vector3[] positions = new Vector3[]
		{
			topRight,
			bottomRight,
			bottomLeft,
			topLeft
		};
		int incrementBy = (!this.pathIsClockwise) ? -1 : 1;
		float distance = 0f;
		float speed = 0f;
		Vector3 endPos = positions[this.pathIndex];
		if (this.previousPathType != ChessBOldALevelBishop.PathType.Square)
		{
			yield return base.StartCoroutine(this.move_cr(base.transform.position, endPos));
		}
		while (!this.isStunned)
		{
			YieldInstruction wait = new WaitForFixedUpdate();
			distance = Vector3.Distance(base.transform.position, endPos);
			speed = distance / this.pathSpeed;
			while (base.transform.position != endPos)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, endPos, speed * CupheadTime.FixedDelta);
				if (this.isStunned)
				{
					break;
				}
				yield return wait;
			}
			if (!this.isStunned)
			{
				if (this.pathIsClockwise && this.pathIndex >= positions.Length - 1)
				{
					this.pathIndex = 0;
				}
				else if (!this.pathIsClockwise && this.pathIndex <= 0)
				{
					this.pathIndex = positions.Length - 1;
				}
				else
				{
					this.pathIndex += incrementBy;
				}
			}
			endPos = positions[this.pathIndex];
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x000D62A8 File Offset: 0x000D46A8
	private void SetPinkValues()
	{
		LevelProperties.ChessBOldA.Pink pink = base.properties.CurrentState.pink;
		int num = Mathf.Clamp(this.phase, 0, pink.pinkSpeedString.Split(new char[]
		{
			','
		}).Length - 1);
		int num2 = Mathf.Clamp(this.phase, 0, pink.pinkDirString.Split(new char[]
		{
			','
		}).Length - 1);
		Parser.FloatTryParse(pink.pinkSpeedString.Split(new char[]
		{
			','
		})[num], out this.pinkSpeed);
		this.pinkIsClockwise = (pink.pinkDirString.Split(new char[]
		{
			','
		})[num2][0] == 'R');
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x000D6360 File Offset: 0x000D4760
	private IEnumerator pink_cr()
	{
		LevelProperties.ChessBOldA.Pink p = base.properties.CurrentState.pink;
		float angle = 0f;
		this.pink.transform.SetScale(new float?(p.pinkScale), new float?(p.pinkScale), null);
		Vector3 handleRotationX = Vector3.zero;
		Vector3 handleRotationY = Vector3.zero;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (this.pinkIsClockwise)
			{
				angle += this.pinkSpeed * CupheadTime.FixedDelta;
			}
			else
			{
				angle -= this.pinkSpeed * CupheadTime.FixedDelta;
			}
			handleRotationX = new Vector3(Mathf.Sin(angle) * p.pinkPathRadius, 0f, 0f);
			handleRotationY = new Vector3(0f, Mathf.Cos(angle) * p.pinkPathRadius, 0f);
			this.pink.transform.position = base.transform.position;
			this.pink.transform.position += handleRotationX + handleRotationY;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x000D637C File Offset: 0x000D477C
	private void SetWallValues()
	{
		LevelProperties.ChessBOldA.Walls walls = base.properties.CurrentState.walls;
		this.nullIndex = Mathf.Clamp(this.phase, 0, walls.wallNullString.Length - 1);
		int num = Mathf.Clamp(this.phase, 0, walls.wallNumberString.Split(new char[]
		{
			','
		}).Length - 1);
		int num2 = Mathf.Clamp(this.phase, 0, walls.wallSpeedString.Split(new char[]
		{
			','
		}).Length - 1);
		int num3 = Mathf.Clamp(this.phase, 0, walls.wallDirString.Split(new char[]
		{
			','
		}).Length - 1);
		int num4 = 0;
		float speed = 0f;
		int[] array = new int[walls.wallNullString[this.nullIndex].Split(new char[]
		{
			','
		}).Length];
		Parser.IntTryParse(walls.wallNumberString.Split(new char[]
		{
			','
		})[num], out num4);
		Parser.FloatTryParse(walls.wallSpeedString.Split(new char[]
		{
			','
		})[num2], out speed);
		bool isClockwise = walls.wallDirString.Split(new char[]
		{
			','
		})[num3][0] == 'R';
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			flag = Parser.IntTryParse(walls.wallNullString[this.nullIndex].Split(new char[]
			{
				','
			})[i], out array[i]);
		}
		float num5 = 360f / (float)num4;
		for (int j = 0; j < num4; j++)
		{
			bool flag2 = false;
			for (int k = 0; k < array.Length; k++)
			{
				if (!flag)
				{
					break;
				}
				if (j == array[k])
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				ChessBOldALevelWall chessBOldALevelWall = this.wallPrefab.Spawn<ChessBOldALevelWall>();
				chessBOldALevelWall.StartRotate(num5 * (float)j, this, walls.wallPathRadius, speed, isClockwise, walls.wallLength);
				chessBOldALevelWall.transform.parent = base.transform;
				this.walls.Add(chessBOldALevelWall);
			}
		}
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x000D65C8 File Offset: 0x000D49C8
	private void RemoveCurrentWalls()
	{
		foreach (ChessBOldALevelWall chessBOldALevelWall in this.walls)
		{
			chessBOldALevelWall.Dead();
		}
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x000D6624 File Offset: 0x000D4A24
	private IEnumerator turret_cr()
	{
		LevelProperties.ChessBOldA.BishopPath p = base.properties.CurrentState.bishopPath;
		AbstractPlayerController player = PlayerManager.GetNext();
		string[] turretString = p.turretShotDelayString.Split(new char[]
		{
			','
		});
		int turretIndex = UnityEngine.Random.Range(0, turretString.Length);
		bool gotStunned = false;
		float delay = 0f;
		for (;;)
		{
			if (gotStunned)
			{
				Parser.FloatTryParse(turretString[turretIndex], out delay);
				yield return CupheadTime.WaitForSeconds(this, delay);
				gotStunned = false;
			}
			Vector3 dir = player.transform.position - base.transform.position;
			this.turretShot.Create(base.transform.position, MathUtils.DirectionToAngle(dir), p.turretShotSpeed);
			player = PlayerManager.GetNext();
			Parser.FloatTryParse(turretString[turretIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			while (this.isStunned)
			{
				gotStunned = true;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x040020EF RID: 8431
	[SerializeField]
	private BasicProjectile turretShot;

	// Token: 0x040020F0 RID: 8432
	[SerializeField]
	private ParrySwitch pink;

	// Token: 0x040020F1 RID: 8433
	[SerializeField]
	private Transform pivotPoint;

	// Token: 0x040020F2 RID: 8434
	[SerializeField]
	private ChessBOldALevelWall wallPrefab;

	// Token: 0x040020F3 RID: 8435
	private List<ChessBOldALevelWall> walls;

	// Token: 0x040020F4 RID: 8436
	private DamageDealer damageDealer;

	// Token: 0x040020F5 RID: 8437
	private ChessBOldALevelBishop.PathType pathType;

	// Token: 0x040020F6 RID: 8438
	private ChessBOldALevelBishop.PathType previousPathType;

	// Token: 0x040020F7 RID: 8439
	private bool pathIsClockwise;

	// Token: 0x040020F8 RID: 8440
	private float pathSpeed;

	// Token: 0x040020F9 RID: 8441
	private int pathIndex;

	// Token: 0x040020FA RID: 8442
	private Vector3[] positions;

	// Token: 0x040020FB RID: 8443
	private bool pinkIsClockwise;

	// Token: 0x040020FC RID: 8444
	private float pinkSpeed;

	// Token: 0x040020FD RID: 8445
	private bool isStunned;

	// Token: 0x040020FE RID: 8446
	private float HPToDecrease;

	// Token: 0x040020FF RID: 8447
	private float lerpPos;

	// Token: 0x04002100 RID: 8448
	private int phase;

	// Token: 0x04002101 RID: 8449
	private int nullIndex;

	// Token: 0x04002102 RID: 8450
	private float straightValue;

	// Token: 0x04002103 RID: 8451
	private float infinityAngle;

	// Token: 0x02000529 RID: 1321
	private enum PathType
	{
		// Token: 0x04002105 RID: 8453
		Straight,
		// Token: 0x04002106 RID: 8454
		Infinite,
		// Token: 0x04002107 RID: 8455
		Square,
		// Token: 0x04002108 RID: 8456
		Pending
	}
}
