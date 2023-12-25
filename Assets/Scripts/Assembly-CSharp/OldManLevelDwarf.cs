using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000700 RID: 1792
public class OldManLevelDwarf : AbstractProjectile
{
	// Token: 0x170003CA RID: 970
	// (get) Token: 0x0600265C RID: 9820 RVA: 0x0016674C File Offset: 0x00164B4C
	// (set) Token: 0x0600265D RID: 9821 RVA: 0x00166754 File Offset: 0x00164B54
	public bool inPlace { get; private set; }

	// Token: 0x0600265E RID: 9822 RVA: 0x0016675D File Offset: 0x00164B5D
	protected override void OnDieDistance()
	{
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x0016675F File Offset: 0x00164B5F
	protected override void OnDieLifetime()
	{
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x00166764 File Offset: 0x00164B64
	protected override void Start()
	{
		base.Start();
		this.startPos = base.transform.position;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiver.enabled = false;
		this.inPlace = true;
	}

	// Token: 0x06002661 RID: 9825 RVA: 0x001667BE File Offset: 0x00164BBE
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002662 RID: 9826 RVA: 0x001667DC File Offset: 0x00164BDC
	public override void OnParry(AbstractPlayerController player)
	{
		this.Death(true);
	}

	// Token: 0x06002663 RID: 9827 RVA: 0x001667E5 File Offset: 0x00164BE5
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Death(false);
		}
	}

	// Token: 0x06002664 RID: 9828 RVA: 0x0016681C File Offset: 0x00164C1C
	private IEnumerator move_up_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float speed = 200f;
		this.rend.sortingLayerID = SortingLayer.NameToID("Default");
		this.rend.sortingOrder = 2;
		while (base.transform.position.y < -430f)
		{
			base.transform.AddPosition(0f, speed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.beardController.CueRuffle(this.rufflePos);
		base.animator.SetTrigger("Continue");
		string typeString = (!this.typeA) ? "_B" : "_A";
		yield return base.animator.WaitForAnimationToEnd(this, "Trans" + typeString + this.colorString, false, true);
		yield return null;
		yield break;
	}

	// Token: 0x06002665 RID: 9829 RVA: 0x00166837 File Offset: 0x00164C37
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
	}

	// Token: 0x06002666 RID: 9830 RVA: 0x00166840 File Offset: 0x00164C40
	public void ShootInArc(float apexHeight, float timeToApex, float health, bool typeA, bool parryable, float warningTime)
	{
		this.apexheight = apexHeight;
		this.apexTime = timeToApex;
		this.health = health;
		this.typeA = typeA;
		this.inPlace = false;
		this.damageReceiver.enabled = true;
		this.parryable = parryable;
		this.warningTime = warningTime;
		if (parryable)
		{
			this.colorString = "_Pink";
		}
		else
		{
			this.colorString = ((!this.isBlue) ? "_Teal" : string.Empty);
			this.isBlue = !this.isBlue;
		}
		this.SetParryable(false);
		base.StartCoroutine(this.arc_cr());
	}

