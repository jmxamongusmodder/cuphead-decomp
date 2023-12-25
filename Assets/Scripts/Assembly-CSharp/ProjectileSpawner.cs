using System;
using UnityEngine;

// Token: 0x02000AF7 RID: 2807
public class ProjectileSpawner : AbstractPausableComponent
{
	// Token: 0x0600440C RID: 17420 RVA: 0x00240A38 File Offset: 0x0023EE38
	private void Start()
	{
		Level.Current.OnLevelStartEvent += this.OnStart;
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
	}

	// Token: 0x0600440D RID: 17421 RVA: 0x00240A8C File Offset: 0x0023EE8C
	public void OnStart()
	{
		this.started = true;
	}

	// Token: 0x0600440E RID: 17422 RVA: 0x00240A95 File Offset: 0x0023EE95
	public void OnStop()
	{
		this.started = false;
	}

	// Token: 0x0600440F RID: 17423 RVA: 0x00240AA0 File Offset: 0x0023EEA0
	private void Update()
	{
		if (!this.started)
		{
			return;
		}
		if (Level.Current == null || PlayerManager.Count < 1)
		{
			return;
		}
		if (this.projectilePrefab == null)
		{
			return;
		}
		if (this.timer >= this.delay)
		{
			if (this.type == ProjectileSpawner.Type.Aimed)
			{
				this.aim.LookAt2D(PlayerManager.GetNext().transform);
				this.angle = this.aim.transform.eulerAngles.z;
			}
			BasicProjectile basicProjectile = this.projectilePrefab.Create(base.transform.position, this.angle, this.speed);
			if (this.parryable)
			{
				basicProjectile.SetParryable(this.parryable);
			}
			if (this.stoneTime > 0f)
			{
				basicProjectile.SetStoneTime(this.stoneTime);
			}
			this.timer = 0f;
		}
		else
		{
			this.timer += CupheadTime.Delta;
		}
	}

	// Token: 0x040049AB RID: 18859
	public ProjectileSpawner.Type type;

	// Token: 0x040049AC RID: 18860
	public float delay = 1f;

	// Token: 0x040049AD RID: 18861
	public float speed = 500f;

	// Token: 0x040049AE RID: 18862
	public bool parryable;

	// Token: 0x040049AF RID: 18863
	[Space(10f)]
	public float stoneTime;

	// Token: 0x040049B0 RID: 18864
	[Space(10f)]
	[SerializeField]
	private BasicProjectile projectilePrefab;

	// Token: 0x040049B1 RID: 18865
	public float angle;

	// Token: 0x040049B2 RID: 18866
	private float timer;

	// Token: 0x040049B3 RID: 18867
	private bool started;

	// Token: 0x040049B4 RID: 18868
	private Transform aim;

	// Token: 0x02000AF8 RID: 2808
	public enum Type
	{
		// Token: 0x040049B6 RID: 18870
		Straight,
		// Token: 0x040049B7 RID: 18871
		Aimed
	}
}
