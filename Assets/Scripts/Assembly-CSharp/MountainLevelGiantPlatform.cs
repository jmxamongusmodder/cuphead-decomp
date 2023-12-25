using System;
using UnityEngine;

// Token: 0x020008D7 RID: 2263
public class MountainLevelGiantPlatform : LevelPlatform
{
	// Token: 0x060034EC RID: 13548 RVA: 0x001EC780 File Offset: 0x001EAB80
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter && hit.GetComponent<MountainPlatformingLevelCyclops>())
		{
			this.explosion.Create(base.transform.position);
			this.SpawnParts();
			if (base.transform.childCount > 0 && base.GetComponentInChildren<LevelPlayerMotor>())
			{
				base.GetComponentInChildren<LevelPlayerMotor>().OnPitKnockUp(10f, 1f);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x001EC80C File Offset: 0x001EAC0C
	private void SpawnParts()
	{
		foreach (SpriteDeathParts spriteDeathParts in this.sprites)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x04003D1E RID: 15646
	[SerializeField]
	private Effect explosion;

	// Token: 0x04003D1F RID: 15647
	[SerializeField]
	private SpriteDeathParts[] sprites;
}
