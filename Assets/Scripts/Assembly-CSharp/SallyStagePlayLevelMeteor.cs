using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007B1 RID: 1969
public class SallyStagePlayLevelMeteor : AbstractProjectile
{
	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06002C46 RID: 11334 RVA: 0x001A08D4 File Offset: 0x0019ECD4
	// (set) Token: 0x06002C47 RID: 11335 RVA: 0x001A08DC File Offset: 0x0019ECDC
	public float spawnPosition { get; private set; }

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06002C48 RID: 11336 RVA: 0x001A08E5 File Offset: 0x0019ECE5
	// (set) Token: 0x06002C49 RID: 11337 RVA: 0x001A08ED File Offset: 0x0019ECED
	public SallyStagePlayLevelMeteor.State state { get; private set; }

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06002C4A RID: 11338 RVA: 0x001A08F6 File Offset: 0x0019ECF6
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06002C4B RID: 11339 RVA: 0x001A08FD File Offset: 0x0019ECFD
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002C4C RID: 11340 RVA: 0x001A0904 File Offset: 0x0019ED04
	public SallyStagePlayLevelMeteor Create(float pos, float hp, LevelProperties.SallyStagePlay.Meteor properties)
	{
		SallyStagePlayLevelMeteor sallyStagePlayLevelMeteor = base.Create() as SallyStagePlayLevelMeteor;
		sallyStagePlayLevelMeteor.properties = properties;
		sallyStagePlayLevelMeteor.spawnPosition = pos;
		sallyStagePlayLevelMeteor.hp = hp;
		return sallyStagePlayLevelMeteor;
	}

