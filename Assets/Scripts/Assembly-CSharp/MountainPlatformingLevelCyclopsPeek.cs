using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008DA RID: 2266
public class MountainPlatformingLevelCyclopsPeek : AbstractPausableComponent
{
	// Token: 0x06003510 RID: 13584 RVA: 0x001ED6D9 File Offset: 0x001EBAD9
	private void Start()
	{
		base.StartCoroutine(this.check_player_cr());
		this.emitAudioFromObject.Add("castle_giant_head_peer");
	}

	// Token: 0x06003511 RID: 13585 RVA: 0x001ED6F8 File Offset: 0x001EBAF8
	private IEnumerator check_player_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.player = PlayerManager.GetNext();
		float t = 0f;
		float time = UnityEngine.Random.Range(3f, 6f);
		float laughTime = UnityEngine.Random.Range(6f, 10f);
		float t_laugh = 0f;
		for (;;)
		{
			if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
			{
				t += CupheadTime.Delta;
				if (t >= time)
				{
					this.player = PlayerManager.GetNext();
					t = 0f;
					time = UnityEngine.Random.Range(3f, 6f);
				}
			}
			if (Vector3.Distance(PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position, base.transform.position) < 1000f)
			{
				t_laugh += CupheadTime.Delta;
				if (t_laugh >= laughTime)
				{
					this.SoundGiantHeadPeer();
					t_laugh = 0f;
					laughTime = UnityEngine.Random.Range(6f, 10f);
				}
			}
			if (this.player.transform.position.x < base.transform.position.x - this.offset)
			{
				if (this.currentEyeState != MountainPlatformingLevelCyclopsPeek.EyeState.left)
				{
					base.animator.SetInteger("SideOn", 0);
					this.currentEyeState = MountainPlatformingLevelCyclopsPeek.EyeState.left;
				}
			}
			else if (this.player.transform.position.x > base.transform.position.x + this.offset)
			{
				if (this.currentEyeState != MountainPlatformingLevelCyclopsPeek.EyeState.right)
				{
					base.animator.SetInteger("SideOn", 2);
					this.currentEyeState = MountainPlatformingLevelCyclopsPeek.EyeState.right;
				}
			}
			else if (this.currentEyeState != MountainPlatformingLevelCyclopsPeek.EyeState.middle)
			{
				base.animator.SetInteger("SideOn", 1);
				this.currentEyeState = MountainPlatformingLevelCyclopsPeek.EyeState.middle;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x001ED713 File Offset: 0x001EBB13
	private void SoundGiantHeadPeer()
	{
		AudioManager.Play("castle_giant_head_peer");
		this.emitAudioFromObject.Add("castle_giant_head_peer");
	}

	// Token: 0x04003D33 RID: 15667
	private MountainPlatformingLevelCyclopsPeek.EyeState currentEyeState;

	// Token: 0x04003D34 RID: 15668
	private float offset = 300f;

	// Token: 0x04003D35 RID: 15669
	private AbstractPlayerController player;

	// Token: 0x020008DB RID: 2267
	public enum EyeState
	{
		// Token: 0x04003D37 RID: 15671
		left,
		// Token: 0x04003D38 RID: 15672
		middle,
		// Token: 0x04003D39 RID: 15673
		right
	}
}
