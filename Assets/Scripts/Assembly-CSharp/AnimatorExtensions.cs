using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000366 RID: 870
public static class AnimatorExtensions
{
	// Token: 0x060009AA RID: 2474 RVA: 0x0007C344 File Offset: 0x0007A744
	public static float GetCurrentClipLength(this Animator animator, int layer = 0)
	{
		AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(layer);
		if (currentAnimatorClipInfo.Length == 0)
		{
			return 0f;
		}
		AnimationClip clip = currentAnimatorClipInfo[0].clip;
		return clip.length;
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0007C37A File Offset: 0x0007A77A
	public static void FloorFrame(this Animator animator, int layer = 0)
	{
		if (AnimatorExtensions.<>f__mg$cache0 == null)
		{
			AnimatorExtensions.<>f__mg$cache0 = new Func<float, float>(Mathf.Floor);
		}
		AnimatorExtensions.roundFrame(animator, layer, AnimatorExtensions.<>f__mg$cache0);
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0007C3A0 File Offset: 0x0007A7A0
	public static void RoundFrame(this Animator animator, int layer = 0)
	{
		if (AnimatorExtensions.<>f__mg$cache1 == null)
		{
			AnimatorExtensions.<>f__mg$cache1 = new Func<float, float>(Mathf.Round);
		}
		AnimatorExtensions.roundFrame(animator, layer, AnimatorExtensions.<>f__mg$cache1);
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x0007C3C8 File Offset: 0x0007A7C8
	private static void roundFrame(Animator animator, int layer, Func<float, float> rounder)
	{
		AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(layer);
		if (currentAnimatorClipInfo.Length == 0)
		{
			return;
		}
		AnimationClip clip = currentAnimatorClipInfo[0].clip;
		float frameRate = clip.frameRate;
		float length = clip.length;
		float normalizedTime = animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
		float arg = normalizedTime * length * frameRate;
		float normalizedTime2 = rounder(arg) / frameRate / length;
		animator.Play(0, layer, normalizedTime2);
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0007C433 File Offset: 0x0007A833
	public static Coroutine WaitForAnimationToStart(this Animator animator, MonoBehaviour parent, string animationName, bool waitForEndOfFrame = false)
	{
		return animator.WaitForAnimationToStart(parent, animationName, 0, waitForEndOfFrame);
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0007C440 File Offset: 0x0007A840
	public static Coroutine WaitForAnimationToStart(this Animator animator, MonoBehaviour parent, string animationName, int layer, bool waitForEndOfFrame = false)
	{
		int animationHash = Animator.StringToHash(animator.GetLayerName(layer) + "." + animationName);
		return animator.WaitForAnimationToStart(parent, animationHash, layer, waitForEndOfFrame);
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0007C470 File Offset: 0x0007A870
	public static Coroutine WaitForAnimationToStart(this Animator animator, MonoBehaviour parent, int animationHash, int layer, bool waitForEndOfFrame = false)
	{
		return parent.StartCoroutine(AnimatorExtensions.waitForAnimStart_cr(animator, layer, animationHash, waitForEndOfFrame));
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0007C484 File Offset: 0x0007A884
	private static IEnumerator waitForAnimStart_cr(Animator animator, int layer, int animationHash, bool waitForEndOfFrame)
	{
		while (animator.GetCurrentAnimatorStateInfo(layer).fullPathHash != animationHash)
		{
			if (waitForEndOfFrame)
			{
				yield return new WaitForEndOfFrame();
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x0007C4B4 File Offset: 0x0007A8B4
	public static Coroutine WaitForAnimationToEnd(this Animator animator, MonoBehaviour parent, bool waitForEndOfFrame = false)
	{
		return parent.StartCoroutine(AnimatorExtensions.waitForAnimEnd_cr(parent, animator, 0, waitForEndOfFrame));
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x0007C4C8 File Offset: 0x0007A8C8
	private static IEnumerator waitForAnimEnd_cr(MonoBehaviour parent, Animator animator, int layer, bool waitForEndOfFrame)
	{
		int current = animator.GetCurrentAnimatorStateInfo(layer).fullPathHash;
		while (current == animator.GetCurrentAnimatorStateInfo(layer).fullPathHash)
		{
			if (waitForEndOfFrame)
			{
				yield return new WaitForEndOfFrame();
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x0007C4F1 File Offset: 0x0007A8F1
	public static Coroutine WaitForAnimationToEnd(this Animator animator, MonoBehaviour parent, string name, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		return animator.WaitForAnimationToEnd(parent, name, 0, waitForEndOfFrame, waitForStart);
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x0007C500 File Offset: 0x0007A900
	public static Coroutine WaitForAnimationToEnd(this Animator animator, MonoBehaviour parent, string name, int layer, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		int animationHash = Animator.StringToHash(animator.GetLayerName(layer) + "." + name);
		return animator.WaitForAnimationToEnd(parent, animationHash, layer, waitForEndOfFrame, waitForStart);
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x0007C532 File Offset: 0x0007A932
	public static Coroutine WaitForAnimationToEnd(this Animator animator, MonoBehaviour parent, int animationHash, int layer = 0, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		return parent.StartCoroutine(AnimatorExtensions.waitForNamedAnimEnd_cr(parent, animator, animationHash, layer, waitForEndOfFrame, waitForStart));
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x0007C548 File Offset: 0x0007A948
	private static IEnumerator waitForNamedAnimEnd_cr(MonoBehaviour parent, Animator animator, int animationHash, int layer, bool waitForEndOfFrame, bool waitForStart = true)
	{
		if (waitForStart)
		{
			yield return parent.StartCoroutine(AnimatorExtensions.waitForAnimStart_cr(animator, layer, animationHash, waitForEndOfFrame));
		}
		while (animator.GetCurrentAnimatorStateInfo(layer).fullPathHash == animationHash)
		{
			if (waitForEndOfFrame)
			{
				yield return new WaitForEndOfFrame();
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x0007C588 File Offset: 0x0007A988
	public static Coroutine WaitForNormalizedTime(this Animator animator, MonoBehaviour parent, float normalizedTime, string name = null, int layer = 0, bool allowEqualTime = false, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		int? animationHash = null;
		if (name != null)
		{
			animationHash = new int?(Animator.StringToHash(animator.GetLayerName(layer) + "." + name));
		}
		return animator.WaitForNormalizedTime(parent, normalizedTime, animationHash, layer, allowEqualTime, waitForEndOfFrame, waitForStart);
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x0007C5D4 File Offset: 0x0007A9D4
	public static Coroutine WaitForNormalizedTime(this Animator animator, MonoBehaviour parent, float normalizedTime, int? animationHash, int layer = 0, bool allowEqualTime = false, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		return parent.StartCoroutine(AnimatorExtensions.waitForNormalizedTime_cr(parent, animator, normalizedTime, animationHash, layer, allowEqualTime, waitForEndOfFrame, waitForStart, false));
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x0007C5FC File Offset: 0x0007A9FC
	public static Coroutine WaitForNormalizedTimeLooping(this Animator animator, MonoBehaviour parent, float normalizedTimeDecimal, string name = null, int layer = 0, bool allowEqualTime = false, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		int? animationHash = null;
		if (name != null)
		{
			animationHash = new int?(Animator.StringToHash(animator.GetLayerName(layer) + "." + name));
		}
		return parent.StartCoroutine(AnimatorExtensions.waitForNormalizedTime_cr(parent, animator, normalizedTimeDecimal, animationHash, layer, allowEqualTime, waitForEndOfFrame, waitForStart, true));
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0007C650 File Offset: 0x0007AA50
	private static IEnumerator waitForNormalizedTime_cr(MonoBehaviour parent, Animator animator, float normalizedTime, int? animationHash, int layer, bool allowEqualTime, bool waitForEndOfFrame, bool waitForStart, bool looping)
	{
		if (animationHash != null && waitForStart)
		{
			yield return parent.StartCoroutine(AnimatorExtensions.waitForAnimStart_cr(animator, layer, animationHash.Value, waitForEndOfFrame));
		}
		int target;
		if (animationHash != null)
		{
			target = animationHash.Value;
		}
		else
		{
			target = animator.GetCurrentAnimatorStateInfo(layer).fullPathHash;
		}
		for (;;)
		{
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
			float num = (!looping) ? stateInfo.normalizedTime : MathUtilities.DecimalPart(stateInfo.normalizedTime);
			if (((!allowEqualTime) ? (stateInfo.normalizedTime >= normalizedTime) : (stateInfo.normalizedTime > normalizedTime)) || stateInfo.fullPathHash != target)
			{
				break;
			}
			if (waitForEndOfFrame)
			{
				yield return new WaitForEndOfFrame();
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04001447 RID: 5191
	[CompilerGenerated]
	private static Func<float, float> <>f__mg$cache0;

	// Token: 0x04001448 RID: 5192
	[CompilerGenerated]
	private static Func<float, float> <>f__mg$cache1;
}
