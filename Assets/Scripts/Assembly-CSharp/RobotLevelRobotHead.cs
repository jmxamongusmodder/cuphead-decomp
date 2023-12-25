using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000779 RID: 1913
public class RobotLevelRobotHead : RobotLevelRobotBodyPart
{
	// Token: 0x060029E1 RID: 10721 RVA: 0x00187A44 File Offset: 0x00185E44
	public override void InitBodyPart(RobotLevelRobot parent, LevelProperties.Robot properties, int primaryHP = 1, int secondaryHP = 1, float attackDelayMinus = 0f)
	{
		base.GetComponent<BoxCollider2D>().enabled = true;
		this.currentPlayer = PlayerManager.GetNext();
		this.primaryAttackDelay = properties.CurrentState.hose.attackDelayRange.RandomFloat();
		this.secondaryAttackDelay = properties.CurrentState.cannon.attackDelay;
		this.attackStringGroup = UnityEngine.Random.Range(0, properties.CurrentState.cannon.shootString.Length);
		this.attackStringIndex = UnityEngine.Random.Range(0, properties.CurrentState.cannon.shootString[this.attackStringGroup].Split(new char[]
		{
			','
		}).Length);
		parent.OnDeathEvent += this.OnPrimaryDeath;
		primaryHP = properties.CurrentState.hose.health;
		attackDelayMinus = properties.CurrentState.hose.delayMinus;
		base.InitBodyPart(parent, properties, primaryHP, secondaryHP, attackDelayMinus);
		this.StartPrimary();
	}

