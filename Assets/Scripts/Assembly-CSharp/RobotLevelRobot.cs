using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000772 RID: 1906
public class RobotLevelRobot : LevelProperties.Robot.Entity
{
	// Token: 0x14000046 RID: 70
	// (add) Token: 0x06002985 RID: 10629 RVA: 0x00184004 File Offset: 0x00182404
	// (remove) Token: 0x06002986 RID: 10630 RVA: 0x0018403C File Offset: 0x0018243C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x06002987 RID: 10631 RVA: 0x00184074 File Offset: 0x00182474
	// (remove) Token: 0x06002988 RID: 10632 RVA: 0x001840AC File Offset: 0x001824AC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPrimaryDeathEvent;

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x06002989 RID: 10633 RVA: 0x001840E4 File Offset: 0x001824E4
	// (remove) Token: 0x0600298A RID: 10634 RVA: 0x0018411C File Offset: 0x0018251C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSecondaryDeathEvent;

	// Token: 0x14000049 RID: 73
	// (add) Token: 0x0600298B RID: 10635 RVA: 0x00184154 File Offset: 0x00182554
	// (remove) Token: 0x0600298C RID: 10636 RVA: 0x0018418C File Offset: 0x0018258C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private event Action callback;

	// Token: 0x0600298D RID: 10637 RVA: 0x001841C4 File Offset: 0x001825C4
	protected override void Awake()
	{
		foreach (CollisionChild s in this.collisionChilds)
		{
			base.RegisterCollisionChild(s);
		}
		base.Awake();
	}

	// Token: 0x0600298E RID: 10638 RVA: 0x00184200 File Offset: 0x00182600
	public override void LevelInit(LevelProperties.Robot properties)
	{
		Level.Current.OnIntroEvent += this.OnIntro;
		if (Level.Current.mode == Level.Mode.Easy)
		{
			Level.Current.OnWinEvent += this.OnDeathDance;
		}
		this.damageDealer = DamageDealer.NewEnemy();
		this.walkPCT = (this.walkTime = 0f);
		base.StartCoroutine(this.disableIntro_cr());
		base.LevelInit(properties);
	}

	// Token: 0x0600298F RID: 10639 RVA: 0x0018427C File Offset: 0x0018267C
	private IEnumerator disableIntro_cr()
	{
		yield return new WaitForEndOfFrame();
		base.animator.enabled = false;
		yield break;
	}

