using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000680 RID: 1664
public class FlyingMermaidLevelBackgroundChange : AbstractPausableComponent
{
	// Token: 0x06002323 RID: 8995 RVA: 0x0014A07C File Offset: 0x0014847C
	private void Start()
	{
		this.points = new List<Transform>();
		this.size = this.toCopy.GetComponent<Collider2D>().bounds.size.x;
		this.getOffset.x = base.transform.position.x;
		this.copy1 = UnityEngine.Object.Instantiate<FlyingMermaidLevelCoralCluster>(this.toCopy);
		this.copy1.transform.parent = base.transform;
		FlyingMermaidLevelCoralCluster flyingMermaidLevelCoralCluster = UnityEngine.Object.Instantiate<FlyingMermaidLevelCoralCluster>(this.toCopy);
		flyingMermaidLevelCoralCluster.transform.parent = base.transform;
		this.copy1.transform.SetPosition(new float?(this.getOffset.x + this.size), new float?(this.toCopy.transform.position.y), new float?(0f));
		flyingMermaidLevelCoralCluster.transform.SetPosition(new float?(this.getOffset.x + this.size * 2f), new float?(this.toCopy.transform.position.y), new float?(0f));
		this.points.AddRange(this.toCopy.points);
		this.points.AddRange(this.copy1.points);
		this.points.AddRange(flyingMermaidLevelCoralCluster.points);
		this.copies = new List<FlyingMermaidLevelCoralCluster>();
		this.copies.Add(this.toCopy);
		this.copies.Add(this.copy1);
		this.copies.Add(flyingMermaidLevelCoralCluster);
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x0014A234 File Offset: 0x00148634
	private void FixedUpdate()
	{
		if (base.GetComponent<ParallaxLayer>() != null)
		{
			base.GetComponent<ParallaxLayer>().enabled = false;
		}
		Vector3 localPosition = base.transform.localPosition;
		if (this.copies[this.index].transform.position.x <= -this.size)
		{
			this.copies[this.index].transform.position = new Vector2(this.size * 2f, this.copies[this.index].transform.position.y);
			this.index = (this.index + 1) % this.copies.Count;
		}
		localPosition.x -= this.speed * CupheadTime.FixedDelta * this.b_playbackSpeed;
		base.transform.localPosition = localPosition;
	}

	// Token: 0x04002BC0 RID: 11200
	public List<Transform> points;

	// Token: 0x04002BC1 RID: 11201
	private float size;

	// Token: 0x04002BC2 RID: 11202
	private const float X_OUT = -1280f;

	// Token: 0x04002BC3 RID: 11203
	[Range(0f, 2000f)]
	public float speed;

	// Token: 0x04002BC4 RID: 11204
	[NonSerialized]
	public float b_playbackSpeed = 1f;

	// Token: 0x04002BC5 RID: 11205
	[SerializeField]
	private FlyingMermaidLevelCoralCluster toCopy;

	// Token: 0x04002BC6 RID: 11206
	private FlyingMermaidLevelCoralCluster copy1;

	// Token: 0x04002BC7 RID: 11207
	private List<FlyingMermaidLevelCoralCluster> copies;

	// Token: 0x04002BC8 RID: 11208
	private Vector3 getOffset;

	// Token: 0x04002BC9 RID: 11209
	private Vector3 _offset;

	// Token: 0x04002BCA RID: 11210
	private int index;
}
