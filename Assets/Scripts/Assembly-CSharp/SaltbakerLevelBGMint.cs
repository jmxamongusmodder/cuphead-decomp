using System;
using UnityEngine;

// Token: 0x020007BE RID: 1982
public class SaltbakerLevelBGMint : MonoBehaviour
{
	// Token: 0x06002CDF RID: 11487 RVA: 0x001A6E30 File Offset: 0x001A5230
	public void StartAnimation(int which)
	{
		this.anim.Play(which.ToString(), 0, UnityEngine.Random.Range(0f, 0.5f));
	}

	// Token: 0x06002CE0 RID: 11488 RVA: 0x001A6E5C File Offset: 0x001A525C
	private void AniEvent_JumpDown()
	{
		base.transform.position += this.nextPos.position - this.startPos.position;
		if (base.transform.position.y < -1000f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04003553 RID: 13651
	[SerializeField]
	private Transform startPos;

	// Token: 0x04003554 RID: 13652
	[SerializeField]
	private Transform nextPos;

	// Token: 0x04003555 RID: 13653
	[SerializeField]
	private Animator anim;
}
