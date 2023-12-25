using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B35 RID: 2869
public class WinScreenTicker : AbstractMonoBehaviour
{
	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x0600458D RID: 17805 RVA: 0x0024AA8C File Offset: 0x00248E8C
	// (set) Token: 0x0600458E RID: 17806 RVA: 0x0024AA94 File Offset: 0x00248E94
	public int TargetValue { get; set; }

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x0600458F RID: 17807 RVA: 0x0024AA9D File Offset: 0x00248E9D
	// (set) Token: 0x06004590 RID: 17808 RVA: 0x0024AAA5 File Offset: 0x00248EA5
	public int MaxValue { get; set; }

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x06004591 RID: 17809 RVA: 0x0024AAAE File Offset: 0x00248EAE
	// (set) Token: 0x06004592 RID: 17810 RVA: 0x0024AAB6 File Offset: 0x00248EB6
	public bool FinishedCounting { get; private set; }

	// Token: 0x06004593 RID: 17811 RVA: 0x0024AABF File Offset: 0x00248EBF
	protected override void Awake()
	{
		base.Awake();
		this.input = new CupheadInput.AnyPlayerInput(false);
	}

	// Token: 0x06004594 RID: 17812 RVA: 0x0024AAD3 File Offset: 0x00248ED3
	private void Start()
	{
		base.StartCoroutine(this.select_type_cr());
	}

