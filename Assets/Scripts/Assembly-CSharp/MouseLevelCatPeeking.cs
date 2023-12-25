using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006EA RID: 1770
public class MouseLevelCatPeeking : MonoBehaviour
{
	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x060025E7 RID: 9703 RVA: 0x00163399 File Offset: 0x00161799
	public float Peek1Threshold
	{
		get
		{
			return this.peek1Threshold;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x060025E8 RID: 9704 RVA: 0x001633A1 File Offset: 0x001617A1
	public float Peek2Threshold
	{
		get
		{
			return this.peek2Threshold;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x060025E9 RID: 9705 RVA: 0x001633A9 File Offset: 0x001617A9
	// (set) Token: 0x060025EA RID: 9706 RVA: 0x001633B1 File Offset: 0x001617B1
	public bool IsPhase2
	{
		get
		{
			return this.isPhase2;
		}
		set
		{
			this.isPhase2 = value;
			this.catAnimator.SetBool("IsPhase2", value);
		}
	}

	// Token: 0x060025EB RID: 9707 RVA: 0x001633CB File Offset: 0x001617CB
	public void StartPeeking()
	{
		this.peekRoutine = this.catPeeking_cr();
		base.StartCoroutine(this.peekRoutine);
	}

	// Token: 0x060025EC RID: 9708 RVA: 0x001633E6 File Offset: 0x001617E6
	public void StopPeeking()
	{
		base.StopCoroutine(this.peekRoutine);
	}

	// Token: 0x060025ED RID: 9709 RVA: 0x001633F4 File Offset: 0x001617F4
	private IEnumerator catPeeking_cr()
	{
		Transform catTransform = base.transform;
		for (;;)
		{
			bool isRight = Rand.Bool();
			this.catAnimator.SetBool("IsRight", isRight);
			catTransform.eulerAngles = Vector3.forward * this.catRotationRange.RandomFloat();
			this.catAnimator.SetTrigger("Peek");
			yield return null;
			yield return this.catAnimator.WaitForAnimationToEnd(this, true);
			yield return CupheadTime.WaitForSeconds(this, this.catDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x04002E6F RID: 11887
	private const string CatPeekParameterName = "Peek";

	// Token: 0x04002E70 RID: 11888
	private const string IsRightParameterName = "IsRight";

	// Token: 0x04002E71 RID: 11889
	[SerializeField]
	private Animator catAnimator;

	// Token: 0x04002E72 RID: 11890
	[SerializeField]
	private MinMax catDelay;

	// Token: 0x04002E73 RID: 11891
	[SerializeField]
	private MinMax catRotationRange;

	// Token: 0x04002E74 RID: 11892
	[Range(0f, 1f)]
	[SerializeField]
	private float peek1Threshold;

	// Token: 0x04002E75 RID: 11893
	[Range(0f, 1f)]
	[SerializeField]
	private float peek2Threshold;

	// Token: 0x04002E76 RID: 11894
	private bool isPhase2;

	// Token: 0x04002E77 RID: 11895
	private IEnumerator peekRoutine;
}
