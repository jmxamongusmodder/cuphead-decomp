using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200048F RID: 1167
public class LevelHUDPlayerSuperCard : AbstractMonoBehaviour
{
	// Token: 0x06001256 RID: 4694 RVA: 0x000A9D71 File Offset: 0x000A8171
	private void Start()
	{
		this.end = base.transform.localPosition;
		this.start = this.end + new Vector3(0f, -30f, 0f);
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x000A9DA9 File Offset: 0x000A81A9
	private void Update()
	{
		this.UpdatePosition();
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x000A9DB4 File Offset: 0x000A81B4
	private void UpdatePosition()
	{
		if (!this.initialized)
		{
			return;
		}
		this.target = Vector3.Lerp(this.start, this.end, this.current / this.max);
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this.target, CupheadTime.Delta * 10f);
		this.image.fillAmount = Mathf.Lerp(this.image.fillAmount, this.current / this.max, CupheadTime.Delta * 10f);
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x000A9E5C File Offset: 0x000A825C
	public void Init(PlayerId playerId, float exCost)
	{
		this.max = exCost;
		if (playerId != PlayerId.PlayerOne)
		{
			if (playerId != PlayerId.PlayerTwo)
			{
				if (playerId != PlayerId.Any && playerId != PlayerId.None)
				{
				}
			}
			else
			{
				base.animator.SetInteger("Player", (!PlayerManager.player1IsMugman) ? 1 : 0);
			}
		}
		else
		{
			base.animator.SetInteger("Player", (!PlayerManager.player1IsMugman) ? 0 : 1);
		}
		this.initialized = true;
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x000A9EF0 File Offset: 0x000A82F0
	public void SetAmount(float amount)
	{
		this.current = Mathf.Clamp(amount, 0f, this.max);
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x000A9F09 File Offset: 0x000A8309
	public void SetSuper(bool super)
	{
		base.animator.SetBool("Super", super);
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x000A9F1C File Offset: 0x000A831C
	public void SetEx(bool ex)
	{
		base.animator.SetBool("Ex", ex);
	}

	// Token: 0x04001BC3 RID: 7107
	private const float SPEED = 10f;

	// Token: 0x04001BC4 RID: 7108
	private const float Y_DIFF = -30f;

	// Token: 0x04001BC5 RID: 7109
	[SerializeField]
	private Image image;

	// Token: 0x04001BC6 RID: 7110
	private bool initialized;

	// Token: 0x04001BC7 RID: 7111
	private float current;

	// Token: 0x04001BC8 RID: 7112
	private float max;

	// Token: 0x04001BC9 RID: 7113
	private Vector3 start;

	// Token: 0x04001BCA RID: 7114
	private Vector3 end;

	// Token: 0x04001BCB RID: 7115
	private Vector3 target;
}
