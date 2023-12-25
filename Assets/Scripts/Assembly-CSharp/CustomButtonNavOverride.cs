using System;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C29 RID: 3113
public class CustomButtonNavOverride : CustomButton
{
	// Token: 0x06004C10 RID: 19472 RVA: 0x002721BD File Offset: 0x002705BD
	public override Selectable FindSelectableOnUp()
	{
		if (!PlayerManager.Multiplayer)
		{
			return this.upOnSinglePlayer;
		}
		return (!this.upOnMultiPlayer) ? this.mapper.GetUnselectedPlayerButton() : this.upOnMultiPlayer;
	}

	// Token: 0x06004C11 RID: 19473 RVA: 0x002721F8 File Offset: 0x002705F8
	public override Selectable FindSelectableOnDown()
	{
		if (PlayerManager.Multiplayer)
		{
			return (!this.downOnMultiPlayer) ? this.mapper.GetUnselectedPlayerButton() : this.downOnMultiPlayer;
		}
		if (PlatformHelper.IsConsole)
		{
			return this;
		}
		return this.downOnSinglePlayer;
	}

	// Token: 0x040050B5 RID: 20661
	[SerializeField]
	private Selectable upOnSinglePlayer;

	// Token: 0x040050B6 RID: 20662
	[SerializeField]
	private Selectable downOnSinglePlayer;

	// Token: 0x040050B7 RID: 20663
	[SerializeField]
	private Selectable upOnMultiPlayer;

	// Token: 0x040050B8 RID: 20664
	[SerializeField]
	private Selectable downOnMultiPlayer;

	// Token: 0x040050B9 RID: 20665
	[SerializeField]
	private ControlMapper mapper;
}
