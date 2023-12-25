using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007F2 RID: 2034
public class SnowCultLevelQuadShot : AbstractProjectile
{
	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06002EAD RID: 11949 RVA: 0x001B8830 File Offset: 0x001B6C30
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002EAE RID: 11950 RVA: 0x001B8838 File Offset: 0x001B6C38
	public virtual SnowCultLevelQuadShot Init(Vector3 startPos, Vector3 destPos, float speed, string hazardDirectionInstruction, LevelProperties.SnowCult.QuadShot properties, int rowPosition, float delay, float distanceBetween, AbstractPlayerController targetPlayer)
	{
		base.ResetLifetime();
		base.ResetDistance();
		((SnowCultLevel)Level.Current).OnYetiHitGround += this.WhaleDeath;
		this.id = ((rowPosition % 2 != 0) ? 'B' : 'A');
		base.animator.Play("Emerge" + this.id);
		base.transform.position = startPos;
		this.startPos = startPos;
		this.destPos = destPos;
		this.properties = properties;
		this.speed = speed;
		this.hazardDirectionInstruction = hazardDirectionInstruction;
		this.rowPosition = rowPosition;
		this.distanceBetween = distanceBetween;
		this.targetPlayer = targetPlayer;
		this.delay = delay;
		base.transform.localScale = new Vector3((float)((this.rowPosition <= 1) ? 1 : -1), 1f);
		base.StartCoroutine(this.move_to_launch_pos_cr());
		base.tag = "EnemyProjectile";
		return this;
	}

