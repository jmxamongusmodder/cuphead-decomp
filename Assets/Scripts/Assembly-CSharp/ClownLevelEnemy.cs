using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000568 RID: 1384
public class ClownLevelEnemy : AbstractProjectile
{
	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06001A18 RID: 6680 RVA: 0x000EE911 File Offset: 0x000ECD11
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001A19 RID: 6681 RVA: 0x000EE918 File Offset: 0x000ECD18
	public ClownLevelEnemy Create(Vector3 pos, float targetPosition, float HP, LevelProperties.Clown.Swing properties, ClownLevelClownSwing parent)
	{
		ClownLevelEnemy clownLevelEnemy = base.Create(pos) as ClownLevelEnemy;
		clownLevelEnemy.transform.position = pos;
		clownLevelEnemy.properties = properties;
		clownLevelEnemy.targetPosition = targetPosition;
		clownLevelEnemy.HP = HP;
		clownLevelEnemy.parent = parent;
		return clownLevelEnemy;
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x000EE964 File Offset: 0x000ECD64
	protected override void Start()
	{
		base.Start();
		ClownLevelClownSwing clownLevelClownSwing = this.parent;
		clownLevelClownSwing.OnDeath = (Action)Delegate.Combine(clownLevelClownSwing.OnDeath, new Action(this.Die));
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiver.enabled = false;
		AudioManager.Play("clown_penguin_roll_start");
		this.emitAudioFromObject.Add("clown_penguin_roll_start");
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x000EE9E8 File Offset: 0x000ECDE8
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x000EEA06 File Offset: 0x000ECE06
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x000EEA24 File Offset: 0x000ECE24
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.HP -= info.damage;
		if (this.HP <= 0f && !this.isDead)
		{
			this.isDead = true;
			this.Die();
		}
	}

	// Token: 0x06001A1E RID: 6686 RVA: 0x000EEA61 File Offset: 0x000ECE61
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		if (hit.GetComponent<ClownLevelCoaster>())
		{
			this.Die();
		}
	}

	// Token: 0x06001A1F RID: 6687 RVA: 0x000EEA81 File Offset: 0x000ECE81
	private void StartMoving()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001A20 RID: 6688 RVA: 0x000EEA90 File Offset: 0x000ECE90
	private IEnumerator move_cr()
	{
		Vector3 pos = base.transform.position;
		float target = -640f + this.targetPosition;
		while (base.transform.position.x != target)
		{
			pos.x = Mathf.MoveTowards(base.transform.position.x, target, this.properties.movementSpeed * CupheadTime.Delta);
			base.transform.position = pos;
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		AudioManager.Play("clown_penguin_roll_end");
		this.emitAudioFromObject.Add("clown_penguin_roll_end");
		this.damageReceiver.enabled = true;
		base.StartCoroutine(this.shoot_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x000EEAAC File Offset: 0x000ECEAC
	private IEnumerator shoot_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.initialAttackDelay);
		for (;;)
		{
			base.animator.SetTrigger("OnAttack");
			yield return CupheadTime.WaitForSeconds(this, this.properties.attackDelayRange.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A22 RID: 6690 RVA: 0x000EEAC8 File Offset: 0x000ECEC8
	private void ShootBullet()
	{
		AudioManager.Play("clown_penguin_clap");
		this.emitAudioFromObject.Add("clown_penguin_clap");
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 v = next.transform.position - base.transform.position;
		this.bullet.Create(this.root.transform.position, MathUtils.DirectionToAngle(v), this.properties.bulletSpeed);
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x000EEB48 File Offset: 0x000ECF48
	protected override void Die()
	{
		AudioManager.Play("clown_penguin_death");
		this.emitAudioFromObject.Add("clown_penguin_death");
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		ClownLevelClownSwing clownLevelClownSwing = this.parent;
		clownLevelClownSwing.OnDeath = (Action)Delegate.Remove(clownLevelClownSwing.OnDeath, new Action(this.Die));
		base.Die();
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x000EEBC0 File Offset: 0x000ECFC0
	private void SwitchLayer()
	{
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Projectiles.ToString();
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x000EEBE7 File Offset: 0x000ECFE7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.bullet = null;
	}

	// Token: 0x0400233C RID: 9020
	[SerializeField]
	private BasicProjectile bullet;

	// Token: 0x0400233D RID: 9021
	[SerializeField]
	private Transform root;

	// Token: 0x0400233E RID: 9022
	private LevelProperties.Clown.Swing properties;

	// Token: 0x0400233F RID: 9023
	private ClownLevelClownSwing parent;

	// Token: 0x04002340 RID: 9024
	private ClownLevelCoasterHandler handler;

	// Token: 0x04002341 RID: 9025
	private DamageReceiver damageReceiver;

	// Token: 0x04002342 RID: 9026
	private float targetPosition;

	// Token: 0x04002343 RID: 9027
	private float HP;

	// Token: 0x04002344 RID: 9028
	private bool isDead;
}
