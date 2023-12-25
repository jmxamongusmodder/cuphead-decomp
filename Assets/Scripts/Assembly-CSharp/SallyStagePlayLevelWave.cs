using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007BB RID: 1979
public class SallyStagePlayLevelWave : AbstractCollidableObject
{
	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06002CBC RID: 11452 RVA: 0x001A6262 File Offset: 0x001A4662
	// (set) Token: 0x06002CBD RID: 11453 RVA: 0x001A626A File Offset: 0x001A466A
	public bool isMoving { get; private set; }

	// Token: 0x06002CBE RID: 11454 RVA: 0x001A6273 File Offset: 0x001A4673
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.startPos = base.transform.position;
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x001A6291 File Offset: 0x001A4691
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002CC0 RID: 11456 RVA: 0x001A62A9 File Offset: 0x001A46A9
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002CC1 RID: 11457 RVA: 0x001A62C7 File Offset: 0x001A46C7
	public void StartWave(LevelProperties.SallyStagePlay.Tidal properties)
	{
		this.properties = properties;
		base.transform.position = this.startPos;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002CC2 RID: 11458 RVA: 0x001A62F0 File Offset: 0x001A46F0
	private IEnumerator move_cr()
	{
		float sizeX = base.GetComponent<Renderer>().bounds.size.x;
		this.isMoving = true;
		while (base.transform.position.x < 640f + sizeX)
		{
			base.transform.position += base.transform.right * this.properties.tidalSpeed * CupheadTime.Delta;
			yield return null;
		}
		this.isMoving = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002CC3 RID: 11459 RVA: 0x001A630B File Offset: 0x001A470B
	private void SoundBigWaveFeet()
	{
		if (this.isMoving)
		{
			AudioManager.Play("sally_wave");
			this.emitAudioFromObject.Add("sally_wave");
		}
	}

	// Token: 0x06002CC4 RID: 11460 RVA: 0x001A6332 File Offset: 0x001A4732
	private void SoundBigWaveVoice()
	{
		if (this.isMoving)
		{
			AudioManager.Play("sally_wave_sweet");
			this.emitAudioFromObject.Add("sally_wave_sweet");
		}
	}

	// Token: 0x0400353D RID: 13629
	private DamageDealer damageDealer;

	// Token: 0x0400353E RID: 13630
	private LevelProperties.SallyStagePlay.Tidal properties;

	// Token: 0x0400353F RID: 13631
	private Vector3 startPos;
}
