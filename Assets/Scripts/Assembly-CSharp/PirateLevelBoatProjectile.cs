using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000720 RID: 1824
public class PirateLevelBoatProjectile : AbstractProjectile
{
	// Token: 0x060027B2 RID: 10162 RVA: 0x001740D8 File Offset: 0x001724D8
	public PirateLevelBoatProjectile Create(Vector2 pos, float speed, float rotationSpeed)
	{
		PirateLevelBoatProjectile pirateLevelBoatProjectile = this.Create() as PirateLevelBoatProjectile;
		pirateLevelBoatProjectile.CollisionDeath.OnlyPlayer();
		pirateLevelBoatProjectile.DamagesType.OnlyPlayer();
		pirateLevelBoatProjectile.Init(pos, speed, rotationSpeed);
		return pirateLevelBoatProjectile;
	}

	// Token: 0x060027B3 RID: 10163 RVA: 0x00174112 File Offset: 0x00172512
	private void Init(Vector2 pos, float speed, float rotationSpeed)
	{
		base.StartCoroutine(this.bullet_cr(pos, speed, rotationSpeed));
	}

	// Token: 0x060027B4 RID: 10164 RVA: 0x00174124 File Offset: 0x00172524
	protected override void Update()
	{
		base.Update();
		this.child.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
	}

	// Token: 0x060027B5 RID: 10165 RVA: 0x00174155 File Offset: 0x00172555
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
			this.StopAllCoroutines();
			base.StartCoroutine(this.die_cr());
		}
	}

	// Token: 0x060027B6 RID: 10166 RVA: 0x00174184 File Offset: 0x00172584
	protected override void Die()
	{
		this.child.SetLocalEulerAngles(new float?(0f), new float?(0f), new float?((float)UnityEngine.Random.Range(0, 360)));
		base.Die();
	}

	// Token: 0x060027B7 RID: 10167 RVA: 0x001741BC File Offset: 0x001725BC
	private void End()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x001741D0 File Offset: 0x001725D0
	private IEnumerator bullet_cr(Vector2 pos, float speed, float rotationSpeed)
	{
		base.transform.position = pos - this.child.localPosition;
		(base.GetComponent<Collider2D>() as CircleCollider2D).offset = this.child.localPosition;
		for (;;)
		{
			if (base.transform.position.x < -1280f)
			{
				this.End();
			}
			base.transform.AddPosition(-speed * CupheadTime.Delta, 0f, 0f);
			base.transform.AddEulerAngles(0f, 0f, -rotationSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060027B9 RID: 10169 RVA: 0x00174200 File Offset: 0x00172600
	private IEnumerator die_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04003074 RID: 12404
	[SerializeField]
	private Transform child;
}
