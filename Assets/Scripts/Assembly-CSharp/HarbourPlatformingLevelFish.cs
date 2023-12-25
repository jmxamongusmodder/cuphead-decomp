using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008C7 RID: 2247
public class HarbourPlatformingLevelFish : AbstractPlatformingLevelEnemy
{
	// Token: 0x06003485 RID: 13445 RVA: 0x001E7F1D File Offset: 0x001E631D
	public void Init(Vector2 pos, float rotation, string type)
	{
		base.transform.position = pos;
		this.type = type;
		this.rotation = rotation;
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x001E7F3E File Offset: 0x001E633E
	protected override void OnStart()
	{
	}

	// Token: 0x06003487 RID: 13447 RVA: 0x001E7F40 File Offset: 0x001E6340
	protected override void Start()
	{
		this.blinkLayer.enabled = false;
		this.blinkCounterMax = UnityEngine.Random.Range(10, 20);
		base.transform.SetScale(new float?((this.rotation != 180f) ? (-base.transform.localScale.x) : base.transform.localScale.x), null, null);
		for (int i = 0; i < 5; i++)
		{
			if (this.type.Substring(0, 1) == this.letters.Split(new char[]
			{
				','
			})[i])
			{
				this.num = i + 1;
			}
		}
		this.isA = (this.type.Substring(1, 1) == "A");
		base.animator.SetInteger("Type", this.num);
		base.animator.SetBool("Is1", this.isA);
		if (this.num == 4)
		{
			this._canParry = true;
		}
		base.StartCoroutine(this.movement_cr());
		base.Start();
	}

	// Token: 0x06003488 RID: 13448 RVA: 0x001E808B File Offset: 0x001E648B
	private void FlyingFishSFX()
	{
		AudioManager.Play("harbour_flying_fish_idle");
		this.emitAudioFromObject.Add("harbour_flying_fish_idle");
	}

	// Token: 0x06003489 RID: 13449 RVA: 0x001E80A8 File Offset: 0x001E64A8
	private IEnumerator movement_cr()
	{
		float angle = 0f;
		Vector3 xVelocity = Vector3.zero;
		for (;;)
		{
			angle += base.Properties.flyingFishSinVelocity * CupheadTime.Delta;
			xVelocity = ((this.rotation != 180f) ? base.transform.right : (-base.transform.right));
			Vector3 moveY = new Vector3(0f, Mathf.Sin(angle) * CupheadTime.Delta * 60f * base.Properties.flyingFishSinSize);
			Vector3 moveX = xVelocity * base.Properties.flyingFishVelocity * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				base.transform.position += moveX + moveY;
			}
			if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING))
			{
				AudioManager.Stop("harbour_flying_fish_idle");
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600348A RID: 13450 RVA: 0x001E80C4 File Offset: 0x001E64C4
	private void IncrementBlinkCounter()
	{
		this.FlyingFishSFX();
		if (this.blinkCounter < this.blinkCounterMax)
		{
			this.blinkLayer.enabled = false;
			this.blinkCounter++;
		}
		else
		{
			this.blinkLayer.enabled = true;
			this.blinkCounter = 0;
			this.blinkCounterMax = UnityEngine.Random.Range(5, 10);
		}
	}

	// Token: 0x0600348B RID: 13451 RVA: 0x001E8128 File Offset: 0x001E6528
	protected override void Die()
	{
		AudioManager.Stop("harbour_flying_fish_idle");
		AudioManager.Play("harbour_flying_fish_death");
		this.emitAudioFromObject.Add("harbour_flying_fish_death");
		base.Die();
	}

	// Token: 0x04003CA8 RID: 15528
	[SerializeField]
	private SpriteRenderer blinkLayer;

	// Token: 0x04003CA9 RID: 15529
	private string letters = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P";

	// Token: 0x04003CAA RID: 15530
	private string type;

	// Token: 0x04003CAB RID: 15531
	private float rotation;

	// Token: 0x04003CAC RID: 15532
	private int num;

	// Token: 0x04003CAD RID: 15533
	private bool isA;

	// Token: 0x04003CAE RID: 15534
	private int blinkCounter;

	// Token: 0x04003CAF RID: 15535
	private int blinkCounterMax;
}
