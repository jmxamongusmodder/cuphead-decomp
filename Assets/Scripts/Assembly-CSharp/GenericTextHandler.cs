using System;
using UnityEngine;

// Token: 0x02000405 RID: 1029
public class GenericTextHandler : AbstractPausableComponent
{
	// Token: 0x06000E4E RID: 3662 RVA: 0x00092950 File Offset: 0x00090D50
	private void Start()
	{
		foreach (GameObject gameObject in this.otherText)
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x00092983 File Offset: 0x00090D83
	private void ShowText()
	{
		this.textChosen.SetActive(true);
	}

	// Token: 0x0400179A RID: 6042
	[SerializeField]
	private GameObject textChosen;

	// Token: 0x0400179B RID: 6043
	[SerializeField]
	private GameObject[] otherText;
}
