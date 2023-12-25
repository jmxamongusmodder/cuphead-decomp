using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200062F RID: 1583
public class FlyingBlimpLevelBlimpLady : LevelProperties.FlyingBlimp.Entity
{
	// Token: 0x17000381 RID: 897
	// (get) Token: 0x0600203D RID: 8253 RVA: 0x00128520 File Offset: 0x00126920
	// (set) Token: 0x0600203E RID: 8254 RVA: 0x00128528 File Offset: 0x00126928
	public FlyingBlimpLevelBlimpLady.State state { get; private set; }

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x0600203F RID: 8255 RVA: 0x00128531 File Offset: 0x00126931
	// (set) Token: 0x06002040 RID: 8256 RVA: 0x00128539 File Offset: 0x00126939
	public bool moving { get; private set; }

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06002041 RID: 8257 RVA: 0x00128542 File Offset: 0x00126942
	// (set) Token: 0x06002042 RID: 8258 RVA: 0x0012854A File Offset: 0x0012694A
	public bool fading { get; private set; }

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x06002043 RID: 8259 RVA: 0x00128554 File Offset: 0x00126954
	// (remove) Token: 0x06002044 RID: 8260 RVA: 0x0012858C File Offset: 0x0012698C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x06002045 RID: 8261 RVA: 0x001285C4 File Offset: 0x001269C4
	protected override void Awake()
	{
		base.Awake();
		this.state = FlyingBlimpLevelBlimpLady.State.Intro;
		this.pivotOffset = Vector3.up * 2f * this.loopSize;
		this.pivotPoint.position = base.transform.position;
		this.startPos = base.transform.position;
		this.ResetPivotPos(base.transform.position);
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.constellationHandler.color = new Color(1f, 1f, 1f, 0f);
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x00128688 File Offset: 0x00126A88
	public override void LevelInit(LevelProperties.FlyingBlimp properties)
	{
		base.LevelInit(properties);
		this.originalSpeed = properties.CurrentState.move.pathSpeed;
		this.moving = true;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x001286BB File Offset: 0x00126ABB
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
	}

	// Token: 0x06002048 RID: 8264 RVA: 0x001286C4 File Offset: 0x00126AC4
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != FlyingBlimpLevelBlimpLady.State.Death)
		{
			this.state = FlyingBlimpLevelBlimpLady.State.Death;
			this.StartDeath();
		}
	}

	// Token: 0x06002049 RID: 8265 RVA: 0x00128710 File Offset: 0x00126B10
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600204A RID: 8266 RVA: 0x00128728 File Offset: 0x00126B28
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600204B RID: 8267 RVA: 0x00128748 File Offset: 0x00126B48
	private void ResetPivotPos(Vector3 newPos)
	{
		this.pivotPoint.position = newPos;
		Vector3 position = base.transform.position;
		position.y = newPos.y + this.loopSize;
		base.transform.position = position;
	}

	// Token: 0x0600204C RID: 8268 RVA: 0x00128790 File Offset: 0x00126B90
	private IEnumerator intro_cr()
	{
		AudioManager.Play("level_flying_blimp_intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_End", false, true);
		AudioManager.PlayLoop("level_flying_blimp_pedal_loop");
		base.StartCoroutine(this.move_cr());
		while (this.movementSpeed < this.originalSpeed)
		{
			this.movementSpeed += 0.2f;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.move.initalAttackDelayRange.RandomFloat());
		this.state = FlyingBlimpLevelBlimpLady.State.Idle;
		yield break;
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x001287AC File Offset: 0x00126BAC
	private IEnumerator move_cr()
	{
		this.angle = ((!Rand.Bool()) ? 0f : 6.2831855f);
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (this.moving)
			{
				this.PathMovement();
				yield return wait;
			}
			else
			{
				yield return wait;
			}
		}
		yield break;
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x001287C8 File Offset: 0x00126BC8
	public void PathMovement()
	{
		this.angle += this.movementSpeed * CupheadTime.FixedDelta;
		if (this.angle > 6.2831855f)
		{
			this.invert = !this.invert;
			this.angle -= 6.2831855f;
		}
		if (this.angle < 0f)
		{
			this.angle += 6.2831855f;
		}
		float num;
		if (this.invert)
		{
			base.transform.position = this.pivotPoint.position + this.pivotOffset;
			num = -1f;
		}
		else
		{
			base.transform.position = this.pivotPoint.position;
			num = 1f;
		}
		Vector3 b = new Vector3(-Mathf.Sin(this.angle) * this.loopSize, Mathf.Cos(this.angle) * num * this.loopSize, 0f);
		base.transform.position += b;
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x001288DE File Offset: 0x00126CDE
	private void ChangeMat(Material mat)
	{
		base.GetComponent<SpriteRenderer>().material = mat;
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x001288EC File Offset: 0x00126CEC
	public void StartDash()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.dash_cr());
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x00128918 File Offset: 0x00126D18
	private IEnumerator dash_cr()
	{
		bool startedClouds = false;
		this.smallClouds = true;
		this.transitionToSummon = true;
		Animator constAnimator = this.constellationHandler.GetComponent<Animator>();
		this.state = FlyingBlimpLevelBlimpLady.State.Dash;
		LevelProperties.FlyingBlimp.DashSummon p = base.properties.CurrentState.dashSummon;
		string[] pattern = p.patternString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.moving = false;
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i][0] == 'D')
			{
				float waitTime = 0f;
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
				yield return CupheadTime.WaitForSeconds(this, waitTime);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, 0.1f);
				AudioManager.Stop("level_flying_blimp_pedal_loop");
				AudioManager.Play("level_flying_blimp_inhale");
				base.animator.Play("Dash_Start");
				yield return base.animator.WaitForAnimationToEnd(this, "Dash_Start", false, true);
				yield return CupheadTime.WaitForSeconds(this, p.hold);
				AudioManager.Play("level_flying_blimp_exhale");
				base.animator.SetBool("Deflate", true);
				yield return CupheadTime.WaitForSeconds(this, 0.2f);
				this.fading = true;
				base.StartCoroutine(this.fade_constellation_cr(true));
				AudioManager.Play("level_flying_blimp_lady_constellation_loop");
				switch (this.constellation)
				{
				case FlyingBlimpLevelBlimpLady.constellationPossibility.Taurus:
					constAnimator.Play("Taurus");
					break;
				case FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius:
					constAnimator.Play("Sagittarius");
					break;
				case FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini:
					constAnimator.Play("Gemini");
					break;
				}
				base.animator.SetTrigger("Move");
				while (base.transform.position.x >= -1280f)
				{
					if (this.state != FlyingBlimpLevelBlimpLady.State.Death)
					{
						base.transform.position += base.transform.right * -p.dashSpeed * CupheadTime.Delta;
					}
					yield return null;
				}
				this.dashExplosions = false;
				base.animator.SetTrigger("ToOff");
				yield return CupheadTime.WaitForSeconds(this, p.reeentryDelay);
				base.animator.SetTrigger("StartSummon");
				AudioManager.PlayLoop("level_flying_blimp_pedal_loop");
				AudioManager.Play("level_flying_blimp_lady_constellation_transform");
				Vector3 endPos = this.startPos;
				if (this.constellation == FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini)
				{
					endPos.x = this.startPos.x + 100f;
					this.ResetPivotPos(endPos);
				}
				Vector3 pos = base.transform.position;
				pos.y = this.startPos.y;
				base.transform.position = pos;
				while (base.transform.position.x <= endPos.x)
				{
					if (base.transform.position.x >= this.transformationPoint.position.x && !startedClouds)
					{
						this.fading = false;
						base.StartCoroutine(this.fade_constellation_cr(false));
						base.StartCoroutine(this.spawn_clouds_cr());
						startedClouds = true;
					}
					if (this.state != FlyingBlimpLevelBlimpLady.State.Death)
					{
						base.transform.position += base.transform.right * p.summonSpeed * CupheadTime.FixedDelta;
					}
					yield return new WaitForFixedUpdate();
				}
				if (base.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Generic)
				{
					this.transitionToSummon = false;
				}
				else
				{
					this.transitionToSummon = true;
				}
				AudioManager.Stop("level_flying_blimp_pedal_loop");
				base.animator.Play("Big_Cloud");
				this.smallClouds = false;
				base.animator.SetBool("Deflate", false);
			}
		}
		yield break;
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x00128934 File Offset: 0x00126D34
	private IEnumerator select_constellation_cr()
	{
		if (this.transitionToSummon)
		{
			switch (this.constellation)
			{
			case FlyingBlimpLevelBlimpLady.constellationPossibility.Taurus:
				this.ToTaurus();
				base.animator.Play("Taurus_Idle");
				this.ChangeMat(this.taurusMat);
				break;
			case FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius:
				this.ToSagittarius();
				base.animator.Play("Sag_Cloud");
				base.animator.Play("Sagittarius_Idle");
				this.ChangeMat(this.sagittariusMat);
				break;
			case FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini:
				this.ToGemini();
				base.animator.Play("Gemini");
				base.animator.Play("Sphere_Idle");
				this.ChangeMat(this.geminiMat);
				break;
			}
			this.transitionToSummon = false;
		}
		else
		{
			this.ResetPivotPos(this.startPos);
			AudioManager.Play("level_flying_blimp_lady_constellation_transform_end");
			base.animator.Play("Appear");
			this.ChangeMat(this.blimpMat);
			if (this.constellation == FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius)
			{
				base.animator.Play("Sag_Off");
			}
			yield return base.animator.WaitForAnimationToEnd(this, "Appear", false, true);
			this.moving = true;
			AudioManager.PlayLoop("level_flying_blimp_pedal_loop");
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.dashSummon.summonHesitate);
			this.state = FlyingBlimpLevelBlimpLady.State.Idle;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x00128950 File Offset: 0x00126D50
	private IEnumerator check_state_cr(LevelProperties.FlyingBlimp.States currentState)
	{
		this.isLooping = true;
		while (base.properties.CurrentState.stateName == currentState)
		{
			yield return null;
		}
		this.isLooping = false;
		this.waitLoopTime = 0f;
		yield break;
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x00128972 File Offset: 0x00126D72
	private void StartSmoke()
	{
		base.animator.Play("Dash_Smoke");
		this.dashExplosions = true;
		base.StartCoroutine(this.spawn_explosions_cr());
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x00128998 File Offset: 0x00126D98
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.explosionOffset, this.explosionRadius);
	}

	// Token: 0x06002056 RID: 8278 RVA: 0x001289D8 File Offset: 0x00126DD8
	private IEnumerator spawn_explosions_cr()
	{
		while (this.dashExplosions)
		{
			Effect explosion = UnityEngine.Object.Instantiate<Effect>(this.dashExplosionEffect);
			explosion.transform.position = base.transform.position;
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x06002057 RID: 8279 RVA: 0x001289F4 File Offset: 0x00126DF4
	private IEnumerator spawn_clouds_cr()
	{
		while (this.smallClouds)
		{
			GameObject cloud = UnityEngine.Object.Instantiate<GameObject>(this.cloudEffect);
			Vector3 scale = new Vector3(1f, 1f, 1f);
			scale.x = ((!Rand.Bool()) ? (-scale.x) : scale.x);
			scale.y = ((!Rand.Bool()) ? (-scale.y) : scale.y);
			cloud.transform.SetScale(new float?(scale.x), new float?(scale.y), new float?(1f));
			cloud.transform.eulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(0f, 360f));
			cloud.GetComponent<SpriteRenderer>().sortingOrder = UnityEngine.Random.Range(0, 3);
			cloud.transform.position = this.GetRandomPoint();
			base.StartCoroutine(this.delete_cloud_cr(cloud));
			yield return CupheadTime.WaitForSeconds(this, 0.05f);
		}
		yield break;
	}

	// Token: 0x06002058 RID: 8280 RVA: 0x00128A10 File Offset: 0x00126E10
	private IEnumerator delete_cloud_cr(GameObject cloud)
	{
		yield return cloud.GetComponent<Animator>().WaitForAnimationToEnd(this, "Cloud", false, true);
		UnityEngine.Object.Destroy(cloud);
		yield break;
	}

	// Token: 0x06002059 RID: 8281 RVA: 0x00128A34 File Offset: 0x00126E34
	private Vector2 GetRandomPoint()
	{
		Vector2 a = base.transform.position + this.explosionOffset;
		Vector2 vector = new Vector2((float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1));
		Vector2 b = vector.normalized * (this.explosionRadius * UnityEngine.Random.value) * 2f;
		return a + b;
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x00128AA0 File Offset: 0x00126EA0
	private IEnumerator fade_constellation_cr(bool fadeIn)
	{
		float fadeTime = 0.5f;
		float blackMaxFade = 0.25f;
		float blackMidFade = 0.13f;
		float blackCurrentFade = 0f;
		if (fadeIn)
		{
			float t = 0f;
			while (t < fadeTime)
			{
				this.constellationHandler.color = new Color(1f, 1f, 1f, t / fadeTime);
				if (blackCurrentFade < blackMaxFade)
				{
					this.blackDim.color = new Color(0f, 0f, 0f, blackCurrentFade + t / 3f);
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			this.constellationHandler.color = new Color(1f, 1f, 1f, 1f);
			this.blackDim.color = new Color(0f, 0f, 0f, blackMaxFade);
		}
		else
		{
			float t2 = 0f;
			blackCurrentFade = blackMaxFade;
			while (t2 < fadeTime)
			{
				this.constellationHandler.color = new Color(1f, 1f, 1f, 1f - t2 / fadeTime);
				if (blackCurrentFade > blackMidFade)
				{
					this.blackDim.color = new Color(0f, 0f, 0f, blackCurrentFade - t2 / 3f);
				}
				t2 += CupheadTime.Delta;
				yield return null;
			}
			this.constellationHandler.color = new Color(1f, 1f, 1f, 0f);
			this.blackDim.color = new Color(0f, 0f, 0f, blackMidFade);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x00128AC4 File Offset: 0x00126EC4
	private IEnumerator final_fade_cr()
	{
		float fadeTime = 0.5f;
		float blackMidFade = 0.13f;
		float t = 0f;
		while (t < fadeTime)
		{
			this.blackDim.color = new Color(0f, 0f, 0f, blackMidFade - t / 3f);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.constellationHandler.color = new Color(1f, 1f, 1f, 0f);
		yield return null;
		yield break;
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x00128ADF File Offset: 0x00126EDF
	public void StartTaurus()
	{
		this.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Taurus;
		this.StartDash();
		base.StartCoroutine(this.check_state_cr(base.properties.CurrentState.stateName));
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x00128B0B File Offset: 0x00126F0B
	private void ToTaurus()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.taurus_cr());
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x00128B38 File Offset: 0x00126F38
	private IEnumerator taurus_cr()
	{
		LevelProperties.FlyingBlimp.Taurus p = base.properties.CurrentState.taurus;
		this.waitLoopTime = p.attackDelayRange.RandomFloat();
		this.moving = true;
		this.state = FlyingBlimpLevelBlimpLady.State.Taurus;
		this.movementSpeed = p.movementSpeed;
		do
		{
			float t = 0f;
			while (t < this.waitLoopTime)
			{
				t += CupheadTime.Delta;
				if (!this.isLooping)
				{
					break;
				}
				yield return null;
			}
			t = 0f;
			this.moving = false;
			base.animator.SetTrigger("TaurusATK");
			yield return base.animator.WaitForAnimationToStart(this, "Taurus_Attack", false);
			AudioManager.Play("level_flying_blimp_taurus_attack");
			AudioManager.Stop("level_flying_blimp_taurus_idle");
			this.emitAudioFromObject.Add("level_flying_blimp_taurus_attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Taurus_Attack", false, true);
			this.moving = true;
			yield return null;
		}
		while (this.isLooping);
		base.animator.Play("Big_Cloud");
		this.movementSpeed = this.originalSpeed;
		base.StartCoroutine(this.final_fade_cr());
		yield return null;
		yield break;
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x00128B53 File Offset: 0x00126F53
	public void StartSagittarius()
	{
		this.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius;
		this.StartDash();
		base.StartCoroutine(this.check_state_cr(base.properties.CurrentState.stateName));
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x00128B7F File Offset: 0x00126F7F
	private void ToSagittarius()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.sagittarius_cr());
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x00128BAC File Offset: 0x00126FAC
	private IEnumerator sagittarius_cr()
	{
		LevelProperties.FlyingBlimp.Sagittarius p = base.properties.CurrentState.sagittarius;
		this.waitLoopTime = p.attackDelayRange.RandomFloat();
		this.moving = true;
		this.state = FlyingBlimpLevelBlimpLady.State.Sagittarius;
		this.movementSpeed = (float)p.movementSpeed;
		do
		{
			base.animator.SetTrigger("SagittariusATK");
			yield return base.animator.WaitForAnimationToStart(this, "Sagittarius_Attack_Loop", false);
			AudioManager.Play("level_flying_blimp_sagittarius_anticipation");
			yield return CupheadTime.WaitForSeconds(this, p.arrowWarning);
			base.animator.SetTrigger("Continue");
			AudioManager.Stop("level_flying_blimp_sagittarius_anticipation");
			float t = 0f;
			while (t < this.waitLoopTime)
			{
				t += CupheadTime.Delta;
				if (!this.isLooping)
				{
					break;
				}
				yield return null;
			}
			t = 0f;
			yield return null;
		}
		while (this.isLooping);
		base.animator.Play("Big_Cloud");
		this.movementSpeed = this.originalSpeed;
		base.StartCoroutine(this.final_fade_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x00128BC8 File Offset: 0x00126FC8
	private void FireArrowsStars()
	{
		LevelProperties.FlyingBlimp.Sagittarius sagittarius = base.properties.CurrentState.sagittarius;
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			AbstractPlayerController next = PlayerManager.GetNext();
			float num2 = sagittarius.homingSpreadAngle.GetFloatAt((float)i / ((float)num - 1f));
			float num3 = sagittarius.homingSpreadAngle.max / 2f;
			num2 -= num3;
			float num4 = Mathf.Atan2(0f, -360f) * 57.29578f;
			this.sagittariusStarPrefab.Create(this.arrowEffectRoot.transform.position, num4 + num2, sagittarius.arrowInitialSpeed, sagittarius.homingSpeed, sagittarius.homingRotation, sagittarius.homingDurationRange.RandomFloat(), sagittarius.homingDelay, next, (float)sagittarius.arrowHP);
		}
		this.arrowEffect.Create(this.arrowEffectRoot.transform.position);
		this.sagittariusArrowPrefab.Create(this.arrowRoot.position, 180f, sagittarius.arrowInitialSpeed);
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x00128CEA File Offset: 0x001270EA
	public void StartGemini()
	{
		this.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini;
		this.StartDash();
		base.StartCoroutine(this.check_state_cr(base.properties.CurrentState.stateName));
	}

	// Token: 0x06002064 RID: 8292 RVA: 0x00128D16 File Offset: 0x00127116
	private void ToGemini()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.gemini_cr());
	}

	// Token: 0x06002065 RID: 8293 RVA: 0x00128D44 File Offset: 0x00127144
	private IEnumerator gemini_cr()
	{
		LevelProperties.FlyingBlimp.Gemini p = base.properties.CurrentState.gemini;
		this.waitLoopTime = base.properties.CurrentState.gemini.spawnerDelay.RandomFloat();
		this.pivotPoint.position = base.transform.position;
		this.moving = true;
		bool repeat = false;
		this.state = FlyingBlimpLevelBlimpLady.State.Gemini;
		do
		{
			if (this.geminiObject == null)
			{
				if (repeat)
				{
					AudioManager.Play("level_flying_blimp_gemini_sphere_reappear");
					base.animator.Play("Sphere_Reappear");
				}
				float t = 0f;
				while (t < this.waitLoopTime)
				{
					t += CupheadTime.Delta;
					if (!this.isLooping)
					{
						break;
					}
					yield return null;
				}
				t = 0f;
				yield return base.animator.WaitForAnimationToEnd(this, "Gemini", false, true);
				base.animator.SetTrigger("GeminiATK");
				AudioManager.Play("level_flying_blimp_gemini_attack");
				base.animator.Play("Gemini_Attack");
				yield return CupheadTime.WaitForSeconds(this, p.spawnerSpeed);
				this.SpawnGemini();
				repeat = true;
				yield return null;
			}
			yield return null;
		}
		while (this.isLooping);
		base.animator.Play("Big_Cloud");
		this.movementSpeed = this.originalSpeed;
		base.StartCoroutine(this.final_fade_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002066 RID: 8294 RVA: 0x00128D60 File Offset: 0x00127160
	private void SpawnGemini()
	{
		this.geminiTarget = this.objectSpawnRoot.transform.position;
		Vector2 a = this.geminiTarget;
		Vector2 vector = new Vector2(UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1), UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1));
		this.geminiTarget = a + vector.normalized * this.objectSpawnRoot.radius * UnityEngine.Random.value;
		this.geminiObject = UnityEngine.Object.Instantiate<FlyingBlimpLevelGeminiShoot>(this.geminiObjectPrefab);
		this.geminiObject.Init(base.properties.CurrentState.gemini, this.geminiTarget);
	}

	// Token: 0x06002067 RID: 8295 RVA: 0x00128E23 File Offset: 0x00127223
	private void SwitchCloneBottomLayer()
	{
		this.geminiClone.sortingOrder = 1;
		base.GetComponent<SpriteRenderer>().sortingOrder = 3;
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x00128E3D File Offset: 0x0012723D
	private void SwitchCloneTopLayer()
	{
		this.geminiClone.sortingOrder = 3;
		base.GetComponent<SpriteRenderer>().sortingOrder = 1;
	}

	// Token: 0x06002069 RID: 8297 RVA: 0x00128E57 File Offset: 0x00127257
	private void SwitchSphereLayer(int layer)
	{
		this.sphere.sortingOrder = layer;
	}

	// Token: 0x0600206A RID: 8298 RVA: 0x00128E68 File Offset: 0x00127268
	public void SummonTornado()
	{
		LevelProperties.FlyingBlimp.Tornado properties = base.properties.CurrentState.tornado;
		this.tornado = UnityEngine.Object.Instantiate<FlyingBlimpLevelTornado>(this.tornadoPrefab);
		this.tornado.Init(this.projectileRoot.transform.position, PlayerManager.GetNext(), properties);
	}

	// Token: 0x0600206B RID: 8299 RVA: 0x00128EBD File Offset: 0x001272BD
	private void MoveTornado()
	{
		if (this.tornadoPrefab != null)
		{
			base.StartCoroutine(this.tornado.move_cr());
		}
	}

	// Token: 0x0600206C RID: 8300 RVA: 0x00128EE2 File Offset: 0x001272E2
	public void StartTornado()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.tornado_cr());
	}

	// Token: 0x0600206D RID: 8301 RVA: 0x00128F10 File Offset: 0x00127310
	public IEnumerator tornado_cr()
	{
		this.state = FlyingBlimpLevelBlimpLady.State.Tornado;
		LevelProperties.FlyingBlimp.Tornado p = base.properties.CurrentState.tornado;
		this.moving = false;
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AudioManager.Stop("level_flying_blimp_pedal_loop");
		AudioManager.Play("level_flying_blimp_tornado");
		this.SummonTornado();
		base.animator.Play("Tornado_Start");
		yield return base.animator.WaitForAnimationToEnd(this, "Tornado_Start", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.loopDuration);
		base.animator.SetTrigger("FinishTornado");
		yield return base.animator.WaitForAnimationToEnd(this, "Tornado_Finish", false, true);
		AudioManager.PlayLoop("level_flying_blimp_pedal_loop");
		this.moving = true;
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack);
		this.state = FlyingBlimpLevelBlimpLady.State.Idle;
		yield break;
	}

	// Token: 0x0600206E RID: 8302 RVA: 0x00128F2B File Offset: 0x0012732B
	public void StartShoot()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x0600206F RID: 8303 RVA: 0x00128F58 File Offset: 0x00127358
	private IEnumerator shoot_cr()
	{
		this.state = FlyingBlimpLevelBlimpLady.State.Shoot;
		LevelProperties.FlyingBlimp.Shoot p = base.properties.CurrentState.shoot;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("level_flying_blimp_fire");
		base.animator.Play("Shoot_Start");
		yield return base.animator.WaitForAnimationToEnd(this, "Shoot_Start", false, true);
		this.spawnProjectile();
		yield return CupheadTime.WaitForSeconds(this, 0.7f);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttackRange.RandomFloat());
		this.state = FlyingBlimpLevelBlimpLady.State.Idle;
		yield break;
	}

	// Token: 0x06002070 RID: 8304 RVA: 0x00128F73 File Offset: 0x00127373
	private void spawnProjectile()
	{
		this.shootProjectilePrefab.Create(this.projectileRoot.position, 0f, base.properties.CurrentState.shoot);
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x00128FA8 File Offset: 0x001273A8
	private void SummonEnemy(FlyingBlimpLevelEnemy prefab, Vector3 startPoint, float stopPoint, bool type)
	{
		FlyingBlimpLevelEnemy flyingBlimpLevelEnemy = UnityEngine.Object.Instantiate<FlyingBlimpLevelEnemy>(prefab);
		Vector2 v = flyingBlimpLevelEnemy.transform.position;
		v.y = 360f - startPoint.y;
		v.x = 740f;
		stopPoint = base.properties.CurrentState.enemy.stopDistance.RandomFloat();
		flyingBlimpLevelEnemy.transform.position = v;
		flyingBlimpLevelEnemy.Init(base.properties, startPoint, stopPoint, type, this);
	}

	// Token: 0x06002072 RID: 8306 RVA: 0x0012902C File Offset: 0x0012742C
	public IEnumerator spawnEnemy_cr()
	{
		LevelProperties.FlyingBlimp.Enemy p = base.properties.CurrentState.enemy;
		string[] spawnPattern = p.spawnString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] typePattern = p.typeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		bool AParryable = false;
		float waitTime = 0f;
		int counter = 0;
		int typeIndex = 0;
		int spawnIndex = UnityEngine.Random.Range(0, spawnPattern.Length);
		Vector3 spawnPos = Vector3.zero;
		for (;;)
		{
			for (int i = spawnIndex; i < spawnPattern.Length; i++)
			{
				if (waitTime > 0f)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
				}
				if (spawnPattern[i][0] == 'D')
				{
					Parser.FloatTryParse(spawnPattern[i].Substring(1), out waitTime);
				}
				else
				{
					string[] array = spawnPattern[i].Split(new char[]
					{
						'-'
					});
					foreach (string s in array)
					{
						float y = 0f;
						float stopPoint = 0f;
						Parser.FloatTryParse(s, out y);
						Parser.FloatTryParse(s, out stopPoint);
						FlyingBlimpLevelEnemy prefab = null;
						if (typePattern[typeIndex][0] == 'A')
						{
							prefab = this.enemyPrefabA;
							if ((float)counter >= p.APinkOccurance.RandomFloat())
							{
								AParryable = true;
								counter = 0;
							}
							else
							{
								AParryable = false;
								counter++;
							}
						}
						else if (typePattern[typeIndex][0] == 'B')
						{
							prefab = this.enemyPrefabB;
							AParryable = false;
						}
						spawnPos.y = y;
						if (this.state != FlyingBlimpLevelBlimpLady.State.Death)
						{
							this.SummonEnemy(prefab, spawnPos, stopPoint, AParryable);
						}
						typeIndex = (typeIndex + 1) % typePattern.Length;
					}
					waitTime = p.stringDelay;
				}
				i %= spawnPattern.Length;
			}
			spawnIndex = 0;
		}
		yield break;
	}

	// Token: 0x06002073 RID: 8307 RVA: 0x00129047 File Offset: 0x00127447
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06002074 RID: 8308 RVA: 0x0012905C File Offset: 0x0012745C
	private IEnumerator die_cr()
	{
		base.animator.SetTrigger("Death");
		this.moving = false;
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		base.GetComponent<Collider2D>().enabled = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x00129077 File Offset: 0x00127477
	public void SpawnMoonLady()
	{
		base.StartCoroutine(this.spawn_moon_lady_cr());
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x00129088 File Offset: 0x00127488
	private IEnumerator spawn_moon_lady_cr()
	{
		while (this.angle > 0.2617994f || this.angle < -0.2617994f)
		{
			yield return null;
		}
		this.moving = false;
		this.moonLady.StartIntro();
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x001290A3 File Offset: 0x001274A3
	private void SagAttackSFX()
	{
		AudioManager.Play("level_flying_blimp_sagittarius_attack");
		this.emitAudioFromObject.Add("level_flying_blimp_sagittarius_attack");
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x001290BF File Offset: 0x001274BF
	private void TaurusIdleSFX()
	{
		AudioManager.Play("level_flying_blimp_taurus_idle");
		this.emitAudioFromObject.Add("level_flying_blimp_taurus_idle");
	}

	// Token: 0x040028BD RID: 10429
	[Header("Phase Materials")]
	[SerializeField]
	private Material blimpMat;

	// Token: 0x040028BE RID: 10430
	[SerializeField]
	private Material taurusMat;

	// Token: 0x040028BF RID: 10431
	[SerializeField]
	private Material sagittariusMat;

	// Token: 0x040028C0 RID: 10432
	[SerializeField]
	private Material geminiMat;

	// Token: 0x040028C1 RID: 10433
	[Space(10f)]
	[SerializeField]
	private Transform pivotPoint;

	// Token: 0x040028C2 RID: 10434
	[SerializeField]
	private Transform transformationPoint;

	// Token: 0x040028C3 RID: 10435
	[SerializeField]
	private Effect dashExplosionEffect;

	// Token: 0x040028C4 RID: 10436
	[SerializeField]
	private GameObject cloudEffect;

	// Token: 0x040028C5 RID: 10437
	[SerializeField]
	private GameObject bigCloud;

	// Token: 0x040028C6 RID: 10438
	[SerializeField]
	private SpriteRenderer constellationHandler;

	// Token: 0x040028C7 RID: 10439
	[SerializeField]
	private SpriteRenderer blackDim;

	// Token: 0x040028C8 RID: 10440
	[SerializeField]
	private FlyingBlimpLevelEnemy enemyPrefabA;

	// Token: 0x040028C9 RID: 10441
	[SerializeField]
	private FlyingBlimpLevelEnemy enemyPrefabB;

	// Token: 0x040028CA RID: 10442
	[SerializeField]
	private FlyingBlimpLevelTornado tornadoPrefab;

	// Token: 0x040028CB RID: 10443
	private FlyingBlimpLevelTornado tornado;

	// Token: 0x040028CC RID: 10444
	[SerializeField]
	private FlyingBlimpLevelShootProjectile shootProjectilePrefab;

	// Token: 0x040028CD RID: 10445
	[SerializeField]
	private FlyingBlimpLevelArrowProjectile sagittariusStarPrefab;

	// Token: 0x040028CE RID: 10446
	[SerializeField]
	private BasicProjectile sagittariusArrowPrefab;

	// Token: 0x040028CF RID: 10447
	[SerializeField]
	private FlyingBlimpLevelGeminiShoot geminiObjectPrefab;

	// Token: 0x040028D0 RID: 10448
	private FlyingBlimpLevelGeminiShoot geminiObject;

	// Token: 0x040028D1 RID: 10449
	[SerializeField]
	private SpriteRenderer geminiClone;

	// Token: 0x040028D2 RID: 10450
	[SerializeField]
	private SpriteRenderer sphere;

	// Token: 0x040028D3 RID: 10451
	[SerializeField]
	private FlyingBlimpLevelSpawnRadius objectSpawnRoot;

	// Token: 0x040028D4 RID: 10452
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x040028D5 RID: 10453
	[SerializeField]
	private Transform arrowRoot;

	// Token: 0x040028D6 RID: 10454
	[SerializeField]
	private Transform arrowEffectRoot;

	// Token: 0x040028D7 RID: 10455
	[SerializeField]
	private FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x040028D8 RID: 10456
	[SerializeField]
	private Vector2 explosionOffset = Vector2.zero;

	// Token: 0x040028D9 RID: 10457
	[SerializeField]
	private Effect arrowEffect;

	// Token: 0x040028DA RID: 10458
	[SerializeField]
	private float explosionRadius = 100f;

	// Token: 0x040028DB RID: 10459
	private float angle;

	// Token: 0x040028DC RID: 10460
	private float originalSpeed;

	// Token: 0x040028DD RID: 10461
	private float loopSize = 80f;

	// Token: 0x040028DE RID: 10462
	private float movementSpeed;

	// Token: 0x040028DF RID: 10463
	private float waitLoopTime;

	// Token: 0x040028E0 RID: 10464
	private Vector3 startPos;

	// Token: 0x040028E1 RID: 10465
	private Vector3 pivotOffset;

	// Token: 0x040028E2 RID: 10466
	private Vector3 getPos;

	// Token: 0x040028E3 RID: 10467
	private Vector2 geminiTarget;

	// Token: 0x040028E4 RID: 10468
	private bool invert;

	// Token: 0x040028E5 RID: 10469
	private bool isLooping;

	// Token: 0x040028E6 RID: 10470
	private bool smallClouds;

	// Token: 0x040028E7 RID: 10471
	private bool dashExplosions;

	// Token: 0x040028E8 RID: 10472
	private bool transitionToSummon = true;

	// Token: 0x040028E9 RID: 10473
	private DamageDealer damageDealer;

	// Token: 0x040028EA RID: 10474
	private DamageReceiver damageReceiver;

	// Token: 0x040028EB RID: 10475
	private Coroutine patternCoroutine;

	// Token: 0x040028EC RID: 10476
	private FlyingBlimpLevelBlimpLady.constellationPossibility constellation;

	// Token: 0x02000630 RID: 1584
	public enum State
	{
		// Token: 0x040028EE RID: 10478
		Intro,
		// Token: 0x040028EF RID: 10479
		Idle,
		// Token: 0x040028F0 RID: 10480
		Dash,
		// Token: 0x040028F1 RID: 10481
		Tornado,
		// Token: 0x040028F2 RID: 10482
		Shoot,
		// Token: 0x040028F3 RID: 10483
		Taurus,
		// Token: 0x040028F4 RID: 10484
		Sagittarius,
		// Token: 0x040028F5 RID: 10485
		Gemini,
		// Token: 0x040028F6 RID: 10486
		Death
	}

	// Token: 0x02000631 RID: 1585
	public enum constellationPossibility
	{
		// Token: 0x040028F8 RID: 10488
		Taurus = 1,
		// Token: 0x040028F9 RID: 10489
		Sagittarius,
		// Token: 0x040028FA RID: 10490
		Gemini
	}
}
