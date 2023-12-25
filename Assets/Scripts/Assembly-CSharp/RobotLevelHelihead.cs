using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200076F RID: 1903
public class RobotLevelHelihead : AbstractCollidableObject
{
	// Token: 0x06002967 RID: 10599 RVA: 0x00182177 File Offset: 0x00180577
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.current = RobotLevelHelihead.state.first;
		base.Awake();
	}

	// Token: 0x06002968 RID: 10600 RVA: 0x001821B4 File Offset: 0x001805B4
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.properties != null && this.properties.CurrentHealth <= 0f)
		{
			this.StopAllCoroutines();
		}
	}

	// Token: 0x06002969 RID: 10601 RVA: 0x001821F4 File Offset: 0x001805F4
	protected void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		Level.Current.timeline.DealDamage(info.damage);
		this.properties.DealDamage(info.damage);
		if (this.properties.CurrentHealth <= 0f && this.current != RobotLevelHelihead.state.dead)
		{
			this.current = RobotLevelHelihead.state.dead;
			this.StartDeath();
		}
	}

	// Token: 0x0600296A RID: 10602 RVA: 0x00182258 File Offset: 0x00180658
	public void InitHeliHead(LevelProperties.Robot properties)
	{
		this.introActive = true;
		this.screenHeights = properties.CurrentState.heliHead.onScreenHeight.Split(new char[]
		{
			','
		});
		this.coordinateIndex = UnityEngine.Random.Range(0, this.screenHeights.Length);
		this.attackTypeIndex = UnityEngine.Random.Range(0, properties.CurrentState.inventor.gemColourString.Split(new char[]
		{
			','
		}).Length);
		this.pivotPoint = new GameObject("pivotPoint");
		this.pivotPoint.transform.position = new Vector3((float)(Level.Current.Right - Level.Current.Width / 4), (float)(Level.Current.Ground + Level.Current.Height / 2), 0f);
		this.speed = (float)properties.CurrentState.heliHead.heliheadMovementSpeed;
		this.attackDelay = properties.CurrentState.heliHead.attackDelay;
		this.width = 300f;
		this.properties = properties;
		this.speed = (float)properties.CurrentState.heliHead.heliheadMovementSpeed;
		base.transform.Rotate(Vector3.forward, 90f);
		base.transform.position = this.spawnPoint.position;
		base.StartCoroutine(this.horizontalMovement_cr());
		base.StartCoroutine(this.attack_cr());
		base.StartCoroutine(this.check_sound_cr());
	}

	// Token: 0x0600296B RID: 10603 RVA: 0x001823D6 File Offset: 0x001807D6
	private void SpinSFX()
	{
		AudioManager.Play("robot_headspin");
		this.emitAudioFromObject.Add("robot_headspin");
	}

	// Token: 0x0600296C RID: 10604 RVA: 0x001823F4 File Offset: 0x001807F4
	private IEnumerator check_sound_cr()
	{
		bool onscreen = false;
		while (this.current == RobotLevelHelihead.state.first)
		{
			if (base.transform.position.x < (float)Level.Current.Right && base.transform.position.x > (float)Level.Current.Left)
			{
				if (!onscreen)
				{
					this.SpinSFX();
					onscreen = true;
				}
			}
			else if (onscreen)
			{
				AudioManager.Stop("robot_headspin");
				onscreen = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x00182410 File Offset: 0x00180810
	private IEnumerator horizontalMovement_cr()
	{
		this.offScreen = false;
		yield return new WaitForEndOfFrame();
		base.transform.position += Vector3.left * base.GetComponent<SpriteRenderer>().bounds.size.x / 10f;
		for (;;)
		{
			base.transform.position += Vector3.left * this.speed * CupheadTime.Delta;
			if (base.transform.position.x <= (float)Level.Current.Left - this.width)
			{
				base.GetComponent<BoxCollider2D>().enabled = false;
				this.offScreen = true;
				if (this.introActive)
				{
					this.introActive = false;
					base.animator.Play("Loop");
				}
				this.speed = 0f;
				base.transform.position = new Vector3(base.transform.position.x, (float)(Level.Current.Ground + Parser.IntParse(this.screenHeights[this.coordinateIndex])), base.transform.position.z);
				base.transform.Rotate(Vector3.forward, 180f);
				this.coordinateIndex++;
				if (this.coordinateIndex >= this.screenHeights.Length)
				{
					this.coordinateIndex = 0;
				}
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.heliHead.offScreenDelay);
				this.speed = (float)(-(float)this.properties.CurrentState.heliHead.heliheadMovementSpeed);
				base.transform.position += Vector3.right * 50f;
				this.offScreen = false;
				base.GetComponent<BoxCollider2D>().enabled = true;
			}
			if (base.transform.position.x >= (float)Level.Current.Right + this.width)
			{
				base.GetComponent<BoxCollider2D>().enabled = false;
				this.offScreen = true;
				this.speed = 0f;
				base.transform.position = new Vector3(base.transform.position.x, (float)(Level.Current.Ground + Parser.IntParse(this.screenHeights[this.coordinateIndex])), base.transform.position.z);
				base.transform.Rotate(Vector3.forward, 180f);
				base.transform.position += Vector3.left * 50f;
				this.coordinateIndex++;
				if (this.coordinateIndex >= this.screenHeights.Length)
				{
					this.coordinateIndex = 0;
				}
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.heliHead.offScreenDelay);
				this.speed = (float)this.properties.CurrentState.heliHead.heliheadMovementSpeed;
				this.offScreen = false;
				base.GetComponent<BoxCollider2D>().enabled = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x0018242C File Offset: 0x0018082C
	private IEnumerator inventorIntro_cr()
	{
		Vector3 end = new Vector3(this.pivotPoint.transform.position.x, -760f);
		Vector3 start = base.transform.position;
		float pct = 0f;
		while (pct < 1f)
		{
			base.transform.position = Vector3.Lerp(start, end, pct);
			pct += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = end;
		base.StartCoroutine(this.stateEasing_cr());
		yield break;
	}

	// Token: 0x0600296F RID: 10607 RVA: 0x00182448 File Offset: 0x00180848
	private IEnumerator stateEasing_cr()
	{
		base.transform.rotation = Quaternion.identity;
		Vector3 start = base.transform.position;
		Vector3 end = new Vector3(this.pivotPoint.transform.position.x - 200f, this.pivotPoint.transform.position.y);
		float pct = 0f;
		while (pct < 1f)
		{
			base.transform.position = Vector3.Lerp(start, end, pct);
			pct += CupheadTime.Delta;
			yield return null;
		}
		AudioManager.Stop("robot_headspin");
		base.animator.Play("Inventor Intro");
		base.StartCoroutine(this.verticalMovement_cr());
		this.speed *= 2f;
		yield return base.animator.WaitForAnimationToEnd(this, "End", true, true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.inventor.initialAttackDelay);
		float normalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
		float delay = 0f;
		if (base.animator.GetCurrentAnimatorStateInfo(0).length / normalizedTime < 1f)
		{
			delay -= normalizedTime;
		}
		else
		{
			delay += 1f - normalizedTime;
		}
		yield return CupheadTime.WaitForSeconds(this, delay);
		base.StartCoroutine(this.blockade_cr());
		if (this.properties.CurrentState.inventor.gemColourString.Split(new char[]
		{
			','
		})[this.attackTypeIndex] == "R")
		{
			base.animator.Play("Red Gem Attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Red Gem Attack", false, true);
			base.animator.Play("RedGemFXIntro", 2);
			this.gem.InitFinalStage(this, this.properties, false);
		}
		else
		{
			base.animator.Play("Blue Gem Attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Blue Gem Attack", false, true);
			base.animator.Play("BlueGemFXIntro", 2);
			this.gem.InitFinalStage(this, this.properties, true);
		}
		this.speed /= 2f;
		base.StartCoroutine(this.easeValues_cr(true));
		yield break;
	}

	// Token: 0x06002970 RID: 10608 RVA: 0x00182464 File Offset: 0x00180864
	private IEnumerator verticalMovement_cr()
	{
		this.speed = 1f;
		float time = 0f;
		for (;;)
		{
			time += CupheadTime.Delta * 2f;
			base.transform.position = this.pivotPoint.transform.position + Vector3.left * 200f + Vector3.up * Mathf.Sin(time * this.speed) * this.verticalMovementStrength + Vector3.right * Mathf.Sin(time * (2f * this.speed)) * this.horizontalMovementStrength;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002971 RID: 10609 RVA: 0x00182480 File Offset: 0x00180880
	private IEnumerator easeValues_cr(bool easeIn = true)
	{
		if (easeIn)
		{
			this.speed = this.properties.CurrentState.inventor.inventorIdleSpeedMultiplier;
		}
		base.StartCoroutine(this.easeStrength_cr(easeIn));
		if (easeIn)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.inventor.attackDuration.RandomFloat());
			if (this.properties.CurrentState.inventor.gemColourString.Split(new char[]
			{
				','
			})[this.attackTypeIndex] == "R")
			{
				base.animator.SetTrigger("RedGemAttack");
			}
			else
			{
				base.animator.SetTrigger("BlueGemAttack");
			}
			this.gem.OnAttackEnd();
			this.attackTypeIndex++;
			if (this.attackTypeIndex >= this.properties.CurrentState.inventor.gemColourString.Split(new char[]
			{
				','
			}).Length)
			{
				this.attackTypeIndex = 0;
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.inventor.attackDelay.RandomFloat());
			base.StartCoroutine(this.easeValues_cr(false));
		}
		else
		{
			float normalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
			float delay = 0f;
			if (base.animator.GetCurrentAnimatorStateInfo(0).length / normalizedTime < 1f)
			{
				delay -= normalizedTime;
			}
			else
			{
				delay += 1f - normalizedTime;
			}
			yield return CupheadTime.WaitForSeconds(this, delay);
			if (this.properties.CurrentState.inventor.gemColourString.Split(new char[]
			{
				','
			})[this.attackTypeIndex] == "R")
			{
				base.animator.Play("Red Gem Attack");
				yield return base.animator.WaitForAnimationToEnd(this, "Red Gem Attack", false, true);
				base.animator.Play("RedGemFXIntro", 2);
				this.gem.InitFinalStage(this, this.properties, false);
			}
			else
			{
				base.animator.Play("Blue Gem Attack");
				yield return base.animator.WaitForAnimationToEnd(this, "Blue Gem Attack", false, true);
				base.animator.Play("BlueGemFXIntro", 2);
				this.gem.InitFinalStage(this, this.properties, true);
			}
			base.StartCoroutine(this.easeValues_cr(true));
		}
		yield break;
	}

	// Token: 0x06002972 RID: 10610 RVA: 0x001824A2 File Offset: 0x001808A2
	public void OnGemEnd()
	{
		this.GemEndSFX();
		base.animator.SetTrigger("StopGemFX");
	}

	// Token: 0x06002973 RID: 10611 RVA: 0x001824BA File Offset: 0x001808BA
	private void GemStartSFX()
	{
		AudioManager.Play("robot_diamond_attack_start");
		this.emitAudioFromObject.Add("robot_diamond_attack_start");
		AudioManager.PlayLoop("robot_diamond_attack_loop");
		this.emitAudioFromObject.Add("robot_diamond_attack_loop");
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x001824F0 File Offset: 0x001808F0
	private void GemEndSFX()
	{
		AudioManager.Stop("robot_diamond_attack_loop");
		AudioManager.Play("robot_diamond_attack_end");
		this.emitAudioFromObject.Add("robot_diamond_attack_end");
	}

	// Token: 0x06002975 RID: 10613 RVA: 0x00182516 File Offset: 0x00180916
	private void IntroSFX()
	{
		AudioManager.Play("robot_head_transform");
		this.emitAudioFromObject.Add("robot_head_transform");
	}

	// Token: 0x06002976 RID: 10614 RVA: 0x00182534 File Offset: 0x00180934
	private IEnumerator easeSpeed_cr()
	{
		float pct = 0f;
		while (pct < 1f)
		{
			this.speed = 1f + (this.properties.CurrentState.inventor.inventorIdleSpeedMultiplier - 1f) * pct;
			pct += 10f * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x00182550 File Offset: 0x00180950
	private IEnumerator easeStrength_cr(bool easeIn)
	{
		float pct = 0f;
		float hStrength = this.horizontalMovementStrength;
		float vStrength = this.verticalMovementStrength;
		while (pct < 1f)
		{
			if (easeIn)
			{
				this.horizontalMovementStrength = hStrength + (25f - hStrength) * pct;
				this.verticalMovementStrength = vStrength + (160f - vStrength) * pct;
			}
			else
			{
				this.horizontalMovementStrength = hStrength + (0f - hStrength) * pct;
				this.verticalMovementStrength = vStrength + (20f - vStrength) * pct;
			}
			pct += 0.25f * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002978 RID: 10616 RVA: 0x00182574 File Offset: 0x00180974
	private IEnumerator attack_cr()
	{
		for (;;)
		{
			if (this.offScreen)
			{
				this.attackDelay -= CupheadTime.Delta;
				if (this.attackDelay <= 0f)
				{
					this.SpawnBombBot();
					this.attackDelay = 100f;
				}
			}
			else
			{
				this.attackDelay = this.properties.CurrentState.heliHead.attackDelay;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002979 RID: 10617 RVA: 0x00182590 File Offset: 0x00180990
	private IEnumerator blockade_cr()
	{
		float groupSize = (float)this.properties.CurrentState.inventor.blockadeGroupSize;
		int dir = 1;
		for (;;)
		{
			int i = 0;
			while ((float)i < groupSize)
			{
				if (dir > 0)
				{
					RobotLevelBlockade robotLevelBlockade = this.blockadeSegement.Create(new Vector3((float)Level.Current.Right, (float)Level.Current.Ceiling, 0f), dir);
					robotLevelBlockade.InitBlockade(dir, this.properties.CurrentState.inventor.blockadeHorizontalSpeed, this.properties.CurrentState.inventor.blockadeVerticalSpeed);
				}
				else
				{
					RobotLevelBlockade robotLevelBlockade2 = this.blockadeSegement.Create(new Vector3((float)Level.Current.Right, (float)Level.Current.Ground, 0f), dir);
					robotLevelBlockade2.InitBlockade(dir, this.properties.CurrentState.inventor.blockadeHorizontalSpeed, this.properties.CurrentState.inventor.blockadeVerticalSpeed);
				}
				dir *= -1;
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.inventor.blockadeIndividualDelay);
				yield return null;
				i++;
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.inventor.blockadeGroupDelay);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x001825AC File Offset: 0x001809AC
	private void SpawnBombBot()
	{
		HomingProjectile homingProjectile = this.bombBotPrefab.GetComponent<RobotLevelHatchBombBot>().Create(base.transform.GetChild(0).transform.position, (float)((int)base.transform.eulerAngles.z + 90), (float)this.properties.CurrentState.bombBot.initialBombMovementSpeed, (float)this.properties.CurrentState.bombBot.bombHomingSpeed, this.properties.CurrentState.bombBot.bombRotationSpeed, (float)this.properties.CurrentState.bombBot.bombLifeTime, 4f, PlayerManager.GetNext());
		homingProjectile.GetComponent<RobotLevelHatchBombBot>().InitBombBot(this.properties.CurrentState.bombBot);
	}

	// Token: 0x0600297B RID: 10619 RVA: 0x00182679 File Offset: 0x00180A79
	public void ChangeState()
	{
		this.current = RobotLevelHelihead.state.second;
		this.StopAllCoroutines();
		base.StartCoroutine(this.inventorIntro_cr());
	}

	// Token: 0x0600297C RID: 10620 RVA: 0x00182695 File Offset: 0x00180A95
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		AudioManager.Stop("robot_diamond_attack_loop");
	}

	// Token: 0x0600297D RID: 10621 RVA: 0x001826AD File Offset: 0x00180AAD
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x001826CC File Offset: 0x00180ACC
	private void StartDeath()
	{
		this.StopAllCoroutines();
		if (this.OnDeath != null)
		{
			this.OnDeath();
		}
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
		AudioManager.Stop("robot_diamond_attack_loop");
	}

	// Token: 0x04003268 RID: 12904
	[SerializeField]
	private float verticalMovementStrength;

	// Token: 0x04003269 RID: 12905
	[SerializeField]
	private float horizontalMovementStrength;

	// Token: 0x0400326A RID: 12906
	[SerializeField]
	private Transform spawnPoint;

	// Token: 0x0400326B RID: 12907
	private GameObject pivotPoint;

	// Token: 0x0400326C RID: 12908
	private bool introActive;

	// Token: 0x0400326D RID: 12909
	private int coordinateIndex;

	// Token: 0x0400326E RID: 12910
	private string[] screenHeights;

	// Token: 0x0400326F RID: 12911
	private float speed;

	// Token: 0x04003270 RID: 12912
	private float width;

	// Token: 0x04003271 RID: 12913
	private float attackDelay;

	// Token: 0x04003272 RID: 12914
	private bool offScreen;

	// Token: 0x04003273 RID: 12915
	private int attackTypeIndex;

	// Token: 0x04003274 RID: 12916
	private RobotLevelHelihead.state current;

	// Token: 0x04003275 RID: 12917
	private LevelProperties.Robot properties;

	// Token: 0x04003276 RID: 12918
	private DamageDealer damageDealer;

	// Token: 0x04003277 RID: 12919
	private DamageReceiver damageReceiver;

	// Token: 0x04003278 RID: 12920
	[SerializeField]
	private GameObject bombBotPrefab;

	// Token: 0x04003279 RID: 12921
	[SerializeField]
	private RobotLevelBlockade blockadeSegement;

	// Token: 0x0400327A RID: 12922
	[SerializeField]
	private RobotLevelGem gem;

	// Token: 0x0400327B RID: 12923
	public Action OnDeath;

	// Token: 0x02000770 RID: 1904
	private enum state
	{
		// Token: 0x0400327D RID: 12925
		first,
		// Token: 0x0400327E RID: 12926
		second,
		// Token: 0x0400327F RID: 12927
		dead
	}
}
