using System;
using UnityEngine;

// Token: 0x02000B17 RID: 2839
public class ScrollingAnimatedSprite : AbstractPausableComponent
{
	// Token: 0x060044CC RID: 17612 RVA: 0x00246ABC File Offset: 0x00244EBC
	protected override void Awake()
	{
		base.Awake();
		if (ScrollingAnimatedSprite.copying)
		{
			return;
		}
		ScrollingAnimatedSprite.copying = true;
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		this.size = ((this.axis != ScrollingAnimatedSprite.Axis.X) ? ((int)component.sprite.bounds.size.y) : ((int)component.sprite.bounds.size.x)) - this.offset;
		for (int i = 0; i < this.count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
			gameObject.GetComponent<ScrollingAnimatedSprite>().enabled = false;
			gameObject2.GetComponent<ScrollingAnimatedSprite>().enabled = false;
			gameObject.transform.SetParent(base.transform);
			gameObject2.transform.SetParent(base.transform);
			if (this.axis == ScrollingAnimatedSprite.Axis.X)
			{
				gameObject.transform.SetLocalPosition(new float?((float)(this.size + this.size * i)), new float?(0f), new float?(0f));
				gameObject2.transform.SetLocalPosition(new float?((float)(-(float)(this.size + this.size * i))), new float?(0f), new float?(0f));
			}
			else
			{
				gameObject.transform.SetLocalPosition(new float?(0f), new float?((float)(this.size + this.size * i)), new float?(0f));
				gameObject2.transform.SetLocalPosition(new float?(0f), new float?((float)(-(float)(this.size + this.size * i))), new float?(0f));
			}
		}
		ScrollingAnimatedSprite.copying = false;
	}

	// Token: 0x060044CD RID: 17613 RVA: 0x00246CA8 File Offset: 0x002450A8
	private void Update()
	{
		Vector3 localPosition = base.transform.localPosition;
		if (this.axis == ScrollingAnimatedSprite.Axis.X)
		{
			if (localPosition.x <= (float)(-(float)this.size))
			{
				localPosition.x += (float)this.size;
			}
			if (localPosition.x >= (float)this.size)
			{
				localPosition.x -= (float)this.size;
			}
			localPosition.x -= (float)((!this.negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta * this.playbackSpeed;
		}
		else
		{
			if (localPosition.y <= (float)(-(float)this.size))
			{
				localPosition.y += (float)this.size;
			}
			if (localPosition.y >= (float)this.size)
			{
				localPosition.y -= (float)this.size;
			}
			localPosition.y -= (float)((!this.negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta * this.playbackSpeed;
		}
		base.transform.localPosition = localPosition;
	}

	// Token: 0x04004A85 RID: 19077
	public ScrollingAnimatedSprite.Axis axis;

	// Token: 0x04004A86 RID: 19078
	[SerializeField]
	private bool negativeDirection;

	// Token: 0x04004A87 RID: 19079
	[SerializeField]
	[Range(0f, 2000f)]
	public float speed;

	// Token: 0x04004A88 RID: 19080
	[SerializeField]
	private int offset;

	// Token: 0x04004A89 RID: 19081
	[SerializeField]
	[Range(1f, 10f)]
	private int count = 1;

	// Token: 0x04004A8A RID: 19082
	[NonSerialized]
	public float playbackSpeed = 1f;

	// Token: 0x04004A8B RID: 19083
	private int size;

	// Token: 0x04004A8C RID: 19084
	private static bool copying;

	// Token: 0x02000B18 RID: 2840
	public enum Axis
	{
		// Token: 0x04004A8E RID: 19086
		X,
		// Token: 0x04004A8F RID: 19087
		Y
	}
}
