using System;
using UnityEngine;

// Token: 0x0200054D RID: 1357
public class ChessQueenLevelLooseMouse : MonoBehaviour
{
	// Token: 0x06001916 RID: 6422 RVA: 0x000E36B7 File Offset: 0x000E1AB7
	private void Start()
	{
		this.jumpSwitchTime = UnityEngine.Random.Range(2f, 3f);
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x000E36D0 File Offset: 0x000E1AD0
	private void Update()
	{
		this.anim.SetBool("Right", this.queen.transform.position.x > -200f);
		if (!this.won && this.activeCannonball == null)
		{
			this.jumpSwitchTime -= CupheadTime.Delta;
			if (this.jumpSwitchTime < 0f)
			{
				this.anim.SetBool("Jump", !this.anim.GetBool("Jump"));
				this.jumpSwitchTime = ((!this.anim.GetBool("Jump")) ? UnityEngine.Random.Range(3f, 4f) : UnityEngine.Random.Range(0.5f, 2f));
			}
		}
	}

	// Token: 0x06001918 RID: 6424 RVA: 0x000E37B0 File Offset: 0x000E1BB0
	public void HitQueen()
	{
		this.anim.SetTrigger("HitQueen");
		this.anim.SetBool("Jump", true);
		this.jumpSwitchTime = UnityEngine.Random.Range(2f, 3f);
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x000E37E8 File Offset: 0x000E1BE8
	public void CannonFired(GameObject cannonBall)
	{
		this.anim.SetBool("Jump", false);
		this.activeCannonball = cannonBall;
		this.jumpSwitchTime = UnityEngine.Random.Range(3f, 4f);
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x000E3817 File Offset: 0x000E1C17
	public void Win()
	{
		this.anim.SetBool("Jump", true);
		this.won = true;
	}

	// Token: 0x04002230 RID: 8752
	[SerializeField]
	private Animator anim;

	// Token: 0x04002231 RID: 8753
	[SerializeField]
	private ChessQueenLevelQueen queen;

	// Token: 0x04002232 RID: 8754
	private bool won;

	// Token: 0x04002233 RID: 8755
	private float jumpSwitchTime;

	// Token: 0x04002234 RID: 8756
	private GameObject activeCannonball;
}
