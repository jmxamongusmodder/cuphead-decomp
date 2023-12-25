using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056D RID: 1389
public class ClownLevelSwings : AbstractCollidableObject
{
	// Token: 0x06001A40 RID: 6720 RVA: 0x000F0275 File Offset: 0x000EE675
	public void Init(Vector3 pos, LevelProperties.Clown.Swing properties, float spacing, float enterAngle)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.spacing = spacing;
		this.enterAngle = enterAngle;
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x000F029C File Offset: 0x000EE69C
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
		this.defaultColor = base.GetComponent<SpriteRenderer>().color;
		if (this.isBackSeat)
		{
			this.rod.transform.SetEulerAngles(null, null, new float?(this.startAngleBack - this.enterAngle));
		}
		else
		{
			this.rod.transform.SetEulerAngles(null, null, new float?(this.startAngleFront + this.enterAngle));
		}
		base.StartCoroutine(this.rotation_cr());
		base.StartCoroutine(this.move_swing_y_cr());
		base.StartCoroutine(this.move_swing_x_cr());
		if (this.properties.swingDropOn)
		{
			base.StartCoroutine(this.player_check_cr());
		}
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x000F0384 File Offset: 0x000EE784
	private IEnumerator move_swing_y_cr()
	{
		bool starting = true;
		float distanceLeft = 0f;
		float distanceRight = 0f;
		for (;;)
		{
			Vector3 pos = base.transform.position;
			float speed = (!starting) ? 50f : 150f;
			if (this.isBackSeat)
			{
				distanceLeft = -this.distAmount;
				distanceRight = this.distAmount + this.distAmount / 2f;
			}
			else
			{
				distanceLeft = -this.distAmount - this.distAmount / 2f;
				distanceRight = this.distAmount;
			}
			if (base.transform.position.x > distanceRight || base.transform.position.x < distanceLeft)
			{
				if (base.transform.position.y != this.highPoint)
				{
					pos.y = Mathf.MoveTowards(base.transform.position.y, this.highPoint, speed * CupheadTime.Delta);
					base.transform.position = pos;
				}
				else
				{
					starting = false;
				}
			}
			else if (base.transform.position.y != this.lowestPoint)
			{
				pos.y = Mathf.MoveTowards(base.transform.position.y, this.lowestPoint, speed * CupheadTime.Delta);
				base.transform.position = pos;
			}
			else
			{
				starting = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x000F03A0 File Offset: 0x000EE7A0
	private IEnumerator move_swing_x_cr()
	{
		ClownLevelSwings.moveSpeed = this.properties.swingSpeed;
		float size = base.transform.GetComponent<Renderer>().bounds.size.x;
		float sizeDivided = size / 4f;
		for (;;)
		{
			if (this.isBackSeat)
			{
				float end = 640f - this.spacing * 5f;
				while (base.transform.position.x > end)
				{
					base.transform.position -= base.transform.right * ClownLevelSwings.moveSpeed * CupheadTime.Delta;
					yield return null;
				}
				base.transform.position = new Vector3(640f + this.spacing, this.highPoint, 0f);
			}
			else
			{
				float end2 = -640f + (this.spacing * 5f + size);
				base.transform.GetComponent<Collider2D>().enabled = true;
				while (base.transform.position.x < end2)
				{
					if (base.transform.position.x > 640f + sizeDivided)
					{
						base.transform.GetComponent<Collider2D>().enabled = false;
					}
					base.transform.position += base.transform.right * ClownLevelSwings.moveSpeed * CupheadTime.Delta;
					yield return null;
				}
				this.resetWarning = true;
				this.SwingReappear();
				base.transform.position = new Vector3(-640f - (this.spacing - size), this.highPoint, 0f);
			}
			base.StopCoroutine(this.rotation_cr());
			this.enterAngle = 0f;
			if (this.isBackSeat)
			{
				this.rod.transform.SetEulerAngles(null, null, new float?(this.startAngleBack));
			}
			else
			{
				this.rod.transform.SetEulerAngles(null, null, new float?(this.startAngleFront));
			}
			base.StartCoroutine(this.rotation_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x000F03BC File Offset: 0x000EE7BC
	private IEnumerator rotation_cr()
	{
		float t = 0f;
		if (this.isBackSeat)
		{
			while (this.rod.transform.eulerAngles.z > this.endAngleBack)
			{
				if (CupheadTime.Delta != 0f)
				{
					this.rod.transform.SetEulerAngles(null, null, new float?(this.rod.transform.eulerAngles.z - t));
					t += 0.001f;
				}
				yield return null;
			}
		}
		else
		{
			while (this.rod.transform.eulerAngles.z < this.endAngleFront)
			{
				if (CupheadTime.Delta != 0f)
				{
					this.rod.transform.SetEulerAngles(null, null, new float?(this.rod.transform.eulerAngles.z + t));
					t += 0.001f;
				}
				yield return null;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x000F03D8 File Offset: 0x000EE7D8
	private IEnumerator player_check_cr()
	{
		AbstractPlayerController player = PlayerManager.GetNext();
		for (;;)
		{
			while (player == null || player.transform.parent != base.transform)
			{
				player = PlayerManager.GetNext();
				yield return null;
			}
			if (!this.resetWarning)
			{
				this.sprite.color = Color.red;
				yield return CupheadTime.WaitForSeconds(this, this.properties.swingDropWarningDuration);
				yield return null;
				this.sprite.color = Color.black;
				base.transform.GetComponent<Collider2D>().enabled = false;
				yield return CupheadTime.WaitForSeconds(this, this.properties.swingfullDropDuration);
			}
			this.SwingReappear();
			this.resetWarning = false;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x000F03F3 File Offset: 0x000EE7F3
	private void SwingReappear()
	{
		base.transform.GetComponent<Collider2D>().enabled = true;
		this.sprite.color = this.defaultColor;
	}

	// Token: 0x0400235B RID: 9051
	public static float moveSpeed;

	// Token: 0x0400235C RID: 9052
	private const float FALL_GRAVITY = -100f;

	// Token: 0x0400235D RID: 9053
	public bool isBackSeat;

	// Token: 0x0400235E RID: 9054
	[SerializeField]
	private Transform rod;

	// Token: 0x0400235F RID: 9055
	private LevelProperties.Clown.Swing properties;

	// Token: 0x04002360 RID: 9056
	private SpriteRenderer sprite;

	// Token: 0x04002361 RID: 9057
	private Color defaultColor;

	// Token: 0x04002362 RID: 9058
	private bool resetWarning;

	// Token: 0x04002363 RID: 9059
	private float spacing;

	// Token: 0x04002364 RID: 9060
	private float lowestPoint;

	// Token: 0x04002365 RID: 9061
	private float highPoint = 100f;

	// Token: 0x04002366 RID: 9062
	private float distAmount = 450f;

	// Token: 0x04002367 RID: 9063
	private float startAngleFront = 320f;

	// Token: 0x04002368 RID: 9064
	private float startAngleBack = 40f;

	// Token: 0x04002369 RID: 9065
	private float endAngleFront = 350f;

	// Token: 0x0400236A RID: 9066
	private float endAngleBack = 10f;

	// Token: 0x0400236B RID: 9067
	private float enterAngle;
}
