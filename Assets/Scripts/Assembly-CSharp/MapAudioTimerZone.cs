using System;
using UnityEngine;

// Token: 0x0200092E RID: 2350
public class MapAudioTimerZone : AbstractCollidableObject
{
	// Token: 0x060036FD RID: 14077 RVA: 0x001FB200 File Offset: 0x001F9600
	private void Start()
	{
		this.waitTime = UnityEngine.Random.Range(this.audioDelayRange.minimum, this.audioDelayRange.maximum);
	}

	// Token: 0x060036FE RID: 14078 RVA: 0x001FB224 File Offset: 0x001F9624
	private void Update()
	{
		if (this.playerCount > 0)
		{
			this.elapsedTime += CupheadTime.Delta;
			if (this.elapsedTime > this.waitTime)
			{
				AudioManager.Play(this.audioKey);
				this.elapsedTime = 0f;
				this.waitTime = UnityEngine.Random.Range(this.audioDelayRange.minimum, this.audioDelayRange.maximum);
			}
		}
	}

	// Token: 0x060036FF RID: 14079 RVA: 0x001FB29C File Offset: 0x001F969C
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (!hit.CompareTag("Player_Map"))
		{
			return;
		}
		if (phase == CollisionPhase.Enter)
		{
			this.playerCount++;
		}
		else if (phase == CollisionPhase.Exit)
		{
			this.playerCount--;
		}
	}

	// Token: 0x04003F30 RID: 16176
	[SerializeField]
	private string audioKey;

	// Token: 0x04003F31 RID: 16177
	[SerializeField]
	private Rangef audioDelayRange;

	// Token: 0x04003F32 RID: 16178
	private int playerCount;

	// Token: 0x04003F33 RID: 16179
	private float elapsedTime;

	// Token: 0x04003F34 RID: 16180
	private float waitTime;
}
