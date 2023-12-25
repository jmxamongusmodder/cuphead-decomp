using System;
using UnityEngine;

// Token: 0x020006FA RID: 1786
public class MouseLevelSpring : ParrySwitch
{
	// Token: 0x0600263F RID: 9791 RVA: 0x00165850 File Offset: 0x00163C50
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (phase == CollisionPhase.Enter && hit.GetComponent<MouseLevelCanMouse>() != null && !this.isLaunched)
		{
			this.smallExplosion.Create(base.transform.position);
			base.animator.SetTrigger("OnDeath");
		}
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x001658AE File Offset: 0x00163CAE
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		player.GetComponent<LevelPlayerMotor>().OnTrampolineKnockUp(this.knockUpHeight);
		if (!this.isLaunched)
		{
			base.animator.SetTrigger("OnLaunch");
		}
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x001658E3 File Offset: 0x00163CE3
	private void GotRunOver()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x00165900 File Offset: 0x00163D00
	public void LaunchSpring(Vector2 position, Vector2 velocity, float gravity)
	{
		if (base.gameObject.activeSelf)
		{
			base.animator.Play("Flip");
		}
		base.transform.position = position;
		this.velocity = velocity;
		this.gravity = gravity;
		this.isLaunched = true;
		base.gameObject.SetActive(true);
		base.GetComponent<Collider2D>().enabled = true;
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x0016596C File Offset: 0x00163D6C
	private void Update()
	{
		if (this.isLaunched)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			if (base.transform.position.y < (float)Level.Current.Ground + this.offset)
			{
				base.transform.SetPosition(null, new float?((float)Level.Current.Ground + this.offset), null);
				if (this.isLaunched)
				{
					this.Landed();
				}
				this.isLaunched = false;
			}
		}
	}

	// Token: 0x06002644 RID: 9796 RVA: 0x00165A59 File Offset: 0x00163E59
	private void Landed()
	{
		base.animator.SetTrigger("OnLand");
		AudioManager.Play("level_mouse_can_springboard_land");
		this.emitAudioFromObject.Add("level_mouse_can_springboard_land");
	}

	// Token: 0x06002645 RID: 9797 RVA: 0x00165A85 File Offset: 0x00163E85
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.smallExplosion = null;
	}

	// Token: 0x04002ECB RID: 11979
	[SerializeField]
	private Effect smallExplosion;

	// Token: 0x04002ECC RID: 11980
	public float knockUpHeight = -1.5f;

	// Token: 0x04002ECD RID: 11981
	private bool isLaunched;

	// Token: 0x04002ECE RID: 11982
	private Vector2 velocity;

	// Token: 0x04002ECF RID: 11983
	private float gravity;

	// Token: 0x04002ED0 RID: 11984
	private float offset = 120f;
}
