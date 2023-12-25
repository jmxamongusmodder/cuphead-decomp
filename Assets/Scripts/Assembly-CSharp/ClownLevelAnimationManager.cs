using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000555 RID: 1365
public class ClownLevelAnimationManager : AbstractPausableComponent
{
	// Token: 0x06001979 RID: 6521 RVA: 0x000E71D9 File Offset: 0x000E55D9
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x000E71E4 File Offset: 0x000E55E4
	private void Start()
	{
		this.headSprite = this.headSprite.GetComponent<Animator>();
		this.pivotPoint.position = this.balloonSprite.position;
		base.StartCoroutine(this.head_cr());
		base.StartCoroutine(this.balloon_loop_cr());
		foreach (Animator ani in this.twelveFpsAnimations)
		{
			base.StartCoroutine(this.manual_fps_animation_cr(ani, 0.083333336f));
		}
		foreach (Animator ani2 in this.twentyFourFpsAnimations)
		{
			base.StartCoroutine(this.manual_fps_animation_cr(ani2, 0.041666668f));
		}
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x000E72A0 File Offset: 0x000E56A0
	private IEnumerator head_cr()
	{
		for (;;)
		{
			float getSeconds = UnityEngine.Random.Range(3f, 8f);
			this.headSprite.SetTrigger("Continue");
			yield return CupheadTime.WaitForSeconds(this, getSeconds);
		}
		yield break;
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x000E72BC File Offset: 0x000E56BC
	private IEnumerator balloon_loop_cr()
	{
		float loopSize = 20f;
		float speed = 1f;
		float angle = 0f;
		for (;;)
		{
			Vector3 pivotOffset = Vector3.left * 2f * loopSize;
			angle += speed * CupheadTime.Delta;
			if (angle > 6.2831855f)
			{
				this.invert = !this.invert;
				angle -= 6.2831855f;
			}
			if (angle < 0f)
			{
				angle += 6.2831855f;
			}
			float value;
			if (this.invert)
			{
				this.balloonSprite.position = this.pivotPoint.position + pivotOffset;
				value = 1f;
			}
			else
			{
				this.balloonSprite.position = this.pivotPoint.position;
				value = -1f;
			}
			Vector3 handleRotationX = new Vector3(Mathf.Cos(angle) * value * loopSize, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Sin(angle) * loopSize, 0f);
			this.balloonSprite.position += handleRotationX + handleRotationY;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x000E72D8 File Offset: 0x000E56D8
	private IEnumerator manual_fps_animation_cr(Animator ani, float fps)
	{
		float frameTime = 0f;
		for (;;)
		{
			frameTime += CupheadTime.Delta;
			if (frameTime > fps)
			{
				frameTime -= fps;
				ani.enabled = true;
				ani.Update(fps);
				ani.enabled = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002292 RID: 8850
	[SerializeField]
	private Animator headSprite;

	// Token: 0x04002293 RID: 8851
	[SerializeField]
	private Transform balloonSprite;

	// Token: 0x04002294 RID: 8852
	[SerializeField]
	private Transform pivotPoint;

	// Token: 0x04002295 RID: 8853
	[SerializeField]
	private Animator[] twelveFpsAnimations;

	// Token: 0x04002296 RID: 8854
	[SerializeField]
	private Animator[] twentyFourFpsAnimations;

	// Token: 0x04002297 RID: 8855
	private bool invert;
}
