using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000960 RID: 2400
public class MapShmupTutorialBridgeActivator : MonoBehaviour
{
	// Token: 0x06003804 RID: 14340 RVA: 0x002010F4 File Offset: 0x001FF4F4
	private void Start()
	{
		if (!PlayerData.Data.IsFlyingTutorialCompleted && Level.PreviousLevel == Levels.ShmupTutorial)
		{
			PlayerData.Data.IsFlyingTutorialCompleted = true;
			this.blueprintObstacle.OnConditionNotMet();
			base.StartCoroutine(this.DoTransition());
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.SaveCurrentFile();
		}
		else if (!PlayerData.Data.IsFlyingTutorialCompleted)
		{
			this.blueprintObstacle.OnConditionNotMet();
		}
		else
		{
			this.blueprintObstacle.OnConditionAlreadyMet();
		}
	}

	// Token: 0x06003805 RID: 14341 RVA: 0x00201188 File Offset: 0x001FF588
	private IEnumerator DoTransition()
	{
		yield return CupheadTime.WaitForSeconds(this, this.DoTransitionDelay);
		this.blueprintObstacle.DoTransition();
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		this.blueprintObstacle.OnConditionAlreadyMet();
		yield break;
	}

	// Token: 0x04003FE9 RID: 16361
	[SerializeField]
	private MapLevelDependentObstacle blueprintObstacle;

	// Token: 0x04003FEA RID: 16362
	[SerializeField]
	private float DoTransitionDelay;

	// Token: 0x04003FEB RID: 16363
	[SerializeField]
	private int dialoguerVariableID = 5;
}
