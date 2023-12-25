using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CF RID: 2255
public class HarbourPlatformingLevelOctopus : PlatformingLevelAutoscrollObject
{
	// Token: 0x060034B9 RID: 13497 RVA: 0x001EA8EC File Offset: 0x001E8CEC
	protected override void Awake()
	{
		base.Awake();
		this.anchor.OnActivate += this.Switched;
		this.yPosStart = base.transform.position.y;
		this.collisionChild.OnPlayerProjectileCollision += this.OnCollisionPlayerProjectile;
		this.collisionChild.OnAnyCollision += this.OnCollision;
		this.checkToLock = false;
		this.pinkGem.SetActive(true);
		base.StartCoroutine(this.gem_shine_switch_cr());
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x001EA97F File Offset: 0x001E8D7F
	protected override void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayerProjectile(hit, phase);
		if (!this.tuckedDown)
		{
			this.timeSinceShot = 0f;
			base.animator.SetTrigger("PlayerShooting");
		}
	}

	// Token: 0x060034BB RID: 13499 RVA: 0x001EA9B0 File Offset: 0x001E8DB0
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<HarbourPlatformingLevelIceberg>())
		{
			if (!this.tuckedDown)
			{
				base.StartCoroutine(this.disable_cr());
			}
			hit.GetComponent<HarbourPlatformingLevelIceberg>().DeathParts();
			UnityEngine.Object.Destroy(hit.gameObject);
		}
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x001EAA03 File Offset: 0x001E8E03
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x001EAA10 File Offset: 0x001E8E10
	protected override void Update()
	{
		base.Update();
		CupheadLevelCamera cupheadLevelCamera = CupheadLevelCamera.Current;
		float num = cupheadLevelCamera.autoScrollSpeedMultiplier;
		this.timeSinceShot += CupheadTime.Delta;
		if (this.tuckedDown || this.timeSinceShot > this.holdSpeedTime)
		{
			num -= CupheadTime.Delta * (this.speedupMultiplier - 1f) / this.deccelerationTime;
			num = Mathf.Max(1f, num);
		}
		else
		{
			num += CupheadTime.Delta * (this.speedupMultiplier - 1f) / this.accelerationTime;
			num = Mathf.Min(this.speedupMultiplier, num);
		}
		cupheadLevelCamera.SetAutoscrollSpeedMultiplier(num);
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			base.animator.speed = num;
		}
		else
		{
			base.animator.speed = 1f;
		}
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x001EAB0C File Offset: 0x001E8F0C
	private void Switched()
	{
		this.pinkGem.SetActive(false);
		if (this.firstSwitch)
		{
			base.animator.SetTrigger("StartOctopus");
			this.StartAutoscroll();
			base.StartCoroutine(this.start_octopus_cr());
			this.firstSwitch = false;
		}
		else
		{
			base.animator.SetTrigger("Shoot");
			this.ShootSFX();
		}
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x001EAB75 File Offset: 0x001E8F75
	public bool Started()
	{
		return base.isMoving;
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x001EAB80 File Offset: 0x001E8F80
	private IEnumerator start_octopus_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.transform.position.x + this.scrollMinMax.min)
		{
			yield return null;
		}
		CupheadLevelCamera.Current.OffsetCamera(true, true);
		base.StartCoroutine(this.idle_bounce_cr());
		base.animator.SetTrigger("StartTentacles");
		this.IdleTentaclesSFX();
		base.transform.parent = CupheadLevelCamera.Current.transform;
		yield return null;
		yield break;
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x001EAB9C File Offset: 0x001E8F9C
	private IEnumerator disable_cr()
	{
		this.HeadSquishSFX();
		base.animator.SetBool("IsHit", true);
		this.tuckedDown = true;
		float endPos = base.transform.position.y - 500f;
		float speed = 300f;
		Vector3 pos = base.transform.position;
		while (base.transform.position.y > endPos)
		{
			base.transform.AddPosition(0f, -speed * CupheadTime.FixedDelta, 0f);
			yield return null;
		}
		pos = base.transform.position;
		yield return CupheadTime.WaitForSeconds(this, this.tuckDownDelay);
		yield return null;
		while (base.transform.position.y < this.yPosStart)
		{
			base.transform.AddPosition(0f, speed * CupheadTime.FixedDelta, 0f);
			yield return null;
		}
		base.transform.position = new Vector3(base.transform.position.x, this.yPosStart);
		base.animator.SetBool("IsHit", false);
		this.HeadSquishSFX();
		this.tuckedDown = false;
		yield return null;
		yield break;
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x001EABB8 File Offset: 0x001E8FB8
	private IEnumerator end_octopus_cr()
	{
		this.MoveLoop();
		base.animator.SetTrigger("EndOctopus");
		base.transform.parent = null;
		float endPos = base.transform.position.y - 1000f;
		float speed = 100f;
		while (base.transform.position.y > endPos)
		{
			base.transform.AddPosition(0f, -speed * CupheadTime.FixedDelta, 0f);
			yield return null;
		}
		CupheadLevelCamera.Current.OffsetCamera(false, true);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x001EABD3 File Offset: 0x001E8FD3
	protected override void EndAutoscroll()
	{
		base.StartCoroutine(this.end_octopus_cr());
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x001EABE4 File Offset: 0x001E8FE4
	private void Shoot()
	{
		this.ShootSFX();
		this.anchor.enabled = false;
		this.puff.Create(this.projectileRoot.transform.position);
		this.projectile.Create(this.projectileRoot.transform.position);
		base.StartCoroutine(this.gem_timer_cr());
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x001EAC50 File Offset: 0x001E9050
	private IEnumerator gem_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.gemOffTime);
		this.pinkGem.SetActive(true);
		this.anchor.enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x001EAC6C File Offset: 0x001E906C
	private IEnumerator gem_shine_switch_cr()
	{
		string order = "A1,B1,B2,A2,B1,A1,B2,A2";
		int orderIndex = 0;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.42f, 0.67f));
			base.animator.Play("Shine_" + order.Split(new char[]
			{
				','
			})[orderIndex], 1);
			orderIndex = (orderIndex + 1) % order.Split(new char[]
			{
				','
			}).Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x001EAC88 File Offset: 0x001E9088
	private void TentacleBackSwitch()
	{
		Vector3 localPosition = this.tentacleBack.localPosition;
		localPosition.x = ((!this.moveTentacles) ? (this.tentacleBack.localPosition.x + this.tentacleOffset) : (this.tentacleBack.localPosition.x - this.tentacleOffset));
		this.tentacleBack.localPosition = localPosition;
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x001EACF8 File Offset: 0x001E90F8
	private void TentacleFrontSwitch()
	{
		this.moveTentacles = !this.moveTentacles;
		Vector3 localPosition = this.tentacleFront.localPosition;
		localPosition.x = ((!this.moveTentacles) ? (this.tentacleFront.localPosition.x + this.tentacleOffset) : (this.tentacleFront.localPosition.x - this.tentacleOffset));
		this.tentacleFront.localPosition = localPosition;
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x001EAD78 File Offset: 0x001E9178
	private IEnumerator idle_bounce_cr()
	{
		float angle = 0f;
		float yVelocity = 7f;
		float sinSize = 2f;
		for (;;)
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && CupheadTime.Delta != 0f)
			{
				angle += yVelocity * CupheadTime.Delta;
				Vector3 moveY = new Vector3(0f, Mathf.Sin(angle) * sinSize);
				base.transform.localPosition += moveY;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x001EAD93 File Offset: 0x001E9193
	private void HeadSquishSFX()
	{
		AudioManager.Play("harbour_octopus_head_squish");
		this.emitAudioFromObject.Add("harbour_octopus_head_squish");
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x001EADAF File Offset: 0x001E91AF
	private void ShootSFX()
	{
		AudioManager.Play("harbour_octopus_shoot");
		this.emitAudioFromObject.Add("harbour_octopus_shoot");
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x001EADCB File Offset: 0x001E91CB
	private void IdleTentaclesSFX()
	{
		AudioManager.Stop("harbour_octopus_move_loop");
		AudioManager.PlayLoop("harbour_octopus_idle_tentacles");
		this.emitAudioFromObject.Add("harbour_octopus_idle_tentacles");
	}

	// Token: 0x060034CD RID: 13517 RVA: 0x001EADF1 File Offset: 0x001E91F1
	private void MoveLoop()
	{
		AudioManager.Stop("harbour_octopus_idle_tentacles");
		AudioManager.PlayLoop("harbour_octopus_move_loop");
		this.emitAudioFromObject.Add("harbour_octopus_move_loop");
	}

	// Token: 0x060034CE RID: 13518 RVA: 0x001EAE17 File Offset: 0x001E9217
	private void RideStartSFX()
	{
		AudioManager.Stop("harbour_octopus_idle_tentacles");
		AudioManager.Stop("harbour_octopus_move_loop");
		AudioManager.Play("harbour_octopus_ride_start");
		this.emitAudioFromObject.Add("harbour_octopus_ride_start");
	}

	// Token: 0x060034CF RID: 13519 RVA: 0x001EAE47 File Offset: 0x001E9247
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.puff = null;
		this.projectile = null;
	}

	// Token: 0x04003CE5 RID: 15589
	[SerializeField]
	private Effect puff;

	// Token: 0x04003CE6 RID: 15590
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04003CE7 RID: 15591
	[SerializeField]
	private Transform tentacleFront;

	// Token: 0x04003CE8 RID: 15592
	[SerializeField]
	private Transform tentacleBack;

	// Token: 0x04003CE9 RID: 15593
	[SerializeField]
	private ParrySwitch anchor;

	// Token: 0x04003CEA RID: 15594
	[SerializeField]
	private HarbourPlatformingLevelOctoProjectile projectile;

	// Token: 0x04003CEB RID: 15595
	[SerializeField]
	private MinMax scrollMinMax = new MinMax(-300f, 200f);

	// Token: 0x04003CEC RID: 15596
	[SerializeField]
	private GameObject pinkGem;

	// Token: 0x04003CED RID: 15597
	[SerializeField]
	private CollisionChild collisionChild;

	// Token: 0x04003CEE RID: 15598
	[SerializeField]
	private float accelerationTime = 0.3f;

	// Token: 0x04003CEF RID: 15599
	[SerializeField]
	private float holdSpeedTime = 2f;

	// Token: 0x04003CF0 RID: 15600
	[SerializeField]
	private float deccelerationTime = 1f;

	// Token: 0x04003CF1 RID: 15601
	[SerializeField]
	private float speedupMultiplier = 1.2f;

	// Token: 0x04003CF2 RID: 15602
	[SerializeField]
	private float tuckDownDelay = 2f;

	// Token: 0x04003CF3 RID: 15603
	[SerializeField]
	private float gemOffTime = 0.7f;

	// Token: 0x04003CF4 RID: 15604
	private bool firstSwitch = true;

	// Token: 0x04003CF5 RID: 15605
	private float timeSinceShot = 1000f;

	// Token: 0x04003CF6 RID: 15606
	private bool tuckedDown;

	// Token: 0x04003CF7 RID: 15607
	private bool moveTentacles;

	// Token: 0x04003CF8 RID: 15608
	private float tentacleOffset = 100f;

	// Token: 0x04003CF9 RID: 15609
	private float yPosStart;

	// Token: 0x04003CFA RID: 15610
	private const float LOCK_DISTANCE = 600f;
}
