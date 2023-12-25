using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000612 RID: 1554
public class FlowerLevelSeedBullet : AbstractProjectile
{
	// Token: 0x06001F5B RID: 8027 RVA: 0x001201A8 File Offset: 0x0011E5A8
	public void OnBulletSeedStart(FlowerLevelFlower parent, AbstractPlayerController player, float a, float min, float max)
	{
		base.transform.LookAt2D(player.transform.position);
		base.transform.Rotate(Vector3.forward, 180f);
		this.minSpeed = min;
		this.maxSpeed = max;
		this.accelerationTime = a;
		this.player = player;
		this.parent = parent;
		parent.OnDeathEvent += this.Die;
	}

	// Token: 0x06001F5C RID: 8028 RVA: 0x00120218 File Offset: 0x0011E618
	protected override void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.Update();
	}

	// Token: 0x06001F5D RID: 8029 RVA: 0x00120236 File Offset: 0x0011E636
	public void LaunchBullet()
	{
		base.StartCoroutine(this.launch_bullet_cr());
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x00120248 File Offset: 0x0011E648
	private IEnumerator launch_bullet_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		base.transform.LookAt2D(this.player.transform.position);
		base.transform.Rotate(Vector3.forward, 180f);
		for (;;)
		{
			if (this.timePassed < this.accelerationTime)
			{
				this.timePassed += CupheadTime.FixedDelta;
			}
			if (!this.isDead)
			{
				this.speed = this.minSpeed + (this.maxSpeed - this.minSpeed) * this.timePassed;
			}
			if (this.speed > 0f && !this.launched)
			{
				base.animator.SetTrigger("Launch");
				this.launched = true;
				base.StartCoroutine(this.spawn_effect_cr());
			}
			base.transform.position -= base.transform.right * (this.speed * CupheadTime.FixedDelta);
			if (base.transform.position.x < (float)(Level.Current.Left - 100))
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (base.transform.position.y > (float)(Level.Current.Ceiling + 100))
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x00120264 File Offset: 0x0011E664
	private IEnumerator spawn_effect_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		Effect puff = UnityEngine.Object.Instantiate<Effect>(this.puffPrefab);
		puff.transform.position = this.root.transform.position;
		puff.transform.LookAt2D(this.player.transform.position);
		yield break;
	}

	// Token: 0x06001F60 RID: 8032 RVA: 0x0012027F File Offset: 0x0011E67F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001F61 RID: 8033 RVA: 0x001202A8 File Offset: 0x0011E6A8
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		FlowerLevelMiniFlowerSpawn component = hit.GetComponent<FlowerLevelMiniFlowerSpawn>();
		if (component != null)
		{
			component.FriendlyFireDamage();
			this.Die();
			base.OnCollisionEnemy(hit, phase);
		}
	}

	// Token: 0x06001F62 RID: 8034 RVA: 0x001202DC File Offset: 0x0011E6DC
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		this.DeathAudio();
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x06001F63 RID: 8035 RVA: 0x001202EC File Offset: 0x0011E6EC
	protected override void Die()
	{
		this.isDead = true;
		this.speed = 0f;
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x06001F64 RID: 8036 RVA: 0x00120318 File Offset: 0x0011E718
	private void DeathAudio()
	{
		AudioManager.Play("flower_bullet_seed_poof");
	}

	// Token: 0x06001F65 RID: 8037 RVA: 0x00120324 File Offset: 0x0011E724
	protected override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.Die;
		base.OnDestroy();
		this.puffPrefab = null;
	}

	// Token: 0x040027F6 RID: 10230
	[SerializeField]
	private Effect puffPrefab;

	// Token: 0x040027F7 RID: 10231
	[SerializeField]
	private Transform root;

	// Token: 0x040027F8 RID: 10232
	private bool isDead;

	// Token: 0x040027F9 RID: 10233
	private bool launched;

	// Token: 0x040027FA RID: 10234
	private float speed;

	// Token: 0x040027FB RID: 10235
	private float minSpeed;

	// Token: 0x040027FC RID: 10236
	private float maxSpeed;

	// Token: 0x040027FD RID: 10237
	private float timePassed;

	// Token: 0x040027FE RID: 10238
	private float accelerationTime;

	// Token: 0x040027FF RID: 10239
	private FlowerLevelFlower parent;

	// Token: 0x04002800 RID: 10240
	private AbstractPlayerController player;
}
