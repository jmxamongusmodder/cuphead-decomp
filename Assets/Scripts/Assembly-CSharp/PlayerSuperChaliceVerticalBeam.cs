using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A58 RID: 2648
public class PlayerSuperChaliceVerticalBeam : AbstractPlayerSuper
{
	// Token: 0x06003F1A RID: 16154 RVA: 0x00228E63 File Offset: 0x00227263
	protected override void Awake()
	{
		base.Awake();
		this.damageReceivers = new List<DamageReceiver>();
	}

	// Token: 0x06003F1B RID: 16155 RVA: 0x00228E78 File Offset: 0x00227278
	protected override void Update()
	{
		base.Update();
		if (this.updateStraw)
		{
			this.UpdateStraw();
		}
		if (this.player == null)
		{
			this.Interrupt();
		}
		else
		{
			this.player.transform.position = base.transform.position;
		}
	}

	// Token: 0x06003F1C RID: 16156 RVA: 0x00228ED4 File Offset: 0x002272D4
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

	// Token: 0x06003F1D RID: 16157 RVA: 0x00228F14 File Offset: 0x00227314
	private void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer dealer)
	{
		Collider2D componentInChildren = receiver.GetComponentInChildren<Collider2D>();
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
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
		vector2 = new Vector2(componentInChildren.transform.position.x + UnityEngine.Random.Range(-vector.x / 2f, vector.x / 2f), componentInChildren.transform.position.y + UnityEngine.Random.Range(-vector.y / 2f, vector.y / 2f));
		vector2 += componentInChildren.offset;
		this.hitPrefab.Create(vector2);
	}

	// Token: 0x06003F1E RID: 16158 RVA: 0x0022901C File Offset: 0x0022741C
	protected override void Fire()
	{
		AudioManager.Play("player_super_chalice_superbeam");
		base.Fire();
		this.damageDealer = new DamageDealer(WeaponProperties.LevelSuperChaliceVertBeam.damage, WeaponProperties.LevelSuperChaliceVertBeam.damageRate, DamageDealer.DamageSource.Super, false, true, true);
		this.damageDealer.OnDealDamage += this.OnDealDamage;
		this.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
		this.damageDealer.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		meterScoreTracker.Add(this.damageDealer);
	}

	// Token: 0x06003F1F RID: 16159 RVA: 0x002290AC File Offset: 0x002274AC
	protected override void StartSuper()
	{
		if (this.player == null)
		{
			return;
		}
		this.player.weaponManager.OnSuperStart -= this.player.motor.StartSuper;
		this.player.weaponManager.OnSuperEnd -= this.player.motor.OnSuperEnd;
		base.StartSuper();
		string str = (!this.player.motor.Grounded) ? "_Air" : string.Empty;
		base.animator.Play("Vert_Beam_Loop" + str);
		AudioManager.Play("player_super_chalice_superbeam_start");
	}

	// Token: 0x06003F20 RID: 16160 RVA: 0x00229162 File Offset: 0x00227562
	private void AnimationDone()
	{
		if (this.player != null)
		{
			this.player.motor.CheckForPostSuperHop();
		}
		this.EndSuper(true);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003F21 RID: 16161 RVA: 0x00229198 File Offset: 0x00227598
	private void UpdateStraw()
	{
		Camera main = Camera.main;
		Transform transform = main.transform;
		this.StrawFX.transform.SetPosition(null, new float?(transform.position.y + -200f * base.transform.localScale.y), null);
	}

	// Token: 0x06003F22 RID: 16162 RVA: 0x00229202 File Offset: 0x00227602
	private void Ani_LockStraw()
	{
		this.updateStraw = true;
	}

	// Token: 0x06003F23 RID: 16163 RVA: 0x0022920B File Offset: 0x0022760B
	private void Ani_UnLockStraw()
	{
		this.updateStraw = false;
		AudioManager.Play("player_super_chalice_superbeam_end");
	}

	// Token: 0x06003F24 RID: 16164 RVA: 0x0022921E File Offset: 0x0022761E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.hitPrefab = null;
		this.damageReceivers.Clear();
		this.damageReceivers = null;
	}

	// Token: 0x04004632 RID: 17970
	private const float STRAW_Y_OFFSET = -200f;

	// Token: 0x04004633 RID: 17971
	[Header("Effects")]
	[SerializeField]
	private Effect hitPrefab;

	// Token: 0x04004634 RID: 17972
	[SerializeField]
	private GameObject StrawFX;

	// Token: 0x04004635 RID: 17973
	private bool updateStraw;

	// Token: 0x04004636 RID: 17974
	private List<DamageReceiver> damageReceivers;
}
