using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007CA RID: 1994
public class SaltbakerLevelHeart : AbstractProjectile
{
	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x06002D37 RID: 11575 RVA: 0x001A9ECA File Offset: 0x001A82CA
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002D38 RID: 11576 RVA: 0x001A9ED4 File Offset: 0x001A82D4
	protected override void Start()
	{
		base.Start();
		this.ballSize = base.GetComponent<Collider2D>().bounds.size.y / 2f;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.impactFX.transform.parent = null;
	}

	// Token: 0x06002D39 RID: 11577 RVA: 0x001A9F42 File Offset: 0x001A8342
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002D3A RID: 11578 RVA: 0x001A9F60 File Offset: 0x001A8360
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.parent.TakeDamage(info);
	}

	// Token: 0x06002D3B RID: 11579 RVA: 0x001A9F70 File Offset: 0x001A8370
	public void Init(Vector3 pos, GameObject leftPillar, GameObject rightPillar, LevelProperties.Saltbaker.DarkHeart properties, SaltbakerLevelPillarHandler parent)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.isMoving = false;
		this.speed = properties.heartSpeed;
		this.parent = parent;
		this.leftPillarColl = leftPillar.GetComponent<Collider2D>();
		this.rightPillarColl = rightPillar.GetComponent<Collider2D>();
		this.SetParryable(true);
		this.coll.enabled = false;
		this.angleString = new PatternString(properties.angleOffsetString, true, true);
		this.dir = MathUtils.AngleToDirection(properties.baseAngle);
		base.transform.localScale = new Vector3(-Mathf.Sign(this.dir.x), 1f);
		this.lastDirNoOffset = this.dir;
		base.StartCoroutine(this.warning_cr());
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x001AA044 File Offset: 0x001A8444
	private IEnumerator warning_cr()
	{
		this.SFX_SALTB_HeartWarning();
		yield return base.animator.WaitForAnimationToEnd(this, "Warning", false, true);
		this.isMoving = true;
		this.coll.enabled = true;
		yield break;
	}

	// Token: 0x06002D3D RID: 11581 RVA: 0x001AA060 File Offset: 0x001A8460
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.isMoving)
		{
			base.transform.position += this.dir * this.speed * CupheadTime.Delta;
		}
		this.CheckBounds();
		if (!this.isDead)
		{
			this.pinkSprite.enabled = base.CanParry;
		}
		if (base.CanParry && !this.isDead)
		{
			this.pinkSprite.color = new Color(this.pinkSprite.color.r, this.pinkSprite.color.g, this.pinkSprite.color.b, this.pinkSprite.color.a + Time.deltaTime * 2f);
			this.regularSprite.color = new Color(this.regularSprite.color.r, this.regularSprite.color.g, this.regularSprite.color.b, this.regularSprite.color.a - Time.deltaTime * 0.5f);
		}
	}

	// Token: 0x06002D3E RID: 11582 RVA: 0x001AA1D8 File Offset: 0x001A85D8
	public override void OnParry(AbstractPlayerController player)
	{
		this.SetParryable(false);
		this.pinkSprite.color = new Color(0f, 0f, 0f, 0f);
		this.regularSprite.color = Color.black;
		base.StartCoroutine(this.coolDown_cr());
		base.StartCoroutine(this.colliderCoolDown_cr());
	}

	// Token: 0x06002D3F RID: 11583 RVA: 0x001AA23C File Offset: 0x001A863C
	private IEnumerator coolDown_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.parryTimeOut);
		this.SetParryable(true);
		yield return null;
		yield break;
	}

	// Token: 0x06002D40 RID: 11584 RVA: 0x001AA258 File Offset: 0x001A8658
	private IEnumerator colliderCoolDown_cr()
	{
		this.damageDealer.SetDamageFlags(false, false, false);
		yield return CupheadTime.WaitForSeconds(this, this.properties.collisionTimeOut);
		this.damageDealer.SetDamageFlags(true, false, false);
		yield return null;
		yield break;
	}

	// Token: 0x06002D41 RID: 11585 RVA: 0x001AA274 File Offset: 0x001A8674
	private void CheckBounds()
	{
		if (this.lastHit != SaltbakerLevelHeart.LastHit.Up && base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax - this.ballSize)
		{
			this.SetNewDir(true, false);
			this.lastHit = SaltbakerLevelHeart.LastHit.Up;
		}
		else if (this.lastHit != SaltbakerLevelHeart.LastHit.Down && base.transform.position.y < CupheadLevelCamera.Current.Bounds.yMin + this.ballSize)
		{
			this.SetNewDir(false, false);
			this.lastHit = SaltbakerLevelHeart.LastHit.Down;
		}
		else if (this.lastHit != SaltbakerLevelHeart.LastHit.Left && base.transform.position.x < this.leftPillarColl.bounds.max.x + this.ballSize)
		{
			this.SetNewDir(false, true);
			this.lastHit = SaltbakerLevelHeart.LastHit.Left;
		}
		else if (this.lastHit != SaltbakerLevelHeart.LastHit.Right && base.transform.position.x > this.rightPillarColl.bounds.min.x - this.ballSize)
		{
			this.SetNewDir(true, true);
			this.lastHit = SaltbakerLevelHeart.LastHit.Right;
		}
	}

	// Token: 0x06002D42 RID: 11586 RVA: 0x001AA3D8 File Offset: 0x001A87D8
	private void SetNewDir(bool getMin, bool isX)
	{
		this.angleOffset = this.angleString.PopFloat();
		Vector3 v = this.lastDirNoOffset;
		if (getMin)
		{
			if (isX)
			{
				v.x = Mathf.Min(v.x, -v.x);
				base.StartCoroutine(this.turn_cr());
			}
			else
			{
				v.y = Mathf.Min(v.y, -v.y);
			}
		}
		else if (isX)
		{
			v.x = Mathf.Max(v.x, -v.x);
			base.StartCoroutine(this.turn_cr());
		}
		else
		{
			v.y = Mathf.Max(v.y, -v.y);
		}
		this.lastDirNoOffset = v;
		float num = MathUtils.DirectionToAngle(v);
		num += this.angleOffset;
		v = MathUtils.AngleToDirection(num);
		this.dir = v;
	}

	// Token: 0x06002D43 RID: 11587 RVA: 0x001AA4D4 File Offset: 0x001A88D4
	private IEnumerator turn_cr()
	{
		this.isMoving = false;
		base.animator.Play("Turn");
		this.impactFX.transform.position = base.transform.position;
		this.impactFX.transform.localScale = base.transform.localScale;
		this.impactFX.Play((!Rand.Bool()) ? "B" : "A", 0, 0f);
		this.SFX_SALTB_HeartBounce();
		yield return null;
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.18181819f)
		{
			yield return null;
		}
		this.isMoving = true;
		base.StartCoroutine(this.turn_fx_cr());
		yield break;
	}

	// Token: 0x06002D44 RID: 11588 RVA: 0x001AA4F0 File Offset: 0x001A88F0
	private IEnumerator turn_fx_cr()
	{
		Vector3 pos = base.transform.position;
		int fxCount = UnityEngine.Random.Range(2, 4);
		for (int i = 0; i < fxCount; i++)
		{
			this.turnFX.Create(pos);
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0f, 0.1f));
		}
		yield break;
	}

	// Token: 0x06002D45 RID: 11589 RVA: 0x001AA50C File Offset: 0x001A890C
	private void AniEvent_Turn()
	{
		base.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
	}

	// Token: 0x06002D46 RID: 11590 RVA: 0x001AA544 File Offset: 0x001A8944
	public new void Die()
	{
		this.StopAllCoroutines();
		this.coll.enabled = false;
		this.isMoving = false;
		this.regularSprite.enabled = false;
		this.pinkSprite.enabled = true;
		this.pinkSprite.color = new Color(0f, 0f, 0f, 1f);
		this.isDead = true;
		base.animator.Play("Death");
		AudioManager.Play("level_explosion_boss_death");
	}

	// Token: 0x06002D47 RID: 11591 RVA: 0x001AA5C7 File Offset: 0x001A89C7
	private void SFX_SALTB_HeartBounce()
	{
		AudioManager.Play("sfx_DLC_Saltbaker_P4_Heart_Bounce");
		this.emitAudioFromObject.Add("sfx_DLC_Saltbaker_P4_Heart_Bounce");
	}

	// Token: 0x06002D48 RID: 11592 RVA: 0x001AA5E3 File Offset: 0x001A89E3
	private void SFX_SALTB_HeartWarning()
	{
		AudioManager.Play("sfx_DLC_Saltbaker_P4_Heart_Warning");
		this.emitAudioFromObject.Add("sfx_DLC_Saltbaker_P4_Heart_Warning");
	}

	// Token: 0x040035AD RID: 13741
	[SerializeField]
	private SpriteRenderer pinkSprite;

	// Token: 0x040035AE RID: 13742
	[SerializeField]
	private SpriteRenderer regularSprite;

	// Token: 0x040035AF RID: 13743
	[SerializeField]
	private Collider2D coll;

	// Token: 0x040035B0 RID: 13744
	[SerializeField]
	private Animator impactFX;

	// Token: 0x040035B1 RID: 13745
	[SerializeField]
	private Effect turnFX;

	// Token: 0x040035B2 RID: 13746
	private float ballSize;

	// Token: 0x040035B3 RID: 13747
	private float speed;

	// Token: 0x040035B4 RID: 13748
	private float angleOffset;

	// Token: 0x040035B5 RID: 13749
	private bool isMoving;

	// Token: 0x040035B6 RID: 13750
	private bool isDead;

	// Token: 0x040035B7 RID: 13751
	private LevelProperties.Saltbaker.DarkHeart properties;

	// Token: 0x040035B8 RID: 13752
	private SaltbakerLevelPillarHandler parent;

	// Token: 0x040035B9 RID: 13753
	private DamageReceiver damageReceiver;

	// Token: 0x040035BA RID: 13754
	private Collider2D leftPillarColl;

	// Token: 0x040035BB RID: 13755
	private Collider2D rightPillarColl;

	// Token: 0x040035BC RID: 13756
	private Vector3 dir;

	// Token: 0x040035BD RID: 13757
	private Vector3 lastDirNoOffset;

	// Token: 0x040035BE RID: 13758
	private PatternString angleString;

	// Token: 0x040035BF RID: 13759
	public SaltbakerLevelHeart.LastHit lastHit;

	// Token: 0x020007CB RID: 1995
	public enum LastHit
	{
		// Token: 0x040035C1 RID: 13761
		None,
		// Token: 0x040035C2 RID: 13762
		Left,
		// Token: 0x040035C3 RID: 13763
		Right,
		// Token: 0x040035C4 RID: 13764
		Up,
		// Token: 0x040035C5 RID: 13765
		Down
	}
}
