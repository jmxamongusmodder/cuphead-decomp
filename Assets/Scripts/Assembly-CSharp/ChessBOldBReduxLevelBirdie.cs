using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052B RID: 1323
public class ChessBOldBReduxLevelBirdie : AbstractProjectile
{
	// Token: 0x1700032F RID: 815
	// (get) Token: 0x060017E2 RID: 6114 RVA: 0x000D7CBB File Offset: 0x000D60BB
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x000D7CC4 File Offset: 0x000D60C4
	public void Setup(Transform pivotPoint, float angle, LevelProperties.ChessBOldB.Birdie properties, float loopSize, bool chosenBall)
	{
		this.angle = angle;
		this.pivotPoint = pivotPoint;
		this.properties = properties;
		this.loopSize = loopSize;
		this.isMoving = false;
		this.chosenBall = chosenBall;
		this.RepositionBall();
		this.sprite.color = this.defaultColor;
		this.SetParryable(false);
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x000D7D1B File Offset: 0x000D611B
	protected override void Awake()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
		base.Awake();
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x000D7D2F File Offset: 0x000D612F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x000D7D4D File Offset: 0x000D614D
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.isMoving)
		{
			this.MoveBirdie();
		}
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x000D7D66 File Offset: 0x000D6166
	public void StopMoving()
	{
		this.isMoving = false;
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x000D7D6F File Offset: 0x000D616F
	public void HandleMovement(float rotationTime, bool goingClockwise)
	{
		this.rotationTime = ((!goingClockwise) ? (-rotationTime) : rotationTime);
		this.isMoving = true;
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x000D7D8C File Offset: 0x000D618C
	public void RepositionBall()
	{
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		this.angle *= 0.017453292f;
		zero = new Vector3(Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		zero2 = new Vector3(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		base.transform.position = this.pivotPoint.position;
		base.transform.position += zero + zero2;
		this.offScreen = false;
		this.damageDealer.SetDamageFlags(false, false, false);
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x000D7E50 File Offset: 0x000D6250
	private void MoveBirdie()
	{
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		this.angle += this.rotationTime * CupheadTime.FixedDelta;
		zero = new Vector3(Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		zero2 = new Vector3(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		base.transform.position = this.pivotPoint.position;
		base.transform.position += zero + zero2;
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x000D7F08 File Offset: 0x000D6308
	public override void OnParry(AbstractPlayerController player)
	{
		if (this.ParryBirdie != null)
		{
			this.isMoving = false;
			this.ParryBirdie(this.chosenBall);
			base.StartCoroutine(this.turn_off_collider_cr());
			if (!this.chosenBall)
			{
				base.StartCoroutine(this.attack_cr());
			}
		}
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000D7F5D File Offset: 0x000D635D
	public void TurnPink()
	{
		this.SetParryable(true);
		this.sprite.color = this.pinkColor;
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x000D7F78 File Offset: 0x000D6378
	private IEnumerator turn_off_collider_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.properties.colliderOffTime);
		base.GetComponent<Collider2D>().enabled = true;
		this.offScreen = false;
		this.damageDealer.SetDamageFlags(true, false, false);
		yield break;
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x000D7F94 File Offset: 0x000D6394
	private IEnumerator attack_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		AbstractPlayerController player = PlayerManager.GetNext();
		Vector3 dir = player.transform.position - base.transform.position;
		float angle = MathUtils.DirectionToAngle(dir);
		bool changedDirection = false;
		float straightTime = 0f;
		float timeToStraight = this.properties.timeToStraight;
		float arcTime = 0f;
		float timeToArc = this.properties.timeToMaxSpeed;
		MinMax xSpeedMinMax = this.properties.xSpeed;
		MinMax ySpeedMinMax = this.properties.ySpeed;
		float xSpeed = 0f;
		float ySpeed = 0f;
		base.StartCoroutine(this.check_bounds_cr());
		while (!this.offScreen)
		{
			if (arcTime < timeToArc)
			{
				arcTime += CupheadTime.FixedDelta;
				xSpeed = xSpeedMinMax.GetFloatAt(arcTime / timeToArc);
				ySpeed = ySpeedMinMax.GetFloatAt(1f - arcTime / timeToArc);
			}
			if (xSpeed > 0f && !changedDirection)
			{
				if (straightTime < timeToStraight)
				{
					straightTime += CupheadTime.FixedDelta;
					dir = player.transform.position - base.transform.position;
					angle = MathUtils.DirectionToAngle(dir);
				}
				else
				{
					changedDirection = true;
				}
			}
			Vector3 speed = new Vector3(xSpeed, ySpeed);
			Quaternion rot = Quaternion.Euler(0f, 0f, angle);
			speed = rot * speed;
			base.transform.position += speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x000D7FB0 File Offset: 0x000D63B0
	private IEnumerator check_bounds_cr()
	{
		float offset = 200f;
		while (base.transform.position.x < (float)Level.Current.Right + offset && base.transform.position.x > (float)Level.Current.Left - offset && base.transform.position.y < (float)Level.Current.Ceiling + offset && base.transform.position.y > (float)Level.Current.Ground - offset)
		{
			yield return null;
		}
		this.offScreen = true;
		yield return null;
		yield break;
	}

	// Token: 0x0400210E RID: 8462
	private const float ONE = 1f;

	// Token: 0x0400210F RID: 8463
	public ChessBOldBReduxLevelBirdie.OnParryBirdie ParryBirdie;

	// Token: 0x04002110 RID: 8464
	[SerializeField]
	private Color defaultColor;

	// Token: 0x04002111 RID: 8465
	[SerializeField]
	private Color pinkColor;

	// Token: 0x04002112 RID: 8466
	private LevelProperties.ChessBOldB.Birdie properties;

	// Token: 0x04002113 RID: 8467
	private SpriteRenderer sprite;

	// Token: 0x04002114 RID: 8468
	private Transform pivotPoint;

	// Token: 0x04002115 RID: 8469
	private int timesToChangeDir;

	// Token: 0x04002116 RID: 8470
	private float angle;

	// Token: 0x04002117 RID: 8471
	private float loopSize;

	// Token: 0x04002118 RID: 8472
	private float rotationTime;

	// Token: 0x04002119 RID: 8473
	private bool isMoving;

	// Token: 0x0400211A RID: 8474
	private bool chosenBall;

	// Token: 0x0400211B RID: 8475
	private bool offScreen;

	// Token: 0x0200052C RID: 1324
	// (Invoke) Token: 0x060017F1 RID: 6129
	public delegate void OnParryBirdie(bool correctBall);
}