	// Token: 0x06002EAF RID: 11951 RVA: 0x001B8937 File Offset: 0x001B6D37
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002EB0 RID: 11952 RVA: 0x001B8958 File Offset: 0x001B6D58
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (!this.grounded)
		{
			return;
		}
		base.OnCollisionEnemy(hit, phase);
		if (phase != CollisionPhase.Exit && (hit.GetComponent<SnowCultLevelWhaleCollision>() || hit.GetComponent<SnowCultLevelQuadShot>()) && phase == CollisionPhase.Enter)
		{
			base.transform.localScale = new Vector3(Mathf.Sign(base.transform.position.x - hit.gameObject.transform.position.x), 1f);
			this.WhaleDeath();
		}
	}

	// Token: 0x06002EB1 RID: 11953 RVA: 0x001B89F4 File Offset: 0x001B6DF4
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (!this.grounded)
		{
			return;
		}
		this.health -= info.damage;
		if (this.health < 0f)
		{
			if (this.running)
			{
				if (!base.dead)
				{
					Level.Current.RegisterMinionKilled();
					base.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), 1f);
					this.rend.flipX = Rand.Bool();
					this.Dead();
				}
			}
			else
			{
				this.running = true;
				this.health = this.properties.hazardHealth;
			}
		}
	}

	// Token: 0x06002EB2 RID: 11954 RVA: 0x001B8AA0 File Offset: 0x001B6EA0
	private IEnumerator move_to_launch_pos_cr()
	{
		float t = 0f;
		while (t < 0.33333334f)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
			t += 0.041666668f;
			base.transform.position = Vector3.Lerp(this.startPos, this.destPos, Mathf.InverseLerp(0f, 0.33333334f, t));
		}
		yield break;
	}

	// Token: 0x06002EB3 RID: 11955 RVA: 0x001B8ABC File Offset: 0x001B6EBC
	public void Shoot(float angle)
	{
		this.angle = angle;
		base.transform.localScale = new Vector3((float)((angle >= -90f) ? -1 : 1), 1f);
		base.animator.Play("Launch" + this.id);
		this.sparkEffect.Create(base.transform.position);
		if (this.rowPosition % 2 == 0)
		{
			this.rend.sortingOrder = 3;
		}
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06002EB4 RID: 11956 RVA: 0x001B8B58 File Offset: 0x001B6F58
	private IEnumerator shoot_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.delay);
		while (base.transform.position.y > -240f)
		{
			base.transform.position += MathUtils.AngleToDirection(this.angle) * this.speed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.transform.position = new Vector3(base.transform.position.x, -240f);
		base.StartCoroutine(this.run_away_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002EB5 RID: 11957 RVA: 0x001B8B74 File Offset: 0x001B6F74
	private IEnumerator run_away_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		AnimationHelper animHelper = base.GetComponent<AnimationHelper>();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.tag = "Enemy";
		this.grounded = true;
		this.health = this.properties.groundHealth;
		base.animator.Play("HitGround" + this.id);
		this.SFX_SNOWCULT_QuadshotMinionStuckInGround();
		this.snowLandEffect.Create(new Vector3(base.transform.position.x, (float)Level.Current.Ground));
		float t = (this.properties.hazardMoveDelay - this.delay) * this.popOutWarningTimeNormalized;
		while (t > 0f && !this.running)
		{
			t -= CupheadTime.Delta;
			yield return null;
		}
		animHelper.Speed = 1.5f;
		t = (this.properties.hazardMoveDelay - this.delay) * (1f - this.popOutWarningTimeNormalized);
		while (t > 0f && !this.running)
		{
			t -= CupheadTime.Delta;
			yield return null;
		}
		animHelper.Speed = 1f;
		this.running = true;
		this.health = this.properties.hazardHealth;
		float direction = 0f;
		string text = this.hazardDirectionInstruction;
		if (text != null)
		{
			if (!(text == "L"))
			{
				if (!(text == "R"))
				{
					if (!(text == "F"))
					{
						if (!(text == "G"))
						{
							if (text == "P")
							{
								direction = (float)((base.transform.position.x - this.targetPlayer.transform.position.x <= 0f) ? 1 : -1);
							}
						}
						else
						{
							float num = base.transform.position.x + ((float)(2 - this.rowPosition) - 0.5f) * this.distanceBetween;
							direction = (float)((Mathf.Abs(num - (float)Level.Current.Left) <= Mathf.Abs(num - (float)Level.Current.Right)) ? 1 : -1);
						}
					}
					else
					{
						direction = (float)((Mathf.Abs(base.transform.position.x - (float)Level.Current.Left) <= Mathf.Abs(base.transform.position.x - (float)Level.Current.Right)) ? 1 : -1);
					}
				}
				else
				{
					direction = 1f;
				}
			}
			else
			{
				direction = -1f;
			}
		}
		base.transform.localScale = new Vector3(-direction, 1f);
		base.animator.Play("PopOut" + this.id);
		this.SFX_SNOWCULT_QuadshotMinionFlipUp();
		this.snowPopOutEffect.Create(new Vector3(base.transform.position.x, (float)Level.Current.Ground));
		yield return null;
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.16666667f)
		{
			yield return null;
		}
		this.health = this.properties.hazardHealth;
		this.rend.sortingOrder = 1;
		t = 0f;
		while (base.transform.position.x > (float)(Level.Current.Left - 200) && base.transform.position.x < (float)(Level.Current.Right + 200))
		{
			t = Mathf.Clamp(t + CupheadTime.FixedDelta * 2f, 0f, 1f);
			base.transform.position += Vector3.right * direction * this.properties.hazardSpeed * CupheadTime.FixedDelta * t;
			yield return wait;
		}
		this.Recycle<SnowCultLevelQuadShot>();
		yield break;
	}

	// Token: 0x06002EB6 RID: 11958 RVA: 0x001B8B8F File Offset: 0x001B6F8F
	private void WhaleDeath()
	{
		this.Dead();
		this.rend.sortingLayerName = "Foreground";
		base.animator.Play("WhaleDeath" + this.id);
	}

	// Token: 0x06002EB7 RID: 11959 RVA: 0x001B8BC8 File Offset: 0x001B6FC8
	private void Dead()
	{
		this.StopAllCoroutines();
		this.deathPuff.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		this.deathPuff.flipX = Rand.Bool();
		this.deathPuff.flipY = Rand.Bool();
		base.animator.Play("Death");
		this.SFX_SNOWCULT_QuadshotMinionDie();
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002EB8 RID: 11960 RVA: 0x001B8C48 File Offset: 0x001B7048
	private void aniEvent_Dead()
	{
		this.Recycle<SnowCultLevelQuadShot>();
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x001B8C50 File Offset: 0x001B7050
	protected override void OnDestroy()
	{
		if (Level.Current)
		{
			((SnowCultLevel)Level.Current).OnYetiHitGround -= this.WhaleDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x001B8C82 File Offset: 0x001B7082
	private void SFX_SNOWCULT_QuadshotMinionStuckInGround()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_minion_stuckinground");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_minion_stuckinground");
	}

	// Token: 0x06002EBB RID: 11963 RVA: 0x001B8C9E File Offset: 0x001B709E
	private void SFX_SNOWCULT_QuadshotMinionFlipUp()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_minion_flipup");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_minion_flipup");
	}

	// Token: 0x06002EBC RID: 11964 RVA: 0x001B8CBA File Offset: 0x001B70BA
	private void SFX_SNOWCULT_QuadshotMinionDie()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_minion_death_explode");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_minion_death_explode");
	}

	// Token: 0x0400374F RID: 14159
	private const float GROUND_Y = -240f;

	// Token: 0x04003750 RID: 14160
	[SerializeField]
	private float popOutWarningTimeNormalized = 0.8f;

	// Token: 0x04003751 RID: 14161
	[SerializeField]
	private Effect sparkEffect;

	// Token: 0x04003752 RID: 14162
	[SerializeField]
	private Effect snowLandEffect;

	// Token: 0x04003753 RID: 14163
	[SerializeField]
	private Effect snowPopOutEffect;

	// Token: 0x04003754 RID: 14164
	[SerializeField]
	private SpriteRenderer deathPuff;

	// Token: 0x04003755 RID: 14165
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04003756 RID: 14166
	private LevelProperties.SnowCult.QuadShot properties;

	// Token: 0x04003757 RID: 14167
	private DamageReceiver damageReceiver;

	// Token: 0x04003758 RID: 14168
	private float speed;

	// Token: 0x04003759 RID: 14169
	private float delay;

	// Token: 0x0400375A RID: 14170
	private float angle;

	// Token: 0x0400375B RID: 14171
	private string hazardDirectionInstruction;

	// Token: 0x0400375C RID: 14172
	private int rowPosition;

	// Token: 0x0400375D RID: 14173
	private float distanceBetween;

	// Token: 0x0400375E RID: 14174
	private AbstractPlayerController targetPlayer;

	// Token: 0x0400375F RID: 14175
	private Vector3 startPos;

	// Token: 0x04003760 RID: 14176
	private Vector3 destPos;

	// Token: 0x04003761 RID: 14177
	private float health;

	// Token: 0x04003762 RID: 14178
	private bool isDead;

	// Token: 0x04003763 RID: 14179
	private bool grounded;

	// Token: 0x04003764 RID: 14180
	private bool running;

	// Token: 0x04003765 RID: 14181
	private char id;
}
