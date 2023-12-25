using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007AA RID: 1962
public class SallyStagePlayLevelCherubProjectile : BasicProjectile
{
	// Token: 0x06002C18 RID: 11288 RVA: 0x0019EBD0 File Offset: 0x0019CFD0
	public SallyStagePlayLevelCherubProjectile Create(Vector3 position, float rotation, float speed)
	{
		return base.Create(position, rotation, speed) as SallyStagePlayLevelCherubProjectile;
	}

	// Token: 0x06002C19 RID: 11289 RVA: 0x0019EBF2 File Offset: 0x0019CFF2
	protected override void Start()
	{
		base.Start();
		this.selfAnimator = base.GetComponent<Animator>();
		AudioManager.Play("sally_cherub_vocal_wheelin");
		this.emitAudioFromObject.Add("sally_cherub_vocal_wheelin");
		base.StartCoroutine(this.wait_to_launch());
	}

	// Token: 0x06002C1A RID: 11290 RVA: 0x0019EC30 File Offset: 0x0019D030
	private IEnumerator wait_to_launch()
	{
		while (base.transform.position.x < -400f)
		{
			yield return null;
		}
		this.selfAnimator.SetTrigger("OnLaunch");
		yield return this.selfAnimator.WaitForAnimationToEnd(this, "Cherub_Push", false, true);
		base.StartCoroutine(this.cherub_leaves_cr());
		yield break;
	}

	// Token: 0x06002C1B RID: 11291 RVA: 0x0019EC4C File Offset: 0x0019D04C
	private IEnumerator cherub_leaves_cr()
	{
		float time = 1f;
		float t = 0f;
		float start = this.cherub.transform.position.x;
		float end = -740f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
			this.cherub.transform.SetPosition(new float?(Mathf.Lerp(start, end, val)), null, null);
			t += CupheadTime.Delta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002C1C RID: 11292 RVA: 0x0019EC67 File Offset: 0x0019D067
	private void CherubPush()
	{
		this.move = false;
		base.StartCoroutine(this.launch_firewheel_cr());
	}

	// Token: 0x06002C1D RID: 11293 RVA: 0x0019EC80 File Offset: 0x0019D080
	private IEnumerator launch_firewheel_cr()
	{
		this.firewheel.PlaySound();
		while (this.firewheel.transform.position.x < 740f)
		{
			this.firewheel.transform.position += Vector3.right * this.Speed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x040034CD RID: 13517
	private const float DIST_TO_LAUNCH = -400f;

	// Token: 0x040034CE RID: 13518
	private const float OFFSET = 100f;

	// Token: 0x040034CF RID: 13519
	[SerializeField]
	private GameObject cherub;

	// Token: 0x040034D0 RID: 13520
	[SerializeField]
	private SallyStagePlayLevelFirewheel firewheel;

	// Token: 0x040034D1 RID: 13521
	private Animator selfAnimator;
}