	// Token: 0x06002667 RID: 9831 RVA: 0x001668E8 File Offset: 0x00164CE8
	private void CalculateArc()
	{
		float num = this.apexheight;
		float num2 = this.apexTime * this.apexTime;
		float num3 = -2f * num / num2;
		float num4 = 2f * num / this.apexTime;
		float num5 = num4 * num4;
		Vector3 position = base.transform.position;
		Vector3 position2 = PlayerManager.GetNext().transform.position;
		float num6 = position2.x - position.x;
		float num7 = position2.y - position.y;
		float num8 = num5 + 2f * num3 * num7;
		if (num8 < 0f)
		{
			num8 = 0f;
		}
		float a = (-num4 + Mathf.Sqrt(num8)) / num3;
		float b = (-num4 - Mathf.Sqrt(num8)) / num3;
		float num9 = Mathf.Max(a, b);
		this.vel.x = num6 / num9;
		this.vel.y = num4;
		this.gravity = num3;
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x001669D4 File Offset: 0x00164DD4
	private IEnumerator arc_cr()
	{
		bool finishedArcing = false;
		this.inPlace = false;
		string typeString = (!this.typeA) ? "_B" : "_A";
		base.animator.Play("Climb" + typeString + this.colorString);
		base.GetComponent<SpriteRenderer>().sortingOrder = 2;
		yield return base.StartCoroutine(this.move_up_cr());
		Effect beardPopPrefab = (!this.typeA) ? this.beardPopB : this.beardPopA;
		this.beardPop = beardPopPrefab.Create(new Vector3(base.transform.position.x, -335f));
		yield return null;
		yield return this.beardPop.animator.WaitForAnimationToStart(this, "Pimple_Warning", false);
		AudioManager.Play("sfx_dlc_omm_gnome_groundstretch");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_groundstretch");
		yield return CupheadTime.WaitForSeconds(this, this.warningTime);
		this.CalculateArc();
		this.SetParryable(this.parryable);
		this.coll.enabled = true;
		base.transform.position = new Vector3(base.transform.position.x, -325f);
		base.transform.localScale = new Vector3(Mathf.Sign(this.vel.x), 1f);
		base.animator.Play("Spin" + typeString + this.colorString);
		this.rend.sortingLayerID = SortingLayer.NameToID("Player");
		this.rend.sortingOrder = 50;
		AudioManager.Play("sfx_dlc_omm_gnome_groundstretchpop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_groundstretchpop");
		AudioManager.Play("sfx_dlc_omm_gnome_somersault_voice");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_somersault_voice");
		AudioManager.Play("sfx_dlc_omm_gnome_somersault");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_somersault");
		this.beardPop.animator.Play("Pimple_End");
		this.groundShadow = true;
		this.currentArcTime = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!finishedArcing)
		{
			this.vel += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
			base.transform.Translate(this.vel * CupheadTime.FixedDelta);
			if (this.rend.sortingOrder == 50 && this.vel.y < 0f)
			{
				this.rend.sortingLayerID = SortingLayer.NameToID("Enemies");
				this.rend.sortingOrder = 4;
			}
			if (this.vel.y < 0f && base.transform.position.y < -289f)
			{
				finishedArcing = true;
				break;
			}
			this.currentArcTime += CupheadTime.FixedDelta;
			yield return wait;
		}
		this.groundShadow = false;
		Vector3 pos = new Vector3(base.transform.position.x, -289f);
		Effect beardHealPrefab = (!this.typeA) ? this.beardHealB : this.beardHealA;
		beardHealPrefab.Create(pos + Vector3.down * 25f);
		this.rend.sortingLayerID = SortingLayer.NameToID("Default");
		this.rend.sortingOrder = 5;
		this.vel.x = 0f;
		this.vel.y = this.vel.y * 0.5f;
		float t = 0f;
		while (t < 0.041666668f && base.transform.position.y > -334f)
		{
			t += CupheadTime.FixedDelta;
			base.transform.Translate(this.vel * CupheadTime.FixedDelta);
			yield return wait;
		}
		this.Respawn();
		yield return null;
		yield break;
	}

	// Token: 0x06002669 RID: 9833 RVA: 0x001669F0 File Offset: 0x00164DF0
	public void Death(bool parried = false)
	{
		if (this.beardPop)
		{
			UnityEngine.Object.Destroy(this.beardPop.gameObject);
		}
		if (base.transform.position.y > this.startPos.y)
		{
			this.deathPuff.Create(base.transform.position);
		}
		if (!parried)
		{
			for (int i = 0; i < this.deathParts.Length; i++)
			{
				if (i != 0 || UnityEngine.Random.Range(0, 10) == 0)
				{
					SpriteDeathParts spriteDeathParts = this.deathParts[i].CreatePart(base.transform.position);
					if (i != 0)
					{
						spriteDeathParts.animator.Play(this.colorString);
					}
				}
			}
			AudioManager.Play("sfx_dlc_omm_gnome_popper_death");
			this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_popper_death");
		}
		AudioManager.Stop("sfx_dlc_omm_gnome_somersault");
		AudioManager.Stop("sfx_dlc_omm_gnome_somersault_voice");
		this.groundShadow = false;
		this.Respawn();
	}

	// Token: 0x0600266A RID: 9834 RVA: 0x00166AF4 File Offset: 0x00164EF4
	private void Respawn()
	{
		this.StopAllCoroutines();
		this.damageReceiver.enabled = false;
		base.transform.position = this.startPos;
		this.inPlace = true;
		this.coll.enabled = false;
	}

