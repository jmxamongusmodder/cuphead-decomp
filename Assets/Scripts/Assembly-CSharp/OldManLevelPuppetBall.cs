using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200070B RID: 1803
public class OldManLevelPuppetBall : AbstractProjectile
{
	// Token: 0x170003CE RID: 974
	// (get) Token: 0x060026E3 RID: 9955 RVA: 0x0016C659 File Offset: 0x0016AA59
	// (set) Token: 0x060026E4 RID: 9956 RVA: 0x0016C661 File Offset: 0x0016AA61
	public bool readyToCatch { get; private set; }

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x060026E5 RID: 9957 RVA: 0x0016C66A File Offset: 0x0016AA6A
	// (set) Token: 0x060026E6 RID: 9958 RVA: 0x0016C672 File Offset: 0x0016AA72
	public bool isMoving { get; private set; }

	// Token: 0x060026E7 RID: 9959 RVA: 0x0016C67C File Offset: 0x0016AA7C
	public virtual OldManLevelPuppetBall Init(Vector3 startPos, Vector3 platformPos, Vector3 endPos, LevelProperties.OldMan.Hands properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = startPos;
		base.transform.localScale = new Vector3(Mathf.Sign(endPos.x - startPos.x), 1f);
		this.startPos = startPos;
		this.endPos = endPos;
		this.platformPos = platformPos + Vector3.up * -10f;
		this.properties = properties;
		this.Move();
		base.animator.Play("Idle", 0, 0.7647059f);
		return this;
	}

