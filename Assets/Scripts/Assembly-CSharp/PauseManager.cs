using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009CA RID: 2506
public static class PauseManager
{
	// Token: 0x06003AE6 RID: 15078 RVA: 0x0021258A File Offset: 0x0021098A
	public static void Reset()
	{
		PauseManager.state = PauseManager.State.Unpaused;
	}

	// Token: 0x06003AE7 RID: 15079 RVA: 0x00212592 File Offset: 0x00210992
	public static void AddChild(AbstractPausableComponent child)
	{
		PauseManager.children.Add(child);
	}

	// Token: 0x06003AE8 RID: 15080 RVA: 0x0021259F File Offset: 0x0021099F
	public static void RemoveChild(AbstractPausableComponent child)
	{
		PauseManager.children.Remove(child);
	}

	// Token: 0x06003AE9 RID: 15081 RVA: 0x002125B0 File Offset: 0x002109B0
	public static void Pause()
	{
		if (PauseManager.state == PauseManager.State.Paused)
		{
			return;
		}
		PauseManager.state = PauseManager.State.Paused;
		AudioListener.pause = true;
		PauseManager.oldSpeed = CupheadTime.GlobalSpeed;
		CupheadTime.GlobalSpeed = 0f;
		foreach (AbstractPausableComponent abstractPausableComponent in PauseManager.children)
		{
			abstractPausableComponent.OnPause();
		}
		PauseManager.SetChildren(false);
	}

	// Token: 0x06003AEA RID: 15082 RVA: 0x0021263C File Offset: 0x00210A3C
	public static void Unpause()
	{
		if (PauseManager.state == PauseManager.State.Unpaused)
		{
			return;
		}
		PauseManager.state = PauseManager.State.Unpaused;
		AudioListener.pause = false;
		CupheadTime.GlobalSpeed = PauseManager.oldSpeed;
		foreach (AbstractPausableComponent abstractPausableComponent in PauseManager.children)
		{
			abstractPausableComponent.OnUnpause();
		}
		PauseManager.SetChildren(true);
	}

	// Token: 0x06003AEB RID: 15083 RVA: 0x002126C0 File Offset: 0x00210AC0
	public static void Toggle()
	{
		if (PauseManager.state == PauseManager.State.Paused)
		{
			PauseManager.Unpause();
		}
		else
		{
			PauseManager.Pause();
		}
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x002126DC File Offset: 0x00210ADC
	private static void SetChildren(bool enabled)
	{
		for (int i = 0; i < PauseManager.children.Count; i++)
		{
			AbstractPausableComponent abstractPausableComponent = PauseManager.children[i];
			if (abstractPausableComponent == null)
			{
				PauseManager.children.Remove(abstractPausableComponent);
				i--;
			}
			else if (enabled)
			{
				abstractPausableComponent.enabled = abstractPausableComponent.preEnabled;
			}
			else
			{
				abstractPausableComponent.preEnabled = abstractPausableComponent.enabled;
				abstractPausableComponent.enabled = false;
			}
		}
	}

	// Token: 0x040042A4 RID: 17060
	public static PauseManager.State state;

	// Token: 0x040042A5 RID: 17061
	private static float oldSpeed;

	// Token: 0x040042A6 RID: 17062
	private static List<AbstractPausableComponent> children = new List<AbstractPausableComponent>();

	// Token: 0x020009CB RID: 2507
	public enum State
	{
		// Token: 0x040042A8 RID: 17064
		Unpaused,
		// Token: 0x040042A9 RID: 17065
		Paused
	}
}
