using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class AirplaneLevelBulldogParachute : LevelProperties.Airplane.Entity
{
	// Token: 0x1700030E RID: 782
	// (get) Token: 0x060013BA RID: 5050 RVA: 0x000AEB31 File Offset: 0x000ACF31
	// (set) Token: 0x060013BB RID: 5051 RVA: 0x000AEB39 File Offset: 0x000ACF39
	public bool isMoving { get; private set; }

	// Token: 0x060013BC RID: 5052 RVA: 0x000AEB42 File Offset: 0x000ACF42
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.pinkString = new PatternString(base.properties.CurrentState.parachute.pinkString, true, true);
	}

	// Token: 0x060013BD RID: 5053 RVA: 0x000AEB77 File Offset: 0x000ACF77
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x000AEB85 File Offset: 0x000ACF85
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x000AEB8E File Offset: 0x000ACF8E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060013C0 RID: 5056 RVA: 0x000AEBAC File Offset: 0x000ACFAC
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060013C1 RID: 5057 RVA: 0x000AEBC4 File Offset: 0x000ACFC4
	public void StartDescent(Vector2 pos, float scale)
	{
		this.isMoving = true;
		base.transform.position = pos;
		base.transform.localScale = new Vector3(scale, 1f);
		this.count = 0;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x000AEC14 File Offset: 0x000AD014
	private IEnumerator move_cr()
	{
		base.animator.Play("Drop");
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "Drop", false, true);
		this.isMoving = false;
		yield break;
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x000AEC30 File Offset: 0x000AD030
	public void EarlyExit()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.early_exit_cr());
		if (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.41379312f)
		{
			base.transform.position = new Vector3(base.transform.position.x, this.collider.transform.localPosition.y + 400f);
			base.animator.Play("Drop", 0, 0.87356323f);
			base.animator.Update(0f);
		}
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x000AECD8 File Offset: 0x000AD0D8
	private IEnumerator early_exit_cr()
	{
		if (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.41379312f)
		{
			base.animator.Play("Drop", 0, 0.87356323f);
		}
		yield return base.animator.WaitForAnimationToStart(this, "None", false);
		this.isMoving = false;
		if (this.main.isDead && this.mainAnimator)
		{
			this.mainAnimator.SetBool("InParachuteATK", false);
		}
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x000AECF3 File Offset: 0x000AD0F3
	private void OnDisable()
	{
		base.GetComponent<HitFlash>().StopAllCoroutinesWithoutSettingScale();
		base.GetComponent<SpriteRenderer>().color = Color.black;
	}

	// Token: 0x060013C6 RID: 5062 RVA: 0x000AED10 File Offset: 0x000AD110
	private void AniEvent_Shoot()
	{
		LevelProperties.Airplane.Parachute parachute = base.properties.CurrentState.parachute;
		Vector3 v = new Vector3((this.count != 1) ? this.spawnRoot1.position.x : this.spawnRoot2.position.x, this.shotYPos[this.count]);
		float delay = (this.count != 0) ? ((this.count != 1) ? parachute.shotCReturnDelay.RandomFloat() : parachute.shotBReturnDelay.RandomFloat()) : parachute.shotAReturnDelay.RandomFloat();
		if (this.pinkString.PopLetter() == 'P')
		{
			this.boomerangPink.Create(v, parachute.speedForward, parachute.easeDistanceForward, parachute.speedReturn, parachute.easeDistanceReturn, delay, base.transform.localScale.x > 0f, 1);
		}
		else
		{
			this.boomerang.Create(v, parachute.speedForward, parachute.easeDistanceForward, parachute.speedReturn, parachute.easeDistanceReturn, delay, base.transform.localScale.x > 0f, 1);
		}
		this.count++;
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_bicepflex");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_bicepflex");
		AudioManager.Play("sfx_dlc_dogfight_dogflexhugovocal");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_dogflexhugovocal");
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x000AEEA6 File Offset: 0x000AD2A6
	private void AniEvent_SFX_BulldogPlane_ParachuteEnd()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_SpringsUp");
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x000AEEB4 File Offset: 0x000AD2B4
	private void WORKAROUND_NullifyFields()
	{
		this.shotYPos = null;
		this.spawnRoot1 = null;
		this.spawnRoot2 = null;
		this.boomerang = null;
		this.boomerangPink = null;
		this.pinkString = null;
		this.damageDealer = null;
		this.main = null;
		this.mainAnimator = null;
		this.collider = null;
	}

	// Token: 0x04001CCE RID: 7374
	private float[] shotYPos = new float[]
	{
		100f,
		-50f,
		-200f
	};

	// Token: 0x04001CCF RID: 7375
	[SerializeField]
	private Transform spawnRoot1;

	// Token: 0x04001CD0 RID: 7376
	[SerializeField]
	private Transform spawnRoot2;

	// Token: 0x04001CD1 RID: 7377
	[SerializeField]
	private AirplaneLevelBoomerang boomerang;

	// Token: 0x04001CD2 RID: 7378
	[SerializeField]
	private AirplaneLevelBoomerang boomerangPink;

	// Token: 0x04001CD3 RID: 7379
	private PatternString pinkString;

	// Token: 0x04001CD4 RID: 7380
	private DamageDealer damageDealer;

	// Token: 0x04001CD5 RID: 7381
	[SerializeField]
	private AirplaneLevelBulldogPlane main;

	// Token: 0x04001CD6 RID: 7382
	[SerializeField]
	private Animator mainAnimator;

	// Token: 0x04001CD7 RID: 7383
	private int count;

	// Token: 0x04001CD8 RID: 7384
	[SerializeField]
	private GameObject collider;
}
