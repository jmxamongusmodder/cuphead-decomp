using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008F2 RID: 2290
public class MountainPlatformingLevelWall : AbstractPlatformingLevelEnemy
{
	// Token: 0x060035AC RID: 13740 RVA: 0x001F49D5 File Offset: 0x001F2DD5
	protected override void OnStart()
	{
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x001F49D8 File Offset: 0x001F2DD8
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
		base.GetComponent<Collider2D>().enabled = false;
		this.head.GetComponent<Collider2D>().enabled = false;
		this.platform.gameObject.SetActive(false);
		base.GetComponent<DamageReceiver>().enabled = false;
		this.head.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.head.gameObject.tag = "Enemy";
		ParrySwitch component = this.head.GetComponent<ParrySwitch>();
		component.OnActivate += component.StartParryCooldown;
	}

	// Token: 0x060035AE RID: 13742 RVA: 0x001F4A82 File Offset: 0x001F2E82
	private void FaceOn()
	{
		base.animator.Play("Face_Idle");
		base.animator.Play("Shield_Idle");
	}

	// Token: 0x060035AF RID: 13743 RVA: 0x001F4AA4 File Offset: 0x001F2EA4
	private IEnumerator move_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AbstractPlayerController player = PlayerManager.GetNext();
		while (player.transform.position.x < this.startTrigger.transform.position.x)
		{
			yield return null;
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
		}
		base.GetComponent<Collider2D>().enabled = true;
		this.head.GetComponent<Collider2D>().enabled = true;
		this.platform.gameObject.SetActive(true);
		base.animator.SetTrigger("OnIntro");
		yield return base.animator.WaitForAnimationToEnd(this, "Wall_Intro", false, true);
		base.StartCoroutine(this.shoot_cr());
		float t = 0f;
		float time = base.Properties.wallFaceTravelTime;
		bool movingUp = false;
		float top = this.head.transform.position.y + 100f;
		float bottom = this.head.transform.position.y - 100f;
		float start = this.head.transform.position.y;
		float end = 0f;
		for (;;)
		{
			start = this.head.transform.position.y;
			if (movingUp)
			{
				end = top;
			}
			else
			{
				end = bottom;
			}
			while (t < time)
			{
				float val = t / time;
				this.head.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			this.head.transform.SetPosition(null, new float?(end), null);
			movingUp = !movingUp;
			t = 0f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060035B0 RID: 13744 RVA: 0x001F4AC0 File Offset: 0x001F2EC0
	private IEnumerator shoot_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.Properties.wallAttackDelay.RandomFloat());
			base.animator.SetTrigger("Attack");
			yield return null;
		}
		yield break;
	}

	// Token: 0x060035B1 RID: 13745 RVA: 0x001F4ADC File Offset: 0x001F2EDC
	private void ShootProjectileEffect()
	{
		if (this.projectileCount == 2)
		{
			this.projectilePinkEffect.Create(new Vector3(this.projectileRoot.transform.position.x - 20f, this.projectileRoot.transform.position.y));
		}
		else
		{
			this.projectileEffect.Create(new Vector3(this.projectileRoot.transform.position.x - 20f, this.projectileRoot.transform.position.y));
		}
	}

	// Token: 0x060035B2 RID: 13746 RVA: 0x001F4B88 File Offset: 0x001F2F88
	private void ShootProjectile()
	{
		if (this.projectileCount == 2)
		{
			this.projectileCount = 0;
			this.bouncyPinkProjectile.Create(this.projectileRoot.position, 0f, new Vector2(-base.Properties.wallProjectileXSpeed, base.Properties.wallProjectileYSpeed), base.Properties.wallProjectileGravity, this.groundPosY.position.y);
		}
		else
		{
			this.projectileCount++;
			this.bouncyProjectile.Create(this.projectileRoot.position, 0f, new Vector2(-base.Properties.wallProjectileXSpeed, base.Properties.wallProjectileYSpeed), base.Properties.wallProjectileGravity, this.groundPosY.position.y);
		}
	}

	// Token: 0x060035B3 RID: 13747 RVA: 0x001F4C74 File Offset: 0x001F3074
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawLine(this.startTrigger.transform.position, new Vector3(this.startTrigger.transform.position.x, 5000f, 0f));
	}

	// Token: 0x060035B4 RID: 13748 RVA: 0x001F4CE4 File Offset: 0x001F30E4
	protected override void Die()
	{
		this.StopAllCoroutines();
		this.head.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
		base.StartCoroutine(this.dying_cr());
		base.StartCoroutine(this.death_shake_cr());
		base.StartCoroutine(this.create_explosions_cr());
	}

	// Token: 0x060035B5 RID: 13749 RVA: 0x001F4D3F File Offset: 0x001F313F
	private void FaceDead()
	{
		base.animator.Play("Face_Death_Loop");
	}

	// Token: 0x060035B6 RID: 13750 RVA: 0x001F4D54 File Offset: 0x001F3154
	private IEnumerator death_shake_cr()
	{
		bool movingUp = false;
		float top = base.transform.position.y + 4f;
		float bottom = base.transform.position.y - 4f;
		float start = base.transform.position.y;
		float end = 0f;
		float t = 0f;
		float time = 0.01f;
		while (!this.isDead)
		{
			start = base.transform.position.y;
			if (movingUp)
			{
				end = top;
			}
			else
			{
				end = bottom;
			}
			while (t < time)
			{
				float val = t / time;
				base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, start, end, val)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			base.transform.SetPosition(null, new float?(end), null);
			movingUp = !movingUp;
			t = 0f;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060035B7 RID: 13751 RVA: 0x001F4D70 File Offset: 0x001F3170
	private IEnumerator create_explosions_cr()
	{
		while (!this.isDead)
		{
			base.GetComponent<EffectRadius>().CreateInRadius();
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.2f, 0.4f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x060035B8 RID: 13752 RVA: 0x001F4D8C File Offset: 0x001F318C
	private IEnumerator dying_cr()
	{
		AudioManager.Play("castle_mountain_wall_death");
		this.emitAudioFromObject.Add("castle_mountain_wall_death");
		yield return base.animator.WaitForAnimationToEnd(this, "Wall_Death", false, true);
		yield return CupheadTime.WaitForSeconds(this, 1.67f);
		float t = 0f;
		float time = 0.65f;
		while (t < time)
		{
			base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			this.head.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			this.shield.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			this.foreground1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			this.foreground2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.isDead = true;
		base.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060035B9 RID: 13753 RVA: 0x001F4DA7 File Offset: 0x001F31A7
	private void SoundMountainWallShoot()
	{
		AudioManager.Play("castle_mountain_wall_attack");
		this.emitAudioFromObject.Add("castle_mountain_wall_attack");
	}

	// Token: 0x060035BA RID: 13754 RVA: 0x001F4DC3 File Offset: 0x001F31C3
	private void SoundMountainWallIntro()
	{
		AudioManager.Play("castle_mountain_wall_spawn");
		this.emitAudioFromObject.Add("castle_mountain_wall_spawn");
	}

	// Token: 0x060035BB RID: 13755 RVA: 0x001F4DDF File Offset: 0x001F31DF
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.projectileEffect = null;
		this.projectilePinkEffect = null;
		this.bouncyPinkProjectile = null;
		this.bouncyProjectile = null;
	}

	// Token: 0x04003DC4 RID: 15812
	[SerializeField]
	private Transform groundPosY;

	// Token: 0x04003DC5 RID: 15813
	[SerializeField]
	private Transform platform;

	// Token: 0x04003DC6 RID: 15814
	[SerializeField]
	private SpriteRenderer foreground1;

	// Token: 0x04003DC7 RID: 15815
	[SerializeField]
	private SpriteRenderer foreground2;

	// Token: 0x04003DC8 RID: 15816
	[SerializeField]
	private SpriteRenderer shield;

	// Token: 0x04003DC9 RID: 15817
	[SerializeField]
	private Transform head;

	// Token: 0x04003DCA RID: 15818
	[SerializeField]
	private Transform startTrigger;

	// Token: 0x04003DCB RID: 15819
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04003DCC RID: 15820
	[SerializeField]
	private Effect projectileEffect;

	// Token: 0x04003DCD RID: 15821
	[SerializeField]
	private Effect projectilePinkEffect;

	// Token: 0x04003DCE RID: 15822
	[SerializeField]
	private MountainPlatformingLevelWallProjectile bouncyProjectile;

	// Token: 0x04003DCF RID: 15823
	[SerializeField]
	private MountainPlatformingLevelWallProjectile bouncyPinkProjectile;

	// Token: 0x04003DD0 RID: 15824
	private int projectileCount;

	// Token: 0x04003DD1 RID: 15825
	private bool isDead;
}