	// Token: 0x06002C4D RID: 11341 RVA: 0x001A0934 File Offset: 0x0019ED34
	protected override void Awake()
	{
		base.Awake();
		this.star.OnActivate += this.ParryStar;
		this.star.GetComponent<Collider2D>().enabled = false;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002C4E RID: 11342 RVA: 0x001A0992 File Offset: 0x0019ED92
	protected override void Start()
	{
		base.Start();
		base.transform.position = new Vector2(-640f + this.spawnPosition, 360f);
		base.StartCoroutine(this.move_down_cr());
	}

	// Token: 0x06002C4F RID: 11343 RVA: 0x001A09CD File Offset: 0x0019EDCD
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002C50 RID: 11344 RVA: 0x001A09EB File Offset: 0x0019EDEB
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f && this.state == SallyStagePlayLevelMeteor.State.Meteor)
		{
			this.state = SallyStagePlayLevelMeteor.State.Hook;
			this.OnMeteorDie();
		}
	}

	// Token: 0x06002C51 RID: 11345 RVA: 0x001A0A28 File Offset: 0x0019EE28
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.state == SallyStagePlayLevelMeteor.State.Meteor && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002C52 RID: 11346 RVA: 0x001A0A54 File Offset: 0x0019EE54
	private IEnumerator move_down_cr()
	{
		AudioManager.Play("sally_meteor_ascend_decend");
		this.emitAudioFromObject.Add("sally_meteor_ascend_decend");
		this.state = SallyStagePlayLevelMeteor.State.Meteor;
		while (base.transform.position.y > (float)Level.Current.Ground + 100f)
		{
			base.transform.position -= base.transform.up * this.properties.meteorSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002C53 RID: 11347 RVA: 0x001A0A70 File Offset: 0x0019EE70
	private IEnumerator move_up_cr()
	{
		AudioManager.Play("sally_meteor_ascend_decend");
		this.emitAudioFromObject.Add("sally_meteor_ascend_decend");
		while (this.star.transform.position.y < 360f - this.properties.hookMaxHeight)
		{
			this.star.transform.position += this.star.transform.up * this.properties.meteorSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x001A0A8C File Offset: 0x0019EE8C
	private IEnumerator leave_cr()
	{
		while (this.star.transform.position.y < 460f)
		{
			this.star.transform.position += this.star.transform.up * this.properties.meteorSpeed * CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06002C55 RID: 11349 RVA: 0x001A0AA8 File Offset: 0x0019EEA8
	private IEnumerator leave_all_cr()
	{
		while (base.transform.position.y < 460f)
		{
			base.transform.position += base.transform.up * this.properties.meteorSpeed * CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06002C56 RID: 11350 RVA: 0x001A0AC4 File Offset: 0x0019EEC4
	private IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.hookParryExitDelay);
		base.StartCoroutine(this.leave_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002C57 RID: 11351 RVA: 0x001A0AE0 File Offset: 0x0019EEE0
	private void OnMeteorDie()
	{
		this.star.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<Animator>().SetTrigger("OpenMeteor");
		AudioManager.Play("sally_meteor_open");
		this.emitAudioFromObject.Add("sally_meteor_open");
		this.damageReceiver.enabled = false;
		base.StartCoroutine(this.move_up_cr());
		base.StartCoroutine(this.slide_meteor_cr());
	}

	// Token: 0x06002C58 RID: 11352 RVA: 0x001A0B5C File Offset: 0x0019EF5C
	public void ParryStar()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null && !player2.IsDead && !player.IsDead && this.parryCounter < 1)
		{
			this.parryCounter++;
			return;
		}
		base.GetComponent<Animator>().SetTrigger("SpinStar");
		this.state = SallyStagePlayLevelMeteor.State.Leaving;
		base.StartCoroutine(this.leave_cr());
		this.star.StartParryCooldown();
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x001A0BE4 File Offset: 0x0019EFE4
	protected override void Die()
	{
		base.Die();
		this.state = SallyStagePlayLevelMeteor.State.Leaving;
		this.spawnPosition = 0f;
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.enabled = false;
		}
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x001A0C47 File Offset: 0x0019F047
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		if (hit.GetComponent<SallyStagePlayLevelWave>())
		{
			base.StartCoroutine(this.leave_all_cr());
		}
	}

	// Token: 0x06002C5B RID: 11355 RVA: 0x001A0C70 File Offset: 0x0019F070
	private IEnumerator slide_meteor_cr()
	{
		float t = 0f;
		float time = 1f;
		Vector3 start = this.meteor.transform.position;
		Vector3 end = new Vector3(this.meteor.transform.position.x, this.meteor.transform.position.y + 700f);
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
			this.meteor.transform.position = Vector3.Lerp(start, end, val);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C5C RID: 11356 RVA: 0x001A0C8B File Offset: 0x0019F08B
	public void MeteorChangePhase()
	{
		base.StartCoroutine(this.change_phase_cr());
	}

	// Token: 0x06002C5D RID: 11357 RVA: 0x001A0C9C File Offset: 0x0019F09C
	private IEnumerator change_phase_cr()
	{
		AudioManager.Play("sally_meteor_ascend_decend");
		this.emitAudioFromObject.Add("sally_meteor_ascend_decend");
		this.state = SallyStagePlayLevelMeteor.State.Meteor;
		while (base.transform.position.y < (float)Level.Current.Ceiling + 100f)
		{
			base.transform.position += base.transform.up * this.properties.meteorSpeed * CupheadTime.Delta;
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040034ED RID: 13549
	[SerializeField]
	private GameObject meteor;

	// Token: 0x040034EE RID: 13550
	[SerializeField]
	private ParrySwitch star;

	// Token: 0x040034F0 RID: 13552
	private DamageReceiver damageReceiver;

	// Token: 0x040034F1 RID: 13553
	private LevelProperties.SallyStagePlay.Meteor properties;

	// Token: 0x040034F2 RID: 13554
	private float hp;

	// Token: 0x040034F3 RID: 13555
	private int parryCounter;

	// Token: 0x020007B2 RID: 1970
	public enum State
	{
		// Token: 0x040034F5 RID: 13557
		Meteor,
		// Token: 0x040034F6 RID: 13558
		Hook,
		// Token: 0x040034F7 RID: 13559
		Leaving
	}
}
