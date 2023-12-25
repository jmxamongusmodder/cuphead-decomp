using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200087C RID: 2172
public class ForestPlatformingLevelChomper : AbstractPlatformingLevelEnemy
{
	// Token: 0x0600326F RID: 12911 RVA: 0x001D5D3C File Offset: 0x001D413C
	protected override void OnStart()
	{
		this.startY = base.transform.position.y;
	}

	// Token: 0x06003270 RID: 12912 RVA: 0x001D5D62 File Offset: 0x001D4162
	public void StartAttacking()
	{
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06003271 RID: 12913 RVA: 0x001D5D74 File Offset: 0x001D4174
	private IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.initialDelay.RandomFloat());
		float timeToApex = this.speed / this.gravityUp;
		float upAnimTime = timeToApex - 0.333333f;
		float normalizedExtraTime = upAnimTime / 0.333333f % 1f;
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
			{
				AudioManager.Play("level_chomper_up");
				this.emitAudioFromObject.Add("level_chomper_up");
			}
			base.animator.Play("Up", 0, 1f - normalizedExtraTime);
			base.StartCoroutine(this.move_cr());
			yield return CupheadTime.WaitForSeconds(this, upAnimTime - 0.1666665f);
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
			{
				AudioManager.Play("level_chomper_bite");
				this.emitAudioFromObject.Add("level_chomper_bite");
			}
			base.animator.SetTrigger("Bite");
			while (this.moving)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, this.mainDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x001D5D90 File Offset: 0x001D4190
	private IEnumerator move_cr()
	{
		float timeToApex = this.speed / this.gravityUp;
		float t = 0f;
		this.moving = true;
		while (t < timeToApex)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(null, new float?(this.startY + this.speed * t - 0.5f * this.gravityUp * t * t), null);
			yield return new WaitForFixedUpdate();
		}
		yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		float apexY = base.transform.position.y;
		t = 0f;
		while (t < timeToApex)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(null, new float?(apexY - 0.5f * this.gravityDown * t * t), null);
			yield return new WaitForFixedUpdate();
		}
		this.moving = false;
		yield break;
	}

	// Token: 0x04003ACC RID: 15052
	[SerializeField]
	private float speed = 1000f;

	// Token: 0x04003ACD RID: 15053
	[SerializeField]
	private float gravityUp = 1600f;

	// Token: 0x04003ACE RID: 15054
	[SerializeField]
	private float gravityDown = 2400f;

	// Token: 0x04003ACF RID: 15055
	[SerializeField]
	private MinMax initialDelay = new MinMax(0f, 0.5f);

	// Token: 0x04003AD0 RID: 15056
	[SerializeField]
	private MinMax mainDelay = new MinMax(1f, 3f);

	// Token: 0x04003AD1 RID: 15057
	private const float UP_ANIM_LENGTH = 0.333333f;

	// Token: 0x04003AD2 RID: 15058
	private const float BITE_ANIM_TIME_TO_APEX = 0.333333f;

	// Token: 0x04003AD3 RID: 15059
	private const float FREEZE_TIME = 0.041666668f;

	// Token: 0x04003AD4 RID: 15060
	private const float ON_SCREEN_SOUND_PADDING = 100f;

	// Token: 0x04003AD5 RID: 15061
	private float startY;

	// Token: 0x04003AD6 RID: 15062
	private bool moving;
}
