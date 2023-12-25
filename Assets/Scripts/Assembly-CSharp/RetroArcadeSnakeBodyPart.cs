using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000758 RID: 1880
public class RetroArcadeSnakeBodyPart : AbstractCollidableObject
{
	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x060028F6 RID: 10486 RVA: 0x0017DE73 File Offset: 0x0017C273
	// (set) Token: 0x060028F7 RID: 10487 RVA: 0x0017DE7B File Offset: 0x0017C27B
	public RetroArcadeSnakeBodyPart.Direction currentDirection { get; private set; }

	// Token: 0x060028F8 RID: 10488 RVA: 0x0017DE84 File Offset: 0x0017C284
	public RetroArcadeSnakeBodyPart Create(Vector2 pos, bool isHead, RetroArcadeSnakeBodyPart.Direction direction, RetroArcadeSnakeManager manager, RetroArcadeSnakeBodyPart previousPart, float speed)
	{
		RetroArcadeSnakeBodyPart retroArcadeSnakeBodyPart = this.InstantiatePrefab<RetroArcadeSnakeBodyPart>();
		retroArcadeSnakeBodyPart.transform.position = pos;
		retroArcadeSnakeBodyPart.currentDirection = direction;
		retroArcadeSnakeBodyPart.partInFront = previousPart;
		retroArcadeSnakeBodyPart.speed = speed;
		retroArcadeSnakeBodyPart.isHead = isHead;
		retroArcadeSnakeBodyPart.manager = manager;
		return retroArcadeSnakeBodyPart;
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x0017DED0 File Offset: 0x0017C2D0
	private void Start()
	{
		this.canTurn = true;
		this.ChangeDirection(this.currentDirection, false);
		if (this.isHead)
		{
			base.StartCoroutine(this.head_check_cr());
		}
		else
		{
			base.StartCoroutine(this.body_check_cr());
		}
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x0017DF10 File Offset: 0x0017C310
	private void FixedUpdate()
	{
		if (!this.isDead)
		{
			base.transform.position += this.dir * this.speed * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x0017DF50 File Offset: 0x0017C350
	private IEnumerator body_check_cr()
	{
		for (;;)
		{
			if (this.currentDirection != this.partInFront.currentDirection)
			{
				if (this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Right)
				{
					if (base.transform.position.x >= this.partInFront.turnPos.x)
					{
						this.ClampDirectionChange();
					}
				}
				else if (this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Left)
				{
					if (base.transform.position.x <= this.partInFront.turnPos.x)
					{
						this.ClampDirectionChange();
					}
				}
				else if (this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Up)
				{
					if (base.transform.position.y >= this.partInFront.turnPos.y)
					{
						this.ClampDirectionChange();
					}
				}
				else if (this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Down && base.transform.position.y <= this.partInFront.turnPos.y)
				{
					this.ClampDirectionChange();
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x0017DF6C File Offset: 0x0017C36C
	private void ClampDirectionChange()
	{
		base.transform.position = this.partInFront.transform.position + -this.partInFront.dir * 60f;
		this.ChangeDirection(this.partInFront.currentDirection, false);
	}

	// Token: 0x060028FD RID: 10493 RVA: 0x0017DFC8 File Offset: 0x0017C3C8
	private IEnumerator head_check_cr()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		for (;;)
		{
			if (this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Up || this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Down)
			{
				if (base.transform.position.y >= 230f || base.transform.position.y <= -120f)
				{
					this.SwitchToHorizontal();
				}
				else
				{
					if (player != null && !player.IsDead)
					{
						this.CheckPlayerY(player);
					}
					if (player2 != null && !player2.IsDead)
					{
						this.CheckPlayerY(player2);
					}
				}
			}
			else if (base.transform.position.x >= 330f || base.transform.position.x <= -330f)
			{
				this.SwitchToVertical();
			}
			else
			{
				if (player != null && !player.IsDead)
				{
					this.CheckPlayerX(player);
				}
				if (player2 != null && !player2.IsDead)
				{
					this.CheckPlayerX(player2);
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060028FE RID: 10494 RVA: 0x0017DFE4 File Offset: 0x0017C3E4
	private void CheckPlayerX(AbstractPlayerController player)
	{
		float f = player.transform.position.x - base.transform.position.x;
		if (Mathf.Abs(f) < 20f)
		{
			if (player.transform.position.y < base.transform.position.y)
			{
				if (base.transform.position.y <= -100f)
				{
					return;
				}
				this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Down, true);
			}
			else
			{
				if (base.transform.position.y >= 210f)
				{
					return;
				}
				this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Up, true);
			}
		}
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x0017E0A8 File Offset: 0x0017C4A8
	private void CheckPlayerY(AbstractPlayerController player)
	{
		float f = player.transform.position.y - base.transform.position.y;
		if (Mathf.Abs(f) < 20f)
		{
			if (player.transform.position.x < base.transform.position.x)
			{
				if (player.transform.position.x <= -310f)
				{
					return;
				}
				this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Left, true);
			}
			else
			{
				if (player.transform.position.x >= 310f)
				{
					return;
				}
				this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Right, true);
			}
		}
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x0017E16C File Offset: 0x0017C56C
	private void SwitchToHorizontal()
	{
		if (base.transform.position.x < 0f)
		{
			this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Right, true);
		}
		else
		{
			this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Left, true);
		}
	}

	// Token: 0x06002901 RID: 10497 RVA: 0x0017E1AC File Offset: 0x0017C5AC
	private void SwitchToVertical()
	{
		if (base.transform.position.y < 0f)
		{
			this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Up, true);
		}
		else
		{
			this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Down, true);
		}
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x0017E1EC File Offset: 0x0017C5EC
	private void ChangeDirection(RetroArcadeSnakeBodyPart.Direction direction, bool checkTurn)
	{
		if (checkTurn && !this.canTurn)
		{
			return;
		}
		this.currentDirection = direction;
		this.turnPos = base.transform.position;
		switch (this.currentDirection)
		{
		case RetroArcadeSnakeBodyPart.Direction.Left:
			this.dir = Vector3.left;
			break;
		case RetroArcadeSnakeBodyPart.Direction.Right:
			this.dir = Vector3.right;
			break;
		case RetroArcadeSnakeBodyPart.Direction.Up:
			this.dir = Vector3.up;
			break;
		case RetroArcadeSnakeBodyPart.Direction.Down:
			this.dir = Vector3.down;
			break;
		}
		base.StartCoroutine(this.turn_timer_cr());
	}

	// Token: 0x06002903 RID: 10499 RVA: 0x0017E294 File Offset: 0x0017C694
	private IEnumerator turn_timer_cr()
	{
		float t = 0f;
		float time = 0.5f;
		this.canTurn = false;
		while (t < time)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.canTurn = true;
		yield return null;
		yield break;
	}

	// Token: 0x06002904 RID: 10500 RVA: 0x0017E2B0 File Offset: 0x0017C6B0
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (this.isHead && !this.isDead && hit.GetComponent<RetroArcadeSnakeBodyPart>() != this.partBehind)
		{
			this.manager.EndPhase();
			this.isDead = true;
		}
	}

	// Token: 0x06002905 RID: 10501 RVA: 0x0017E303 File Offset: 0x0017C703
	public void GetPartBehind(RetroArcadeSnakeBodyPart behind)
	{
		this.partBehind = behind;
	}

	// Token: 0x06002906 RID: 10502 RVA: 0x0017E30C File Offset: 0x0017C70C
	public void Die()
	{
		this.StopAllCoroutines();
		this.isDead = true;
	}

	// Token: 0x040031DD RID: 12765
	public Vector3 turnPos;

	// Token: 0x040031DE RID: 12766
	public Vector3 dir;

	// Token: 0x040031DF RID: 12767
	private const float TOP_Y = 230f;

	// Token: 0x040031E0 RID: 12768
	private const float BOTTOM_Y = -120f;

	// Token: 0x040031E1 RID: 12769
	private const float OFFSCREEN_Y = 300f;

	// Token: 0x040031E2 RID: 12770
	private const float SIDE_X = 330f;

	// Token: 0x040031E3 RID: 12771
	private const float MIN_DISTANCE = 20f;

	// Token: 0x040031E4 RID: 12772
	private const float SPACING = 60f;

	// Token: 0x040031E6 RID: 12774
	private RetroArcadeSnakeBodyPart partInFront;

	// Token: 0x040031E7 RID: 12775
	private RetroArcadeSnakeBodyPart partBehind;

	// Token: 0x040031E8 RID: 12776
	private RetroArcadeSnakeManager manager;

	// Token: 0x040031E9 RID: 12777
	private float speed;

	// Token: 0x040031EA RID: 12778
	private bool canTurn;

	// Token: 0x040031EB RID: 12779
	private bool isHead;

	// Token: 0x040031EC RID: 12780
	private bool isDead;

	// Token: 0x02000759 RID: 1881
	public enum Direction
	{
		// Token: 0x040031EE RID: 12782
		Left,
		// Token: 0x040031EF RID: 12783
		Right,
		// Token: 0x040031F0 RID: 12784
		Up,
		// Token: 0x040031F1 RID: 12785
		Down
	}
}
