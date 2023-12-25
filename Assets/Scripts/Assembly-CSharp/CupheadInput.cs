using System;
using Rewired;

// Token: 0x0200046F RID: 1135
public class CupheadInput
{
	// Token: 0x06001163 RID: 4451 RVA: 0x000A53CC File Offset: 0x000A37CC
	public static Localization.Translation InputDisplayForButton(CupheadButton button, int rewiredPlayerId = 0)
	{
		Player.ControllerHelper controllers = ReInput.players.GetPlayer(rewiredPlayerId).controllers;
		ActionElementMap firstElementMapWithAction;
		if (controllers != null && controllers.joystickCount > 0)
		{
			ControllerType controllerType = ControllerType.Joystick;
			Controller lastActiveController = ReInput.players.GetPlayer(rewiredPlayerId).controllers.GetLastActiveController();
			if (lastActiveController != null)
			{
				controllerType = lastActiveController.type;
			}
			firstElementMapWithAction = controllers.maps.GetFirstElementMapWithAction(controllerType, (int)button, true);
		}
		else
		{
			if (PlatformHelper.IsConsole)
			{
				return default(Localization.Translation);
			}
			firstElementMapWithAction = ReInput.players.GetPlayer(rewiredPlayerId).controllers.maps.GetFirstElementMapWithAction((int)button, true);
		}
		if (firstElementMapWithAction == null)
		{
			return new Localization.Translation
			{
				text = string.Empty
			};
		}
		string text = firstElementMapWithAction.elementIdentifierName;
		if (button == CupheadButton.EquipMenu && text.Contains("Shift"))
		{
			text = "Shift";
		}
		string text2 = CupheadInput.handleCustomGlyphs(text, rewiredPlayerId);
		Localization.Translation result = Localization.Translate(text);
		if (text2 == null)
		{
			if (!string.IsNullOrEmpty(result.text))
			{
				text = result.text;
			}
		}
		else
		{
			text = text2;
		}
		text = text.ToUpper();
		text = text.Replace(" SHOULDER", "B");
		text = text.Replace(" BUMPER", "B");
		text = text.Replace(" TRIGGER", "T");
		text = text.Replace("LEFT", "L");
		text = text.Replace("RIGHT", "R");
		text = text.Replace("R SHIFT", "SHIFT");
		text = text.Replace("L SHIFT", "SHIFT");
		text = text.Replace(" +", string.Empty);
		text = text.Replace(" -", string.Empty);
		result.text = text;
		return result;
	}

