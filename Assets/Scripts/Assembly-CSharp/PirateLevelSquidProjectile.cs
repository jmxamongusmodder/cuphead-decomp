using System;
using UnityEngine;

// Token: 0x0200072D RID: 1837
public class PirateLevelSquidProjectile : AbstractMonoBehaviour
{
	// Token: 0x06002803 RID: 10243 RVA: 0x00176208 File Offset: 0x00174608
	private void Update()
	{
		if (this.state == PirateLevelSquidProjectile.State.Moving)
		{
			if (this.lifetime > 5f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
		}
		this.lifetime += CupheadTime.Delta;
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x001762B4 File Offset: 0x001746B4
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.name == PlayerId.PlayerOne.ToString() || collider.name == PlayerId.PlayerTwo.ToString())
		{
			PirateLevelSquidInkOverlay.Current.Hit();
			CupheadLevelCamera.Current.Shake(4f, 0.3f, false);
			this.Die();
		}
		else if (collider.name == "Level_Ground")
		{
			this.Die();
		}
	}

	// Token: 0x06002805 RID: 10245 RVA: 0x00176344 File Offset: 0x00174744
	public void Create(Vector2 pos, Vector2 velocity, float gravity)
	{
		this.InstantiatePrefab<PirateLevelSquidProjectile>().Init(pos, velocity, gravity);
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x00176354 File Offset: 0x00174754
	private void Init(Vector2 pos, Vector2 velocity, float gravity)
	{
		base.transform.position = pos;
		this.velocity = velocity;
		this.gravity = gravity;
		this.state = PirateLevelSquidProjectile.State.Moving;
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x0017637C File Offset: 0x0017477C
	private void Die()
	{
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<Collider2D>().enabled = false;
		this.state = PirateLevelSquidProjectile.State.Dead;
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x001763A1 File Offset: 0x001747A1
	private void OnDeathAnimationComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040030C6 RID: 12486
	public const float MAX_LIFETIME = 5f;

	// Token: 0x040030C7 RID: 12487
	private PirateLevelSquidProjectile.State state;

	// Token: 0x040030C8 RID: 12488
	private Vector2 velocity;

	// Token: 0x040030C9 RID: 12489
	private float gravity;

	// Token: 0x040030CA RID: 12490
	private float lifetime;

	// Token: 0x0200072E RID: 1838
	public enum State
	{
		// Token: 0x040030CC RID: 12492
		Init,
		// Token: 0x040030CD RID: 12493
		Moving,
		// Token: 0x040030CE RID: 12494
		Dead
	}
}
