using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005CE RID: 1486
public class DicePalaceFlyingMemoryMusicNote : BasicProjectile
{
	// Token: 0x06001D2F RID: 7471 RVA: 0x0010C2B4 File Offset: 0x0010A6B4
	public DicePalaceFlyingMemoryMusicNote Create(Vector3 pos, float rotation, float speed, float deathTimer)
	{
		DicePalaceFlyingMemoryMusicNote dicePalaceFlyingMemoryMusicNote = base.Create(pos, rotation, speed) as DicePalaceFlyingMemoryMusicNote;
		dicePalaceFlyingMemoryMusicNote.deathTimer = deathTimer;
		return dicePalaceFlyingMemoryMusicNote;
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x0010C2DE File Offset: 0x0010A6DE
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.death_timer_cr());
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x0010C2F4 File Offset: 0x0010A6F4
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.sprite.transform.SetEulerAngles(null, null, new float?(0f));
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x0010C334 File Offset: 0x0010A734
	private IEnumerator death_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.deathTimer);
		base.animator.SetTrigger("OnDeath");
		yield return base.animator.WaitForAnimationToEnd(this, "Note_Death", false, true);
		this.move = false;
		yield return null;
		yield break;
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x0010C34F File Offset: 0x0010A74F
	private void Kill()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400261D RID: 9757
	[SerializeField]
	private Transform sprite;

	// Token: 0x0400261E RID: 9758
	private float deathTimer;
}
