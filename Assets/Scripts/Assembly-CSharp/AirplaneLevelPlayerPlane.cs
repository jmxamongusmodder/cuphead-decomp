using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C0 RID: 1216
public class AirplaneLevelPlayerPlane : LevelProperties.Airplane.Entity
{
	// Token: 0x0600143D RID: 5181 RVA: 0x000B475C File Offset: 0x000B2B5C
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.tiltableBasePos = this.tiltable.localPosition;
		this.maxParallaxX = CupheadLevelCamera.Current.Bounds.xMax - properties.CurrentState.plane.endScreenOffset;
		this.rotationDist = Vector3.Distance(this.edgeLeft.position, this.edgeRight.position);
		this.rotationVal = this.rotationDist / 2f;
		base.StartCoroutine(this.handle_player_move_cr());
		base.StartCoroutine(this.handle_tilt_cr());
		this.puffTimer[0] = 1f;
		this.puffTimer[1] = 0.8f;
		this.SFX_DOGFIGHT_PlayerPlane_Loop();
		this.SFX_DOGFIGHT_PlayerPlane_HighSpeed_Loop();
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x000B4820 File Offset: 0x000B2C20
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.player1 != null)
		{
			LevelPlayerWeaponManager component = this.player1.gameObject.GetComponent<LevelPlayerWeaponManager>();
			component.OnSuperStart -= this.StartP1Super;
			component.OnSuperEnd -= this.EndP1Super;
			component.OnExStart -= this.StartP1Super;
			component.OnExEnd -= this.EndP1Super;
		}
		if (this.player2 != null)
		{
			LevelPlayerWeaponManager component2 = this.player2.gameObject.GetComponent<LevelPlayerWeaponManager>();
			component2.OnSuperStart -= this.StartP2Super;
			component2.OnSuperEnd -= this.EndP2Super;
			component2.OnExStart -= this.StartP2Super;
			component2.OnExEnd -= this.EndP2Super;
		}
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x000B4910 File Offset: 0x000B2D10
	private void Update()
	{
		if (((AirplaneLevel)Level.Current).Rotating)
		{
			if (this.playerInSuper[0])
			{
				this.restorePlayerPos[0] = true;
			}
			if (this.playerInSuper[1])
			{
				this.restorePlayerPos[1] = true;
			}
		}
		if (this.player1 == null)
		{
			this.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			if (this.player1 != null)
			{
				LevelPlayerWeaponManager component = this.player1.gameObject.GetComponent<LevelPlayerWeaponManager>();
				component.OnSuperStart += this.StartP1Super;
				component.OnSuperEnd += this.EndP1Super;
				component.OnExStart += this.StartP1Super;
				component.OnExEnd += this.EndP1Super;
			}
		}
		if (this.player2 == null)
		{
			this.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (this.player2 != null)
			{
				LevelPlayerWeaponManager component2 = this.player2.gameObject.GetComponent<LevelPlayerWeaponManager>();
				component2.OnSuperStart += this.StartP2Super;
				component2.OnSuperEnd += this.EndP2Super;
				component2.OnExStart += this.StartP2Super;
				component2.OnExEnd += this.EndP2Super;
			}
		}
		if (this.player1 != null)
		{
			this.p1IsColliding = (this.player1.transform.parent == this.airplane1.transform);
			this.player1.transform.SetEulerAngles(null, null, new float?(0f));
		}
		else
		{
			this.p1IsColliding = false;
		}
		if (this.player2 != null)
		{
			this.p2IsColliding = (this.player2.transform.parent == this.airplane1.transform);
			this.player2.transform.SetEulerAngles(null, null, new float?(0f));
		}
		else
		{
			this.p2IsColliding = false;
		}
		this.autoTiltTime = Mathf.Clamp(this.autoTiltTime + CupheadTime.Delta * ((!this.autoX || !this.autoTilt) ? -1f : 3f), 0f, 1f);
		for (int i = 0; i < this.puffTimer.Length; i++)
		{
			this.puffTimer[i] -= CupheadTime.Delta;
			if (this.puffTimer[i] <= 0f)
			{
				this.puffTimer[i] += ((i != 0) ? 0.8f : 1f);
				Effect effect = this.planePuffFX.Create(this.planePuffPos[i].position);
				effect.transform.SetEulerAngles(null, null, new float?((float)((i != 0) ? 30 : -30)));
			}
		}
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x000B4C4C File Offset: 0x000B304C
	private void StartP1Super()
	{
		this.playerInSuper[0] = true;
		this.playerRelativePosAtSuperStart[0] = this.player1.transform.position.y - base.transform.position.y;
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x000B4C98 File Offset: 0x000B3098
	private void EndP1Super()
	{
		this.playerInSuper[0] = false;
		if (this.restorePlayerPos[0])
		{
			this.player1.transform.position = new Vector3(this.player1.transform.position.x, base.transform.position.y + this.playerRelativePosAtSuperStart[0]);
		}
		this.restorePlayerPos[0] = false;
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x000B4D10 File Offset: 0x000B3110
	private void StartP2Super()
	{
		this.playerInSuper[1] = true;
		this.playerRelativePosAtSuperStart[1] = this.player2.transform.position.y - base.transform.position.y;
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x000B4D5C File Offset: 0x000B315C
	private void EndP2Super()
	{
		this.playerInSuper[1] = false;
		if (this.restorePlayerPos[1])
		{
			this.player2.transform.position = new Vector3(this.player1.transform.position.x, base.transform.position.y + this.playerRelativePosAtSuperStart[1]);
		}
		this.restorePlayerPos[1] = false;
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x000B4DD4 File Offset: 0x000B31D4
	public void AutoMoveToPos(Vector3 pos, bool controlTilt = true, bool holdForYToReleaseX = true)
	{
		if (this.autoMoveCoroutine != null)
		{
			base.StopCoroutine(this.autoMoveCoroutine);
		}
		this.autoTilt = controlTilt;
		this.autoDest = pos;
		this.autoX = true;
		this.autoY = true;
		this.autoMoveCoroutine = base.StartCoroutine(this.auto_move_to_pos_cr(holdForYToReleaseX));
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x000B4E28 File Offset: 0x000B3228
	private IEnumerator auto_move_to_pos_cr(bool holdForYToReleaseX)
	{
		if (Mathf.Abs(this.autoDest.y - base.transform.position.y) > 50f)
		{
			this.moveSpeed.y = Mathf.Sign(base.transform.position.y - this.autoDest.y) * 100f;
		}
		float maxYSpeed = 100f;
		float yDir = Mathf.Sign(this.autoDest.y - base.transform.position.y);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (this.autoX || this.autoY)
		{
			if (!CupheadTime.IsPaused())
			{
				if (this.autoX)
				{
					float num = (Mathf.Abs(this.autoDest.x - base.transform.position.x) >= 100f) ? 1f : 0.5f;
					this.moveSpeed.x = Mathf.Clamp(this.moveSpeed.x + Mathf.Sign(this.autoDest.x - base.transform.position.x) * 5f * num, -400f, 400f);
					this.MoveAirplane();
					if (Mathf.Abs(base.transform.position.x - this.autoDest.x) < 50f && Mathf.Abs(base.transform.position.y - this.autoDest.y) < (float)((!holdForYToReleaseX) ? 1000 : 20))
					{
						this.autoX = false;
						this.moveSpeed.x = Mathf.Clamp(this.moveSpeed.x, base.properties.CurrentState.plane.speedAtMaxTilt.min, base.properties.CurrentState.plane.speedAtMaxTilt.max);
					}
				}
				if (this.autoY)
				{
					float num2 = (Mathf.Abs(this.autoDest.y - base.transform.position.y) >= 50f) ? 1f : 0.5f;
					this.moveSpeed.y = Mathf.Clamp(this.moveSpeed.y + Mathf.Sign(this.autoDest.y - base.transform.position.y) * 3f * num2, -maxYSpeed, maxYSpeed);
					if (yDir != Mathf.Sign(this.autoDest.y - base.transform.position.y))
					{
						maxYSpeed *= 0.99f;
					}
					if (Mathf.Abs(base.transform.position.y - this.autoDest.y) < 5f && this.moveSpeed.y < 2f)
					{
						this.autoY = false;
						this.moveSpeed.y = 0f;
						base.transform.position = new Vector3(base.transform.position.x, this.autoDest.y);
					}
				}
			}
			yield return wait;
		}
		this.autoX = false;
		yield break;
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x000B4E4C File Offset: 0x000B324C
	private void HandleDip()
	{
		this.p1contactTime = ((!this.p1IsColliding) ? 0f : Mathf.Clamp(this.p1contactTime + CupheadTime.Delta, 0f, 1.2f));
		this.p2contactTime = ((!this.p2IsColliding) ? 0f : Mathf.Clamp(this.p2contactTime + CupheadTime.Delta, 0f, 1.2f));
		float d = Mathf.Clamp(Mathf.Sin(this.p1contactTime / 1.2f * 3.1415927f) * 12f + Mathf.Sin(this.p2contactTime / 1.2f * 3.1415927f) * 12f, 0f, 12f);
		if (!this.p1IsColliding && !this.p2IsColliding)
		{
			this.tiltable.localPosition = Vector3.Lerp(this.tiltable.transform.localPosition, this.tiltableBasePos + Vector3.up * 9f, 0.1f);
		}
		else
		{
			this.tiltable.localPosition = Vector3.Lerp(this.tiltable.transform.localPosition, this.tiltableBasePos + Vector3.down * d, 0.5f);
		}
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x000B4FB0 File Offset: 0x000B33B0
	private float GetDestRotationVal()
	{
		LevelProperties.Airplane.Plane plane = base.properties.CurrentState.plane;
		float num = Vector3.Distance(this.edgeRight.position, Vector3.Lerp(this.edgeLeft.position, this.edgeRight.position, Mathf.InverseLerp(plane.speedAtMaxTilt.min, plane.speedAtMaxTilt.max, this.moveSpeed.x)));
		float a;
		if (this.p1IsColliding && this.p2IsColliding && this.player1 != null && this.player2 != null)
		{
			a = Vector3.Distance(Vector3.Lerp(this.player1.transform.position, this.player2.transform.position, 0.5f), this.edgeRight.position);
		}
		else
		{
			AbstractPlayerController abstractPlayerController = null;
			if (this.p1IsColliding && this.player1 != null)
			{
				abstractPlayerController = this.player1;
			}
			else if (this.p2IsColliding && this.player2 != null)
			{
				abstractPlayerController = this.player2;
			}
			if (abstractPlayerController != null)
			{
				a = Vector3.Distance(this.edgeRight.position, abstractPlayerController.transform.position);
			}
			else
			{
				a = num;
			}
		}
		return Mathf.Lerp(a, num, this.autoTiltTime);
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x000B5128 File Offset: 0x000B3528
	private IEnumerator handle_tilt_cr()
	{
		LevelProperties.Airplane.Plane p = base.properties.CurrentState.plane;
		float destRotationVal = this.rotationVal;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (!CupheadTime.IsPaused())
			{
				if (!this.p1IsColliding && !this.p2IsColliding && this.moveSpeed.x == 0f)
				{
					Mathf.Lerp(destRotationVal, Vector3.Distance(this.edgeRight.position, base.transform.position), 0.05f);
				}
				else
				{
					destRotationVal = this.GetDestRotationVal();
				}
				if (Mathf.Abs(destRotationVal - this.rotationVal) > 0.1f)
				{
					this.rotationVal = Mathf.Lerp(this.rotationVal, destRotationVal, 0.15f);
				}
				else
				{
					this.rotationVal = destRotationVal;
				}
				this.tiltable.transform.SetEulerAngles(null, null, new float?(p.tiltAngle.GetFloatAt(this.rotationVal / this.rotationDist)));
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x000B5144 File Offset: 0x000B3544
	private IEnumerator handle_player_move_cr()
	{
		LevelProperties.Airplane.Plane p = base.properties.CurrentState.plane;
		float destMoveSpeed = 0f;
		bool goingLeft = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (!CupheadTime.IsPaused())
			{
				while (this.autoX)
				{
					yield return null;
				}
				goingLeft = (this.moveSpeed.x < 0f);
				if (!this.p1IsColliding && !this.p2IsColliding)
				{
					if (this.moveSpeed.x < 0f && goingLeft)
					{
						this.moveSpeed.x = this.moveSpeed.x + p.decelerationAmount;
					}
					else if (this.moveSpeed.x > 0f && !goingLeft)
					{
						this.moveSpeed.x = this.moveSpeed.x - p.decelerationAmount;
					}
					destMoveSpeed = this.moveSpeed.x;
				}
				else
				{
					destMoveSpeed = -p.speedAtMaxTilt.GetFloatAt(this.rotationVal / this.rotationDist);
					if (Mathf.Abs(destMoveSpeed - this.moveSpeed.x) > 4.1f)
					{
						this.moveSpeed.x = this.moveSpeed.x + Mathf.Sign(destMoveSpeed - this.moveSpeed.x) * 4.1f;
					}
					else
					{
						this.moveSpeed.x = destMoveSpeed;
					}
				}
				this.MoveAirplane();
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600144A RID: 5194 RVA: 0x000B515F File Offset: 0x000B355F
	public void SetXRange(float min, float max)
	{
		this.minX = min;
		this.maxX = max;
	}

	// Token: 0x0600144B RID: 5195 RVA: 0x000B5170 File Offset: 0x000B3570
	private void SetPartAngles()
	{
		float num = base.transform.position.x / this.maxParallaxX;
		for (int i = 0; i < this.planeParts.Length; i++)
		{
			this.planeParts[i].SetLocalEulerAngles(null, null, new float?(Mathf.LerpUnclamped(0f, this.planePartAngleRanges[i], num)));
			this.planeParts[i].SetLocalPosition(new float?(Mathf.LerpUnclamped(0f, this.planePartPosOffsets[i].x, num)), new float?(Mathf.Lerp(0f, this.planePartPosOffsets[i].y, Mathf.Abs(num))), null);
		}
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x000B5248 File Offset: 0x000B3648
	private void MoveAirplane()
	{
		if (CupheadTime.IsPaused())
		{
			return;
		}
		this.HandleDip();
		this.SetPartAngles();
		base.transform.position += Vector3.up * this.moveSpeed.y * CupheadTime.FixedDelta;
		base.transform.position += Vector3.right * this.moveSpeed.x * CupheadTime.FixedDelta;
		if (!this.autoX)
		{
			if (base.transform.position.x < this.minX && this.moveSpeed.x < 0f)
			{
				this.moveSpeed.x = this.moveSpeed.x * (12.5f * CupheadTime.FixedDelta);
				this.AutoMoveToPos(new Vector3(this.minX + 50f, base.transform.position.y), false, false);
			}
			if (base.transform.position.x > this.maxX && this.moveSpeed.x > 0f)
			{
				this.moveSpeed.x = this.moveSpeed.x * (12.5f * CupheadTime.FixedDelta);
				this.AutoMoveToPos(new Vector3(this.maxX - 50f, base.transform.position.y), false, false);
			}
		}
		this.updateCount++;
		if (this.updateCount % 5 == 0)
		{
			this.UpdateSound();
		}
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x000B53FC File Offset: 0x000B37FC
	private void UpdateSound()
	{
		float num = Mathf.Abs(this.moveSpeed.x) / base.properties.CurrentState.plane.speedAtMaxTilt.max;
		if (Mathf.Abs(num - this.lastNormalizedSpeed) < 0.01f)
		{
			return;
		}
		this.lastNormalizedSpeed = num;
		AudioManager.ChangeSFXPitch("sfx_dlc_dogfight_playerplane_loop", 1f + num * this.pitchIncreaseFactor, 0f);
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_playerplane_loop", this.volume.GetFloatAt(num), 0f);
		float floatAt = this.volumeHighSpeed.GetFloatAt(Mathf.InverseLerp(this.volumeHighSpeedSpeedFloor, 1f, num));
		if (floatAt > this.cachedHighSpeedVolume)
		{
			this.cachedHighSpeedVolume += this.highSpeedVolumeIncreaseRate * CupheadTime.FixedDelta;
			if (this.cachedHighSpeedVolume > floatAt)
			{
				this.cachedHighSpeedVolume = floatAt;
			}
		}
		if (floatAt < this.cachedHighSpeedVolume)
		{
			this.cachedHighSpeedVolume -= this.highSpeedVolumeDecreaseRate * CupheadTime.FixedDelta;
			if (this.cachedHighSpeedVolume < floatAt)
			{
				this.cachedHighSpeedVolume = floatAt;
			}
		}
		AudioManager.ChangeSFXPitch("sfx_dlc_dogfight_playerplane_highspeed_loop", 1f + num * this.pitchIncreaseFactorHighSpeed, 0f);
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_playerplane_highspeed_loop", this.cachedHighSpeedVolume, 0f);
	}

	// Token: 0x0600144E RID: 5198 RVA: 0x000B554B File Offset: 0x000B394B
	private void SFX_DOGFIGHT_PlayerPlane_Loop()
	{
		AudioManager.PlayLoop("sfx_dlc_dogfight_playerplane_loop");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_playerplane_loop");
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_dogfight_playerplane_loop", 0.25f, 3f);
	}

	// Token: 0x0600144F RID: 5199 RVA: 0x000B557B File Offset: 0x000B397B
	private void SFX_DOGFIGHT_PlayerPlane_HighSpeed_Loop()
	{
		AudioManager.PlayLoop("sfx_dlc_dogfight_playerplane_highspeed_loop");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_playerplane_highspeed_loop");
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_dogfight_playerplane_highspeed_loop", 0.6f, 3f);
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x000B55AB File Offset: 0x000B39AB
	private void SFX_DOGFIGHT_PlayerPlane_StopLoop()
	{
		AudioManager.Stop("sfx_dlc_dogfight_playerplane_loop");
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x000B55B7 File Offset: 0x000B39B7
	private void AnimationEvent_SFX_DOGFIGHT_PlayerPlane_CanteenCheer()
	{
		AudioManager.Play("sfx_dlc_dogfight_p2_pilotclap");
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x000B55C4 File Offset: 0x000B39C4
	private void WORKAROUND_NullifyFields()
	{
		this.volume = null;
		this.volumeHighSpeed = null;
		this.edgeLeft = null;
		this.edgeRight = null;
		this.airplane1 = null;
		this.tiltable = null;
		this.planeParts = null;
		this.planePartAngleRanges = null;
		this.planePartPosOffsets = null;
		this.planePuffFX = null;
		this.planePuffPos = null;
		this.player1 = null;
		this.player2 = null;
		this.playerInSuper = null;
		this.restorePlayerPos = null;
		this.playerRelativePosAtSuperStart = null;
		this.puffTimer = null;
		this.autoMoveCoroutine = null;
	}

	// Token: 0x04001D67 RID: 7527
	private const float PUFF_DELAY_L = 1f;

	// Token: 0x04001D68 RID: 7528
	private const float PUFF_DELAY_R = 0.8f;

	// Token: 0x04001D69 RID: 7529
	private const float AUTO_MOVE_MAX_X_DIST = 50f;

	// Token: 0x04001D6A RID: 7530
	private const float AUTO_MOVE_MAX_Y_DIST = 5f;

	// Token: 0x04001D6B RID: 7531
	private const float AUTO_MOVE_MAX_Y_END_SPEED = 2f;

	// Token: 0x04001D6C RID: 7532
	private const float AUTO_MOVE_MAX_X_SPEED = 400f;

	// Token: 0x04001D6D RID: 7533
	private const float AUTO_MOVE_MAX_Y_SPEED = 100f;

	// Token: 0x04001D6E RID: 7534
	private const float AUTO_ACCEL_X = 5f;

	// Token: 0x04001D6F RID: 7535
	private const float AUTO_ACCEL_Y = 3f;

	// Token: 0x04001D70 RID: 7536
	private const float MIN_TILT_DIFFERENCE = 0.1f;

	// Token: 0x04001D71 RID: 7537
	private const float TILT_ATTENUATION = 0.15f;

	// Token: 0x04001D72 RID: 7538
	private const float ACCEL_SPEED = 4.1f;

	// Token: 0x04001D73 RID: 7539
	private const float BOUNCE_TIME = 1.2f;

	// Token: 0x04001D74 RID: 7540
	private const float BOUNCE_DIST = 12f;

	// Token: 0x04001D75 RID: 7541
	private const float RISE_DIST = 9f;

	// Token: 0x04001D76 RID: 7542
	private const float RISE_RATE = 0.1f;

	// Token: 0x04001D77 RID: 7543
	private const float BOUNCE_RATE = 0.5f;

	// Token: 0x04001D78 RID: 7544
	private const float DAMP_ON_BOUNDARY_COLLIDE = 12.5f;

	// Token: 0x04001D79 RID: 7545
	[SerializeField]
	private float pitchIncreaseFactor = 0.5f;

	// Token: 0x04001D7A RID: 7546
	[SerializeField]
	private float pitchIncreaseFactorHighSpeed = 0.5f;

	// Token: 0x04001D7B RID: 7547
	[SerializeField]
	private MinMax volume = new MinMax(0.25f, 0.5f);

	// Token: 0x04001D7C RID: 7548
	[SerializeField]
	private MinMax volumeHighSpeed = new MinMax(0.25f, 0.5f);

	// Token: 0x04001D7D RID: 7549
	private float cachedHighSpeedVolume = 1E-06f;

	// Token: 0x04001D7E RID: 7550
	[SerializeField]
	private float highSpeedVolumeIncreaseRate = 1f;

	// Token: 0x04001D7F RID: 7551
	[SerializeField]
	private float highSpeedVolumeDecreaseRate = 0.25f;

	// Token: 0x04001D80 RID: 7552
	[SerializeField]
	private float volumeHighSpeedSpeedFloor = 0.5f;

	// Token: 0x04001D81 RID: 7553
	[SerializeField]
	private Transform edgeLeft;

	// Token: 0x04001D82 RID: 7554
	[SerializeField]
	private Transform edgeRight;

	// Token: 0x04001D83 RID: 7555
	[SerializeField]
	private Transform airplane1;

	// Token: 0x04001D84 RID: 7556
	[SerializeField]
	private Transform tiltable;

	// Token: 0x04001D85 RID: 7557
	[SerializeField]
	private Transform[] planeParts;

	// Token: 0x04001D86 RID: 7558
	[SerializeField]
	private float[] planePartAngleRanges;

	// Token: 0x04001D87 RID: 7559
	[SerializeField]
	private Vector2[] planePartPosOffsets;

	// Token: 0x04001D88 RID: 7560
	[SerializeField]
	private Effect planePuffFX;

	// Token: 0x04001D89 RID: 7561
	[SerializeField]
	private Transform[] planePuffPos;

	// Token: 0x04001D8A RID: 7562
	private AbstractPlayerController player1;

	// Token: 0x04001D8B RID: 7563
	private AbstractPlayerController player2;

	// Token: 0x04001D8C RID: 7564
	private bool p1IsColliding;

	// Token: 0x04001D8D RID: 7565
	private bool p2IsColliding;

	// Token: 0x04001D8E RID: 7566
	private Vector3 tiltableBasePos;

	// Token: 0x04001D8F RID: 7567
	private float maxParallaxX;

	// Token: 0x04001D90 RID: 7568
	public bool autoX;

	// Token: 0x04001D91 RID: 7569
	public bool autoY;

	// Token: 0x04001D92 RID: 7570
	public Vector3 autoDest;

	// Token: 0x04001D93 RID: 7571
	private float autoTiltTime;

	// Token: 0x04001D94 RID: 7572
	private bool autoTilt;

	// Token: 0x04001D95 RID: 7573
	private float minX;

	// Token: 0x04001D96 RID: 7574
	private float maxX;

	// Token: 0x04001D97 RID: 7575
	private float rotationDist;

	// Token: 0x04001D98 RID: 7576
	private float rotationVal;

	// Token: 0x04001D99 RID: 7577
	private float p1contactTime;

	// Token: 0x04001D9A RID: 7578
	private float p2contactTime;

	// Token: 0x04001D9B RID: 7579
	private bool[] playerInSuper = new bool[2];

	// Token: 0x04001D9C RID: 7580
	private bool[] restorePlayerPos = new bool[2];

	// Token: 0x04001D9D RID: 7581
	private float[] playerRelativePosAtSuperStart = new float[2];

	// Token: 0x04001D9E RID: 7582
	private float[] puffTimer = new float[2];

	// Token: 0x04001D9F RID: 7583
	private Vector3 moveSpeed;

	// Token: 0x04001DA0 RID: 7584
	private Coroutine autoMoveCoroutine;

	// Token: 0x04001DA1 RID: 7585
	private float lastNormalizedSpeed;

	// Token: 0x04001DA2 RID: 7586
	private int updateCount;
}
