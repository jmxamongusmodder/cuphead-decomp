using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D1 RID: 2257
public class HarbourPlatformingLevelStarfish : AbstractPlatformingLevelEnemy
{
	// Token: 0x060034D4 RID: 13524 RVA: 0x001EB780 File Offset: 0x001E9B80
	public void Init(float rotation, float speedX, float speedY, float loopSize, string type)
	{
		base.transform.SetEulerAngles(null, null, new float?(rotation - 90f));
		this.figureEightSpeed = speedX;
		this.movementSpeed = speedY;
		this.loopSize = loopSize;
		this.type = type;
	}

	// Token: 0x060034D5 RID: 13525 RVA: 0x001EB7D4 File Offset: 0x001E9BD4
	protected override void OnStart()
	{
	}

	// Token: 0x060034D6 RID: 13526 RVA: 0x001EB7D8 File Offset: 0x001E9BD8
	protected override void Start()
	{
		base.Start();
		this.pivotOffset = Vector3.up * 2f * this.loopSize;
		this.pivotPoint = new GameObject("PivotPoint");
		this.pivotPoint.transform.position = base.transform.position;
		if (this.type == "A")
		{
			this._canParry = true;
			this.bubbles = this.pinkBubbles;
		}
		else
		{
			this.bubbles = this.normalBubbles;
		}
		base.GetComponent<PlatformingLevelEnemyAnimationHandler>().SelectAnimation(this.type);
		base.StartCoroutine(this.spawn_bubbles_cr());
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.figure_eight_cr());
	}

	// Token: 0x060034D7 RID: 13527 RVA: 0x001EB8A8 File Offset: 0x001E9CA8
	private IEnumerator move_cr()
	{
		for (;;)
		{
			this.pivotPoint.transform.position += base.transform.up * this.movementSpeed * CupheadTime.FixedDelta;
			if (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax + 200f)
			{
				break;
			}
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x060034D8 RID: 13528 RVA: 0x001EB8C4 File Offset: 0x001E9CC4
	private IEnumerator figure_eight_cr()
	{
		bool invert = false;
		for (;;)
		{
			this.angle += this.figureEightSpeed * CupheadTime.Delta;
			if (this.angle > 6.2831855f)
			{
				invert = !invert;
				this.angle -= 6.2831855f;
			}
			if (this.angle < 0f)
			{
				this.angle += 6.2831855f;
			}
			float value;
			if (invert)
			{
				base.transform.position = this.pivotPoint.transform.position + this.pivotOffset;
				value = -1f;
			}
			else
			{
				base.transform.position = this.pivotPoint.transform.position;
				value = 1f;
			}
			Vector3 handleRotation = new Vector3(-Mathf.Sin(this.angle) * this.loopSize, Mathf.Cos(this.angle) * value * this.loopSize, 0f);
			base.transform.position += handleRotation;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x001EB8E0 File Offset: 0x001E9CE0
	private IEnumerator spawn_bubbles_cr()
	{
		string bubbleTypes = "A,B,C,D";
		for (;;)
		{
			string bubbleType = bubbleTypes.Split(new char[]
			{
				','
			})[UnityEngine.Random.Range(0, bubbleTypes.Split(new char[]
			{
				','
			}).Length)];
			this.bubbles.Create(base.transform.position).GetComponent<PlatformingLevelEnemyAnimationHandler>().SelectAnimation(bubbleType);
			yield return CupheadTime.WaitForSeconds(this, 1f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x001EB8FB File Offset: 0x001E9CFB
	protected override void Die()
	{
		AudioManager.Play("harbour_star_death");
		this.emitAudioFromObject.Add("harbour_star_death");
		base.Die();
	}

	// Token: 0x060034DB RID: 13531 RVA: 0x001EB91D File Offset: 0x001E9D1D
	protected override void OnDestroy()
	{
		base.OnDestroy();
		UnityEngine.Object.Destroy(this.pivotPoint.gameObject);
	}

	// Token: 0x04003CFC RID: 15612
	[SerializeField]
	private Effect normalBubbles;

	// Token: 0x04003CFD RID: 15613
	[SerializeField]
	private Effect pinkBubbles;

	// Token: 0x04003CFE RID: 15614
	private GameObject pivotPoint;

	// Token: 0x04003CFF RID: 15615
	private float angle;

	// Token: 0x04003D00 RID: 15616
	private float figureEightSpeed;

	// Token: 0x04003D01 RID: 15617
	private float movementSpeed;

	// Token: 0x04003D02 RID: 15618
	private float loopSize;

	// Token: 0x04003D03 RID: 15619
	private string type;

	// Token: 0x04003D04 RID: 15620
	private Vector3 pivotOffset;

	// Token: 0x04003D05 RID: 15621
	private Effect bubbles;
}
