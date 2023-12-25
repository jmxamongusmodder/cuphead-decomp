using System;
using UnityEngine;

// Token: 0x0200083D RID: 2109
public class VeggiesLevelBeetBaby : AbstractCollidableObject
{
	// Token: 0x060030E0 RID: 12512 RVA: 0x001CBCB4 File Offset: 0x001CA0B4
	public VeggiesLevelBeetBaby Create(VeggiesLevelBeetBaby.Type type, float speed, float childSpeed, float range, Vector2 pos, float rot)
	{
		VeggiesLevelBeetBaby veggiesLevelBeetBaby = this.InstantiatePrefab<VeggiesLevelBeetBaby>();
		veggiesLevelBeetBaby.Init(type, speed, childSpeed, range, pos, rot);
		return veggiesLevelBeetBaby;
	}

	// Token: 0x060030E1 RID: 12513 RVA: 0x001CBCD8 File Offset: 0x001CA0D8
	private void Init(VeggiesLevelBeetBaby.Type type, float speed, float childSpeed, float range, Vector2 pos, float rot)
	{
		this.type = type;
		this.speed = speed;
		this.childSpeed = childSpeed;
		this.range = range;
		base.transform.position = pos;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rot));
		base.animator.Play(type.ToString());
		this.damageDealer = new DamageDealer(1f, 0.2f, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
	}

	// Token: 0x060030E2 RID: 12514 RVA: 0x001CBD7C File Offset: 0x001CA17C
	private void Update()
	{
		if (this.state == VeggiesLevelBeetBaby.State.Dead)
		{
			return;
		}
		base.transform.position += base.transform.right * this.speed * CupheadTime.Delta;
		if (base.transform.position.y > 360f)
		{
			this.Die();
		}
	}

	// Token: 0x060030E3 RID: 12515 RVA: 0x001CBDF4 File Offset: 0x001CA1F4
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x001CBE0C File Offset: 0x001CA20C
	private void Die()
	{
		this.state = VeggiesLevelBeetBaby.State.Dead;
		base.animator.SetTrigger("Explode");
		int num = (this.type != VeggiesLevelBeetBaby.Type.Fat) ? 3 : 5;
		for (int i = 0; i < num; i++)
		{
			float t = (float)i / (float)(num - 1);
			float rot = Mathf.Lerp(0f, this.range, t) - 90f - this.range / 2f;
			VeggiesLevelBeetBabyBullet veggiesLevelBeetBabyBullet = this.bulletPrefab.Create(this.childSpeed, base.transform.position, rot);
			if (this.type == VeggiesLevelBeetBaby.Type.Pink)
			{
				veggiesLevelBeetBabyBullet.SetParryable(true);
			}
		}
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x001CBEBB File Offset: 0x001CA2BB
	private void OnDeathAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04003981 RID: 14721
	private const int BULLET_COUNT = 3;

	// Token: 0x04003982 RID: 14722
	private const int BULLET_COUNT_FAT = 5;

	// Token: 0x04003983 RID: 14723
	[SerializeField]
	private VeggiesLevelBeetBabyBullet bulletPrefab;

	// Token: 0x04003984 RID: 14724
	private VeggiesLevelBeetBaby.Type type;

	// Token: 0x04003985 RID: 14725
	private float speed;

	// Token: 0x04003986 RID: 14726
	private float childSpeed;

	// Token: 0x04003987 RID: 14727
	private float range;

	// Token: 0x04003988 RID: 14728
	private VeggiesLevelBeetBaby.State state;

	// Token: 0x04003989 RID: 14729
	private DamageDealer damageDealer;

	// Token: 0x0200083E RID: 2110
	public enum Type
	{
		// Token: 0x0400398B RID: 14731
		Regular,
		// Token: 0x0400398C RID: 14732
		Fat,
		// Token: 0x0400398D RID: 14733
		Pink
	}

	// Token: 0x0200083F RID: 2111
	public enum State
	{
		// Token: 0x0400398F RID: 14735
		Go,
		// Token: 0x04003990 RID: 14736
		Dead
	}
}
