using System;
using UnityEngine;

// Token: 0x0200059A RID: 1434
public class DiceGateLevelKingDice : MonoBehaviour
{
	// Token: 0x06001B7C RID: 7036 RVA: 0x000FB282 File Offset: 0x000F9682
	public void SetDisappearBool()
	{
		PlayerData.Data.CurrentMapData.hasKingDiceDisappeared = true;
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x000FB299 File Offset: 0x000F9699
	private void SoundKingDiceExitAnim()
	{
		AudioManager.Play("dicegate_kingdice_exit");
	}
}
