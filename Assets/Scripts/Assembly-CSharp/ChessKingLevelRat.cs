using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200053E RID: 1342
public class ChessKingLevelRat : AbstractCollidableObject
{
	// Token: 0x0600187E RID: 6270 RVA: 0x000DDBA0 File Offset: 0x000DBFA0
	public void Init(Vector3 position, float speed)
	{
		base.transform.position = position;
		this.startPosX = base.transform.position.x;
		this.speed = speed;
		this.Move();
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x000DDBDF File Offset: 0x000DBFDF
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x000DDBF2 File Offset: 0x000DBFF2
	protected void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x000DDC0A File Offset: 0x000DC00A
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x000DDC28 File Offset: 0x000DC028
	private void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x000DDC38 File Offset: 0x000DC038
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float leftBound = (float)Level.Current.Left + 75f;
		float rightBound = this.startPosX;
		bool goingRight = false;
		for (;;)
		{
			if (goingRight)
			{
				while (base.transform.position.x < rightBound)
				{
					base.transform.position += Vector3.right * this.speed * CupheadTime.FixedDelta;
					yield return wait;
				}
				goingRight = false;
				base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
			}
			else
			{
				while (base.transform.position.x > leftBound)
				{
					base.transform.position += Vector3.left * this.speed * CupheadTime.FixedDelta;
					yield return wait;
				}
				goingRight = true;
				base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040021A3 RID: 8611
	private float speed;

	// Token: 0x040021A4 RID: 8612
	private float startPosX;

	// Token: 0x040021A5 RID: 8613
	private DamageDealer damageDealer;
}
