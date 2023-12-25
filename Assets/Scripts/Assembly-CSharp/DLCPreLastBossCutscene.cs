using System;
using UnityEngine;

// Token: 0x02000403 RID: 1027
public class DLCPreLastBossCutscene : DLCGenericCutscene
{
	// Token: 0x06000E45 RID: 3653 RVA: 0x00092414 File Offset: 0x00090814
	protected override void Start()
	{
		base.Start();
		if (this.trappedChar == DLCGenericCutscene.TrappedChar.None)
		{
			this.trappedChar = base.DetectCharacter();
		}
		DLCGenericCutscene.TrappedChar trappedChar = this.trappedChar;
		if (trappedChar != DLCGenericCutscene.TrappedChar.Chalice)
		{
			if (trappedChar != DLCGenericCutscene.TrappedChar.Mugman)
			{
				if (trappedChar == DLCGenericCutscene.TrappedChar.Cuphead)
				{
					this.trappedChalice[0].SetActive(false);
					this.trappedChalice[1].SetActive(false);
					this.trappedMugman[0].SetActive(false);
					this.trappedMugman[1].SetActive(false);
					this.text[5] = this.altText;
					this.text[6] = this.altTextTrappedCharCuphead;
				}
			}
			else
			{
				this.trappedChalice[0].SetActive(false);
				this.trappedChalice[1].SetActive(false);
				this.trappedCuphead[0].SetActive(false);
				this.trappedCuphead[1].SetActive(false);
				this.text[5] = this.altText;
				this.text[6] = this.altTextTrappedCharMugman;
			}
		}
		else
		{
			this.trappedMugman[0].SetActive(false);
			this.trappedMugman[1].SetActive(false);
			this.trappedCuphead[0].SetActive(false);
			this.trappedCuphead[1].SetActive(false);
		}
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x0009254E File Offset: 0x0009094E
	protected override void OnCutsceneOver()
	{
		SceneLoader.LoadLevel(Levels.Saltbaker, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x0400178D RID: 6029
	[SerializeField]
	private DLCGenericCutscene.TrappedChar trappedChar;

	// Token: 0x0400178E RID: 6030
	[SerializeField]
	private GameObject[] trappedChalice;

	// Token: 0x0400178F RID: 6031
	[SerializeField]
	private GameObject[] trappedMugman;

	// Token: 0x04001790 RID: 6032
	[SerializeField]
	private GameObject[] trappedCuphead;

	// Token: 0x04001791 RID: 6033
	[SerializeField]
	private GameObject altText;

	// Token: 0x04001792 RID: 6034
	[SerializeField]
	private GameObject altTextTrappedCharCuphead;

	// Token: 0x04001793 RID: 6035
	[SerializeField]
	private GameObject altTextTrappedCharMugman;
}
