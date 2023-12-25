using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A8 RID: 2216
public class CircusPlatformingLevelPoleBot : AbstractPlatformingLevelEnemy
{
	// Token: 0x060033A4 RID: 13220 RVA: 0x001DFFF8 File Offset: 0x001DE3F8
	protected override void Start()
	{
		base.Start();
		this.start = base.transform.position.y;
		this.velocity = new Vector2(UnityEngine.Random.Range(this.minVelocity.min, this.minVelocity.max), UnityEngine.Random.Range(this.maxVelocity.min, this.maxVelocity.max));
		this.startVelocity = this.velocity;
		this.gravity = 1000f;
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x001E007C File Offset: 0x001DE47C
	public void SlideDown()
	{
		base.StartCoroutine(this.slide_cr());
	}

	// Token: 0x060033A6 RID: 13222 RVA: 0x001E008C File Offset: 0x001DE48C
	private IEnumerator slide_cr()
	{
		this.isSliding = true;
		base.animator.SetBool("Falling", true);
		YieldInstruction wait = new WaitForFixedUpdate();
		yield return CupheadTime.WaitForSeconds(this, this.fallDelay);
		while (base.transform.position.y > this.start - base.GetComponent<BoxCollider2D>().size.y * 1.38f)
		{
			base.transform.AddPosition(0f, -base.Properties.poleSpeedMovement * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.start = base.transform.position.y;
		this.isSliding = false;
		base.animator.SetBool("Falling", false);
		yield return null;
		yield break;
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x001E00A7 File Offset: 0x001DE4A7
	protected override void OnStart()
	{
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x001E00A9 File Offset: 0x001DE4A9
	protected override void Die()
	{
		this.PoleBotDeathSFX();
		this.isDying = true;
		base.animator.SetTrigger("Dead");
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.fly_cr());
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x001E00E4 File Offset: 0x001DE4E4
	private IEnumerator fly_cr()
	{
		base._damageReceiver.enabled = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		float timeToApex = Mathf.Sqrt(2f * base.transform.position.y / this.gravity);
		this.startVelocity.y = timeToApex * this.gravity;
		while (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin)
		{
			base.transform.AddPosition(-this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
			base.transform.Rotate(Vector3.forward, this.deadSpin * CupheadTime.Delta);
			yield return wait;
		}
		base.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x001E00FF File Offset: 0x001DE4FF
	private void PoleBotIdleSFX()
	{
		if (!AudioManager.CheckIfPlaying("circus_pole_guy_idle"))
		{
			AudioManager.Play("circus_pole_guy_idle");
			this.emitAudioFromObject.Add("circus_pole_guy_idle");
		}
	}

	// Token: 0x060033AB RID: 13227 RVA: 0x001E012A File Offset: 0x001DE52A
	private void PoleBotFallSFX()
	{
		AudioManager.Play("circus_pole_guy_falling");
		this.emitAudioFromObject.Add("circus_pole_guy_falling");
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x001E0146 File Offset: 0x001DE546
	private void PoleBotDeathSFX()
	{
		AudioManager.Play("circus_pole_guy_death");
		this.emitAudioFromObject.Add("circus_pole_guy_death");
	}

	// Token: 0x04003BF0 RID: 15344
	private const string FallingParameterName = "Falling";

	// Token: 0x04003BF1 RID: 15345
	private const string DeadParameterName = "Dead";

	// Token: 0x04003BF2 RID: 15346
	private Vector2 velocity;

	// Token: 0x04003BF3 RID: 15347
	private Vector2 startVelocity;

	// Token: 0x04003BF4 RID: 15348
	private float gravity;

	// Token: 0x04003BF5 RID: 15349
	public bool isDying;

	// Token: 0x04003BF6 RID: 15350
	public bool isSliding;

	// Token: 0x04003BF7 RID: 15351
	[SerializeField]
	private float fallDelay;

	// Token: 0x04003BF8 RID: 15352
	[SerializeField]
	private float deadSpin;

	// Token: 0x04003BF9 RID: 15353
	[SerializeField]
	private MinMax minVelocity;

	// Token: 0x04003BFA RID: 15354
	[SerializeField]
	private MinMax maxVelocity;

	// Token: 0x04003BFB RID: 15355
	private float start;
}
