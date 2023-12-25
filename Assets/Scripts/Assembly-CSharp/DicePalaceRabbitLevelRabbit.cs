using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005E0 RID: 1504
public class DicePalaceRabbitLevelRabbit : LevelProperties.DicePalaceRabbit.Entity
{
	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06001DBB RID: 7611 RVA: 0x00111389 File Offset: 0x0010F789
	// (set) Token: 0x06001DBC RID: 7612 RVA: 0x00111391 File Offset: 0x0010F791
	public DicePalaceRabbitLevelRabbit.State state { get; private set; }

	// Token: 0x06001DBD RID: 7613 RVA: 0x0011139C File Offset: 0x0010F79C
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.idle_voice_sfx_cr());
		base.StartCoroutine(this.idle_sfx_cr());
	}

	// Token: 0x06001DBE RID: 7614 RVA: 0x001113F7 File Offset: 0x0010F7F7
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001DBF RID: 7615 RVA: 0x0011140A File Offset: 0x0010F80A
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001DC0 RID: 7616 RVA: 0x00111422 File Offset: 0x0010F822
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x00111440 File Offset: 0x0010F840
	public override void LevelInit(LevelProperties.DicePalaceRabbit properties)
	{
		base.LevelInit(properties);
		this.attacking = false;
		this.playerOneCircleIndex = UnityEngine.Random.Range(0, properties.CurrentState.magicWand.safeZoneString.Split(new char[]
		{
			','
		}).Length);
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		zero.x = (float)Parser.IntParse(properties.CurrentState.general.platformOnePosition.Split(new char[]
		{
			','
		})[0]);
		zero.y = (float)Parser.IntParse(properties.CurrentState.general.platformOnePosition.Split(new char[]
		{
			','
		})[1]);
		this.platform1.transform.position = zero;
		this.platform1.YPositionUp = zero.y;
		zero2.x = (float)Parser.IntParse(properties.CurrentState.general.platformTwoPosition.Split(new char[]
		{
			','
		})[0]);
		zero2.y = (float)Parser.IntParse(properties.CurrentState.general.platformTwoPosition.Split(new char[]
		{
			','
		})[1]);
		this.platform2.transform.position = zero2;
		this.platform2.YPositionUp = zero2.y;
		this.isMagicParryTop = Rand.Bool();
		this.state = DicePalaceRabbitLevelRabbit.State.Idle;
		Level.Current.OnWinEvent += this.Death;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x001115D8 File Offset: 0x0010F9D8
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_Continue", false, true);
		base.animator.Play("Off");
		yield return null;
		yield break;
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x001115F4 File Offset: 0x0010F9F4
	private IEnumerator idle_voice_sfx_cr()
	{
		MinMax delay = new MinMax(1f, 4f);
		for (;;)
		{
			while (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, delay);
			AudioManager.Play("dice_palace_rabbit_idle_vox");
			this.emitAudioFromObject.Add("dice_palace_rabbit_idle_vox");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x00111610 File Offset: 0x0010FA10
	private IEnumerator idle_sfx_cr()
	{
		bool loopingIdle = false;
		for (;;)
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				if (!loopingIdle)
				{
				}
			}
			else if (loopingIdle)
			{
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x0011162B File Offset: 0x0010FA2B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.orbPrefab = null;
		this.magicPrefab = null;
		this.explosionPrefab = null;
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x00111648 File Offset: 0x0010FA48
	public void OnMagicWand()
	{
		base.StartCoroutine(this.magicwand_cr());
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x00111658 File Offset: 0x0010FA58
	private IEnumerator magicwand_cr()
	{
		this.attacking = true;
		this.state = DicePalaceRabbitLevelRabbit.State.MagicWand;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicWand.initialAttackDelay);
		AbstractPlayerController player = PlayerManager.GetNext();
		base.animator.SetTrigger("OnAttack");
		base.StartCoroutine(this.orbs_cr(player.id, Parser.IntParse(base.properties.CurrentState.magicWand.safeZoneString.Split(new char[]
		{
			','
		})[this.playerOneCircleIndex])));
		this.playerOneCircleIndex++;
		if (this.playerOneCircleIndex >= base.properties.CurrentState.magicWand.safeZoneString.Split(new char[]
		{
			','
		}).Length)
		{
			this.playerOneCircleIndex = 0;
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicWand.attackDelayRange.RandomFloat());
		this.attacking = false;
		base.animator.SetTrigger("OnAttackEnd");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicWand.hesitate);
		this.state = DicePalaceRabbitLevelRabbit.State.Idle;
		yield break;
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x00111674 File Offset: 0x0010FA74
	private IEnumerator orbs_cr(PlayerId target, int safeZone)
	{
		GameObject centerPoint = new GameObject();
		AbstractPlayerController player = PlayerManager.GetPlayer(target);
		centerPoint.transform.position = player.center;
		this.currentCenterPoint = centerPoint;
		Vector3 dir = Vector3.up;
		float dist = base.properties.CurrentState.magicWand.circleDiameter / 2f;
		safeZone = this.GetSafeZone(safeZone);
		Transform[] orbs = new Transform[7];
		int orbsIndex = 0;
		float initialRotation = (float)UnityEngine.Random.Range(0, 350);
		for (int i = 0; i < 8; i++)
		{
			if (i != safeZone)
			{
				DicePalaceRabbitLevelOrb dicePalaceRabbitLevelOrb = this.orbPrefab.Create(player.center + dir * dist, 0f, Vector2.one) as DicePalaceRabbitLevelOrb;
				dicePalaceRabbitLevelOrb.transform.parent = centerPoint.transform;
				dicePalaceRabbitLevelOrb.transform.Rotate(Vector3.forward, -initialRotation);
				dicePalaceRabbitLevelOrb.SetAsGold(i % 2 == 1);
				Color color = dicePalaceRabbitLevelOrb.GetComponent<SpriteRenderer>().color;
				color.a = 0.2f;
				dicePalaceRabbitLevelOrb.GetComponent<SpriteRenderer>().color = color;
				orbs[orbsIndex] = dicePalaceRabbitLevelOrb.transform;
				orbsIndex++;
			}
			dir = Quaternion.AngleAxis(-45f, Vector3.forward) * dir;
		}
		centerPoint.transform.Rotate(Vector3.forward, initialRotation);
		while (this.attacking)
		{
			if (player != null && !player.IsDead)
			{
				centerPoint.transform.position = player.center;
			}
			centerPoint.transform.Rotate(Vector3.forward * CupheadTime.FixedDelta, -base.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
			for (int j = 0; j < orbs.Length; j++)
			{
				SpriteRenderer component = orbs[j].GetComponent<SpriteRenderer>();
				Color color2 = component.color;
				color2.a += CupheadTime.Delta / 2f;
				component.color = color2;
				if (color2.a >= 1f)
				{
					orbs[j].GetComponent<Collider2D>().enabled = true;
				}
				orbs[j].Rotate(Vector3.forward * CupheadTime.FixedDelta, base.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
			}
			yield return new WaitForFixedUpdate();
		}
		for (int k = 0; k < orbs.Length; k++)
		{
			orbs[k].GetComponent<Collider2D>().enabled = true;
		}
		while (Vector3.Angle(Vector3.up, centerPoint.transform.up) > 5f)
		{
			if (player != null && !player.IsDead)
			{
				centerPoint.transform.position = player.center;
			}
			centerPoint.transform.Rotate(Vector3.forward * CupheadTime.FixedDelta, -base.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
			for (int l = 0; l < orbs.Length; l++)
			{
				orbs[l].Rotate(Vector3.forward * CupheadTime.FixedDelta, base.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
			}
			yield return new WaitForFixedUpdate();
		}
		centerPoint.transform.up = Vector3.up;
		base.StartCoroutine(this.collapse_cr(centerPoint));
		yield break;
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x001116A0 File Offset: 0x0010FAA0
	private IEnumerator collapse_cr(GameObject centerPoint)
	{
		float dist = base.properties.CurrentState.magicWand.circleDiameter / 2f;
		float explodeDist = base.properties.CurrentState.magicWand.circleDiameter * 0.1f;
		while (dist >= explodeDist)
		{
			for (int i = 0; i < 7; i++)
			{
				Vector3 b = (centerPoint.transform.GetChild(i).position - centerPoint.transform.position).normalized * dist;
				centerPoint.transform.GetChild(i).position = centerPoint.transform.position + b;
			}
			dist -= base.properties.CurrentState.magicWand.bulletSpeed * CupheadTime.Delta;
			yield return null;
		}
		AudioManager.Play("projectile_explo");
		this.explosionPrefab.Create(centerPoint.transform.position);
		this.currentCenterPoint = null;
		UnityEngine.Object.Destroy(centerPoint);
		yield break;
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x001116C4 File Offset: 0x0010FAC4
	private int GetSafeZone(int index)
	{
		int result = 0;
		switch (index)
		{
		case 1:
			result = 5;
			break;
		case 2:
			result = 4;
			break;
		case 3:
			result = 3;
			break;
		case 4:
			result = 6;
			break;
		case 6:
			result = 2;
			break;
		case 7:
			result = 7;
			break;
		case 8:
			result = 0;
			break;
		case 9:
			result = 1;
			break;
		}
		return result;
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x00111744 File Offset: 0x0010FB44
	private IEnumerator kill_orbs_cr()
	{
		float t = 0f;
		float time = 1f;
		float speed = 2500f;
		float[] angles = new float[7];
		for (int i = 0; i < 7; i++)
		{
			this.currentCenterPoint.transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
			angles[i] = (float)UnityEngine.Random.Range(0, 360);
		}
		while (t < time)
		{
			t += CupheadTime.Delta;
			for (int j = 0; j < 7; j++)
			{
				this.currentCenterPoint.transform.GetChild(j).position += MathUtils.AngleToDirection(angles[j]) * speed * CupheadTime.FixedDelta;
			}
			yield return null;
		}
		UnityEngine.Object.Destroy(this.currentCenterPoint);
		yield return null;
		yield break;
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x0011175F File Offset: 0x0010FB5F
	public void OnMagicParry()
	{
		base.StartCoroutine(this.magicparry_cr());
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x00111770 File Offset: 0x0010FB70
	private IEnumerator magicparry_cr()
	{
		this.attacking = true;
		this.state = DicePalaceRabbitLevelRabbit.State.MagicParry;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicParry.initialAttackDelay);
		base.animator.SetTrigger("OnAttack");
		string[] positionsSplits = base.properties.CurrentState.magicParry.magicPositions.Split(new char[]
		{
			'-'
		});
		DicePalaceRabbitLevelMagic[] magicOrbs = new DicePalaceRabbitLevelMagic[positionsSplits.Length];
		string[] parryPattern = base.properties.CurrentState.magicParry.pinkString.Split(new char[]
		{
			','
		});
		string[] parryIndexes = parryPattern[this.parryCurrentIndex].Split(new char[]
		{
			'-'
		});
		float yOffset = base.properties.CurrentState.magicParry.yOffset;
		float posY = (!this.isMagicParryTop) ? (-360f + yOffset) : (360f - yOffset);
		int suit = 0;
		for (int i = 0; i < magicOrbs.Length; i++)
		{
			float num = 0f;
			Parser.FloatTryParse(positionsSplits[i], out num);
			num += -640f;
			magicOrbs[i] = (DicePalaceRabbitLevelMagic)this.magicPrefab.Create(new Vector3(num, posY));
			magicOrbs[i].IsOffset(i % 2 == 1);
			magicOrbs[i].AppearTime = base.properties.CurrentState.magicParry.attackDelayRange;
			bool flag = false;
			for (int j = 0; j < parryIndexes.Length; j++)
			{
				int num2 = 0;
				if (Parser.IntTryParse(parryIndexes[j], out num2) && num2 - 1 == i)
				{
					magicOrbs[i].SetParryable(true);
					flag = true;
				}
			}
			if (!flag)
			{
				magicOrbs[i].SetSuit(suit);
				suit = (suit + 1) % 3;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicParry.attackDelayRange);
		for (int k = 0; k < magicOrbs.Length; k++)
		{
			magicOrbs[k].ActivateOrb();
			magicOrbs[k].Move(posY, this.isMagicParryTop, base.properties.CurrentState.magicParry.speed);
		}
		base.animator.SetTrigger("OnAttackEnd");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicParry.hesitate);
		this.attacking = false;
		this.isMagicParryTop = !this.isMagicParryTop;
		this.parryCurrentIndex = (this.parryCurrentIndex + 1) % parryPattern.Length;
		this.state = DicePalaceRabbitLevelRabbit.State.Idle;
		yield break;
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x0011178B File Offset: 0x0010FB8B
	private void AttackSFX()
	{
		base.StartCoroutine(this.attack_sfx_cr());
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x0011179C File Offset: 0x0010FB9C
	private IEnumerator attack_sfx_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
		yield return base.animator.WaitForAnimationToStart(this, "Attack_End", false);
		yield return null;
		yield break;
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x001117B8 File Offset: 0x0010FBB8
	private void Death()
	{
		AudioManager.Stop("dice_palace_rabbit_idle_loop");
		AudioManager.Stop("dice_palace_rabbit_attack_loop");
		this.SFX_StickTwirlStop();
		base.animator.SetTrigger("Death");
		this.StopAllCoroutines();
		if (this.currentCenterPoint != null)
		{
			base.StartCoroutine(this.kill_orbs_cr());
		}
		base.OnDestroy();
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x00111819 File Offset: 0x0010FC19
	private void SFX_IntroContinue()
	{
		AudioManager.Play("intro_continue");
		this.emitAudioFromObject.Add("intro_continue");
	}

	// Token: 0x06001DD2 RID: 7634 RVA: 0x00111835 File Offset: 0x0010FC35
	private void SFX_Death()
	{
		AudioManager.Play("dice_palace_rabbit_death");
		this.emitAudioFromObject.Add("dice_palace_rabbit_death");
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x00111851 File Offset: 0x0010FC51
	private void SFX_AttackStart()
	{
		AudioManager.Play("dice_palace_rabbit_attack_start");
		this.emitAudioFromObject.Add("dice_palace_rabbit_attack_start");
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x0011186D File Offset: 0x0010FC6D
	private void SFX_Attack()
	{
		if (!this.AttackSFXPlaying)
		{
			AudioManager.PlayLoop("dice_palace_rabbit_attack_loop");
			this.emitAudioFromObject.Add("dice_palace_rabbit_attack_loop");
			this.AttackSFXPlaying = true;
		}
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x0011189B File Offset: 0x0010FC9B
	private void SFX_AttackEnd()
	{
		AudioManager.Stop("dice_palace_rabbit_attack_loop");
		AudioManager.Play("dice_palace_rabbit_attack_end");
		this.emitAudioFromObject.Add("dice_palace_rabbit_attack_end");
		this.AttackSFXPlaying = false;
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x001118C8 File Offset: 0x0010FCC8
	private void SFX_IdleRock()
	{
		AudioManager.Play("idle_rock");
		this.emitAudioFromObject.Add("idle_rock");
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x001118E4 File Offset: 0x0010FCE4
	private void SFX_StickTwirl()
	{
		if (!this.StickTwirlActive)
		{
			this.StickTwirlActive = true;
			AudioManager.PlayLoop("stick_twirl");
			this.emitAudioFromObject.Add("stick_twirl");
		}
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x00111912 File Offset: 0x0010FD12
	private void SFX_StickTwirlStop()
	{
		this.StickTwirlActive = false;
		AudioManager.Stop("stick_twirl");
	}

	// Token: 0x04002694 RID: 9876
	private const float OrbAppearTime = 2f;

	// Token: 0x04002695 RID: 9877
	[SerializeField]
	private AbstractProjectile orbPrefab;

	// Token: 0x04002696 RID: 9878
	[SerializeField]
	private DicePalaceRabbitLevelMagic magicPrefab;

	// Token: 0x04002697 RID: 9879
	[SerializeField]
	private FlowerLevelPlatform platform1;

	// Token: 0x04002698 RID: 9880
	[SerializeField]
	private FlowerLevelPlatform platform2;

	// Token: 0x04002699 RID: 9881
	[SerializeField]
	private Effect explosionPrefab;

	// Token: 0x0400269A RID: 9882
	private bool attacking;

	// Token: 0x0400269B RID: 9883
	private bool isDying;

	// Token: 0x0400269C RID: 9884
	private int playerOneCircleIndex;

	// Token: 0x0400269D RID: 9885
	private DamageDealer damageDealer;

	// Token: 0x0400269E RID: 9886
	private DamageReceiver damageReceiver;

	// Token: 0x0400269F RID: 9887
	private bool isMagicParryTop;

	// Token: 0x040026A0 RID: 9888
	private int parryCurrentIndex;

	// Token: 0x040026A1 RID: 9889
	private GameObject currentCenterPoint;

	// Token: 0x040026A2 RID: 9890
	private bool AttackSFXPlaying;

	// Token: 0x040026A3 RID: 9891
	private bool StickTwirlActive;

	// Token: 0x020005E1 RID: 1505
	public enum State
	{
		// Token: 0x040026A6 RID: 9894
		Idle,
		// Token: 0x040026A7 RID: 9895
		MagicWand,
		// Token: 0x040026A8 RID: 9896
		MagicParry
	}
}
