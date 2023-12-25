using System;
using System.Text;
using Rewired;
using UnityEngine;

// Token: 0x02000B3E RID: 2878
public class DEBUG_RewiredPrinter : MonoBehaviour
{
	// Token: 0x060045B1 RID: 17841 RVA: 0x0024C27A File Offset: 0x0024A67A
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060045B2 RID: 17842 RVA: 0x0024C288 File Offset: 0x0024A688
	private void OnGUI()
	{
		if (!ReInput.isReady)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("===PLAYERS===");
		foreach (Player player in ReInput.players.AllPlayers)
		{
			stringBuilder.AppendLine(player.name);
			foreach (Joystick j in player.controllers.Joysticks)
			{
				DEBUG_RewiredPrinter.appendControllerInfo(j, stringBuilder);
			}
		}
		stringBuilder.AppendLine("===UNASSIGNED===");
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (!ReInput.controllers.IsJoystickAssigned(joystick))
			{
				DEBUG_RewiredPrinter.appendControllerInfo(joystick, stringBuilder);
			}
		}
		stringBuilder.AppendLine("===BUTTONS===");
		GUI.Box(new Rect(0f, 0f, 700f, 400f), stringBuilder.ToString());
	}

	// Token: 0x060045B3 RID: 17843 RVA: 0x0024C400 File Offset: 0x0024A800
	private static void appendControllerInfo(Joystick j, StringBuilder builder)
	{
		builder.AppendFormat("{0} :: {1}", j.name, j.id.ToString());
		builder.Append("\n");
	}
}
