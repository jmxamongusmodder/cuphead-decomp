using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000646 RID: 1606
public class FlyingBlimpLevelUFO : AbstractCollidableObject
{
	// Token: 0x060020F4 RID: 8436 RVA: 0x001305DC File Offset: 0x0012E9DC
	protected override void Awake()
	{
		base.Awake();
		this.beamTriggered = false;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
		this.collisionChild = this.beamPrefab.GetComponent<CollisionChild>();
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		float num = (float)UnityEngine.Random.Range(0, 2);
		if (num == 0f)
		{
			this.beamPrefab.SetScale(new float?(-base.transform.localScale.x), new float?(base.transform.localScale.y), new float?(base.transform.localScale.z));
		}
	}

	// Token: 0x060020F5 RID: 8437 RVA: 0x001306B5 File Offset: 0x0012EAB5
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x001306CD File Offset: 0x0012EACD
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x001306F8 File Offset: 0x0012EAF8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060020F8 RID: 8440 RVA: 0x00130718 File Offset: 0x0012EB18
	public void Init(Vector2 startPos, Vector2 midPos, Vector2 endPos, float speed, float health, LevelProperties.FlyingBlimp.UFO properties)
	{
		base.transform.position = startPos;
		this.ufoMidPoint = midPos;
		this.ufoStopPoint = endPos;
		this.speed = speed;
		this.properties = properties;
		this.health = health;
		base.StartCoroutine(this.to_position_cr());
	}

	// Token: 0x060020F9 RID: 8441 RVA: 0x00130774 File Offset: 0x0012EB74
	private IEnumerator to_position_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position != this.ufoMidPoint)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.ufoMidPoint, this.speed * CupheadTime.FixedDelta);
			yield return wait;
		}
		base.transform.GetComponent<SpriteRenderer>().sortingOrder = 3;
		while (base.transform.position != this.ufoStopPoint)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.ufoStopPoint, this.speed * CupheadTime.FixedDelta);
			yield return wait;
		}
		base.StartCoroutine(this.move_cr());
		yield break;
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x00130790 File Offset: 0x0012EB90
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float offset = 50f;
		while (base.transform.position.x > -640f - offset)
		{
			this.player = PlayerManager.GetNext();
			float dist = this.player.transform.position.x - base.transform.position.x;
			Vector3 pos = base.transform.position;
			pos.x += -this.speed * CupheadTime.FixedDelta;
			base.transform.position = pos;
			this.proximity = ((!this.typeB) ? this.properties.UFOProximityA : this.properties.UFOProximityB);
			if (dist > -this.proximity && dist < this.proximity && !this.beamTriggered)
			{
				this.beamTriggered = true;
				base.StartCoroutine(this.ActivateBeam());
			}
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x001307AC File Offset: 0x0012EBAC
	private IEnumerator ActivateBeam()
	{
		base.animator.SetTrigger("StartBeam");
		yield return CupheadTime.WaitForSeconds(this, this.properties.UFOWarningBeamDuration);
		AudioManager.Play("level_flying_blimp_moon_UFO_fire_laser");
		base.animator.SetTrigger("Continue");
		yield return CupheadTime.WaitForSeconds(this, this.properties.beamDuration);
		base.animator.SetTrigger("End");
		yield break;
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x001307C7 File Offset: 0x0012EBC7
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400298B RID: 10635
	public bool typeB;

	// Token: 0x0400298C RID: 10636
	[SerializeField]
	private Transform beamPrefab;

	// Token: 0x0400298D RID: 10637
	private DamageDealer damageDealer;

	// Token: 0x0400298E RID: 10638
	private DamageReceiver damageReceiver;

	// Token: 0x0400298F RID: 10639
	private CollisionChild collisionChild;

	// Token: 0x04002990 RID: 10640
	private Vector3 ufoMidPoint;

	// Token: 0x04002991 RID: 10641
	private Vector3 ufoStopPoint;

	// Token: 0x04002992 RID: 10642
	private AbstractPlayerController player;

	// Token: 0x04002993 RID: 10643
	private LevelProperties.FlyingBlimp.UFO properties;

	// Token: 0x04002994 RID: 10644
	private float speed;

	// Token: 0x04002995 RID: 10645
	private float health;

	// Token: 0x04002996 RID: 10646
	private float proximity;

	// Token: 0x04002997 RID: 10647
	private bool beamTriggered;
}
