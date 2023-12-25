using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class TrainLevelSkeleton : LevelProperties.Train.Entity
{
	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06003081 RID: 12417 RVA: 0x001C9160 File Offset: 0x001C7560
	// (remove) Token: 0x06003082 RID: 12418 RVA: 0x001C9198 File Offset: 0x001C7598
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TrainLevelSkeleton.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06003083 RID: 12419 RVA: 0x001C91D0 File Offset: 0x001C75D0
	// (remove) Token: 0x06003084 RID: 12420 RVA: 0x001C9208 File Offset: 0x001C7608
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x06003085 RID: 12421 RVA: 0x001C923E File Offset: 0x001C763E
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = this.head.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06003086 RID: 12422 RVA: 0x001C9270 File Offset: 0x001C7670
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x001C92CE File Offset: 0x001C76CE
	private void Die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x001C92F6 File Offset: 0x001C76F6
	public override void LevelInit(LevelProperties.Train properties)
	{
		base.LevelInit(properties);
		this.health = properties.CurrentState.skeleton.health;
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x001C9315 File Offset: 0x001C7715
	public void StartSkeleton()
	{
		AudioManager.Play("train_passenger_car_explode");
		this.emitAudioFromObject.Add("train_passenger_car_explode");
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x001C933E File Offset: 0x001C773E
	private void In()
	{
		AudioManager.Play("level_train_skeleton_up");
		this.head.In();
		this.leftHand.In();
		this.rightHand.In();
	}

	// Token: 0x0600308B RID: 12427 RVA: 0x001C936B File Offset: 0x001C776B
	private void Out()
	{
		AudioManager.Play("train_skeleton_hand_out");
		this.emitAudioFromObject.Add("train_skeleton_hand_out");
		this.head.Out();
		this.leftHand.Out();
		this.rightHand.Out();
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x001C93A8 File Offset: 0x001C77A8
	private void RandomizeLocations()
	{
		TrainLevelSkeleton.Position position;
		for (position = this.currentPosition; position == this.currentPosition; position = (TrainLevelSkeleton.Position)UnityEngine.Random.Range(0, 3))
		{
		}
		this.currentPosition = position;
		this.head.SetPosition(position);
		switch (position)
		{
		case TrainLevelSkeleton.Position.Right:
			this.leftHand.SetPosition(TrainLevelSkeleton.Position.Left);
			this.rightHand.SetPosition(TrainLevelSkeleton.Position.Center);
			break;
		case TrainLevelSkeleton.Position.Center:
			this.leftHand.SetPosition(TrainLevelSkeleton.Position.Left);
			this.rightHand.SetPosition(TrainLevelSkeleton.Position.Right);
			break;
		default:
			this.leftHand.SetPosition(TrainLevelSkeleton.Position.Center);
			this.rightHand.SetPosition(TrainLevelSkeleton.Position.Right);
			break;
		}
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x001C9458 File Offset: 0x001C7858
	private IEnumerator loop_cr()
	{
		this.currentPosition = TrainLevelSkeleton.Position.Center;
		float attackDelay = 0f;
		Animator handAnimator = this.rightHand.GetComponent<Animator>();
		for (;;)
		{
			attackDelay = Mathf.Lerp(base.properties.CurrentState.skeleton.attackDelay.max, base.properties.CurrentState.skeleton.attackDelay.min, this.health / base.properties.CurrentState.skeleton.health);
			this.In();
			yield return handAnimator.WaitForAnimationToEnd(this, "In", false, true);
			yield return CupheadTime.WaitForSeconds(this, attackDelay);
			AudioManager.Play("train_skeleton_hand_slap");
			this.leftHand.Slap();
			this.rightHand.Slap();
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.skeleton.slapHoldTime);
			this.Out();
			yield return this.head.animator.WaitForAnimationToEnd(this, "Out", false, true);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.skeleton.appearDelay);
			this.RandomizeLocations();
		}
		yield break;
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x001C9474 File Offset: 0x001C7874
	private IEnumerator die_cr()
	{
		Animator handAnimator = this.rightHand.GetComponent<Animator>();
		this.head.Die();
		this.rightHand.Die();
		this.leftHand.Die();
		AudioManager.Play("train_skeleton_hand_death");
		this.emitAudioFromObject.Add("train_skeleton_hand_death");
		yield return handAnimator.WaitForAnimationToEnd(this, "Death", false, true);
		this.head.EndDeath();
		this.rightHand.EndDeath();
		this.leftHand.EndDeath();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		yield break;
	}

	// Token: 0x04003928 RID: 14632
	[SerializeField]
	private TrainLevelSkeletonHead head;

	// Token: 0x04003929 RID: 14633
	[SerializeField]
	private TrainLevelSkeletonHand leftHand;

	// Token: 0x0400392A RID: 14634
	[SerializeField]
	private TrainLevelSkeletonHand rightHand;

	// Token: 0x0400392B RID: 14635
	private DamageReceiver damageReceiver;

	// Token: 0x0400392C RID: 14636
	private float health;

	// Token: 0x0400392D RID: 14637
	private bool dead;

	// Token: 0x0400392E RID: 14638
	private TrainLevelSkeleton.Position currentPosition;

	// Token: 0x02000829 RID: 2089
	public enum Position
	{
		// Token: 0x04003932 RID: 14642
		Right,
		// Token: 0x04003933 RID: 14643
		Center,
		// Token: 0x04003934 RID: 14644
		Left
	}

	// Token: 0x0200082A RID: 2090
	// (Invoke) Token: 0x06003090 RID: 12432
	public delegate void OnDamageTakenHandler(float damage);
}
