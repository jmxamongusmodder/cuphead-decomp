using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200077F RID: 1919
public class RobotLevelOrb : AbstractProjectile
{
	// Token: 0x06002A24 RID: 10788 RVA: 0x0018A28A File Offset: 0x0018868A
	protected override void Awake()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x0018A2B6 File Offset: 0x001886B6
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.fade_in_cr());
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x0018A2CB File Offset: 0x001886CB
	protected override void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.Update();
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x0018A2E9 File Offset: 0x001886E9
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x0018A307 File Offset: 0x00188707
	protected virtual void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (!this.activeShields)
		{
			this.health -= info.damage;
			if (this.health <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x0018A344 File Offset: 0x00188744
	public override void OnParry(AbstractPlayerController player)
	{
		if (this.activeShields)
		{
			this.lasers.SetActive(false);
			this.activeShields = false;
			this.pinkTop.enabled = false;
			this.pinkBottom.enabled = false;
			this.SetParryable(false);
			AudioManager.Play("robot_orb_death");
			this.emitAudioFromObject.Add("robot_orb_death");
			base.animator.SetTrigger("Continue");
			base.StartCoroutine(this.slide_in_cr());
		}
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x0018A3C8 File Offset: 0x001887C8
	public RobotLevelOrb Create(Vector3 position, Vector3 offsetAfterSpawn)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject, position, Quaternion.identity);
		RobotLevelOrb component = gameObject.GetComponent<RobotLevelOrb>();
		component.offsetAfterSpawn = offsetAfterSpawn;
		return component;
	}

	// Token: 0x06002A2B RID: 10795 RVA: 0x0018A3F8 File Offset: 0x001887F8
	private IEnumerator fade_in_cr()
	{
		base.transform.SetScale(new float?(0.5f), new float?(0.5f), null);
		this.pinkTop.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
		this.pinkBottom.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
		float t = 0f;
		float time = 0.9f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0.5f, 1f, t / time);
			base.transform.SetScale(new float?(val), new float?(val), null);
			this.pinkTop.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / time);
			this.pinkBottom.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / time);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x0018A414 File Offset: 0x00188814
	public void InitOrb(LevelProperties.Robot properties)
	{
		base.transform.position += Vector3.left * (float)properties.CurrentState.orb.orbMovementSpeed * CupheadTime.Delta;
		this.properties = properties;
		if (properties.CurrentState.orb.orbShieldIsActive)
		{
			this.SetParryable(true);
			this.activeShields = true;
			this.pinkTop.enabled = true;
			this.pinkBottom.enabled = true;
		}
		else
		{
			this.activeShields = false;
		}
		this.health = (float)properties.CurrentState.orb.orbHP;
		this.speed = properties.CurrentState.orb.orbMovementSpeed;
		base.transform.right = Vector3.left;
		base.StartCoroutine(this.fade_color_cr());
		base.StartCoroutine(this.lasers_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x0018A513 File Offset: 0x00188913
	public void InitChildOrb(int speed, float health, bool activeShields)
	{
		this.speed = speed;
		this.health = health;
		this.activeShields = activeShields;
		base.StartCoroutine(this.move_cr());
		this.lasers.SetActive(this.lasers.activeSelf);
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x0018A550 File Offset: 0x00188950
	private IEnumerator lasers_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.orb.orbInitalOpenDelay);
		if (!this.activeShields)
		{
			yield break;
		}
		base.StartCoroutine(this.slide_out_cr());
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.orb.orbInitialLaserDelay);
		if (!this.activeShields)
		{
			yield break;
		}
		base.animator.Play("Laser_Start");
		this.lasers.SetActive(true);
		AudioManager.PlayLoop("robot_orb_spark_loop");
		yield return null;
		yield break;
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x0018A56C File Offset: 0x0018896C
	private IEnumerator shields_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.orb.orbSpawnDelay);
		this.activeShields = true;
		yield break;
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x0018A588 File Offset: 0x00188988
	private IEnumerator move_cr()
	{
		for (;;)
		{
			if (base.transform.position.x < this.offsetAfterSpawn.x && base.transform.position.y < this.offsetAfterSpawn.y)
			{
				base.transform.position += Vector3.up * (float)this.speed * CupheadTime.Delta * 0.5f;
			}
			base.transform.position += Vector3.left * (float)this.speed * CupheadTime.Delta;
			if (base.transform.position.x < (float)Level.Current.Left - base.GetComponents<BoxCollider2D>()[0].size.x / 2f)
			{
				AudioManager.Stop("robot_orb_spark_loop");
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x0018A5A4 File Offset: 0x001889A4
	private IEnumerator slide_out_cr()
	{
		float sizeY = base.GetComponent<Collider2D>().bounds.size.y;
		float localPosTop = this.top.transform.localPosition.y + sizeY / 4f;
		float localPosBottom = this.bottom.transform.localPosition.y - sizeY / 4f;
		Vector3 topPos = this.top.transform.localPosition;
		Vector3 bottomPos = this.bottom.transform.localPosition;
		float time = 0.5f;
		float t = 0f;
		if (this.activeShields)
		{
			this.wasActive = true;
			AudioManager.Play("robot_orb_spark_start");
			base.animator.Play("Sparks_Start");
		}
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			topPos.y = Mathf.Lerp(0f, localPosTop, val);
			bottomPos.y = Mathf.Lerp(0f, localPosBottom, val);
			this.top.transform.localPosition = topPos;
			this.bottom.transform.localPosition = bottomPos;
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002A32 RID: 10802 RVA: 0x0018A5C0 File Offset: 0x001889C0
	private IEnumerator slide_in_cr()
	{
		Vector3 topPos = this.top.transform.localPosition;
		Vector3 bottomPos = this.bottom.transform.localPosition;
		float time = 0.5f;
		float t = 0f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			topPos.y = Mathf.Lerp(topPos.y, 0f, val);
			bottomPos.y = Mathf.Lerp(bottomPos.y, 0f, val);
			this.top.transform.localPosition = topPos;
			this.bottom.transform.localPosition = bottomPos;
			t += CupheadTime.Delta;
			yield return null;
		}
		if (this.wasActive)
		{
			AudioManager.Stop("robot_orb_spark_loop");
			base.animator.Play("Sparks_End");
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x0018A5DC File Offset: 0x001889DC
	protected virtual IEnumerator fade_color_cr()
	{
		float t = 0f;
		float fadeTime = 0.5f;
		while (t < fadeTime)
		{
			if (!this.activeShields)
			{
				this.top.GetComponent<SpriteRenderer>().color = new Color(t / fadeTime, t / fadeTime, t / fadeTime, 1f);
				this.bottom.GetComponent<SpriteRenderer>().color = new Color(t / fadeTime, t / fadeTime, t / fadeTime, 1f);
			}
			else
			{
				this.pinkTop.color = new Color(t / fadeTime, t / fadeTime, t / fadeTime, 1f);
				this.pinkBottom.color = new Color(t / fadeTime, t / fadeTime, t / fadeTime, 1f);
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x04003302 RID: 13058
	[SerializeField]
	private GameObject lasers;

	// Token: 0x04003303 RID: 13059
	[SerializeField]
	private Transform top;

	// Token: 0x04003304 RID: 13060
	[SerializeField]
	private Transform bottom;

	// Token: 0x04003305 RID: 13061
	[SerializeField]
	private SpriteRenderer pinkTop;

	// Token: 0x04003306 RID: 13062
	[SerializeField]
	private SpriteRenderer pinkBottom;

	// Token: 0x04003307 RID: 13063
	private LevelProperties.Robot properties;

	// Token: 0x04003308 RID: 13064
	private DamageReceiver damageReceiver;

	// Token: 0x04003309 RID: 13065
	private bool activeShields;

	// Token: 0x0400330A RID: 13066
	private bool wasActive;

	// Token: 0x0400330B RID: 13067
	private float health;

	// Token: 0x0400330C RID: 13068
	private int speed;

	// Token: 0x0400330D RID: 13069
	private Vector3 offsetAfterSpawn;
}