	// Token: 0x06004595 RID: 17813 RVA: 0x0024AAE4 File Offset: 0x00248EE4
	private IEnumerator select_type_cr()
	{
		switch (this.tickerType)
		{
		case WinScreenTicker.TickerType.Time:
			base.StartCoroutine(this.time_tally_up_cr());
			break;
		case WinScreenTicker.TickerType.Health:
			base.StartCoroutine(this.health_tally_up_cr());
			break;
		case WinScreenTicker.TickerType.Score:
			base.StartCoroutine(this.score_tally_up_cr());
			break;
		case WinScreenTicker.TickerType.Stars:
			base.StartCoroutine(this.stars_tally_up_cr());
			break;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06004596 RID: 17814 RVA: 0x0024AB00 File Offset: 0x00248F00
	private IEnumerator health_tally_up_cr()
	{
		bool isTallying = true;
		float t = 0f;
		int counter = 0;
		this.valueText.text = counter + " " + this.MaxValue;
		while (!this.startedCounting)
		{
			yield return null;
		}
		while (counter < this.TargetValue && isTallying)
		{
			if (counter >= this.TargetValue)
			{
				break;
			}
			while (t < 0.03f)
			{
				if (this.input.GetButtonDown(CupheadButton.Jump))
				{
					isTallying = false;
					break;
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			if (isTallying)
			{
				AudioManager.Play("win_score_tick");
				counter++;
				this.valueText.text = counter + " " + this.MaxValue;
			}
		}
		this.valueText.text = this.TargetValue + " " + this.MaxValue;
		this.valueText.GetComponent<Animator>().SetTrigger("MakeBigTally");
		if (this.TargetValue == this.MaxValue)
		{
			AudioManager.Play("win_score_tick");
			this.valueText.color = ColorUtils.HexToColor("FCC93D");
		}
		yield return null;
		this.FinishedCounting = true;
		yield break;
	}

	// Token: 0x06004597 RID: 17815 RVA: 0x0024AB1C File Offset: 0x00248F1C
	private IEnumerator score_tally_up_cr()
	{
		bool isTallying = true;
		float t = 0f;
		int counter = 0;
		this.valueText.text = counter + " " + this.MaxValue;
		if (this.leaderDots.Length > 0)
		{
			this.leaderDots[0].enabled = true;
			this.leaderDots[1].enabled = false;
		}
		while (!this.startedCounting)
		{
			yield return null;
		}
		while (counter <= this.TargetValue && isTallying)
		{
			if (counter >= this.TargetValue)
			{
				break;
			}
			while (t < 0.03f)
			{
				if (this.input.GetButtonDown(CupheadButton.Jump))
				{
					isTallying = false;
					break;
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			if (isTallying)
			{
				AudioManager.Play("win_score_tick");
				counter++;
				if (this.leaderDots.Length > 0 && counter > 9)
				{
					this.leaderDots[0].enabled = false;
					this.leaderDots[1].enabled = true;
				}
				this.valueText.text = counter + " " + this.MaxValue;
			}
			yield return null;
		}
		this.valueText.text = this.TargetValue + " " + this.MaxValue;
		this.valueText.GetComponent<Animator>().SetTrigger("MakeBigTally");
		if (this.TargetValue == this.MaxValue)
		{
			AudioManager.Play("win_score_tick");
			this.valueText.color = ColorUtils.HexToColor("FCC93D");
		}
		yield return null;
		this.FinishedCounting = true;
		yield break;
	}

	// Token: 0x06004598 RID: 17816 RVA: 0x0024AB38 File Offset: 0x00248F38
	private IEnumerator time_tally_up_cr()
	{
		bool isTallying = true;
		float t = 0f;
		int minutesMax = this.MaxValue / 60;
		int secondsMax = this.MaxValue % 60;
		int minutesTarget = this.TargetValue / 60;
		int secondsTarget = this.TargetValue % 60;
		int secondCounter = 0;
		int minuteCounter = 0;
		this.valueText.text = "00 00";
		while (!this.startedCounting)
		{
			yield return null;
		}
		AudioManager.PlayLoop("win_time_ticker_loop");
		while (isTallying)
		{
			if (secondCounter < 60)
			{
				secondCounter++;
			}
			else
			{
				minuteCounter++;
				secondCounter = 0;
			}
			string displayedMinutes = (minuteCounter > 9) ? minuteCounter.ToString() : ("0" + minuteCounter.ToString());
			string displayedSeconds = (secondCounter > 9) ? secondCounter.ToString() : ("0" + secondCounter.ToString());
			this.valueText.text = displayedMinutes + " " + displayedSeconds;
			if (minuteCounter >= minutesTarget && secondCounter >= secondsTarget)
			{
				isTallying = false;
				break;
			}
			while (t < 0.03f)
			{
				if (this.input.GetButtonDown(CupheadButton.Jump))
				{
					isTallying = false;
					break;
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
		}
		AudioManager.Stop("win_time_ticker_loop");
		AudioManager.Play("win_time_ticker_loop_end");
		string minutes = (minutesTarget > 9) ? minutesTarget.ToString() : ("0" + minutesTarget.ToString());
		string seconds = (secondsTarget > 9) ? secondsTarget.ToString() : ("0" + secondsTarget.ToString());
		this.valueText.text = minutes + " " + seconds;
		if (minutesTarget == minutesMax)
		{
			if (secondsTarget <= secondsMax)
			{
				AudioManager.Play("win_score_tick");
				this.valueText.color = ColorUtils.HexToColor("FCC93D");
			}
		}
		else if (minutesTarget < minutesMax)
		{
			AudioManager.Play("win_score_tick");
			this.valueText.color = ColorUtils.HexToColor("FCC93D");
		}
		this.valueText.GetComponent<Animator>().SetTrigger("MakeBigTally");
		this.FinishedCounting = true;
		yield return null;
		yield break;
	}

	// Token: 0x06004599 RID: 17817 RVA: 0x0024AB54 File Offset: 0x00248F54
	private IEnumerator stars_tally_up_cr()
	{
		int startVal = 0;
		if (this.TargetValue == 2)
		{
			this.leaderDots[0].enabled = false;
			this.leaderDots[1].enabled = true;
			this.stars[0].gameObject.SetActive(true);
		}
		else
		{
			this.leaderDots[0].enabled = true;
			this.leaderDots[1].enabled = false;
			this.stars[0].gameObject.SetActive(false);
			startVal = 1;
		}
		YieldInstruction time = new WaitForSeconds(0.5f);
		while (!this.startedCounting)
		{
			yield return null;
		}
		for (int i = startVal; i < this.TargetValue + 1 + startVal; i++)
		{
			this.stars[i].SetTrigger("OnAppear");
			AudioManager.Play("win_skill_lvl");
			if (!this.input.GetButtonDown(CupheadButton.Accept))
			{
				yield return time;
			}
		}
		this.FinishedCounting = true;
		yield return null;
		yield break;
	}

	// Token: 0x0600459A RID: 17818 RVA: 0x0024AB6F File Offset: 0x00248F6F
	public void StartCounting()
	{
		this.startedCounting = true;
	}

	// Token: 0x04004BB9 RID: 19385
	public WinScreenTicker.TickerType tickerType;

	// Token: 0x04004BBA RID: 19386
	[SerializeField]
	private Animator[] stars;

	// Token: 0x04004BBB RID: 19387
	[SerializeField]
	private Text[] leaderDots;

	// Token: 0x04004BBC RID: 19388
	[SerializeField]
	private Text label;

	// Token: 0x04004BBD RID: 19389
	[SerializeField]
	private Text valueText;

	// Token: 0x04004BC0 RID: 19392
	private bool startedCounting;

	// Token: 0x04004BC2 RID: 19394
	private const float TIME_COUNTER_TIME = 0.03f;

	// Token: 0x04004BC3 RID: 19395
	private const float USUAL_COUNTER_TIME = 0.07f;

	// Token: 0x04004BC4 RID: 19396
	private const float STAR_COUNTER_TIME = 0.5f;

	// Token: 0x04004BC5 RID: 19397
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x02000B36 RID: 2870
	public enum TickerType
	{
		// Token: 0x04004BC7 RID: 19399
		Time,
		// Token: 0x04004BC8 RID: 19400
		Health,
		// Token: 0x04004BC9 RID: 19401
		Score,
		// Token: 0x04004BCA RID: 19402
		Stars
	}
}
