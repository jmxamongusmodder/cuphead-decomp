using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200074F RID: 1871
public class RetroArcadeBigRobot : RetroArcadeEnemy
{
	// Token: 0x060028CC RID: 10444 RVA: 0x0017BFDC File Offset: 0x0017A3DC
	public RetroArcadeBigRobot Create(float xPos, LevelProperties.RetroArcade.Robots properties, float sinOffset, RetroArcadeRobotManager manager, string[] orbiterPattern)
	{
		RetroArcadeBigRobot retroArcadeBigRobot = this.InstantiatePrefab<RetroArcadeBigRobot>();
		retroArcadeBigRobot.t = sinOffset * 3.1415927f * 2f;
		float num = this.OFFSCREEN_Y + properties.smallRobotRotationDistance - properties.mainRobotY.min;
		retroArcadeBigRobot.properties = properties;
		retroArcadeBigRobot.transform.position = new Vector2(xPos, retroArcadeBigRobot.getYPos(retroArcadeBigRobot.t) + num);
		retroArcadeBigRobot.hp = properties.mainRobotHp;
		retroArcadeBigRobot.manager = manager;
		float num2 = sinOffset * 360f;
		retroArcadeBigRobot.orbiters = new RetroArcadeOrbiterRobot[3];
		for (int i = 0; i < 3; i++)
		{
			int num3;
			if (Parser.IntTryParse(orbiterPattern[i], out num3) && num3 > 0 && num3 <= this.orbiterPrefabs.Length)
			{
				retroArcadeBigRobot.orbiters[i] = retroArcadeBigRobot.orbiterPrefabs[num3 - 1].Create(retroArcadeBigRobot, properties, num2);
				num2 += 120f;
			}
		}
		retroArcadeBigRobot.MoveY(-num, properties.mainRobotMoveSpeed);
		retroArcadeBigRobot.StartCoroutine(retroArcadeBigRobot.shoot_cr());
		retroArcadeBigRobot.StartCoroutine(retroArcadeBigRobot.orbiterShoot_cr());
		return retroArcadeBigRobot;
	}

	// Token: 0x060028CD RID: 10445 RVA: 0x0017C0F5 File Offset: 0x0017A4F5
	protected override void Start()
	{
		base.PointsWorth = this.properties.pointsGained;
		base.PointsBonus = this.properties.pointsBonus;
	}

	// Token: 0x060028CE RID: 10446 RVA: 0x0017C11C File Offset: 0x0017A51C
	protected override void FixedUpdate()
	{
		if (this.movingY || this.groupDead)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta * (this.properties.mainRobotMoveSpeed / (this.properties.mainRobotY.max - this.properties.mainRobotY.min)) * 3.1415927f;
		base.transform.SetPosition(null, new float?(this.getYPos(this.t)), null);
		bool flag = true;
		foreach (RetroArcadeOrbiterRobot retroArcadeOrbiterRobot in this.orbiters)
		{
			if (!retroArcadeOrbiterRobot.IsDead)
			{
				flag = false;
			}
		}
		if (flag)
		{
			base.StartCoroutine(this.moveOffscreen_cr());
			this.groupDead = true;
			this.manager.OnRobotGroupDie();
		}
	}

	// Token: 0x060028CF RID: 10447 RVA: 0x0017C20C File Offset: 0x0017A60C
	private float getYPos(float t)
	{
		return this.properties.mainRobotY.GetFloatAt(Mathf.Sin(t) * 0.5f + 0.5f);
	}

	// Token: 0x060028D0 RID: 10448 RVA: 0x0017C230 File Offset: 0x0017A630
	private IEnumerator moveOffscreen_cr()
	{
		base.MoveY(this.OFFSCREEN_Y + this.properties.smallRobotRotationDistance - base.transform.position.y, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		foreach (RetroArcadeOrbiterRobot retroArcadeOrbiterRobot in this.orbiters)
		{
			UnityEngine.Object.Destroy(retroArcadeOrbiterRobot.gameObject);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060028D1 RID: 10449 RVA: 0x0017C24C File Offset: 0x0017A64C
	private IEnumerator shoot_cr()
	{
		while (this.movingY)
		{
			yield return null;
		}
		string[] pattern = this.properties.mainRobotShootString.Split(new char[]
		{
			','
		});
		int currentIndex = UnityEngine.Random.Range(0, pattern.Length);
		while (!base.IsDead)
		{
			float waitTime = 0f;
			Parser.FloatTryParse(pattern[currentIndex], out waitTime);
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			if (base.IsDead)
			{
				break;
			}
			float shootAngle = MathUtils.DirectionToAngle(PlayerManager.GetNext().center - this.projectileRoot.position);
			this.projectilePrefab.Create(this.projectileRoot.position, this.properties.mainRobotShootSpeed, shootAngle, this.properties.mainRobotShotBounce);
		}
		yield break;
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x0017C268 File Offset: 0x0017A668
	private IEnumerator orbiterShoot_cr()
	{
		while (this.movingY)
		{
			yield return null;
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.smallRobotAttackDelay.RandomFloat());
			List<RetroArcadeOrbiterRobot> aliveOrbiters = new List<RetroArcadeOrbiterRobot>();
			foreach (RetroArcadeOrbiterRobot retroArcadeOrbiterRobot in this.orbiters)
			{
				if (!retroArcadeOrbiterRobot.IsDead)
				{
					aliveOrbiters.Add(retroArcadeOrbiterRobot);
				}
			}
			if (aliveOrbiters.Count == 0)
			{
				break;
			}
			aliveOrbiters.RandomChoice<RetroArcadeOrbiterRobot>().Shoot();
		}
		yield break;
	}

	// Token: 0x040031A4 RID: 12708
	[SerializeField]
	private RetroArcadeOrbiterRobot[] orbiterPrefabs;

	// Token: 0x040031A5 RID: 12709
	[SerializeField]
	private RetroArcadeRobotBouncingProjectile projectilePrefab;

	// Token: 0x040031A6 RID: 12710
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x040031A7 RID: 12711
	private float OFFSCREEN_Y = 300f;

	// Token: 0x040031A8 RID: 12712
	private const float MOVE_OFFSCREEN_SPEED = 500f;

	// Token: 0x040031A9 RID: 12713
	private LevelProperties.RetroArcade.Robots properties;

	// Token: 0x040031AA RID: 12714
	private float t;

	// Token: 0x040031AB RID: 12715
	private RetroArcadeOrbiterRobot[] orbiters;

	// Token: 0x040031AC RID: 12716
	private RetroArcadeRobotManager manager;

	// Token: 0x040031AD RID: 12717
	private bool groupDead;
}
