using System;
using UnityEngine;

// Token: 0x0200095E RID: 2398
public class MapSecretAchievementUnlocker : AbstractMonoBehaviour
{
	// Token: 0x060037FE RID: 14334 RVA: 0x00200F64 File Offset: 0x001FF364
	private void OnTriggerEnter2D(Collider2D collider)
	{
		MapPlayerController component = collider.GetComponent<MapPlayerController>();
		OnlineManager.Instance.Interface.UnlockAchievement(component.id, "FoundSecretPassage");
		if (this.updateDialogue)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x04003FE5 RID: 16357
	[SerializeField]
	private bool updateDialogue = true;

	// Token: 0x04003FE6 RID: 16358
	[SerializeField]
	private int dialoguerVariableID = 7;
}
