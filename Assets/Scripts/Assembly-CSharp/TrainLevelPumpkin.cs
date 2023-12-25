using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000826 RID: 2086
public class TrainLevelPumpkin : AbstractCollidableObject
{
	// Token: 0x0600306C RID: 12396 RVA: 0x001C8870 File Offset: 0x001C6C70
	public void Create(Vector2 pos, int direction, float speed, float health, float fallTime, Transform target)
	{
		TrainLevelPumpkin trainLevelPumpkin = this.InstantiatePrefab<TrainLevelPumpkin>();
		trainLevelPumpkin.transform.position = pos;
		trainLevelPumpkin.transform.SetScale(new float?((float)(-(float)direction)), new float?(1f), new float?(1f));
		trainLevelPumpkin.direction = direction;
		trainLevelPumpkin.health = health;
		trainLevelPumpkin.speed = speed;
		trainLevelPumpkin.target = target;
		trainLevelPumpkin.fallTime = fallTime;
	}

	// Token: 0x0600306D RID: 12397 RVA: 0x001C88E4 File Offset: 0x001C6CE4
	private void Start()
	{
		base.StartCoroutine(this.x_cr());
		base.StartCoroutine(this.drop_cr());
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.brick = (this.brickPrefab.Create() as TrainLevelPumpkinProjectile);
		this.brick.fallTime = this.fallTime;
		this.brick.transform.SetParent(base.transform);
		this.brick.transform.ResetLocalTransforms();
	}

	// Token: 0x0600306E RID: 12398 RVA: 0x001C897B File Offset: 0x001C6D7B
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x001C89A6 File Offset: 0x001C6DA6
	private void Drop()
	{
		if (this.brick != null)
		{
			this.brick.Drop();
			this.brick = null;
			base.StartCoroutine(this.y_cr());
		}
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x001C89D8 File Offset: 0x001C6DD8
	private void Die()
	{
		this.StopAllCoroutines();
		this.Drop();
		base.animator.Play("Die");
		AudioManager.Play("train_pumpkin_die");
		this.emitAudioFromObject.Add("train_pumpkin_die");
	}

	// Token: 0x06003071 RID: 12401 RVA: 0x001C8A10 File Offset: 0x001C6E10
	private void OnDeathAnimComplete()
	{
		this.End();
	}

	// Token: 0x06003072 RID: 12402 RVA: 0x001C8A18 File Offset: 0x001C6E18
	private void End()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003073 RID: 12403 RVA: 0x001C8A2C File Offset: 0x001C6E2C
	private IEnumerator x_cr()
	{
		for (;;)
		{
			base.transform.AddPosition(this.speed * CupheadTime.Delta * (float)this.direction, 0f, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003074 RID: 12404 RVA: 0x001C8A48 File Offset: 0x001C6E48
	private IEnumerator y_cr()
	{
		float ySpeed = 1f;
		float mult = 1.1f;
		for (;;)
		{
			base.transform.AddPosition(0f, ySpeed * CupheadTime.Delta, 0f);
			if (CupheadTime.Delta != 0f)
			{
				ySpeed *= mult;
			}
			yield return null;
			if (base.transform.position.y > 720f)
			{
				this.End();
			}
		}
		yield break;
	}

	// Token: 0x06003075 RID: 12405 RVA: 0x001C8A64 File Offset: 0x001C6E64
	private IEnumerator drop_cr()
	{
		bool check = true;
		while (check)
		{
			if (this.direction > 0 && base.transform.position.x > this.target.position.x)
			{
				check = false;
				this.Drop();
			}
			if (this.direction < 0 && base.transform.position.x < this.target.position.x)
			{
				check = false;
				this.Drop();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003076 RID: 12406 RVA: 0x001C8A7F File Offset: 0x001C6E7F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.brickPrefab = null;
	}

	// Token: 0x0400391D RID: 14621
	[SerializeField]
	private TrainLevelPumpkinProjectile brickPrefab;

	// Token: 0x0400391E RID: 14622
	private TrainLevelPumpkinProjectile brick;

	// Token: 0x0400391F RID: 14623
	private int direction;

	// Token: 0x04003920 RID: 14624
	private float speed;

	// Token: 0x04003921 RID: 14625
	private float health;

	// Token: 0x04003922 RID: 14626
	private float fallTime;

	// Token: 0x04003923 RID: 14627
	private Transform target;

	// Token: 0x04003924 RID: 14628
	private DamageReceiver damageReceiver;
}
