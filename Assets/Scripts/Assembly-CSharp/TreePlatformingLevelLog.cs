using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000891 RID: 2193
public class TreePlatformingLevelLog : AbstractPlatformingLevelEnemy
{
	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x060032F9 RID: 13049 RVA: 0x001D9FE5 File Offset: 0x001D83E5
	public bool CanShoot
	{
		get
		{
			return this.canShoot;
		}
	}

	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x060032FA RID: 13050 RVA: 0x001D9FED File Offset: 0x001D83ED
	public float ShootDelay
	{
		get
		{
			return this.shootDelay;
		}
	}

	// Token: 0x060032FB RID: 13051 RVA: 0x001D9FF8 File Offset: 0x001D83F8
	protected override void Start()
	{
		base.Start();
		base._damageReceiver.enabled = false;
		this.pinkPattern = this.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = UnityEngine.Random.Range(0, this.pinkPattern.Length);
	}

	// Token: 0x060032FC RID: 13052 RVA: 0x001DA047 File Offset: 0x001D8447
	protected override void OnStart()
	{
	}

	// Token: 0x060032FD RID: 13053 RVA: 0x001DA049 File Offset: 0x001D8449
	public void SlideDown(float belowBoundsY)
	{
		base.StartCoroutine(this.slide_cr(belowBoundsY));
	}

	// Token: 0x060032FE RID: 13054 RVA: 0x001DA05C File Offset: 0x001D845C
	private IEnumerator slide_cr(float belowBoundsY)
	{
		this.isSliding = true;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.y > this.start - belowBoundsY)
		{
			base.transform.AddPosition(0f, -base.Properties.MoveSpeed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.start = base.transform.position.y;
		this.isSliding = false;
		yield return null;
		yield break;
	}

	// Token: 0x060032FF RID: 13055 RVA: 0x001DA07E File Offset: 0x001D847E
	protected override void Die()
	{
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x001DA080 File Offset: 0x001D8480
	public void KillLog()
	{
		this.SpawnPieces();
		this.isDying = true;
		base.Die();
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x001DA098 File Offset: 0x001D8498
	private void SpawnPieces()
	{
		AudioManager.Play("level_platform_logface_death");
		this.emitAudioFromObject.Add("level_platform_logface_death");
		foreach (SpriteDeathParts spriteDeathParts in this.parts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x001DA0F0 File Offset: 0x001D84F0
	public void OnShoot()
	{
		if (this.canShoot)
		{
			base.animator.SetTrigger("OnShoot");
		}
	}

	// Token: 0x06003303 RID: 13059 RVA: 0x001DA110 File Offset: 0x001D8510
	private void Shoot()
	{
		float num = base.Properties.ProjectileSpeed;
		if (this.facingRight)
		{
			num *= -1f;
		}
		this.projectile.Create(this.root.transform.position, 180f, num, !this.facingRight, this.pinkPattern[this.pinkIndex][0] == 'P');
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
		Effect effect = this.projectilePuff.Create(this.root.transform.position);
		effect.GetComponent<SpriteRenderer>().flipY = this.facingRight;
	}

	// Token: 0x06003304 RID: 13060 RVA: 0x001DA1C8 File Offset: 0x001D85C8
	public void SetDirection(bool isRight)
	{
		this.facingRight = isRight;
		if (this.facingRight)
		{
			Vector3 localScale = base.transform.localScale;
			localScale.x *= -1f;
			base.transform.localScale = localScale;
		}
	}

	// Token: 0x06003305 RID: 13061 RVA: 0x001DA212 File Offset: 0x001D8612
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.projectile = null;
		this.projectilePuff = null;
		this.parts = null;
	}

	// Token: 0x04003B20 RID: 15136
	[SerializeField]
	private TreePlatformingLevelLogProjectile projectile;

	// Token: 0x04003B21 RID: 15137
	[SerializeField]
	private Transform root;

	// Token: 0x04003B22 RID: 15138
	[SerializeField]
	private float shootDelay;

	// Token: 0x04003B23 RID: 15139
	[SerializeField]
	private SpriteDeathParts[] parts;

	// Token: 0x04003B24 RID: 15140
	[SerializeField]
	private bool canShoot;

	// Token: 0x04003B25 RID: 15141
	[SerializeField]
	private string pinkString;

	// Token: 0x04003B26 RID: 15142
	[SerializeField]
	private Effect projectilePuff;

	// Token: 0x04003B27 RID: 15143
	private bool facingRight;

	// Token: 0x04003B28 RID: 15144
	private string[] pinkPattern;

	// Token: 0x04003B29 RID: 15145
	private int pinkIndex;

	// Token: 0x04003B2A RID: 15146
	public bool isDying;

	// Token: 0x04003B2B RID: 15147
	public bool isSliding;

	// Token: 0x04003B2C RID: 15148
	public float start;
}
