using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000824 RID: 2084
public class TrainLevelPlatform : LevelPlatform
{
	// Token: 0x06003063 RID: 12387 RVA: 0x001C824C File Offset: 0x001C664C
	protected override void Awake()
	{
		base.Awake();
		this.animHelper = base.GetComponent<AnimationHelper>();
		this.middlePos = base.transform.position.x + 390f;
		this.leftPos = base.transform.position.x;
		this.rightPos = base.transform.position.x + 780f;
		this.position = TrainLevelPlatform.CartPosition.Left;
		this.rightSwitch.OnActivate += this.OnRight;
		this.leftSwitch.OnActivate += this.OnLeft;
		base.StartCoroutine(this.spark_cr());
	}

	// Token: 0x06003064 RID: 12388 RVA: 0x001C8304 File Offset: 0x001C6704
	private void OnLeft()
	{
		if (this.isMoving)
		{
			return;
		}
		AudioManager.Play("train_hand_car_valves_spin");
		this.emitAudioFromObject.Add("train_hand_car_valves_spin");
		this.position = ((this.position != TrainLevelPlatform.CartPosition.Right) ? TrainLevelPlatform.CartPosition.Left : TrainLevelPlatform.CartPosition.Middle);
		this.Move(this.SelectPosition());
	}

	// Token: 0x06003065 RID: 12389 RVA: 0x001C835C File Offset: 0x001C675C
	private void OnRight()
	{
		if (this.isMoving)
		{
			return;
		}
		AudioManager.Play("train_hand_car_valves_spin");
		this.emitAudioFromObject.Add("train_hand_car_valves_spin");
		this.position = ((this.position != TrainLevelPlatform.CartPosition.Left) ? TrainLevelPlatform.CartPosition.Right : TrainLevelPlatform.CartPosition.Middle);
		this.Move(this.SelectPosition());
	}

	// Token: 0x06003066 RID: 12390 RVA: 0x001C83B4 File Offset: 0x001C67B4
	private float SelectPosition()
	{
		float result = 0f;
		TrainLevelPlatform.CartPosition cartPosition = this.position;
		if (cartPosition != TrainLevelPlatform.CartPosition.Left)
		{
			if (cartPosition != TrainLevelPlatform.CartPosition.Right)
			{
				if (cartPosition == TrainLevelPlatform.CartPosition.Middle)
				{
					result = this.middlePos;
				}
			}
			else
			{
				result = this.rightPos;
			}
		}
		else
		{
			result = this.leftPos;
		}
		return result;
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x001C840C File Offset: 0x001C680C
	private void Move(float x)
	{
		base.StartCoroutine(this.move_cr(x));
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x001C841C File Offset: 0x001C681C
	private IEnumerator move_cr(float x)
	{
		this.isMoving = true;
		this.rightSwitch.gameObject.SetActive(false);
		this.leftSwitch.gameObject.SetActive(false);
		base.animator.SetTrigger("OnSlap");
		base.animator.SetBool("Spinning", true);
		base.animator.SetBool("Effect", false);
		this.animHelper.Speed = 1f;
		float t = 0f;
		float time = 1.5f;
		float startX = base.transform.position.x;
		base.transform.SetPosition(new float?(startX), null, null);
		yield return null;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = t / time;
			base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutCubic, startX, x, val)), null, null);
			if (val > 0.5f)
			{
				this.animHelper.Speed = 0.5f;
			}
			yield return null;
		}
		base.transform.SetPosition(new float?(x), null, null);
		this.isMoving = false;
		yield break;
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x001C8440 File Offset: 0x001C6840
	private void FadeIn()
	{
		this.rightSwitch.gameObject.SetActive(true);
		this.leftSwitch.gameObject.SetActive(true);
		this.animHelper.Speed = 1f;
		base.animator.SetTrigger("OnContinue");
		base.animator.SetBool("Effect", true);
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x001C84A0 File Offset: 0x001C68A0
	private IEnumerator spark_cr()
	{
		for (;;)
		{
			while (!this.leftSwitch.isActiveAndEnabled)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.5f, 1f));
			this.sparkEffectPrefab.Create(this.sparkRoots.RandomChoice<Transform>().position);
			yield return CupheadTime.WaitForSeconds(this, 0.33333334f);
		}
		yield break;
	}

	// Token: 0x0400390D RID: 14605
	private const float DISTANCE = 390f;

	// Token: 0x0400390E RID: 14606
	[SerializeField]
	private ParrySwitch rightSwitch;

	// Token: 0x0400390F RID: 14607
	[SerializeField]
	private ParrySwitch leftSwitch;

	// Token: 0x04003910 RID: 14608
	[SerializeField]
	private Transform[] sparkRoots;

	// Token: 0x04003911 RID: 14609
	[SerializeField]
	private Effect sparkEffectPrefab;

	// Token: 0x04003912 RID: 14610
	private TrainLevelPlatform.CartPosition position;

	// Token: 0x04003913 RID: 14611
	private AnimationHelper animHelper;

	// Token: 0x04003914 RID: 14612
	private SpriteRenderer spriteRenderer;

	// Token: 0x04003915 RID: 14613
	private float middlePos;

	// Token: 0x04003916 RID: 14614
	private float leftPos;

	// Token: 0x04003917 RID: 14615
	private float rightPos;

	// Token: 0x04003918 RID: 14616
	private bool isMoving;

	// Token: 0x02000825 RID: 2085
	public enum CartPosition
	{
		// Token: 0x0400391A RID: 14618
		Left,
		// Token: 0x0400391B RID: 14619
		Middle,
		// Token: 0x0400391C RID: 14620
		Right
	}
}
