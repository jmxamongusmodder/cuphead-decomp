using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008EC RID: 2284
public class MountainPlatformingLevelRock : AbstractProjectile
{
	// Token: 0x06003590 RID: 13712 RVA: 0x001F34F0 File Offset: 0x001F18F0
	public MountainPlatformingLevelRock Create(Vector2 startPos, Vector2 fallPos, float velocity, float delay)
	{
		MountainPlatformingLevelRock mountainPlatformingLevelRock = base.Create() as MountainPlatformingLevelRock;
		mountainPlatformingLevelRock.transform.position = startPos;
		mountainPlatformingLevelRock.fallPos = fallPos;
		mountainPlatformingLevelRock.velocity = velocity;
		mountainPlatformingLevelRock.delay = delay;
		return mountainPlatformingLevelRock;
	}

	// Token: 0x06003591 RID: 13713 RVA: 0x001F3531 File Offset: 0x001F1931
	protected override void Awake()
	{
		base.Awake();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetBool("PickedA", Rand.Bool());
	}

	// Token: 0x06003592 RID: 13714 RVA: 0x001F355A File Offset: 0x001F195A
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.launch_cr());
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x001F356F File Offset: 0x001F196F
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003594 RID: 13716 RVA: 0x001F358D File Offset: 0x001F198D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x001F35AC File Offset: 0x001F19AC
	private IEnumerator launch_cr()
	{
		float x = UnityEngine.Random.Range(-500f, 500f);
		while (base.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax + 100f)
		{
			base.transform.AddPosition(x * CupheadTime.Delta, 1000f * CupheadTime.Delta, 0f);
			yield return null;
		}
		base.animator.SetTrigger("getBig");
		base.transform.position = this.fallPos;
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Projectiles.ToString();
		yield return CupheadTime.WaitForSeconds(this, this.delay);
		for (;;)
		{
			base.transform.AddPosition(0f, -this.velocity * CupheadTime.Delta, 0f);
			this.velocity += 1000f * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x001F35C8 File Offset: 0x001F19C8
	protected override void Die()
	{
		base.Die();
		AudioManager.Play("castle_giant_rock_smash");
		this.emitAudioFromObject.Add("castle_giant_rock_smash");
		this.StopAllCoroutines();
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.DeathParts();
		CupheadLevelCamera.Current.Shake(10f, 0.4f, false);
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x001F3624 File Offset: 0x001F1A24
	public void DeathParts()
	{
		this.explosion.Create(base.transform.position);
		foreach (SpriteDeathParts spriteDeathParts in this.deathParts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x04003DA9 RID: 15785
	[SerializeField]
	private Effect explosion;

	// Token: 0x04003DAA RID: 15786
	[SerializeField]
	private SpriteDeathParts[] deathParts;

	// Token: 0x04003DAB RID: 15787
	private Vector2 fallPos;

	// Token: 0x04003DAC RID: 15788
	private float velocity;

	// Token: 0x04003DAD RID: 15789
	private const float LAUNCH_VELOCITY_Y = 1000f;

	// Token: 0x04003DAE RID: 15790
	private const float LAUNCH_VELOCITY_X = 500f;

	// Token: 0x04003DAF RID: 15791
	private const float GRAVITY = 1000f;

	// Token: 0x04003DB0 RID: 15792
	private float delay;
}