	// Token: 0x0600266B RID: 9835 RVA: 0x00166B2C File Offset: 0x00164F2C
	private void LateUpdate()
	{
		if (this.groundShadow)
		{
			this.shadowRend.sortingOrder = 5;
			this.shadowRend.transform.position = new Vector3(base.transform.position.x, -314f + Mathf.Lerp(40f, 60f, this.currentArcTime / (this.apexTime * 2f)));
			if (base.transform.position.y < -314f + this.shadowRange)
			{
				float num = Mathf.Lerp(0f, (float)(this.shadowSprites.Length - 4), Mathf.InverseLerp(-314f, -314f + this.shadowRange, base.transform.position.y));
				this.shadowRend.sprite = this.shadowSprites[(int)num];
			}
			else
			{
				this.shadowRend.sprite = this.shadowSprites[this.shadowSprites.Length - 3 + (int)(this.currentArcTime * 24f) % 3];
			}
		}
		else
		{
			this.shadowRend.sortingOrder = 1;
			this.shadowRend.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x04002EEB RID: 12011
	private const float START_POS_Y = -430f;

	// Token: 0x04002EEC RID: 12012
	private const float JUMP_POS_Y = -325f;

	// Token: 0x04002EED RID: 12013
	private const float LAND_POS_Y = -314f;

	// Token: 0x04002EEE RID: 12014
	private const float LAND_OFFSET = 25f;

	// Token: 0x04002EEF RID: 12015
	private const float SHADOW_OFFSET_START = 40f;

	// Token: 0x04002EF0 RID: 12016
	private const float SHADOW_OFFSET_END = 60f;

	// Token: 0x04002EF1 RID: 12017
	[Header("Death FX")]
	[SerializeField]
	private Effect deathPuff;

	// Token: 0x04002EF2 RID: 12018
	[SerializeField]
	private SpriteDeathParts[] deathParts;

	// Token: 0x04002EF3 RID: 12019
	[Header("Beard FX")]
	[SerializeField]
	private Effect beardPopA;

	// Token: 0x04002EF4 RID: 12020
	[SerializeField]
	private Effect beardPopB;

	// Token: 0x04002EF5 RID: 12021
	[SerializeField]
	private Effect beardHealA;

	// Token: 0x04002EF6 RID: 12022
	[SerializeField]
	private Effect beardHealB;

	// Token: 0x04002EF7 RID: 12023
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04002EF8 RID: 12024
	[SerializeField]
	private Collider2D coll;

	// Token: 0x04002EFA RID: 12026
	private DamageReceiver damageReceiver;

	// Token: 0x04002EFB RID: 12027
	private Vector3 startPos;

	// Token: 0x04002EFC RID: 12028
	private Vector3 vel;

	// Token: 0x04002EFD RID: 12029
	private float gravity;

	// Token: 0x04002EFE RID: 12030
	private float health;

	// Token: 0x04002EFF RID: 12031
	private float apexTime;

	// Token: 0x04002F00 RID: 12032
	private float bulletSpeed;

	// Token: 0x04002F01 RID: 12033
	private float apexheight;

	// Token: 0x04002F02 RID: 12034
	private float currentArcTime;

	// Token: 0x04002F03 RID: 12035
	private float warningTime;

	// Token: 0x04002F04 RID: 12036
	private bool typeA;

	// Token: 0x04002F05 RID: 12037
	private bool parryable;

	// Token: 0x04002F06 RID: 12038
	private string colorString;

	// Token: 0x04002F07 RID: 12039
	private bool isBlue = true;

	// Token: 0x04002F08 RID: 12040
	private Effect beardPop;

	// Token: 0x04002F09 RID: 12041
	[SerializeField]
	private float shadowRange = 100f;

	// Token: 0x04002F0A RID: 12042
	[SerializeField]
	private SpriteRenderer shadowRend;

	// Token: 0x04002F0B RID: 12043
	[SerializeField]
	private Sprite[] shadowSprites;

	// Token: 0x04002F0C RID: 12044
	private bool groundShadow;

	// Token: 0x04002F0D RID: 12045
	[SerializeField]
	private OldManLevelBeardController beardController;

	// Token: 0x04002F0E RID: 12046
	[SerializeField]
	private int rufflePos;
}
