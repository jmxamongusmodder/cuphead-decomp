using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007B3 RID: 1971
public class SallyStagePlayLevelProjectile : AbstractCollidableObject
{
	// Token: 0x06002C5F RID: 11359 RVA: 0x001A154F File Offset: 0x0019F94F
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		base.Awake();
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x001A1564 File Offset: 0x0019F964
	private void Update()
	{
		this.sprite.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x001A15B0 File Offset: 0x0019F9B0
	public void Init(Vector2 pos, float rotation, LevelProperties.SallyStagePlay.Projectile properties)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.speed = properties.projectileSpeed;
		base.transform.SetEulerAngles(null, null, new float?(rotation));
		base.StartCoroutine(this.move_cr());
		AudioManager.Play("sally_fan_shoot");
		this.emitAudioFromObject.Add("sally_fan_shoot");
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x001A162C File Offset: 0x0019FA2C
	private IEnumerator move_cr()
	{
		AudioManager.PlayLoop("sally_fan_shoot_loop");
		this.emitAudioFromObject.Add("sally_fan_shoot_loop");
		while (base.transform.position.y > (float)Level.Current.Ground)
		{
			base.transform.position += base.transform.right * this.speed * CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("OnLand");
		AudioManager.Play("sally_fan_stick");
		this.emitAudioFromObject.Add("sally_fan_stick");
		AudioManager.Stop("sally_fan_shoot_loop");
		yield return CupheadTime.WaitForSeconds(this, this.properties.groundDuration);
		base.animator.SetTrigger("OnDeath");
		yield break;
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x001A1647 File Offset: 0x0019FA47
	private void Die()
	{
		this.StopAllCoroutines();
		AudioManager.Play("sally_fan_dissappear");
		this.emitAudioFromObject.Add("sally_fan_dissappear");
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x001A1674 File Offset: 0x0019FA74
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x040034F8 RID: 13560
	[SerializeField]
	private Transform sprite;

	// Token: 0x040034F9 RID: 13561
	private float speed;

	// Token: 0x040034FA RID: 13562
	private DamageDealer damageDealer;

	// Token: 0x040034FB RID: 13563
	private LevelProperties.SallyStagePlay.Projectile properties;
}
