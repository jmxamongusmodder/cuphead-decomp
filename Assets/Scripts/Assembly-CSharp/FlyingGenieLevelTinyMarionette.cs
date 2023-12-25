using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200067E RID: 1662
public class FlyingGenieLevelTinyMarionette : AbstractCollidableObject
{
	// Token: 0x06002311 RID: 8977 RVA: 0x00149338 File Offset: 0x00147738
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiver.enabled = false;
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x00149385 File Offset: 0x00147785
	private void FixedUpdate()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x0014939D File Offset: 0x0014779D
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && !this.isDead)
		{
			this.isDead = true;
			this.Die();
		}
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x001493DA File Offset: 0x001477DA
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x001493F8 File Offset: 0x001477F8
	public void Activate(Vector3 endPos, LevelProperties.FlyingGenie.Scan properties, bool movingClockwise)
	{
		this.properties = properties;
		this.hp = properties.miniHP;
		this.isClockwise = movingClockwise;
		base.StartCoroutine(this.tiny_marionette(endPos));
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x00149424 File Offset: 0x00147824
	private IEnumerator bounce_marionette_cr()
	{
		float t = 0f;
		float time = 0.5f;
		float start = base.transform.position.y;
		float end = base.transform.position.y + 100f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutBounce, 0f, 1f, t / time);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x00149440 File Offset: 0x00147840
	private IEnumerator tiny_marionette(Vector3 endPos)
	{
		base.StartCoroutine(this.bounce_marionette_cr());
		yield return base.animator.WaitForAnimationToEnd(this, "Puppet_Intro", false, true);
		this.damageReceiver.enabled = true;
		float t = 0f;
		float time = this.properties.movementSpeed;
		Vector3 start = base.transform.position;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
			base.transform.position = Vector3.Lerp(start, endPos, val);
			t += CupheadTime.Delta;
			yield return new WaitForFixedUpdate();
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.startedDown = Rand.Bool();
		this.turningDown = !this.startedDown;
		base.animator.SetBool("OnTurningDown", this.turningDown);
		string dirString = (!this.startedDown) ? "Up_" : "Down_";
		base.animator.SetTrigger("OnStartCycle");
		base.animator.SetBool("IsDown", this.startedDown);
		this.bulletMainIndex = UnityEngine.Random.Range(0, this.properties.bulletString.Length);
		string[] bulletString = this.properties.bulletString[this.bulletMainIndex].Split(new char[]
		{
			','
		});
		this.bulletIndex = UnityEngine.Random.Range(0, bulletString.Length);
		yield return base.animator.WaitForAnimationToEnd(this, dirString + "Warning_Shoot", false, true);
		for (;;)
		{
			bulletString = this.properties.bulletString[this.bulletMainIndex].Split(new char[]
			{
				','
			});
			yield return CupheadTime.WaitForSeconds(this, this.properties.shootDelay);
			base.animator.SetTrigger("OnShoot");
			yield return base.animator.WaitForAnimationToEnd(this, false);
			if (this.bulletIndex < bulletString.Length - 1)
			{
				this.bulletIndex++;
			}
			else
			{
				this.bulletMainIndex = (this.bulletMainIndex + 1) % this.properties.bulletString.Length;
				this.bulletIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x00149464 File Offset: 0x00147864
	private void ShootBullet(Vector3 pos, float rotation)
	{
		string[] array = this.properties.bulletString[this.bulletMainIndex].Split(new char[]
		{
			','
		});
		if (array[this.bulletIndex][0] == 'P')
		{
			this.pinkProjectile.Create(pos, rotation + 90f, this.properties.bulletSpeed);
		}
		else
		{
			this.projectile.Create(pos, rotation + 90f, this.properties.bulletSpeed);
		}
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x001494F8 File Offset: 0x001478F8
	private void AniEventCheckFlip()
	{
		if (this.hasStarted)
		{
			base.transform.SetScale(new float?(base.transform.localScale.x * -1f), null, null);
			this.turningDown = !this.turningDown;
			base.animator.SetBool("OnTurningDown", this.turningDown);
		}
		else
		{
			this.hasStarted = true;
			if (!this.isClockwise && !this.startedDown)
			{
				base.transform.SetScale(new float?(base.transform.localScale.x * -1f), null, null);
			}
			else if (this.isClockwise && this.startedDown)
			{
				base.transform.SetScale(new float?(base.transform.localScale.x * -1f), null, null);
			}
		}
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x00149624 File Offset: 0x00147A24
	private void AniEventShoot()
	{
		Effect effect = this.shootFX.Create(this.shootRoot.transform.position);
		AudioManager.Play("genie_puppetsmall_shoot");
		this.emitAudioFromObject.Add("genie_puppetsmall_shoot");
		effect.transform.SetEulerAngles(null, null, new float?(this.shootRoot.transform.eulerAngles.z));
		this.ShootBullet(this.shootRoot.transform.position, this.shootRoot.transform.eulerAngles.z);
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x001496CF File Offset: 0x00147ACF
	public void Die()
	{
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("genie_puppetsmall_death");
		this.emitAudioFromObject.Add("genie_puppetsmall_death");
		this.StopAllCoroutines();
		base.StartCoroutine(this.dead_move_cr());
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x00149710 File Offset: 0x00147B10
	private IEnumerator dead_move_cr()
	{
		float t = 0f;
		float timer = 0.5f;
		float downTimer = 0.5f;
		float start = base.transform.position.y;
		float end = 660f;
		float downEnd = base.transform.position.y - 50f;
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		yield return base.animator.WaitForAnimationToEnd(this, "Death_Start", false, true);
		while (t < downTimer)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / downTimer);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, downEnd, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		start = base.transform.position.y;
		while (t < timer)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / timer);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, val2)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x0014972B File Offset: 0x00147B2B
	private void SoundPuppetSmallEnterPuppet()
	{
		AudioManager.Play("genie_puppetsmall_enter_puppetsmall");
		this.emitAudioFromObject.Add("genie_puppetsmall_enter_puppetsmall");
	}

	// Token: 0x0600231E RID: 8990 RVA: 0x00149747 File Offset: 0x00147B47
	private void SoundPuppetSmallDance()
	{
		AudioManager.Play("genie_puppetsmall_move");
		this.emitAudioFromObject.Add("genie_puppetsmall_move");
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x00149763 File Offset: 0x00147B63
	private void SoundPuppetShootWarning()
	{
		AudioManager.Play("genie_puppetsmall_shootwarning");
		this.emitAudioFromObject.Add("genie_puppetsmall_shootwarning");
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x0014977F File Offset: 0x00147B7F
	private void SoundPuppetWarningShot()
	{
		AudioManager.Play("genie_puppetsmall_warningshot");
		this.emitAudioFromObject.Add("genie_puppetsmall_warningshot");
	}

	// Token: 0x04002BB1 RID: 11185
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04002BB2 RID: 11186
	[SerializeField]
	private BasicProjectile pinkProjectile;

	// Token: 0x04002BB3 RID: 11187
	[SerializeField]
	private Effect shootFX;

	// Token: 0x04002BB4 RID: 11188
	[SerializeField]
	private Transform shootRoot;

	// Token: 0x04002BB5 RID: 11189
	private DamageDealer damageDealer;

	// Token: 0x04002BB6 RID: 11190
	private DamageReceiver damageReceiver;

	// Token: 0x04002BB7 RID: 11191
	private float hp;

	// Token: 0x04002BB8 RID: 11192
	private bool turningDown;

	// Token: 0x04002BB9 RID: 11193
	private bool isClockwise;

	// Token: 0x04002BBA RID: 11194
	private bool startedDown;

	// Token: 0x04002BBB RID: 11195
	private bool hasStarted;

	// Token: 0x04002BBC RID: 11196
	private bool isDead;

	// Token: 0x04002BBD RID: 11197
	private int bulletMainIndex;

	// Token: 0x04002BBE RID: 11198
	private int bulletIndex;

	// Token: 0x04002BBF RID: 11199
	private LevelProperties.FlyingGenie.Scan properties;
}
