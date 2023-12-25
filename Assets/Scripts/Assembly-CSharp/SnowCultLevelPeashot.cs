using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007EF RID: 2031
public class SnowCultLevelPeashot : BasicProjectile
{
	// Token: 0x06002E9C RID: 11932 RVA: 0x001B7C7F File Offset: 0x001B607F
	protected override void Start()
	{
		base.Start();
		this.SFX_SNOWCULT_TarotCardTravelLoop();
	}

	// Token: 0x06002E9D RID: 11933 RVA: 0x001B7C90 File Offset: 0x001B6090
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		if (parryable)
		{
			int num = UnityEngine.Random.Range(0, 2);
			if (num != 0)
			{
				if (num == 1)
				{
					base.animator.Play("SunPink", 0, 0.25f);
				}
			}
			else
			{
				base.animator.Play("SwordPink", 0, 0.25f);
			}
		}
		else
		{
			int num2 = UnityEngine.Random.Range(0, 3);
			if (num2 != 0)
			{
				if (num2 != 1)
				{
					if (num2 == 2)
					{
						base.animator.Play("Moon", 0, 0.25f);
					}
				}
				else
				{
					base.animator.Play("Sun", 0, 0.25f);
				}
			}
			else
			{
				base.animator.Play("Sword", 0, 0.25f);
			}
		}
		base.animator.Update(0f);
	}

	// Token: 0x06002E9E RID: 11934 RVA: 0x001B7D84 File Offset: 0x001B6184
	private IEnumerator dead_cr()
	{
		this.Speed = 0f;
		this.move = false;
		this.boxCollider.enabled = false;
		this.SFX_SNOWCULT_TarotCardHitGround();
		switch ((int)(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f * 10f))
		{
		case 0:
		case 5:
			base.animator.Play("Die");
			break;
		case 1:
		case 6:
			base.animator.Play("DieAngleB");
			base.GetComponent<SpriteRenderer>().flipX = true;
			break;
		case 2:
		case 7:
			base.animator.Play("DieAngleA");
			base.GetComponent<SpriteRenderer>().flipX = true;
			break;
		case 3:
		case 8:
			base.animator.Play("DieAngleA");
			break;
		case 4:
		case 9:
			base.animator.Play("DieAngleB");
			break;
		}
		base.animator.Update(0f);
		Vector3 impactPos = new Vector3(base.transform.position.x, (float)Level.Current.Ground);
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
		{
			Vector3 offset = MathUtils.AngleToDirection((float)UnityEngine.Random.Range(0, 360)) * base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 250f;
			offset.y *= 0.3f;
			this.sparkleEffect.Create(impactPos + offset);
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.005f, 0.01f));
		}
		this.Die();
		yield break;
	}

	// Token: 0x06002E9F RID: 11935 RVA: 0x001B7DA0 File Offset: 0x001B61A0
	protected override void Move()
	{
		base.transform.position += base.transform.up * -this.Speed * CupheadTime.FixedDelta - new Vector3(0f, this._accumulativeGravity * CupheadTime.FixedDelta, 0f);
		if (base.transform.position.y <= (float)Level.Current.Ground + 15f || base.transform.position.x < (float)Level.Current.Left || base.transform.position.x > (float)Level.Current.Right)
		{
			base.StartCoroutine(this.dead_cr());
		}
	}

	// Token: 0x06002EA0 RID: 11936 RVA: 0x001B7E80 File Offset: 0x001B6280
	public override void OnParryDie()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop");
		base.OnParryDie();
	}

	// Token: 0x06002EA1 RID: 11937 RVA: 0x001B7E92 File Offset: 0x001B6292
	private void SFX_SNOWCULT_TarotCardTravelLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop");
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x001B7EAE File Offset: 0x001B62AE
	private void SFX_SNOWCULT_TarotCardHitGround()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop");
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_tarotcard_hitground");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_tarotcard_hitground");
	}

	// Token: 0x0400373B RID: 14139
	private const float GROUND_OFFSET = 15f;

	// Token: 0x0400373C RID: 14140
	[SerializeField]
	private BoxCollider2D boxCollider;

	// Token: 0x0400373D RID: 14141
	[SerializeField]
	private Effect sparkleEffect;
}
