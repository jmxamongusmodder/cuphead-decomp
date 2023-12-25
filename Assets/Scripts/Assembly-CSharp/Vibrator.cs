using System;
using Rewired;
using UnityEngine;

// Token: 0x02000474 RID: 1140
public class Vibrator : AbstractMonoBehaviour
{
	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06001175 RID: 4469 RVA: 0x000A5C78 File Offset: 0x000A4078
	// (set) Token: 0x06001176 RID: 4470 RVA: 0x000A5C7F File Offset: 0x000A407F
	public static Vibrator Current { get; private set; }

	// Token: 0x06001177 RID: 4471 RVA: 0x000A5C87 File Offset: 0x000A4087
	public static void Vibrate(float amount, float time, PlayerId player)
	{
		Vibrator.Current._Vibrate(amount, time, player);
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x000A5C96 File Offset: 0x000A4096
	public static void StopVibrating(PlayerId player)
	{
		Vibrator.Current._StopVibrating(player);
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x000A5CA3 File Offset: 0x000A40A3
	protected override void Awake()
	{
		base.Awake();
		Vibrator.Current = this;
	}

	// Token: 0x0600117A RID: 4474 RVA: 0x000A5CB4 File Offset: 0x000A40B4
	private void Update()
	{
		if (!SettingsData.Data.canVibrate)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			float num = this.durationsLeft[i];
			float num2 = this.currentVibrations[i];
			num -= CupheadTime.Delta;
			if (num <= 0f)
			{
				if (num2 > 0f)
				{
					this.currentVibrations[i] = 0f;
					this._StopVibrating((PlayerId)i);
				}
			}
			else if (num2 <= 0f)
			{
				this._StopVibrating((PlayerId)i);
			}
			else
			{
				this.durationsLeft[i] = num;
			}
		}
	}

	// Token: 0x0600117B RID: 4475 RVA: 0x000A5D58 File Offset: 0x000A4158
	private void _Vibrate(float amount, float time, PlayerId playerId)
	{
		if (!SettingsData.Data.canVibrate)
		{
			return;
		}
		if (amount <= 0f || time <= 0f)
		{
			this._StopVibrating(playerId);
			return;
		}
		this.currentVibrations[(int)playerId] = amount;
		this.durationsLeft[(int)playerId] = time;
		Player player = ReInput.players.GetPlayer((int)playerId);
		foreach (Joystick joystick in player.controllers.Joysticks)
		{
			if (joystick.supportsVibration)
			{
				joystick.SetVibration(amount * Vibrator.PlatformMultiplier, amount * Vibrator.PlatformMultiplier);
			}
		}
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x000A5E20 File Offset: 0x000A4220
	private void _StopVibrating(PlayerId playerId)
	{
		if (!SettingsData.Data.canVibrate)
		{
			return;
		}
		Player player = ReInput.players.GetPlayer((int)playerId);
		foreach (Joystick joystick in player.controllers.Joysticks)
		{
			joystick.StopVibration();
		}
	}

	// Token: 0x04001B0A RID: 6922
	private static float PlatformMultiplier = 1f;

	// Token: 0x04001B0C RID: 6924
	private Coroutine vibrateCoroutine;

	// Token: 0x04001B0D RID: 6925
	private float[] durationsLeft = new float[2];

	// Token: 0x04001B0E RID: 6926
	private float[] currentVibrations = new float[2];
}
