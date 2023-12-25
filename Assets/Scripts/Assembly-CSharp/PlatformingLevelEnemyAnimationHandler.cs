using System;
using UnityEngine;

// Token: 0x02000903 RID: 2307
public class PlatformingLevelEnemyAnimationHandler : AbstractPausableComponent
{
	// Token: 0x0600361D RID: 13853 RVA: 0x001F6F08 File Offset: 0x001F5308
	public void SelectAnimation(string type1)
	{
		for (int i = 0; i < this.numOfTypes; i++)
		{
			if (type1.Substring(0, 1) == "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(new char[]
			{
				','
			})[i])
			{
				this.index1 = i;
			}
			if (this.secondaryTypes > 0 && type1.Substring(1, 1) == "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(new char[]
			{
				','
			})[i])
			{
				this.index2 = i + 1;
			}
		}
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.enabled = false;
		}
		base.GetComponentsInChildren<SpriteRenderer>()[this.index1].enabled = true;
		if (this.secondaryTypes > 0)
		{
			base.GetComponent<Animator>().SetInteger("type", this.index2);
		}
	}

	// Token: 0x04003E20 RID: 15904
	[SerializeField]
	private int numOfTypes;

	// Token: 0x04003E21 RID: 15905
	[SerializeField]
	private int secondaryTypes;

	// Token: 0x04003E22 RID: 15906
	private int index1;

	// Token: 0x04003E23 RID: 15907
	private int index2;

	// Token: 0x04003E24 RID: 15908
	private const string LETTERS = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
}
