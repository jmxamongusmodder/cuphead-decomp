using System;
using UnityEngine;

// Token: 0x0200066A RID: 1642
public class FlyingGenieLevelGenieHead : AbstractProjectile
{
	// Token: 0x06002263 RID: 8803 RVA: 0x00142137 File Offset: 0x00140537
	public void Init(Vector3 pos, float health, FlyingGenieLevelGenie parent)
	{
		base.transform.position = pos;
		this.parent = parent;
		this.health = health;
	}

	// Token: 0x06002264 RID: 8804 RVA: 0x00142154 File Offset: 0x00140554
	protected override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.darkSprite.sortingOrder = base.GetComponent<SpriteRenderer>().sortingOrder + 1;
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x001421A2 File Offset: 0x001405A2
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x001421C0 File Offset: 0x001405C0
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x001421DE File Offset: 0x001405DE
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.Die();
		}
		this.parent.DoDamage(info.damage);
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x0014221C File Offset: 0x0014061C
	protected override void Die()
	{
		AudioManager.Play("genie_pillar_destruction");
		this.emitAudioFromObject.Add("genie_pillar_destruction");
		this.headExplode.Create(new Vector3(base.transform.position.x - 75f, base.transform.position.y));
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.darkSprite.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x04002B0F RID: 11023
	[SerializeField]
	private Effect headExplode;

	// Token: 0x04002B10 RID: 11024
	[SerializeField]
	private SpriteRenderer darkSprite;

	// Token: 0x04002B11 RID: 11025
	private DamageReceiver damageReceiver;

	// Token: 0x04002B12 RID: 11026
	private FlyingGenieLevelGenie parent;

	// Token: 0x04002B13 RID: 11027
	private float health;
}
