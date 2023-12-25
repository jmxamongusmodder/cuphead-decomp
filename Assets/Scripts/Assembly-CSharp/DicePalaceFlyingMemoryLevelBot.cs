using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005C6 RID: 1478
public class DicePalaceFlyingMemoryLevelBot : AbstractProjectile
{
	// Token: 0x06001CDC RID: 7388 RVA: 0x0010869A File Offset: 0x00106A9A
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x001086C8 File Offset: 0x00106AC8
	public void Init(LevelProperties.DicePalaceFlyingMemory.Bots properties, DicePalaceFlyingMemoryLevelContactPoint startingPoint, bool moveOnY, float health, AbstractPlayerController player)
	{
		base.transform.position = startingPoint.transform.position;
		this.currentPoint = startingPoint;
		this.health = health;
		this.player = player;
		this.moveOnY = moveOnY;
		this.properties = properties;
		base.transform.SetScale(new float?(properties.botsScale), new float?(properties.botsScale), null);
		this.gameManager = DicePalaceFlyingMemoryLevelGameManager.Instance;
		this.targetPoint = this.gameManager.contactPoints[1, 1];
		this.movementString = properties.movementString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.directionString = properties.directionString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.movementIndex = UnityEngine.Random.Range(0, this.movementString.Length);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x001087C0 File Offset: 0x00106BC0
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x001087DE File Offset: 0x00106BDE
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x001087FC File Offset: 0x00106BFC
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x00108828 File Offset: 0x00106C28
	private void CalculateYTarget(bool followPlayer)
	{
		this.reachedEnd = false;
		if (followPlayer)
		{
			if (this.player.transform.position.y <= base.transform.position.y)
			{
				this.MoveUp();
			}
			else
			{
				this.MoveDown();
			}
		}
		else if (this.currentPoint.Ycoord <= (this.gameManager.contactDimY - 1) / 2)
		{
			this.MoveUp();
		}
		else
		{
			this.MoveDown();
		}
		if (this.reachedEnd)
		{
			this.targetPos.y = this.gameManager.contactPoints[this.currentPoint.Xcoord, this.setPosition].transform.position.y + this.offsetEnd;
		}
		else
		{
			this.targetPoint = this.gameManager.contactPoints[this.currentPoint.Xcoord, this.setPosition];
			this.targetPos = this.targetPoint.transform.position;
		}
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x0010894C File Offset: 0x00106D4C
	private void CalculateXTarget(bool followPlayer)
	{
		this.reachedEnd = false;
		if (followPlayer)
		{
			if (this.player.transform.position.x <= base.transform.position.x)
			{
				this.MoveRight();
			}
			else
			{
				this.MoveLeft();
			}
		}
		else if (this.currentPoint.Xcoord <= (this.gameManager.contactDimX - 1) / 2)
		{
			this.MoveRight();
		}
		else
		{
			this.MoveLeft();
		}
		if (this.reachedEnd)
		{
			this.targetPos.x = this.gameManager.contactPoints[this.setPosition, this.currentPoint.Ycoord].transform.position.x + this.offsetEnd;
		}
		else
		{
			this.targetPoint = this.gameManager.contactPoints[this.setPosition, this.currentPoint.Ycoord];
			this.targetPos = this.targetPoint.transform.position;
		}
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x00108A70 File Offset: 0x00106E70
	private IEnumerator move_cr()
	{
		bool followPlayer = false;
		Vector3 pos = base.transform.position;
		for (;;)
		{
			Parser.IntTryParse(this.movementString[this.movementIndex], out this.movement);
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
			this.GetMovement(followPlayer);
			if (this.moveOnY)
			{
				this.CalculateYTarget(followPlayer);
			}
			else
			{
				this.CalculateXTarget(followPlayer);
			}
			if (this.moveOnY)
			{
				while (base.transform.position.y != this.targetPos.y)
				{
					pos.y = Mathf.MoveTowards(base.transform.position.y, this.targetPos.y, this.properties.botsSpeed * CupheadTime.Delta);
					base.transform.position = pos;
					yield return null;
				}
			}
			else
			{
				while (base.transform.position.x != this.targetPos.x)
				{
					pos.x = Mathf.MoveTowards(base.transform.position.x, this.targetPos.x, this.properties.botsSpeed * CupheadTime.Delta);
					base.transform.position = pos;
					yield return null;
				}
			}
			if (this.reachedEnd)
			{
				this.OnDestroy();
			}
			else
			{
				this.currentPoint = this.targetPoint;
				this.moveOnY = !this.moveOnY;
				this.movementIndex = (this.movementIndex + 1) % this.movementString.Length;
				this.directionIndex = (this.directionIndex + 1) % this.directionString.Length;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x00108A8C File Offset: 0x00106E8C
	private bool GetMovement(bool followPlayer)
	{
		if (this.directionString[this.directionIndex][0] == 'N')
		{
			followPlayer = false;
		}
		else if (this.directionString[this.directionIndex][0] == 'P')
		{
			followPlayer = true;
		}
		return followPlayer;
	}

	// Token: 0x06001CE5 RID: 7397 RVA: 0x00108ADC File Offset: 0x00106EDC
	private void MoveUp()
	{
		int num = this.currentPoint.Ycoord + this.movement;
		if (num > this.gameManager.contactDimY - 1)
		{
			this.setPosition = this.gameManager.contactDimY - 1;
			this.reachedEnd = true;
			this.offsetEnd = -200f;
		}
		else
		{
			this.setPosition = num;
			this.reachedEnd = false;
		}
	}

	// Token: 0x06001CE6 RID: 7398 RVA: 0x00108B48 File Offset: 0x00106F48
	private void MoveDown()
	{
		int num = this.currentPoint.Ycoord - this.movement;
		if (num < 0)
		{
			this.setPosition = 0;
			this.reachedEnd = true;
			this.offsetEnd = 200f;
		}
		else
		{
			this.setPosition = num;
			this.reachedEnd = false;
		}
	}

	// Token: 0x06001CE7 RID: 7399 RVA: 0x00108B9C File Offset: 0x00106F9C
	private void MoveLeft()
	{
		int num = this.currentPoint.Xcoord - this.movement;
		if (num < 0)
		{
			this.setPosition = 0;
			this.reachedEnd = true;
			this.offsetEnd = -200f;
		}
		else
		{
			this.setPosition = num;
			this.reachedEnd = false;
		}
	}

	// Token: 0x06001CE8 RID: 7400 RVA: 0x00108BF0 File Offset: 0x00106FF0
	private void MoveRight()
	{
		int num = this.currentPoint.Xcoord + this.movement;
		if (num > this.gameManager.contactDimX - 1)
		{
			this.setPosition = this.gameManager.contactDimX - 1;
			this.reachedEnd = true;
			this.offsetEnd = 200f;
		}
		else
		{
			this.setPosition = num;
			this.reachedEnd = false;
		}
	}

	// Token: 0x06001CE9 RID: 7401 RVA: 0x00108C5C File Offset: 0x0010705C
	private IEnumerator shoot_bullets_cr()
	{
		for (;;)
		{
			base.animator.Play("Bot_Warning");
			yield return CupheadTime.WaitForSeconds(this, this.properties.bulletWarningDuration);
			this.FireProjectile();
			this.player = PlayerManager.GetNext();
			base.animator.Play("Off");
			yield return null;
			yield return CupheadTime.WaitForSeconds(this, this.properties.bulletDelay);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CEA RID: 7402 RVA: 0x00108C78 File Offset: 0x00107078
	private void FireProjectile()
	{
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 v = this.player.transform.position - base.transform.position;
		float rotation = MathUtils.DirectionToAngle(v);
		this.projectile.Create(base.transform.position, rotation, this.properties.bulletSpeed);
	}

	// Token: 0x06001CEB RID: 7403 RVA: 0x00108D06 File Offset: 0x00107106
	protected override void Die()
	{
		this.StopAllCoroutines();
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.OnDestroy();
		base.Die();
	}

	// Token: 0x040025C4 RID: 9668
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x040025C5 RID: 9669
	private DicePalaceFlyingMemoryLevelGameManager gameManager;

	// Token: 0x040025C6 RID: 9670
	private LevelProperties.DicePalaceFlyingMemory.Bots properties;

	// Token: 0x040025C7 RID: 9671
	private DicePalaceFlyingMemoryLevelContactPoint currentPoint;

	// Token: 0x040025C8 RID: 9672
	private DicePalaceFlyingMemoryLevelContactPoint targetPoint;

	// Token: 0x040025C9 RID: 9673
	private AbstractPlayerController player;

	// Token: 0x040025CA RID: 9674
	private DamageReceiver damageReceiver;

	// Token: 0x040025CB RID: 9675
	private bool moveOnY;

	// Token: 0x040025CC RID: 9676
	private bool reachedEnd;

	// Token: 0x040025CD RID: 9677
	private float health;

	// Token: 0x040025CE RID: 9678
	private int movement;

	// Token: 0x040025CF RID: 9679
	private int movementIndex;

	// Token: 0x040025D0 RID: 9680
	private int directionIndex;

	// Token: 0x040025D1 RID: 9681
	private int setPosition;

	// Token: 0x040025D2 RID: 9682
	private float offsetEnd;

	// Token: 0x040025D3 RID: 9683
	private Vector3 targetPos;

	// Token: 0x040025D4 RID: 9684
	private string[] movementString;

	// Token: 0x040025D5 RID: 9685
	private string[] directionString;
}
