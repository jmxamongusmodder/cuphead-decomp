using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005C0 RID: 1472
public class DicePalaceEightBallLevelPoolBall : AbstractProjectile
{
	// Token: 0x06001CA4 RID: 7332 RVA: 0x00105FA0 File Offset: 0x001043A0
	public DicePalaceEightBallLevelPoolBall Create(Vector2 pos, float horSpeed, float verSpeed, float gravity, float delay, bool onLeft, DicePalaceEightBallLevelEightBall parent)
	{
		DicePalaceEightBallLevelPoolBall dicePalaceEightBallLevelPoolBall = base.Create() as DicePalaceEightBallLevelPoolBall;
		dicePalaceEightBallLevelPoolBall.transform.position = pos;
		dicePalaceEightBallLevelPoolBall.horSpeed = horSpeed;
		dicePalaceEightBallLevelPoolBall.verSpeed = verSpeed;
		dicePalaceEightBallLevelPoolBall.gravity = gravity;
		dicePalaceEightBallLevelPoolBall.delay = delay;
		dicePalaceEightBallLevelPoolBall.onLeft = onLeft;
		dicePalaceEightBallLevelPoolBall.parent = parent;
		return dicePalaceEightBallLevelPoolBall;
	}

	// Token: 0x06001CA5 RID: 7333 RVA: 0x00105FFC File Offset: 0x001043FC
	protected override void Start()
	{
		base.Start();
		this.shadowInstance = UnityEngine.Object.Instantiate<GameObject>(this.shadowPrefab).transform;
		this.shadowInstance.gameObject.SetActive(false);
		this.dustInstance = UnityEngine.Object.Instantiate<GameObject>(this.dustPrefab).transform;
		this.shadowInstance.gameObject.SetActive(false);
		base.StartCoroutine(this.jump_cr());
		base.StartCoroutine(this.check_dying_cr());
		DicePalaceEightBallLevelEightBall dicePalaceEightBallLevelEightBall = this.parent;
		dicePalaceEightBallLevelEightBall.OnEightBallDeath = (Action)Delegate.Combine(dicePalaceEightBallLevelEightBall.OnEightBallDeath, new Action(this.EightBallDead));
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x001060A0 File Offset: 0x001044A0
	public void SetVariation(int index)
	{
		for (int i = 0; i < this.colorVariations.Length; i++)
		{
			this.colorVariations[i].SetActive(false);
		}
		if (index >= 0 && index < this.colorVariations.Length)
		{
			this.colorVariations[index].SetActive(true);
		}
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x001060F7 File Offset: 0x001044F7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001CA8 RID: 7336 RVA: 0x00106115 File Offset: 0x00104515
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CA9 RID: 7337 RVA: 0x00106134 File Offset: 0x00104534
	private IEnumerator jump_cr()
	{
		bool jumping = false;
		bool goingUp = false;
		bool upsideDown = false;
		float velocityY = this.verSpeed;
		float velocityX = this.horSpeed;
		float ground = (float)Level.Current.Ground + 55f;
		this.dustInstance.gameObject.SetActive(false);
		while (base.transform.position.y > ground)
		{
			velocityY -= this.gravity / 2f * CupheadTime.Delta;
			base.transform.AddPosition(0f, velocityY * CupheadTime.Delta, 0f);
			yield return null;
		}
		Vector3 p = base.transform.position;
		p.y = ground;
		base.transform.position = p;
		this.dustInstance.position = base.transform.position;
		this.dustInstance.gameObject.SetActive(true);
		base.animator.SetTrigger("Smash");
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.delay);
			jumping = true;
			goingUp = true;
			velocityY = this.verSpeed;
			velocityX = ((!this.onLeft) ? (-this.horSpeed) : this.horSpeed);
			base.animator.SetTrigger("Jump");
			this.shadowInstance.gameObject.SetActive(false);
			if (upsideDown)
			{
				yield return base.animator.WaitForAnimationToEnd(this, "UpsideDownJump", true, true);
			}
			else
			{
				yield return base.animator.WaitForAnimationToEnd(this, "Jump", true, true);
			}
			this.shadowInstance.gameObject.SetActive(true);
			this.dustInstance.gameObject.SetActive(false);
			while (jumping)
			{
				this.shadowInstance.position = new Vector3(base.transform.position.x, ground, 0f);
				velocityY -= this.gravity * CupheadTime.Delta;
				base.transform.AddPosition(velocityX * CupheadTime.Delta, velocityY * CupheadTime.Delta, 0f);
				if (velocityY < 0f && goingUp)
				{
					base.animator.SetTrigger("Turn");
					goingUp = false;
					if (upsideDown)
					{
						yield return base.animator.WaitForAnimationToEnd(this, "RightSideUpSmash_start", true, true);
					}
					else
					{
						yield return base.animator.WaitForAnimationToEnd(this, "JumpTurn", true, true);
					}
				}
				if (velocityY < 0f && jumping && base.transform.position.y <= ground)
				{
					base.animator.SetTrigger("Smash");
					jumping = false;
					upsideDown = !upsideDown;
					Vector3 position = base.transform.position;
					position.y = ground;
					base.transform.position = position;
					this.dustInstance.position = base.transform.position;
					this.dustInstance.gameObject.SetActive(true);
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001CAA RID: 7338 RVA: 0x00106150 File Offset: 0x00104550
	private IEnumerator check_dying_cr()
	{
		for (;;)
		{
			if (this.onLeft)
			{
				if (base.transform.position.x > 840f)
				{
					break;
				}
			}
			else if (base.transform.position.x < -840f)
			{
				break;
			}
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x0010616B File Offset: 0x0010456B
	private void EightBallDead()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.eight_ball_death_cr());
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x00106180 File Offset: 0x00104580
	private IEnumerator eight_ball_death_cr()
	{
		float speed = 2500f;
		float angle = (float)UnityEngine.Random.Range(0, 360);
		Vector3 dir = MathUtils.AngleToDirection(angle);
		base.GetComponent<Collider2D>().enabled = false;
		for (;;)
		{
			base.transform.position += dir * speed * CupheadTime.FixedDelta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x0010619C File Offset: 0x0010459C
	protected override void OnDestroy()
	{
		if (this.shadowInstance != null)
		{
			UnityEngine.Object.Destroy(this.shadowInstance.gameObject);
		}
		if (this.dustInstance != null)
		{
			UnityEngine.Object.Destroy(this.dustInstance.gameObject);
		}
		base.OnDestroy();
		this.shadowPrefab = null;
		this.dustPrefab = null;
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x001061FF File Offset: 0x001045FF
	protected override void Die()
	{
		base.Die();
		DicePalaceEightBallLevelEightBall dicePalaceEightBallLevelEightBall = this.parent;
		dicePalaceEightBallLevelEightBall.OnEightBallDeath = (Action)Delegate.Remove(dicePalaceEightBallLevelEightBall.OnEightBallDeath, new Action(this.EightBallDead));
	}

	// Token: 0x0400258D RID: 9613
	private const float OffsetY = 55f;

	// Token: 0x0400258E RID: 9614
	[SerializeField]
	private GameObject shadowPrefab;

	// Token: 0x0400258F RID: 9615
	[SerializeField]
	private GameObject dustPrefab;

	// Token: 0x04002590 RID: 9616
	[SerializeField]
	private GameObject[] colorVariations;

	// Token: 0x04002591 RID: 9617
	private DicePalaceEightBallLevelEightBall parent;

	// Token: 0x04002592 RID: 9618
	private float horSpeed;

	// Token: 0x04002593 RID: 9619
	private float verSpeed;

	// Token: 0x04002594 RID: 9620
	private float gravity;

	// Token: 0x04002595 RID: 9621
	private float delay;

	// Token: 0x04002596 RID: 9622
	private bool onLeft;

	// Token: 0x04002597 RID: 9623
	private Transform shadowInstance;

	// Token: 0x04002598 RID: 9624
	private Transform dustInstance;
}
