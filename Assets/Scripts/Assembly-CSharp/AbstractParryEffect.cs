using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009D0 RID: 2512
public abstract class AbstractParryEffect : Effect
{
	// Token: 0x06003AF9 RID: 15097 RVA: 0x00212BE4 File Offset: 0x00210FE4
	public AbstractParryEffect Create(AbstractPlayerController player)
	{
		AbstractParryEffect abstractParryEffect = base.Create(player.center, player.transform.localScale) as AbstractParryEffect;
		abstractParryEffect.SetPlayer(player);
		return abstractParryEffect;
	}

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x06003AFA RID: 15098
	protected abstract bool IsHit { get; }

	// Token: 0x06003AFB RID: 15099 RVA: 0x00212C18 File Offset: 0x00211018
	public override void Initialize(Vector3 position, Vector3 scale, bool randomR)
	{
		base.Initialize(position, scale, randomR);
		base.animator.enabled = false;
		this.sprites.SetActive(false);
		this.projectiles = new List<AbstractProjectile>();
		this.sparks = new List<Effect>();
		this.switches = new List<ParrySwitch>();
		this.entities = new List<AbstractLevelEntity>();
		base.tag = "Parry";
	}

	// Token: 0x06003AFC RID: 15100 RVA: 0x00212C80 File Offset: 0x00211080
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		if (this.cancel)
		{
			return;
		}
		base.OnCollision(hit, phase);
		if (!this.player.IsDead && phase == CollisionPhase.Enter)
		{
			AbstractProjectile component = hit.GetComponent<AbstractProjectile>();
			if (component == null)
			{
				CollisionChild component2 = hit.GetComponent<CollisionChild>();
				AbstractCollidableObject abstractCollidableObject;
				if (component2 != null && component2.ForwardParry(out abstractCollidableObject))
				{
					component = abstractCollidableObject.GetComponent<AbstractProjectile>();
				}
			}
			if (component != null && component.CanParry)
			{
				this.projectiles.Add(component);
				if (!this.player.stats.NextParryActivatesHealerCharm())
				{
					this.sparks.Add(this.spark.Create(component.transform.position));
				}
				if (!this.didHitSomething)
				{
					base.StartCoroutine(this.hit_cr(false));
				}
			}
			ParrySwitch component3 = hit.GetComponent<ParrySwitch>();
			if (component3 != null && component3.enabled && component3.IsParryable)
			{
				this.switches.Add(component3);
				if (!this.didHitSomething)
				{
					base.StartCoroutine(this.hit_cr(false));
				}
			}
			AbstractLevelEntity component4 = hit.GetComponent<AbstractLevelEntity>();
			if (component4 != null && component4.enabled && component4.canParry)
			{
				this.entities.Add(component4);
				if (!this.didHitSomething)
				{
					base.StartCoroutine(this.hit_cr(false));
				}
			}
			if ((this.player.stats.Loadout.charm == Charm.charm_parry_attack || this.player.stats.CurseWhetsone) && !this.didHitSomething && !Level.IsChessBoss)
			{
				IParryAttack component5 = this.player.GetComponent<IParryAttack>();
				if (component5 != null && !component5.AttackParryUsed)
				{
					DamageReceiver damageReceiver = hit.GetComponent<DamageReceiver>();
					if (damageReceiver == null)
					{
						DamageReceiverChild component6 = hit.GetComponent<DamageReceiverChild>();
						if (component6 != null)
						{
							damageReceiver = component6.Receiver;
						}
					}
					if (damageReceiver != null && damageReceiver.type == DamageReceiver.Type.Enemy)
					{
						component5.HasHitEnemy = true;
						DamageDealer damageDealer = new DamageDealer(WeaponProperties.CharmParryAttack.damage, 0f, false, true, false);
						damageDealer.DealDamage(hit);
						this.ShowParryAttackEffect(hit);
						base.StartCoroutine(this.hit_cr(true));
					}
				}
			}
		}
	}

	// Token: 0x06003AFD RID: 15101 RVA: 0x00212EF4 File Offset: 0x002112F4
	private void ShowParryAttackEffect(GameObject hit)
	{
		int num = Physics2D.RaycastNonAlloc(hit.transform.position, base.transform.position - hit.transform.position, this.contactsBuffer, (base.transform.position - hit.transform.position).magnitude);
		if (num == 0)
		{
			return;
		}
		Vector3 position = this.contactsBuffer[0].point;
		for (int i = 1; i < num; i++)
		{
			if (this.contactsBuffer[i].collider.tag == "Parry")
			{
				position = this.contactsBuffer[i].point;
			}
		}
		ParryAttackSpark parryAttackSpark = this.parryAttack.Create(position) as ParryAttackSpark;
		parryAttackSpark.IsCuphead = (this.player.id == PlayerId.PlayerOne);
		this.sparks.Add(parryAttackSpark);
		parryAttackSpark.Play();
	}

	// Token: 0x06003AFE RID: 15102 RVA: 0x00213008 File Offset: 0x00211408
	protected virtual void SetPlayer(AbstractPlayerController player)
	{
		this.player = player;
		base.transform.SetParent(player.transform);
		base.StartCoroutine(this.lifetime_cr());
	}

	// Token: 0x06003AFF RID: 15103 RVA: 0x0021302F File Offset: 0x0021142F
	protected virtual void OnHitCancel()
	{
		if (this == null)
		{
			return;
		}
		this.Cancel();
		AudioManager.Stop("player_parry");
	}

	// Token: 0x06003B00 RID: 15104 RVA: 0x00213050 File Offset: 0x00211450
	protected virtual void Cancel()
	{
		foreach (Effect effect in this.sparks)
		{
			UnityEngine.Object.Destroy(effect.gameObject);
		}
		this.cancel = true;
		this.CancelSwitch();
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003B01 RID: 15105 RVA: 0x002130D0 File Offset: 0x002114D0
	protected virtual void CancelSwitch()
	{
	}

	// Token: 0x06003B02 RID: 15106 RVA: 0x002130D2 File Offset: 0x002114D2
	protected virtual void OnPaused()
	{
	}

	// Token: 0x06003B03 RID: 15107 RVA: 0x002130D4 File Offset: 0x002114D4
	protected virtual void OnUnpaused()
	{
	}

	// Token: 0x06003B04 RID: 15108 RVA: 0x002130D6 File Offset: 0x002114D6
	protected virtual void OnSuccess()
	{
	}

	// Token: 0x06003B05 RID: 15109 RVA: 0x002130D8 File Offset: 0x002114D8
	protected virtual void OnEnd()
	{
	}

	// Token: 0x06003B06 RID: 15110 RVA: 0x002130DC File Offset: 0x002114DC
	private IEnumerator lifetime_cr()
	{
		if (this.player != null && (this.player.stats.Loadout.charm != Charm.charm_parry_plus || Level.IsChessBoss))
		{
			if (this.player.stats.isChalice)
			{
				yield return CupheadTime.WaitForSeconds(this, (Level.Current.playerMode != PlayerMode.Plane) ? 0.3f : 0.4f);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, 0.2f);
			}
			base.GetComponent<Collider2D>().enabled = false;
			this.CancelSwitch();
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003B07 RID: 15111 RVA: 0x002130F8 File Offset: 0x002114F8
	private IEnumerator hit_cr(bool hitEnemy = false)
	{
		if (this.player.IsDead || !this.player.gameObject.activeInHierarchy || !base.gameObject.activeInHierarchy)
		{
			yield break;
		}
		bool hit = false;
		this.didHitSomething = true;
		IParryAttack parryController = this.player.GetComponent<IParryAttack>();
		if (parryController != null)
		{
			parryController.AttackParryUsed = true;
		}
		base.animator.enabled = true;
		this.sprites.SetActive(true);
		if (!hitEnemy)
		{
			foreach (ParrySwitch parrySwitch in this.switches)
			{
				parrySwitch.OnParryPrePause(this.player);
			}
			foreach (AbstractLevelEntity abstractLevelEntity in this.entities)
			{
				abstractLevelEntity.OnParry(this.player);
			}
			foreach (AbstractProjectile abstractProjectile in this.projectiles)
			{
				abstractProjectile.OnParry(this.player);
				this.player.stats.OnParry(abstractProjectile.ParryMeterMultiplier, abstractProjectile.CountParryTowardsScore);
			}
		}
		if (this.player.IsDead || !this.player.gameObject.activeInHierarchy || !base.gameObject.activeInHierarchy)
		{
			yield break;
		}
		if (Level.Current == null || !Level.IsChessBoss || !Level.Current.Ending)
		{
			PauseManager.Pause();
		}
		AudioManager.Play("player_parry");
		this.OnPaused();
		float pauseTime = (!hitEnemy) ? 0.185f : 0.13875f;
		float t = 0f;
		while (t < pauseTime)
		{
			hit = this.IsHit;
			if (hit)
			{
				t = pauseTime;
			}
			t += Time.fixedDeltaTime;
			for (int i = 0; i < 2; i++)
			{
				PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				if (this.player != null && this.player.id == playerId)
				{
					if (pauseTime - t < 0.134f)
					{
						this.player.BufferInputs();
					}
				}
				else
				{
					AbstractPlayerController abstractPlayerController = PlayerManager.GetPlayer(playerId);
					if (abstractPlayerController != null)
					{
						abstractPlayerController.BufferInputs();
					}
				}
			}
			yield return new WaitForFixedUpdate();
		}
		while (LevelNewPlayerGUI.Current != null && LevelNewPlayerGUI.Current.gameObject.activeInHierarchy)
		{
			yield return null;
		}
		if (!hit)
		{
			this.OnSuccess();
			if (Level.Current == null || !Level.IsChessBoss || !Level.Current.Ending)
			{
				PauseManager.Unpause();
			}
			this.OnUnpaused();
			this.OnEnd();
			base.transform.parent = null;
			base.GetComponent<Collider2D>().enabled = false;
			if (!hitEnemy)
			{
				foreach (ParrySwitch parrySwitch2 in this.switches)
				{
					parrySwitch2.OnParryPostPause(this.player);
				}
			}
		}
		yield break;
	}

	// Token: 0x06003B08 RID: 15112 RVA: 0x0021311A File Offset: 0x0021151A
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.spark = null;
		this.parryAttack = null;
	}

	// Token: 0x040042B5 RID: 17077
	public const string TAG = "Parry";

	// Token: 0x040042B6 RID: 17078
	private const float PAUSE_TIME = 0.185f;

	// Token: 0x040042B7 RID: 17079
	private const float COLLIDER_LIFETIME = 0.2f;

	// Token: 0x040042B8 RID: 17080
	private const float CHALICE_COLLIDER_LIFETIME = 0.3f;

	// Token: 0x040042B9 RID: 17081
	private const float CHALICE_PLANE_COLLIDER_LIFETIME = 0.4f;

	// Token: 0x040042BA RID: 17082
	private const float SPRITE_LIFETIME = 1f;

	// Token: 0x040042BB RID: 17083
	[SerializeField]
	private GameObject sprites;

	// Token: 0x040042BC RID: 17084
	[SerializeField]
	private Effect spark;

	// Token: 0x040042BD RID: 17085
	[SerializeField]
	private ParryAttackSpark parryAttack;

	// Token: 0x040042BE RID: 17086
	protected AbstractPlayerController player;

	// Token: 0x040042BF RID: 17087
	protected bool didHitSomething;

	// Token: 0x040042C0 RID: 17088
	protected bool cancel;

	// Token: 0x040042C1 RID: 17089
	private List<AbstractProjectile> projectiles;

	// Token: 0x040042C2 RID: 17090
	private List<Effect> sparks;

	// Token: 0x040042C3 RID: 17091
	private List<ParrySwitch> switches;

	// Token: 0x040042C4 RID: 17092
	private List<AbstractLevelEntity> entities;

	// Token: 0x040042C5 RID: 17093
	private RaycastHit2D[] contactsBuffer = new RaycastHit2D[10];
}
