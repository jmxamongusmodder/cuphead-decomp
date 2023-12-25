using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200073B RID: 1851
public class RetroArcadeBouncyBallHolder : RetroArcadeEnemy
{
	// Token: 0x06002855 RID: 10325 RVA: 0x001781C4 File Offset: 0x001765C4
	public RetroArcadeBouncyBallHolder Create(RetroArcadeBouncyManager manager, LevelProperties.RetroArcade.Bouncy properties, Vector3 pos, string[] ballTypes)
	{
		RetroArcadeBouncyBallHolder retroArcadeBouncyBallHolder = this.InstantiatePrefab<RetroArcadeBouncyBallHolder>();
		retroArcadeBouncyBallHolder.manager = manager;
		retroArcadeBouncyBallHolder.properties = properties;
		retroArcadeBouncyBallHolder.transform.position = pos;
		retroArcadeBouncyBallHolder.ballTypes = ballTypes;
		return retroArcadeBouncyBallHolder;
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x001781FB File Offset: 0x001765FB
	protected override void Start()
	{
		this.hp = 1f;
		this.SetBalls();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x0017821C File Offset: 0x0017661C
	private void SetBalls()
	{
		this.ballsHeld = new RetroArcadeBouncyBall[this.ballPositions.Length];
		RetroArcadeBouncyBall retroArcadeBouncyBall = this.typeABall;
		float num = 120f;
		int i = 0;
		while (i < this.ballPositions.Length)
		{
			string text = this.ballTypes[i];
			if (text == null)
			{
				goto IL_8F;
			}
			if (!(text == "A"))
			{
				if (!(text == "B"))
				{
					if (!(text == "C"))
					{
						goto IL_8F;
					}
					retroArcadeBouncyBall = this.typeCBall;
				}
				else
				{
					retroArcadeBouncyBall = this.typeBBall;
				}
			}
			else
			{
				retroArcadeBouncyBall = this.typeABall;
			}
			IL_9F:
			RetroArcadeBouncyBall retroArcadeBouncyBall2 = retroArcadeBouncyBall.Create(this.ballPositions[i].position, this.manager, this.properties, num * (float)i);
			this.ballsHeld[i] = retroArcadeBouncyBall2;
			this.ballsHeld[i].transform.parent = base.transform;
			i++;
			continue;
			IL_8F:
			global::Debug.LogError("Something bad happened", null);
			goto IL_9F;
		}
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x00178324 File Offset: 0x00176724
	private void SeparateBalls()
	{
		base.GetComponent<Collider2D>().enabled = false;
		foreach (RetroArcadeBouncyBall retroArcadeBouncyBall in this.ballsHeld)
		{
			retroArcadeBouncyBall.StartMoving(base.transform.position);
		}
		base.StartCoroutine(this.check_to_die_cr());
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x0017837C File Offset: 0x0017677C
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		foreach (Transform transform in this.ballPositions)
		{
			Gizmos.DrawWireSphere(transform.position, 20f);
		}
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x001783C8 File Offset: 0x001767C8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600285B RID: 10331 RVA: 0x001783D0 File Offset: 0x001767D0
	private IEnumerator move_cr()
	{
		this.velocity = MathUtils.AngleToDirection(this.properties.angleRange.RandomFloat());
		for (;;)
		{
			base.transform.position += this.velocity * this.properties.groupMoveSpeed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x0600285C RID: 10332 RVA: 0x001783EC File Offset: 0x001767EC
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Min(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x0017842C File Offset: 0x0017682C
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Max(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x0600285E RID: 10334 RVA: 0x0017846C File Offset: 0x0017686C
	protected void ChangeDir(Vector3 newVelocity)
	{
		this.velocity = newVelocity;
		this.currentAngle = Mathf.Atan2(this.velocity.y, this.velocity.x) * 57.29578f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.currentAngle));
		foreach (RetroArcadeBouncyBall retroArcadeBouncyBall in this.ballsHeld)
		{
			retroArcadeBouncyBall.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		}
	}

	// Token: 0x0600285F RID: 10335 RVA: 0x0017851C File Offset: 0x0017691C
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		Vector3 newVelocity = this.velocity;
		if (base.transform.position.x > 0f)
		{
			newVelocity.x = Mathf.Min(newVelocity.x, -newVelocity.x);
			this.ChangeDir(newVelocity);
		}
		else
		{
			newVelocity.x = Mathf.Max(newVelocity.x, -newVelocity.x);
			this.ChangeDir(newVelocity);
		}
	}

	// Token: 0x06002860 RID: 10336 RVA: 0x0017859E File Offset: 0x0017699E
	public override void Dead()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		this.SeparateBalls();
	}

	// Token: 0x06002861 RID: 10337 RVA: 0x001785B8 File Offset: 0x001769B8
	private IEnumerator check_to_die_cr()
	{
		bool allDead = true;
		for (;;)
		{
			allDead = true;
			for (int i = 0; i < this.ballsHeld.Length; i++)
			{
				if (!this.ballsHeld[i].IsDead)
				{
					allDead = false;
				}
			}
			if (allDead)
			{
				break;
			}
			yield return null;
		}
		base.IsDead = true;
		yield break;
	}

	// Token: 0x06002862 RID: 10338 RVA: 0x001785D4 File Offset: 0x001769D4
	public void DestroyBallsHeld()
	{
		foreach (RetroArcadeBouncyBall retroArcadeBouncyBall in this.ballsHeld)
		{
			UnityEngine.Object.Destroy(retroArcadeBouncyBall.gameObject);
		}
	}

	// Token: 0x04003119 RID: 12569
	[SerializeField]
	private Transform[] ballPositions;

	// Token: 0x0400311A RID: 12570
	[SerializeField]
	private RetroArcadeBouncyBall typeABall;

	// Token: 0x0400311B RID: 12571
	[SerializeField]
	private RetroArcadeBouncyBall typeBBall;

	// Token: 0x0400311C RID: 12572
	[SerializeField]
	private RetroArcadeBouncyBall typeCBall;

	// Token: 0x0400311D RID: 12573
	private RetroArcadeBouncyBall[] ballsHeld;

	// Token: 0x0400311E RID: 12574
	private float currentAngle;

	// Token: 0x0400311F RID: 12575
	private string[] ballTypes;

	// Token: 0x04003120 RID: 12576
	private Vector3 velocity;

	// Token: 0x04003121 RID: 12577
	private LevelProperties.RetroArcade.Bouncy properties;

	// Token: 0x04003122 RID: 12578
	private RetroArcadeBouncyManager manager;
}
