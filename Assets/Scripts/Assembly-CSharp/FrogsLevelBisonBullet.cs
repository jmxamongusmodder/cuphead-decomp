using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006A8 RID: 1704
public class FrogsLevelBisonBullet : AbstractFrogsLevelSlotBullet
{
	// Token: 0x170003AB RID: 939
	// (get) Token: 0x0600241F RID: 9247 RVA: 0x001532B2 File Offset: 0x001516B2
	protected override EaseUtils.EaseType Y_Ease
	{
		get
		{
			return EaseUtils.EaseType.easeOutElastic;
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06002420 RID: 9248 RVA: 0x001532B6 File Offset: 0x001516B6
	protected override float Y
	{
		get
		{
			return -60f;
		}
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x06002421 RID: 9249 RVA: 0x001532BD File Offset: 0x001516BD
	protected override float Y_Time
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06002422 RID: 9250 RVA: 0x001532C4 File Offset: 0x001516C4
	public FrogsLevelBisonBullet Create(Vector2 pos, float s, FrogsLevelBisonBullet.Direction direction, float bigX, float smallX)
	{
		FrogsLevelBisonBullet frogsLevelBisonBullet = base.Create(pos, s) as FrogsLevelBisonBullet;
		frogsLevelBisonBullet.Init(direction, bigX, smallX);
		return frogsLevelBisonBullet;
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x001532EC File Offset: 0x001516EC
	private void Init(FrogsLevelBisonBullet.Direction dir, float big, float small)
	{
		this.flame.GetComponent<Collider2D>().enabled = false;
		this.flame.GetComponent<CollisionChild>().OnPlayerCollision += base.DealDamage;
		this.direction = dir;
		this.bigX = big;
		base.StartCoroutine(this.bison_cr());
		base.StartCoroutine(this.small_cr());
	}

	// Token: 0x06002424 RID: 9252 RVA: 0x00153350 File Offset: 0x00151750
	private IEnumerator small_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		this.flame.GetComponent<Collider2D>().enabled = true;
		base.animator.SetTrigger("Small");
		yield break;
	}

	// Token: 0x06002425 RID: 9253 RVA: 0x0015336C File Offset: 0x0015176C
	private IEnumerator bison_cr()
	{
		if (this.direction == FrogsLevelBisonBullet.Direction.Down)
		{
			this.flame.SetEulerAngles(new float?(0f), new float?(0f), new float?(180f));
			this.flame.AddLocalPosition(0f, -115f, 0f);
			this.flame.GetComponent<SpriteRenderer>().sortingOrder = base.GetComponent<SpriteRenderer>().sortingOrder - 1;
		}
		yield return null;
		yield return null;
		yield return null;
		bool big = false;
		for (;;)
		{
			float distance = float.MaxValue;
			AbstractPlayerController p = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			AbstractPlayerController p2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (p != null)
			{
				distance = Mathf.Min(distance, base.transform.position.x - p.center.x);
			}
			if (p2 != null)
			{
				distance = Mathf.Min(distance, base.transform.position.x - p2.center.x);
			}
			if (distance <= this.bigX && !big)
			{
				big = true;
				AudioManager.Play("level_frogs_flame_platform_fire_burst");
				this.emitAudioFromObject.Add("level_frogs_flame_platform_fire_burst");
				AudioManager.PlayLoop("level_frogs_flame_platform_fire_loop");
				this.emitAudioFromObject.Add("level_frogs_flame_platform_fire_loop");
				this.flame.GetComponent<Collider2D>().enabled = true;
				base.animator.SetTrigger("Big");
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x00153387 File Offset: 0x00151787
	protected override void End()
	{
		AudioManager.Stop("level_frogs_flame_platform_fire_loop");
		base.End();
	}

	// Token: 0x04002CE5 RID: 11493
	public Transform flame;

	// Token: 0x04002CE6 RID: 11494
	private FrogsLevelBisonBullet.Direction direction;

	// Token: 0x04002CE7 RID: 11495
	private float bigX;

	// Token: 0x020006A9 RID: 1705
	public enum Direction
	{
		// Token: 0x04002CE9 RID: 11497
		Up,
		// Token: 0x04002CEA RID: 11498
		Down
	}
}
