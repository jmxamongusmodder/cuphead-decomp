using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D9 RID: 2265
public class MountainPlatformingLevelCyclopsBG : AbstractPausableComponent
{
	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x060034FE RID: 13566 RVA: 0x001ECF9B File Offset: 0x001EB39B
	// (set) Token: 0x060034FF RID: 13567 RVA: 0x001ECFA3 File Offset: 0x001EB3A3
	public bool isWalking { get; private set; }

	// Token: 0x06003500 RID: 13568 RVA: 0x001ECFAC File Offset: 0x001EB3AC
	private void Start()
	{
		this.sizeY = base.GetComponent<SpriteRenderer>().bounds.size.y;
		this.blinkCounterMax = UnityEngine.Random.Range(3, 6);
		this.eye.enabled = false;
	}

	// Token: 0x06003501 RID: 13569 RVA: 0x001ECFF4 File Offset: 0x001EB3F4
	private void OnTurn()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
		if (base.transform.localScale.x == 1f)
		{
			base.transform.AddPosition(-47f, 0f, 0f);
		}
		else
		{
			base.transform.AddPosition(47f, 0f, 0f);
		}
	}

	// Token: 0x06003502 RID: 13570 RVA: 0x001ED08D File Offset: 0x001EB48D
	private void DropRocks()
	{
		base.StartCoroutine(this.drop_rocks_cr());
	}

	// Token: 0x06003503 RID: 13571 RVA: 0x001ED09C File Offset: 0x001EB49C
	private void StopWalking()
	{
		this.isWalking = false;
	}

	// Token: 0x06003504 RID: 13572 RVA: 0x001ED0A5 File Offset: 0x001EB4A5
	private void StartWalking()
	{
		this.isWalking = true;
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x001ED0AE File Offset: 0x001EB4AE
	public void GetPlayer(AbstractPlayerController player)
	{
		this.player = player;
	}

	// Token: 0x06003506 RID: 13574 RVA: 0x001ED0B8 File Offset: 0x001EB4B8
	private IEnumerator drop_rocks_cr()
	{
		for (int i = 0; i < this.rockCount; i++)
		{
			Vector2 zero = Vector2.zero;
			zero.y = CupheadLevelCamera.Current.Bounds.yMax + 200f;
			zero.x = CupheadLevelCamera.Current.Bounds.xMin + this.projectile.GetComponent<Renderer>().bounds.size.x / 2f + this.projectile.GetComponent<Renderer>().bounds.size.x * (float)i;
			this.projectile.Create(base.transform.position, zero, this.rockSpeed, this.rockDelay * (float)i);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x001ED0D3 File Offset: 0x001EB4D3
	private void SlideUp()
	{
		if (!this.isDead)
		{
			base.StartCoroutine(this.slide_up_cr());
		}
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x001ED0ED File Offset: 0x001EB4ED
	private void SlideDown()
	{
		base.StartCoroutine(this.slide_down_cr());
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x001ED0FC File Offset: 0x001EB4FC
	private IEnumerator slide_up_cr()
	{
		this.player = PlayerManager.GetNext();
		base.transform.SetPosition(new float?(this.player.transform.position.x), null, null);
		float t = 0f;
		float time = 0.4f;
		float startPos = base.transform.position.y;
		float endPos = this.start.y;
		while (t < time)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(startPos, endPos, t / time)), null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x001ED118 File Offset: 0x001EB518
	private IEnumerator slide_down_cr()
	{
		float t = 0f;
		float time = 0.8f;
		float startPos = base.transform.position.y;
		float endPos = this.start.y - this.sizeY;
		while (t < time)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(startPos, endPos, t / time)), null);
			yield return null;
		}
		if (this.isDead)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			yield return CupheadTime.WaitForSeconds(this, 0.8f);
			base.animator.SetTrigger("Continue");
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600350B RID: 13579 RVA: 0x001ED134 File Offset: 0x001EB534
	private void BlinkMaybe()
	{
		if (this.blinkCounter < this.blinkCounterMax)
		{
			this.eye.enabled = false;
			this.blinkCounter++;
		}
		else
		{
			this.eye.enabled = true;
			this.blinkCounter = 0;
			this.blinkCounterMax = UnityEngine.Random.Range(3, 6);
		}
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x001ED191 File Offset: 0x001EB591
	private void SoundGiantRockThrow()
	{
		AudioManager.Play("castle_giant_rock_throw");
		this.emitAudioFromObject.Add("castle_giant_rock_throw");
	}

	// Token: 0x0600350D RID: 13581 RVA: 0x001ED1AD File Offset: 0x001EB5AD
	private void SoundGiantRockThrowAppear()
	{
		AudioManager.Play("castle_giant_rock_throw_appear");
		this.emitAudioFromObject.Add("castle_giant_rock_throw_appear");
	}

	// Token: 0x0600350E RID: 13582 RVA: 0x001ED1C9 File Offset: 0x001EB5C9
	private void SoundGiantStartle()
	{
		AudioManager.Play("castle_giant_startle");
		this.emitAudioFromObject.Add("castle_giant_startle");
	}

	// Token: 0x04003D26 RID: 15654
	[SerializeField]
	private SpriteRenderer eye;

	// Token: 0x04003D27 RID: 15655
	[SerializeField]
	private float rockDelay;

	// Token: 0x04003D28 RID: 15656
	[SerializeField]
	private float rockSpeed;

	// Token: 0x04003D29 RID: 15657
	[SerializeField]
	private int rockCount;

	// Token: 0x04003D2A RID: 15658
	[SerializeField]
	private MountainPlatformingLevelRock projectile;

	// Token: 0x04003D2B RID: 15659
	private int blinkCounter;

	// Token: 0x04003D2C RID: 15660
	private int blinkCounterMax;

	// Token: 0x04003D2D RID: 15661
	private float sizeY;

	// Token: 0x04003D2E RID: 15662
	private bool isAltPattern;

	// Token: 0x04003D2F RID: 15663
	private AbstractPlayerController player;

	// Token: 0x04003D30 RID: 15664
	public Vector3 start;

	// Token: 0x04003D32 RID: 15666
	public bool isDead;
}
