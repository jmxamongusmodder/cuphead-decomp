using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200061D RID: 1565
public class FlyingBirdLevelEnemyProjectile : AbstractProjectile
{
	// Token: 0x06001FD7 RID: 8151 RVA: 0x00124380 File Offset: 0x00122780
	public virtual AbstractProjectile Create(float time, float height, Vector2 pos)
	{
		FlyingBirdLevelEnemyProjectile flyingBirdLevelEnemyProjectile = this.Create(pos, 0f) as FlyingBirdLevelEnemyProjectile;
		flyingBirdLevelEnemyProjectile.time = time;
		flyingBirdLevelEnemyProjectile.height = height;
		flyingBirdLevelEnemyProjectile.DamagesType.OnlyPlayer();
		flyingBirdLevelEnemyProjectile.CollisionDeath.OnlyPlayer();
		flyingBirdLevelEnemyProjectile.Init();
		return flyingBirdLevelEnemyProjectile;
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x001243CB File Offset: 0x001227CB
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x001243E9 File Offset: 0x001227E9
	private void Init()
	{
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001FDA RID: 8154 RVA: 0x001243F8 File Offset: 0x001227F8
	private void Check()
	{
		if (base.transform.position.y < -460f)
		{
			this.StopAllCoroutines();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x00124434 File Offset: 0x00122834
	private IEnumerator go_cr()
	{
		float start = base.transform.position.y;
		float end = start + this.height;
		float t = 0f;
		float speed = 0f;
		t = 0f;
		while (t < this.time)
		{
			float val = t / this.time;
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		while (t < this.time)
		{
			float val2 = t / this.time;
			float last = base.transform.position.y;
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, end, start, val2)), null);
			speed = base.transform.position.y - last;
			t += CupheadTime.Delta;
			yield return null;
		}
		for (;;)
		{
			this.Check();
			base.transform.AddPosition(0f, speed * CupheadTime.GlobalSpeed, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002855 RID: 10325
	private float time;

	// Token: 0x04002856 RID: 10326
	private float height;
}