	// Token: 0x060026E8 RID: 9960 RVA: 0x0016C718 File Offset: 0x0016AB18
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x060026E9 RID: 9961 RVA: 0x0016C726 File Offset: 0x0016AB26
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		this.SFX_OMM_P2_DamagePlayerCheer();
	}

	// Token: 0x060026EA RID: 9962 RVA: 0x0016C74A File Offset: 0x0016AB4A
	private void Move()
	{
		this.isMoving = true;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060026EB RID: 9963 RVA: 0x0016C760 File Offset: 0x0016AB60
	private IEnumerator move_cr()
	{
		this.readyToCatch = false;
		float STRAIGHT_BOUNCE_CUTOFF = 0.66f;
		YieldInstruction wait = new WaitForFixedUpdate();
		float percentage = Mathf.Abs(this.startPos.x - this.platformPos.x) / Mathf.Abs(this.startPos.x - this.endPos.x);
		float newX = base.transform.position.x;
		float newY = base.transform.position.y;
		float direction = Mathf.Sign(this.endPos.x - this.startPos.x);
		float xTotalDist = Mathf.Abs(this.startPos.x - this.platformPos.x);
		float yRad = base.transform.position.y - (this.platformPos.y + this.size);
		while (base.transform.position.y > this.platformPos.y + this.size + 20f)
		{
			newX += direction * CupheadTime.FixedDelta * this.properties.ballSpeed * ((!this.puppetDead) ? 1f : 0f);
			float xDist = Mathf.Abs(newX - this.startPos.x);
			newY = ((percentage >= 1f - STRAIGHT_BOUNCE_CUTOFF) ? (this.platformPos.y + this.size + yRad * Mathf.Cos(xDist / xTotalDist * 1.5707964f)) : Mathf.Lerp(this.startPos.y, this.platformPos.y + this.size, xDist / xTotalDist));
			base.transform.SetPosition(new float?(newX), new float?(newY), null);
			yield return wait;
		}
		base.transform.SetPosition(new float?(this.platformPos.x), new float?(this.platformPos.y + this.size), null);
		newX = base.transform.position.x;
		xTotalDist = Mathf.Abs(this.platformPos.x - this.endPos.x);
		yRad = this.endPos.y - base.transform.position.y;
		base.animator.SetTrigger("OnBounce");
		yield return base.animator.WaitForAnimationToEnd(this, "Bounce", false, true);
		while (Mathf.Sign(this.endPos.x - base.transform.position.x) == direction || this.puppetDead)
		{
			newX += direction * CupheadTime.FixedDelta * this.properties.ballSpeed * ((!this.puppetDead) ? 1f : 0f);
			float xDist = Mathf.Abs(newX - this.platformPos.x);
			newY = ((percentage <= STRAIGHT_BOUNCE_CUTOFF) ? (this.platformPos.y + this.size + yRad * Mathf.Sin(xDist / xTotalDist * 1.5707964f)) : Mathf.Lerp(this.platformPos.y + this.size, this.endPos.y, xDist / xTotalDist));
			base.transform.SetPosition(new float?(newX), new float?(newY), null);
			if (!this.readyToCatch && xDist / xTotalDist >= 0.9f && !this.puppetDead)
			{
				this.readyToCatch = true;
			}
			if (this.puppetDead && (base.transform.position.x > 1140f || base.transform.position.x < -1140f))
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060026EC RID: 9964 RVA: 0x0016C77C File Offset: 0x0016AB7C
	private void LateUpdate()
	{
		this.shadowRend.transform.position = new Vector3(base.transform.position.x, this.platformPos.y + this.size);
		if (base.transform.position.y < this.platformPos.y + this.size + this.shadowRange)
		{
			float num = Mathf.Lerp((float)(this.shadowSprites.Length - 1), 0f, Mathf.InverseLerp(this.platformPos.y + this.size, this.platformPos.y + this.size + this.shadowRange, base.transform.position.y));
			this.shadowRend.sprite = this.shadowSprites[(int)num];
		}
	}

	// Token: 0x060026ED RID: 9965 RVA: 0x0016C861 File Offset: 0x0016AC61
	public void GetCaught()
	{
		this.isMoving = false;
		this.Recycle<OldManLevelPuppetBall>();
	}

	// Token: 0x060026EE RID: 9966 RVA: 0x0016C870 File Offset: 0x0016AC70
	public void Explode()
	{
		this.puppetDead = true;
		this.shadowRend.enabled = false;
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		component.sortingLayerName = "Effects";
		component.sortingOrder = 100;
		base.animator.Play("Explode");
		for (int i = 0; i < 12; i++)
		{
			this.coinPrefab.Create(base.transform.position + MathUtils.AngleToDirection((float)(UnityEngine.Random.Range(0, 360) * UnityEngine.Random.Range(0, 50))));
		}
		for (int j = 0; j < 15; j++)
		{
			this.featherPrefab.Create(base.transform.position + MathUtils.AngleToDirection((float)(UnityEngine.Random.Range(0, 360) * UnityEngine.Random.Range(0, 50))));
		}
	}

	// Token: 0x060026EF RID: 9967 RVA: 0x0016C956 File Offset: 0x0016AD56
	private void SFX_OMM_P2_DamagePlayerCheer()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_ball_damageplayercheer");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_ball_damageplayercheer");
	}

	// Token: 0x060026F0 RID: 9968 RVA: 0x0016C972 File Offset: 0x0016AD72
	private void AnimationEvent_SFX_OMM_P2_PuppetBallBounce()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_ball_bounce");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_ball_bounce");
	}

	// Token: 0x060026F1 RID: 9969 RVA: 0x0016C98E File Offset: 0x0016AD8E
	private void AnimationEvent_ExplodeEnd()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060026F2 RID: 9970 RVA: 0x0016C99B File Offset: 0x0016AD9B
	private void WORKAROUND_NullifyFields()
	{
		this.sprite = null;
		this.shadowRend = null;
		this.shadowSprites = null;
		this.coinPrefab = null;
		this.featherPrefab = null;
	}

	// Token: 0x04002F90 RID: 12176
	private const float GROUND_Y_OFFSET = -10f;

	// Token: 0x04002F91 RID: 12177
	private const float HIT_GROUND_OFFSET = 20f;

	// Token: 0x04002F94 RID: 12180
	private LevelProperties.OldMan.Hands properties;

	// Token: 0x04002F95 RID: 12181
	private Vector3 startPos;

	// Token: 0x04002F96 RID: 12182
	private Vector3 endPos;

	// Token: 0x04002F97 RID: 12183
	private Vector3 platformPos;

	// Token: 0x04002F98 RID: 12184
	private float size = 50f;

	// Token: 0x04002F99 RID: 12185
	[SerializeField]
	private float shadowRange = 100f;

	// Token: 0x04002F9A RID: 12186
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x04002F9B RID: 12187
	[SerializeField]
	private SpriteRenderer shadowRend;

	// Token: 0x04002F9C RID: 12188
	[SerializeField]
	private Sprite[] shadowSprites;

	// Token: 0x04002F9D RID: 12189
	[SerializeField]
	private Effect coinPrefab;

	// Token: 0x04002F9E RID: 12190
	[SerializeField]
	private Effect featherPrefab;

	// Token: 0x04002F9F RID: 12191
	private bool puppetDead;
}
