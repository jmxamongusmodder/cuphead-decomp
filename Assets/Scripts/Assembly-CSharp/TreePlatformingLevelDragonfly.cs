using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200088A RID: 2186
public class TreePlatformingLevelDragonfly : PlatformingLevelBigEnemy
{
	// Token: 0x060032D3 RID: 13011 RVA: 0x001D8838 File Offset: 0x001D6C38
	protected override void Start()
	{
		base.Start();
		this.LockDistance = 1550f;
		this.startPos = base.transform.position;
		this.aimIndex = UnityEngine.Random.Range(0, base.Properties.dragonFlyAimString.Split(new char[]
		{
			','
		}).Length);
		this.delayIndex = UnityEngine.Random.Range(0, base.Properties.dragonFlyAtkDelayString.Split(new char[]
		{
			','
		}).Length);
		this.LockDistance -= base.Properties.dragonFlyLockDistOffset;
		this.mosquitos = new List<TreePlatformingLevelMosquito>(this.platforms.GetComponentsInChildren<TreePlatformingLevelMosquito>());
		this.currentMosquitos = this.randomizeList(this.mosquitos);
		base.StartCoroutine(this.enter_cr());
	}

	// Token: 0x060032D4 RID: 13012 RVA: 0x001D8906 File Offset: 0x001D6D06
	protected override void Shoot()
	{
		if (!this.isShooting)
		{
			base.StartCoroutine(this.shoot_cr());
		}
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x001D8920 File Offset: 0x001D6D20
	private IEnumerator enter_cr()
	{
		base.transform.position = new Vector3(this.startPos.x + 800f, this.startPos.y);
		while (!this.bigEnemyCameraLock)
		{
			yield return null;
		}
		float t = 0f;
		float time = base.Properties.dragonFlyInitRiseTime;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(base.transform.position, this.startPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.startPos;
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.sine_cr());
		yield break;
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x001D893C File Offset: 0x001D6D3C
	private IEnumerator shoot_cr()
	{
		float t = 0f;
		float t2 = 0f;
		float angle = 0f;
		bool pickDir = false;
		Vector3 direction = Vector3.zero;
		this.isShooting = true;
		base.animator.SetTrigger("Shoot");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning_Start", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.Properties.dragonFlyWarningDuration);
		base.animator.SetTrigger("Continue");
		while (t < base.Properties.dragonFlyAttackDuration)
		{
			pickDir = false;
			while (t2 < base.Properties.dragonFlyProjectileDelay)
			{
				t2 += CupheadTime.Delta;
				t += CupheadTime.Delta;
				yield return null;
			}
			t2 = 0f;
			if (base.Properties.dragonFlyAimString.Split(new char[]
			{
				','
			})[this.aimIndex][0] == 'R')
			{
				while (!pickDir)
				{
					if (this.currentMosquitos[this.cycleIndex].isActive)
					{
						direction = this.currentMosquitos[this.cycleIndex].transform.position - base.transform.position;
						this.currentMosquitos.RemoveAt(this.cycleIndex);
						if (this.currentMosquitos.Count > 0)
						{
							this.cycleIndex = (this.cycleIndex + 1) % this.currentMosquitos.Count;
						}
						else
						{
							this.currentMosquitos = this.randomizeList(this.mosquitos);
						}
						pickDir = true;
					}
					else
					{
						this.cycleIndex = (this.cycleIndex + 1) % this.currentMosquitos.Count;
						this.currentMosquitos = this.randomizeList(this.mosquitos);
					}
					yield return null;
				}
			}
			else if (base.Properties.dragonFlyAimString.Split(new char[]
			{
				','
			})[this.aimIndex][0] == 'P' && this._target.transform.position.x < base.transform.position.x)
			{
				direction = this._target.transform.position - base.transform.position;
			}
			angle = MathUtils.DirectionToAngle(direction);
			this.projectile.Create(this.projectileRoot.transform.position, angle + 5f, base.Properties.dragonFlyProjectileSpeed);
			this.aimIndex = (this.aimIndex + 1) % base.Properties.dragonFlyAimString.Split(new char[]
			{
				','
			}).Length;
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Attack_To_Idle", false, true);
		yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.Properties.dragonFlyAtkDelayString.Split(new char[]
		{
			','
		})[this.delayIndex]));
		this.delayIndex = (this.delayIndex + 1) % base.Properties.dragonFlyAtkDelayString.Split(new char[]
		{
			','
		}).Length;
		this.isShooting = false;
		yield return null;
		yield break;
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x001D8958 File Offset: 0x001D6D58
	private List<TreePlatformingLevelMosquito> randomizeList(List<TreePlatformingLevelMosquito> platforms)
	{
		List<TreePlatformingLevelMosquito> list = new List<TreePlatformingLevelMosquito>();
		List<TreePlatformingLevelMosquito> list2 = new List<TreePlatformingLevelMosquito>();
		list2.AddRange(platforms);
		for (int i = 0; i < platforms.Count; i++)
		{
			int index = UnityEngine.Random.Range(0, list2.Count);
			list.Add(list2[index]);
			list2.RemoveAt(index);
		}
		this.cycleIndex = 0;
		return list;
	}

	// Token: 0x060032D8 RID: 13016 RVA: 0x001D89B8 File Offset: 0x001D6DB8
	public IEnumerator sine_cr()
	{
		float time = 0.5f;
		float t = 0f;
		float val = 1f;
		for (;;)
		{
			if (!this.isShooting && CupheadTime.Delta != 0f)
			{
				t += CupheadTime.Delta;
				float num = Mathf.Sin(t / time);
				base.transform.AddPosition(0f, num * val, 0f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060032D9 RID: 13017 RVA: 0x001D89D4 File Offset: 0x001D6DD4
	protected override void Die()
	{
		if (!this.isDead)
		{
			this.StopAllCoroutines();
			this.isDead = true;
			base.GetComponent<Collider2D>().enabled = false;
			base.animator.Play("Death");
			AudioManager.Play("level_platform_dragonfly_death");
			this.emitAudioFromObject.Add("level_platform_dragonfly_death");
			this.explosion.StartExplosion();
			base.StartCoroutine(this.fall_cr());
		}
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x001D8A48 File Offset: 0x001D6E48
	private IEnumerator fall_cr()
	{
		float velocity = 0f;
		float gravity = 1500f;
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.explosion.StopExplosions();
		while (base.transform.position.y > -CupheadLevelCamera.Current.Height - 200f)
		{
			base.transform.AddPosition(0f, velocity * CupheadTime.Delta, 0f);
			velocity -= gravity * CupheadTime.Delta;
			yield return null;
		}
		base.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060032DB RID: 13019 RVA: 0x001D8A63 File Offset: 0x001D6E63
	private void SoundDragonflyAttackWarning()
	{
		AudioManager.Play("level_platform_dragonfly_attack_warning");
		this.emitAudioFromObject.Add("level_platform_dragonfly_attack_warning");
	}

	// Token: 0x060032DC RID: 13020 RVA: 0x001D8A7F File Offset: 0x001D6E7F
	private void SoundDragonflyAttackStart()
	{
		AudioManager.Play("level_platform_dragonfly_attack_start");
		this.emitAudioFromObject.Add("level_platform_dragonfly_attack_start");
	}

	// Token: 0x04003B02 RID: 15106
	[SerializeField]
	private LevelBossDeathExploder explosion;

	// Token: 0x04003B03 RID: 15107
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04003B04 RID: 15108
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04003B05 RID: 15109
	public GameObject platforms;

	// Token: 0x04003B06 RID: 15110
	private List<TreePlatformingLevelMosquito> mosquitos;

	// Token: 0x04003B07 RID: 15111
	private List<TreePlatformingLevelMosquito> currentMosquitos;

	// Token: 0x04003B08 RID: 15112
	private Vector3 startPos;

	// Token: 0x04003B09 RID: 15113
	private int delayIndex;

	// Token: 0x04003B0A RID: 15114
	private int aimIndex;

	// Token: 0x04003B0B RID: 15115
	private int cycleIndex;

	// Token: 0x04003B0C RID: 15116
	private bool isShooting;
}
