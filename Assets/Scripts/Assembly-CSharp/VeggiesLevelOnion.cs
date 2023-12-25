using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200084B RID: 2123
public class VeggiesLevelOnion : LevelProperties.Veggies.Entity
{
	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x06003128 RID: 12584 RVA: 0x001CD074 File Offset: 0x001CB474
	// (set) Token: 0x06003129 RID: 12585 RVA: 0x001CD07C File Offset: 0x001CB47C
	public VeggiesLevelOnion.State state { get; private set; }

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x0600312A RID: 12586 RVA: 0x001CD085 File Offset: 0x001CB485
	// (set) Token: 0x0600312B RID: 12587 RVA: 0x001CD08D File Offset: 0x001CB48D
	public bool HappyLeave { get; private set; }

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x0600312C RID: 12588 RVA: 0x001CD098 File Offset: 0x001CB498
	// (remove) Token: 0x0600312D RID: 12589 RVA: 0x001CD0D0 File Offset: 0x001CB4D0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VeggiesLevelOnion.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x0600312E RID: 12590 RVA: 0x001CD108 File Offset: 0x001CB508
	// (remove) Token: 0x0600312F RID: 12591 RVA: 0x001CD140 File Offset: 0x001CB540
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnHappyLeave;

	// Token: 0x06003130 RID: 12592 RVA: 0x001CD178 File Offset: 0x001CB578
	private void Start()
	{
		if (this.properties == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.noSecret = true;
		this.circleCollider = base.GetComponent<CircleCollider2D>();
		this.state = VeggiesLevelOnion.State.Idle;
		base.StartCoroutine(this.happyTimer_cr());
		this.SfxGround();
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x001CD1C9 File Offset: 0x001CB5C9
	private void SfxGround()
	{
		AudioManager.Play("level_veggies_onion_rise");
	}

	// Token: 0x06003132 RID: 12594 RVA: 0x001CD1D5 File Offset: 0x001CB5D5
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003133 RID: 12595 RVA: 0x001CD1F0 File Offset: 0x001CB5F0
	public override void LevelInitWithGroup(AbstractLevelPropertyGroup propertyGroup)
	{
		base.LevelInitWithGroup(propertyGroup);
		this.properties = (propertyGroup as LevelProperties.Veggies.Onion);
		this.hp = (float)this.properties.hp;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 0.2f, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x001CD264 File Offset: 0x001CB664
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state == VeggiesLevelOnion.State.Idle)
		{
			this.state = VeggiesLevelOnion.State.Crying;
			this.noSecret = false;
			base.animator.SetTrigger("SadStart");
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x001CD2E0 File Offset: 0x001CB6E0
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x001CD2FE File Offset: 0x001CB6FE
	private void OnDeathAnimComplete()
	{
		this.state = VeggiesLevelOnion.State.Complete;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x001CD312 File Offset: 0x001CB712
	private void Die()
	{
		this.StopCrying();
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06003138 RID: 12600 RVA: 0x001CD32D File Offset: 0x001CB72D
	private void StartExplosions()
	{
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
	}

	// Token: 0x06003139 RID: 12601 RVA: 0x001CD33A File Offset: 0x001CB73A
	private void StopExplosions()
	{
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
	}

	// Token: 0x0600313A RID: 12602 RVA: 0x001CD348 File Offset: 0x001CB748
	private void StartCrying()
	{
		AudioManager.Play("level_veggies_onion_crying");
		this.rightStream = this.tearStreamPrefab.Create(this.rightRoot.position, 1);
		this.leftStream = this.tearStreamPrefab.Create(this.leftRoot.position, -1);
		this.StartTearCoroutines();
	}

	// Token: 0x0600313B RID: 12603 RVA: 0x001CD3AC File Offset: 0x001CB7AC
	private void StopCrying()
	{
		AudioManager.Stop("level_veggies_onion_crying");
		this.currentCryLoop = 0;
		this.targetCryLoops = this.properties.cryLoops.RandomInt();
		base.animator.SetBool("ContinueCrying", true);
		if (this.rightStream != null)
		{
			this.rightStream.End();
			this.rightStream = null;
		}
		if (this.leftStream != null)
		{
			this.leftStream.End();
			this.leftStream = null;
		}
		this.StopTearCoroutines();
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x001CD43D File Offset: 0x001CB83D
	private void CryLoop()
	{
		this.currentCryLoop++;
		if (this.currentCryLoop >= this.targetCryLoops)
		{
			base.animator.SetBool("ContinueCrying", false);
		}
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x001CD470 File Offset: 0x001CB870
	private void BashfulAnimComplete()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector2 pos;
		bool onLeft;
		if (next.transform.position.x > base.transform.position.x)
		{
			pos = this.radishRootRight.position;
			onLeft = false;
		}
		else
		{
			pos = this.radishRootLeft.position;
			onLeft = true;
		}
		this.homingHeartPrefab.CreateRadish(pos, this.properties.heartMaxSpeed, this.properties.heartAcceleration, this.properties.heartHP, onLeft);
		this.state = VeggiesLevelOnion.State.Complete;
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x001CD510 File Offset: 0x001CB910
	private IEnumerator happyTimer_cr()
	{
		float t = 0f;
		while (t < this.properties.happyTime)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		if (this.noSecret)
		{
			if (this.OnHappyLeave != null)
			{
				this.OnHappyLeave();
			}
			if (this.state == VeggiesLevelOnion.State.Idle)
			{
				this.HappyLeave = true;
				base.animator.SetTrigger("HappyExit");
				base.StartCoroutine(this.handle_dirt_cr());
			}
		}
		yield break;
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x001CD52B File Offset: 0x001CB92B
	public void StopTearCoroutines()
	{
		if (this.rightTearsCoroutine != null)
		{
			base.StopCoroutine(this.rightTearsCoroutine);
		}
		if (this.leftTearsCoroutine != null)
		{
			base.StopCoroutine(this.leftTearsCoroutine);
		}
		this.rightTearsCoroutine = null;
		this.leftTearsCoroutine = null;
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x001CD56C File Offset: 0x001CB96C
	public void StartTearCoroutines()
	{
		this.StopTearCoroutines();
		string pattern = this.properties.tearPatterns.GetRandom<string>().ToUpper();
		this.rightTearsCoroutine = this.tears_cr(VeggiesLevelOnion.Side.Right, pattern);
		this.leftTearsCoroutine = this.tears_cr(VeggiesLevelOnion.Side.Left, pattern);
		base.StartCoroutine(this.rightTearsCoroutine);
		base.StartCoroutine(this.leftTearsCoroutine);
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x001CD5CC File Offset: 0x001CB9CC
	private IEnumerator tears_cr(VeggiesLevelOnion.Side side, string pattern)
	{
		float tearDelay = UnityEngine.Random.Range(0.3f, 0.7f);
		yield return CupheadTime.WaitForSeconds(this, this.properties.tearAnticipate);
		string[] patterns = pattern.Split(new char[]
		{
			','
		});
		int currentPattern = 0;
		int numUntilPink = this.properties.pinkTearRange.RandomInt();
		for (;;)
		{
			if (patterns[currentPattern][0] == 'D')
			{
				float wait = 0f;
				bool success = Parser.FloatTryParse(patterns[currentPattern].Replace("D", string.Empty), out wait);
				if (success)
				{
					yield return CupheadTime.WaitForSeconds(this, wait);
				}
			}
			else
			{
				string[] destinations = patterns[currentPattern].Split(new char[]
				{
					'-'
				});
				for (int i = 0; i < destinations.Length; i++)
				{
					float x = 0f;
					bool success2 = Parser.FloatTryParse(destinations[i], out x);
					if (success2)
					{
						numUntilPink--;
						if (numUntilPink <= 0)
						{
							yield return CupheadTime.WaitForSeconds(this, tearDelay);
							numUntilPink = this.properties.pinkTearRange.RandomInt();
							this.pinkProjectilePrefab.Create(this.properties.tearTime, (side != VeggiesLevelOnion.Side.Right) ? (-x) : x);
						}
						else
						{
							yield return CupheadTime.WaitForSeconds(this, tearDelay);
							this.projectilePrefab.Create(this.properties.tearTime, (side != VeggiesLevelOnion.Side.Right) ? (-x) : x);
						}
						tearDelay = UnityEngine.Random.Range(0.3f, 0.7f);
					}
				}
			}
			currentPattern = (int)Mathf.Repeat((float)(currentPattern + 1), (float)patterns.Length);
			yield return CupheadTime.WaitForSeconds(this, this.properties.tearCommaDelay);
		}
		yield break;
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x001CD5F8 File Offset: 0x001CB9F8
	private IEnumerator die_cr()
	{
		this.circleCollider.enabled = false;
		AudioManager.Play("level_veggies_onion_die");
		base.animator.Play("Sad_Die");
		this.StartExplosions();
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.StopExplosions();
		yield return null;
		yield break;
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x001CD614 File Offset: 0x001CBA14
	private IEnumerator handle_dirt_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("FadeDirt");
		yield break;
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x001CD62F File Offset: 0x001CBA2F
	private void OnionVoiceExisBashfulSFX()
	{
		AudioManager.Play("level_veggies_onion_exit_bashful");
		this.emitAudioFromObject.Add("level_veggies_onion_exit_bashful");
	}

	// Token: 0x040039BE RID: 14782
	private const float START_SHOOTING_TIME = 0.6f;

	// Token: 0x040039C1 RID: 14785
	[SerializeField]
	private Transform leftRoot;

	// Token: 0x040039C2 RID: 14786
	[SerializeField]
	private Transform rightRoot;

	// Token: 0x040039C3 RID: 14787
	[SerializeField]
	private Transform radishRootRight;

	// Token: 0x040039C4 RID: 14788
	[SerializeField]
	private Transform radishRootLeft;

	// Token: 0x040039C5 RID: 14789
	[SerializeField]
	private VeggiesLevelOnionTearsStream tearStreamPrefab;

	// Token: 0x040039C6 RID: 14790
	[SerializeField]
	private VeggiesLevelOnionTearProjectile projectilePrefab;

	// Token: 0x040039C7 RID: 14791
	[SerializeField]
	private VeggiesLevelOnionTearProjectile pinkProjectilePrefab;

	// Token: 0x040039C8 RID: 14792
	[SerializeField]
	private VeggiesLevelOnionHomingHeart homingHeartPrefab;

	// Token: 0x040039C9 RID: 14793
	private new LevelProperties.Veggies.Onion properties;

	// Token: 0x040039CA RID: 14794
	private CircleCollider2D circleCollider;

	// Token: 0x040039CB RID: 14795
	private float hp;

	// Token: 0x040039CC RID: 14796
	private DamageDealer damageDealer;

	// Token: 0x040039CD RID: 14797
	private VeggiesLevelOnionTearsStream rightStream;

	// Token: 0x040039CE RID: 14798
	private VeggiesLevelOnionTearsStream leftStream;

	// Token: 0x040039CF RID: 14799
	private int currentCryLoop;

	// Token: 0x040039D0 RID: 14800
	private int targetCryLoops = 8;

	// Token: 0x040039D3 RID: 14803
	private bool noSecret;

	// Token: 0x040039D4 RID: 14804
	private IEnumerator rightTearsCoroutine;

	// Token: 0x040039D5 RID: 14805
	private IEnumerator leftTearsCoroutine;

	// Token: 0x0200084C RID: 2124
	public enum Side
	{
		// Token: 0x040039D7 RID: 14807
		Left = -1,
		// Token: 0x040039D8 RID: 14808
		Right = 1
	}

	// Token: 0x0200084D RID: 2125
	public enum State
	{
		// Token: 0x040039DA RID: 14810
		Idle,
		// Token: 0x040039DB RID: 14811
		Crying,
		// Token: 0x040039DC RID: 14812
		Complete
	}

	// Token: 0x0200084E RID: 2126
	// (Invoke) Token: 0x06003146 RID: 12614
	public delegate void OnDamageTakenHandler(float damage);
}
