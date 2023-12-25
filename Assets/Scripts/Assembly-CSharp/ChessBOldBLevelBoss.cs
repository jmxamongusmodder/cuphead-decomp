using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052D RID: 1325
public class ChessBOldBLevelBoss : LevelProperties.ChessBOldB.Entity
{
	// Token: 0x060017F5 RID: 6133 RVA: 0x000D852A File Offset: 0x000D692A
	public override void LevelInit(LevelProperties.ChessBOldB properties)
	{
		base.LevelInit(properties);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x000D8540 File Offset: 0x000D6940
	private IEnumerator intro_cr()
	{
		this.brown = this.bossTwo.GetComponent<SpriteRenderer>().color;
		yield return CupheadTime.WaitForSeconds(this, 4f);
		this.MoveBosses();
		base.StartCoroutine(this.wait_to_shoot());
		yield return null;
		yield break;
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x000D855C File Offset: 0x000D695C
	public void HandleHurt(bool gettingHurt)
	{
		this.bossOne.GetComponent<SpriteRenderer>().color = ((!gettingHurt) ? this.brown : Color.red);
		this.bossTwo.GetComponent<SpriteRenderer>().color = ((!gettingHurt) ? this.brown : Color.red);
		this.isMoving = !gettingHurt;
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x000D85C8 File Offset: 0x000D69C8
	public void OnStateChanged()
	{
		LevelProperties.ChessBOldB.Boss boss = base.properties.CurrentState.boss;
		this.moveTime = boss.bossTime;
		this.bulletDelayStringMainIndex = UnityEngine.Random.Range(0, boss.bulletDelayString.Length);
		this.bulletDelayString = boss.bulletDelayString[this.bulletDelayStringMainIndex].Split(new char[]
		{
			','
		});
		this.bulletDelayStringIndex = UnityEngine.Random.Range(0, this.bulletDelayString.Length);
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x000D863D File Offset: 0x000D6A3D
	private void MoveBosses()
	{
		base.StartCoroutine(this.move_bosses_cr());
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x000D864C File Offset: 0x000D6A4C
	private IEnumerator move_bosses_cr()
	{
		LevelProperties.ChessBOldB.Boss p = base.properties.CurrentState.boss;
		float t = 0f;
		float one = 1f;
		this.moveTime = p.bossTime;
		bool countingUp = true;
		YieldInstruction wait = new WaitForFixedUpdate();
		this.isMoving = true;
		for (;;)
		{
			while (!this.isMoving)
			{
				yield return null;
			}
			t += CupheadTime.FixedDelta;
			this.bossOne.transform.SetPosition(null, new float?(Mathf.Lerp(225f, -225f, (!countingUp) ? (one - t / this.moveTime) : (t / this.moveTime))), null);
			this.bossTwo.transform.SetPosition(null, new float?(Mathf.Lerp(225f, -225f, (!countingUp) ? (t / this.moveTime) : (one - t / this.moveTime))), null);
			if (t >= this.moveTime)
			{
				countingUp = !countingUp;
				t = 0f;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x000D8668 File Offset: 0x000D6A68
	private IEnumerator wait_to_shoot()
	{
		bool leftOneShoot = Rand.Bool();
		LevelProperties.ChessBOldB.Boss p = base.properties.CurrentState.boss;
		this.OnStateChanged();
		float delay = 0f;
		for (;;)
		{
			while (!this.isMoving)
			{
				yield return null;
			}
			p = base.properties.CurrentState.boss;
			this.bulletDelayString = p.bulletDelayString[this.bulletDelayStringMainIndex].Split(new char[]
			{
				','
			});
			Parser.FloatTryParse(this.bulletDelayString[this.bulletDelayStringIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			GameObject boss = (!leftOneShoot) ? this.bossTwo : this.bossOne;
			this.Shoot(boss);
			leftOneShoot = !leftOneShoot;
			if (this.bulletDelayStringIndex < this.bulletDelayString.Length - 1)
			{
				this.bulletDelayStringIndex++;
			}
			else
			{
				this.bulletDelayStringMainIndex = (this.bulletDelayStringMainIndex + 1) % p.bulletDelayString.Length;
				this.bulletDelayStringIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x000D8684 File Offset: 0x000D6A84
	private void Shoot(GameObject boss)
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 v = next.center - boss.transform.position;
		float rotation = MathUtils.DirectionToAngle(v);
		this.projectile.Create(boss.transform.position, rotation, base.properties.CurrentState.boss.bulletSpeed);
	}

	// Token: 0x0400211C RID: 8476
	private const float Y_POS = 225f;

	// Token: 0x0400211D RID: 8477
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x0400211E RID: 8478
	[SerializeField]
	private GameObject bossOne;

	// Token: 0x0400211F RID: 8479
	[SerializeField]
	private GameObject bossTwo;

	// Token: 0x04002120 RID: 8480
	private Color brown;

	// Token: 0x04002121 RID: 8481
	private bool isMoving;

	// Token: 0x04002122 RID: 8482
	private float moveTime;

	// Token: 0x04002123 RID: 8483
	private int bulletDelayStringMainIndex;

	// Token: 0x04002124 RID: 8484
	private string[] bulletDelayString;

	// Token: 0x04002125 RID: 8485
	private int bulletDelayStringIndex;
}
