using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000547 RID: 1351
public class ChessQueenLevelCannon : AbstractCollidableObject
{
	// Token: 0x1700033C RID: 828
	// (get) Token: 0x060018E7 RID: 6375 RVA: 0x000E1C08 File Offset: 0x000E0008
	// (set) Token: 0x060018E8 RID: 6376 RVA: 0x000E1C10 File Offset: 0x000E0010
	public bool IsActive { get; set; }

	// Token: 0x060018E9 RID: 6377 RVA: 0x000E1C1C File Offset: 0x000E001C
	private void Start()
	{
		this.parryColliders = new Collider2D[this.parry.Length];
		for (int i = 0; i < this.parry.Length; i++)
		{
			this.parry[i].OnActivate += this.shootCannonball;
			this.parryColliders[i] = this.parry[i].GetComponent<Collider2D>();
		}
		this.SetActive(false);
		this.mouseAnimator.Play("Idle");
		this.mouseLookTime = UnityEngine.Random.Range(1f, 3f);
		this.setupWickParabola();
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x000E1CB5 File Offset: 0x000E00B5
	public void SetProperties(float minAngle, float maxAngle, float rotationTime, ChessQueenLevelCannon.CannonPosition cannonPosition, LevelProperties.ChessQueen.Turret properties, ChessQueenLevelQueen queen)
	{
		this.minAngle = minAngle;
		this.maxAngle = maxAngle;
		this.rotationTime = rotationTime;
		this.cannonPosition = cannonPosition;
		this.properties = properties;
		this.queen = queen;
		this.move();
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x000E1CEC File Offset: 0x000E00EC
	public void SetActive(bool setActive)
	{
		this.mouseAnimator.SetBool("Idle", !setActive);
		if (setActive)
		{
			this.mouseAnimator.SetBool("LookRight", false);
		}
		this.IsActive = setActive;
		foreach (Collider2D collider2D in this.parryColliders)
		{
			collider2D.enabled = setActive;
		}
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x000E1D54 File Offset: 0x000E0154
	private void LateUpdate()
	{
		if (!this.IsActive)
		{
			this.mouseLookTime -= CupheadTime.Delta;
			if (this.mouseLookTime < 0f)
			{
				this.mouseAnimator.SetBool("LookRight", !this.mouseAnimator.GetBool("LookRight"));
				this.mouseLookTime += UnityEngine.Random.Range(1f, 3f);
			}
		}
		int num = Array.IndexOf<Sprite>(this.baseSprites, this.baseRenderer.sprite);
		if (num < 0)
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("01"))
			{
				num = 0;
			}
			else if (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("05"))
			{
				num = 4;
			}
			else if (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("10"))
			{
				num = 9;
			}
			else
			{
				if (!base.animator.GetCurrentAnimatorStateInfo(0).IsTag("15"))
				{
					return;
				}
				num = 14;
			}
		}
		float num2 = (float)num / (float)(this.baseSprites.Length - 1);
		if (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Side)
		{
			num2 = EaseUtils.EaseInOutCubic(0f, 1f, num2);
		}
		float num3 = Mathf.Lerp(this.minAngle, this.maxAngle, num2);
		if (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Center)
		{
			num3 *= (float)((!this.baseRenderer.flipX) ? 1 : -1);
		}
		this.barrelTransform.rotation = Quaternion.Euler(0f, 0f, num3);
		this.barrelHighlightRenderer.sprite = this.barrelHighlightSprites[Mathf.RoundToInt((1f - num2) * (float)(this.barrelHighlightSprites.Length - 1))];
		this.barrelHighlightRenderer.flipX = (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Center && this.baseRenderer.flipX);
		this.barrelHighlightRenderer.enabled = base.animator.GetCurrentAnimatorStateInfo(1).IsName("Idle");
		if (this.wickFollowsParabola)
		{
			float num4 = num2 * this.wickParametricDuration;
			Vector3 position = this.wickStartPosition;
			position.x -= 2f * this.wickParabolaParameter * num4 * (float)((!this.baseRenderer.flipX) ? 1 : -1);
			position.y += this.wickParabolaParameter * num4 * num4;
			this.wickTransform.position = position;
		}
		if (Level.Current.Ending)
		{
			foreach (Collider2D collider2D in this.parryColliders)
			{
				collider2D.enabled = false;
			}
		}
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x000E203F File Offset: 0x000E043F
	private void move()
	{
		if (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Side)
		{
			base.animator.Play(0, ChessQueenLevelCannon.BaseAnimatorLayer, 0.5f);
		}
		base.StartCoroutine(this.cannonActive_cr());
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x000E2070 File Offset: 0x000E0470
	private IEnumerator cannonActive_cr()
	{
		for (;;)
		{
			while (!this.IsActive)
			{
				yield return null;
			}
			base.animator.SetBool("Moving", true);
			base.animator.SetFloat("BaseSpeed", ChessQueenLevelCannon.BaseRotationDuration / this.rotationTime);
			base.animator.SetTrigger("WickIgnite");
			this.SFX_KOG_QUEEN_CannonFuseLoop();
			while (this.IsActive)
			{
				foreach (Collider2D collider2D in this.parryColliders)
				{
					if (this.queen.activeLightning == null || this.queen.activeLightning.isGone)
					{
						collider2D.enabled = true;
					}
					else
					{
						collider2D.enabled = (Mathf.Abs(collider2D.transform.position.x + collider2D.offset.x - this.queen.activeLightning.transform.position.x) > this.queen.lightningDisableRange);
					}
				}
				float animTime = base.animator.GetCurrentAnimatorStateInfo(ChessQueenLevelCannon.BaseAnimatorLayer).normalizedTime % 1f;
				if (this.mouseReverses)
				{
					if (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Side)
					{
						this.mouseAnimator.SetBool("Reverse", animTime > 0.4f && animTime <= 0.9f);
					}
					else
					{
						this.mouseAnimator.SetBool("Reverse", animTime > 0.5f != this.baseRenderer.flipX);
					}
				}
				yield return null;
			}
			base.animator.SetBool("Moving", false);
		}
		yield break;
	}

	// Token: 0x060018EF RID: 6383 RVA: 0x000E208C File Offset: 0x000E048C
	private void shootCannonball()
	{
		if (this.IsActive)
		{
			base.animator.SetTrigger("CannonBlast");
			base.animator.SetTrigger("WickBlast");
			this.SFX_KOG_QUEEN_CannonShoot();
			this.SFX_KOG_QUEEN_CannonFuseLoopStop();
			this.SetActive(false);
			base.StartCoroutine(this.shoot_cr());
		}
	}

	// Token: 0x060018F0 RID: 6384 RVA: 0x000E20E4 File Offset: 0x000E04E4
	private IEnumerator shoot_cr()
	{
		this.wickFollowsParabola = false;
		this.wickTransform.parent = this.wickBlastPositionerTransform;
		this.blastFXTransform.position = this.blastFXSpawnPoint.position;
		this.blastFXTransform.eulerAngles = this.barrelTransform.eulerAngles;
		base.animator.Play((!((ChessQueenLevel)Level.Current).cannonBlastFXVariant) ? "BlastFXB" : "BlastFXA", ChessQueenLevelCannon.BlastFXAnimatorLayer, 0f);
		((ChessQueenLevel)Level.Current).cannonBlastFXVariant = !((ChessQueenLevel)Level.Current).cannonBlastFXVariant;
		base.animator.Update(0f);
		while (base.animator.GetCurrentAnimatorStateInfo(ChessQueenLevelCannon.CannonAnimatorLayer).normalizedTime < 0.7f)
		{
			yield return null;
		}
		base.animator.SetFloat("BaseSpeed", ChessQueenLevelCannon.BaseRotationDuration / this.rotationTime);
		base.animator.SetBool("Moving", false);
		this.wickTransform.parent = base.transform;
		this.wickFollowsParabola = true;
		this.wickTransform.localEulerAngles = Vector3.zero;
		yield break;
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x000E20FF File Offset: 0x000E04FF
	private void animationEvent_CannonFinishedCycle()
	{
		if (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Center)
		{
			this.baseRenderer.flipX = !this.baseRenderer.flipX;
			this.baseTopperRenderer.flipX = this.baseRenderer.flipX;
		}
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x000E213C File Offset: 0x000E053C
	private void animationEvent_FireBullet()
	{
		if (Level.Current.Ending)
		{
			return;
		}
		this.looseMouse.CannonFired(this.cannonBall.Create(this.bulletSpawnPoint.transform.position, MathUtils.DirectionToAngle(this.barrelTransform.up), this.properties.turretCannonballSpeed).gameObject);
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x000E21AC File Offset: 0x000E05AC
	private void setupWickParabola()
	{
		this.wickStartPosition = this.wickTransform.position;
		Vector3 vector = this.wickParabolaEndTransform.position - this.wickStartPosition;
		this.wickParabolaParameter = vector.x * vector.x / (4f * vector.y);
		this.wickParametricDuration = vector.x / (2f * this.wickParabolaParameter);
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x000E2220 File Offset: 0x000E0620
	private void SFX_KOG_QUEEN_CannonShoot()
	{
		AudioManager.Stop("sfx_DLC_KOG_Queen_CannonFuse_Loop");
		AudioManager.Play("sfx_DLC_KOG_Queen_CannonShoot");
		AudioManager.Pan("sfx_DLC_KOG_Queen_CannonShoot", (Mathf.Abs(base.transform.position.x) <= 100f) ? 0f : Mathf.Sign(base.transform.position.x));
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x000E2290 File Offset: 0x000E0690
	private void SFX_KOG_QUEEN_CannonFuseLoop()
	{
		AudioManager.PlayLoop("sfx_DLC_KOG_Queen_CannonFuse_Loop");
		AudioManager.Pan("sfx_DLC_KOG_Queen_CannonFuse_Loop", (Mathf.Abs(base.transform.position.x) <= 100f) ? 0f : Mathf.Sign(base.transform.position.x));
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x000E22F5 File Offset: 0x000E06F5
	private void SFX_KOG_QUEEN_CannonFuseLoopStop()
	{
		AudioManager.Stop("sfx_DLC_KOG_Queen_CannonFuse_Loop");
	}

	// Token: 0x040021EA RID: 8682
	private static readonly float BaseRotationDuration = 0.625f;

	// Token: 0x040021EB RID: 8683
	private static readonly int BaseAnimatorLayer;

	// Token: 0x040021EC RID: 8684
	private static readonly int CannonAnimatorLayer = 1;

	// Token: 0x040021ED RID: 8685
	private static readonly int BlastFXAnimatorLayer = 3;

	// Token: 0x040021EE RID: 8686
	[SerializeField]
	private ChessQueenLevelCannonball cannonBall;

	// Token: 0x040021EF RID: 8687
	[SerializeField]
	private ParrySwitch[] parry;

	// Token: 0x040021F0 RID: 8688
	[SerializeField]
	private SpriteRenderer baseRenderer;

	// Token: 0x040021F1 RID: 8689
	[SerializeField]
	private SpriteRenderer barrelHighlightRenderer;

	// Token: 0x040021F2 RID: 8690
	[SerializeField]
	private SpriteRenderer baseTopperRenderer;

	// Token: 0x040021F3 RID: 8691
	[SerializeField]
	private Transform barrelTransform;

	// Token: 0x040021F4 RID: 8692
	[SerializeField]
	private Transform bulletSpawnPoint;

	// Token: 0x040021F5 RID: 8693
	[SerializeField]
	private Transform blastFXSpawnPoint;

	// Token: 0x040021F6 RID: 8694
	[SerializeField]
	private Transform blastFXTransform;

	// Token: 0x040021F7 RID: 8695
	[SerializeField]
	private Sprite[] baseSprites;

	// Token: 0x040021F8 RID: 8696
	[SerializeField]
	private Sprite[] barrelHighlightSprites;

	// Token: 0x040021F9 RID: 8697
	[SerializeField]
	private Transform wickTransform;

	// Token: 0x040021FA RID: 8698
	[SerializeField]
	private Transform wickParabolaEndTransform;

	// Token: 0x040021FB RID: 8699
	[SerializeField]
	private Transform wickBlastPositionerTransform;

	// Token: 0x040021FC RID: 8700
	[SerializeField]
	private Animator mouseAnimator;

	// Token: 0x040021FD RID: 8701
	[SerializeField]
	private ChessQueenLevelLooseMouse looseMouse;

	// Token: 0x040021FE RID: 8702
	[SerializeField]
	private bool mouseReverses;

	// Token: 0x04002200 RID: 8704
	private Collider2D[] parryColliders;

	// Token: 0x04002201 RID: 8705
	private LevelProperties.ChessQueen.Turret properties;

	// Token: 0x04002202 RID: 8706
	private float rotationTime;

	// Token: 0x04002203 RID: 8707
	private float minAngle;

	// Token: 0x04002204 RID: 8708
	private float maxAngle;

	// Token: 0x04002205 RID: 8709
	private ChessQueenLevelCannon.CannonPosition cannonPosition;

	// Token: 0x04002206 RID: 8710
	private ChessQueenLevelQueen queen;

	// Token: 0x04002207 RID: 8711
	private float mouseLookTime;

	// Token: 0x04002208 RID: 8712
	private bool wickFollowsParabola = true;

	// Token: 0x04002209 RID: 8713
	private Vector3 wickStartPosition;

	// Token: 0x0400220A RID: 8714
	private float wickParabolaParameter;

	// Token: 0x0400220B RID: 8715
	private float wickParametricDuration;

	// Token: 0x02000548 RID: 1352
	public enum CannonPosition
	{
		// Token: 0x0400220D RID: 8717
		Side,
		// Token: 0x0400220E RID: 8718
		Center
	}
}