	// Token: 0x060029E2 RID: 10722 RVA: 0x00187B38 File Offset: 0x00185F38
	protected override void OnPrimaryAttack()
	{
		if (this.currentPlayer == null || this.currentPlayer.IsDead)
		{
			this.currentPlayer = PlayerManager.GetNext();
		}
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			if (this.currentPlayer.id == PlayerId.PlayerOne)
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					this.currentPlayer = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
				}
			}
			else
			{
				this.currentPlayer = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			}
			base.StartCoroutine(this.warningLaser_cr());
			base.OnPrimaryAttack();
		}
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x00187BD0 File Offset: 0x00185FD0
	private IEnumerator warningLaser_cr()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.hose.warningDuration);
			if (this.current == RobotLevelRobotBodyPart.state.primary)
			{
				if (this.currentPlayer == null || this.currentPlayer.IsDead)
				{
					this.currentPlayer = PlayerManager.GetNext();
				}
				Vector3 dir = (this.currentPlayer.center - base.transform.position).normalized;
				this.angle = Vector3.Angle(Vector3.up, dir);
				if (this.angle < 0f)
				{
					this.angle *= -1f;
				}
				this.angle = Mathf.Clamp(this.angle, this.properties.CurrentState.hose.aimAngleParameter.min, this.properties.CurrentState.hose.aimAngleParameter.max);
				yield return null;
				this.laser = this.primary.GetComponent<RobotLevelHoseLaser>().Create(base.transform.position, this.angle - 90f, this);
				this.laser.animator.SetTrigger("OnWarning");
				AudioManager.Play("robot_raygun_charge");
				this.emitAudioFromObject.Add("robot_raygun_charge");
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.hose.warningDuration);
			if (this.current == RobotLevelRobotBodyPart.state.primary)
			{
				this.laser.animator.SetTrigger("OnAttack");
				AudioManager.Play("robot_raygun_shoot");
				this.emitAudioFromObject.Add("robot_raygun_shoot");
				yield return null;
			}
			else
			{
				AudioManager.Stop("robot_raygun_charge");
			}
			yield return null;
		}
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			base.StartCoroutine(this.attackLaser_cr());
		}
		yield break;
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x00187BEC File Offset: 0x00185FEC
	private IEnumerator attackLaser_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, (float)this.properties.CurrentState.hose.beamDuration);
		AudioManager.Stop("robot_raygun_charge");
		if (this.laser != null)
		{
			UnityEngine.Object.Destroy(this.laser.gameObject);
			this.isAttacking = false;
		}
		if ((float)UnityEngine.Random.Range(0, 100) <= 25f && !AudioManager.CheckIfPlaying("robot_vocals_laugh"))
		{
			AudioManager.Play("robot_vocals_laugh");
			this.emitAudioFromObject.Add("robot_vocals_laugh");
		}
		yield break;
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x00187C08 File Offset: 0x00186008
	protected override void OnPrimaryDeath()
	{
		if (this.current != RobotLevelRobotBodyPart.state.secondary && this.currentHealth[0] <= 0f)
		{
			this.parent.animator.SetBool("HeadStageTwoTransition", true);
			base.StartCoroutine(this.endLasers_cr());
		}
		base.OnPrimaryDeath();
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x00187C5C File Offset: 0x0018605C
	private IEnumerator endLasers_cr()
	{
		yield return this.parent.animator.WaitForAnimationToEnd(this.parent, "Idle", 2, true, true);
		AudioManager.Play("robot_head_antennae_destroyed");
		base.GetComponent<BoxCollider2D>().enabled = false;
		base.StopCoroutine(this.warningLaser_cr());
		base.StopCoroutine(this.attackLaser_cr());
		this.ExitCurrentAttacks();
		this.StartSecondary();
		this.deathEffect.Create(base.transform.position);
		AudioManager.Play("robot_upper_chest_port_destroyed");
		this.emitAudioFromObject.Add("robot_upper_chest_port_destroyed");
		yield break;
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x00187C78 File Offset: 0x00186078
	private IEnumerator startSecondary_cr()
	{
		this.StartSecondary();
		yield return null;
		yield break;
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x00187C94 File Offset: 0x00186094
	protected override void OnSecondaryAttack()
	{
		this.secondaryAttackDelay = this.properties.CurrentState.cannon.attackDelay;
		string attackString = this.properties.CurrentState.cannon.shootString[this.attackStringGroup].Split(new char[]
		{
			','
		})[this.attackStringIndex];
		this.attackStringIndex++;
		if (this.attackStringIndex >= this.properties.CurrentState.cannon.shootString[this.attackStringGroup].Split(new char[]
		{
			','
		}).Length - 1)
		{
			this.secondaryAttackDelay = this.properties.CurrentState.cannon.attackDelay;
			this.attackStringIndex = 0;
			this.attackStringGroup++;
			if (this.attackStringGroup >= this.properties.CurrentState.cannon.shootString.Length - 1)
			{
				this.attackStringGroup = 0;
				this.secondaryAttackDelay = this.properties.CurrentState.cannon.attackDelayRange.RandomFloat();
			}
		}
		this.parent.animator.SetTrigger("HeadAttack");
		base.StartCoroutine(this.spreadShot_cr(attackString));
		base.OnSecondaryAttack();
	}

	// Token: 0x060029E9 RID: 10729 RVA: 0x00187DE0 File Offset: 0x001861E0
	private IEnumerator spreadShot_cr(string attackString)
	{
		yield return this.parent.animator.WaitForAnimationToEnd(this, "Stage Two Idle", 2, true, true);
		this.cannonSpreadShot(attackString);
		yield break;
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x00187E04 File Offset: 0x00186204
	private void cannonSpreadShot(string attackString)
	{
		int num = 0;
		Parser.IntTryParse(attackString.Substring(1), out num);
		num--;
		string[] array = this.properties.CurrentState.cannon.spreadVariableGroups[num].Split(new char[]
		{
			','
		});
		float speed = 0f;
		int num2 = 0;
		MinMax minMax = new MinMax(0f, 0f);
		foreach (string text in array)
		{
			if (text[0] == 'S')
			{
				Parser.FloatTryParse(text.Substring(1), out speed);
			}
			else if (text[0] == 'N')
			{
				Parser.IntTryParse(text.Substring(1), out num2);
			}
			else
			{
				string[] array3 = text.Split(new char[]
				{
					'-'
				});
				Parser.FloatTryParse(array3[0], out minMax.min);
				Parser.FloatTryParse(array3[1], out minMax.max);
			}
		}
		AudioManager.Play("robot_head_shoot");
		this.emitAudioFromObject.Add("robot_head_shoot");
		for (int j = 0; j < num2; j++)
		{
			float floatAt = minMax.GetFloatAt((float)j / ((float)num2 - 1f));
			if (j % 2 == 0)
			{
				BasicProjectile component = this.secondary.GetComponent<BasicProjectile>();
				component.Create(base.transform.position, floatAt, speed);
			}
			else
			{
				this.nutProjectile.Create(base.transform.position, floatAt, speed);
			}
		}
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x00187FA2 File Offset: 0x001863A2
	protected override void ExitCurrentAttacks()
	{
		if (this.laser != null)
		{
			UnityEngine.Object.Destroy(this.laser.gameObject);
		}
		base.ExitCurrentAttacks();
	}

	// Token: 0x040032C9 RID: 13001
	private RobotLevelHoseLaser laser;

	// Token: 0x040032CA RID: 13002
	private AbstractPlayerController currentPlayer;

	// Token: 0x040032CB RID: 13003
	private float angle;

	// Token: 0x040032CC RID: 13004
	private int attackStringGroup;

	// Token: 0x040032CD RID: 13005
	private int attackStringIndex;

	// Token: 0x040032CE RID: 13006
	[SerializeField]
	private BasicProjectile nutProjectile;
}