	// Token: 0x06002990 RID: 10640 RVA: 0x00184298 File Offset: 0x00182698
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.introEnded)
		{
			float num = Mathf.Max(PlayerManager.GetNext().center.x, PlayerManager.GetNext().center.x);
			if (num > base.transform.position.x)
			{
				this.UpdatePosition(true);
			}
			else
			{
				this.UpdatePosition(false);
			}
		}
	}

	// Token: 0x06002991 RID: 10641 RVA: 0x0018431C File Offset: 0x0018271C
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x0018433C File Offset: 0x0018273C
	private void IntroEnded()
	{
		AudioManager.Play("robot_vocals_laugh");
		this.emitAudioFromObject.Add("robot_vocals_laugh");
		this.introEnded = true;
		base.animator.SetBool("MainAnimationActive", false);
		this.head.GetComponent<RobotLevelRobotHead>().InitBodyPart(this, base.properties, 1, 1, 0f);
		this.chest.GetComponent<RobotLevelRobotChest>().InitBodyPart(this, base.properties, 0, 1, 0f);
		this.hatch.GetComponent<RobotLevelRobotHatch>().InitBodyPart(this, base.properties, 0, 1, 0f);
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x001843D5 File Offset: 0x001827D5
	public void TriggerPhaseTwo(Action callback)
	{
		base.animator.Play("Phase2 Transition", 2);
		this.callback = callback;
	}

	// Token: 0x06002994 RID: 10644 RVA: 0x001843EF File Offset: 0x001827EF
	private void OnDeathDance()
	{
		this.chest.animator.Play("Off", 1);
		base.animator.Play("Death Dance");
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x06002995 RID: 10645 RVA: 0x00184424 File Offset: 0x00182824
	private IEnumerator death_cr()
	{
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < 3; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(false);
		}
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		if (Level.Current.mode != Level.Mode.Easy && this.callback != null)
		{
			this.callback();
		}
		yield break;
	}

	// Token: 0x06002996 RID: 10646 RVA: 0x0018443F File Offset: 0x0018283F
	private void OnRobotIntro()
	{
		this.chest.GetComponent<RobotLevelRobotChest>().InitAnims();
		this.hatch.GetComponent<RobotLevelRobotHatch>().InitAnims();
	}

	// Token: 0x06002997 RID: 10647 RVA: 0x00184461 File Offset: 0x00182861
	private void OnIntro()
	{
		this.SoundRobotIntro();
		base.animator.enabled = true;
	}

	// Token: 0x06002998 RID: 10648 RVA: 0x00184478 File Offset: 0x00182878
	public void PrimaryDied()
	{
		if (this.OnPrimaryDeathEvent != null)
		{
			this.OnPrimaryDeathEvent();
		}
		this.remainingPrimaryAttacks--;
		if (this.remainingPrimaryAttacks <= 0 && this.OnSecondaryDeathEvent != null)
		{
			this.OnSecondaryDeathEvent();
		}
	}

	// Token: 0x06002999 RID: 10649 RVA: 0x001844CC File Offset: 0x001828CC
	private void UpdatePosition(bool closeGap)
	{
		float duration = 4f;
		float levelTime = Level.Current.LevelTime;
		if (closeGap)
		{
			Vector3 position = this.walkingPositions[0].position;
			Vector3 position2 = this.walkingPositions[1].position;
			this.Move(position, position2, duration, 1);
		}
		else
		{
			Vector3 position = this.walkingPositions[1].position;
			Vector3 position2 = this.walkingPositions[0].position;
			this.Move(position, position2, duration, -1);
		}
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x00184544 File Offset: 0x00182944
	private void Move(Vector3 startPosition, Vector3 endPosition, float duration, int direction)
	{
		this.walkTime += CupheadTime.Delta * (float)direction;
		if (direction < 0)
		{
			if (this.walkTime <= 0f)
			{
				this.walkTime = 0f;
			}
		}
		else if (this.walkTime >= duration)
		{
			this.walkTime = duration;
		}
		this.walkPCT = this.walkTime / duration;
		if (this.walkPCT >= 1f)
		{
			this.walkPCT = 1f;
		}
		if (direction < 0)
		{
			this.walkPCT = 1f - this.walkPCT;
		}
		base.transform.position = startPosition + (endPosition - startPosition) * this.walkPCT;
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x0018460D File Offset: 0x00182A0D
	private void SpawnSmoke()
	{
		this.headcannonSmoke.Create(this.head.transform.position);
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x0018462B File Offset: 0x00182A2B
	private void OnDeathSFX()
	{
		AudioManager.Play("robot_vocals_dying");
		this.emitAudioFromObject.Add("robot_vocals_dying");
	}

	// Token: 0x0600299D RID: 10653 RVA: 0x00184647 File Offset: 0x00182A47
	private void SoundRobotIntro()
	{
		AudioManager.Play("robot_intro");
		this.emitAudioFromObject.Add("robot_intro");
	}

	// Token: 0x04003281 RID: 12929
	private Action attackCallback;

	// Token: 0x04003286 RID: 12934
	[SerializeField]
	private Effect headcannonSmoke;

	// Token: 0x04003287 RID: 12935
	[SerializeField]
	private Transform[] walkingPositions;

	// Token: 0x04003288 RID: 12936
	[SerializeField]
	private RobotLevelRobotHead head;

	// Token: 0x04003289 RID: 12937
	[SerializeField]
	private RobotLevelRobotBodyPart chest;

	// Token: 0x0400328A RID: 12938
	[SerializeField]
	private RobotLevelRobotHatch hatch;

	// Token: 0x0400328B RID: 12939
	[Space(10f)]
	[SerializeField]
	private GameObject finalForm;

	// Token: 0x0400328C RID: 12940
	private bool introEnded;

	// Token: 0x0400328D RID: 12941
	private float walkPCT;

	// Token: 0x0400328E RID: 12942
	private float walkTime;

	// Token: 0x0400328F RID: 12943
	private int remainingPrimaryAttacks = 3;

	// Token: 0x04003290 RID: 12944
	private DamageDealer damageDealer;

	// Token: 0x04003291 RID: 12945
	[SerializeField]
	private CollisionChild[] collisionChilds;
}