	// Token: 0x06001164 RID: 4452 RVA: 0x000A55AF File Offset: 0x000A39AF
	private static string handleCustomGlyphs(string input, int rewiredPlayerId)
	{
		return null;
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x000A55B4 File Offset: 0x000A39B4
	public static CupheadInput.InputSymbols InputSymbolForButton(CupheadButton button)
	{
		CupheadInput.InputSymbols result;
		switch (button)
		{
		case CupheadButton.Jump:
			result = CupheadInput.InputSymbols.XBOX_A;
			break;
		case CupheadButton.Shoot:
			result = CupheadInput.InputSymbols.XBOX_X;
			break;
		case CupheadButton.Super:
			result = CupheadInput.InputSymbols.XBOX_B;
			break;
		case CupheadButton.SwitchWeapon:
			result = CupheadInput.InputSymbols.XBOX_LB;
			break;
		case CupheadButton.Lock:
			result = CupheadInput.InputSymbols.XBOX_RB;
			break;
		case CupheadButton.Dash:
			result = CupheadInput.InputSymbols.XBOX_Y;
			break;
		default:
			if (button != CupheadButton.None)
			{
			}
			result = CupheadInput.InputSymbols.XBOX_NONE;
			break;
		case CupheadButton.Accept:
			result = CupheadInput.InputSymbols.XBOX_A;
			break;
		case CupheadButton.Cancel:
			result = CupheadInput.InputSymbols.XBOX_B;
			break;
		}
		return result;
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x000A564F File Offset: 0x000A3A4F
	public static string DialogueStringFromButton(CupheadButton button)
	{
		return " {" + button + "} ";
	}

	// Token: 0x06001167 RID: 4455 RVA: 0x000A5668 File Offset: 0x000A3A68
	public static Joystick CheckForUnconnectedControllerPress()
	{
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (!ReInput.controllers.IsJoystickAssigned(joystick))
			{
				if (joystick.GetAnyButtonDown())
				{
					return joystick;
				}
			}
		}
		return null;
	}

	// Token: 0x06001168 RID: 4456 RVA: 0x000A56E8 File Offset: 0x000A3AE8
	public static Joystick CheckForControllerPress(long systemID)
	{
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (joystick.systemId == systemID && joystick.GetAnyButtonDown())
			{
				return joystick;
			}
		}
		return null;
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x000A5774 File Offset: 0x000A3B74
	public static bool AutoAssignController(int rewiredPlayerId)
	{
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (!ReInput.controllers.IsJoystickAssigned(joystick))
			{
				Player player = ReInput.players.GetPlayer(rewiredPlayerId);
				if (player != null)
				{
					if (player.controllers.joystickCount <= 0)
					{
						player.controllers.AddController(joystick, true);
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x04001AF8 RID: 6904
	public static readonly CupheadInput.Pair[] pairs = new CupheadInput.Pair[]
	{
		new CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_A, "<sprite=0>", "<sprite=1>"),
		new CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_B, "<sprite=2>", "<sprite=3>"),
		new CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_X, "<sprite=4>", "<sprite=5>"),
		new CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_Y, "<sprite=6>", "<sprite=7>")
	};

	// Token: 0x02000470 RID: 1136
	public enum InputDevice
	{
		// Token: 0x04001AFA RID: 6906
		Keyboard,
		// Token: 0x04001AFB RID: 6907
		Controller_1,
		// Token: 0x04001AFC RID: 6908
		Controller_2
	}

	// Token: 0x02000471 RID: 1137
	public enum InputSymbols
	{
		// Token: 0x04001AFE RID: 6910
		XBOX_NONE,
		// Token: 0x04001AFF RID: 6911
		XBOX_A,
		// Token: 0x04001B00 RID: 6912
		XBOX_B,
		// Token: 0x04001B01 RID: 6913
		XBOX_X,
		// Token: 0x04001B02 RID: 6914
		XBOX_Y,
		// Token: 0x04001B03 RID: 6915
		XBOX_RB,
		// Token: 0x04001B04 RID: 6916
		XBOX_LB
	}

	// Token: 0x02000472 RID: 1138
	public class AnyPlayerInput
	{
		// Token: 0x0600116B RID: 4459 RVA: 0x000A5884 File Offset: 0x000A3C84
		public AnyPlayerInput(bool checkIfDead = false)
		{
			this.checkIfDead = checkIfDead;
			this.players = new Player[]
			{
				PlayerManager.GetPlayerInput(PlayerId.PlayerOne),
				PlayerManager.GetPlayerInput(PlayerId.PlayerTwo)
			};
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x000A58B4 File Offset: 0x000A3CB4
		public bool GetButton(CupheadButton button)
		{
			foreach (Player player in this.players)
			{
				if (player.GetButton((int)button) && (!this.checkIfDead || !this.IsDead(player)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x000A5908 File Offset: 0x000A3D08
		public bool GetButtonDown(CupheadButton button)
		{
			if (InterruptingPrompt.IsInterrupting())
			{
				return false;
			}
			foreach (Player player in this.players)
			{
				if (player.GetButtonDown((int)button) && (!this.checkIfDead || !this.IsDead(player)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x000A5968 File Offset: 0x000A3D68
		public bool GetActionButtonDown()
		{
			if (InterruptingPrompt.IsInterrupting())
			{
				return false;
			}
			foreach (Player player in this.players)
			{
				if ((player.GetButtonDown(13) || player.GetButtonDown(14) || player.GetButtonDown(7) || player.GetButtonDown(15) || player.GetButtonDown(2) || player.GetButtonDown(6) || player.GetButtonDown(8) || player.GetButtonDown(3) || player.GetButtonDown(4) || player.GetButtonDown(5)) && (!this.checkIfDead || !this.IsDead(player)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x000A5A38 File Offset: 0x000A3E38
		public bool GetAnyButtonDown()
		{
			if (InterruptingPrompt.IsInterrupting())
			{
				return false;
			}
			foreach (Player player in this.players)
			{
				foreach (Controller controller in player.controllers.Controllers)
				{
					if (controller.GetAnyButtonDown() && (!this.checkIfDead || !this.IsDead(player)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x000A5AF0 File Offset: 0x000A3EF0
		public bool GetAnyButtonHeld()
		{
			if (InterruptingPrompt.IsInterrupting())
			{
				return false;
			}
			foreach (Player player in this.players)
			{
				foreach (Controller controller in player.controllers.Controllers)
				{
					if (controller.GetAnyButton() && (!this.checkIfDead || !this.IsDead(player)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x000A5BA8 File Offset: 0x000A3FA8
		public bool GetButtonUp(CupheadButton button)
		{
			foreach (Player player in this.players)
			{
				if (player.GetButtonUp((int)button) && (!this.checkIfDead || !this.IsDead(player)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x000A5BFC File Offset: 0x000A3FFC
		private bool IsDead(Player player)
		{
			PlayerId id = (player != this.players[0]) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
			AbstractPlayerController player2 = PlayerManager.GetPlayer(id);
			return player2 == null || player2.IsDead;
		}

		// Token: 0x04001B05 RID: 6917
		private Player[] players;

		// Token: 0x04001B06 RID: 6918
		public bool checkIfDead;
	}

	// Token: 0x02000473 RID: 1139
	public class Pair
	{
		// Token: 0x06001173 RID: 4467 RVA: 0x000A5C3B File Offset: 0x000A403B
		public Pair(CupheadInput.InputSymbols symbol, string first, string second)
		{
			this.symbol = symbol;
			this.first = first;
			this.second = second;
		}

		// Token: 0x04001B07 RID: 6919
		public readonly CupheadInput.InputSymbols symbol;

		// Token: 0x04001B08 RID: 6920
		public readonly string first;

		// Token: 0x04001B09 RID: 6921
		public readonly string second;
	}
}
