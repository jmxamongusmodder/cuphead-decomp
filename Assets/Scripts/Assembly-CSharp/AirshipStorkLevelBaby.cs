using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
public class AirshipStorkLevelBaby : AbstractCollidableObject
{
	// Token: 0x17000319 RID: 793
	// (get) Token: 0x0600152F RID: 5423 RVA: 0x000BDFAE File Offset: 0x000BC3AE
	// (set) Token: 0x06001530 RID: 5424 RVA: 0x000BDFB6 File Offset: 0x000BC3B6
	public AirshipStorkLevelBaby.State state { get; private set; }

	// Token: 0x06001531 RID: 5425 RVA: 0x000BDFBF File Offset: 0x000BC3BF
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x000BDFF5 File Offset: 0x000BC3F5
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x000BE00D File Offset: 0x000BC40D
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f && this.state != AirshipStorkLevelBaby.State.Dying)
		{
			this.state = AirshipStorkLevelBaby.State.Dying;
			this.Die();
		}
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x000BE04B File Offset: 0x000BC44B
	public void Init(LevelProperties.AirshipStork.Babies properties, Vector2 pos, float health)
	{
		this.properties = properties;
		this.health = health;
		base.transform.position = pos;
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x000BE079 File Offset: 0x000BC479
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x000BE090 File Offset: 0x000BC490
	private IEnumerator jump_cr()
	{
		this.state = AirshipStorkLevelBaby.State.Move;
		string[] pattern = this.properties.babyDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int i = UnityEngine.Random.Range(0, pattern.Length);
		float waitTime = 0f;
		this.onGroundY = (float)Level.Current.Ground;
		while (base.transform.position.x > -740f)
		{
			if (pattern[i][0] == 'D')
			{
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, waitTime);
				bool goingUp = true;
				bool highJump = pattern[i][0] == 'H';
				float velocityY = (!highJump) ? this.properties.lowVerticalSpeed : this.properties.highVerticalSpeed;
				float speedX = (!highJump) ? this.properties.lowHorizontalSpeed : this.properties.highHorizontalSpeed;
				this.gravity = ((!highJump) ? this.properties.lowGravity : this.properties.highGravity);
				while (goingUp || base.transform.position.y > this.onGroundY)
				{
					velocityY -= this.gravity * CupheadTime.FixedDelta;
					base.transform.AddPosition(-speedX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0f);
					if (velocityY < 0f && goingUp)
					{
						goingUp = false;
					}
					yield return null;
				}
				base.transform.SetPosition(null, new float?(this.onGroundY), null);
			}
			i = (i + 1) % pattern.Length;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x000BE0AB File Offset: 0x000BC4AB
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001E8E RID: 7822
	private LevelProperties.AirshipStork.Babies properties;

	// Token: 0x04001E8F RID: 7823
	private DamageDealer damageDealer;

	// Token: 0x04001E90 RID: 7824
	private DamageReceiver damageReceiver;

	// Token: 0x04001E91 RID: 7825
	private float onGroundY;

	// Token: 0x04001E92 RID: 7826
	private float gravity;

	// Token: 0x04001E93 RID: 7827
	private float health;

	// Token: 0x020004D9 RID: 1241
	public enum State
	{
		// Token: 0x04001E95 RID: 7829
		Move,
		// Token: 0x04001E96 RID: 7830
		Dying
	}
}
