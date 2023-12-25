using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000659 RID: 1625
public class FlyingCowboyLevelRicochetDebris : BasicUprightProjectile
{
	// Token: 0x060021DC RID: 8668 RVA: 0x0013B820 File Offset: 0x00139C20
	public virtual BasicProjectile Create(Vector3 position, float speed, float bulletSpeed, FlyingCowboyLevelRicochetDebris.BulletType bulletType, bool bulletParryable)
	{
		FlyingCowboyLevelRicochetDebris flyingCowboyLevelRicochetDebris = this.Create(position, MathUtils.DirectionToAngle(Vector3.down), speed) as FlyingCowboyLevelRicochetDebris;
		flyingCowboyLevelRicochetDebris.bulletType = bulletType;
		flyingCowboyLevelRicochetDebris.bulletSpeed = bulletSpeed;
		flyingCowboyLevelRicochetDebris.bulletParryable = bulletParryable;
		return flyingCowboyLevelRicochetDebris;
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x0013B868 File Offset: 0x00139C68
	protected override void Start()
	{
		base.Start();
		base.animator.Update(0f);
		base.animator.Play(0, 0, UnityEngine.Random.Range(0f, 1f));
		Vector3 localScale = base.transform.localScale;
		localScale.x *= (float)Rand.PosOrNeg();
		base.transform.localScale = localScale;
		if (base.animator.GetInteger(AbstractProjectile.Variant) == 0)
		{
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(FlyingCowboyLevelRicochetDebris.AllowedRotations.GetRandom<float>()));
		}
		base.StartCoroutine(this.fall_cr());
		base.StartCoroutine(this.shadowScale_cr());
		base.StartCoroutine(this.shadowPosition_cr());
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x0013B93D File Offset: 0x00139D3D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.SFX_COWGIRL_COWGIRL_P2_SafeHitPlayer();
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x0013B950 File Offset: 0x00139D50
	private IEnumerator fall_cr()
	{
		while (base.transform.position.y > -360f + FlyingCowboyLevelRicochetDebris.GroundOffset)
		{
			yield return null;
		}
		FlyingCowboyLevelRicochetDebris.BulletType bulletType = this.bulletType;
		if (bulletType != FlyingCowboyLevelRicochetDebris.BulletType.Nothing)
		{
			if (bulletType == FlyingCowboyLevelRicochetDebris.BulletType.Ricochet)
			{
				this.shootRicochetProjectile();
			}
		}
		this.SFX_COWGIRL_COWGIRL_P2_SafeDropImpact();
		this.Die();
		yield break;
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x0013B96C File Offset: 0x00139D6C
	private IEnumerator shadowScale_cr()
	{
		this.shadowTransform.rotation = Quaternion.identity;
		float ground = -360f + FlyingCowboyLevelRicochetDebris.GroundOffset;
		while (base.transform.position.y > ground + FlyingCowboyLevelRicochetDebris.ShadowStartOffset)
		{
			yield return null;
		}
		float startY = ground + FlyingCowboyLevelRicochetDebris.ShadowStartOffset;
		float endY = ground + FlyingCowboyLevelRicochetDebris.ShadowEndOffset;
		base.animator.Play("On", 1);
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.041666668f, false);
		while (!base.dead)
		{
			float parentScale = base.transform.localScale.x;
			Vector3 scale = this.shadowTransform.localScale;
			scale.x = (scale.y = MathUtilities.LerpMapping(base.transform.position.y, startY, endY, FlyingCowboyLevelRicochetDebris.ShadowScaleRange.min, FlyingCowboyLevelRicochetDebris.ShadowScaleRange.max, true) / parentScale);
			this.shadowTransform.localScale = scale;
			yield return wait;
		}
		base.animator.Play("Off", 1);
		yield break;
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x0013B988 File Offset: 0x00139D88
	private IEnumerator shadowPosition_cr()
	{
		float ground = -360f + FlyingCowboyLevelRicochetDebris.GroundOffset;
		while (!base.dead)
		{
			Vector3 position = this.shadowTransform.position;
			position.y = ground + FlyingCowboyLevelRicochetDebris.ShadowPositionOffset;
			this.shadowTransform.position = position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x0013B9A4 File Offset: 0x00139DA4
	private void shootRicochetProjectile()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		float rotation = MathUtils.DirectionToAngle(next.transform.position - base.transform.position);
		int num;
		BasicProjectile basicProjectile;
		if (this.bulletParryable)
		{
			num = UnityEngine.Random.Range(0, this.parryableProjectiles.Length);
			basicProjectile = this.parryableProjectiles[num].Create(base.transform.position, rotation, this.bulletSpeed);
			num++;
		}
		else
		{
			num = UnityEngine.Random.Range(0, this.regularProjectiles.Length);
			basicProjectile = this.regularProjectiles[num].Create(base.transform.position, rotation, this.bulletSpeed);
		}
		basicProjectile.SetParryable(this.bulletParryable);
		basicProjectile.GetComponent<SpriteRenderer>().sortingOrder = num;
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x0013BA74 File Offset: 0x00139E74
	protected override void Die()
	{
		this.RandomizeVariant();
		base.Die();
		bool flag = Rand.Bool();
		int num;
		if (FlyingCowboyLevelRicochetDebris.LastBitsIndex == 0)
		{
			num = ((!flag) ? 2 : 1);
		}
		else if (FlyingCowboyLevelRicochetDebris.LastBitsIndex == 1)
		{
			num = ((!flag) ? 2 : 0);
		}
		else
		{
			num = ((!flag) ? 1 : 0);
		}
		FlyingCowboyLevelRicochetDebris.LastBitsIndex = num;
		for (int i = 0; i < this.deathBits.Length; i++)
		{
			this.deathBits[i].enabled = (i == num);
		}
		this.deathEffect.Create(new Vector3(base.transform.position.x, -360f + FlyingCowboyLevelRicochetDebris.GroundOffset - 10f));
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x0013BB41 File Offset: 0x00139F41
	private void SFX_COWGIRL_COWGIRL_P2_SafeDropImpact()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_safedropimpact");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_safedropimpact");
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x0013BB5D File Offset: 0x00139F5D
	private void SFX_COWGIRL_COWGIRL_P2_SafeHitPlayer()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_safehitplayer");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_safehitplayer");
	}

	// Token: 0x04002A94 RID: 10900
	private static readonly float GroundOffset = 100f;

	// Token: 0x04002A95 RID: 10901
	private static readonly float ShadowPositionOffset = -50f;

	// Token: 0x04002A96 RID: 10902
	private static readonly float ShadowStartOffset = 300f;

	// Token: 0x04002A97 RID: 10903
	private static readonly float ShadowEndOffset = 50f;

	// Token: 0x04002A98 RID: 10904
	private static readonly MinMax ShadowScaleRange = new MinMax(0.1f, 1f);

	// Token: 0x04002A99 RID: 10905
	private static readonly float[] AllowedRotations = new float[]
	{
		-20f,
		-10f,
		0f,
		10f,
		20f
	};

	// Token: 0x04002A9A RID: 10906
	private static int LastBitsIndex = 0;

	// Token: 0x04002A9B RID: 10907
	[SerializeField]
	private SpriteRenderer[] deathBits;

	// Token: 0x04002A9C RID: 10908
	[SerializeField]
	private BasicProjectile[] regularProjectiles;

	// Token: 0x04002A9D RID: 10909
	[SerializeField]
	private BasicProjectile[] parryableProjectiles;

	// Token: 0x04002A9E RID: 10910
	[SerializeField]
	private Transform shadowTransform;

	// Token: 0x04002A9F RID: 10911
	[SerializeField]
	private Effect deathEffect;

	// Token: 0x04002AA0 RID: 10912
	private FlyingCowboyLevelRicochetDebris.BulletType bulletType;

	// Token: 0x04002AA1 RID: 10913
	private float bulletSpeed;

	// Token: 0x04002AA2 RID: 10914
	private bool bulletParryable;

	// Token: 0x0200065A RID: 1626
	public enum BulletType
	{
		// Token: 0x04002AA4 RID: 10916
		Nothing,
		// Token: 0x04002AA5 RID: 10917
		Ricochet
	}
}
