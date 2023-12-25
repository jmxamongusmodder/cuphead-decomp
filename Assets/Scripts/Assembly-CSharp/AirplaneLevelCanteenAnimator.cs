using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class AirplaneLevelCanteenAnimator : LevelProperties.Airplane.Entity
{
	// Token: 0x060013F7 RID: 5111 RVA: 0x000B16B4 File Offset: 0x000AFAB4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x000B16C4 File Offset: 0x000AFAC4
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.level = (Level.Current as AirplaneLevel);
		this.curState = properties.CurrentState.stateName;
		this.idleLoops = UnityEngine.Random.Range(3, 6);
		base.StartCoroutine(this.check_players_cr());
		base.StartCoroutine(this.handle_canteen_cr());
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x000B1720 File Offset: 0x000AFB20
	private IEnumerator check_players_cr()
	{
		while (this.p1health == -1)
		{
			this.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			this.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			this.p1health = ((!this.player1) ? -1 : PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health);
			this.p2health = ((!this.player2) ? -1 : PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x000B173C File Offset: 0x000AFB3C
	private void OnPlayerHit(bool dead)
	{
		base.animator.Play((!dead) ? ((!this.playerHitAltAnim) ? "CanteenHitB" : "CanteenHitA") : "CanteenOnePlayerDied");
		base.animator.SetBool("CanteenTrackBoss", false);
		if (!dead)
		{
			this.playerHitAltAnim = !this.playerHitAltAnim;
		}
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x000B17A4 File Offset: 0x000AFBA4
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		if (!Level.Won)
		{
			base.animator.SetBool("CanteenTrackBoss", false);
			base.animator.Play("CanteenAllPlayersDied");
		}
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x000B17D8 File Offset: 0x000AFBD8
	private IEnumerator handle_canteen_cr()
	{
		for (;;)
		{
			if (Level.Won)
			{
				base.animator.SetBool("CanteenTrackBoss", false);
				base.animator.Play("CanteenWin");
			}
			else if (this.triggerCheer)
			{
				base.animator.SetBool("CanteenTrackBoss", false);
				base.animator.Play("CanteenCheer");
				this.triggerCheer = false;
			}
			else
			{
				this.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
				this.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
				if (this.player1)
				{
					if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health < this.p1health)
					{
						this.p1health = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health;
						this.OnPlayerHit(this.p1health == 0);
					}
					else if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health > this.p1health)
					{
						base.animator.SetBool("CanteenTrackBoss", false);
						base.animator.Play("CanteenCheer");
					}
					this.p1health = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health;
				}
				if (this.player2)
				{
					if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health < this.p2health)
					{
						this.p2health = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health;
						this.OnPlayerHit(this.p2health == 0);
					}
					else if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health > this.p2health)
					{
						base.animator.SetBool("CanteenTrackBoss", false);
						base.animator.Play("CanteenCheer");
					}
					this.p2health = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x000B17F4 File Offset: 0x000AFBF4
	private void LookAtBoss()
	{
		switch (base.properties.CurrentState.stateName)
		{
		case LevelProperties.Airplane.States.Main:
		case LevelProperties.Airplane.States.Generic:
		case LevelProperties.Airplane.States.Rocket:
			if (this.level.CurrentEnemyPos().x - base.transform.position.x < -250f)
			{
				base.animator.Play("CanteenLookUpLeft");
				base.animator.SetInteger("CanteenLookUpDir", -1);
				base.animator.SetBool("CanteenTrackBoss", true);
			}
			else if (this.level.CurrentEnemyPos().x - base.transform.position.x > 250f)
			{
				base.animator.Play("CanteenLookUpRight");
				base.animator.SetInteger("CanteenLookUpDir", 1);
				base.animator.SetBool("CanteenTrackBoss", true);
			}
			else
			{
				base.animator.Play("CanteenLookUp");
				base.animator.SetInteger("CanteenLookUpDir", 0);
				base.animator.SetBool("CanteenTrackBoss", true);
			}
			break;
		case LevelProperties.Airplane.States.Terriers:
			base.animator.SetBool("CanteenTrackBoss", false);
			switch (UnityEngine.Random.Range(0, 6))
			{
			case 0:
				base.animator.Play("CanteenLookUpLeft");
				break;
			case 1:
				base.animator.Play("CanteenLookUp");
				break;
			case 2:
				base.animator.Play("CanteenLookUpRight");
				break;
			case 3:
				base.animator.Play("CanteenLookDownRight");
				break;
			case 4:
				base.animator.Play("CanteenLookDown");
				break;
			case 5:
				base.animator.Play("CanteenLookDownLeft");
				break;
			}
			break;
		case LevelProperties.Airplane.States.Leader:
			if (this.level.ScreenHorizontal())
			{
				if (this.level.CurrentEnemyPos().x - base.transform.position.x < -100f)
				{
					base.animator.Play("CanteenLookUpLeft");
					base.animator.SetInteger("CanteenLookUpDir", -1);
					base.animator.SetBool("CanteenTrackBoss", true);
				}
				else if (this.level.CurrentEnemyPos().x - base.transform.position.x > 100f)
				{
					base.animator.Play("CanteenLookUpRight");
					base.animator.SetInteger("CanteenLookUpDir", 1);
					base.animator.SetBool("CanteenTrackBoss", true);
				}
				else
				{
					base.animator.Play("CanteenLookUp");
					base.animator.SetInteger("CanteenLookUpDir", 0);
					base.animator.SetBool("CanteenTrackBoss", true);
				}
			}
			else
			{
				int num = UnityEngine.Random.Range(0, 2);
				if (num != 0)
				{
					if (num == 1)
					{
						base.animator.Play("CanteenLookUpRight");
					}
				}
				else
				{
					base.animator.Play("CanteenLookUpLeft");
				}
			}
			break;
		}
		this.lookLoops = UnityEngine.Random.Range(7, 9);
	}

	// Token: 0x060013FE RID: 5118 RVA: 0x000B1B6F File Offset: 0x000AFF6F
	public void ForceLook(Vector3 target, int loops)
	{
		this.lookLoops = loops;
		this.idleLoops = 1;
		this.idleClipPos = -1;
		this.forceLookTarget = target;
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x000B1B90 File Offset: 0x000AFF90
	private void LookInDirection()
	{
		base.animator.SetBool("CanteenTrackBoss", false);
		switch ((int)(((double)Vector3.SignedAngle(Vector3.up, this.forceLookTarget - base.transform.position, Vector3.back) + 202.5) % 360.0) / 45)
		{
		case 0:
			base.animator.Play("CanteenLookDown");
			break;
		case 1:
			base.animator.Play("CanteenLookDownLeft");
			break;
		case 2:
		case 3:
			base.animator.Play("CanteenLookUpLeft");
			break;
		case 4:
			base.animator.Play("CanteenLookUp");
			break;
		case 5:
		case 6:
			base.animator.Play("CanteenLookUpRight");
			break;
		case 7:
			base.animator.Play("CanteenLookDownRight");
			break;
		}
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x000B1C98 File Offset: 0x000B0098
	private void OnCanteenIdleLoop()
	{
		this.idleLoops--;
		if (this.idleLoops == 0)
		{
			int num = this.idleClipPos;
			switch (num + 1)
			{
			case 0:
				this.LookInDirection();
				base.animator.SetBool("CanteenTrackBoss", false);
				break;
			case 1:
				base.animator.SetTrigger("CanteenBlink");
				base.animator.SetBool("CanteenTrackBoss", false);
				break;
			case 2:
				if (UnityEngine.Random.Range(0, (base.properties.CurrentState.stateName != LevelProperties.Airplane.States.Terriers) ? 10 : 4) == 0)
				{
					this.LookAtBoss();
				}
				else
				{
					base.animator.SetTrigger("CanteenGlanceAround");
					base.animator.SetBool("CanteenTrackBoss", false);
				}
				break;
			case 3:
				base.animator.SetTrigger("CanteenBlink");
				base.animator.SetBool("CanteenTrackBoss", false);
				break;
			case 4:
				this.LookAtBoss();
				break;
			}
			this.idleClipPos = (this.idleClipPos + 1) % 4;
			this.idleLoops = UnityEngine.Random.Range(3, 6);
		}
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x000B1DD0 File Offset: 0x000B01D0
	private void OnCanteenLookLoop()
	{
		this.lookLoops--;
		if (this.lookLoops <= 0)
		{
			base.animator.SetTrigger("CanteenEndLookLoop");
		}
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x000B1DFC File Offset: 0x000B01FC
	private void Update()
	{
		if (base.animator.GetBool("CanteenTrackBoss"))
		{
			switch (base.properties.CurrentState.stateName)
			{
			case LevelProperties.Airplane.States.Main:
			case LevelProperties.Airplane.States.Generic:
			case LevelProperties.Airplane.States.Rocket:
				if (this.level.CurrentEnemyPos().x - base.transform.position.x < -250f)
				{
					base.animator.SetInteger("CanteenLookUpDir", -1);
				}
				else if (this.level.CurrentEnemyPos().x - base.transform.position.x > 250f)
				{
					base.animator.SetInteger("CanteenLookUpDir", 1);
				}
				else
				{
					base.animator.SetInteger("CanteenLookUpDir", 0);
				}
				break;
			case LevelProperties.Airplane.States.Leader:
				if (this.level.ScreenHorizontal())
				{
					if (this.level.CurrentEnemyPos().x - base.transform.position.x < -100f)
					{
						base.animator.SetInteger("CanteenLookUpDir", -1);
					}
					else if (this.level.CurrentEnemyPos().x - base.transform.position.x > 100f)
					{
						base.animator.SetInteger("CanteenLookUpDir", 1);
					}
					else
					{
						base.animator.SetInteger("CanteenLookUpDir", 0);
					}
				}
				break;
			}
		}
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x000B1FAB File Offset: 0x000B03AB
	private void WORKAROUND_NullifyFields()
	{
		this.player1 = null;
		this.player2 = null;
		this.level = null;
	}

	// Token: 0x04001D16 RID: 7446
	private const int MIN_IDLE_LOOPS = 3;

	// Token: 0x04001D17 RID: 7447
	private const int MAX_IDLE_LOOPS = 6;

	// Token: 0x04001D18 RID: 7448
	private const int MIN_LOOK_LOOPS = 7;

	// Token: 0x04001D19 RID: 7449
	private const int MAX_LOOK_LOOPS = 9;

	// Token: 0x04001D1A RID: 7450
	private const float PHASE_ONE_LOOK_ANGLE_THRESHOLD = 250f;

	// Token: 0x04001D1B RID: 7451
	private const float PHASE_THREE_LOOK_ANGLE_THRESHOLD = 100f;

	// Token: 0x04001D1C RID: 7452
	private int idleLoops;

	// Token: 0x04001D1D RID: 7453
	private int idleClipPos;

	// Token: 0x04001D1E RID: 7454
	private int lookLoops;

	// Token: 0x04001D1F RID: 7455
	private Vector3 forceLookTarget;

	// Token: 0x04001D20 RID: 7456
	private AbstractPlayerController player1;

	// Token: 0x04001D21 RID: 7457
	private AbstractPlayerController player2;

	// Token: 0x04001D22 RID: 7458
	private int p1health = -1;

	// Token: 0x04001D23 RID: 7459
	private int p2health = -1;

	// Token: 0x04001D24 RID: 7460
	private bool playerHitAltAnim;

	// Token: 0x04001D25 RID: 7461
	public bool triggerCheer;

	// Token: 0x04001D26 RID: 7462
	private LevelProperties.Airplane.States curState;

	// Token: 0x04001D27 RID: 7463
	private AirplaneLevel level;
}
