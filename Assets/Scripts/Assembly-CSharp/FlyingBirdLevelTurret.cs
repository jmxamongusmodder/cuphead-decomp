using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200062B RID: 1579
public class FlyingBirdLevelTurret : AbstractCollidableObject
{
	// Token: 0x0600202A RID: 8234 RVA: 0x00127B44 File Offset: 0x00125F44
	public FlyingBirdLevelTurret Create(Vector2 pos, FlyingBirdLevelTurret.Properties properties)
	{
		FlyingBirdLevelTurret flyingBirdLevelTurret = this.InstantiatePrefab<FlyingBirdLevelTurret>();
		flyingBirdLevelTurret.transform.position = pos;
		flyingBirdLevelTurret.properties = properties;
		flyingBirdLevelTurret.Init();
		return flyingBirdLevelTurret;
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x0600202B RID: 8235 RVA: 0x00127B77 File Offset: 0x00125F77
	// (set) Token: 0x0600202C RID: 8236 RVA: 0x00127B7F File Offset: 0x00125F7F
	public FlyingBirdLevelTurret.State state { get; private set; }

	// Token: 0x0600202D RID: 8237 RVA: 0x00127B88 File Offset: 0x00125F88
	protected override void Awake()
	{
		base.Awake();
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
	}

	// Token: 0x0600202E RID: 8238 RVA: 0x00127BC1 File Offset: 0x00125FC1
	private void Init()
	{
		this.startPos = base.transform.position;
		base.StartCoroutine(this.go_cr());
		base.StartCoroutine(this.y_cr());
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x00127BF4 File Offset: 0x00125FF4
	private void Shoot()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		if (next == null || next.transform == null)
		{
			return;
		}
		this.aim.LookAt2D(next.transform);
		BasicProjectile basicProjectile = this.childPrefab.Create(base.transform.position, this.aim.transform.eulerAngles.z, this.properties.bulletSpeed);
		basicProjectile.CollisionDeath.OnlyPlayer();
		basicProjectile.DamagesType.OnlyPlayer();
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x00127C8C File Offset: 0x0012608C
	private IEnumerator y_cr()
	{
		float start = this.startPos.y + this.properties.floatRange / 2f;
		float end = this.startPos.y - this.properties.floatRange / 2f;
		base.transform.SetPosition(null, new float?(start), null);
		float t = 0f;
		for (;;)
		{
			t = 0f;
			while (t < this.properties.floatTime)
			{
				float val = t / this.properties.floatTime;
				base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			while (t < this.properties.floatTime)
			{
				float val2 = t / this.properties.floatTime;
				base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, end, start, val2)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x00127CA8 File Offset: 0x001260A8
	private IEnumerator go_cr()
	{
		float t = 0f;
		while (t < this.properties.inTime)
		{
			float val = t / this.properties.inTime;
			base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, this.startPos.x, this.properties.x, val)), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(new float?(this.properties.x), null, null);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.bulletDelay);
			this.Shoot();
		}
		yield break;
	}

	// Token: 0x040028A4 RID: 10404
	[SerializeField]
	private BasicProjectile childPrefab;

	// Token: 0x040028A5 RID: 10405
	private Vector2 startPos;

	// Token: 0x040028A6 RID: 10406
	private FlyingBirdLevelTurret.Properties properties;

	// Token: 0x040028A7 RID: 10407
	private Transform aim;

	// Token: 0x0200062C RID: 1580
	public enum State
	{
		// Token: 0x040028A9 RID: 10409
		Alive,
		// Token: 0x040028AA RID: 10410
		Dying,
		// Token: 0x040028AB RID: 10411
		Dead,
		// Token: 0x040028AC RID: 10412
		Respawn
	}

	// Token: 0x0200062D RID: 1581
	public class Properties
	{
		// Token: 0x06002032 RID: 8242 RVA: 0x00127CC3 File Offset: 0x001260C3
		public Properties(float health, float inTime, float x, float bulletSpeed, float bulletDelay, float floatRange, float floatTime)
		{
			this.health = health;
			this.inTime = inTime;
			this.x = x;
			this.bulletSpeed = bulletSpeed;
			this.bulletDelay = bulletDelay;
			this.floatRange = floatRange;
			this.floatTime = floatTime;
		}

		// Token: 0x040028AD RID: 10413
		public readonly float health;

		// Token: 0x040028AE RID: 10414
		public readonly float inTime;

		// Token: 0x040028AF RID: 10415
		public readonly float x;

		// Token: 0x040028B0 RID: 10416
		public readonly float bulletSpeed;

		// Token: 0x040028B1 RID: 10417
		public readonly float bulletDelay;

		// Token: 0x040028B2 RID: 10418
		public readonly float floatRange;

		// Token: 0x040028B3 RID: 10419
		public readonly float floatTime;
	}
}
