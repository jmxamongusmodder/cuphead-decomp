using System;
using UnityEngine;

// Token: 0x020005DD RID: 1501
public class DicePalaceRabbitLevelForegroundFog : AbstractPausableComponent
{
	// Token: 0x06001DA7 RID: 7591 RVA: 0x00110B5C File Offset: 0x0010EF5C
	private void Update()
	{
		this.angle += this.speed * CupheadTime.Delta;
		Vector3 a = new Vector3(-Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		Vector3 b = new Vector3(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		base.transform.position = this.pivotPoint.position;
		base.transform.position += a + b;
		if (this.fadingOut)
		{
			if (this.time < this.fadeTime)
			{
				if (base.GetComponent<SpriteRenderer>().color.a > 0.5f)
				{
					base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - this.time / this.fadeTime);
				}
				this.time += CupheadTime.Delta;
			}
			else
			{
				this.fadingOut = !this.fadingOut;
				this.time = 0f;
			}
		}
		else if (this.time < this.fadeTime)
		{
			base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f + this.time / this.fadeTime);
			this.time += CupheadTime.Delta;
		}
		else
		{
			this.fadingOut = !this.fadingOut;
			this.time = 0f;
		}
	}

	// Token: 0x04002685 RID: 9861
	[SerializeField]
	private Transform pivotPoint;

	// Token: 0x04002686 RID: 9862
	private float loopSize = 5f;

	// Token: 0x04002687 RID: 9863
	private float speed = 2f;

	// Token: 0x04002688 RID: 9864
	private float angle;

	// Token: 0x04002689 RID: 9865
	private float time;

	// Token: 0x0400268A RID: 9866
	private float fadeTime = 5f;

	// Token: 0x0400268B RID: 9867
	private bool fadingOut = true;
}
