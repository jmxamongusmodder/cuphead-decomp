using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051D RID: 1309
public class BeeLevelQueenFollower : AbstractProjectile
{
	// Token: 0x17000328 RID: 808
	// (get) Token: 0x06001766 RID: 5990 RVA: 0x000D2BCD File Offset: 0x000D0FCD
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x000D2BD4 File Offset: 0x000D0FD4
	public BeeLevelQueenFollower Create(Vector2 pos, BeeLevelQueenFollower.Properties properties)
	{
		BeeLevelQueenFollower beeLevelQueenFollower = base.Create() as BeeLevelQueenFollower;
		beeLevelQueenFollower.transform.position = pos;
		beeLevelQueenFollower.properties = properties;
		return beeLevelQueenFollower;
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x06001768 RID: 5992 RVA: 0x000D2C06 File Offset: 0x000D1006
	protected override float DestroyLifetime
	{
		get
		{
			return 300f;
		}
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x000D2C10 File Offset: 0x000D1010
	protected override void Awake()
	{
		base.Awake();
		this.circleCollider = base.GetComponent<CircleCollider2D>();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		AudioManager.PlayLoop("bee_queen_follower_loop");
		this.emitAudioFromObject.Add("bee_queen_follower_loop");
		base.StartCoroutine(this.check_pos_cr());
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x000D2C7C File Offset: 0x000D107C
	private IEnumerator check_pos_cr()
	{
		float offset = 175f;
		while (base.transform.position.y < (float)Level.Current.Ceiling + offset && base.transform.position.y > (float)Level.Current.Ground - offset && base.transform.position.x > (float)Level.Current.Left - offset && base.transform.position.x < (float)Level.Current.Right + offset)
		{
			yield return null;
		}
		this.Die();
		yield break;
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x000D2C97 File Offset: 0x000D1097
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		this.childPrefab = null;
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x000D2CBD File Offset: 0x000D10BD
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.attacking && this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x000D2CF1 File Offset: 0x000D10F1
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.properties.health -= info.damage;
		if (this.properties.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x0600176E RID: 5998 RVA: 0x000D2D26 File Offset: 0x000D1126
	protected override void Start()
	{
		base.Start();
		if (this.properties.parryable)
		{
			this.SetParryable(true);
		}
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x0600176F RID: 5999 RVA: 0x000D2D54 File Offset: 0x000D1154
	protected override void Update()
	{
		base.Update();
		if (this.aim == null || this.properties.player == null || base.dead)
		{
			return;
		}
		base.transform.position += base.transform.right * (this.properties.speed * CupheadTime.Delta);
		this.aim.LookAt2D(this.properties.player.center);
		if (this.rotate)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.aim.rotation, this.properties.rotationSpeed * CupheadTime.Delta);
		}
	}

	// Token: 0x06001770 RID: 6000 RVA: 0x000D2E38 File Offset: 0x000D1238
	protected override void Die()
	{
		base.Die();
		AudioManager.Stop("bee_queen_follower_loop");
		this.circleCollider.enabled = false;
		this.StopAllCoroutines();
	}

	// Token: 0x06001771 RID: 6001 RVA: 0x000D2E5C File Offset: 0x000D125C
	private IEnumerator go_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.introTime);
		base.animator.SetTrigger("Continue");
		this.attacking = true;
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
		float t = 0f;
		while (t < 2f)
		{
			float val = t / 2f;
			this.properties.speed = Mathf.Lerp(0f, this.properties.speedMax, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.properties.speed = this.properties.speedMax;
		yield return CupheadTime.WaitForSeconds(this, this.properties.homingTime);
		this.rotate = false;
		yield break;
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x000D2E78 File Offset: 0x000D1278
	private IEnumerator children_cr()
	{
		for (;;)
		{
			this.childPrefab.Create(base.transform.position, (float)UnityEngine.Random.Range(0, 360), 0f, this.properties.childHealth);
			yield return CupheadTime.WaitForSeconds(this, this.properties.childDelay);
		}
		yield break;
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x000D2E93 File Offset: 0x000D1293
	public override void OnParry(AbstractPlayerController player)
	{
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x000D2EA2 File Offset: 0x000D12A2
	public override void OnParryDie()
	{
	}

	// Token: 0x06001775 RID: 6005 RVA: 0x000D2EA4 File Offset: 0x000D12A4
	private IEnumerator timer_cr()
	{
		this.SetParryable(false);
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.SetParryable(true);
		yield break;
	}

	// Token: 0x040020A0 RID: 8352
	public float coolDown = 0.4f;

	// Token: 0x040020A1 RID: 8353
	[SerializeField]
	private BasicDamagableProjectile childPrefab;

	// Token: 0x040020A2 RID: 8354
	private BeeLevelQueenFollower.Properties properties;

	// Token: 0x040020A3 RID: 8355
	private Transform aim;

	// Token: 0x040020A4 RID: 8356
	private CircleCollider2D circleCollider;

	// Token: 0x040020A5 RID: 8357
	private DamageReceiver damageReceiver;

	// Token: 0x040020A6 RID: 8358
	private bool attacking;

	// Token: 0x040020A7 RID: 8359
	private bool rotate = true;

	// Token: 0x0200051E RID: 1310
	public class Properties
	{
		// Token: 0x06001776 RID: 6006 RVA: 0x000D2EC0 File Offset: 0x000D12C0
		public Properties(AbstractPlayerController player, float introTime, float speed, float rotationSpeed, float homingTime, float health, float childDelay, float childHealth, bool parryable)
		{
			this.player = player;
			this.introTime = introTime;
			this.speedMax = speed;
			this.rotationSpeed = rotationSpeed;
			this.homingTime = homingTime;
			this.healthMax = health;
			this.childDelay = childDelay;
			this.childHealth = childHealth;
			this.speed = 0f;
			this.health = health;
			this.parryable = parryable;
		}

		// Token: 0x040020A8 RID: 8360
		public readonly AbstractPlayerController player;

		// Token: 0x040020A9 RID: 8361
		public readonly float introTime;

		// Token: 0x040020AA RID: 8362
		public readonly float speedMax;

		// Token: 0x040020AB RID: 8363
		public readonly float rotationSpeed;

		// Token: 0x040020AC RID: 8364
		public readonly float homingTime;

		// Token: 0x040020AD RID: 8365
		public readonly float healthMax;

		// Token: 0x040020AE RID: 8366
		public readonly float childDelay;

		// Token: 0x040020AF RID: 8367
		public readonly float childHealth;

		// Token: 0x040020B0 RID: 8368
		public readonly bool parryable;

		// Token: 0x040020B1 RID: 8369
		public float speed;

		// Token: 0x040020B2 RID: 8370
		public float health;
	}
}
