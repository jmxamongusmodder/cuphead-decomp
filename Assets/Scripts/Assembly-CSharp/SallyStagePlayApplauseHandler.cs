using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
public class SallyStagePlayApplauseHandler : AbstractPausableComponent
{
	// Token: 0x06002BC3 RID: 11203 RVA: 0x00198BE8 File Offset: 0x00196FE8
	private void Start()
	{
		this.handsStartPos = new Vector3[this.hands.Length];
		for (int i = 0; i < this.hands.Length; i++)
		{
			this.hands[i].GetComponent<Animator>().Play((!Rand.Bool()) ? "B" : "A");
			this.handsStartPos[i] = this.hands[i].transform.position;
		}
		this.pinkPattern = this.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = UnityEngine.Random.Range(0, this.pinkPattern.Length);
	}

	// Token: 0x06002BC4 RID: 11204 RVA: 0x00198C9F File Offset: 0x0019709F
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(base.transform.position, this.endPos.transform.position);
	}

	// Token: 0x06002BC5 RID: 11205 RVA: 0x00198CC8 File Offset: 0x001970C8
	public void SlideApplause(bool slideIn)
	{
		for (int i = 0; i < this.hands.Length; i++)
		{
			base.StartCoroutine(this.slide_cr(this.hands[i], this.handsStartPos[i], slideIn, UnityEngine.Random.Range(0.3f, 0.8f)));
			AudioManager.Play("sally_audience_applause");
		}
	}

	// Token: 0x06002BC6 RID: 11206 RVA: 0x00198D30 File Offset: 0x00197130
	private IEnumerator slide_cr(Transform hand, Vector3 handStart, bool slideIn, float delay)
	{
		Vector3 start = (!slideIn) ? new Vector3(hand.transform.position.x, this.endPos.position.y) : handStart;
		Vector3 end = (!slideIn) ? handStart : new Vector3(hand.transform.position.x, this.endPos.position.y);
		float t = 0f;
		float frameTime = 0f;
		float time = 0.6f;
		yield return CupheadTime.WaitForSeconds(this, delay);
		while (t < time)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				frameTime -= 0.041666668f;
				hand.transform.position = Vector3.Lerp(start, end, t / time);
			}
			yield return null;
		}
		hand.transform.position = end;
		yield return null;
		yield break;
	}

	// Token: 0x06002BC7 RID: 11207 RVA: 0x00198D68 File Offset: 0x00197168
	public void ThrowRose(Vector3 pos, LevelProperties.SallyStagePlay.Roses properties)
	{
		base.StartCoroutine(this.throw_rose_cr(pos, properties, this.roseHands[UnityEngine.Random.Range(0, this.roseHands.Length)]));
	}

	// Token: 0x06002BC8 RID: 11208 RVA: 0x00198D90 File Offset: 0x00197190
	private IEnumerator throw_rose_cr(Vector3 pos, LevelProperties.SallyStagePlay.Roses properties, Transform arm)
	{
		string animationName;
		arm.GetComponent<Animator>().Play(animationName = ((!Rand.Bool()) ? "Rose_2_A" : "Rose_1_A"));
		arm.transform.SetPosition(new float?(pos.x), null, null);
		float speed = 900f;
		yield return arm.GetComponent<Animator>().WaitForAnimationToEnd(this, animationName, false, true);
		this.roseStill.transform.position = new Vector3(arm.transform.position.x, arm.transform.position.y + 50f);
		while (this.roseStill.transform.position.y < (float)Level.Current.Ceiling + 100f)
		{
			this.roseStill.transform.position += Vector3.up * speed * CupheadTime.Delta;
			yield return null;
		}
		SallyStagePlayLevelRose r = this.rose.Create(pos, properties);
		r.SetParryable(this.pinkPattern[this.pinkIndex][0] == 'P');
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
		yield return null;
		yield break;
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x00198DC0 File Offset: 0x001971C0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.rose = null;
	}

	// Token: 0x0400346E RID: 13422
	private const float FRAME_TIME = 0.041666668f;

	// Token: 0x0400346F RID: 13423
	[SerializeField]
	private SallyStagePlayLevelRose rose;

	// Token: 0x04003470 RID: 13424
	[SerializeField]
	private Transform[] hands;

	// Token: 0x04003471 RID: 13425
	private Vector3[] handsStartPos;

	// Token: 0x04003472 RID: 13426
	[SerializeField]
	private Transform[] roseHands;

	// Token: 0x04003473 RID: 13427
	[SerializeField]
	private Transform roseStill;

	// Token: 0x04003474 RID: 13428
	[SerializeField]
	private Transform endPos;

	// Token: 0x04003475 RID: 13429
	[SerializeField]
	private string pinkString;

	// Token: 0x04003476 RID: 13430
	private string[] pinkPattern;

	// Token: 0x04003477 RID: 13431
	private int pinkIndex;
}
