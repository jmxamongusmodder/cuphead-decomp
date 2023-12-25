using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E8 RID: 2280
public class MountainPlatformingLevelMudman : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x0600356C RID: 13676 RVA: 0x001F23DD File Offset: 0x001F07DD
	protected override void Start()
	{
		base.Start();
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.come_up_cr());
		base.StartCoroutine(this.check_cr());
	}

	// Token: 0x0600356D RID: 13677 RVA: 0x001F240B File Offset: 0x001F080B
	protected override void FixedUpdate()
	{
		if (!this.melting)
		{
			base.FixedUpdate();
		}
	}

	// Token: 0x0600356E RID: 13678 RVA: 0x001F2420 File Offset: 0x001F0820
	public void Init(Vector3 pos, PlatformingLevelGroundMovementEnemy.Direction direction)
	{
		base.transform.position = pos;
		this._direction = direction;
		base.transform.SetScale(new float?((float)((direction != PlatformingLevelGroundMovementEnemy.Direction.Right) ? 1 : -1)), null, null);
	}

	// Token: 0x0600356F RID: 13679 RVA: 0x001F2474 File Offset: 0x001F0874
	private IEnumerator come_up_cr()
	{
		base.animator.SetTrigger("Intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		base.GetComponent<Collider2D>().enabled = true;
		this.melting = false;
		yield return null;
		yield break;
	}

	// Token: 0x06003570 RID: 13680 RVA: 0x001F2490 File Offset: 0x001F0890
	private IEnumerator check_cr()
	{
		while (MountainPlatformingLevelElevatorHandler.elevatorIsMoving)
		{
			yield return null;
		}
		base.animator.SetTrigger("Outro");
		yield return base.animator.WaitForAnimationToStart(this, "Outro", false);
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		yield break;
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x001F24AB File Offset: 0x001F08AB
	protected override void CalculateDirection()
	{
	}

	// Token: 0x06003572 RID: 13682 RVA: 0x001F24AD File Offset: 0x001F08AD
	protected override Coroutine Turn()
	{
		this.StopAllCoroutines();
		return base.StartCoroutine(this.despawn_cr());
	}

	// Token: 0x06003573 RID: 13683 RVA: 0x001F24C4 File Offset: 0x001F08C4
	private IEnumerator despawn_cr()
	{
		this.melting = true;
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("Outro");
		yield return null;
		yield break;
	}

	// Token: 0x06003574 RID: 13684 RVA: 0x001F24E0 File Offset: 0x001F08E0
	private IEnumerator explode_cr()
	{
		for (int i = 0; i < this.explodeSpawns.Length; i++)
		{
			this.splash.Create(this.explodeSpawns[i].position);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003575 RID: 13685 RVA: 0x001F24FB File Offset: 0x001F08FB
	protected override void Die()
	{
		base.FrameDelayedCallback(delegate
		{
			this.otherExplosion.Create(base.GetComponent<Collider2D>().bounds.center);
			this.<Die>__BaseCallProxy0();
		}, 1);
		base.StartCoroutine(this.explode_cr());
		if (this.isBig)
		{
			this.MudmanBigDeathSFX();
		}
		else
		{
			this.MudmanSmallDeathSFX();
		}
	}

	// Token: 0x06003576 RID: 13686 RVA: 0x001F253A File Offset: 0x001F093A
	private void Delete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003577 RID: 13687 RVA: 0x001F2547 File Offset: 0x001F0947
	private void MudmanBigSpawnSFX()
	{
		AudioManager.Play("castle_mudman_small_spawn");
		this.emitAudioFromObject.Add("castle_mudman_small_spawn");
	}

	// Token: 0x06003578 RID: 13688 RVA: 0x001F2563 File Offset: 0x001F0963
	private void MudmanBigDeathSFX()
	{
		AudioManager.Play("castle_mudman_large_death");
		this.emitAudioFromObject.Add("castle_mudman_large_death");
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x001F257F File Offset: 0x001F097F
	private void MudmanSmallSpawnSFX()
	{
		AudioManager.Play("castle_mudman_large_spawn");
		this.emitAudioFromObject.Add("castle_mudman_large_spawn");
	}

	// Token: 0x0600357A RID: 13690 RVA: 0x001F259B File Offset: 0x001F099B
	private void MudmanSmallDeathSFX()
	{
		AudioManager.Play("castle_mudman_small_death");
		this.emitAudioFromObject.Add("castle_mudman_small_death");
	}

	// Token: 0x04003D8F RID: 15759
	[SerializeField]
	private PlatformingLevelGenericExplosion splash;

	// Token: 0x04003D90 RID: 15760
	[SerializeField]
	private Transform[] explodeSpawns;

	// Token: 0x04003D91 RID: 15761
	[SerializeField]
	private PlatformingLevelGenericExplosion otherExplosion;

	// Token: 0x04003D92 RID: 15762
	[SerializeField]
	private bool isBig;

	// Token: 0x04003D93 RID: 15763
	private bool melting = true;
}
