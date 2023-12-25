using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200065C RID: 1628
public class FlyingCowboyLevelSausageLink : BasicProjectile
{
	// Token: 0x060021EC RID: 8684 RVA: 0x0013C1C8 File Offset: 0x0013A5C8
	public void Initialize(FlyingCowboyLevelMeat.SausageType sausageType, Transform sausageLinkSqueezePoint, FlyingCowboyLevelSausageLink previousLink)
	{
		this.sausageType = sausageType;
		if (sausageType != FlyingCowboyLevelMeat.SausageType.U1 && sausageType != FlyingCowboyLevelMeat.SausageType.U2 && sausageType != FlyingCowboyLevelMeat.SausageType.U3)
		{
			base.StartCoroutine(this.squeeze_cr(sausageLinkSqueezePoint, previousLink));
		}
		if (sausageType == FlyingCowboyLevelMeat.SausageType.H1 || sausageType == FlyingCowboyLevelMeat.SausageType.H2 || sausageType == FlyingCowboyLevelMeat.SausageType.H3 || sausageType == FlyingCowboyLevelMeat.SausageType.H4 || sausageType == FlyingCowboyLevelMeat.SausageType.L5)
		{
			base.animator.SetFloat("Speed", (float)Rand.PosOrNeg());
		}
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x0013C238 File Offset: 0x0013A638
	public void Squeeze()
	{
		base.animator.Play("Squeeze" + this.sausageType.ToString());
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x0013C260 File Offset: 0x0013A660
	private IEnumerator squeeze_cr(Transform sausageLinkSqueezePoint, FlyingCowboyLevelSausageLink previousLink)
	{
		while (base.transform.position.x > sausageLinkSqueezePoint.position.x)
		{
			yield return null;
		}
		this.Squeeze();
		if (previousLink != null)
		{
			previousLink.Squeeze();
		}
		yield break;
	}

	// Token: 0x04002AA6 RID: 10918
	private FlyingCowboyLevelMeat.SausageType sausageType;
}
