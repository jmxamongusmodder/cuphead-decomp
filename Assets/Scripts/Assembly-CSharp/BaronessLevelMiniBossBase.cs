using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020004F5 RID: 1269
public class BaronessLevelMiniBossBase : AbstractCollidableObject
{
	// Token: 0x1400003F RID: 63
	// (add) Token: 0x06001646 RID: 5702 RVA: 0x000C3390 File Offset: 0x000C1790
	// (remove) Token: 0x06001647 RID: 5703 RVA: 0x000C33C8 File Offset: 0x000C17C8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event BaronessLevelMiniBossBase.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x06001648 RID: 5704 RVA: 0x000C3400 File Offset: 0x000C1800
	protected virtual void Start()
	{
		this.endColor = base.GetComponent<SpriteRenderer>().color;
		base.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f);
		base.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 150;
		base.StartCoroutine(this.switchLayer_cr(this.layerSwitch));
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x000C3489 File Offset: 0x000C1889
	protected virtual void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x000C34A8 File Offset: 0x000C18A8
	protected virtual IEnumerator switchLayer_cr(int layerswitch)
	{
		base.StartCoroutine(this.fade_color_cr());
		yield return CupheadTime.WaitForSeconds(this, 3f);
		base.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 260;
		yield break;
	}

	// Token: 0x0600164B RID: 5707 RVA: 0x000C34C4 File Offset: 0x000C18C4
	protected virtual IEnumerator fade_color_cr()
	{
		float t = 0f;
		Color start = new Color(0f, 0f, 0f, 1f);
		while (t < this.fadeTime)
		{
			base.GetComponent<SpriteRenderer>().color = Color.Lerp(start, this.endColor, t / this.fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = this.endColor;
		yield return null;
		yield break;
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x000C34DF File Offset: 0x000C18DF
	protected virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x000C3500 File Offset: 0x000C1900
	protected virtual void StartExplosions()
	{
		if (base.GetComponent<LevelBossDeathExploder>() != null)
		{
			base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		}
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x000C351E File Offset: 0x000C191E
	protected virtual void EndExplosions()
	{
		if (base.GetComponent<LevelBossDeathExploder>() != null)
		{
			base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		}
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x000C353C File Offset: 0x000C193C
	protected virtual void Die()
	{
		this.EndExplosions();
		this.StopAllCoroutines();
		if (base.GetComponent<Collider2D>() != null)
		{
			base.GetComponent<Collider2D>().enabled = false;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001F92 RID: 8082
	public bool isDying;

	// Token: 0x04001F93 RID: 8083
	public bool startInvisible;

	// Token: 0x04001F94 RID: 8084
	public int layerSwitch = 4;

	// Token: 0x04001F95 RID: 8085
	public BaronessLevelCastle.BossPossibility bossId;

	// Token: 0x04001F97 RID: 8087
	protected float fadeTime = 0.5f;

	// Token: 0x04001F98 RID: 8088
	protected Color endColor;

	// Token: 0x020004F6 RID: 1270
	// (Invoke) Token: 0x06001651 RID: 5713
	public delegate void OnDamageTakenHandler(float damage);
}
