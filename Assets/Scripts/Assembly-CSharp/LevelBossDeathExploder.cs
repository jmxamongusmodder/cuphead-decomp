using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200049D RID: 1181
public class LevelBossDeathExploder : AbstractMonoBehaviour
{
	// Token: 0x0600133A RID: 4922 RVA: 0x000AA38F File Offset: 0x000A878F
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x000AA398 File Offset: 0x000A8798
	protected virtual void Start()
	{
		if (this.ExplosionPrefabOverride)
		{
			this.effectPrefab = this.ExplosionPrefabOverride;
		}
		else
		{
			this.effectPrefab = Level.Current.LevelResources.levelBossDeathExplosion;
		}
		Level.Current.OnBossDeathExplosionsEvent += this.StartExplosion;
		Level.Current.OnBossDeathExplosionsFalloffEvent += this.OnExplosionsRand;
		Level.Current.OnBossDeathExplosionsEndEvent += this.StopExplosions;
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x000AA420 File Offset: 0x000A8820
	private void OnDestroy()
	{
		this.ExplosionPrefabOverride = null;
		this.effectPrefab = null;
		try
		{
			Level.Current.OnBossDeathExplosionsEvent -= this.StartExplosion;
		}
		catch
		{
		}
		try
		{
			Level.Current.OnBossDeathExplosionsFalloffEvent -= this.OnExplosionsRand;
		}
		catch
		{
		}
		try
		{
			Level.Current.OnBossDeathExplosionsEndEvent -= this.StopExplosions;
		}
		catch
		{
		}
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x000AA4C8 File Offset: 0x000A88C8
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.offset, this.radius);
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x000AA505 File Offset: 0x000A8905
	public void StartExplosion()
	{
		this.StartExplosion(false);
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x000AA50E File Offset: 0x000A890E
	public void StartExplosion(bool bypassCameraShakeEvent)
	{
		if (this == null || !base.enabled || !base.isActiveAndEnabled)
		{
			return;
		}
		base.StartCoroutine(this.go_cr(bypassCameraShakeEvent));
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x000AA541 File Offset: 0x000A8941
	public void OnExplosionsRand()
	{
		this.state = LevelBossDeathExploder.State.Random;
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x000AA54A File Offset: 0x000A894A
	public void StopExplosions()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x000AA554 File Offset: 0x000A8954
	private Vector2 GetRandomPoint()
	{
		Vector2 a = base.transform.position + this.offset;
		Vector2 vector = new Vector2((float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1));
		Vector2 b = vector.normalized * (this.radius * UnityEngine.Random.value) * 2f;
		b.x *= this.scaleFactor.x;
		b.y *= this.scaleFactor.y;
		return a + b;
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x000AA5F0 File Offset: 0x000A89F0
	private IEnumerator go_cr(bool bypassCameraShakeEvent)
	{
		HitFlash flash = base.GetComponent<HitFlash>();
		if (!this.disableSound)
		{
			AudioManager.Play("level_explosion_boss_death");
		}
		for (;;)
		{
			this.effectPrefab.Create(this.GetRandomPoint());
			if (flash != null)
			{
				flash.Flash(0.1f);
			}
			CupheadLevelCamera.Current.Shake(10f, 0.4f, bypassCameraShakeEvent);
			LevelBossDeathExploder.State state = this.state;
			if (state != LevelBossDeathExploder.State.Random)
			{
				yield return CupheadTime.WaitForSeconds(this, this.STEADY_DELAY);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(this.MIN_DELAY, this.MAX_DELAY));
			}
		}
		yield break;
	}

	// Token: 0x04001C5C RID: 7260
	public Effect ExplosionPrefabOverride;

	// Token: 0x04001C5D RID: 7261
	[SerializeField]
	private float STEADY_DELAY = 0.3f;

	// Token: 0x04001C5E RID: 7262
	[SerializeField]
	private float MIN_DELAY = 0.4f;

	// Token: 0x04001C5F RID: 7263
	[SerializeField]
	private float MAX_DELAY = 1f;

	// Token: 0x04001C60 RID: 7264
	public Vector2 offset = Vector2.zero;

	// Token: 0x04001C61 RID: 7265
	[SerializeField]
	private float radius = 100f;

	// Token: 0x04001C62 RID: 7266
	[SerializeField]
	private Vector2 scaleFactor = new Vector2(1f, 1f);

	// Token: 0x04001C63 RID: 7267
	private LevelBossDeathExploder.State state;

	// Token: 0x04001C64 RID: 7268
	protected Effect effectPrefab;

	// Token: 0x04001C65 RID: 7269
	[SerializeField]
	private bool disableSound;

	// Token: 0x0200049E RID: 1182
	private enum State
	{
		// Token: 0x04001C67 RID: 7271
		Steady,
		// Token: 0x04001C68 RID: 7272
		Random
	}
}
