using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D5 RID: 2261
public class HarbourPlatformingLevelUrchin : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x060034E7 RID: 13543 RVA: 0x001EC588 File Offset: 0x001EA988
	protected override void Start()
	{
		base.Start();
		base.GetComponent<PlatformingLevelEnemyAnimationHandler>().SelectAnimation(this.type.ToString());
		base.StartCoroutine(this.play_loop_SFX());
	}

	// Token: 0x060034E8 RID: 13544 RVA: 0x001EC5BC File Offset: 0x001EA9BC
	private IEnumerator play_loop_SFX()
	{
		bool playerLeft = false;
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
			{
				playerLeft = false;
				if (!AudioManager.CheckIfPlaying("harbour_urchin_walk"))
				{
					AudioManager.PlayLoop("harbour_urchin_walk");
					this.emitAudioFromObject.Add("harbour_urchin_walk");
				}
			}
			else if (!playerLeft)
			{
				AudioManager.Stop("harbour_urchin_walk");
				playerLeft = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060034E9 RID: 13545 RVA: 0x001EC5D8 File Offset: 0x001EA9D8
	protected override Coroutine Turn()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
		{
			AudioManager.Play("harbour_urchin_turn");
			this.emitAudioFromObject.Add("harbour_urchin_turn");
		}
		return base.Turn();
	}

	// Token: 0x060034EA RID: 13546 RVA: 0x001EC633 File Offset: 0x001EAA33
	protected override void Die()
	{
		base.Die();
		AudioManager.Stop("harbour_urchin_walk");
		AudioManager.Play("harmour_urchin_death");
		this.emitAudioFromObject.Add("harmour_urchin_death");
	}

	// Token: 0x04003D18 RID: 15640
	private const float ON_SCREEN_SOUND_PADDING = 100f;

	// Token: 0x04003D19 RID: 15641
	public HarbourPlatformingLevelUrchin.Type type;

	// Token: 0x04003D1A RID: 15642
	private bool isInSight;

	// Token: 0x020008D6 RID: 2262
	public enum Type
	{
		// Token: 0x04003D1C RID: 15644
		A,
		// Token: 0x04003D1D RID: 15645
		B
	}
}
