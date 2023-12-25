using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B19 RID: 2841
public class ScrollingBackgroundElevator : AbstractPausableComponent
{
	// Token: 0x060044D0 RID: 17616 RVA: 0x00246E00 File Offset: 0x00245200
	public void SetUp(Vector3 direction, float speed)
	{
		if (this.isBackground)
		{
			this.startPos = this.firstSprite.transform.position;
		}
		else
		{
			this.startPos = base.transform.position + direction.normalized * -800f;
		}
		this.endPos = this.lastSprite.transform.position;
		this.direction = direction;
		this.speed = speed;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060044D1 RID: 17617 RVA: 0x00246E8C File Offset: 0x0024528C
	private IEnumerator move_cr()
	{
		while (!this.ending)
		{
			if (this.isBackground)
			{
				while (base.transform.position != this.endPos && !this.ending)
				{
					base.transform.position = Vector3.MoveTowards(base.transform.position, this.endPos, this.speed * CupheadTime.Delta);
					yield return null;
				}
			}
			else
			{
				while ((base.transform.position.y < this.endPos.y && base.transform.position.x > this.endPos.x && !this.ending) || (this.isClouds && this.ending))
				{
					base.transform.position -= this.direction * this.speed * CupheadTime.Delta;
					yield return null;
				}
			}
			if (!this.easingOut)
			{
				base.transform.position = this.startPos;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060044D2 RID: 17618 RVA: 0x00246EA7 File Offset: 0x002452A7
	public void EaseoutSpeed(float time)
	{
		base.StartCoroutine(this.ease_speed_cr(time));
		this.easingOut = true;
	}

	// Token: 0x060044D3 RID: 17619 RVA: 0x00246EC0 File Offset: 0x002452C0
	private IEnumerator ease_speed_cr(float time)
	{
		float startSpeed = this.speed;
		float t = 0f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.speed = Mathf.Lerp(startSpeed, 0f, t / time);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060044D4 RID: 17620 RVA: 0x00246EE2 File Offset: 0x002452E2
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawWireSphere(base.transform.position, 100f);
	}

	// Token: 0x04004A90 RID: 19088
	[SerializeField]
	private bool isClouds;

	// Token: 0x04004A91 RID: 19089
	[SerializeField]
	private bool isBackground;

	// Token: 0x04004A92 RID: 19090
	[SerializeField]
	private SpriteRenderer firstSprite;

	// Token: 0x04004A93 RID: 19091
	[SerializeField]
	private SpriteRenderer lastSprite;

	// Token: 0x04004A94 RID: 19092
	private float speed;

	// Token: 0x04004A95 RID: 19093
	private Vector3 startPos;

	// Token: 0x04004A96 RID: 19094
	private Vector3 endPos;

	// Token: 0x04004A97 RID: 19095
	private Vector3 direction;

	// Token: 0x04004A98 RID: 19096
	public bool ending;

	// Token: 0x04004A99 RID: 19097
	public bool easingOut;
}
