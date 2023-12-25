using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004E6 RID: 1254
public class BaronessLevelPlatform : PlatformingLevelPlatformSag
{
	// Token: 0x060015BC RID: 5564 RVA: 0x000C3058 File Offset: 0x000C1458
	public void getProperties(LevelProperties.Baroness.Platform properties)
	{
		this.properties = properties;
		Vector3 position = base.transform.position;
		position.y = properties.YPosition;
		base.transform.position = position;
		this.maxCounter = UnityEngine.Random.Range(4, 9);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x000C30AC File Offset: 0x000C14AC
	private IEnumerator move_cr()
	{
		bool movingLeft = true;
		for (;;)
		{
			Vector3 pos = base.transform.position;
			if (movingLeft)
			{
				if (this.castle.state == BaronessLevelCastle.State.Chase)
				{
					base.animator.Play("Fast");
				}
				pos.x = Mathf.MoveTowards(base.transform.position.x, -640f + this.properties.LeftBoundaryOffset, this.speed * CupheadTime.Delta);
				movingLeft = (base.transform.position.x != -640f + this.properties.LeftBoundaryOffset);
			}
			else
			{
				if (this.castle.state == BaronessLevelCastle.State.Chase)
				{
					base.animator.Play("Slow");
				}
				pos.x = Mathf.MoveTowards(base.transform.position.x, (float)Level.Current.Right - this.properties.RightBoundaryOffset, this.speed * CupheadTime.Delta);
				movingLeft = (base.transform.position.x == (float)Level.Current.Right - this.properties.RightBoundaryOffset);
			}
			base.transform.position = pos;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060015BE RID: 5566 RVA: 0x000C30C8 File Offset: 0x000C14C8
	private void SweatCounter()
	{
		if (this.counter < this.maxCounter)
		{
			this.counter++;
		}
		else
		{
			base.animator.Play("Sweat");
			this.counter = 0;
			this.maxCounter = UnityEngine.Random.Range(4, 9);
		}
	}

	// Token: 0x04001F10 RID: 7952
	[SerializeField]
	private BaronessLevelCastle castle;

	// Token: 0x04001F11 RID: 7953
	private float speed = 200f;

	// Token: 0x04001F12 RID: 7954
	private int counter;

	// Token: 0x04001F13 RID: 7955
	private int maxCounter;

	// Token: 0x04001F14 RID: 7956
	private LevelProperties.Baroness.Platform properties;
}
