using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200075E RID: 1886
public class RetroArcadeToad : RetroArcadeEnemy
{
	// Token: 0x06002919 RID: 10521 RVA: 0x0017F4D0 File Offset: 0x0017D8D0
	public RetroArcadeToad Create(RetroArcadeToadManager parent, LevelProperties.RetroArcade.Toad properties, bool onLeft)
	{
		RetroArcadeToad retroArcadeToad = this.InstantiatePrefab<RetroArcadeToad>();
		retroArcadeToad.transform.SetPosition(new float?((!onLeft) ? 330f : -330f), new float?(200f), null);
		retroArcadeToad.properties = properties;
		retroArcadeToad.parent = parent;
		retroArcadeToad.hp = properties.hp;
		retroArcadeToad.onLeft = onLeft;
		return retroArcadeToad;
	}

	// Token: 0x0600291A RID: 10522 RVA: 0x0017F53E File Offset: 0x0017D93E
	protected override void Start()
	{
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x0017F550 File Offset: 0x0017D950
	private IEnumerator jump_cr()
	{
		float speedY = this.properties.jumpVerticalSpeedRange.RandomFloat();
		float speedX = this.properties.jumpHorizontalSpeedRange.RandomFloat();
		float velocityX = speedX;
		float velocityY = speedY;
		float ground = (float)Level.Current.Ground + 50f;
		bool jumping = false;
		bool goingUp = false;
		this.gravity = this.properties.jumpGravity;
		while (base.transform.position.y > ground)
		{
			velocityY -= this.gravity * CupheadTime.Delta;
			base.transform.AddPosition(0f, velocityY * CupheadTime.Delta, 0f);
			yield return null;
		}
		Vector3 pos = base.transform.position;
		pos.y = ground;
		base.transform.position = pos;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.jumpDelay.RandomFloat());
			velocityY = speedY;
			velocityX = ((!this.onLeft) ? (-speedX) : speedX);
			jumping = true;
			goingUp = true;
			while (jumping)
			{
				velocityY -= this.gravity * CupheadTime.Delta;
				base.transform.AddPosition(velocityX * CupheadTime.Delta, velocityY * CupheadTime.Delta, 0f);
				if (velocityY < 0f && goingUp)
				{
					goingUp = false;
				}
				if (velocityY < 0f && jumping && base.transform.position.y <= ground)
				{
					jumping = false;
					pos = base.transform.position;
					pos.y = ground;
					base.transform.position = pos;
				}
				if ((base.transform.position.x < -330f && !this.onLeft) || (base.transform.position.x > 330f && this.onLeft))
				{
					if (this.onLeft)
					{
						base.transform.SetPosition(new float?(330f), null, null);
						this.onLeft = false;
					}
					else
					{
						base.transform.SetPosition(new float?(-330f), null, null);
						this.onLeft = true;
					}
					velocityX = ((!this.onLeft) ? (-speedX) : speedX);
				}
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600291C RID: 10524 RVA: 0x0017F56B File Offset: 0x0017D96B
	public override void Dead()
	{
		base.Dead();
		this.parent.OnToadDie();
	}

	// Token: 0x04003209 RID: 12809
	private const float TOAD_MAX_X_POS = 330f;

	// Token: 0x0400320A RID: 12810
	private const float OFFSET_Y = 50f;

	// Token: 0x0400320B RID: 12811
	private const float OFFSCREEN_Y = 200f;

	// Token: 0x0400320C RID: 12812
	private const float BASE_Y = 250f;

	// Token: 0x0400320D RID: 12813
	private const float MOVE_Y_SPEED = 500f;

	// Token: 0x0400320E RID: 12814
	private LevelProperties.RetroArcade.Toad properties;

	// Token: 0x0400320F RID: 12815
	private RetroArcadeToadManager parent;

	// Token: 0x04003210 RID: 12816
	private float gravity;

	// Token: 0x04003211 RID: 12817
	private bool onLeft;
}
