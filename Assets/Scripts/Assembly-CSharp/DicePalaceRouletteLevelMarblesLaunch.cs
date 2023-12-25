using System;
using UnityEngine;

// Token: 0x020005E3 RID: 1507
public class DicePalaceRouletteLevelMarblesLaunch : AbstractMonoBehaviour
{
	// Token: 0x17000371 RID: 881
	// (set) Token: 0x06001DDE RID: 7646 RVA: 0x00112D2A File Offset: 0x0011112A
	public bool IsFirstTime
	{
		set
		{
			base.animator.SetBool("IsFirstTime", value);
		}
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x00112D3D File Offset: 0x0011113D
	public void KillMarblesLaunch()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040026AD RID: 9901
	private const string IsFirstTimeParameterName = "IsFirstTime";
}
