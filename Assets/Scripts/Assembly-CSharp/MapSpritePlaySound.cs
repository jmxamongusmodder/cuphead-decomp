using System;
using UnityEngine;

// Token: 0x02000962 RID: 2402
public class MapSpritePlaySound : AbstractCollidableObject
{
	// Token: 0x0600380B RID: 14347 RVA: 0x00201491 File Offset: 0x001FF891
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x0020149C File Offset: 0x001FF89C
	public void PlaySoundRight(bool isP1)
	{
		MapSpritePlaySound.SoundToPlay soundToPlay = this.getSound;
		if (soundToPlay != MapSpritePlaySound.SoundToPlay.Wood)
		{
			if (soundToPlay != MapSpritePlaySound.SoundToPlay.Rainbow)
			{
			}
		}
		else
		{
			AudioManager.Play((!isP1) ? "player_map_walk_wood_two_p2" : "player_map_walk_wood_two_p1");
		}
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x002014E8 File Offset: 0x001FF8E8
	public void PlaySoundLeft(bool isP1)
	{
		MapSpritePlaySound.SoundToPlay soundToPlay = this.getSound;
		if (soundToPlay != MapSpritePlaySound.SoundToPlay.Wood)
		{
			if (soundToPlay != MapSpritePlaySound.SoundToPlay.Rainbow)
			{
			}
		}
		else
		{
			AudioManager.Play((!isP1) ? "player_map_walk_wood_one_p2" : "player_map_walk_wood_one_p1");
		}
	}

	// Token: 0x04003FED RID: 16365
	public MapSpritePlaySound.SoundToPlay getSound;

	// Token: 0x02000963 RID: 2403
	public enum SoundToPlay
	{
		// Token: 0x04003FEF RID: 16367
		Wood,
		// Token: 0x04003FF0 RID: 16368
		Rainbow
	}
}
