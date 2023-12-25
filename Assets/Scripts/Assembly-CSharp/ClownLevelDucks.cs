using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000567 RID: 1383
public class ClownLevelDucks : AbstractProjectile
{
	// Token: 0x06001A08 RID: 6664 RVA: 0x000EDF3C File Offset: 0x000EC33C
	protected override void Awake()
	{
		base.Awake();
		this.bombDropped = false;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.body.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.collisionChild = this.body.GetComponent<CollisionChild>();
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		if (this.isBombDuck)
		{
			this.bomb.GetComponent<Transform>();
			this.bomb.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		}
	}

	// Token: 0x06001A09 RID: 6665 RVA: 0x000EDFF1 File Offset: 0x000EC3F1
	public ClownLevelDucks Init(Vector2 pos, LevelProperties.Clown.Duck properties, float maxYPos, float speedY)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.maxYPos = maxYPos;
		this.speedY = speedY;
		this.originalSpeed = this.speedY;
		return this;
	}

	// Token: 0x06001A0A RID: 6666 RVA: 0x000EE027 File Offset: 0x000EC427
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001A0B RID: 6667 RVA: 0x000EE03C File Offset: 0x000EC43C
	protected override void Update()
	{
		base.Update();
		this.VaryingSpeed();
		if (this.isBombDuck)
		{
			this.BombCheck();
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A0C RID: 6668 RVA: 0x000EE071 File Offset: 0x000EC471
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.StartCoroutine(this.spin_cr());
		if (this.isBombDuck && !this.bombDropped)
		{
			base.StartCoroutine(this.drop_bomb_cr());
		}
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x000EE0A3 File Offset: 0x000EC4A3
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A0E RID: 6670 RVA: 0x000EE0C1 File Offset: 0x000EC4C1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.sparkPrefab = null;
		this.explosionPrefab = null;
		this.smokePrefab = null;
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x000EE0E0 File Offset: 0x000EC4E0
	private IEnumerator move_cr()
	{
		bool goingUp = false;
		float stopDist = 100f;
		float endPos = 360f - this.maxYPos;
		this.slowDown = true;
		while (base.transform.position.x > -840f)
		{
			Vector3 pos = base.transform.position;
			while (base.transform.position.y != endPos)
			{
				float dist = base.transform.position.y - endPos;
				dist = Mathf.Abs(dist);
				if (dist < stopDist)
				{
					this.slowDown = true;
				}
				pos.x -= this.properties.duckXMovementSpeed * CupheadTime.Delta;
				pos.y = Mathf.MoveTowards(base.transform.position.y, endPos, this.speedY * CupheadTime.Delta);
				base.transform.position = pos;
				yield return null;
			}
			goingUp = !goingUp;
			endPos = ((!goingUp) ? (360f - this.maxYPos) : 360f);
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06001A10 RID: 6672 RVA: 0x000EE0FC File Offset: 0x000EC4FC
	private void VaryingSpeed()
	{
		float num = 4f;
		if (this.slowDown)
		{
			if (this.speedY <= this.originalSpeed / 3f)
			{
				this.slowDown = false;
			}
			else
			{
				this.speedY -= num;
			}
		}
		else if (this.speedY < this.originalSpeed)
		{
			this.speedY += num;
		}
		else
		{
			this.speedY = this.originalSpeed;
		}
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x000EE180 File Offset: 0x000EC580
	private IEnumerator spin_cr()
	{
		AudioManager.Play("clown_regular_duck_spin");
		this.emitAudioFromObject.Add("clown_regular_duck_spin");
		Effect spark = UnityEngine.Object.Instantiate<Effect>(this.sparkPrefab);
		spark.transform.position = base.transform.position;
		base.animator.SetBool("Spin", true);
		base.transform.GetComponent<Collider2D>().enabled = false;
		this.body.transform.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.properties.spinDuration);
		base.animator.SetBool("Spin", false);
		base.transform.GetComponent<Collider2D>().enabled = true;
		this.body.transform.GetComponent<Collider2D>().enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06001A12 RID: 6674 RVA: 0x000EE19C File Offset: 0x000EC59C
	private void BombCheck()
	{
		this.player = PlayerManager.GetNext();
		float num = 10f;
		float num2 = this.player.transform.position.x - base.transform.position.x;
		if (num2 > -num && num2 < num && !this.bombDropped)
		{
			base.StartCoroutine(this.drop_bomb_cr());
		}
	}

	// Token: 0x06001A13 RID: 6675 RVA: 0x000EE210 File Offset: 0x000EC610
	private IEnumerator drop_bomb_cr()
	{
		float vel = this.properties.bombSpeed;
		float acceleration = 5f;
		this.bombDropped = true;
		this.bomb.transform.parent = null;
		this.bomb.GetComponent<Animator>().SetBool("Fall", true);
		while (this.bomb.transform.position.y > (float)Level.Current.Ground)
		{
			this.bomb.transform.AddPosition(0f, -vel * CupheadTime.Delta, 0f);
			vel += acceleration;
			yield return null;
		}
		this.bomb.GetComponent<Animator>().SetBool("Fall", false);
		Effect explosion = UnityEngine.Object.Instantiate<Effect>(this.explosionPrefab);
		explosion.transform.position = this.bomb.transform.position;
		int num = UnityEngine.Random.Range(0, 3);
		if (num == 3)
		{
			Effect effect = UnityEngine.Object.Instantiate<Effect>(this.smokePrefab);
			effect.transform.position = this.bomb.transform.position;
		}
		AudioManager.Play("clown_bulb_explosion");
		this.emitAudioFromObject.Add("clown_bulb_explosion");
		this.CreatePieces();
		UnityEngine.Object.Destroy(this.bomb.gameObject);
		yield break;
	}

	// Token: 0x06001A14 RID: 6676 RVA: 0x000EE22C File Offset: 0x000EC62C
	private void CreatePieces()
	{
		foreach (SpriteDeathParts spriteDeathParts in this.deathParts)
		{
			spriteDeathParts.CreatePart(this.bomb.transform.position);
		}
	}

	// Token: 0x06001A15 RID: 6677 RVA: 0x000EE26F File Offset: 0x000EC66F
	public override void OnParry(AbstractPlayerController player)
	{
		base.animator.SetBool("Spin", true);
		base.transform.GetComponent<Collider2D>().enabled = false;
		this.body.transform.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001A16 RID: 6678 RVA: 0x000EE2A9 File Offset: 0x000EC6A9
	protected override void Die()
	{
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x0400232C RID: 9004
	public bool isBombDuck;

	// Token: 0x0400232D RID: 9005
	[SerializeField]
	private Effect explosionPrefab;

	// Token: 0x0400232E RID: 9006
	[SerializeField]
	private Effect smokePrefab;

	// Token: 0x0400232F RID: 9007
	[SerializeField]
	private Effect sparkPrefab;

	// Token: 0x04002330 RID: 9008
	[SerializeField]
	private SpriteDeathParts[] deathParts;

	// Token: 0x04002331 RID: 9009
	[SerializeField]
	private Transform bomb;

	// Token: 0x04002332 RID: 9010
	[SerializeField]
	private GameObject body;

	// Token: 0x04002333 RID: 9011
	private LevelProperties.Clown.Duck properties;

	// Token: 0x04002334 RID: 9012
	private AbstractPlayerController player;

	// Token: 0x04002335 RID: 9013
	private DamageReceiver damageReceiver;

	// Token: 0x04002336 RID: 9014
	private CollisionChild collisionChild;

	// Token: 0x04002337 RID: 9015
	private float maxYPos;

	// Token: 0x04002338 RID: 9016
	private float speedY;

	// Token: 0x04002339 RID: 9017
	private float originalSpeed;

	// Token: 0x0400233A RID: 9018
	private bool slowDown;

	// Token: 0x0400233B RID: 9019
	private bool bombDropped;
}
