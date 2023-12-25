using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000589 RID: 1417
public class DevilLevelBomb : AbstractProjectile
{
	// Token: 0x06001B0D RID: 6925 RVA: 0x000F8B68 File Offset: 0x000F6F68
	public DevilLevelBomb Create(Vector2 pos, LevelProperties.Devil.BombEye properties, bool onLeft)
	{
		DevilLevelBomb devilLevelBomb = this.InstantiatePrefab<DevilLevelBomb>();
		devilLevelBomb.properties = properties;
		devilLevelBomb.transform.position = pos;
		devilLevelBomb.startPos = pos;
		devilLevelBomb.flipX = Rand.Bool();
		devilLevelBomb.flipY = Rand.Bool();
		devilLevelBomb.onLeft = onLeft;
		return devilLevelBomb;
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x000F8BB9 File Offset: 0x000F6FB9
	protected override void Start()
	{
		base.Start();
		AudioManager.Play("p3_bomb_appear");
		this.emitAudioFromObject.Add("p3_bomb_appear");
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x000F8BE8 File Offset: 0x000F6FE8
	private IEnumerator move_cr()
	{
		float t = 0f;
		float time = 0.5f;
		float end = (!this.onLeft) ? (this.startPos.x - 300f) : (this.startPos.x + 300f);
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Lerp(this.startPos.x, end, t / time)), null, null);
			yield return new WaitForFixedUpdate();
		}
		base.StartCoroutine(this.fade_shadow_cr());
		this.endPos = base.transform.position;
		this.comingOut = false;
		yield return null;
		yield break;
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x000F8C04 File Offset: 0x000F7004
	private IEnumerator fade_shadow_cr()
	{
		float t = 0f;
		float time = 1f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.shadowSprite.color = new Color(1f, 1f, 1f, 1f - t / time);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x000F8C20 File Offset: 0x000F7020
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead || this.comingOut)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta;
		if (this.t > this.properties.explodeDelay)
		{
			this.Explode();
			this.Die();
			return;
		}
		Vector2 vector = this.endPos;
		vector.x += Mathf.Sin(this.t * this.properties.xSinSpeed * (float)((!this.flipX) ? 1 : -1)) * this.properties.xSinHeight;
		vector.y += Mathf.Sin(this.t * this.properties.ySinSpeed * (float)((!this.flipY) ? 1 : -1)) * this.properties.ySinHeight;
		base.transform.SetPosition(new float?(vector.x), new float?(vector.y), null);
	}

	// Token: 0x06001B12 RID: 6930 RVA: 0x000F8D3C File Offset: 0x000F713C
	private void Explode()
	{
		this.explosionPrefab.Create(base.transform.position);
	}

	// Token: 0x06001B13 RID: 6931 RVA: 0x000F8D55 File Offset: 0x000F7155
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x000F8D68 File Offset: 0x000F7168
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.explosionPrefab = null;
	}

	// Token: 0x0400244B RID: 9291
	[SerializeField]
	private DevilLevelBombExplosion explosionPrefab;

	// Token: 0x0400244C RID: 9292
	[SerializeField]
	private SpriteRenderer shadowSprite;

	// Token: 0x0400244D RID: 9293
	private LevelProperties.Devil.BombEye properties;

	// Token: 0x0400244E RID: 9294
	private Vector2 startPos;

	// Token: 0x0400244F RID: 9295
	private Vector2 endPos;

	// Token: 0x04002450 RID: 9296
	private float t;

	// Token: 0x04002451 RID: 9297
	private bool flipX;

	// Token: 0x04002452 RID: 9298
	private bool flipY;

	// Token: 0x04002453 RID: 9299
	private bool onLeft;

	// Token: 0x04002454 RID: 9300
	private bool comingOut = true;
}
