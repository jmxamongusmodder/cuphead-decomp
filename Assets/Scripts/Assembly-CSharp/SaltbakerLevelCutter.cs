using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007C4 RID: 1988
public class SaltbakerLevelCutter : AbstractProjectile
{
	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06002CFA RID: 11514 RVA: 0x001A814B File Offset: 0x001A654B
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002CFB RID: 11515 RVA: 0x001A8154 File Offset: 0x001A6554
	public SaltbakerLevelCutter Create(Vector3 position, float speed, bool goingLeft, int id)
	{
		SaltbakerLevelCutter saltbakerLevelCutter = this.InstantiatePrefab<SaltbakerLevelCutter>();
		saltbakerLevelCutter.transform.position = position;
		saltbakerLevelCutter.speed = speed;
		saltbakerLevelCutter.goingLeft = goingLeft;
		saltbakerLevelCutter.transform.localScale = new Vector3((float)((!goingLeft) ? 1 : -1), 1f);
		if (id == 1)
		{
			saltbakerLevelCutter.transform.position += Vector3.down * 5f;
		}
		saltbakerLevelCutter.sfxID = id;
		saltbakerLevelCutter.SFX_SALTBAKER_P3_PizzaWheel_Loop(id);
		saltbakerLevelCutter.rend.sortingOrder = id;
		return saltbakerLevelCutter;
	}

	// Token: 0x06002CFC RID: 11516 RVA: 0x001A81F0 File Offset: 0x001A65F0
	private void AniEvent_StartMove()
	{
		base.StartCoroutine(this.move_cr());
		this.dustFX.transform.parent = null;
		this.dustFX.SetActive(true);
	}

	// Token: 0x06002CFD RID: 11517 RVA: 0x001A821C File Offset: 0x001A661C
	private void AniEvent_Variation()
	{
		if (((this.goingLeft && base.transform.position.x > (float)Level.Current.Left + 50f + 100f) || (!this.goingLeft && base.transform.position.x < (float)Level.Current.Right - 50f - 100f)) && UnityEngine.Random.Range(0, 4) == 0)
		{
			base.animator.SetTrigger("Variation");
		}
	}

	// Token: 0x06002CFE RID: 11518 RVA: 0x001A82B9 File Offset: 0x001A66B9
	private void AniEvent_ChangeDirection()
	{
		this.goingLeft = !this.goingLeft;
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x001A82CC File Offset: 0x001A66CC
	private void AniEvent_CompleteTurn()
	{
		base.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
		this.turning = false;
	}

	// Token: 0x06002D00 RID: 11520 RVA: 0x001A8309 File Offset: 0x001A6709
	private void AniEvent_CompleteSink()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002D01 RID: 11521 RVA: 0x001A8316 File Offset: 0x001A6716
	public void Sink()
	{
		base.animator.SetBool("Sink", true);
		this.SFX_SALTBAKER_P3_PizzaWheel_Dive(this.sfxID);
	}

	// Token: 0x06002D02 RID: 11522 RVA: 0x001A8338 File Offset: 0x001A6738
	private IEnumerator move_cr()
	{
		float left = (float)Level.Current.Left + 50f;
		float right = (float)Level.Current.Right - 50f;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			Vector3 dir = (!this.goingLeft) ? Vector3.right : Vector3.left;
			base.transform.position += dir * this.speed * CupheadTime.FixedDelta;
			if (!this.turning && ((this.goingLeft && base.transform.position.x < left) || (!this.goingLeft && base.transform.position.x > right)))
			{
				this.turning = true;
				base.animator.Play("Turn");
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002D03 RID: 11523 RVA: 0x001A8353 File Offset: 0x001A6753
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002D04 RID: 11524 RVA: 0x001A8374 File Offset: 0x001A6774
	private void SFX_SALTBAKER_P3_PizzaWheel_Loop(int loopNumber)
	{
		string key = "sfx_dlc_saltbaker_p3_pizzawheel_movement_loop_" + (loopNumber + 1);
		AudioManager.PlayLoop(key);
		this.emitAudioFromObject.Add(key);
	}

	// Token: 0x06002D05 RID: 11525 RVA: 0x001A83A6 File Offset: 0x001A67A6
	private void SFX_SALTBAKER_P3_PizzaWheel_Dive(int loopNumber)
	{
		AudioManager.Stop("sfx_dlc_saltbaker_p3_pizzawheel_movement_loop_" + (loopNumber + 1));
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x001A83BF File Offset: 0x001A67BF
	private void AnimationEvent_SFX_SALTBAKER_P3_RunnerWheel_Spawn()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3_pizzawheel_spawn");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_pizzawheel_spawn");
	}

	// Token: 0x0400357F RID: 13695
	private const float SCREEN_EDGE_OFFSET = 50f;

	// Token: 0x04003580 RID: 13696
	private float speed;

	// Token: 0x04003581 RID: 13697
	private bool goingLeft;

	// Token: 0x04003582 RID: 13698
	private bool turning;

	// Token: 0x04003583 RID: 13699
	private int sfxID;

	// Token: 0x04003584 RID: 13700
	private LevelProperties.Saltbaker.Cutter properties;

	// Token: 0x04003585 RID: 13701
	[SerializeField]
	private GameObject dustFX;

	// Token: 0x04003586 RID: 13702
	[SerializeField]
	private SpriteRenderer rend;
}
