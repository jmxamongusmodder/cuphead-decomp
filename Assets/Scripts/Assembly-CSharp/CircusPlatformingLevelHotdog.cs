using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008A4 RID: 2212
public class CircusPlatformingLevelHotdog : AbstractPlatformingLevelEnemy
{
	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06003377 RID: 13175 RVA: 0x001DEF62 File Offset: 0x001DD362
	// (set) Token: 0x06003378 RID: 13176 RVA: 0x001DEF6C File Offset: 0x001DD36C
	public bool ProjectilesCanHit
	{
		get
		{
			return this.projectilesCanHit;
		}
		set
		{
			this.projectilesCanHit = value;
			for (int i = 0; i < this.projectileList.Count; i++)
			{
				this.projectileList[i].EnableCollider(this.projectilesCanHit);
			}
			base.animator.Play("Dance");
		}
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x001DEFC3 File Offset: 0x001DD3C3
	protected override void OnStart()
	{
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x001DEFC8 File Offset: 0x001DD3C8
	protected override void Start()
	{
		base.Start();
		this.spawnPattern = this.spawnPatternString.Split(new char[]
		{
			','
		});
		this.condimentPattern = this.condimentPatternString.Split(new char[]
		{
			','
		});
		this.sidePattern = this.sidePatternString.Split(new char[]
		{
			','
		});
		this.shotDelayPattern = this.shotDelayPatternString.Split(new char[]
		{
			','
		});
		this.spawnIndex = UnityEngine.Random.Range(0, this.spawnPattern.Length);
		this.condimentIndex = UnityEngine.Random.Range(0, this.condimentPattern.Length);
		this.sideIndex = UnityEngine.Random.Range(0, this.sidePattern.Length);
		this.shotDelayIndex = UnityEngine.Random.Range(0, this.shotDelayPattern.Length);
		this.currentDelay = Parser.IntParse(this.shotDelayPattern[this.shotDelayIndex]);
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x001DF0B4 File Offset: 0x001DD4B4
	public void ShootProjectile()
	{
		this.currentDelay--;
		if (this.currentDelay <= 0)
		{
			this.shotDelayIndex = (this.shotDelayIndex + 1) % this.shotDelayPattern.Length;
			this.currentDelay = Parser.IntParse(this.shotDelayPattern[this.shotDelayIndex]);
			string a = this.sidePattern[this.sideIndex];
			bool flag = a == "R";
			int num = Parser.IntParse(this.spawnPattern[this.spawnIndex]);
			if (flag)
			{
				num += this.projectilesSpawnPoints.Length / 2;
			}
			AudioManager.Play("circus_hotdog_projectile_shoot");
			this.emitAudioFromObject.Add("circus_hotdog_projectile_shoot");
			CircusPlatformingLevelHotdogProjectile circusPlatformingLevelHotdogProjectile = this.projectilePrefab.Create(this.projectilesSpawnPoints[num].position) as CircusPlatformingLevelHotdogProjectile;
			circusPlatformingLevelHotdogProjectile.Speed = -base.Properties.ProjectileSpeed;
			circusPlatformingLevelHotdogProjectile.SetCondiment(this.condimentPattern[this.condimentIndex]);
			circusPlatformingLevelHotdogProjectile.Side(flag);
			circusPlatformingLevelHotdogProjectile.DestroyDistance = this.projectileDistance;
			this.projectileList.Add(circusPlatformingLevelHotdogProjectile);
			circusPlatformingLevelHotdogProjectile.OnDestroyCallback += this.HotDogProjectileDie;
			circusPlatformingLevelHotdogProjectile.EnableCollider(this.projectilesCanHit);
			this.spawnIndex = (this.spawnIndex + 1) % this.spawnPattern.Length;
			this.condimentIndex = (this.condimentIndex + 1) % this.condimentPattern.Length;
			this.sideIndex = (this.sideIndex + 1) % this.sidePattern.Length;
		}
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x001DF22E File Offset: 0x001DD62E
	private void HotDogProjectileDie(CircusPlatformingLevelHotdogProjectile obj)
	{
		obj.OnDestroyCallback -= this.HotDogProjectileDie;
		this.projectileList.Remove(obj);
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x001DF24F File Offset: 0x001DD64F
	protected override void Die()
	{
		base.animator.SetTrigger("Death");
		base.StartCoroutine(this.Explosion_cr());
		base.GetComponent<BoxCollider2D>().enabled = false;
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x001DF27C File Offset: 0x001DD67C
	private IEnumerator Explosion_cr()
	{
		this.exploder.StartExplosion();
		yield return new WaitForSeconds(2.5f);
		this.exploder.StopExplosions();
		yield break;
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x001DF297 File Offset: 0x001DD697
	public void DeathAnimationEnd()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x001DF2A4 File Offset: 0x001DD6A4
	private void HotDogDanceSFX()
	{
		AudioManager.Play("circus_hotdog_dance");
		this.emitAudioFromObject.Add("circus_hotdog_dance");
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x001DF2C0 File Offset: 0x001DD6C0
	private void HotDogDeathSFX()
	{
		AudioManager.Stop("circus_hotdog_dance");
		AudioManager.Play("circus_hotdog_death");
		this.emitAudioFromObject.Add("circus_hotdog_death");
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x001DF2E6 File Offset: 0x001DD6E6
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.projectilePrefab = null;
	}

	// Token: 0x04003BC4 RID: 15300
	private const string DeathParameterName = "Death";

	// Token: 0x04003BC5 RID: 15301
	private const string Right = "R";

	// Token: 0x04003BC6 RID: 15302
	[SerializeField]
	private Transform[] projectilesSpawnPoints;

	// Token: 0x04003BC7 RID: 15303
	[SerializeField]
	private string spawnPatternString;

	// Token: 0x04003BC8 RID: 15304
	[SerializeField]
	private string condimentPatternString;

	// Token: 0x04003BC9 RID: 15305
	[SerializeField]
	private string sidePatternString;

	// Token: 0x04003BCA RID: 15306
	[SerializeField]
	private string shotDelayPatternString;

	// Token: 0x04003BCB RID: 15307
	[SerializeField]
	private float projectileDistance;

	// Token: 0x04003BCC RID: 15308
	[SerializeField]
	private BasicProjectile projectilePrefab;

	// Token: 0x04003BCD RID: 15309
	[SerializeField]
	private LevelBossDeathExploder exploder;

	// Token: 0x04003BCE RID: 15310
	private string[] spawnPattern;

	// Token: 0x04003BCF RID: 15311
	private string[] condimentPattern;

	// Token: 0x04003BD0 RID: 15312
	private string[] sidePattern;

	// Token: 0x04003BD1 RID: 15313
	private string[] shotDelayPattern;

	// Token: 0x04003BD2 RID: 15314
	private int spawnIndex;

	// Token: 0x04003BD3 RID: 15315
	private int condimentIndex;

	// Token: 0x04003BD4 RID: 15316
	private int sideIndex;

	// Token: 0x04003BD5 RID: 15317
	private int shotDelayIndex;

	// Token: 0x04003BD6 RID: 15318
	private int currentDelay;

	// Token: 0x04003BD7 RID: 15319
	private List<CircusPlatformingLevelHotdogProjectile> projectileList = new List<CircusPlatformingLevelHotdogProjectile>();

	// Token: 0x04003BD8 RID: 15320
	private bool projectilesCanHit;
}
