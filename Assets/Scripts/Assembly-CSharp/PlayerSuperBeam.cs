using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A4F RID: 2639
public class PlayerSuperBeam : AbstractPlayerSuper
{
	// Token: 0x06003EDC RID: 16092 RVA: 0x00226BB3 File Offset: 0x00224FB3
	protected override void Awake()
	{
		base.Awake();
		this.damageReceivers = new List<DamageReceiver>();
	}

	// Token: 0x06003EDD RID: 16093 RVA: 0x00226BC6 File Offset: 0x00224FC6
	protected override void StartSuper()
	{
		base.StartSuper();
		AudioManager.Play("player_super_beam_start");
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x06003EDE RID: 16094 RVA: 0x00226BE8 File Offset: 0x00224FE8
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		DamageReceiver component = hit.GetComponent<DamageReceiver>();
		if (component != null)
		{
			if (this.damageReceivers.Contains(component))
			{
				return;
			}
			this.damageReceivers.Add(component);
		}
	}

	// Token: 0x06003EDF RID: 16095 RVA: 0x00226C28 File Offset: 0x00225028
	private void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer dealer)
	{
		Collider2D componentInChildren = receiver.GetComponentInChildren<Collider2D>();
		Vector2 vector = Vector2.zero;
		Vector2 zero = Vector2.zero;
		if (componentInChildren.GetType() == typeof(BoxCollider2D))
		{
			vector = (componentInChildren as BoxCollider2D).size;
		}
		else
		{
			if (componentInChildren.GetType() != typeof(CircleCollider2D))
			{
				return;
			}
			vector = Vector2.one * (componentInChildren as CircleCollider2D).radius;
		}
		float x = receiver.transform.position.x + UnityEngine.Random.Range(-vector.x / 2f, vector.x / 2f);
		zero = new Vector2(x, base.transform.position.y + (float)UnityEngine.Random.Range(-100, 100));
		this.hitPrefab.Create(zero);
	}

	// Token: 0x06003EE0 RID: 16096 RVA: 0x00226D10 File Offset: 0x00225110
	private IEnumerator super_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Start", false, true);
		this.Fire();
		yield return CupheadTime.WaitForSeconds(this, WeaponProperties.LevelSuperBeam.time);
		base.animator.SetTrigger("OnEnd");
		AudioManager.Play("player_super_beam_end_ground");
		AudioManager.Stop("player_superbeam_firing_loop");
		this.EndSuper(true);
		yield break;
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x00226D2C File Offset: 0x0022512C
	protected override void Fire()
	{
		base.Fire();
		AudioManager.Play("player_superbeam_firing_loop");
		AudioManager.Play("player_superbeam_milk_explosion");
		this.damageDealer = new DamageDealer(WeaponProperties.LevelSuperBeam.damage, WeaponProperties.LevelSuperBeam.damageRate, DamageDealer.DamageSource.Super, false, true, true);
		this.damageDealer.OnDealDamage += this.OnDealDamage;
		this.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
		this.damageDealer.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		meterScoreTracker.Add(this.damageDealer);
	}

	// Token: 0x06003EE2 RID: 16098 RVA: 0x00226DC3 File Offset: 0x002251C3
	private void OnEndAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003EE3 RID: 16099 RVA: 0x00226DD0 File Offset: 0x002251D0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.hitPrefab = null;
		this.damageReceivers.Clear();
		this.damageReceivers = null;
	}

	// Token: 0x040045DD RID: 17885
	[Header("Effects")]
	[SerializeField]
	private Effect hitPrefab;

	// Token: 0x040045DE RID: 17886
	private List<DamageReceiver> damageReceivers;
}
