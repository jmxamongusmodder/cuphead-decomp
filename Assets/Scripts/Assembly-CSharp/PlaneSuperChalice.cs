using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AAD RID: 2733
public class PlaneSuperChalice : AbstractPlaneSuper
{
	// Token: 0x0600419B RID: 16795 RVA: 0x00238554 File Offset: 0x00236954
	protected override void Start()
	{
		base.Start();
		this.boom.gameObject.SetActive(true);
		this.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.player.stats.OnStoned += this.OnStoned;
	}

	// Token: 0x0600419C RID: 16796 RVA: 0x002385B0 File Offset: 0x002369B0
	private void FixedUpdate()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.HandleInput();
		if (!this.exploded)
		{
			this.Move();
		}
		this.ClampPosition();
	}

	// Token: 0x0600419D RID: 16797 RVA: 0x002385E8 File Offset: 0x002369E8
	private void EndIntroAnimation()
	{
		base.SnapshotAudio();
		if (this.player != null)
		{
			this.player.UnpauseAll(false);
		}
		this.animHelper.IgnoreGlobal = false;
		PauseManager.Unpause();
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x0600419E RID: 16798 RVA: 0x00238636 File Offset: 0x00236A36
	private void OnStoned()
	{
		this.exploded = true;
	}

	// Token: 0x0600419F RID: 16799 RVA: 0x0023863F File Offset: 0x00236A3F
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.exploded = true;
	}

	// Token: 0x060041A0 RID: 16800 RVA: 0x00238648 File Offset: 0x00236A48
	private IEnumerator super_cr()
	{
		this.player.damageReceiver.Vulnerable();
		this.respawnPos = base.transform.position;
		this.state = PlanePlayerWeaponManager.States.Super.Countdown;
		this.damageDealer = new DamageDealer(WeaponProperties.PlaneSuperChaliceSuperBomb.damage, WeaponProperties.PlaneSuperChaliceSuperBomb.damageRate, DamageDealer.DamageSource.Super, false, true, true);
		this.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
		this.damageDealer.PlayerId = this.player.id;
		MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		tracker.Add(this.damageDealer);
		this.curAngle = MathUtils.DirectionToAngle(Vector3.right);
		this.curSpeed = 0f;
		while (!this.exploded)
		{
			if (this.player != null)
			{
				this.player.transform.position = base.transform.position;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		this.respawnPos = base.transform.position;
		this.Fire();
		if (this.player != null)
		{
			this.player.PauseAll();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		base.animator.SetTrigger("Explode");
		AudioManager.Play("player_plane_bomb_explosion");
		AudioManager.Stop("player_plane_bomb_ticktock_loop");
		yield break;
	}

	// Token: 0x060041A1 RID: 16801 RVA: 0x00238663 File Offset: 0x00236A63
	protected override void Fire()
	{
		base.Fire();
	}

	// Token: 0x060041A2 RID: 16802 RVA: 0x0023866C File Offset: 0x00236A6C
	private void PlayerReappear()
	{
		this.RestoreAudio(true);
		if (this.player == null)
		{
			return;
		}
		this.player.motor.OnRevive(this.respawnPos);
		this.player.UnpauseAll(false);
		this.player.SetSpriteVisible(true);
		this.player.damageReceiver.Invulnerable(2f);
	}

	// Token: 0x060041A3 RID: 16803 RVA: 0x002386DA File Offset: 0x00236ADA
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060041A4 RID: 16804 RVA: 0x002386E7 File Offset: 0x00236AE7
	private void StartBoomScale()
	{
		this.boomRoutine = base.StartCoroutine(this.boomScale_cr());
	}

	// Token: 0x060041A5 RID: 16805 RVA: 0x002386FC File Offset: 0x00236AFC
	private IEnumerator boomScale_cr()
	{
		float t = 0f;
		float frameTime = 0.041666668f;
		float scale = 1f;
		for (;;)
		{
			t += CupheadTime.Delta;
			while (t > frameTime)
			{
				t -= frameTime;
				scale *= 1.15f;
				this.boom.SetScale(new float?(scale), new float?(scale), null);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060041A6 RID: 16806 RVA: 0x00238717 File Offset: 0x00236B17
	public void Pause()
	{
		if (this.boomRoutine != null)
		{
			base.StopCoroutine(this.boomRoutine);
		}
	}

	// Token: 0x060041A7 RID: 16807 RVA: 0x00238730 File Offset: 0x00236B30
	private void HandleInput()
	{
		Trilean trilean = 0;
		Trilean t = 0;
		float num = 0f;
		if (this.player != null)
		{
			num = this.player.input.actions.GetAxis(1);
		}
		if (num > 0.35f || num < -0.35f)
		{
			t = num;
		}
		this.curAngle += t * WeaponProperties.PlaneSuperChaliceSuperBomb.turnRate;
		this.curAngle = Mathf.Clamp(this.curAngle, -WeaponProperties.PlaneSuperChaliceSuperBomb.maxAngle, WeaponProperties.PlaneSuperChaliceSuperBomb.maxAngle);
		this.curAngle *= WeaponProperties.PlaneSuperChaliceSuperBomb.angleDamp;
		this.accelDirection = MathUtils.AngleToDirection(this.curAngle);
		base.animator.SetInteger("Y", t);
	}

	// Token: 0x060041A8 RID: 16808 RVA: 0x00238804 File Offset: 0x00236C04
	private void Move()
	{
		Vector2 b = base.transform.position;
		this.moveDirection = this.accelDirection * this.curSpeed;
		this.curSpeed += WeaponProperties.PlaneSuperChaliceSuperBomb.accel * CupheadTime.FixedDelta;
		base.transform.AddPosition(this.moveDirection.x * CupheadTime.FixedDelta, this.moveDirection.y * CupheadTime.FixedDelta, 0f);
		Vector2 a = base.transform.position;
		this._velocity = (a - b) / CupheadTime.FixedDelta;
	}

	// Token: 0x060041A9 RID: 16809 RVA: 0x002388AC File Offset: 0x00236CAC
	private void ClampPosition()
	{
		Vector2 rhs = base.transform.position;
		rhs.x = Mathf.Clamp(rhs.x, (float)Level.Current.Left, (float)Level.Current.Right - 30f);
		rhs.y = Mathf.Clamp(rhs.y, (float)Level.Current.Ground, (float)Level.Current.Ceiling);
		if (base.transform.position != rhs)
		{
			this.exploded = true;
		}
	}

	// Token: 0x060041AA RID: 16810 RVA: 0x00238944 File Offset: 0x00236D44
	private void CheckPosition()
	{
		Vector2 rhs = base.transform.position;
		rhs.x = Mathf.Clamp(rhs.x, (float)Level.Current.Left - 350f, (float)Level.Current.Right + 150f);
		rhs.y = Mathf.Clamp(rhs.y, (float)Level.Current.Ground - 175f, (float)Level.Current.Ceiling + 325f);
		if (base.transform.position != rhs)
		{
			this.missed = true;
		}
	}

	// Token: 0x060041AB RID: 16811 RVA: 0x002389EE File Offset: 0x00236DEE
	protected virtual void RestoreAudio(bool changePitch = true)
	{
		AudioManager.SnapshotReset(SceneLoader.SceneName, 2f);
		if (changePitch)
		{
			AudioManager.ChangeBGMPitch(1f, 2f);
		}
	}

	// Token: 0x060041AC RID: 16812 RVA: 0x00238A14 File Offset: 0x00236E14
	protected override void OnDestroy()
	{
		this.RestoreAudio(true);
		base.OnDestroy();
		if (this.player != null)
		{
			this.player.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
			this.player.stats.OnStoned -= this.OnStoned;
		}
	}

	// Token: 0x04004813 RID: 18451
	private const float ANALOG_THRESHOLD = 0.35f;

	// Token: 0x04004814 RID: 18452
	private const float PADDING_TOP = 65f;

	// Token: 0x04004815 RID: 18453
	private const float PADDING_BOTTOM = 35f;

	// Token: 0x04004816 RID: 18454
	private const float PADDING_LEFT = 70f;

	// Token: 0x04004817 RID: 18455
	private const float PADDING_RIGHT = 30f;

	// Token: 0x04004818 RID: 18456
	private bool superHappening;

	// Token: 0x04004819 RID: 18457
	private bool invulnerable;

	// Token: 0x0400481A RID: 18458
	private float timer;

	// Token: 0x0400481B RID: 18459
	private Vector2 accelDirection;

	// Token: 0x0400481C RID: 18460
	private Vector2 moveDirection;

	// Token: 0x0400481D RID: 18461
	private Vector2 _velocity;

	// Token: 0x0400481E RID: 18462
	private DamageReceiver damageReceiver;

	// Token: 0x0400481F RID: 18463
	private bool exploded;

	// Token: 0x04004820 RID: 18464
	private bool missed;

	// Token: 0x04004821 RID: 18465
	private Coroutine boomRoutine;

	// Token: 0x04004822 RID: 18466
	[SerializeField]
	private Transform boom;

	// Token: 0x04004823 RID: 18467
	private float curAngle;

	// Token: 0x04004824 RID: 18468
	private float curSpeed;

	// Token: 0x04004825 RID: 18469
	private Vector2 respawnPos;
}
